using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>
/// Assistant d'import : fichier d'origine jamais modifié (SHA-256), -wal/-shm copiés, contrôles bloquants, photo « avant » et
/// comparaison « après », sauvegarde « avant-import », annulation à chaque étape, import d'une sauvegarde chiffrée (7a).
/// </summary>
public class ImportBaseServiceTests : IDisposable
{
    private readonly string _racine = Path.Combine(Path.GetTempPath(), "p2_import_" + Guid.NewGuid().ToString("N"));

    public ImportBaseServiceTests()
    {
        Directory.CreateDirectory(_racine);
        TestDb.Reinitialiser();
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        try { Directory.Delete(_racine, true); } catch { }
    }

    private string Fichier(string nom) => Path.Combine(_racine, nom, CheminsApp.NomFichierBase);

    private static string Sha(string chemin)
    {
        using var f = new FileStream(chemin, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return Convert.ToHexString(SHA256.HashData(f));
    }

    private static long Ventes(string chemin) => MigrationEmplacementBase.CompterVentes(chemin);

    private static IEnumerable<string> SauvegardesAvantImport() => Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-import_*.sqlite");

    // ───────────── cas nominal : tout est contrôlé, comparé et remplacé ─────────────

    [Fact]
    public void Import_complet_original_intact_comparaison_et_remplacement()
    {
        string source = TestDb.CreerBase(Fichier("ancienne"), 7);
        string shaAvant = Sha(source);

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Importee, r.Issue);
        Assert.All(r.Controles, c => Assert.True(c.Ok, c.Nom + " : " + c.Detail));
        foreach (var attendu in new[] { "Intégrité de la base", "Tables de la pharmacie", "Table des migrations", "Version de la base reconnue", "Mise à jour de la copie", "Administrateur actif" })
            Assert.Contains(r.Controles, c => c.Nom == attendu);
        Assert.Contains(r.Comparaison, l => l.Table == "ventes" && l.Avant == 7 && l.Apres == 7);     // photo avant / après
        Assert.DoesNotContain(r.Comparaison, l => l.Perdue);
        Assert.Equal(7, Ventes(CheminsApp.CheminBase));
        Assert.True(MigrationEmplacementBase.ImportEffectue);

        Assert.True(r.OriginalIntact);
        Assert.Equal(shaAvant, Sha(source));                                    // SHA-256 identique : le fichier d'origine n'a pas changé d'un octet
        Assert.False(Directory.GetDirectories(CheminsApp.DossierDonnees, "Import_*").Any());   // copie de travail supprimée
    }

    [Fact]
    public void Les_fichiers_wal_et_shm_sont_copies_et_les_lignes_qui_n_y_sont_que_arrivent_dans_la_base_importee()
    {
        string a = TestDb.CreerBase(Fichier("a"), 2);
        string b = Fichier("b");
        Directory.CreateDirectory(Path.GetDirectoryName(b)!);
        using (var c = new SqliteConnection($"Data Source={a};Pooling=False"))
        {
            c.Open();
            foreach (var sql in new[] { "PRAGMA journal_mode=WAL", "PRAGMA wal_autocheckpoint=0" })
            {
                using var cmd = c.CreateCommand(); cmd.CommandText = sql; cmd.ExecuteScalar();
            }
            for (int i = 100; i < 103; i++) TestDb.Inserer(c, "ventes", new() { ["IdVente"] = (long)i, ["numeroVente"] = "V-" + i, ["Statut"] = "Active", ["MontantTotal"] = "100", ["UserId"] = 1L });
            // la connexion est ouverte : les 3 lignes ne sont que dans le fichier -wal ; on copie les trois fichiers
            foreach (var suffixe in new[] { "", "-wal", "-shm" })
                ImportBaseService.CopierFichier(a + suffixe, b + suffixe);
            Assert.True(new FileInfo(b + "-wal").Length > 0);
        }
        SqliteConnection.ClearAllPools();
        var empreintes = new[] { "", "-wal", "-shm" }.Where(s => File.Exists(b + s)).ToDictionary(s => s, s => Sha(b + s));

        var r = ImportBaseService.Importer(b, null);

        Assert.Equal(IssueImport.Importee, r.Issue);
        Assert.Equal(5, Ventes(CheminsApp.CheminBase));                         // 2 + 3 lignes issues du -wal
        foreach (var kv in empreintes) Assert.Equal(kv.Value, Sha(b + kv.Key));  // aucun des trois fichiers d'origine n'a changé
        Assert.True(r.OriginalIntact);
    }

    // ───────────── contrôles bloquants ─────────────

    private static string Controle(ResultatImport r, string nom) => r.Controles.Single(c => c.Nom == nom).Detail;

    [Fact]
    public void Fichier_abime_ou_pas_une_base_est_refuse_et_rien_n_est_modifie()
    {
        string dossier = Path.GetDirectoryName(Fichier("abime"))!;
        Directory.CreateDirectory(dossier);
        string source = Path.Combine(dossier, CheminsApp.NomFichierBase);
        File.WriteAllBytes(source, Enumerable.Range(0, 5000).Select(i => (byte)(i * 31 % 251)).ToArray());
        string sha = Sha(source);

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Bloquee, r.Issue);
        Assert.Contains(r.Controles, c => c.Nom == "Intégrité de la base" && !c.Ok);
        Assert.False(File.Exists(CheminsApp.CheminBase));
        Assert.Equal(sha, Sha(source));
    }

    [Fact]
    public void Base_sans_table_des_migrations_est_refusee()
    {
        string dossier = Path.GetDirectoryName(Fichier("sansmigrations"))!;
        Directory.CreateDirectory(dossier);
        string source = Path.Combine(dossier, CheminsApp.NomFichierBase);
        using (var c = new SqliteConnection($"Data Source={source};Pooling=False"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "CREATE TABLE produits (Id INTEGER); CREATE TABLE ventes (IdVente INTEGER); CREATE TABLE Users (Id INTEGER);";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Bloquee, r.Issue);
        Assert.Contains("Aucune table des migrations", Controle(r, "Table des migrations"));
        Assert.False(File.Exists(CheminsApp.CheminBase));
    }

    [Fact]
    public void Migration_inconnue_base_d_une_version_plus_recente_est_refusee()
    {
        string source = TestDb.CreerBase(Fichier("futur"), 3);
        using (var c = new SqliteConnection($"Data Source={source};Pooling=False"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "INSERT INTO \"__EFMigrationsHistory\" (MigrationId, ProductVersion) VALUES ('29990101000000_VersionDuFutur', '9.0.0')";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Bloquee, r.Issue);
        Assert.Contains("migration inconnue", Controle(r, "Version de la base reconnue"));
        Assert.Contains("29990101000000_VersionDuFutur", Controle(r, "Version de la base reconnue"));
        Assert.False(File.Exists(CheminsApp.CheminBase));
    }

    [Theory]
    [InlineData(false, true)]    // aucun utilisateur administrateur
    [InlineData(true, false)]    // administrateur désactivé
    public void Sans_administrateur_actif_l_import_est_refuse(bool admin, bool actif)
    {
        TestDb.CreerBase(CheminsApp.CheminBase, 4);          // base active : doit rester intacte
        string shaActive = Sha(CheminsApp.CheminBase);
        string source = TestDb.CreerBase(Fichier("sansadmin" + admin + actif), 5, admin: admin, adminActif: actif);
        var sauvegardesAvant = SauvegardesAvantImport().ToList();

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Bloquee, r.Issue);
        Assert.Contains(r.Controles, c => c.Nom == "Administrateur actif" && !c.Ok);
        Assert.Equal(shaActive, Sha(CheminsApp.CheminBase));                        // la base active n'a pas été touchée
        Assert.Equal(sauvegardesAvant, SauvegardesAvantImport().ToList());           // rien n'a été remplacé : pas de sauvegarde « avant-import »
    }

    [Fact]
    public void Une_ligne_qui_disparait_pendant_la_mise_a_jour_bloque_l_import()
    {
        TestDb.CreerBase(CheminsApp.CheminBase, 4);
        string shaActive = Sha(CheminsApp.CheminBase);
        string source = TestDb.CreerBase(Fichier("perte"), 6);

        // on simule une migration qui ferait disparaître une vente de la copie
        var r = ImportBaseService.Importer(source, null, apresMigration: copie =>
        {
            using var c = new SqliteConnection($"Data Source={copie};Pooling=False");
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "DELETE FROM ventes WHERE IdVente = 1";
            cmd.ExecuteNonQuery();
        });

        Assert.Equal(IssueImport.Bloquee, r.Issue);
        Assert.Contains(r.Comparaison, l => l.Table == "ventes" && l.Perdue && l.Avant == 6 && l.Apres == 5);
        Assert.Contains("ventes", r.Message);
        Assert.Equal(shaActive, Sha(CheminsApp.CheminBase));
        Assert.Equal(4, Ventes(CheminsApp.CheminBase));
    }

    // ───────────── sauvegarde avant-import et annulation ─────────────

    [Fact]
    public void La_base_active_est_sauvegardee_avant_d_etre_remplacee()
    {
        TestDb.CreerBase(CheminsApp.CheminBase, 4);
        string source = TestDb.CreerBase(Fichier("nouvelle"), 9);
        var avant = SauvegardesAvantImport().ToHashSet();

        var r = ImportBaseService.Importer(source, null);

        Assert.Equal(IssueImport.Importee, r.Issue);
        Assert.NotNull(r.SauvegardeAvantImport);
        Assert.Contains("avant-import", Path.GetFileName(r.SauvegardeAvantImport));
        Assert.Single(SauvegardesAvantImport().Where(f => !avant.Contains(f)));
        Assert.Equal(4, Ventes(r.SauvegardeAvantImport!));                          // la sauvegarde contient l'ancienne base active
        Assert.Equal(9, Ventes(CheminsApp.CheminBase));                             // et la base active est maintenant l'import
    }

    [Theory]
    [InlineData(EtapeImport.Migration)]
    [InlineData(EtapeImport.Remplacement)]
    public void Annuler_a_chaque_etape_ne_modifie_rien(EtapeImport etapeAnnulee)
    {
        TestDb.CreerBase(CheminsApp.CheminBase, 4);
        string shaActive = Sha(CheminsApp.CheminBase);
        string source = TestDb.CreerBase(Fichier("annule" + etapeAnnulee), 8);
        string shaSource = Sha(source);
        var sauvegardesAvant = SauvegardesAvantImport().ToList();
        var etapesVues = new List<EtapeImport>();

        var r = ImportBaseService.Importer(source, null, e => { etapesVues.Add(e); return e != etapeAnnulee; });

        Assert.Equal(IssueImport.Abandon, r.Issue);
        Assert.Contains(etapeAnnulee, etapesVues);
        Assert.Equal(shaActive, Sha(CheminsApp.CheminBase));
        Assert.Equal(shaSource, Sha(source));
        Assert.Equal(sauvegardesAvant, SauvegardesAvantImport().ToList());
        Assert.False(Directory.GetDirectories(CheminsApp.DossierDonnees, "Import_*").Any());   // la copie de travail est supprimée
    }

    // ───────────── 7a : import d'une sauvegarde chiffrée ─────────────

    private string Chiffrer(string source, string phrase)
    {
        string destination = Path.Combine(Path.GetDirectoryName(source)!, "PharmacieDB_2026-10-13" + SauvegardeExterneService.Extension);
        byte[] sel = RandomNumberGenerator.GetBytes(16);
        SauvegardeExterneService.ChiffrerFichier(source, destination, SauvegardeExterneService.DeriverCle(phrase, sel), sel);
        return destination;
    }

    [Fact]
    public void Une_sauvegarde_chiffree_s_importe_avec_la_bonne_phrase_et_est_refusee_avec_une_mauvaise()
    {
        string clair = TestDb.CreerBase(Fichier("clair"), 6);
        string chiffre = Chiffrer(clair, "une phrase secrète de test");
        string shaChiffre = Sha(chiffre);

        var mauvaise = ImportBaseService.Importer(chiffre, "mauvaise phrase");
        Assert.Equal(IssueImport.Bloquee, mauvaise.Issue);
        Assert.Contains("phrase secrète", mauvaise.Message);
        Assert.False(File.Exists(CheminsApp.CheminBase));

        var bonne = ImportBaseService.Importer(chiffre, "une phrase secrète de test");
        Assert.Equal(IssueImport.Importee, bonne.Issue);
        Assert.Equal(6, Ventes(CheminsApp.CheminBase));
        Assert.Equal(shaChiffre, Sha(chiffre));                                     // le fichier chiffré n'est pas modifié non plus
    }

    // ───────────── l'assistant à l'écran ─────────────

    [Fact]
    public void Assistant_a_l_ecran_quatre_etapes_annulables_puis_remplacement()
    {
        string source = TestDb.CreerBase(Fichier("ecran"), 5);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAssistantImport(source, null);
                f.Demarrer();
                Assert.Equal(FormAssistantImport.Etape.Controles, f.Courante);                 // étape 2 : contrôles réussis
                Assert.True(f.Grille.Rows.Count >= 4);
                f.Suivant();
                Assert.Equal(FormAssistantImport.Etape.Comparaison, f.Courante);               // étape 3 : mise à jour et comparaison
                Assert.Contains(f.Grille.Rows.Cast<System.Windows.Forms.DataGridViewRow>(), r => Convert.ToString(r.Cells["Nom"].Value) == "Lignes : ventes");
                Assert.False(File.Exists(CheminsApp.CheminBase));                              // rien n'est encore remplacé
                f.Suivant();
                Assert.Equal(FormAssistantImport.Etape.Remplacement, f.Courante);              // étape 4 : confirmation
                Assert.False(File.Exists(CheminsApp.CheminBase));
                f.Suivant();
                Assert.Equal(FormAssistantImport.Etape.Termine, f.Courante);
            });
        }
        finally { cts.Cancel(); }
        Assert.Equal(5, Ventes(CheminsApp.CheminBase));
    }

    [Fact]
    public void Assistant_a_l_ecran_annuler_apres_les_controles_ne_remplace_rien_et_un_controle_en_echec_bloque()
    {
        string bon = TestDb.CreerBase(Fichier("bon"), 5);
        UiHelper.EnSta(() =>
        {
            using var f = new FormAssistantImport(bon, null);
            f.Demarrer();
            Assert.Equal(FormAssistantImport.Etape.Controles, f.Courante);
            // Annuler : la fenêtre se ferme sans rien remplacer
            f.Dispose();
        });
        Assert.False(File.Exists(CheminsApp.CheminBase));

        string futur = TestDb.CreerBase(Fichier("futur2"), 2);
        using (var c = new SqliteConnection($"Data Source={futur};Pooling=False"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "INSERT INTO \"__EFMigrationsHistory\" (MigrationId, ProductVersion) VALUES ('29990101000000_Autre', '9.0.0')";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();
        UiHelper.EnSta(() =>
        {
            using var f = new FormAssistantImport(futur, null);
            f.Demarrer();
            Assert.Equal(FormAssistantImport.Etape.Bloque, f.Courante);
            f.Suivant();                                                                       // rien à faire : le bouton « Continuer » est masqué
            Assert.Equal(FormAssistantImport.Etape.Bloque, f.Courante);
        });
        Assert.False(File.Exists(CheminsApp.CheminBase));
    }
}
