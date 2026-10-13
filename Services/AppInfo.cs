namespace Pharmacie2.Services
{
    /// <summary>Noms affichés dans l'application (un seul endroit).</summary>
    public static class AppInfo
    {
        /// <summary>Nom de la pharmacie, déjà utilisé sur les factures.</summary>
        public const string NomPharmacie = "LIGUAPHARME";

        /// <summary>Nom du logiciel, utilisé dans les titres de fenêtres et les rapports.</summary>
        public const string NomLogiciel = "LIGUAPHARME — Gestion";

        /// <summary>
        /// Version de l'application (celle du fichier .csproj, ou celle du tag lors d'une publication), sans l'empreinte du commit.
        /// </summary>
        public static string Version
        {
            get
            {
                string v = typeof(AppInfo).Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
                    .OfType<System.Reflection.AssemblyInformationalVersionAttribute>().FirstOrDefault()?.InformationalVersion
                    ?? typeof(AppInfo).Assembly.GetName().Version?.ToString(3) ?? "inconnue";
                int plus = v.IndexOf('+');
                return plus >= 0 ? v[..plus] : v;
            }
        }

        /// <summary>Titre d'une fenêtre : « Stock — LIGUAPHARME — Gestion ».</summary>
        public static string Titre(string ecran) => ecran + " — " + NomLogiciel;
    }
}
