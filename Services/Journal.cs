using System.Text;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Journal texte mensuel (Logs\journal_yyyy-MM.log). Thread-safe, ne lève jamais d'exception.
    /// </summary>
    public static class Journal
    {
        private static readonly object _verrou = new();

        public static void Info(string message) => Ecrire("INFO", message);

        public static void Erreur(string message, Exception? ex = null)
            => Ecrire("ERREUR", ex == null ? message : message + Environment.NewLine + ex);

        private static void Ecrire(string niveau, string message)
        {
            try
            {
                string fichier = Path.Combine(CheminsApp.DossierLogs, $"journal_{DateTime.Now:yyyy-MM}.log");
                string ligne = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{niveau}] {message}{Environment.NewLine}";
                lock (_verrou)
                {
                    File.AppendAllText(fichier, ligne, Encoding.UTF8);
                }
            }
            catch
            {
                // Le journal ne doit jamais faire échouer l'application.
            }
        }
    }
}
