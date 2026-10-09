using System.Text.Json;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Réglages de l'application (fichier parametres.json dans le dossier de données, hors base : une restauration
    /// de la base ne les écrase pas). Aucun mot de passe ni phrase secrète n'y figure.
    /// </summary>
    public sealed class ParametresApp
    {
        /// <summary>Dossier synchronisé (Google Drive pour ordinateur, clé USB…) où partent les copies chiffrées.</summary>
        public string? DossierSauvegardeExterne { get; set; }

        /// <summary>Sel PBKDF2 de la phrase secrète (base 64) ; la clé dérivée est protégée par Windows (DPAPI), jamais écrite ici.</summary>
        public string? SelSauvegarde { get; set; }

        /// <summary>Date de la dernière copie externe réussie.</summary>
        public DateTime? DerniereCopieExterne { get; set; }

        /// <summary>Verrouillage automatique après inactivité, en minutes (0 = désactivé).</summary>
        public int DelaiVerrouillageMinutes { get; set; } = 5;

        private static readonly object _verrou = new();
        private static ParametresApp? _actuels;
        private static string Chemin => Path.Combine(CheminsApp.DossierDonnees, "parametres.json");

        public static ParametresApp Actuels
        {
            get
            {
                lock (_verrou)
                    return _actuels ??= Lire();
            }
        }

        /// <summary>Modifie les réglages puis les enregistre (fichier temporaire puis remplacement).</summary>
        public static void Modifier(Action<ParametresApp> modification)
        {
            lock (_verrou)
            {
                var p = _actuels ??= Lire();
                modification(p);
                string temporaire = Chemin + ".tmp";
                File.WriteAllText(temporaire, JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true }));
                File.Move(temporaire, Chemin, overwrite: true);
            }
        }

        /// <summary>Oublie le cache et le fichier (tests uniquement).</summary>
        public static void ReinitialiserPourTests()
        {
            lock (_verrou)
            {
                _actuels = null;
                if (File.Exists(Chemin)) File.Delete(Chemin);
            }
        }

        private static ParametresApp Lire()
        {
            try
            {
                if (File.Exists(Chemin))
                    return JsonSerializer.Deserialize<ParametresApp>(File.ReadAllText(Chemin)) ?? new ParametresApp();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Lecture des réglages", ex);
            }
            return new ParametresApp();
        }
    }
}
