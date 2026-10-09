using Pharmacie2.Services;
using Xunit;

public class FormatTests
{
    [Theory]
    [InlineData(0, "0 KMF")]
    [InlineData(500, "500 KMF")]
    [InlineData(12500, "12 500 KMF")]
    [InlineData(1234567, "1 234 567 KMF")]
    [InlineData(-1200, "-1 200 KMF")]
    public void Montant_sans_decimales_avec_espace_pour_les_milliers(int valeur, string attendu)
        => Assert.Equal(attendu, Format.Montant((decimal)valeur).Replace(' ', ' '));

    [Fact]
    public void Montant_arrondit_et_gere_les_valeurs_vides()
    {
        Assert.Equal("12 500 KMF", Format.Montant(12500.4m).Replace(' ', ' '));
        Assert.Equal("1 000 KMF", Format.Montant(999.5m).Replace(' ', ' '));
        Assert.Equal("—", Format.Montant((decimal?)null).Replace(' ', ' '));
        Assert.Equal("12 500", Format.Nombre(12500m).Replace(' ', ' '));
    }

    [Fact]
    public void Espace_insecable_entre_un_nombre_et_son_unite()
    {
        Assert.Equal("Paracétamol 1 g", Format.Insecable("Paracétamol 1 g"));
        Assert.Equal("500 mg et 30 jours", Format.Insecable("500 mg et 30 jours"));
        Assert.Equal("2 400 KMF", Format.Insecable("2 400 KMF"));
        Assert.Equal("Il reste 3 boîtes ici", Format.Insecable("Il reste 3 boîtes ici").Replace(' ', ' '));
        Assert.Equal("1 g", Format.Montant(0).Length > 0 ? Format.Insecable("1 g") : "");   // jamais d'exception sur les montants
    }

    [Fact]
    public void Evolution_par_rapport_a_la_periode_precedente()
    {
        Assert.Equal("+25 %", Format.Evolution(125m, 100m));
        Assert.Equal("-50 %", Format.Evolution(50m, 100m));
        Assert.Equal("0 %", Format.Evolution(100m, 100m));
        Assert.Equal("nouveau", Format.Evolution(100m, 0m));
        Assert.Equal("—", Format.Evolution(0m, 0m));
        Assert.Equal(1, Format.Tendance(125m, 100m));
        Assert.Equal(-1, Format.Tendance(50m, 100m));
        Assert.Equal(0, Format.Tendance(100m, 100m));
    }
}

public class FondationsTests
{
    [Fact]
    public void Un_seul_nom_pour_la_pharmacie()
    {
        Assert.Equal("LIGUAPHARME", AppInfo.NomPharmacie);
        Assert.Contains("LIGUAPHARME", AppInfo.NomLogiciel);
        Assert.StartsWith("Stock", AppInfo.Titre("Stock"));
    }

    [Fact]
    public void Palette_et_polices_respectent_la_charte()
    {
        Assert.Equal(System.Drawing.ColorTranslator.FromHtml("#1B5E20"), Theme.Principal);
        Assert.Equal(System.Drawing.ColorTranslator.FromHtml("#C62828"), Theme.UrgentTexte);
        Assert.Equal(22f, Theme.Kpi.SizeInPoints);
        Assert.Equal(16f, Theme.TitrePage.SizeInPoints);
        Assert.True(Theme.Police(6f).SizeInPoints >= 9f);   // rien sous 9 pt
    }

    [Fact]
    public void Mvola_remplace_Mvolo_dans_les_modes_de_paiement()
    {
        Assert.Contains("Mvola", ModesPaiement.Tous);
        Assert.DoesNotContain("Mvolo", ModesPaiement.Tous);
        Assert.True(ModesPaiement.EstMobile("Mvola"));
        Assert.True(ModesPaiement.EstMobile("Huri Money"));
        Assert.False(ModesPaiement.EstMobile("Comptant"));
    }

    [Fact]
    public void Migration_renomme_Mvolo_en_Mvola_dans_les_ventes()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("NettoyageChampsAnnulation"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["MoyenPaiement"] = "Mvolo", ["Type"] = "Mvolo" });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 2L, ["numeroVente"] = "V-000002", ["MoyenPaiement"] = "Huri Money", ["Type"] = "Huri Money" });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 3L, ["numeroVente"] = "V-000003", ["MoyenPaiement"] = "Comptant", ["Type"] = "Comptant" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE MoyenPaiement='Mvola' AND Type='Mvola'"));
        Assert.Equal(0, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE MoyenPaiement='Mvolo' OR Type='Mvolo'"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE MoyenPaiement='Huri Money'"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM ventes WHERE MoyenPaiement='Comptant'"));
    }
}
