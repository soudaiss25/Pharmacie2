using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class SauvegardeExterneTests
{
    private static string DossierTemp(string nom)
    {
        string racine = Environment.GetEnvironmentVariable("RUNNER_TEMP") is { Length: > 0 } r ? r : Path.GetTempPath();
        string d = Path.Combine(racine, "p2ext_" + nom + "_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(d);
        return d;
    }

    private static void Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        ParametresApp.ReinitialiserPourTests();
    }

    private static string SauvegardeDuJour()
    {
        string locale = Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_{DateTime.Now:yyyy-MM-dd}.sqlite");
        if (File.Exists(locale)) File.Delete(locale);
        SauvegardeAutomatique.Sauvegarder(locale);
        return locale;
    }

    [Fact]
    public void Chiffrement_puis_dechiffrement_redonne_le_meme_fichier_octet_pour_octet()
    {
        string dossier = DossierTemp("chiffre");
        string source = Path.Combine(dossier, "source.bin");
        var contenu = new byte[1_500_000];
        new Random(7).NextBytes(contenu);
        File.WriteAllBytes(source, contenu);

        byte[] sel = new byte[16]; new Random(1).NextBytes(sel);
        byte[] cle = SauvegardeExterneService.DeriverCle("une phrase secrète solide", sel, 1000);
        string chiffre = Path.Combine(dossier, "copie" + SauvegardeExterneService.Extension);
        SauvegardeExterneService.ChiffrerFichier(source, chiffre, cle, sel, 1000);

        Assert.NotEqual(contenu, File.ReadAllBytes(chiffre));                 // vraiment chiffré
        string clair = Path.Combine(dossier, "clair.bin");
        SauvegardeExterneService.DechiffrerFichier(chiffre, clair, "une phrase secrète solide");
        Assert.Equal(contenu, File.ReadAllBytes(clair));
    }

    [Fact]
    public void Mauvaise_phrase_secrete_refusee_proprement_et_sans_rien_ecrire()
    {
        string dossier = DossierTemp("mauvaise");
        string source = Path.Combine(dossier, "s.bin");
        File.WriteAllBytes(source, new byte[] { 1, 2, 3, 4, 5 });
        byte[] sel = new byte[16];
        string chiffre = Path.Combine(dossier, "c" + SauvegardeExterneService.Extension);
        SauvegardeExterneService.ChiffrerFichier(source, chiffre, SauvegardeExterneService.DeriverCle("bonne phrase 123", sel, 1000), sel, 1000);

        string sortie = Path.Combine(dossier, "sortie.bin");
        Assert.Throws<PhraseSecreteIncorrecteException>(() => SauvegardeExterneService.DechiffrerFichier(chiffre, sortie, "mauvaise phrase"));
        Assert.False(File.Exists(sortie));
        Assert.Throws<FichierSauvegardeInvalideException>(() => SauvegardeExterneService.DechiffrerFichier(source, sortie, "x"));   // pas une sauvegarde chiffrée
    }

    [Fact]
    public void Copie_du_jour_chiffree_puis_restauration_complete_sur_base_remplie()
    {
        Preparer();
        int pid = TestDb.NouveauProduit("Doliprane test", 42, 10, "Plaquette");
        string externe = DossierTemp("externe");
        SauvegardeExterneService.Configurer(externe, "phrase secrète de la pharmacie");
        SauvegardeDuJour();

        Assert.True(SauvegardeExterneService.CopierDuJour());
        string copie = Directory.GetFiles(externe, "*" + SauvegardeExterneService.Extension).Single();
        Assert.Empty(Directory.GetFiles(externe, "*.tmp"));                    // copie via fichier temporaire puis renommage
        Assert.NotNull(ParametresApp.Actuels.DerniereCopieExterne);

        // la base est ensuite abîmée : le produit disparaît
        using (var c = TestDb.Ouvrir()) { using var cmd = c.CreateCommand(); cmd.CommandText = "DELETE FROM produits"; cmd.ExecuteNonQuery(); }
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));

        // mauvaise phrase : refusée, base inchangée
        Assert.Throws<PhraseSecreteIncorrecteException>(() => SauvegardeExterneService.Restaurer(copie, "pas la bonne"));
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));

        SauvegardeExterneService.Restaurer(copie, "phrase secrète de la pharmacie");
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
        Assert.Equal(42, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
        Assert.NotEmpty(Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-restauration_*.sqlite"));   // base actuelle sauvegardée avant remplacement
    }

    [Fact]
    public void Fichier_qui_n_est_pas_une_base_est_refuse_et_la_base_reste_intacte()
    {
        Preparer();
        TestDb.NouveauProduit("Reste", 5, 1);
        string faux = Path.Combine(DossierTemp("faux"), "faux.sqlite");
        File.WriteAllText(faux, "ceci n'est pas une base de données");
        Assert.Throws<FichierSauvegardeInvalideException>(() => SauvegardeExterneService.Restaurer(faux, null));
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
    }

    [Fact]
    public void Dossier_externe_absent_ne_bloque_pas_et_n_enregistre_aucune_copie()
    {
        Preparer();
        string externe = DossierTemp("debranche");
        SauvegardeExterneService.Configurer(externe, "phrase secrète de la pharmacie");
        SauvegardeDuJour();
        Directory.Delete(externe, true);   // clé USB débranchée

        Assert.False(SauvegardeExterneService.CopierDuJour());                  // aucune exception
        Assert.Null(ParametresApp.Actuels.DerniereCopieExterne);

        Directory.CreateDirectory(externe);                                     // clé rebranchée : l'essai suivant réussit
        Assert.True(SauvegardeExterneService.CopierDuJour());
        Assert.NotNull(ParametresApp.Actuels.DerniereCopieExterne);
    }

    [Fact]
    public void Rotation_gardant_les_30_dernieres_copies()
    {
        Preparer();
        string externe = DossierTemp("rotation");
        SauvegardeExterneService.Configurer(externe, "phrase secrète de la pharmacie");
        SauvegardeDuJour();
        for (int i = 1; i <= 34; i++)
            File.WriteAllText(Path.Combine(externe, $"PharmacieDB_{new DateTime(2025, 1, 1).AddDays(i):yyyy-MM-dd}{SauvegardeExterneService.Extension}"), "ancienne");

        Assert.True(SauvegardeExterneService.CopierDuJour());
        var restantes = Directory.GetFiles(externe, "PharmacieDB_*" + SauvegardeExterneService.Extension).Select(Path.GetFileName).OrderBy(n => n).ToList();
        Assert.Equal(SauvegardeExterneService.NbCopiesGardees, restantes.Count);
        Assert.Contains($"PharmacieDB_{DateTime.Now:yyyy-MM-dd}{SauvegardeExterneService.Extension}", restantes);   // la copie du jour est gardée
    }

    [Fact]
    public void Phrase_trop_courte_refusee_et_rien_n_est_ecrit_en_clair()
    {
        Preparer();
        Assert.Throws<ArgumentException>(() => SauvegardeExterneService.Configurer(DossierTemp("court"), "court"));

        SauvegardeExterneService.Configurer(DossierTemp("ok"), "phrase secrète de la pharmacie");
        string reglages = File.ReadAllText(Path.Combine(CheminsApp.DossierDonnees, "parametres.json"));
        Assert.DoesNotContain("phrase secrète", reglages);
        string journal = Directory.GetFiles(CheminsApp.DossierLogs).Select(File.ReadAllText).Aggregate("", (a, b) => a + b);
        Assert.DoesNotContain("phrase secrète de la pharmacie", journal);
    }

    [Fact]
    public void Alerte_Ma_journee_si_aucun_dossier_ou_copie_trop_ancienne()
    {
        Preparer();
        var alerte = TableauDeBordService.AlerteSauvegardeExterne(DateTime.Now);
        Assert.NotNull(alerte);                                                // rien de configuré
        Assert.Equal(TypeAFaire.SauvegardeExterne, alerte!.Type);
        Assert.Equal("attention", alerte.Niveau);

        SauvegardeExterneService.Configurer(DossierTemp("alerte"), "phrase secrète de la pharmacie");
        ParametresApp.Modifier(p => p.DerniereCopieExterne = DateTime.Now.AddDays(-3));
        Assert.Null(TableauDeBordService.AlerteSauvegardeExterne(DateTime.Now));   // 3 jours : pas d'alerte

        ParametresApp.Modifier(p => p.DerniereCopieExterne = DateTime.Now.AddDays(-9));
        var vieille = TableauDeBordService.AlerteSauvegardeExterne(DateTime.Now);
        Assert.NotNull(vieille);
        Assert.Contains("9 jours", vieille!.Texte);
    }
}
