using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using Pharmacie2.views;

namespace Pharmacie2.Services
{
    /// <summary>Une base de données trouvée à un ancien emplacement.</summary>
    public sealed record BaseTrouvee(string Chemin, long Taille, DateTime DateModif, int NbVentes)
    {
        /// <summary>Non vide = fichier non vide qui contient des ventes (ou illisible : on ne prend pas le risque de l'ignorer).</summary>
        public bool NonVide => Taille > 0 && NbVentes != 0;
    }

    /// <summary>Où chercher les anciennes bases (injectable pour les tests ; <see cref="Systeme"/> donne les vrais emplacements Windows).</summary>
    public sealed class EmplacementsAnciens
    {
        /// <summary>Dossiers où PharmacieDB.sqlite est cherché directement (dossier de l'application, dossier courant…).</summary>
        public List<string> Dossiers { get; init; } = new();

        /// <summary>Racines (Program Files, Program Files (x86), %LOCALAPPDATA%\Programs, VirtualStore…) : leurs sous-dossiers dont le nom contient « Pharma » sont fouillés.</summary>
        public List<string> RacinesPharma { get; init; } = new();

        /// <summary>%APPDATA%\Pharmacie2\ : sa seule existence prouve que l'application a déjà été installée.</summary>
        public string? DossierAppData { get; init; }

        /// <summary>Entrées de désinstallation Windows (HKCU et HKLM) : nom affiché et InstallLocation.</summary>
        public List<(string Nom, string? Emplacement)> EntreesDesinstallation { get; init; } = new();

        public static EmplacementsAnciens Systeme()
        {
            string Dossier(Environment.SpecialFolder d) => Environment.GetFolderPath(d);
            string local = Dossier(Environment.SpecialFolder.LocalApplicationData);
            var racines = new List<string>
            {
                Dossier(Environment.SpecialFolder.ProgramFiles),
                Dossier(Environment.SpecialFolder.ProgramFilesX86),
                Path.Combine(local, "Programs"),
                Path.Combine(local, "VirtualStore", "Program Files"),
                Path.Combine(local, "VirtualStore", "Program Files (x86)")
            };
            return new EmplacementsAnciens
            {
                Dossiers = new[] { AppContext.BaseDirectory, Environment.CurrentDirectory }.ToList(),
                RacinesPharma = racines.Where(r => !string.IsNullOrEmpty(r)).ToList(),
                DossierAppData = Path.Combine(Dossier(Environment.SpecialFolder.ApplicationData), "Pharmacie2"),
                EntreesDesinstallation = LireDesinstallation()
            };
        }

        private static List<(string, string?)> LireDesinstallation()
        {
            var liste = new List<(string, string?)>();
            const string cle = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var ruches = new (RegistryKey racine, string chemin)[]
            {
                (Registry.CurrentUser, cle), (Registry.LocalMachine, cle), (Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall")
            };
            foreach (var (racine, chemin) in ruches)
            {
                try
                {
                    using var k = racine.OpenSubKey(chemin);
                    if (k == null) continue;
                    foreach (var nom in k.GetSubKeyNames())
                    {
                        try
                        {
                            using var s = k.OpenSubKey(nom);
                            string? affiche = s?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(affiche))
                                liste.Add((affiche, s!.GetValue("InstallLocation") as string));
                        }
                        catch { /* entrée illisible : ignorée */ }
                    }
                }
                catch { /* ruche inaccessible : ignorée */ }
            }
            return liste;
        }
    }

    public enum CasBase { PremiereInstallation, UneBase, PlusieursBases, IntrouvableDejaInstallee }

    public sealed record AnalyseBase(CasBase Cas, List<BaseTrouvee> Bases, bool DejaInstallee)
    {
        public List<BaseTrouvee> NonVides => Bases.Where(b => b.NonVide).OrderByDescending(b => b.DateModif).ToList();
    }

    /// <summary>
    /// Retrouve l'ancienne base (déploiement d'avril) et la COPIE (jamais déplace) vers l'emplacement fixe.
    /// Cherche : dossier de l'application et dossier courant, %APPDATA%\Pharmacie2, VirtualStore, dossiers d'installation des clés de
    /// désinstallation Windows, sous-dossiers « Pharma » de Program Files, Program Files (x86) et %LOCALAPPDATA%\Programs.
    /// </summary>
    public static class MigrationEmplacementBase
    {
        private static readonly string[] Suffixes = { "", "-wal", "-shm" };

        /// <summary>Vrai si une ancienne base vient d'être copiée pendant ce démarrage : le diagnostic automatique s'exécutera après les migrations.</summary>
        public static bool ImportEffectue { get; private set; }

        private static bool ContientPharma(string? s)
            => s != null && (s.Contains("Pharma", StringComparison.OrdinalIgnoreCase) || s.Contains("LIGUAPHARME", StringComparison.OrdinalIgnoreCase));

        /// <summary>Toutes les bases présentes aux anciens emplacements (vides ou non), journalisées une par une.</summary>
        public static AnalyseBase Analyser(EmplacementsAnciens e)
        {
            var dossiers = new List<string>();
            void Ajouter(string? d) { if (!string.IsNullOrWhiteSpace(d)) dossiers.Add(d); }

            e.Dossiers.ForEach(Ajouter);
            if (e.DossierAppData != null && Directory.Exists(e.DossierAppData)) Ajouter(e.DossierAppData);

            var entreesPharma = e.EntreesDesinstallation.Where(x => ContientPharma(x.Nom)).ToList();
            foreach (var x in entreesPharma) Ajouter(x.Emplacement);

            foreach (var racine in e.RacinesPharma)
            {
                try
                {
                    if (!Directory.Exists(racine)) continue;
                    foreach (var sous in Directory.EnumerateDirectories(racine).Where(d => ContientPharma(Path.GetFileName(d))))
                    {
                        Ajouter(sous);
                        foreach (var niveau2 in Directory.EnumerateDirectories(sous)) Ajouter(niveau2);
                    }
                }
                catch { /* dossier non lisible : ignoré */ }
            }

            string cible = Path.GetFullPath(CheminsApp.CheminBase);
            var bases = dossiers
                .Select(d => { try { return Path.GetFullPath(Path.Combine(d.Trim().Trim('"'), CheminsApp.NomFichierBase)); } catch { return null; } })
                .Where(f => f != null && !f.Equals(cible, StringComparison.OrdinalIgnoreCase) && File.Exists(f))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(f => { var i = new FileInfo(f!); return new BaseTrouvee(i.FullName, i.Length, i.LastWriteTime, i.Length > 0 ? CompterVentes(i.FullName) : 0); })
                .OrderByDescending(b => b.DateModif)
                .ToList();

            foreach (var b in bases)
                Journal.Info($"Base candidate : {b.Chemin} — {b.Taille} octets — modifiée le {b.DateModif:dd/MM/yyyy HH:mm} — {b.NbVentes} vente(s)");
            foreach (var x in entreesPharma)
                Journal.Info($"Entrée de désinstallation trouvée : {x.Nom} ({x.Emplacement ?? "emplacement inconnu"})");

            bool dejaInstallee = (e.DossierAppData != null && Directory.Exists(e.DossierAppData)) || entreesPharma.Count > 0 || bases.Count > 0;
            int nonVides = bases.Count(b => b.NonVide);
            var cas = nonVides > 1 ? CasBase.PlusieursBases
                    : nonVides == 1 ? CasBase.UneBase
                    : dejaInstallee ? CasBase.IntrouvableDejaInstallee
                    : CasBase.PremiereInstallation;
            return new AnalyseBase(cas, bases, dejaInstallee);
        }

        /// <summary>Nombre de ventes d'une base (lecture seule) ; -1 si elle est illisible.</summary>
        public static int CompterVentes(string chemin)
        {
            try
            {
                using var c = new SqliteConnection($"Data Source={chemin};Mode=ReadOnly;Pooling=False");
                c.Open();
                using var cmd = c.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM ventes";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        /// <summary>Copie la base choisie (et ses fichiers -wal / -shm) vers l'emplacement fixe. L'original est conservé.</summary>
        public static void Copier(string source)
        {
            foreach (var suffixe in Suffixes)
            {
                string src = source + suffixe;
                if (File.Exists(src))
                    File.Copy(src, CheminsApp.CheminBase + suffixe, overwrite: false);
            }
            ImportEffectue = true;
            Journal.Info($"Ancienne base copiée : {source} ({new FileInfo(source).Length} octets) -> {CheminsApp.CheminBase}. L'original est conservé.");
        }

        public enum Issue { RienACopier, Copiee, Abandon }

        /// <summary>
        /// Déroulement complet, sans écran de dialogue en dur (les choix de l'utilisateur sont fournis) : testable.
        /// <paramref name="choisir"/> : plusieurs bases → la base retenue (null = abandon).
        /// <paramref name="fichierManuel"/> : application déjà installée mais base introuvable → fichier choisi par l'utilisateur (null = abandon).
        /// </summary>
        public static Issue Executer(EmplacementsAnciens e, Func<IReadOnlyList<BaseTrouvee>, BaseTrouvee?> choisir, Func<string?> fichierManuel)
        {
            if (File.Exists(CheminsApp.CheminBase)) return Issue.RienACopier;

            var analyse = Analyser(e);
            switch (analyse.Cas)
            {
                case CasBase.UneBase:
                    Copier(analyse.NonVides[0].Chemin);
                    return Issue.Copiee;

                case CasBase.PlusieursBases:
                    var retenue = choisir(analyse.NonVides);   // la plus récente est présélectionnée par l'écran
                    if (retenue == null) return Issue.Abandon;
                    Journal.Info("Plusieurs bases non vides : l'utilisateur a choisi " + retenue.Chemin);
                    Copier(retenue.Chemin);
                    return Issue.Copiee;

                case CasBase.IntrouvableDejaInstallee:
                    Journal.Info("Application déjà installée mais aucune base non vide trouvée : écran « Base de données introuvable ».");
                    string? fichier = fichierManuel();
                    if (fichier == null) return Issue.Abandon;
                    Copier(fichier);
                    return Issue.Copiee;

                default:
                    Journal.Info("Aucune trace d'une ancienne installation : première installation, une nouvelle base sera créée dans " + CheminsApp.DossierDonnees);
                    return Issue.RienACopier;
            }
        }

        /// <summary>Appelé au démarrage, avant tout accès à la base.</summary>
        public static void Executer()
        {
            try
            {
                var issue = Executer(EmplacementsAnciens.Systeme(), ChoisirParmiPlusieurs, DemanderFichier);
                if (issue == Issue.Abandon)
                {
                    // Jamais de base vide créée « en silence » alors qu'une installation existe
                    Environment.Exit(0);
                }
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

        private static BaseTrouvee? ChoisirParmiPlusieurs(IReadOnlyList<BaseTrouvee> bases)
        {
            using var f = new FormChoixBase(bases);
            return f.ShowDialog() == DialogResult.OK ? f.BaseChoisie : null;
        }

        private static string? DemanderFichier()
        {
            using var f = new FormBaseIntrouvable();
            return f.ShowDialog() == DialogResult.OK ? f.FichierChoisi : null;
        }
    }
}
