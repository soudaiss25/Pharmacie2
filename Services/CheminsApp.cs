namespace Pharmacie2.Services
{
    /// <summary>
    /// Emplacements fixes des données de l'application, indépendants du répertoire courant.
    /// </summary>
    public static class CheminsApp
    {
        public const string NomFichierBase = "PharmacieDB.sqlite";

        /// <summary>Variable d'environnement réservée aux tests : remplace le dossier de données.</summary>
        private const string VariableDossierTest = "PHARMACIE2_DATA_DIR";

        public static string DossierDonnees { get; } = CreerDossier(
            Environment.GetEnvironmentVariable(VariableDossierTest) is { Length: > 0 } test
                ? test
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pharmacie2Data"));

        public static string CheminBase => Path.Combine(DossierDonnees, NomFichierBase);

        public static string DossierSauvegardes => CreerDossier(Path.Combine(DossierDonnees, "Sauvegardes"));

        public static string DossierLogs => CreerDossier(Path.Combine(DossierDonnees, "Logs"));

        private static string CreerDossier(string chemin)
        {
            Directory.CreateDirectory(chemin);
            return chemin;
        }
    }
}
