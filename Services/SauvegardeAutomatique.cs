using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Sauvegardes restaurables de la base (copie cohérente SQLite, pas File.Copy).
    /// Une erreur est journalisée mais ne bloque jamais le démarrage.
    /// </summary>
    public static class SauvegardeAutomatique
    {
        private const int NbSauvegardesJournalieresGardees = 30;

        public static void Executer()
        {
            try
            {
                if (!File.Exists(CheminsApp.CheminBase))
                    return; // première installation : rien à sauvegarder

                if (MigrationsEnAttente())
                    Sauvegarder(Path.Combine(CheminsApp.DossierSauvegardes,
                        $"PharmacieDB_avant-migration_{DateTime.Now:yyyy-MM-dd_HHmmss}.sqlite"));

                string jour = Path.Combine(CheminsApp.DossierSauvegardes,
                    $"PharmacieDB_{DateTime.Now:yyyy-MM-dd}.sqlite");
                if (!File.Exists(jour))
                    Sauvegarder(jour);

                Purger();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Sauvegarde automatique impossible", ex);
            }
        }

        private static bool MigrationsEnAttente()
        {
            try
            {
                using var ctx = new AppDbContext();
                return ctx.Database.GetPendingMigrations().Any();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Lecture des migrations en attente", ex);
                return false;
            }
        }

        private static void Sauvegarder(string destination)
        {
            string temporaire = destination + ".tmp";
            try
            {
                if (File.Exists(temporaire)) File.Delete(temporaire);

                using (var source = new SqliteConnection($"Data Source={CheminsApp.CheminBase};Pooling=False"))
                using (var cible = new SqliteConnection($"Data Source={temporaire};Pooling=False"))
                {
                    source.Open();
                    cible.Open();
                    source.BackupDatabase(cible);
                }

                File.Move(temporaire, destination);
                Journal.Info("Sauvegarde créée : " + destination);
            }
            catch
            {
                try { if (File.Exists(temporaire)) File.Delete(temporaire); } catch { }
                throw;
            }
        }

        /// <summary>Garde les 30 sauvegardes journalières les plus récentes ; « avant-migration » jamais supprimées.</summary>
        private static void Purger()
        {
            var anciennes = new DirectoryInfo(CheminsApp.DossierSauvegardes)
                .GetFiles("PharmacieDB_????-??-??.sqlite")
                .OrderByDescending(f => f.Name)
                .Skip(NbSauvegardesJournalieresGardees);

            foreach (var f in anciennes)
            {
                try { f.Delete(); }
                catch (Exception ex) { Journal.Erreur("Purge de " + f.Name, ex); }
            }
        }
    }
}
