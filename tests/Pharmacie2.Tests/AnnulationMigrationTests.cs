using Pharmacie2.Models;
using Xunit;

/// <summary>Migration CorrectionChampsAnnulation (reconstruction de la table ventes).</summary>
public class AnnulationMigrationTests
{
    private const string MigrationAvant = "20261008202255_AjoutCleSecours";

    [Fact]
    public void Champs_d_annulation_corriges_sans_perdre_ventes_paiements_ni_lignes()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);

        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "mutuels", new() { ["IdMutuel"] = 1L, ["NomEmployeur"] = "M" });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "Doliprane" });
            // vente active créée par l'ancien code : DateAnnulation = date de vente, MotifAnnulation = "null"
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["DateAnnulation"] = "2026-01-01 10:00:00", ["MotifAnnulation"] = "null", ["MutuelId"] = 1L });
            // vente réellement annulée : on garde sa date et son motif
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 2L, ["numeroVente"] = "V-000002", ["Statut"] = "Annulée", ["DateAnnulation"] = "2026-02-01 08:30:00", ["MotifAnnulation"] = "Erreur de saisie" });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 3L, ["numeroVente"] = "V-000003", ["Statut"] = "Active", ["DateAnnulation"] = "2026-03-01 09:00:00", ["MotifAnnulation"] = "" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 1L, ["VenteId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 2L, ["QuantiteUnites"] = 10L, ["PrixUnitaire"] = "500" });
            TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 2L, ["VenteId"] = 2L, ["ProduitId"] = 1L, ["Quantite"] = 1L, ["QuantiteUnites"] = 5L, ["PrixUnitaire"] = "500" });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-1", ["VenteId"] = 1L, ["Montant"] = "300" });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 2L, ["NumeroPaiement"] = "P-2", ["VenteId"] = 1L, ["Montant"] = "200" });
            TestDb.Inserer(c, "MutuelPaiements", new() { ["Id"] = 1L, ["MutuelId"] = 1L, ["VenteId"] = 1L, ["NumeroPaiement"] = "MP-1", ["Montant"] = "100" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));
        Assert.Equal(3, apres["ventes"]);
        Assert.Equal(2, apres["paiement"]);
        Assert.Equal(2, apres["LigneVentes"]);
        Assert.Equal(1, apres["MutuelPaiements"]);

        // Vente active : plus de date ni de motif d'annulation
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE IdVente=1 AND DateAnnulation IS NULL AND MotifAnnulation IS NULL"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE IdVente=3 AND DateAnnulation IS NULL AND MotifAnnulation IS NULL"));
        // Vente annulée : conservée
        using var cmd = c2.CreateCommand();
        cmd.CommandText = "SELECT MotifAnnulation || ' / ' || DateAnnulation FROM ventes WHERE IdVente=2";
        Assert.Equal("Erreur de saisie / 2026-02-01 08:30:00", (string)cmd.ExecuteScalar());

        // La colonne accepte maintenant NULL ; une vente créée par le nouveau code n'a ni motif ni date
        using var ctx = new AppDbContext();
        var v = new Vente
        {
            numeroVente = "V-000004", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "",
            MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant"
        };
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        Assert.Equal(1, TestDb.Scalaire($"SELECT COUNT(*) FROM ventes WHERE IdVente={v.IdVente} AND DateAnnulation IS NULL AND MotifAnnulation IS NULL"));
    }
}
