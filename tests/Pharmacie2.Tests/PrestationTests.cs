using System.Reflection;
using System.Windows.Forms;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>Prestations (articles sans stock) : vendues comme un produit, sans aucun mouvement de stock, absentes du Stock, des alertes, des commandes et de l'inventaire.</summary>
public class PrestationTests
{
    private sealed class Ids { public int Admin, Medicament, Prestation; }

    private static Ids Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ids = new Ids();
        ids.Medicament = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 1000m);
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        ctx.Users.Add(admin);
        var injection = new Produit
        {
            Nom = "Injection", Type = "Autre", SansStock = true, PrixAchat = 0, PrixVente = 500, QuantiteEnStock = 0, SeuilAlerte = 0, NbUniteParBoite = 1, UniteVente = "Boîte",
            DateExpiration = new DateTime(2099, 12, 31), PeremptionConnue = true, Indication = "", Posologie = ""
        };
        ctx.produits.Add(injection);
        ctx.SaveChanges();
        ctx.SessionsCaisse.Add(new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" });
        ctx.SaveChanges();
        ids.Admin = admin.Id; ids.Prestation = injection.Id;
        SessionUtilisateur.Courant = admin;
        UiHelper.ViderTitres();
        return ids;
    }

    private static int VenteMixte(Ids ids)
    {
        var cts = UiHelper.FermerLesMessages(false);   // « Non » à l'impression de la facture
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormVente();
                var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
                lignes.Add(new LigneVente { ProduitId = ids.Medicament, Quantite = 3, PrixUnitaire = 1000m, UniteVendue = "Boîte", Produit = new Produit { Id = ids.Medicament, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Boîte" } });
                lignes.Add(new LigneVente { ProduitId = ids.Prestation, Quantite = 2, PrixUnitaire = 500m, UniteVendue = "Boîte", Produit = new Produit { Id = ids.Prestation, Nom = "Injection", NbUniteParBoite = 1, UniteVente = "Boîte", SansStock = true } });
                UiHelper.Appeler(f, "RafraichirGrille");
                UiHelper.Appeler(f, "RecalculerTotal");
                UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = ModesPaiement.Comptant;
                UiHelper.Champ<NumericUpDown>(f, "numEspeces").Value = 4000;
                UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        using var ctx = new AppDbContext();
        return ctx.ventes.Single().IdVente;
    }

    /// <summary>Exécute une partie d'interface en répondant « Oui » à toute fenêtre de message (aucun test ne doit rester bloqué).</summary>
    private static void AvecMessages(Action a)
    {
        var cts = UiHelper.FermerLesMessages(true);
        try { UiHelper.EnSta(a); }
        finally { cts.Cancel(); }
    }

    private static int Stock(int id) { using var ctx = new AppDbContext(); return ctx.produits.Single(p => p.Id == id).QuantiteEnStock; }

    // ───────── vente et annulation ─────────

    [Fact]
    public void Vente_mixte_medicament_et_prestation_ne_touche_pas_au_stock_de_la_prestation()
    {
        var ids = Preparer();
        int venteId = VenteMixte(ids);

        Assert.Equal(97, Stock(ids.Medicament));          // le médicament est décompté
        Assert.Equal(0, Stock(ids.Prestation));           // la prestation, sans stock, n'est jamais décomptée ni refusée
        using var ctx = new AppDbContext();
        var v = ctx.ventes.Include(x => x.Lignes).Single(x => x.IdVente == venteId);
        Assert.Equal(4000m, v.MontantTotal);              // 3 × 1 000 + 2 × 500
        Assert.Equal(2, v.Lignes.Count);
        var ligne = v.Lignes.Single(l => l.ProduitId == ids.Prestation);
        Assert.Equal((2, 500m), (ligne.Quantite, ligne.PrixUnitaire));
        Assert.Equal(0m, ligne.CoutAchatUnitaire);        // coût 0 : connu, la ligne compte dans la marge
        Assert.False(UiHelper.TitreVu("Stock insuffisant"));
    }

    [Fact]
    public void Annuler_une_vente_avec_prestation_restitue_le_stock_du_medicament_seulement()
    {
        var ids = Preparer();
        int venteId = VenteMixte(ids);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            AvecMessages(() =>
            {
                using var f = new FormAnnulationVente(venteId);
                UiHelper.Champ<TextBox>(f, "txtMotif").Text = "Erreur";
                UiHelper.Appeler(f, "btnConfirmer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        Assert.Equal(100, Stock(ids.Medicament));         // restitué
        Assert.Equal(0, Stock(ids.Prestation));           // aucun mouvement, aucun stock négatif
        using var ctx = new AppDbContext();
        Assert.Equal("Annulée", ctx.ventes.Single().Statut);
    }

    [Fact]
    public void Le_service_de_stock_ignore_les_prestations()
    {
        var p = new Produit { SansStock = true, QuantiteEnStock = 0, Nom = "Pansement" };
        Assert.True(StockService.EstDisponible(p, 50));
        StockService.Retirer(p, 50);
        StockService.Ajouter(p, 10);
        Assert.Equal(0, p.QuantiteEnStock);
        var normal = new Produit { SansStock = false, QuantiteEnStock = 0, Nom = "Doliprane" };
        Assert.False(StockService.EstDisponible(normal, 1));
        Assert.Throws<StockInsuffisantException>(() => StockService.Retirer(normal, 1));
    }

    // ───────── fiche produit ─────────

    [Fact]
    public void La_case_Prestation_masque_stock_conditionnement_peremption_fournisseur_et_prix_d_achat()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            AvecMessages(() =>
            {
                using var f = new FormAddProduit();
                f.Show();
                Application.DoEvents();
                var coche = UiHelper.Champ<CheckBox>(f, "chkPrestation");
                Assert.False(coche.Checked);
                Assert.True(UiHelper.Champ<NumericUpDown>(f, "numPrixAchat").Visible);
                coche.Checked = true;
                foreach (var nom in new[] { "numPrixAchat", "numQuantite", "numSeuil", "dtpDateExpiration", "chkVenteDetail" })
                    Assert.False(UiHelper.Champ<Control>(f, nom).Visible, nom + " doit être masqué");
                Assert.False(UiHelper.Champ<ComboBox>(f, "cbFournisseur").Visible);
                Assert.True(UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Visible);   // le prix de vente reste saisi

                UiHelper.Champ<TextBox>(f, "txtNom").Text = "Mesure de la tension";
                UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 1500;
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using var ctx = new AppDbContext();
        var p = ctx.produits.Single();
        Assert.True(p.SansStock);
        Assert.Equal((0m, 1500m, 0, 1), (p.PrixAchat, p.PrixVente, p.QuantiteEnStock, p.NbUniteParBoite));
        Assert.Null(p.FournisseurId);
        Assert.Null(p.PrixVenteUnite);
        Assert.True(p.PeremptionConnue);
    }

    [Fact]
    public void Une_prestation_sans_prix_de_vente_est_refusee_et_la_nature_d_un_article_existant_ne_change_pas()
    {
        var ids = Preparer();
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            AvecMessages(() =>
            {
                using var f = new FormAddProduit();
                UiHelper.Champ<CheckBox>(f, "chkPrestation").Checked = true;
                UiHelper.Champ<TextBox>(f, "txtNom").Text = "Sans prix";
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
            AvecMessages(() =>
            {
                using var f = new FormAddProduit(ids.Medicament);
                var coche = UiHelper.Champ<CheckBox>(f, "chkPrestation");
                Assert.False(coche.Checked);
                Assert.False(coche.Enabled);                  // pas de conversion d'un médicament en prestation
                using var g = new FormAddProduit(ids.Prestation);
                Assert.True(UiHelper.Champ<CheckBox>(g, "chkPrestation").Checked);
            });
        }
        finally { cts.Cancel(); }
        using var ctx = new AppDbContext();
        Assert.DoesNotContain(ctx.produits, p => p.Nom == "Sans prix");
    }

    // ───────── exclusions ─────────

    [Fact]
    public void Une_prestation_n_apparait_ni_dans_le_stock_ni_dans_les_alertes_ni_dans_l_inventaire_ni_dans_le_choix_pour_commande()
    {
        var ids = Preparer();

        var alertes = TableauDeBordService.AFaireMaintenant(DateTime.Now);
        Assert.DoesNotContain(alertes, a => a.Texte.Contains("Injection"));                 // pas en rupture malgré un stock à 0
        Assert.DoesNotContain(alertes, a => a.Type == TypeAFaire.Ruptures);

        AvecMessages(() =>
        {
            using var hote = new Form();
            var stock = new Uc_Stock { Dock = DockStyle.Fill };
            hote.Controls.Add(stock);
            hote.Show();
            Application.DoEvents();
            var g = UiHelper.Champ<DataGridView>(stock, "dgvStock");
            Assert.Equal(new[] { "Doliprane" }, g.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["Produit"].Value)).ToArray());
            stock.Filtrer("En rupture");
            Assert.Equal(0, g.Rows.Count);
            stock.Dispose();

            var catalogue = new Uc_Produits { Dock = DockStyle.Fill };
            hote.Controls.Clear();
            hote.Controls.Add(catalogue);
            Application.DoEvents();
            var gp = UiHelper.Champ<DataGridView>(catalogue, "dgvProduits");
            var ligne = gp.Rows.Cast<DataGridViewRow>().Single(r => Convert.ToString(r.Cells["Nom"].Value) == "Injection");
            Assert.Equal("Prestation", Convert.ToString(ligne.Cells["Etat"].Value));          // visible dans le Catalogue avec son badge
            catalogue.Dispose();

            using var vente = new FormChoixProduit(pourCommande: false);
            Assert.Equal(2, UiHelper.Champ<DataGridView>(vente, "dgvProduits").Rows.Count);   // vendable
            using var commande = new FormChoixProduit(pourCommande: true);
            var noms = UiHelper.Champ<DataGridView>(commande, "dgvProduits").Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["Nom"].Value)).ToArray();
            Assert.Equal(new[] { "Doliprane" }, noms);                                        // jamais commandée
        });

        string chemin = Path.Combine(Path.GetTempPath(), $"inventaire_{Guid.NewGuid():N}.xlsx");
        InventaireExportService.Exporter(chemin);
        using (var wb = new XLWorkbook(chemin))
        {
            var textes = wb.Worksheets.SelectMany(w => w.CellsUsed()).Select(c => c.GetString()).ToList();
            Assert.Contains("Doliprane", textes);
            Assert.DoesNotContain("Injection", textes);
        }
        File.Delete(chemin);
    }

    [Fact]
    public void Le_diagnostic_ignore_les_prestations()
    {
        var ids = Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.produits.Single(p => p.Id == ids.Prestation).QuantiteEnStock = -3;   // même un stock absurde n'est pas signalé
            ctx.SaveChanges();
        }
        using var ctx2 = new AppDbContext();
        var r = DiagnosticBaseService.Executer(ctx2, true, DateTime.Now);
        Assert.DoesNotContain(r.Constats, c => c.Objet == "Injection");
        Assert.Equal(-3, Stock(ids.Prestation));
    }

    // ───────── statistiques ─────────

    [Fact]
    public void Les_statistiques_donnent_une_ligne_Prestations_distincte_avec_leur_chiffre_d_affaires_et_leur_nombre()
    {
        var ids = Preparer();
        VenteMixte(ids);
        var s = StatistiquesService.Calculer(DateTime.Today, DateTime.Now.AddMinutes(5));
        Assert.Equal(4000m, s.CA);                                   // le chiffre d'affaires total contient tout
        Assert.NotNull(s.Prestations);
        Assert.Equal((1000m, 2), (s.Prestations!.CA, s.Prestations.Nombre));
        Assert.Equal(new[] { "Doliprane" }, s.TopProduits.Select(t => t.Produit).ToArray());   // les médicaments seuls dans le classement

        AvecMessages(() =>
        {
            using var hote = new Form();
            var uc = new Uc_Statistique { Dock = DockStyle.Fill };
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();
            string alertes = (string)typeof(Uc_Statistique).GetField("_alertesTexte", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(uc)!;
            Assert.Contains("Prestations : 2 prestations", alertes.Replace(' ', ' '));
            uc.Dispose();
        });
    }

    // ───────── reprise ─────────

    [Fact]
    public void Migration_AjoutTypeArticle_laisse_tous_les_produits_existants_avec_leur_stock()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutRemboursementCommande"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "A", ["PrixVente"] = "200", ["NbUniteParBoite"] = 1L, ["QuantiteEnStock"] = 12L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "B", ["PrixVente"] = "300", ["NbUniteParBoite"] = 1L, ["QuantiteEnStock"] = 0L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutTypeArticle"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM produits WHERE SansStock = 0"));
        Assert.Equal(12, TestDb.Scalaire(c2, "SELECT QuantiteEnStock FROM produits WHERE Id = 1"));
    }
}
