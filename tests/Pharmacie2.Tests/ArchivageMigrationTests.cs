using System.Text;
using Microsoft.Data.Sqlite;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>
/// Migration AjoutArchivage : reconstruction de LigneVentes, LigneCommandes, MutuelPaiements et commandes.
/// Aucune ligne ni valeur ne doit être perdue (y compris les colonnes ajoutées par les migrations précédentes).
/// </summary>
public class ArchivageMigrationTests
{
    private const string MigrationAvant = "20261008191612_DetectionStocksAVerifier";

    private static string Dump(SqliteConnection c, string table)
    {
        var sb = new StringBuilder();
        using var cmd = c.CreateCommand();
        cmd.CommandText = $"SELECT * FROM \"{table}\" ORDER BY 1";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            // Colonnes triées par nom : l'ordre physique peut changer lors d'une reconstruction de table
            var paires = new List<string>();
            for (int i = 0; i < r.FieldCount; i++)
                paires.Add(r.GetName(i) + "=" + (r.IsDBNull(i) ? "<null>" : Convert.ToString(r.GetValue(i))) + ";");
            paires.Sort(StringComparer.Ordinal);
            foreach (var pr in paires) sb.Append(pr);
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static void Remplir(SqliteConnection c)
    {
        TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
        TestDb.Inserer(c, "mutuels", new() { ["IdMutuel"] = 1L, ["NomEmployeur"] = "Mutuelle X", ["TauxPriseEnCharge"] = "80" });
        TestDb.Inserer(c, "fournisseur", new() { ["Id"] = 1L, ["Nom"] = "Four", ["Contact"] = "c" });
        TestDb.Inserer(c, "produits", new()
        {
            ["Id"] = 1L, ["Nom"] = "Doliprane", ["NbUniteParBoite"] = 5L, ["QuantiteEnStock"] = 9L, ["FournisseurId"] = 1L,
            ["StockAVerifier"] = 1L, ["UnitesManquantesEstimees"] = 40L, ["MotifVerification"] = "à vérifier"
        });
        TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "Sirop", ["NbUniteParBoite"] = 1L, ["QuantiteEnStock"] = 3L });
        TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["MutuelId"] = 1L, ["UserId"] = 1L, ["MontantTotal"] = "1000" });
        TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 1L, ["VenteId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 2L, ["QuantiteUnites"] = 10L, ["UniteVendue"] = "Boîte", ["PrixUnitaire"] = "500" });
        TestDb.Inserer(c, "LigneVentes", new() { ["Id"] = 2L, ["VenteId"] = 1L, ["ProduitId"] = 2L, ["Quantite"] = 3L, ["QuantiteUnites"] = 3L, ["UniteVendue"] = "Boîte", ["PrixUnitaire"] = "100" });
        TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-1", ["VenteId"] = 1L, ["Montant"] = "500", ["UserId"] = 1L });
        TestDb.Inserer(c, "MutuelPaiements", new() { ["Id"] = 1L, ["MutuelId"] = 1L, ["VenteId"] = 1L, ["NumeroPaiement"] = "MP-1", ["Montant"] = "300", ["UserId"] = 1L });
        TestDb.Inserer(c, "SessionsCaisse", new() { ["Id"] = 1L, ["UserId"] = 1L });
        TestDb.Inserer(c, "DepensesAnnexes", new() { ["Id"] = 1L, ["Categorie"] = "Loyer", ["Montant"] = "50", ["UserId"] = 1L });
        TestDb.Inserer(c, "commandes", new() { ["Id"] = 1L, ["FournisseurId"] = 1L, ["Statut"] = "Reçu" });
        TestDb.Inserer(c, "commandes", new() { ["Id"] = 2L, ["FournisseurId"] = 1L, ["Statut"] = "Reçu partiellement" });
        TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = 1L, ["CommandeId"] = 1L, ["ProduitId"] = 1L, ["Quantite"] = 10L, ["QuantiteRecue"] = 10L, ["PrixAchatUnitaire"] = "70" });
        TestDb.Inserer(c, "LigneCommandes", new() { ["Id"] = 2L, ["CommandeId"] = 2L, ["ProduitId"] = 1L, ["Quantite"] = 6L, ["QuantiteRecue"] = 4L, ["PrixAchatUnitaire"] = "70" });
    }

    [Fact]
    public void AjoutArchivage_ne_perd_aucune_ligne_ni_valeur_et_tout_reste_actif()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvant);

        var avant = new Dictionary<string, string>();
        Dictionary<string, long> comptesAvant;
        using (var c = TestDb.Ouvrir())
        {
            Remplir(c);
            comptesAvant = TestDb.Comptes(c, TestDb.TablesMetier);
            foreach (var t in TestDb.TablesMetier) avant[t] = Dump(c, t);
        }

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        var comptesApres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(comptesAvant, kv => Assert.Equal(kv.Value, comptesApres[kv.Key]));
        Assert.All(comptesAvant, kv => Assert.True(kv.Value > 0, "table de test vide : " + kv.Key));

        // Toutes les valeurs d'origine (y compris QuantiteUnites, QuantiteRecue, StockAVerifier,
        // UnitesManquantesEstimees, MotifVerification) sont conservées dans les tables reconstruites
        foreach (var t in new[] { "LigneVentes", "LigneCommandes", "MutuelPaiements", "commandes" })
        {
            string apres = Dump(c2, t);
            foreach (var ligneAvant in avant[t].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                Assert.Contains(ligneAvant, apres);
        }
        foreach (var t in new[] { "produits", "Users", "mutuels", "fournisseur" })
        {
            // Ces tables gagnent seulement la colonne Actif (à la fin) : les valeurs d'origine restent identiques
            string apres = Dump(c2, t).Replace("Actif=1;", "");
            Assert.Equal(avant[t], apres);
        }

        Assert.Equal(10, TestDb.Scalaire(c2, "SELECT QuantiteUnites FROM LigneVentes WHERE Id=1"));
        Assert.Equal(4, TestDb.Scalaire(c2, "SELECT QuantiteRecue FROM LigneCommandes WHERE Id=2"));
        Assert.Equal(40, TestDb.Scalaire(c2, "SELECT UnitesManquantesEstimees FROM produits WHERE Id=1"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT StockAVerifier FROM produits WHERE Id=1"));

        // Tout ce qui existait est ACTIF
        foreach (var t in new[] { "produits", "Users", "mutuels", "fournisseur" })
            Assert.Equal(0, TestDb.Scalaire(c2, $"SELECT COUNT(*) FROM \"{t}\" WHERE Actif <> 1"));

        // Les clés étrangères sont bien en RESTRICT
        foreach (var (table, cible) in new[] { ("LigneVentes", "produits"), ("LigneCommandes", "produits"), ("MutuelPaiements", "mutuels"), ("commandes", "fournisseur") })
        {
            using var cmd = c2.CreateCommand();
            cmd.CommandText = $"SELECT on_delete FROM pragma_foreign_key_list('{table}') WHERE \"table\"='{cible}'";
            Assert.Equal("RESTRICT", (string)cmd.ExecuteScalar());
        }
    }

    [Fact]
    public void Supprimer_un_produit_avec_historique_est_refuse_par_la_base()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 7, 5);
        using (var ctx = new AppDbContext())
        {
            var v = new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "",
                MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MotifAnnulation = ""
            };
            v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 1, QuantiteUnites = 5 });
            ctx.ventes.Add(v);
            ctx.SaveChanges();
        }

        Assert.ThrowsAny<Exception>(() =>
        {
            using var ctx = new AppDbContext();
            ctx.produits.Remove(ctx.produits.Find(pid));
            ctx.SaveChanges();
        });
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM LigneVentes"));
        Assert.Equal(1, TestDb.Scalaire($"SELECT COUNT(*) FROM produits WHERE Id={pid}"));
    }
}
