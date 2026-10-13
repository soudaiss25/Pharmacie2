using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public enum EtapeImport { Migration, Remplacement }

    public enum IssueImport { Importee, Abandon, Bloquee }

    /// <param name="Bloquant">Un contrôle bloquant en échec interdit l'import : rien n'est modifié.</param>
    public sealed record Controle(string Nom, bool Ok, string Detail, bool Bloquant = true);

    public sealed record LigneComparaison(string Table, long Avant, long Apres)
    {
        public bool Perdue => Apres < Avant;
    }

    public sealed class ResultatImport
    {
        public IssueImport Issue { get; set; }
        public string Message { get; set; } = "";
        public List<Controle> Controles { get; } = new();
        public List<LigneComparaison> Comparaison { get; set; } = new();
        public string? SauvegardeAvantImport { get; set; }
        public bool OriginalIntact { get; set; } = true;
    }

    /// <summary>
    /// Assistant d'import d'une ancienne base (fichier .sqlite, ou sauvegarde chiffrée .chiffre de la sauvegarde hors du PC).
    /// Le fichier d'origine n'est JAMAIS modifié : tout se passe sur une copie de travail (avec ses fichiers -wal et -shm).
    /// Étapes : copie ; contrôles bloquants (intégrité, tables, table des migrations, migration inconnue) et « photo avant » ;
    /// migration de la copie ; administrateur actif ; comparaison « après » (blocage si une ligne disparaît) ;
    /// sauvegarde « avant-import » de la base active ; remplacement. Chaque étape peut être annulée.
    /// </summary>
    public sealed class ImportBaseService : IDisposable
    {
        private static readonly string[] Suffixes = { "-wal", "-shm" };

        private readonly string _source;
        private readonly string _travail;
        private readonly Dictionary<string, string> _empreintesOriginales;

        public string Copie { get; }
        public List<LigneComparaison> PhotoAvant { get; private set; } = new();

        public ImportBaseService(string source, string? phrase)
        {
            _source = source;
            if (!File.Exists(source)) throw new FichierSauvegardeInvalideException("Le fichier choisi n'existe pas : " + source);

            _empreintesOriginales = Empreintes(source);                       // SHA-256 de l'original (et de ses -wal / -shm) AVANT toute lecture
            _travail = Path.Combine(CheminsApp.DossierDonnees, "Import_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_travail);
            Copie = Path.Combine(_travail, CheminsApp.NomFichierBase);

            if (source.EndsWith(SauvegardeExterneService.Extension, StringComparison.OrdinalIgnoreCase) || source.EndsWith(".chiffre", StringComparison.OrdinalIgnoreCase))
                SauvegardeExterneService.DechiffrerFichier(source, Copie, phrase ?? "");     // 7a : lève PhraseSecreteIncorrecteException si la phrase est fausse
            else
                foreach (var suffixe in new[] { "" }.Concat(Suffixes))
                    if (File.Exists(source + suffixe)) CopierFichier(source + suffixe, Copie + suffixe);
        }

        /// <summary>Copie un fichier même si une autre application l'a ouvert (lecture seule sur l'original).</summary>
        public static void CopierFichier(string source, string destination)
        {
            using var entree = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var sortie = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
            entree.CopyTo(sortie);
        }

        private static Dictionary<string, string> Empreintes(string source)
        {
            var d = new Dictionary<string, string>();
            foreach (var suffixe in new[] { "" }.Concat(Suffixes))
                if (File.Exists(source + suffixe))
                {
                    using var f = new FileStream(source + suffixe, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);   // lisible même si une autre application a le fichier ouvert
                    d[suffixe] = Convert.ToHexString(SHA256.HashData(f));
                }
            return d;
        }

        /// <summary>L'original n'a pas changé d'un octet (SHA-256 identique, mêmes fichiers).</summary>
        public bool OriginalIntact() => Empreintes(_source).OrderBy(x => x.Key).SequenceEqual(_empreintesOriginales.OrderBy(x => x.Key));

        public string Sha256Original => _empreintesOriginales.TryGetValue("", out var h) ? h : "";

        // ───────────── contrôles bloquants sur la copie, avant toute migration ─────────────

        public List<Controle> Controler()
        {
            var controles = new List<Controle>();
            Controle Essayer(string nom, Func<(bool ok, string detail)> verif)
            {
                try { var (ok, detail) = verif(); return new Controle(nom, ok, detail); }
                catch (Exception ex) { return new Controle(nom, false, "Impossible à vérifier : " + ex.Message); }
            }

            controles.Add(Essayer("Intégrité de la base", () =>
            {
                using var c = Ouvrir();
                string r = Convert.ToString(Scalaire(c, "PRAGMA integrity_check")) ?? "";
                return (r.Equals("ok", StringComparison.OrdinalIgnoreCase), r.Equals("ok", StringComparison.OrdinalIgnoreCase) ? "Le fichier est sain." : "Le fichier est abîmé : " + r);
            }));

            controles.Add(Essayer("Tables de la pharmacie", () =>
            {
                using var c = Ouvrir();
                long n = Convert.ToInt64(Scalaire(c, "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name IN ('produits','ventes','Users')"));
                return (n == 3, n == 3 ? "Produits, ventes et utilisateurs sont présents." : "Ce fichier n'est pas une base de la pharmacie.");
            }));

            controles.Add(Essayer("Table des migrations", () =>
            {
                using var c = Ouvrir();
                long t = Convert.ToInt64(Scalaire(c, "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'"));
                if (t == 0) return (false, "Aucune table des migrations : la version de cette base ne peut pas être reconnue.");
                long n = Convert.ToInt64(Scalaire(c, "SELECT COUNT(*) FROM \"__EFMigrationsHistory\""));
                return (n > 0, n > 0 ? $"{n} migration(s) enregistrée(s)." : "La table des migrations est vide.");
            }));

            controles.Add(Essayer("Version de la base reconnue", () =>
            {
                var connues = new HashSet<string>();
                using (var ctx = new AppDbContext(Copie)) connues = ctx.Database.GetMigrations().ToHashSet();
                var inconnues = new List<string>();
                using (var c = Ouvrir())
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT MigrationId FROM \"__EFMigrationsHistory\"";
                    using var r = cmd.ExecuteReader();
                    while (r.Read()) if (!connues.Contains(r.GetString(0))) inconnues.Add(r.GetString(0));
                }
                return (inconnues.Count == 0, inconnues.Count == 0
                    ? "Toutes les migrations de cette base sont connues de ce logiciel."
                    : "Cette base vient d'une version plus récente (migration inconnue : " + inconnues[0] + ") : import refusé.");
            }));

            if (controles.All(x => x.Ok))
                PhotoAvant = Photo();   // « photo avant » : nombre de lignes de chaque table
            Journaliser(controles);
            return controles;
        }

        private static void Journaliser(IEnumerable<Controle> controles)
        {
            foreach (var c in controles)
                Journal.Info($"Import — contrôle « {c.Nom} » : {(c.Ok ? "OK" : "ÉCHEC")} — {c.Detail}");
        }

        // ───────────── migration de la copie ─────────────

        public Controle Migrer()
        {
            try
            {
                using (var ctx = new AppDbContext(Copie))
                    ctx.Database.Migrate();
                SqliteConnection.ClearAllPools();
                var c = new Controle("Mise à jour de la copie", true, "La copie a été mise à jour vers la version de ce logiciel.");
                Journaliser(new[] { c });
                return c;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Import : migration de la copie impossible", ex);
                return new Controle("Mise à jour de la copie", false, "La mise à jour de la copie a échoué : " + ex.Message);
            }
        }

        public Controle ControlerAdministrateur()
        {
            try
            {
                using var ctx = new AppDbContext(Copie);
                bool admin = ctx.Users.Any(u => u.Role == Roles.Administrateur && u.Actif);
                var c = new Controle("Administrateur actif", admin, admin ? "Au moins un administrateur actif existe." : "Aucun administrateur actif : personne ne pourrait se connecter. Import refusé.");
                Journaliser(new[] { c });
                return c;
            }
            catch (Exception ex)
            {
                return new Controle("Administrateur actif", false, "Impossible à vérifier : " + ex.Message);
            }
        }

        /// <summary>Compare le nombre de lignes de chaque table avant et après la migration. Aucune ligne ne doit disparaître.</summary>
        public List<LigneComparaison> Comparer()
        {
            var apres = Photo().ToDictionary(x => x.Table, x => x.Avant);
            var liste = PhotoAvant.Select(a => new LigneComparaison(a.Table, a.Avant, apres.TryGetValue(a.Table, out var n) ? n : -1)).ToList();
            foreach (var l in liste)
                Journal.Info($"Import — comparaison : {l.Table} : {l.Avant} → {(l.Apres < 0 ? "table absente" : l.Apres.ToString())}{(l.Perdue ? "  ← LIGNES PERDUES" : "")}");
            return liste;
        }

        private List<LigneComparaison> Photo()
        {
            using var c = Ouvrir();
            var tables = new List<string>();
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' AND name <> '__EFMigrationsHistory' ORDER BY name";
                using var r = cmd.ExecuteReader();
                while (r.Read()) tables.Add(r.GetString(0));
            }
            return tables.Select(t => new LigneComparaison(t, Convert.ToInt64(Scalaire(c, $"SELECT COUNT(*) FROM \"{t}\"")), 0)).ToList();
        }

        // ───────────── remplacement de la base active ─────────────

        /// <summary>Sauvegarde « avant-import » de la base active (si elle existe) puis remplacement par la copie migrée. Renvoie le chemin de la sauvegarde.</summary>
        public string? Appliquer()
        {
            string? sauvegarde = null;
            if (File.Exists(CheminsApp.CheminBase))
            {
                sauvegarde = Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_avant-import_{DateTime.Now:yyyy-MM-dd_HHmmss}.sqlite");
                SauvegardeAutomatique.Sauvegarder(sauvegarde);
            }

            SqliteConnection.ClearAllPools();
            foreach (var suffixe in Suffixes)
            {
                if (File.Exists(CheminsApp.CheminBase + suffixe)) File.Delete(CheminsApp.CheminBase + suffixe);
                if (File.Exists(Copie + suffixe)) File.Delete(Copie + suffixe);     // la copie migrée est complète : plus de journal en attente
            }
            File.Copy(Copie, CheminsApp.CheminBase, overwrite: true);
            MigrationEmplacementBase.MarquerImportEffectue();
            Journal.Info($"Import — base remplacée par la copie migrée de {Path.GetFileName(_source)}" + (sauvegarde != null ? $" ; base précédente sauvegardée dans {sauvegarde}" : ""));
            return sauvegarde;
        }

        // ───────────── déroulement complet, avec points d'annulation ─────────────

        /// <summary>
        /// Déroulement complet sans écran (les écrans de l'assistant appellent les mêmes étapes une à une).
        /// <paramref name="continuer"/> est interrogé avant la migration puis avant le remplacement : « false » annule, rien n'est modifié.
        /// </summary>
        public static ResultatImport Importer(string source, string? phrase, Func<EtapeImport, bool>? continuer = null, Action<string>? apresMigration = null)
        {
            continuer ??= _ => true;
            var res = new ResultatImport();
            ImportBaseService? s = null;
            try
            {
                try { s = new ImportBaseService(source, phrase); }
                catch (PhraseSecreteIncorrecteException) { return Bloquer(res, "La phrase secrète est incorrecte (ou le fichier est abîmé). Rien n'a été modifié."); }
                catch (FichierSauvegardeInvalideException ex) { return Bloquer(res, ex.Message + " Rien n'a été modifié."); }

                res.Controles.AddRange(s.Controler());
                if (res.Controles.Any(c => c.Bloquant && !c.Ok)) return Bloquer(res, res.Controles.First(c => !c.Ok).Detail + " Rien n'a été modifié.");

                if (!continuer(EtapeImport.Migration)) return Abandonner(res, s);

                var migration = s.Migrer(); res.Controles.Add(migration);
                if (migration.Ok) apresMigration?.Invoke(s.Copie);   // point d'injection réservé aux tests
                if (!migration.Ok) return Bloquer(res, migration.Detail + " Rien n'a été modifié.");
                var admin = s.ControlerAdministrateur(); res.Controles.Add(admin);
                if (!admin.Ok) return Bloquer(res, admin.Detail + " Rien n'a été modifié.");

                res.Comparaison = s.Comparer();
                var perdues = res.Comparaison.Where(l => l.Perdue).ToList();
                if (perdues.Count > 0)
                    return Bloquer(res, $"Import refusé : des lignes disparaîtraient ({string.Join(", ", perdues.Select(l => l.Table))}). Rien n'a été modifié.");

                if (!continuer(EtapeImport.Remplacement)) return Abandonner(res, s);

                res.SauvegardeAvantImport = s.Appliquer();
                res.OriginalIntact = s.OriginalIntact();
                res.Issue = IssueImport.Importee;
                res.Message = "Import terminé.";
                return res;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Import d'une ancienne base", ex);
                return Bloquer(res, "L'import a échoué : " + ex.Message + " Rien n'a été modifié dans vos données actuelles.");
            }
            finally { s?.Dispose(); }
        }

        private static ResultatImport Bloquer(ResultatImport r, string message)
        {
            r.Issue = IssueImport.Bloquee; r.Message = message;
            Journal.Info("Import bloqué : " + message);
            return r;
        }

        private static ResultatImport Abandonner(ResultatImport r, ImportBaseService s)
        {
            r.Issue = IssueImport.Abandon; r.Message = "Import annulé : rien n'a été modifié.";
            r.OriginalIntact = s.OriginalIntact();
            Journal.Info("Import annulé par l'utilisateur.");
            return r;
        }

        // ───────────── utilitaires ─────────────

        private SqliteConnection Ouvrir()
        {
            var c = new SqliteConnection($"Data Source={Copie};Pooling=False");
            c.Open();
            return c;
        }

        private static object? Scalaire(SqliteConnection c, string sql)
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = sql;
            return cmd.ExecuteScalar();
        }

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();
            try { if (Directory.Exists(_travail)) Directory.Delete(_travail, true); } catch { /* nettoyage sans importance */ }
        }
    }
}
