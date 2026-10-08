using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class CommandeServiceTests
{
    private static (int produit, int cmd, int ligne) Commande(int boites, int stock = 0, string statut = "En attente", int recue = 0)
    {
        int pid = TestDb.NouveauProduit("Amox", stock, 5, "Comprimé");
        using var ctx = new AppDbContext();
        if (!ctx.fournisseur.Any()) { ctx.fournisseur.Add(new Fournisseur { Nom = "Four" }); ctx.SaveChanges(); }
        var c = new Commande
        {
            FournisseurId = ctx.fournisseur.First().Id,
            Statut = statut,
            Lignes = { new LigneCommande { ProduitId = pid, Quantite = boites, QuantiteRecue = recue } }
        };
        ctx.commandes.Add(c);
        ctx.SaveChanges();
        return (pid, c.Id, c.Lignes[0].Id);
    }

    private static long Stock(int pid) => TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}");

    [Fact]
    public void Dix_boites_de_5_ajoutent_50_unites()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var (pid, cmd, _) = Commande(10);
        CommandeService.Receptionner(cmd);
        Assert.Equal(50, Stock(pid));
        Assert.Throws<InvalidOperationException>(() => CommandeService.Receptionner(cmd));   // pas de double réception
        Assert.Equal(50, Stock(pid));
    }

    [Fact]
    public void Partielle_4_puis_le_reste_donne_50_jamais_plus()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var (pid, cmd, ligne) = Commande(10);

        CommandeService.Receptionner(cmd, new Dictionary<int, int> { [ligne] = 4 });
        Assert.Equal(20, Stock(pid));
        using (var ctx = new AppDbContext()) Assert.Equal("Reçu partiellement", ctx.commandes.Find(cmd).Statut);

        CommandeService.Receptionner(cmd);   // le reste : 6 boîtes
        Assert.Equal(50, Stock(pid));
        using (var ctx = new AppDbContext()) Assert.Equal("Reçu", ctx.commandes.Find(cmd).Statut);

        Assert.Throws<InvalidOperationException>(() => CommandeService.Receptionner(cmd));
        Assert.Equal(50, Stock(pid));
    }

    [Fact]
    public void Quantite_demandee_plafonnee_au_reste()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var (pid, cmd, ligne) = Commande(10);
        CommandeService.Receptionner(cmd, new Dictionary<int, int> { [ligne] = 99 });
        Assert.Equal(50, Stock(pid));
    }

    [Fact]
    public void Recu_partiellement_herite_bloque_la_reception_totale_automatique()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var (pid, cmd, ligne) = Commande(10, stock: 15, statut: "Reçu partiellement", recue: 0);

        Assert.True(CommandeService.EstPartielleHeritee(cmd));
        Assert.Throws<InvalidOperationException>(() => CommandeService.Receptionner(cmd));
        Assert.Equal(15, Stock(pid));

        // Réception manuelle ligne par ligne : autorisée
        CommandeService.Receptionner(cmd, new Dictionary<int, int> { [ligne] = 2 });
        Assert.Equal(25, Stock(pid));
        Assert.False(CommandeService.EstPartielleHeritee(cmd));
    }
}
