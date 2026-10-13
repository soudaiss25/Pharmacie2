using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>Mises à jour automatiques (Velopack) : dossiers séparés, aucune exception vers l'interface, jamais de redémarrage pendant une vente.</summary>
public class MiseAJourTests : IDisposable
{
    public MiseAJourTests() => ParametresApp.ReinitialiserPourTests();

    public void Dispose()
    {
        ParametresApp.ReinitialiserPourTests();
        MiseAJourService.Instance = null!;
    }

    private sealed class FauxGestionnaire : IGestionnaireMiseAJour
    {
        public bool Installe = true;
        public string Actuelle = "1.0.0";
        public Func<Task<InfoMiseAJour?>> Verifier = () => Task.FromResult<InfoMiseAJour?>(null);
        public Func<Task> Telecharger = () => Task.CompletedTask;
        public int Verifications, Telechargements, ApplicationsALaFermeture, ApplicationsRedemarrage;
        public bool UnRedemarrageDemande;

        public bool EstInstalle => Installe;
        public string VersionActuelle => Actuelle;
        public Task<InfoMiseAJour?> VerifierAsync() { Interlocked.Increment(ref Verifications); return Verifier(); }
        public Task TelechargerAsync(InfoMiseAJour info) { Interlocked.Increment(ref Telechargements); return Telecharger(); }
        public void AppliquerALaFermeture(InfoMiseAJour info) => ApplicationsALaFermeture++;
        public void AppliquerEtRedemarrer(InfoMiseAJour info) { ApplicationsRedemarrage++; UnRedemarrageDemande = true; }
    }

    private static (MiseAJourService service, FauxGestionnaire faux) Creer(Action<FauxGestionnaire>? regler = null)
    {
        var faux = new FauxGestionnaire();
        regler?.Invoke(faux);
        return (new MiseAJourService(_ => faux), faux);
    }

    private static string RacineDepot()
    {
        string d = AppContext.BaseDirectory;
        for (int i = 0; i < 10 && !File.Exists(Path.Combine(d, "Pharmacie2.csproj")); i++) d = Path.GetFullPath(Path.Combine(d, ".."));
        Assert.True(File.Exists(Path.Combine(d, "Pharmacie2.csproj")), "racine du dépôt introuvable");
        return d;
    }

    // ───────────── 1. installation et données : deux dossiers distincts ─────────────

    [Fact]
    public void Le_dossier_d_installation_Velopack_et_le_dossier_des_donnees_sont_distincts_et_sans_imbrication()
    {
        string install = Path.GetFullPath(MiseAJourService.DossierInstallation).TrimEnd('\\') + "\\";
        string donnees = Path.GetFullPath(CheminsApp.DossierDonnees).TrimEnd('\\') + "\\";
        Assert.NotEqual(install, donnees);
        Assert.False(donnees.StartsWith(install, StringComparison.OrdinalIgnoreCase), "les données ne doivent jamais être dans le dossier d'installation");
        Assert.False(install.StartsWith(donnees, StringComparison.OrdinalIgnoreCase), "l'installation ne doit jamais être dans le dossier des données");
        Assert.Equal("LIGUAPHARME", MiseAJourService.PackId);
        Assert.EndsWith(@"\LIGUAPHARME\", install, StringComparison.OrdinalIgnoreCase);
        // le vrai dossier de données (hors tests) est %LOCALAPPDATA%\Pharmacie2Data : jamais confondu avec %LOCALAPPDATA%\LIGUAPHARME
        string vraiesDonnees = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pharmacie2Data");
        Assert.NotEqual(Path.GetFullPath(vraiesDonnees).TrimEnd('\\'), Path.GetFullPath(MiseAJourService.DossierInstallation).TrimEnd('\\'));
    }

    [Fact]
    public void Version_unique_du_csproj_outil_vpk_et_package_Velopack_a_la_meme_version()
    {
        string racine = RacineDepot();
        var csproj = XDocument.Load(Path.Combine(racine, "Pharmacie2.csproj"));
        string version = csproj.Descendants("Version").Single().Value;
        Assert.Matches(@"^\d+\.\d+\.\d+$", version);
        Assert.Equal(version, AppInfo.Version);                                  // affichée dans Administration et dans le rapport

        string paquet = csproj.Descendants("PackageReference").Single(p => (string?)p.Attribute("Include") == "Velopack").Attribute("Version")!.Value;
        string manifeste = File.ReadAllText(Path.Combine(racine, ".config", "dotnet-tools.json"));
        Assert.Contains("\"vpk\"", manifeste);
        Assert.Matches($"\"version\":\\s*\"{Regex.Escape(paquet)}\"", manifeste);   // vpk à la MÊME version que le package
        Assert.DoesNotContain("*", paquet);                                      // épinglée
    }

    [Fact]
    public void Le_workflow_de_publication_respecte_les_regles()
    {
        string yml = File.ReadAllText(Path.Combine(RacineDepot(), ".github", "workflows", "release.yml"));
        Assert.Contains("tags:", yml);
        Assert.Contains("'v*.*.*'", yml);
        Assert.DoesNotContain("branches:", yml);                                  // jamais déclenché par une simple publication de code
        Assert.Contains("dotnet test", yml);                                      // tests d'abord
        Assert.True(yml.IndexOf("dotnet test", StringComparison.Ordinal) < yml.IndexOf("vpk pack", StringComparison.Ordinal));
        Assert.True(yml.IndexOf("dotnet test", StringComparison.Ordinal) < yml.IndexOf("vpk upload", StringComparison.Ordinal));
        Assert.Contains("<Version>", yml);                                        // version du tag = version du csproj
        Assert.Contains("-test", yml);                                            // canal test
        Assert.Contains("--self-contained -r win-x64", yml);
        Assert.Contains("vpk download github", yml);                              // release précédente pour les deltas
        Assert.Contains("--packId LIGUAPHARME", yml);
        Assert.Contains("--packTitle LIGUAPHARME", yml);
        Assert.Contains("--icon resources/pharmacie_icon.ico", yml);
        Assert.Contains("vpk upload github", yml);
        Assert.Contains("secrets.GITHUB_TOKEN", yml);
        Assert.DoesNotMatch(@"ghp_[A-Za-z0-9]{20,}|github_pat_[A-Za-z0-9_]{20,}", yml);   // aucun jeton écrit dans le dépôt
        Assert.True(File.Exists(Path.Combine(RacineDepot(), "resources", "pharmacie_icon.ico")));
    }

    [Fact]
    public void VelopackApp_est_la_toute_premiere_instruction_de_Main()
    {
        string programme = File.ReadAllText(Path.Combine(RacineDepot(), "Program.cs"));
        int main = programme.IndexOf("static void Main()", StringComparison.Ordinal);
        string corps = programme[(programme.IndexOf('{', main) + 1)..];
        string premiere = corps.Split('\n').Select(l => l.Trim()).First(l => l.Length > 0 && !l.StartsWith("//"));
        Assert.Equal("VelopackApp.Build().Run();", premiere);
    }

    // ───────────── 2. vérification : jamais d'exception vers l'interface ─────────────

    [Fact]
    public async Task Reseau_coupe_ou_source_introuvable_ne_leve_jamais_d_exception()
    {
        var erreurs = new Func<Task<InfoMiseAJour?>>[]
        {
            () => throw new HttpRequestException("Réseau coupé"),
            () => Task.FromException<InfoMiseAJour?>(new HttpRequestException("Pas de connexion")),
            () => Task.FromException<InfoMiseAJour?>(new TaskCanceledException("délai dépassé")),
            () => Task.FromException<InfoMiseAJour?>(new DirectoryNotFoundException(@"Source introuvable : \\serveur\mises-a-jour")),
            () => Task.FromException<InfoMiseAJour?>(new InvalidOperationException("réponse illisible")),
        };
        foreach (var erreur in erreurs)
        {
            var (service, faux) = Creer(f => f.Verifier = erreur);
            var r = await service.VerifierEtTelechargerAsync();
            Assert.Equal(EtatMiseAJour.Erreur, r.Etat);
            Assert.Contains("réessaiera", r.Message);
            Assert.Null(service.VersionPrete);
        }

        // téléchargement interrompu au milieu : même résultat
        var (s2, _) = Creer(f =>
        {
            f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null));
            f.Telecharger = () => Task.FromException(new HttpRequestException("connexion perdue à 40 %"));
        });
        Assert.Equal(EtatMiseAJour.Erreur, (await s2.VerifierEtTelechargerAsync()).Etat);
        Assert.Null(s2.VersionPrete);

        // le gestionnaire lui-même est cassé
        var casse = new MiseAJourService(_ => throw new InvalidOperationException("fabrique cassée"));
        Assert.Equal(EtatMiseAJour.NonInstallee, (await casse.VerifierEtTelechargerAsync()).Etat);
    }

    [Fact]
    public async Task Hors_installation_rien_ne_se_passe_meme_avec_le_vrai_gestionnaire_Velopack()
    {
        var reel = new MiseAJourService(canal => new GestionnaireVelopack(canal));   // les tests ne sont pas une installation Velopack
        Assert.False(reel.EstInstalle);
        var r = await reel.VerifierEtTelechargerAsync();
        Assert.Equal(EtatMiseAJour.NonInstallee, r.Etat);
        reel.Demarrer();                       // ne démarre aucun minuteur
        reel.AppliquerALaFermeture();          // sans effet
        reel.DetecterMiseAJourInstallee();
        Assert.Null(reel.PrendreAnnonce());
    }

    [Fact]
    public async Task Une_mise_a_jour_trouvee_est_telechargee_une_fois_puis_signalee()
    {
        var signalees = new List<string>();
        var (service, faux) = Creer(f => f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", "jeton")));
        service.MiseAJourPrete += v => signalees.Add(v);

        var r = await service.VerifierEtTelechargerAsync();

        Assert.Equal(EtatMiseAJour.Prete, r.Etat);
        Assert.Equal("1.0.1", r.VersionDisponible);
        Assert.Equal("1.0.1", service.VersionPrete);
        Assert.Equal(new[] { "1.0.1" }, signalees);
        Assert.Equal(1, faux.Telechargements);
        Assert.Equal(0, faux.ApplicationsRedemarrage);                     // jamais de redémarrage tout seul

        await service.VerifierEtTelechargerAsync();                         // une deuxième vérification ne re-télécharge pas
        Assert.Equal(1, faux.Telechargements);
        Assert.Equal(1, faux.Verifications);
    }

    [Fact]
    public async Task Un_abonne_qui_plante_ne_fait_pas_echouer_la_verification()
    {
        var (service, _) = Creer(f => f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null)));
        service.MiseAJourPrete += _ => throw new InvalidOperationException("l'écran est fermé");
        var r = await service.VerifierEtTelechargerAsync();
        Assert.Equal(EtatMiseAJour.Prete, r.Etat);
    }

    [Fact]
    public async Task La_verification_periodique_demarre_apres_le_delai_puis_se_repete_sans_bloquer()
    {
        var (service, faux) = Creer();
        Assert.Equal(TimeSpan.FromSeconds(30), MiseAJourService.DelaiPremiereVerification);
        Assert.Equal(TimeSpan.FromHours(4), MiseAJourService.IntervalleVerification);

        service.Demarrer(TimeSpan.FromMilliseconds(40), TimeSpan.FromMilliseconds(60));
        for (int i = 0; i < 80 && faux.Verifications < 3; i++) await Task.Delay(50);
        service.AppliquerALaFermeture();      // arrête le minuteur
        Assert.True(faux.Verifications >= 3, "vérifications effectuées : " + faux.Verifications);

        var (horsInstall, fauxHors) = Creer(f => f.Installe = false);
        horsInstall.Demarrer(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(10));
        await Task.Delay(200);
        Assert.Equal(0, fauxHors.Verifications);                            // hors installation : aucune vérification
    }

    // ───────────── installation : à la fermeture, ou « Installer maintenant » (refusé pendant une vente) ─────────────

    [Fact]
    public async Task A_la_fermeture_la_mise_a_jour_telechargee_est_appliquee_sans_redemarrage_force()
    {
        var (service, faux) = Creer(f => f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null)));
        service.AppliquerALaFermeture();
        Assert.Equal(0, faux.ApplicationsALaFermeture);                    // rien de prêt : rien à appliquer
        await service.VerifierEtTelechargerAsync();
        service.AppliquerALaFermeture();
        Assert.Equal(1, faux.ApplicationsALaFermeture);
        Assert.Equal(0, faux.ApplicationsRedemarrage);
    }

    [Fact]
    public async Task Installer_maintenant_est_refuse_pendant_une_vente_en_cours_de_saisie()
    {
        var (service, faux) = Creer(f => f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null)));
        await service.VerifierEtTelechargerAsync();
        bool confirmationDemandee = false;

        var refus = service.TenterInstallation(() => true, () => { confirmationDemandee = true; return true; });

        Assert.False(refus.Lancee);
        Assert.Equal(MiseAJourService.MessageVenteEnCours, refus.Message);
        Assert.Contains("vente est en cours", refus.Message);
        Assert.False(confirmationDemandee);                                // on ne demande même pas de confirmation
        Assert.Equal(0, faux.ApplicationsRedemarrage);                     // aucun redémarrage

        // sans vente : confirmation, puis application et redémarrage (annulable)
        Assert.False(service.TenterInstallation(() => false, () => false).Lancee);
        Assert.Equal(0, faux.ApplicationsRedemarrage);
        Assert.True(service.TenterInstallation(() => false, () => true).Lancee);
        Assert.Equal(1, faux.ApplicationsRedemarrage);
    }

    [Fact]
    public async Task Installer_maintenant_est_refuse_avec_une_vraie_fenetre_de_vente_contenant_des_produits()
    {
        var (service, faux) = Creer(f => f.Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null)));
        await service.VerifierEtTelechargerAsync();
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        using (var ctx = new AppDbContext()) { ctx.Users.Add(admin); ctx.SaveChanges(); }
        SessionUtilisateur.Courant = admin;
        int pid = TestDb.NouveauProduit("Doliprane", 50, 1, "Boîte", 1000m);

        bool refuse = false, ouverte = false, apresFermeture = false;
        UiHelper.EnSta(() =>
        {
            using var vente = new FormVente();
            var lignes = (List<LigneVente>)vente.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(vente)!;
            vente.Show();
            Assert.False(VerrouillageAuto.VenteEnCours());                  // fenêtre ouverte mais panier vide : pas de vente en cours
            lignes.Add(new LigneVente { ProduitId = pid, Quantite = 2, PrixUnitaire = 1000m, UniteVendue = "Boîte", Produit = new Produit { Id = pid, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Boîte" } });
            ouverte = VerrouillageAuto.VenteEnCours();
            var r = service.TenterInstallation(VerrouillageAuto.VenteEnCours, () => true);
            refuse = !r.Lancee && r.Message == MiseAJourService.MessageVenteEnCours;
            vente.Close();
            Application.DoEvents();
            apresFermeture = !VerrouillageAuto.VenteEnCours();
        });
        Assert.True(ouverte);
        Assert.True(refuse);
        Assert.True(apresFermeture);
        Assert.Equal(0, faux.ApplicationsRedemarrage);
    }

    // ───────────── canal, annonce, rapport, écran ─────────────

    [Fact]
    public async Task Le_canal_est_un_reglage_stable_par_defaut_et_change_de_gestionnaire()
    {
        var canauxDemandes = new List<string>();
        var faux = new FauxGestionnaire { Verifier = () => Task.FromResult<InfoMiseAJour?>(new InfoMiseAJour("1.0.1", null)) };
        var service = new MiseAJourService(c => { canauxDemandes.Add(c); return faux; });

        Assert.Equal("stable", service.Canal);
        await service.VerifierEtTelechargerAsync();
        Assert.Equal("1.0.1", service.VersionPrete);

        service.ChangerCanal("test");
        Assert.Equal("test", service.Canal);
        Assert.Equal("test", ParametresApp.Actuels.CanalMiseAJour);
        Assert.Null(service.VersionPrete);                                  // une mise à jour d'un autre canal n'est plus valable
        await service.VerifierEtTelechargerAsync();
        Assert.Equal(new[] { "stable", "test" }, canauxDemandes.Distinct().ToArray());

        service.ChangerCanal("n'importe quoi");
        Assert.Equal("stable", service.Canal);                              // valeur inconnue : stable
    }

    [Fact]
    public void Au_premier_demarrage_apres_une_mise_a_jour_l_application_l_annonce_une_seule_fois()
    {
        var (service, faux) = Creer(f => f.Actuelle = "1.0.0");
        service.DetecterMiseAJourInstallee();
        Assert.Null(service.PrendreAnnonce());                              // toute première ouverture : pas d'annonce
        Assert.Equal("1.0.0", ParametresApp.Actuels.DerniereVersionLancee);

        service.DetecterMiseAJourInstallee();
        Assert.Null(service.PrendreAnnonce());                              // même version : rien

        faux.Actuelle = "1.0.1";                                            // l'application vient d'être mise à jour
        service.DetecterMiseAJourInstallee();
        Assert.Equal("Application mise à jour en version 1.0.1", service.PrendreAnnonce());
        Assert.Null(service.PrendreAnnonce());                              // une seule fois
        Assert.Equal("1.0.1", ParametresApp.Actuels.DerniereVersionLancee);
    }

    [Fact]
    public void La_sauvegarde_avant_migration_est_bien_faite_quand_la_nouvelle_version_contient_des_migrations()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("ArrondiRepriseVentes"));    // base d'une version précédente : la migration FicheAVerifier est en attente
        using (var c = TestDb.Ouvrir())
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "Produit avant mise à jour", ["NbUniteParBoite"] = 1L });
        var avant = Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-migration_*.sqlite").ToHashSet();

        SauvegardeAutomatique.Executer();

        var nouvelles = Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-migration_*.sqlite").Where(f => !avant.Contains(f)).ToList();
        Assert.Single(nouvelles);
        using (var c = new SqliteConnection($"Data Source={nouvelles[0]};Mode=ReadOnly;Pooling=False"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM produits WHERE Nom = 'Produit avant mise à jour'";
            Assert.Equal(1L, Convert.ToInt64(cmd.ExecuteScalar()));        // la copie contient les données d'avant
            cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('produits') WHERE name = 'FicheAVerifier'";
            Assert.Equal(0L, Convert.ToInt64(cmd.ExecuteScalar()));        // et date bien d'AVANT la migration
        }

        // base à jour : pas de nouvelle sauvegarde « avant-migration »
        TestDb.MigrerTout();
        var apresMigration = Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-migration_*.sqlite").ToHashSet();
        SauvegardeAutomatique.Executer();
        Assert.Equal(apresMigration, Directory.GetFiles(CheminsApp.DossierSauvegardes, "PharmacieDB_avant-migration_*.sqlite").ToHashSet());
    }

    [Fact]
    public void La_version_figure_dans_le_rapport_du_developpeur()
    {
        string infos = RapportDeveloppeurService.Informations(DateTime.Now);
        Assert.Contains("Version de l'application : " + AppInfo.Version, infos);
        Assert.Contains("Installée par Velopack", infos);
        Assert.Contains(MiseAJourService.DossierInstallation, infos);
        Assert.DoesNotContain("Zéphyrine", infos);
    }

    [Fact]
    public void Administration_affiche_la_version_et_reserve_le_choix_du_canal_a_l_administrateur()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = "x" };
        var caissier = new User { Nom = "Ali", Prenom = "H", Role = Roles.Caissier, Login = "caisse", MotDePasse = "x" };
        MiseAJourService.Instance = new MiseAJourService(_ => new FauxGestionnaire { Actuelle = "1.0.0" });

        UiHelper.EnSta(() =>
        {
            SessionUtilisateur.Courant = admin;
            using (var uc = new Uc_Administration())
            {
                Assert.Contains("Version installée : " + AppInfo.Version, UiHelper.Champ<Label>(uc, "lblVersion").Text);
                Assert.Contains("canal : Stable", UiHelper.Champ<Label>(uc, "lblVersion").Text);
                Assert.True(UiHelper.Champ<ComboBox>(uc, "cbCanal").Enabled);
                UiHelper.Champ<ComboBox>(uc, "cbCanal").SelectedIndex = 1;           // « Test »
                Assert.Equal("test", ParametresApp.Actuels.CanalMiseAJour);
                Assert.Contains("canal : Test", UiHelper.Champ<Label>(uc, "lblVersion").Text);
            }
            SessionUtilisateur.Courant = caissier;
            using (var uc = new Uc_Administration())
                Assert.False(UiHelper.Champ<ComboBox>(uc, "cbCanal").Enabled);        // réservé à l'Administrateur
        });
    }

    [Fact]
    public void Le_bandeau_annonce_la_version_prete_et_le_bouton_est_reserve_a_l_administrateur()
    {
        UiHelper.EnSta(() =>
        {
            var bandeauAdmin = new BandeauMiseAJour(peutInstaller: true);
            bandeauAdmin.Afficher("1.2.3");
            Assert.Equal("Une mise à jour (version 1.2.3) est prête. Elle sera installée à la prochaine ouverture de l'application.",
                UiHelper.Champ<Label>(bandeauAdmin, "_texte").Text.Replace('\u00A0', ' '));
            Assert.True(UiHelper.Champ<Button>(bandeauAdmin, "_bouton").Visible);
            Assert.Equal("Installer maintenant", UiHelper.Champ<Button>(bandeauAdmin, "_bouton").Text);

            var bandeauCaissier = new BandeauMiseAJour(peutInstaller: false);
            Assert.False(UiHelper.Champ<Button>(bandeauCaissier, "_bouton").Visible);
        });
    }
}
