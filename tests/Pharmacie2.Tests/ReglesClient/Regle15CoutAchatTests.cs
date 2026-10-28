using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Règle 15 du client : la marge d'une vente utilise le prix d'achat du jour de la vente.</summary>
public class Regle15CoutAchatTests
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    private static (int p1, int p2) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        using var ctx = new AppDbContext();
        ctx.Users.Add(admin);
        var p1 = new Produit { Nom = "Doliprane", Type = "Autre", PrixAchat = 800, PrixVente = 1500, PrixVenteUnite = 300, NbUniteParBoite = 5, UniteVente = "Plaquette", QuantiteEnStock = 100, SeuilAlerte = 1, DateExpiration = new DateTime(2029, 1, 1), Indication = "", Posologie = "" };
        var p2 = new Produit { Nom = "Sirop", Type = "Autre", PrixAchat = 300, PrixVente = 500, NbUniteParBoite = 1, UniteVente = "Boîte", QuantiteEnStock = 100, SeuilAlerte = 1, DateExpiration = new DateTime(2029, 1, 1), Indication = "", Posologie = "" };
        ctx.produits.AddRange(p1, p2);
        ctx.SaveChanges();
        ctx.SessionsCaisse.Add(new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" });   // encaisser des espèces demande une caisse ouverte
        ctx.SaveChanges();
        SessionUtilisateur.Courant = admin;
        return (p1.Id, p2.Id);
    }

    private static void ChangerPrixAchat(int produitId, decimal nouveau)
    {
        using var ctx = new AppDbContext();
        ctx.produits.Single(p => p.Id == produitId).PrixAchat = nouveau;
        ctx.SaveChanges();
    }

    private static void Vendre(int produitId, int boites, int plaquettes)
    {
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormVente();
                var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
                void Ajouter(int q, string unite, decimal prix) => lignes.Add(new LigneVente
                {
                    ProduitId = produitId, Quantite = q, PrixUnitaire = prix, UniteVendue = unite,
                    Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 5, UniteVente = "Plaquette" }
                });
                if (boites > 0) Ajouter(boites, "Boîte", 1500m);
                if (plaquettes > 0) Ajouter(plaquettes, "Plaquette", 300m);
                UiHelper.Appeler(f, "RafraichirGrille");
                UiHelper.Appeler(f, "RecalculerTotal");
                UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = ModesPaiement.Comptant;
                UiHelper.Champ<NumericUpDown>(f, "numEspeces").Value = boites * 1500 + plaquettes * 300;
                UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
    }

    private static decimal Marge()
    {
        using var ctx = new AppDbContext();
        return TableauDeBordService.MargeBrute(ctx, DateTime.Today.AddDays(-1), DateTime.Now.AddDays(1));
    }

    // ───────── migration : reprise documentée ─────────

    [Fact]
    public void Regle15_la_migration_reprend_le_prix_d_achat_actuel_divise_par_le_conditionnement_et_laisse_NULL_un_produit_absent()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("RecalculMargeBeneficiaire"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            using (var pragma = c.CreateCommand()) { pragma.CommandText = "PRAGMA foreign_keys = OFF"; pragma.ExecuteNonQuery(); }   // une ligne de vente orpheline (produit absent) pour le cas NULL
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "A", ["PrixAchat"] = "800", ["PrixVente"] = "1500", ["NbUniteParBoite"] = 5L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "B", ["PrixAchat"] = "500", ["PrixVente"] = "900", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 3L, ["Nom"] = "C", ["PrixAchat"] = "300", ["PrixVente"] = "400", ["NbUniteParBoite"] = 0L });      // conditionnement invalide : max(1, …)
            TestDb.Inserer(c, "produits", new() { ["Id"] = 4L, ["Nom"] = "D", ["PrixAchat"] = "1000", ["PrixVente"] = "1500", ["NbUniteParBoite"] = 3L });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["MontantTotal"] = "1000" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 1L, ["VenteId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 2L, ["QuantiteUnites"] = 10L, ["PrixUnitaire"] = "1500", ["UniteVendue"] = "Boîte" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 2L, ["VenteId"] = 1L, ["ProduitId"] = 2L, ["Quantite"] = 1L, ["QuantiteUnites"] = 1L, ["PrixUnitaire"] = "900", ["UniteVendue"] = "Boîte" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 3L, ["VenteId"] = 1L, ["ProduitId"] = 3L, ["Quantite"] = 1L, ["QuantiteUnites"] = 1L, ["PrixUnitaire"] = "400", ["UniteVendue"] = "Boîte" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 4L, ["VenteId"] = 1L, ["ProduitId"] = 4L, ["Quantite"] = 3L, ["QuantiteUnites"] = 3L, ["PrixUnitaire"] = "500", ["UniteVendue"] = "Plaquette" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 5L, ["VenteId"] = 1L, ["ProduitId"] = 99L, ["Quantite"] = 1L, ["QuantiteUnites"] = 1L, ["PrixUnitaire"] = "100", ["UniteVendue"] = "Boîte" });   // produit absent
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutCoutAchatLigneVente"));

        decimal Cout(int id) => decimal.Parse(TestDb.Texte($"SELECT CoutAchatUnitaire FROM LigneVentes WHERE Id={id}"), Inv);
        Assert.Equal(160m, Cout(1));                                // 800 ÷ 5
        Assert.Equal(500m, Cout(2));
        Assert.Equal(300m, Cout(3));                                // max(1, 0)
        Assert.Equal(333.3333m, Cout(4));                           // 1 000 ÷ 3, arrondi à 4 décimales
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM LigneVentes WHERE Id=5 AND CoutAchatUnitaire IS NULL"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
    }

    // ───────── le coût est figé à chaque vente ─────────

    [Fact]
    public void Regle15_un_changement_du_prix_d_achat_ne_modifie_pas_la_marge_des_ventes_passees()
    {
        var (p1, _) = Preparer();
        Vendre(p1, boites: 2, plaquettes: 3);

        using (var ctx = new AppDbContext())
        {
            var lignes = ctx.LigneVentes.OrderBy(l => l.Id).ToList();
            Assert.Equal(2, lignes.Count);
            Assert.All(lignes, l => Assert.Equal(160m, l.CoutAchatUnitaire));           // 800 ÷ 5 au moment de la vente
        }
        Assert.Equal(3900m - 13 * 160m, Marge());                                        // 2 × 1 500 + 3 × 300 − 13 unités × 160

        ChangerPrixAchat(p1, 1000m);                                                    // le fournisseur augmente ses prix
        Assert.Equal(3900m - 13 * 160m, Marge());                                        // la marge passée n'a PAS bougé
        Assert.Equal(1820m, Marge());

        Vendre(p1, boites: 1, plaquettes: 0);                                           // une nouvelle vente utilise le nouveau coût
        using (var ctx = new AppDbContext())
            Assert.Equal(200m, ctx.LigneVentes.OrderByDescending(l => l.Id).First().CoutAchatUnitaire);   // 1 000 ÷ 5
        Assert.Equal(1820m + 1500m - 5 * 200m, Marge());
    }

    [Fact]
    public void Regle15_la_modification_d_une_vente_garde_le_cout_d_origine_des_lignes_existantes_et_prend_le_cout_du_jour_pour_une_nouvelle_ligne()
    {
        var (p1, p2) = Preparer();
        Vendre(p1, boites: 2, plaquettes: 0);
        int venteId;
        using (var ctx = new AppDbContext()) venteId = ctx.ventes.Single().IdVente;
        ChangerPrixAchat(p1, 1000m);                                                    // le coût change AVANT la modification
        ChangerPrixAchat(p2, 350m);

        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormModificationVente(venteId);
                var lignes = (IList)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
                lignes[0]!.GetType().GetProperty("Quantite")!.SetValue(lignes[0], 3);   // 2 → 3 boîtes (ligne existante)
                var nouvelle = Activator.CreateInstance(lignes[0]!.GetType())!;          // une nouvelle ligne : le sirop
                nouvelle.GetType().GetProperty("ProduitId")!.SetValue(nouvelle, p2);
                nouvelle.GetType().GetProperty("ProduitNom")!.SetValue(nouvelle, "Sirop");
                nouvelle.GetType().GetProperty("Quantite")!.SetValue(nouvelle, 1);
                nouvelle.GetType().GetProperty("PrixUnitaire")!.SetValue(nouvelle, 500m);
                nouvelle.GetType().GetProperty("UniteVendue")!.SetValue(nouvelle, "Boîte");
                lignes.Add(nouvelle);
                UiHelper.Appeler(f, "RafraichirListView");
                UiHelper.Appeler(f, "CalculerTotal");
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using var c2 = new AppDbContext();
        var apres = c2.LigneVentes.Where(l => l.VenteId == venteId).OrderBy(l => l.ProduitId).ToList();
        Assert.Equal(2, apres.Count);
        Assert.Equal((p1, 3, 160m), (apres[0].ProduitId, apres[0].Quantite, apres[0].CoutAchatUnitaire));   // coût d'origine conservé (pas 200)
        Assert.Equal((p2, 1, 350m), (apres[1].ProduitId, apres[1].Quantite, apres[1].CoutAchatUnitaire));   // nouvelle ligne : coût du jour
    }

    // ───────── lignes sans coût connu : exclues de la marge et signalées ─────────

    [Fact]
    public void Regle15_une_ligne_sans_cout_connu_est_exclue_de_la_marge_et_signalee_partout()
    {
        var (p1, _) = Preparer();
        using (var ctx = new AppDbContext())
        {
            var v = new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
                MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = 1500, UserId = SessionUtilisateur.IdCourant, DateVente = DateTime.Now
            };
            v.Lignes.Add(new LigneVente { ProduitId = p1, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 1000, CoutAchatUnitaire = 80m, UniteVendue = "Boîte" });   // CA 1 000, coût 400
            v.Lignes.Add(new LigneVente { ProduitId = p1, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 500, CoutAchatUnitaire = null, UniteVendue = "Boîte" });     // coût inconnu
            ctx.ventes.Add(v);
            ctx.SaveChanges();
        }

        using var c = new AppDbContext();
        var calcul = TableauDeBordService.CalculerMarge(c, DateTime.Today, DateTime.Now.AddDays(1));
        Assert.Equal(600m, calcul.MargeBrute);                                           // 1 000 − 400 : la ligne sans coût n'entre pas dans la marge
        Assert.Equal(1, calcul.LignesSansCout);
        Assert.Equal(500m, calcul.CaSansCout);
        Assert.Equal("1 ligne sans coût d'achat connu", calcul.Signalement);
        Assert.Equal("3 lignes sans coût d'achat connu", MargeVenteService.Signalement(3));
        Assert.Null(MargeVenteService.Signalement(0));

        var stats = StatistiquesService.Calculer(DateTime.Today, DateTime.Now.AddDays(1));
        Assert.Equal(1500m, stats.CA);                                                   // le chiffre d'affaires, lui, reste complet
        Assert.Equal(600m, stats.MargeBrute);
        Assert.Equal(1, stats.LignesSansCout);
        Assert.Equal(0m, TableauDeBordService.CoutAchat(new LigneVente { Quantite = 1, QuantiteUnites = 5 }));         // jamais d'exception
        Assert.Equal(400m, TableauDeBordService.CoutAchat(new LigneVente { Quantite = 1, QuantiteUnites = 5, CoutAchatUnitaire = 80m }));
    }
}
