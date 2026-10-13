using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>R5 : reprise des anciennes ventes payées en totalité dont les paiements manquent (base remplie).</summary>
public class RepriseVentesMigrationTests
{
    private static void Remplir(Microsoft.Data.Sqlite.SqliteConnection c)
    {
        TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
        void V(long id, string type, string total, string statut = "Active", string nom = "Client Secret")
            => TestDb.Inserer(c, "ventes", new()
            {
                ["IdVente"] = id, ["numeroVente"] = "V-00000" + id, ["Statut"] = statut, ["Type"] = type, ["MoyenPaiement"] = type,
                ["MontantTotal"] = total, ["UserId"] = 1L, ["DateVente"] = "2026-03-0" + id + " 10:00:00", ["NomClient"] = nom
            });
        V(1, "Comptant", "3000");                    // aucun paiement : à réparer (Espèces)
        V(2, "Carte bancaire", "2000");              // 500 versés : 1 500 à ajouter
        TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-2", ["VenteId"] = 2L, ["Montant"] = "500", ["UserId"] = 1L });
        V(3, "Mvola", "1000");                       // déjà payée en totalité : rien
        TestDb.Inserer(c, "paiement", new() { ["Id"] = 2L, ["NumeroPaiement"] = "P-3", ["VenteId"] = 3L, ["Montant"] = "1000", ["UserId"] = 1L });
        V(4, "Crédit", "1000");                      // jamais touchée
        V(5, "Mutuelle", "1000");                    // jamais touchée
        V(6, "Comptant", "1000", statut: "Annulée"); // annulée : jamais touchée
        V(7, "Huri Money", "800");                   // à réparer (mode = Huri Money)
    }

    [Fact]
    public void Reprise_sur_base_remplie_ne_fait_qu_ajouter_les_paiements_manquants_et_est_idempotente()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutAttenduCloture"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            Remplir(c);
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        using (var ctx = new AppDbContext())
            Assert.True(RepriseVentesService.EstEnAttente(ctx));

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        foreach (var kv in avant)
        {
            if (kv.Key == "paiement") Assert.Equal(kv.Value + 3, apres[kv.Key]);   // seules les lignes de paiement augmentent
            else Assert.Equal(kv.Value, apres[kv.Key]);
        }

        Assert.Equal(3000, TestDb.Scalaire(c2, "SELECT CAST(Montant AS INTEGER) FROM paiement WHERE NumeroPaiement='P-REPRISE-1'"));
        Assert.Equal("Espèces", TestDb.Texte("SELECT Mode FROM paiement WHERE NumeroPaiement='P-REPRISE-1'"));
        Assert.Equal(1500, TestDb.Scalaire(c2, "SELECT CAST(Montant AS INTEGER) FROM paiement WHERE NumeroPaiement='P-REPRISE-2'"));
        Assert.Equal("Carte bancaire", TestDb.Texte("SELECT Mode FROM paiement WHERE NumeroPaiement='P-REPRISE-2'"));
        Assert.Equal(800, TestDb.Scalaire(c2, "SELECT CAST(Montant AS INTEGER) FROM paiement WHERE NumeroPaiement='P-REPRISE-7'"));
        Assert.Equal("Huri Money", TestDb.Texte("SELECT Mode FROM paiement WHERE NumeroPaiement='P-REPRISE-7'"));
        Assert.Equal("2026-03-01 10:00:00", TestDb.Texte("SELECT DatePaiement FROM paiement WHERE NumeroPaiement='P-REPRISE-1'"));   // date de la vente
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT UserId FROM paiement WHERE NumeroPaiement='P-REPRISE-1'"));
        // Ni la vente à crédit, ni la mutuelle, ni la vente annulée, ni la vente déjà payée n'ont été touchées
        foreach (int id in new[] { 3, 4, 5, 6 })
            Assert.Equal(0, TestDb.Scalaire(c2, $"SELECT COUNT(*) FROM paiement WHERE NumeroPaiement='P-REPRISE-{id}'"));

        // Chaque vente réparée est maintenant soldée
        Assert.Equal(3000, TestDb.Scalaire(c2, "SELECT CAST(SUM(CAST(Montant AS REAL)) AS INTEGER) FROM paiement WHERE VenteId=1"));
        Assert.Equal(2000, TestDb.Scalaire(c2, "SELECT CAST(SUM(CAST(Montant AS REAL)) AS INTEGER) FROM paiement WHERE VenteId=2"));

        // Numéros de vente (sans nom de client) pour le journal
        using (var ctx = new AppDbContext())
        {
            Assert.False(RepriseVentesService.EstEnAttente(ctx));
            Assert.Equal(new[] { "V-000001", "V-000002", "V-000007" }, RepriseVentesService.NumerosRepares(ctx));
            RepriseVentesService.Journaliser(ctx);
        }
        c2.Close();
        string journal = File.ReadAllText(Directory.GetFiles(CheminsApp.DossierLogs, "journal_*.log").OrderBy(f => f).Last());
        Assert.Contains("3 vente(s) réparée(s)", journal);
        Assert.Contains("V-000001, V-000002, V-000007", journal);
        Assert.DoesNotContain("Client Secret", journal);

        // Idempotence : on revient en arrière puis on rejoue la migration, rien n'est ajouté une seconde fois
        using (var ctx = new AppDbContext())
        {
            var migrator = ctx.GetService<IMigrator>();
            migrator.Migrate(TestDb.MigrationId("AjoutAttenduCloture"));
            migrator.Migrate();
        }
        Assert.Equal(avant["paiement"] + 3, TestDb.Scalaire("SELECT COUNT(*) FROM paiement"));
    }
}
