using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;
using Xunit;

public class PageAccueilTests
{
    [Fact]
    public void Ma_journee_s_ouvre_par_defaut_avec_des_chiffres_et_une_liste_a_faire()
    {
        var ids = LayoutSeed.Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(SessionUtilisateur.Courant);
                var panel = UiHelper.Champ<Panel>(page, "panelContent");
                var uc = Assert.IsType<Uc_MaJournee>(Assert.Single(panel.Controls.Cast<Control>()));

                Assert.True(UiHelper.Champ<BoutonMenu>(page, "btnMaJournee").Actif);
                Assert.Contains("KMF", UiHelper.Champ<CarteKpi>(uc, "carteEncaisse").Valeur);
                Assert.Contains("KMF", UiHelper.Champ<CarteKpi>(uc, "carteARecuperer").Valeur);
                Assert.Contains("Bonjour", UiHelper.Champ<Label>(uc, "lblBonjour").Text);
                // jeu de données : rupture (sirop), stock à vérifier (Doliprane), mutuelle en retard
                Assert.True(UiHelper.Champ<ListeActions>(uc, "listeActions").NombreDeLignes >= 3);
                Assert.Equal(7, UiHelper.Champ<GraphiqueBarres>(uc, "graphique").NombreDeBarres);
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Un_clic_dans_la_liste_ouvre_l_ecran_concerne_deja_filtre_et_surligne_le_menu()
    {
        LayoutSeed.Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var page = new PageAccueil(SessionUtilisateur.Courant);
                UiHelper.Appeler(page, "SurDemandeDepuisMaJournee", TypeAFaire.StocksAVerifier);

                var panel = UiHelper.Champ<Panel>(page, "panelContent");
                var stock = Assert.IsType<Uc_Stock>(Assert.Single(panel.Controls.Cast<Control>()));
                Assert.Equal("À vérifier", UiHelper.Champ<ComboBox>(stock, "cbSeuil").Text);

                Assert.True(UiHelper.Champ<BoutonMenu>(page, "btn_stock").Actif);
                Assert.False(UiHelper.Champ<BoutonMenu>(page, "btnMaJournee").Actif);   // un seul bouton surligné
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Deconnexion_ferme_la_page_et_signale_la_deconnexion()
    {
        LayoutSeed.Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                var page = new PageAccueil(SessionUtilisateur.Courant);
                page.Show();
                bool ferme = false;
                page.FormClosed += (s, e) => ferme = true;

                UiHelper.Appeler(page, "btnDeconnexion_Click", null, EventArgs.Empty);

                Assert.True(ferme);
                Assert.True(page.DeconnexionDemandee);
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Le_bandeau_s_affiche_puis_se_masque()
    {
        UiHelper.EnSta(() =>
        {
            using var b = new BandeauNotification();
            Assert.False(b.Visible);
            b.Afficher("Vente enregistrée");
            Assert.True(b.Visible);
            b.Masquer();
            Assert.False(b.Visible);
        });
    }
}
