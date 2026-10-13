using Microsoft.Data.Sqlite;
using Pharmacie2.Services;
using Xunit;

/// <summary>R7 : retrouver l'ancienne base du déploiement d'avril, la copier, et ne jamais créer de base vide quand une installation existe.</summary>
public class MigrationEmplacementBaseTests : IDisposable
{
    private readonly string _racine = Path.Combine(Path.GetTempPath(), "p2_emplacements_" + Guid.NewGuid().ToString("N"));

    public MigrationEmplacementBaseTests()
    {
        Directory.CreateDirectory(_racine);
        TestDb.Reinitialiser();   // la base de destination n'existe pas
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        try { Directory.Delete(_racine, true); } catch { }
    }

    private string Dossier(params string[] parties)
    {
        string d = Path.Combine(new[] { _racine }.Concat(parties).ToArray());
        Directory.CreateDirectory(d);
        return d;
    }

    private static string CreerBase(string dossier, int ventes, DateTime? date = null)
    {
        // une vraie base de la pharmacie (migrations, administrateur, ventes) : l'assistant d'import la contrôle avant de la reprendre
        string f = TestDb.CreerBase(Path.Combine(dossier, CheminsApp.NomFichierBase), ventes);
        if (date.HasValue) File.SetLastWriteTime(f, date.Value);
        return f;
    }

    private static Func<IReadOnlyList<BaseTrouvee>, BaseTrouvee?> JamaisChoisir => _ => throw new InvalidOperationException("aucun choix attendu");
    private static Func<string?> JamaisDemander => () => throw new InvalidOperationException("aucun fichier à demander");

    // ───────── cas 1 : une base trouvée (aux différents emplacements) ─────────

    [Fact]
    public void Une_base_trouvee_est_copiee_et_l_original_est_conserve()
    {
        string source = CreerBase(Dossier("ancien"), 3);
        var e = new EmplacementsAnciens { Dossiers = { Dossier("ancien") } };

        var issue = MigrationEmplacementBase.Executer(e, JamaisChoisir, JamaisDemander);

        Assert.Equal(MigrationEmplacementBase.Issue.Copiee, issue);
        Assert.True(File.Exists(CheminsApp.CheminBase));
        Assert.True(File.Exists(source));                                   // jamais déplacée
        Assert.Equal(3, MigrationEmplacementBase.CompterVentes(CheminsApp.CheminBase));
    }

    [Fact]
    public void Les_anciens_emplacements_sont_tous_fouilles()
    {
        // %APPDATA%\Pharmacie2
        var appData = Dossier("Roaming", "Pharmacie2");
        CreerBase(appData, 1);
        Assert.Single(MigrationEmplacementBase.Analyser(new EmplacementsAnciens { DossierAppData = appData }).Bases);

        // Dossier d'installation lu dans les clés de désinstallation (« LIGUAPHARME » ou « Pharma » dans le nom)
        var install = Dossier("Install", "Quelque part");
        CreerBase(install, 2);
        var viaRegistre = MigrationEmplacementBase.Analyser(new EmplacementsAnciens
        {
            EntreesDesinstallation = { ("LIGUAPHARME 1.0", install), ("Autre logiciel", Dossier("Autre")) }
        });
        Assert.Single(viaRegistre.Bases);
        Assert.Equal(2, viaRegistre.Bases[0].NbVentes);

        // Sous-dossiers « Pharma » de Program Files, Program Files (x86), %LOCALAPPDATA%\Programs et VirtualStore
        var racines = new[] { "ProgramFiles", "ProgramFiles86", "LocalPrograms", "VirtualStore_PF", "VirtualStore_PF86" };
        foreach (var r in racines)
        {
            TestDb.Reinitialiser();
            var racine = Dossier(r);
            CreerBase(Dossier(r, "Ma PHARMAcie"), 4);
            CreerBase(Dossier(r, "Autre logiciel"), 99);                    // le nom ne contient pas « Pharma » : ignoré
            var a = MigrationEmplacementBase.Analyser(new EmplacementsAnciens { RacinesPharma = { racine } });
            Assert.Single(a.Bases);
            Assert.Equal(4, a.Bases[0].NbVentes);
        }
    }

    [Fact]
    public void Toutes_les_candidates_sont_journalisees_avec_chemin_taille_date_et_nombre_de_ventes()
    {
        CreerBase(Dossier("a"), 5);
        MigrationEmplacementBase.Analyser(new EmplacementsAnciens { Dossiers = { Dossier("a") } });
        string journal = File.ReadAllText(Directory.GetFiles(CheminsApp.DossierLogs, "journal_*.log").OrderBy(f => f).Last());
        Assert.Contains("Base candidate : " + Path.Combine(Dossier("a"), CheminsApp.NomFichierBase), journal);
        Assert.Contains("5 vente(s)", journal);
        Assert.Contains("octets", journal);
    }

    // ───────── cas 2 : plusieurs bases non vides ─────────

    [Fact]
    public void Plusieurs_bases_non_vides_proposent_une_liste_avec_la_plus_recente_en_premier_et_l_utilisateur_choisit()
    {
        CreerBase(Dossier("recente"), 40, DateTime.Now.AddDays(-1));
        CreerBase(Dossier("ancienne"), 7, DateTime.Now.AddDays(-200));
        CreerBase(Dossier("videuse"), 0);                                   // fichier vide : n'est pas proposé
        var e = new EmplacementsAnciens { Dossiers = { Dossier("recente"), Dossier("ancienne"), Dossier("videuse") } };

        IReadOnlyList<BaseTrouvee>? propose = null;
        var issue = MigrationEmplacementBase.Executer(e, liste =>
        {
            propose = liste;
            return liste[1];                                                // l'utilisateur préfère l'ancienne
        }, JamaisDemander);

        Assert.Equal(MigrationEmplacementBase.Issue.Copiee, issue);
        Assert.Equal(2, propose!.Count);
        Assert.Equal(40, propose[0].NbVentes);                              // la plus récente d'abord (présélectionnée)
        Assert.Equal(7, MigrationEmplacementBase.CompterVentes(CheminsApp.CheminBase));   // le choix de l'utilisateur est respecté
    }

    [Fact]
    public void Plusieurs_bases_sans_choix_aucune_base_vide_n_est_creee()
    {
        CreerBase(Dossier("a"), 1, DateTime.Now.AddDays(-1));
        CreerBase(Dossier("b"), 2, DateTime.Now.AddDays(-5));
        var e = new EmplacementsAnciens { Dossiers = { Dossier("a"), Dossier("b") } };
        Assert.Equal(MigrationEmplacementBase.Issue.Abandon, MigrationEmplacementBase.Executer(e, _ => null, JamaisDemander));
        Assert.False(File.Exists(CheminsApp.CheminBase));
    }

    // ───────── cas 3 : aucune base, mais des traces → écran « Base de données introuvable », pas de FormInitialize ─────────

    [Fact]
    public void Aucune_base_mais_des_traces_ne_propose_pas_de_premiere_installation_et_demande_le_fichier()
    {
        var traces = new Func<EmplacementsAnciens>[]
        {
            () => new EmplacementsAnciens { DossierAppData = Dossier("Roaming", "Pharmacie2") },                          // dossier %APPDATA%\Pharmacie2
            () => new EmplacementsAnciens { EntreesDesinstallation = { ("LIGUAPHARME", null) } },                          // entrée de désinstallation
            () => { CreerBase(Dossier("vide"), 0); return new EmplacementsAnciens { Dossiers = { Dossier("vide") } }; }    // PharmacieDB.sqlite même vide
        };
        foreach (var trace in traces)
        {
            TestDb.Reinitialiser();
            var e = trace();
            Assert.Equal(CasBase.IntrouvableDejaInstallee, MigrationEmplacementBase.Analyser(e).Cas);

            // L'utilisateur renonce : rien n'est créé (donc jamais de base vide ni de FormInitialize)
            Assert.Equal(MigrationEmplacementBase.Issue.Abandon, MigrationEmplacementBase.Executer(e, JamaisChoisir, () => null));
            Assert.False(File.Exists(CheminsApp.CheminBase));
        }

        // L'utilisateur choisit un fichier : il est COPIÉ
        string choisi = CreerBase(Dossier("cle usb"), 9);
        var e2 = new EmplacementsAnciens { DossierAppData = Dossier("Roaming", "Pharmacie2") };
        Assert.Equal(MigrationEmplacementBase.Issue.Copiee, MigrationEmplacementBase.Executer(e2, JamaisChoisir, () => choisi));
        Assert.True(File.Exists(choisi));
        Assert.Equal(9, MigrationEmplacementBase.CompterVentes(CheminsApp.CheminBase));
    }

    // ───────── cas 4 : aucune trace → vraie première installation ─────────

    [Fact]
    public void Aucune_trace_est_une_vraie_premiere_installation()
    {
        var e = new EmplacementsAnciens
        {
            Dossiers = { Dossier("vide1") },
            DossierAppData = Path.Combine(_racine, "inexistant", "Pharmacie2"),
            RacinesPharma = { Dossier("PF") },
            EntreesDesinstallation = { ("Un autre logiciel", Dossier("autre")) }
        };
        Assert.Equal(CasBase.PremiereInstallation, MigrationEmplacementBase.Analyser(e).Cas);
        Assert.Equal(MigrationEmplacementBase.Issue.RienACopier, MigrationEmplacementBase.Executer(e, JamaisChoisir, JamaisDemander));
        Assert.False(File.Exists(CheminsApp.CheminBase));                   // la base sera créée par l'application (FormInitialize)
    }

    [Fact]
    public void Si_la_base_actuelle_existe_deja_on_ne_cherche_rien()
    {
        TestDb.MigrerTout();
        var e = new EmplacementsAnciens { Dossiers = { Dossier("jamais lu") } };
        Assert.Equal(MigrationEmplacementBase.Issue.RienACopier, MigrationEmplacementBase.Executer(e, JamaisChoisir, JamaisDemander));
    }
}
