using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class StockServiceTests
{
    private static Produit P(int stock) => new Produit { Nom = "Doliprane", NbUniteParBoite = 5, UniteVente = "Plaquette", QuantiteEnStock = stock, SeuilAlerte = 2 };

    [Fact] public void Deux_boites_refusees_avec_7_unites() => Assert.False(StockService.EstDisponible(P(7), StockService.EnUnites(P(7), 2, "Boîte")));
    [Fact] public void Une_boite_acceptee_avec_7_unites() => Assert.True(StockService.EstDisponible(P(7), StockService.EnUnites(P(7), 1, "Boîte")));
    [Fact] public void Double_ajout_refuse() => Assert.False(StockService.EstDisponible(P(7), 5 + 5));
    [Fact] public void Boite_plus_deux_plaquettes_acceptes() => Assert.True(StockService.EstDisponible(P(7), 5 + 2));
    [Fact] public void Formater_9_unites() => Assert.Equal("1 boîte + 4 plaquettes", StockService.Formater(P(9)));
    [Fact] public void Seuil_en_unites() => Assert.Equal(10, StockService.SeuilEnUnites(P(7)));

    [Fact]
    public void Retirer_leve_une_exception_sans_ecreter()
    {
        var p = P(7);
        Assert.Throws<StockInsuffisantException>(() => StockService.Retirer(p, 8));
        Assert.Equal(7, p.QuantiteEnStock);
    }

    [Fact]
    public void StockInsuffisantException_est_une_InvalidOperationException_mais_pas_l_inverse()
    {
        Assert.True(typeof(InvalidOperationException).IsAssignableFrom(typeof(StockInsuffisantException)));
        // Une InvalidOperationException quelconque (ex. EF) ne doit pas être prise pour un « stock insuffisant »
        Exception autre = new InvalidOperationException("erreur EF");
        Assert.False(autre is StockInsuffisantException);
    }
}

public class VenteTransactionTests
{
    [Fact]
    public void Exception_avant_commit_n_enregistre_rien()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 7, 5);

        try
        {
            using var ctx = new AppDbContext();
            using var tx = ctx.Database.BeginTransaction();
            StockService.Retirer(ctx.produits.Find(pid), 5);
            var v = new Vente { numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MotifAnnulation = "" };
            ctx.ventes.Add(v);
            ctx.SaveChanges();
            ctx.LigneVentes.Add(new LigneVente { VenteId = v.IdVente, ProduitId = pid, Quantite = 1, QuantiteUnites = 5 });
            ctx.SaveChanges();
            throw new Exception("erreur simulée avant Commit");
        }
        catch (Exception ex) when (ex.Message.StartsWith("erreur simulée")) { }

        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM ventes"));
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM LigneVentes"));
        Assert.Equal(7, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
    }

    [Fact]
    public void Survente_refusee_a_la_validation()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 7, 5);

        Assert.Throws<StockInsuffisantException>(() =>
        {
            using var ctx = new AppDbContext();
            using var tx = ctx.Database.BeginTransaction();
            StockService.Retirer(ctx.produits.Find(pid), 10);
            tx.Commit();
        });
        Assert.Equal(7, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
    }

    [Fact]
    public void Annulation_restitue_QuantiteUnites_meme_si_NbUniteParBoite_change()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 7, 5);
        int venteId;
        using (var ctx = new AppDbContext())
        {
            var v = new Vente { numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MotifAnnulation = "" };
            v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 2, QuantiteUnites = 10, UniteVendue = "Boîte" });
            v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 3, QuantiteUnites = 3, UniteVendue = "Plaquette" });
            ctx.ventes.Add(v);
            ctx.SaveChanges();
            venteId = v.IdVente;
            ctx.produits.Find(pid).NbUniteParBoite = 4;   // la boîte passe de 5 à 4
            ctx.SaveChanges();
        }
        using (var ctx = new AppDbContext())
        using (var tx = ctx.Database.BeginTransaction())
        {
            foreach (var l in ctx.LigneVentes.Where(l => l.VenteId == venteId).ToList())
                StockService.Ajouter(ctx.produits.Find(l.ProduitId), l.QuantiteUnites);
            ctx.SaveChanges();
            tx.Commit();
        }
        Assert.Equal(7 + 13, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
    }
}
