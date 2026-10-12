using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>Un test par règle du client (voir REGLES_METIER.md à la racine du dépôt).</summary>
public class ReglesClientTests
{
    [Fact]
    public void Regle05_le_stock_est_toujours_en_unites_de_base()
    {
        var p = new Produit { Nom = "Doliprane", NbUniteParBoite = 5, UniteVente = "Plaquette", QuantiteEnStock = 10 };
        Assert.Equal(3, StockService.EnUnites(p, 3, "Plaquette"));
        Assert.Equal(5, StockService.EnUnites(p, 1, "Boîte"));
    }

    [Fact]
    public void Regle11_cheque_carte_mvola_huri_money_sont_payes_en_totalite()
    {
        Assert.Equal(
            new[] { ModesPaiement.Cheque, ModesPaiement.CarteBancaire, ModesPaiement.Mvola, ModesPaiement.HuriMoney }.OrderBy(x => x),
            ModesPaiement.ReglesEnTotalite.OrderBy(x => x));
        Assert.DoesNotContain(ModesPaiement.Credit, ModesPaiement.ReglesEnTotalite);
        Assert.DoesNotContain(ModesPaiement.Mutuelle, ModesPaiement.ReglesEnTotalite);
    }

    [Theory]
    [InlineData(1500.4, "1 500 KMF")]
    [InlineData(1500.5, "1 501 KMF")]
    [InlineData(0, "0 KMF")]
    public void Regle12_les_montants_n_ont_jamais_de_centimes(double valeur, string attendu)
    {
        string s = Format.Montant((decimal)valeur);
        Assert.Equal(attendu, s);
        Assert.DoesNotContain(",", s);
        Assert.DoesNotContain(".", s);
    }

    [Fact]
    public void Regle13_le_verrouillage_automatique_est_de_5_minutes_par_defaut()
        => Assert.Equal(5, new ParametresApp().DelaiVerrouillageMinutes);
}
