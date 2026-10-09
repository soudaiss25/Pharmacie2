using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Pharmacie2.views;
using Xunit;

/// <summary>Le mode compact (largeur logique &lt; 1200) bascule sans recréer l'écran et revient exactement à la disposition normale.</summary>
public class ModeCompactTests
{
    private static string Arbre(Control racine)
    {
        var sb = new StringBuilder();
        void Vider(Control c, int niveau)
        {
            if (!string.IsNullOrEmpty(c.Name))
                sb.AppendLine($"{new string(' ', niveau)}{c.Name} [{c.GetType().Name}] {c.Bounds} {(c.Visible ? "visible" : "caché")}");
            foreach (Control k in c.Controls) Vider(k, niveau + 1);
        }
        Vider(racine, 0);
        return sb.ToString();
    }

    private static int Compter(Control racine)
    {
        int n = 1;
        foreach (Control k in racine.Controls) n += Compter(k);
        return n;
    }

    private static void Redimensionner(Hote h, Config cfg)
    {
        h.Fenetre.ClientSize = new Size(cfg.Phys(cfg.W), cfg.Phys(cfg.H));
        Application.DoEvents();
        h.Fenetre.PerformLayout();
        Application.DoEvents();
    }

    [Theory]
    [InlineData("Uc_MaJournee")]
    [InlineData("Uc_Stock")]
    [InlineData("Uc_Vente")]
    [InlineData("Uc_Produits")]
    [InlineData("Uc_Commande")]
    [InlineData("FormVente")]
    public void Aller_retour_1920_1024_1920_retrouve_la_disposition_normale(string nomEcran)
    {
        var type = LayoutTests.Decouvrir().Single(t => t.Name == nomEcran);
        var ids = LayoutSeed.PreparerDemo();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                var grand = EcranHote.Matrice[2];
                var petit = EcranHote.Matrice[3];
                using var h = EcranHote.Ouvrir(type, ids, grand);
                var page = h.Page;
                var ecran = h.Ecran;
                string avant = Arbre(h.Fenetre);
                int nbAvant = Compter(h.Fenetre);

                var recherche = EcranHote.Chercher(ecran, "txtSearchProduit") as TextBox
                                ?? EcranHote.Chercher(ecran, "txtSearchVente") as TextBox
                                ?? EcranHote.Chercher(ecran, "txtRecherche") as TextBox;
                if (recherche != null) recherche.Text = "a";   // saisie en cours à conserver

                Redimensionner(h, petit);
                if (page != null) Assert.True(page.EstCompact, "mode compact attendu à 1024 px");
                Assert.Same(ecran, h.Ecran);
                if (recherche != null) Assert.Equal("a", recherche.Text);

                Redimensionner(h, grand);
                if (page != null) Assert.False(page.EstCompact, "mode normal attendu à 1920 px");
                if (recherche != null) Assert.Equal("a", recherche.Text);

                string apres = Arbre(h.Fenetre);
                Assert.Equal(nbAvant, Compter(h.Fenetre));   // aucun contrôle perdu ni en double
                Assert.Equal(avant, apres);                   // disposition identique
            });
        }
        finally { cts.Cancel(); Pharmacie2.Services.Theme.EchelleTest = null; }
    }
}
