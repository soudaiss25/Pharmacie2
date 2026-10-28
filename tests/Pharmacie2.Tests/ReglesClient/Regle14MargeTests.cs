using System.Drawing;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>Règle 14 du client : le prix de vente n'est jamais recalculé automatiquement ; la marge s'affiche automatiquement.</summary>
public class Regle14MargeTests
{
    private static string Normal(string s) => s.Replace(' ', ' ');

    private static int Produit(decimal achat, decimal vente, int nb = 1, decimal? unite = null, string nom = "Doliprane")
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        using var ctx = new AppDbContext();
        var p = new Produit
        {
            Nom = nom, Type = "Autre", PrixAchat = achat, PrixVente = vente, PrixVenteUnite = nb > 1 ? unite : null, NbUniteParBoite = nb, UniteVente = nb > 1 ? "Plaquette" : "Boîte",
            QuantiteEnStock = 10, SeuilAlerte = 1, DateExpiration = new DateTime(2029, 1, 1), Indication = "", Posologie = ""
        };
        ctx.produits.Add(p);
        ctx.SaveChanges();
        return p.Id;
    }

    // ───────── service ─────────

    [Fact]
    public void Regle14_la_marge_se_deduit_des_deux_prix_sans_jamais_les_modifier()
    {
        Assert.Equal(500m, MargeService.Kmf(1000m, 1500m));
        Assert.Equal(50m, MargeService.Pourcent(1000m, 1500m));
        Assert.Equal(-33.3m, MargeService.Pourcent(1500m, 1000m));
        Assert.Null(MargeService.Pourcent(0m, 1000m));                       // prix d'achat non renseigné : pas de pourcentage
        Assert.Equal(233.33m, MargeService.Enregistree(300m, 1000m));
        Assert.Equal(0m, MargeService.Enregistree(0m, 1000m));
        Assert.Equal(150m, MargeService.KmfUnite(1000m, 4, 400m));            // 400 − 1 000 ÷ 4
        Assert.Equal("500 KMF (50 % du prix d'achat)", Normal(MargeService.Texte(1000m, 1500m)));
        Assert.Equal("1 000 KMF (prix d'achat non renseigné)", Normal(MargeService.Texte(0m, 1000m)));
    }

    // ───────── fiche produit ─────────

    [Fact]
    public void Regle14_changer_le_prix_d_achat_ne_modifie_jamais_le_prix_de_vente()
    {
        int id = Produit(100m, 150m);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(id);
                Assert.Equal(150m, UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value);
                UiHelper.Champ<NumericUpDown>(f, "numPrixAchat").Value = 120m;
                Assert.Equal(150m, UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value);   // PAS recalculé (l'ancienne fiche l'écrasait)
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using var ctx = new AppDbContext();
        var p = ctx.produits.Single();
        Assert.Equal(120m, p.PrixAchat);
        Assert.Equal(150m, p.PrixVente);                                      // inchangé
        Assert.Equal(25m, p.MargeBeneficiaire);                               // (150 − 120) ÷ 120 × 100, enregistrée
    }

    [Fact]
    public void Regle14_il_n_y_a_plus_de_saisie_de_marge_et_un_nouveau_produit_n_a_aucun_prix_calcule()
    {
        Produit(100m, 150m);
        UiHelper.EnSta(() =>
        {
            using var f = new FormAddProduit();
            Assert.Null(f.GetType().GetField("numMarge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            UiHelper.Champ<NumericUpDown>(f, "numPrixAchat").Value = 800m;
            Assert.Equal(0m, UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value);       // le prix de vente reste celui que le client tape
        });
    }

    [Fact]
    public void Regle14_la_marge_s_affiche_en_direct_en_vert_ou_en_rouge_et_a_l_unite()
    {
        int id = Produit(1000m, 1500m);
        UiHelper.EnSta(() =>
        {
            using var f = new FormAddProduit(id);
            var etiquette = UiHelper.Champ<Label>(f, "lblMarge");
            Assert.Equal("Marge : 500 KMF (50 % du prix d'achat)", Normal(etiquette.Text));
            Assert.Equal(Theme.SuccesTexte, etiquette.ForeColor);

            UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 800m;                     // vente sous le prix d'achat
            Assert.StartsWith("Marge : ", Normal(etiquette.Text));
            Assert.Contains("-200 KMF", Normal(etiquette.Text));
            Assert.Equal(Theme.UrgentTexte, etiquette.ForeColor);

            // produit au détail : marge d'une unité = prix à l'unité − prix d'achat de la boîte ÷ nombre d'unités
            UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 1500m;
            UiHelper.Champ<CheckBox>(f, "chkVenteDetail").Checked = true;
            UiHelper.Champ<ComboBox>(f, "cmbUniteVente").SelectedItem = "Plaquette";
            UiHelper.Champ<NumericUpDown>(f, "numNbUniteParBoite").Value = 4;
            UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 400m;
            Assert.Contains("Par plaquette : 150 KMF (60 % du prix d'achat)", Normal(etiquette.Text));   // 400 − 250 ; 150 ÷ 250
            Assert.Equal(Theme.SuccesTexte, etiquette.ForeColor);

            UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 200m;                 // unité vendue sous son coût (250)
            Assert.Equal(Theme.UrgentTexte, etiquette.ForeColor);
        });
    }

    // ───────── Catalogue, exports, statistiques ─────────

    [Fact]
    public void Regle14_le_catalogue_et_l_export_montrent_la_marge_en_KMF_et_en_pourcent()
    {
        Produit(1000m, 1500m);
        UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var uc = new Uc_Produits { Dock = DockStyle.Fill };
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();
            var g = UiHelper.Champ<DataGridView>(uc, "dgvProduits");
            Assert.Equal("500 KMF (50 % du prix d'achat)", Normal(Convert.ToString(g.Rows[0].Cells["Marge"].Value)!));
            uc.Dispose();
        });

        string dossier = Path.Combine(Path.GetTempPath(), "p2_marge_" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(dossier);
            SauvegardeService.ExporterTout(dossier);
            string[] lignes = File.ReadAllLines(Directory.GetFiles(dossier, "produits.csv", SearchOption.AllDirectories).Single());
            string[] entete = lignes[0].Split(';'), valeurs = lignes[1].Split(';');
            Assert.Contains("MargeKMF", entete);
            Assert.Contains("MargeBeneficiairePourcent", entete);
            Assert.Equal("500", valeurs[Array.IndexOf(entete, "MargeKMF")].Split(',', '.')[0]);
        }
        finally { try { Directory.Delete(dossier, true); } catch { } }
    }

    [Fact]
    public void Regle14_chaque_pourcentage_precise_sa_base()
    {
        UiHelper.EnSta(() =>
        {
            using var caisse = new Uc_Caisse();
            Assert.Equal("Part de l'argent reçu (%)", UiHelper.Champ<DataGridView>(caisse, "dgvVentilation").Columns["PctCA"]!.HeaderText);
            using var depenses = new Uc_Depenses();
            Assert.Equal("Marge (% du chiffre d'affaires)", UiHelper.Champ<DataGridView>(depenses, "dgvAnnuel").Columns["Marge"]!.HeaderText);
        });
    }

    // ───────── migration ─────────

    [Fact]
    public void Regle14_la_migration_recalcule_la_marge_sur_une_base_remplie_sans_modifier_aucun_prix()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutPeremptionConnue"));
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "A", ["PrixAchat"] = "100", ["PrixVente"] = "150", ["MargeBeneficiaire"] = "0", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "B", ["PrixAchat"] = "0", ["PrixVente"] = "500", ["MargeBeneficiaire"] = "7", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 3L, ["Nom"] = "C", ["PrixAchat"] = "300", ["PrixVente"] = "1000", ["MargeBeneficiaire"] = "0", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 4L, ["Nom"] = "D", ["PrixAchat"] = "400", ["PrixVente"] = "300", ["MargeBeneficiaire"] = "0", ["NbUniteParBoite"] = 1L });
        }
        string autres = "group_concat(Id || '|' || Nom || '|' || PrixAchat || '|' || PrixVente || '|' || IFNULL(PrixVenteUnite,'') || '|' || QuantiteEnStock, '#')";
        string avant = TestDb.Texte($"SELECT {autres} FROM produits");
        Dictionary<string, long> comptes;
        using (var c0 = TestDb.Ouvrir()) comptes = TestDb.Comptes(c0, TestDb.TablesMetier);

        TestDb.MigrerJusqua(TestDb.MigrationId("RecalculMargeBeneficiaire"));

        Assert.Equal(avant, TestDb.Texte($"SELECT {autres} FROM produits"));                    // AUCUN prix (ni autre colonne) modifié
        Assert.Equal(50m, Convert.ToDecimal(TestDb.Texte("SELECT MargeBeneficiaire FROM produits WHERE Id=1"), System.Globalization.CultureInfo.InvariantCulture));
        Assert.Equal(0m, Convert.ToDecimal(TestDb.Texte("SELECT MargeBeneficiaire FROM produits WHERE Id=2"), System.Globalization.CultureInfo.InvariantCulture));      // prix d'achat nul
        Assert.Equal(233.33m, Convert.ToDecimal(TestDb.Texte("SELECT MargeBeneficiaire FROM produits WHERE Id=3"), System.Globalization.CultureInfo.InvariantCulture));
        Assert.Equal(-25m, Convert.ToDecimal(TestDb.Texte("SELECT MargeBeneficiaire FROM produits WHERE Id=4"), System.Globalization.CultureInfo.InvariantCulture));       // vendu sous le prix d'achat : marge négative
        using var c2 = TestDb.Ouvrir();
        Assert.All(comptes, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
    }
}
