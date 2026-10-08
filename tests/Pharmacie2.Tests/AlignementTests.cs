using Xunit;

/// <summary>
/// Ne passe que si la proposition propositions/AlignementDepensesAnnexes est copiée dans Migrations/
/// (job CI « alignement »). Exclu du job principal.
/// </summary>
public class AlignementTests
{
    [Fact, Trait("Category", "Alignement")]
    public void AlignementDepensesAnnexes_conserve_les_lignes_et_passe_la_cle_en_SET_NULL()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua("20261008182428_AjoutQuantiteRecueLigneCommande");

        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "a", ["MotDePasse"] = "x" });
            for (long i = 1; i <= 5; i++)
                TestDb.Inserer(c, "DepensesAnnexes", new() { ["Id"] = i, ["Categorie"] = "Loyer", ["Description"] = "d" + i, ["Montant"] = "1000", ["UserId"] = 1L });
            TestDb.Inserer(c, "DepensesAnnexes", new() { ["Id"] = 6L, ["Categorie"] = "Autre", ["Description"] = "sans user", ["Montant"] = "50" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));
        Assert.Equal(1000, TestDb.Scalaire(c2, "SELECT CAST(Montant AS INTEGER) FROM DepensesAnnexes WHERE Id=3"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT UserId FROM DepensesAnnexes WHERE Id=3"));

        using var cmd = c2.CreateCommand();
        cmd.CommandText = "SELECT on_delete FROM pragma_foreign_key_list('DepensesAnnexes') WHERE \"table\"='Users'";
        Assert.Equal("SET NULL", (string)cmd.ExecuteScalar());
    }
}
