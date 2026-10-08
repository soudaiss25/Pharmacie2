using SqliteConnectionAlias = Microsoft.Data.Sqlite.SqliteConnection;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>Migration DetectionStocksAVerifier, jouée sur une base remplie avec la chaîne de migrations réelle.</summary>
public class VerificationStockTests
{
    private const string MigrationAvant = "20261008182428_AjoutQuantiteRecueLigneCommande";

    private static void Produit(SqliteConnectionAlias c, long id, string nom, long nb, long stock)
        => TestDb.Inserer(c, "produits", new() { ["Id"] = id, ["Nom"] = nom, ["NbUniteParBoite"] = nb, ["QuantiteEnStock"] = stock });

    private static void Commande(SqliteConnectionAlias c, long cmdId, string statut, long produitId, long boites, long ligneId)
    {
        TestDb.Inserer(c, "commandes", new() { ["Id"] = cmdId, ["FournisseurId"] = 1L, ["Statut"] = statut });
        TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = ligneId, ["CommandeId"] = cmdId, ["ProduitId"] = produitId, ["Quantite"] = boites });
    }

    private static void Fournisseur(SqliteConnectionAlias c)
        => TestDb.Inserer(c, "fournisseur", new() { ["Id"] = 1L, ["Nom"] = "Four", ["Contact"] = "" });

    [Fact]
    public void Aucune_commande_recue_aucun_produit_marque()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            Fournisseur(c);
            Produit(c, 1, "Doliprane", 5, 20);
            Commande(c, 1, "En attente", 1, 10, 1);
            Commande(c, 2, "Annulée", 1, 10, 2);
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE StockAVerifier = 1"));
        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));
        Assert.Equal(20, TestDb.Scalaire(c2, "SELECT QuantiteEnStock FROM produits WHERE Id=1"));
    }

    [Fact]
    public void Detection_selon_le_statut_des_commandes_et_comptes_identiques()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            Fournisseur(c);
            Produit(c, 1, "P5 recu", 5, 10);          // 10 boîtes 'Reçu'  -> marqué, 40
            Produit(c, 2, "P1 recu", 1, 10);          // NbUniteParBoite = 1 -> non marqué
            Produit(c, 3, "P5 partiel", 5, 12);       // 'Reçu partiellement' -> marqué sans estimation
            Produit(c, 4, "P5 sans commande", 5, 30); // jamais commandé -> non marqué
            Produit(c, 5, "P5 en attente", 5, 7);     // 'En attente' -> non marqué
            Commande(c, 1, "Reçu", 1, 10, 1);
            Commande(c, 2, "Reçu", 2, 10, 2);
            Commande(c, 3, "Reçu partiellement", 3, 6, 3);
            Commande(c, 4, "En attente", 5, 4, 4);
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));

        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=1"));
        Assert.Equal(40, TestDb.Scalaire(c2, "SELECT UnitesManquantesEstimees FROM produits WHERE Id=1"));
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=2"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=3"));
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT UnitesManquantesEstimees FROM produits WHERE Id=3"));
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=4"));
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=5"));

        // Le stock n'est jamais modifié par la migration
        Assert.Equal(10, TestDb.Scalaire(c2, "SELECT QuantiteEnStock FROM produits WHERE Id=1"));

        using var cmd = c2.CreateCommand();
        cmd.CommandText = "SELECT MotifVerification FROM produits WHERE Id=1";
        Assert.Equal("10 boîte(s) reçue(s) avant la correction : seulement 10 unité(s) ajoutée(s) au lieu de 50.", (string)cmd.ExecuteScalar());
        cmd.CommandText = "SELECT MotifVerification FROM produits WHERE Id=3";
        Assert.Contains("quantité reçue inconnue", (string)cmd.ExecuteScalar());
    }

    [Fact]
    public void Appliquer_la_correction_ajoute_40_unites_et_leve_le_marquage()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);
        using (var c = TestDb.Ouvrir())
        {
            Fournisseur(c);
            Produit(c, 1, "P5 recu", 5, 10);
            Commande(c, 1, "Reçu", 1, 10, 1);
        }
        TestDb.MigrerTout();
        Assert.Equal(1, VerificationStockService.NombreAVerifier());

        VerificationStockService.Resoudre(1, ChoixVerification.CorrectionAppliquee);

        Assert.Equal(50, TestDb.Scalaire("SELECT QuantiteEnStock FROM produits WHERE Id=1"));
        Assert.Equal(0, TestDb.Scalaire("SELECT StockAVerifier FROM produits WHERE Id=1"));
        Assert.Equal(0, TestDb.Scalaire("SELECT UnitesManquantesEstimees FROM produits WHERE Id=1"));
        Assert.Equal(0, VerificationStockService.NombreAVerifier());
    }

    [Fact]
    public void Stock_correct_leve_le_marquage_sans_toucher_au_stock()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);
        using (var c = TestDb.Ouvrir())
        {
            Fournisseur(c);
            Produit(c, 1, "P5 recu", 5, 10);
            Commande(c, 1, "Reçu", 1, 10, 1);
        }
        TestDb.MigrerTout();

        VerificationStockService.Resoudre(1, ChoixVerification.StockCorrect);

        Assert.Equal(10, TestDb.Scalaire("SELECT QuantiteEnStock FROM produits WHERE Id=1"));
        Assert.Equal(0, TestDb.Scalaire("SELECT StockAVerifier FROM produits WHERE Id=1"));
    }

    [Fact]
    public void Receptions_faites_avec_la_nouvelle_version_ne_sont_jamais_marquees()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Amox", 0, 5, "Comprimé");
        using (var ctx = new AppDbContext())
        {
            ctx.fournisseur.Add(new Fournisseur { Nom = "Four", Contact = "" });
            ctx.SaveChanges();
            var cmd = new Commande { FournisseurId = ctx.fournisseur.First().Id, Lignes = { new LigneCommande { ProduitId = pid, Quantite = 10 } } };
            ctx.commandes.Add(cmd);
            ctx.SaveChanges();
            CommandeService.Receptionner(cmd.Id);
        }
        Assert.Equal(50, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
        Assert.Equal(0, VerificationStockService.NombreAVerifier());
    }
}
