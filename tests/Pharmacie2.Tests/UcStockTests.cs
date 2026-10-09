using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.UserControls;
using Xunit;

public class UcStockTests
{
    private static void Preparer()
    {
        LayoutSeed.Preparer();   // 1 produit à vérifier (Doliprane) + 1 périmé sans stock (Sirop périmé)
        using var ctx = new AppDbContext();
        Produit P(string nom, int stock, DateTime exp, bool actif = true) => new Produit
        {
            Nom = nom, Type = "Autre", QuantiteEnStock = stock, NbUniteParBoite = 1, UniteVente = "Boîte", SeuilAlerte = 1,
            DateExpiration = exp, Actif = actif
        };
        ctx.produits.AddRange(
            P("Zéro périmé", 5, DateTime.Today.AddDays(-3)),
            P("Zéro bientôt", 5, DateTime.Today.AddDays(12)),
            P("Zéro normal", 50, DateTime.Today.AddYears(1)),
            P("Zéro archivé", 50, DateTime.Today.AddYears(1), actif: false));
        ctx.SaveChanges();
    }

    private static List<string> Noms(Uc_Stock uc)
        => UiHelper.Champ<DataGridView>(uc, "dgvStock").Rows.Cast<DataGridViewRow>()
            .Select(r => r.Cells["Produit"].Value?.ToString() ?? "").ToList();

    [Fact]
    public void Filtres_du_stock_peremption_rupture_a_verifier_et_archives()
    {
        Preparer();
        UiHelper.EnSta(() =>
        {
            using var uc = new Uc_Stock();

            var tous = Noms(uc);
            Assert.Equal("Doliprane 500 mg", tous[0]);        // « À vérifier » en premier
            Assert.DoesNotContain("Zéro archivé", tous);     // archivé caché

            uc.Filtrer("Périmés");
            Assert.Equal(new[] { "Zéro périmé" }, Noms(uc));  // seulement s'il en reste en stock

            uc.Filtrer("Péremption proche");
            Assert.Equal(new[] { "Zéro bientôt" }, Noms(uc));

            uc.Filtrer("À vérifier");
            Assert.Equal(new[] { "Doliprane 500 mg" }, Noms(uc));

            uc.Filtrer("Tous");
            uc.Filtrer("En rupture");
            Assert.Equal(new[] { "Sirop périmé" }, Noms(uc));

            // état affiché
            uc.Filtrer("Tous");
            uc.Filtrer("Tous");
        });
    }

    [Fact]
    public void Taper_un_nom_dans_la_recherche_filtre_la_grille()
    {
        Preparer();
        UiHelper.EnSta(() =>
        {
            using var uc = new Uc_Stock();
            var champ = UiHelper.Champ<TextBox>(uc, "txtSearchProduit");
            Assert.True(Noms(uc).Count > 3);

            champ.Text = "bientôt";
            Assert.Equal(new[] { "Zéro bientôt" }, Noms(uc));

            champ.Text = "dolip";   // insensible à la casse
            Assert.Equal(new[] { "Doliprane 500 mg" }, Noms(uc));

            champ.Text = "";
            Assert.True(Noms(uc).Count > 3);
        });
    }

    [Fact]
    public void Etat_et_stock_lisible_dans_la_grille()
    {
        Preparer();
        UiHelper.EnSta(() =>
        {
            using var uc = new Uc_Stock();
            var grille = UiHelper.Champ<DataGridView>(uc, "dgvStock");
            DataGridViewRow Ligne(string nom) => grille.Rows.Cast<DataGridViewRow>().First(r => (string)r.Cells["Produit"].Value == nom);

            Assert.Equal("Périmé", Ligne("Zéro périmé").Cells["Etat"].Value);
            Assert.Equal("Rupture", Ligne("Sirop périmé").Cells["Etat"].Value);
            Assert.Equal("OK", Ligne("Zéro normal").Cells["Etat"].Value);
            Assert.Equal("1 boîte(s) + 4 plaquette(s)", Ligne("Doliprane 500 mg").Cells["Quantite"].Value);
            Assert.Equal("À vérifier", Ligne("Doliprane 500 mg").Cells["Verification"].Value);
        });
    }
}
