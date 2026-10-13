using System.Globalization;
using Pharmacie2.Services;
using Xunit;

/// <summary>Migration ArrondiRepriseVentes : le paiement repris vaut exactement la différence, à 2 décimales au plus.</summary>
public class ArrondiRepriseVentesTests
{
    [Fact]
    public void Vente_a_1666_67_partiellement_payee_donne_un_paiement_repris_exact_a_2_decimales()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutAttenduCloture"));
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "ventes", new()
            {
                ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["Type"] = "Comptant", ["MoyenPaiement"] = "Comptant",
                ["MontantTotal"] = "1666.67", ["UserId"] = 1L, ["DateVente"] = "2026-03-01 10:00:00"
            });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-1", ["VenteId"] = 1L, ["Montant"] = "1080.00", ["UserId"] = 1L });
            // Une vente de démonstration supplémentaire, payée en totalité : aucune reprise
            TestDb.Inserer(c, "ventes", new()
            {
                ["IdVente"] = 2L, ["numeroVente"] = "V-000002", ["Statut"] = "Active", ["Type"] = "Comptant", ["MoyenPaiement"] = "Comptant",
                ["MontantTotal"] = "500", ["UserId"] = 1L, ["DateVente"] = "2026-03-02 10:00:00"
            });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 2L, ["NumeroPaiement"] = "P-2", ["VenteId"] = 2L, ["Montant"] = "500", ["UserId"] = 1L });
        }

        TestDb.MigrerTout();

        string texte = TestDb.Texte("SELECT Montant FROM paiement WHERE NumeroPaiement='P-REPRISE-1'");
        decimal repris = decimal.Parse(texte, CultureInfo.InvariantCulture);
        Assert.Equal(586.67m, repris);                                       // 1 666,67 − 1 080, exactement
        Assert.True(Math.Round(repris, 2) == repris);                        // 2 décimales au plus
        Assert.True(texte.Length <= "586.67".Length, "valeur brute : " + texte);   // plus de 586.6700000000001
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM paiement WHERE NumeroPaiement='P-REPRISE-2'"));
        // La vente est soldée
        Assert.Equal(1666.67m, 1080m + repris);
    }

    [Fact]
    public void L_arrondi_ne_touche_que_les_paiements_de_reprise()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutAttenduCloture"));
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["Type"] = "Crédit", ["MontantTotal"] = "1000", ["UserId"] = 1L });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-1", ["VenteId"] = 1L, ["Montant"] = "333.3333", ["UserId"] = 1L });
        }
        TestDb.MigrerTout();
        Assert.Equal("333.3333", TestDb.Texte("SELECT Montant FROM paiement WHERE NumeroPaiement='P-1'"));   // intact
    }
}
