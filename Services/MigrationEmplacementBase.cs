namespace Pharmacie2.Services
{
    /// <summary>
    /// Copie (jamais déplace) l'ancienne base relative au répertoire courant vers l'emplacement fixe.
    /// </summary>
    public static class MigrationEmplacementBase
    {
        private static readonly string[] Suffixes = { "", "-wal", "-shm" };

        public static void Executer()
        {
            try
            {
                if (File.Exists(CheminsApp.CheminBase))
                    return;

                var candidats = new[] { AppContext.BaseDirectory, Environment.CurrentDirectory }
                    .Select(d => Path.GetFullPath(Path.Combine(d, CheminsApp.NomFichierBase)))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Where(File.Exists)
                    .Select(f => new FileInfo(f))
                    .Where(f => f.Length > 0)
                    .OrderByDescending(f => f.LastWriteTimeUtc)
                    .ToList();

                if (candidats.Count == 0)
                {
                    Journal.Info("Aucune ancienne base trouvée : une nouvelle base sera créée dans " + CheminsApp.DossierDonnees);
                    return;
                }

                var source = candidats[0];
                foreach (var suffixe in Suffixes)
                {
                    string src = source.FullName + suffixe;
                    if (File.Exists(src))
                        File.Copy(src, CheminsApp.CheminBase + suffixe, overwrite: false);
                }

                Journal.Info($"Ancienne base copiée : {source.FullName} ({source.Length} octets) -> {CheminsApp.CheminBase}. L'original est conservé.");
            }
            catch (Exception ex)
            {
                Journal.Erreur("Échec de la copie de l'ancienne base", ex);
                // Sans cette copie, l'application créerait une base vide : on refuse de continuer en silence.
                MessageBox.Show(
                    "Impossible de récupérer l'ancienne base de données.\n\n" + ex.Message +
                    "\n\nL'application va se fermer pour éviter de créer une base vide.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
    }
}
