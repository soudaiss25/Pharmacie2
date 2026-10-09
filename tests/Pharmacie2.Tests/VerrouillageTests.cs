using System.Reflection;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.Composants;
using Xunit;

public class VerrouillageTests
{
    private static readonly DateTime Debut = new(2026, 10, 9, 9, 0, 0);

    [Fact]
    public void Verrouillage_apres_le_delai_d_inactivite_avec_une_horloge_simulee()
    {
        var maintenant = Debut;
        var suivi = new SuiviInactivite { Maintenant = () => maintenant };
        suivi.Activite();

        maintenant = Debut.AddMinutes(4).AddSeconds(59);
        Assert.False(suivi.DoitVerrouiller(5));
        maintenant = Debut.AddMinutes(5);
        Assert.True(suivi.DoitVerrouiller(5));

        suivi.Activite();                                    // une frappe remet le compteur à zéro
        Assert.False(suivi.DoitVerrouiller(5));
        maintenant = maintenant.AddHours(3);
        Assert.False(suivi.DoitVerrouiller(0));              // délai désactivé : jamais
        Assert.True(suivi.DoitVerrouiller(60));
    }

    [Fact]
    public void Cinq_mots_de_passe_faux_imposent_30_secondes_entre_les_essais_et_sont_journalises()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var maintenant = Debut;
        var limite = new LimiteEssais { Maintenant = () => maintenant };

        for (int i = 0; i < 4; i++)
        {
            Assert.True(limite.PeutEssayer(out _));
            limite.EnregistrerEchec("caisse");
        }
        Assert.True(limite.PeutEssayer(out _));              // le cinquième essai reste possible
        limite.EnregistrerEchec("caisse");
        Assert.False(limite.PeutEssayer(out int reste));     // puis 30 secondes d'attente
        Assert.Equal(30, reste);

        maintenant = Debut.AddSeconds(29);
        Assert.False(limite.PeutEssayer(out _));
        maintenant = Debut.AddSeconds(30);
        Assert.True(limite.PeutEssayer(out _));

        limite.EnregistrerReussite();
        Assert.Equal(0, limite.Echecs);

        string journal = string.Concat(Directory.GetFiles(CheminsApp.DossierLogs).Select(File.ReadAllText));
        Assert.Contains("5 mots de passe erronés", journal);
    }

    private static (User admin, User caissier) Preparer()
    {
        var ids = LayoutSeed.Preparer();
        ParametresApp.ReinitialiserPourTests();
        using var ctx = new AppDbContext();
        return (ctx.Users.First(u => u.Id == ids.Admin), ctx.Users.First(u => u.Id == ids.Caissier));
    }

    [Fact]
    public void Ecran_de_verrouillage_refuse_un_mauvais_mot_de_passe_puis_deverrouille_avec_le_bon()
    {
        var (admin, _) = Preparer();
        UiHelper.EnSta(() =>
        {
            using var f = new FormVerrouillage(admin);
            var maintenant = Debut;
            f.Limite.Maintenant = () => maintenant;

            Assert.False(f.Essayer("pas le bon"));
            Assert.Equal("Mot de passe incorrect.", UiHelper.Champ<Label>(f, "lblMessage").Text);
            Assert.Equal(FormVerrouillage.Issue.Aucune, f.Resultat);

            // cinq faux : le bon mot de passe lui-même est refusé pendant 30 secondes
            for (int i = 0; i < 4; i++) f.Essayer("faux " + i);
            Assert.False(f.Essayer("1234"));
            Assert.Contains("Patientez", UiHelper.Champ<Label>(f, "lblMessage").Text);
            maintenant = Debut.AddSeconds(31);

            Assert.True(f.Essayer("1234"));
            Assert.Equal(FormVerrouillage.Issue.Deverrouille, f.Resultat);
        });
    }

    private static FormVente OuvrirVenteAvecPanier(Form proprietaire, int produitId)
    {
        var vente = new FormVente();
        var lignes = (List<LigneVente>)vente.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(vente)!;
        lignes.Add(new LigneVente { ProduitId = produitId, Quantite = 2, PrixUnitaire = 1500m, UniteVendue = "Boîte", Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Boîte" } });
        UiHelper.Appeler(vente, "RafraichirGrille");
        UiHelper.Appeler(vente, "RecalculerTotal");
        vente.StartPosition = FormStartPosition.Manual;
        vente.Show(proprietaire);
        return vente;
    }

    [Fact]
    public void Verrouillage_automatique_conserve_la_vente_en_cours_sous_l_ecran()
    {
        var (admin, _) = Preparer();
        int produit = (int)TestDb.Scalaire("SELECT Id FROM produits LIMIT 1");
        ParametresApp.Modifier(p => p.DelaiVerrouillageMinutes = 5);
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(admin);
                page.Show();
                var verrou = VerrouillageAuto.Courant!;
                verrou.Modal = false;
                var maintenant = Debut;
                verrou.Suivi.Maintenant = () => maintenant;
                verrou.Suivi.Activite();

                using var vente = OuvrirVenteAvecPanier(page, produit);
                Assert.True(vente.AVenteEnCours);
                var lignes = (List<LigneVente>)vente.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(vente)!;

                maintenant = Debut.AddMinutes(4);
                verrou.Verifier();
                Assert.False(verrou.EstVerrouille);          // pas encore
                maintenant = Debut.AddMinutes(5);
                verrou.Verifier();
                Assert.True(verrou.EstVerrouille);           // délai atteint : verrouillé

                var ecran = verrou.Verrouiller(false)!;      // déjà verrouillé : même écran
                Assert.False(ecran.Essayer("mauvais"));
                Assert.True(verrou.EstVerrouille);
                Assert.True(ecran.Essayer("1234"));
                verrou.Terminer();

                Assert.False(verrou.EstVerrouille);
                Assert.True(vente.AVenteEnCours);            // la vente est intacte
                Assert.Single(lignes);
                Assert.Equal(2, lignes[0].Quantite);
                Assert.False(page.DeconnexionDemandee);
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Delai_a_zero_ne_verrouille_jamais_et_Ctrl_L_verrouille_volontairement()
    {
        var (admin, _) = Preparer();
        ParametresApp.Modifier(p => p.DelaiVerrouillageMinutes = 0);
        UiHelper.EnSta(() =>
        {
            using var page = new PageAccueil(admin);
            page.Show();
            var verrou = VerrouillageAuto.Courant!;
            verrou.Modal = false;
            var maintenant = Debut;
            verrou.Suivi.Maintenant = () => maintenant;
            verrou.Suivi.Activite();
            maintenant = Debut.AddDays(2);
            verrou.Verifier();
            Assert.False(verrou.EstVerrouille);

            // Ctrl+L sur la page
            UiHelper.Appeler(page, "btnVerrouiller_Click", null!, EventArgs.Empty);   // même action que le bouton du menu et que Ctrl+L
            Assert.True(verrou.EstVerrouille);
            Assert.True(verrou.EcranActuel!.Essayer("1234"));
            verrou.Terminer();
            Assert.False(verrou.EstVerrouille);
        });
    }

    [Fact]
    public void Changer_d_utilisateur_avec_une_vente_en_cours_demande_confirmation_puis_abandonne_la_vente()
    {
        var (admin, _) = Preparer();
        int produit = (int)TestDb.Scalaire("SELECT Id FROM produits LIMIT 1");

        // confirmation refusée : l'écran reste verrouillé et la vente intacte
        var non = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(admin);
                page.Show();
                var verrou = VerrouillageAuto.Courant!;
                verrou.Modal = false;
                using var vente = OuvrirVenteAvecPanier(page, produit);
                var ecran = verrou.Verrouiller(false)!;
                UiHelper.Appeler(ecran, "btnChangerUtilisateur_Click", null!, EventArgs.Empty);
                Assert.Equal(FormVerrouillage.Issue.Aucune, ecran.Resultat);
                Assert.True(vente.AVenteEnCours);
                ecran.Essayer("1234");
                verrou.Terminer();
            });
        }
        finally { non.Cancel(); }

        // confirmation acceptée : la vente est abandonnée et l'utilisateur déconnecté
        var oui = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(admin);
                page.Show();
                var verrou = VerrouillageAuto.Courant!;
                verrou.Modal = false;
                var vente = OuvrirVenteAvecPanier(page, produit);
                var ecran = verrou.Verrouiller(false)!;
                UiHelper.Appeler(ecran, "btnChangerUtilisateur_Click", null!, EventArgs.Empty);
                Assert.Equal(FormVerrouillage.Issue.ChangerUtilisateur, ecran.Resultat);
                verrou.Terminer();

                Assert.False(vente.AVenteEnCours);
                Assert.True(vente.IsDisposed || !vente.Visible);
                Assert.True(page.DeconnexionDemandee);
            });
        }
        finally { oui.Cancel(); }
    }

    [Fact]
    public void Sans_vente_en_cours_changer_d_utilisateur_ne_demande_rien()
    {
        var (admin, _) = Preparer();
        var cts = UiHelper.FermerLesMessages(false);   // un éventuel message serait refusé : le test échouerait
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(admin);
                page.Show();
                var verrou = VerrouillageAuto.Courant!;
                verrou.Modal = false;
                var ecran = verrou.Verrouiller(false)!;
                UiHelper.Appeler(ecran, "btnChangerUtilisateur_Click", null!, EventArgs.Empty);
                Assert.Equal(FormVerrouillage.Issue.ChangerUtilisateur, ecran.Resultat);
                verrou.Terminer();
                Assert.True(page.DeconnexionDemandee);
            });
        }
        finally { cts.Cancel(); }
    }
}
