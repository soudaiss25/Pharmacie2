using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

/// <summary>
/// Isole TOUS les tests dans un dossier temporaire (PHARMACIE2_DATA_DIR).
/// Le vrai dossier %LOCALAPPDATA%\Pharmacie2Data n'est jamais utilisé ni supprimé.
/// </summary>
public static class TestDb
{
    public const string MigrationAvantQuantiteUnites = "20260527215141_AjoutDepensesAnnexes";

    [ModuleInitializer]
    public static void Init()
    {
        string racine = Environment.GetEnvironmentVariable("RUNNER_TEMP") is { Length: > 0 } r ? r : Path.GetTempPath();
        string dossier = Path.Combine(racine, "p2tests_" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("PHARMACIE2_DATA_DIR", dossier);
    }

    /// <summary>Repart d'une base vide (fichiers du dossier temporaire uniquement).</summary>
    public static void Reinitialiser()
    {
        SqliteConnection.ClearAllPools();
        string dossier = Path.GetFullPath(CheminsApp.DossierDonnees);
        string tmp = Path.GetFullPath(Path.GetTempPath());
        string runner = Environment.GetEnvironmentVariable("RUNNER_TEMP");
        bool sur = dossier.StartsWith(tmp, StringComparison.OrdinalIgnoreCase)
                   || (!string.IsNullOrEmpty(runner) && dossier.StartsWith(Path.GetFullPath(runner), StringComparison.OrdinalIgnoreCase));
        if (!sur || dossier.EndsWith("Pharmacie2Data", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Dossier de test non isolé : " + dossier);

        foreach (var f in Directory.GetFiles(dossier, "PharmacieDB.sqlite*"))
            File.Delete(f);
    }

    public static void MigrerJusqua(string migration)
    {
        using var ctx = new AppDbContext();
        ctx.GetService<IMigrator>().Migrate(migration);
    }

    /// <summary>Identifiant complet d'une migration à partir de son nom (ex. « AjoutArchivage »).</summary>
    public static string MigrationId(string nom)
    {
        using var ctx = new AppDbContext();
        return ctx.Database.GetMigrations().Single(m => m.EndsWith("_" + nom));
    }

    /// <summary>
    /// Crée une vraie base de la pharmacie (migrations appliquées jusqu'à <paramref name="migrationCible"/>, ou toutes) à un chemin donné,
    /// avec un administrateur et <paramref name="ventes"/> ventes. N'utilise ni n'altère la base de test courante.
    /// </summary>
    public static string CreerBase(string chemin, int ventes, string? migrationCible = null, bool admin = true, bool adminActif = true)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(chemin)!);
        using (var ctx = new AppDbContext(chemin))
        {
            var migrator = ctx.GetService<IMigrator>();
            if (migrationCible == null) migrator.Migrate(); else migrator.Migrate(MigrationId(migrationCible));
        }
        using (var c = new SqliteConnection($"Data Source={chemin};Pooling=False"))
        {
            c.Open();
            if (admin)
            {
                Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
                if (!adminActif)
                {
                    using var cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE Users SET Actif = 0";
                    cmd.ExecuteNonQuery();
                }
            }
            for (int i = 1; i <= ventes; i++)
                Inserer(c, "ventes", new() { ["IdVente"] = (long)i, ["numeroVente"] = $"V-{i:D6}", ["Statut"] = "Active", ["MontantTotal"] = "1000", ["UserId"] = admin ? (object)1L : DBNull.Value });
        }
        SqliteConnection.ClearAllPools();
        return chemin;
    }

    public static void MigrerTout()
    {
        using var ctx = new AppDbContext();
        ctx.Database.Migrate();
    }

    public static SqliteConnection Ouvrir()
    {
        var c = new SqliteConnection($"Data Source={CheminsApp.CheminBase};Pooling=False");
        c.Open();
        return c;
    }

    public static long Scalaire(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        return Convert.ToInt64(cmd.ExecuteScalar());
    }

    public static long Scalaire(string sql)
    {
        using var c = Ouvrir();
        return Scalaire(c, sql);
    }

    public static string Texte(string sql)
    {
        using var c = Ouvrir();
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        return Convert.ToString(cmd.ExecuteScalar()) ?? "";
    }

    public static Dictionary<string, long> Comptes(SqliteConnection c, params string[] tables)
        => tables.ToDictionary(t => t, t => Scalaire(c, $"SELECT COUNT(*) FROM \"{t}\""));

    public static readonly string[] TablesMetier =
    {
        "Users", "produits", "fournisseur", "mutuels", "ventes", "LigneVentes", "paiement",
        "commandes", "LigneCommandes", "MutuelPaiements", "SessionsCaisse", "DepensesAnnexes"
    };

    /// <summary>Insère une ligne en remplissant les colonnes NOT NULL non précisées avec des valeurs neutres.</summary>
    public static void Inserer(SqliteConnection c, string table, Dictionary<string, object> v)
    {
        var cols = new List<string>();
        var vals = new List<object>();
        using (var cmd = c.CreateCommand())
        {
            cmd.CommandText = $"PRAGMA table_info('{table}')";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                string nom = r.GetString(1), type = r.GetString(2).ToUpperInvariant();
                bool notnull = r.GetInt32(3) == 1;
                if (v.TryGetValue(nom, out var val)) { cols.Add(nom); vals.Add(val); }
                else if (notnull && r.GetInt32(5) == 0 && r.IsDBNull(4))   // pas de valeur par défaut en base
                {
                    cols.Add(nom);
                    vals.Add(nom.StartsWith("Date") ? "2030-01-01 00:00:00"
                        : new[] { "Prix", "Marge", "Montant", "Taux", "Fond" }.Any(x => nom.StartsWith(x)) ? "0"
                        : type.Contains("TEXT") ? "" : type.Contains("INT") ? 0L : 0.0);
                }
            }
        }
        using var ins = c.CreateCommand();
        ins.CommandText = $"INSERT INTO \"{table}\" ({string.Join(",", cols.Select(x => $"\"{x}\""))}) VALUES ({string.Join(",", cols.Select((_, i) => "$p" + i))})";
        for (int i = 0; i < vals.Count; i++) ins.Parameters.AddWithValue("$p" + i, vals[i]);
        ins.ExecuteNonQuery();
    }

    public static int NouveauProduit(string nom, int stock, int nbParBoite, string unite = "Plaquette", decimal prixVente = 200m)
    {
        using var ctx = new AppDbContext();
        var p = new Produit
        {
            Nom = nom, Type = "Autre", PrixAchat = 100, PrixVente = prixVente, MargeBeneficiaire = 100,
            PrixVenteUnite = nbParBoite > 1 ? Math.Round(prixVente / nbParBoite, 0) : null,
            QuantiteEnStock = stock, NbUniteParBoite = nbParBoite, UniteVente = unite, SeuilAlerte = 2,
            DateExpiration = DateTime.Today.AddYears(1)
        };
        ctx.produits.Add(p);
        ctx.SaveChanges();
        return p.Id;
    }
}
