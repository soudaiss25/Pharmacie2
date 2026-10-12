using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>Migration AjoutModePaiementPaiement : le mode des paiements existants est repris, aucune ligne n'est perdue.</summary>
public class ModePaiementMigrationTests
{
    [Fact]
    public void AjoutModePaiement_reprend_le_mode_de_chaque_paiement_sur_une_base_remplie()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("CorrectionLibelleMvola"));

        string[] types = { "Comptant", "Chèque", "Carte bancaire", "Mvola", "Huri Money", "Crédit", "Mutuelle" };
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            for (int i = 0; i < types.Length; i++)
            {
                long id = i + 1;
                TestDb.Inserer(c, "ventes", new() { ["IdVente"] = id, ["numeroVente"] = "V-00000" + id, ["Statut"] = "Active", ["Type"] = types[i], ["MoyenPaiement"] = types[i], ["MontantTotal"] = "1000", ["UserId"] = 1L });
                TestDb.Inserer(c, "paiement", new() { ["Id"] = id, ["NumeroPaiement"] = "P-" + id, ["VenteId"] = id, ["Montant"] = "600", ["UserId"] = 1L });
            }
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 20L, ["NumeroPaiement"] = "P-20", ["VenteId"] = 6L, ["Montant"] = "100", ["UserId"] = 1L });   // 2e versement d'un crédit
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));   // aucune ligne ajoutée ni perdue (R5 ne touche ici que des ventes soldées ou à crédit)

        string Mode(long venteId) => TestDb.Texte($"SELECT Mode FROM paiement WHERE VenteId={venteId} ORDER BY Id LIMIT 1");
        Assert.Equal("Espèces", Mode(1));       // Comptant
        Assert.Equal("Chèque", Mode(2));
        Assert.Equal("Carte bancaire", Mode(3));
        Assert.Equal("Mvola", Mode(4));
        Assert.Equal("Huri Money", Mode(5));
        Assert.Equal("Espèces", Mode(6));       // Crédit
        Assert.Equal("Espèces", Mode(7));       // Mutuelle
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM paiement WHERE Mode IS NULL OR Mode = ''"));
    }
}
