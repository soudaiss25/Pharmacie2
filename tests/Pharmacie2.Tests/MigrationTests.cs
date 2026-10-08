using Xunit;

public class MigrationTests
{
    [Fact]
    public void Migrations_reprennent_les_donnees_sans_perte()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationAvantQuantiteUnites);

        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "Doliprane", ["NbUniteParBoite"] = 5L, ["QuantiteEnStock"] = 7L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "Sirop", ["NbUniteParBoite"] = 1L, ["QuantiteEnStock"] = 10L });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 1L, ["VenteId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 2L, ["UniteVendue"] = "Boîte" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 2L, ["VenteId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 3L, ["UniteVendue"] = "Plaquette" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 3L, ["VenteId"] = 1L, ["ProduitId"] = 2L, ["Quantite"] = 4L, ["UniteVendue"] = "Boîte" });
            TestDb.Inserer(c, "fournisseur", new() { ["Id"] = 1L, ["Nom"] = "Four" });
            TestDb.Inserer(c, "commandes", new() { ["Id"] = 1L, ["FournisseurId"] = 1L, ["Statut"] = "Reçu" });
            TestDb.Inserer(c, "commandes", new() { ["Id"] = 2L, ["FournisseurId"] = 1L, ["Statut"] = "Reçu partiellement" });
            TestDb.Inserer(c, "commandes", new() { ["Id"] = 3L, ["FournisseurId"] = 1L, ["Statut"] = "En attente" });
            TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = 1L, ["CommandeId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 10L });
            TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = 2L, ["CommandeId"] = 2L, ["ProduitId"] = 1L, ["Quantite"] = 6L });
            TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = 3L, ["CommandeId"] = 3L, ["ProduitId"] = 1L, ["Quantite"] = 3L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));

        Assert.Equal(10, TestDb.Scalaire(c2, "SELECT QuantiteUnites FROM LigneVentes WHERE Id=1"));  // 2 boîtes de 5
        Assert.Equal(3, TestDb.Scalaire(c2, "SELECT QuantiteUnites FROM LigneVentes WHERE Id=2"));   // 3 plaquettes
        Assert.Equal(4, TestDb.Scalaire(c2, "SELECT QuantiteUnites FROM LigneVentes WHERE Id=3"));   // NbUniteParBoite = 1
        Assert.Equal(7, TestDb.Scalaire(c2, "SELECT QuantiteEnStock FROM produits WHERE Id=1"));

        Assert.Equal(10, TestDb.Scalaire(c2, "SELECT QuantiteRecue FROM LigneCommandes WHERE Id=1"));  // Reçu
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT QuantiteRecue FROM LigneCommandes WHERE Id=2"));   // Reçu partiellement
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT QuantiteRecue FROM LigneCommandes WHERE Id=3"));   // En attente
    }
}
