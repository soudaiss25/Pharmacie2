namespace Pharmacie2.Services
{
    /// <summary>Noms affichés dans l'application (un seul endroit).</summary>
    public static class AppInfo
    {
        /// <summary>Nom de la pharmacie, déjà utilisé sur les factures.</summary>
        public const string NomPharmacie = "LIGUAPHARME";

        /// <summary>Nom du logiciel, utilisé dans les titres de fenêtres et les rapports.</summary>
        public const string NomLogiciel = "LIGUAPHARME — Gestion";

        /// <summary>Titre d'une fenêtre : « Stock — LIGUAPHARME — Gestion ».</summary>
        public static string Titre(string ecran) => ecran + " — " + NomLogiciel;
    }
}
