using Velopack;
using Velopack.Sources;

namespace Pharmacie2.Services
{
    /// <summary>Une mise à jour trouvée ; <c>Jeton</c> est l'objet propre au gestionnaire (UpdateInfo de Velopack).</summary>
    public sealed record InfoMiseAJour(string Version, object? Jeton);

    /// <summary>Ce dont le service a besoin d'un gestionnaire de mises à jour (Velopack en production, un faux gestionnaire dans les tests).</summary>
    public interface IGestionnaireMiseAJour
    {
        bool EstInstalle { get; }
        string VersionActuelle { get; }
        Task<InfoMiseAJour?> VerifierAsync();
        Task TelechargerAsync(InfoMiseAJour info);
        void AppliquerALaFermeture(InfoMiseAJour info);
        void AppliquerEtRedemarrer(InfoMiseAJour info);
    }

    /// <summary>Gestionnaire réel : Velopack, mises à jour publiées dans les Releases GitHub du dépôt.</summary>
    public sealed class GestionnaireVelopack : IGestionnaireMiseAJour
    {
        private readonly UpdateManager _mgr;

        public GestionnaireVelopack(string canal)
        {
            // GithubSource(repoUrl, accessToken, prerelease) : dépôt public, donc pas de jeton ; les versions « test » sont des pré-releases.
            // ExplicitChannel : le canal choisi dans Administration (stable ou test), quel que soit le canal de l'installation d'origine.
            _mgr = new UpdateManager(
                new GithubSource(MiseAJourService.UrlDepot, null, canal == MiseAJourService.CanalTest),
                new UpdateOptions { ExplicitChannel = canal });
        }

        public bool EstInstalle => _mgr.IsInstalled;
        public string VersionActuelle => _mgr.CurrentVersion?.ToString() ?? AppInfo.Version;

        public async Task<InfoMiseAJour?> VerifierAsync()
        {
            var info = await _mgr.CheckForUpdatesAsync().ConfigureAwait(false);
            return info == null ? null : new InfoMiseAJour(info.TargetFullRelease.Version.ToString(), info);
        }

        public Task TelechargerAsync(InfoMiseAJour info) => _mgr.DownloadUpdatesAsync((UpdateInfo)info.Jeton!);

        // sans redémarrage : l'application est déjà en train de se fermer
        public void AppliquerALaFermeture(InfoMiseAJour info)
            => _mgr.WaitExitThenApplyUpdates(((UpdateInfo)info.Jeton!).TargetFullRelease, silent: false, restart: false);

        public void AppliquerEtRedemarrer(InfoMiseAJour info)
            => _mgr.ApplyUpdatesAndRestart(((UpdateInfo)info.Jeton!).TargetFullRelease);
    }

    public enum EtatMiseAJour { NonInstallee, Aucune, Prete, Erreur }

    public sealed record ResultatRecherche(string VersionActuelle, string? VersionDisponible, EtatMiseAJour Etat, string Message);

    public sealed record ResultatInstallation(bool Lancee, string Message);

    /// <summary>
    /// Mises à jour automatiques. Règles : jamais de redémarrage pendant le travail ; la vérification se fait en arrière-plan
    /// (30 secondes après l'ouverture, puis toutes les 4 heures), un échec réseau est journalisé et ignoré ; une mise à jour téléchargée
    /// s'installe à la fermeture normale ou quand l'administrateur clique « Installer maintenant » (refusé pendant une vente en cours).
    /// Hors d'une vraie installation (Visual Studio, tests) rien ne se passe.
    /// </summary>
    public sealed class MiseAJourService : IDisposable
    {
        public const string PackId = "LIGUAPHARME";
        public const string UrlDepot = "https://github.com/soudaiss25/Pharmacie2";
        public const string CanalStable = "stable";
        public const string CanalTest = "test";
        public const string MessageVenteEnCours = "Une vente est en cours de saisie : terminez-la ou abandonnez-la avant d'installer la mise à jour.";

        public static readonly TimeSpan DelaiPremiereVerification = TimeSpan.FromSeconds(30);
        public static readonly TimeSpan IntervalleVerification = TimeSpan.FromHours(4);

        /// <summary>
        /// Dossier d'installation de Velopack : %LOCALAPPDATA%\LIGUAPHARME. Il est volontairement DIFFÉRENT du dossier des données
        /// (%LOCALAPPDATA%\Pharmacie2Data) : une mise à jour ne peut jamais toucher la base, les sauvegardes ni les journaux.
        /// </summary>
        public static string DossierInstallation => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), PackId);

        private static MiseAJourService? _instance;
        public static MiseAJourService Instance
        {
            get => _instance ??= new MiseAJourService(canal => new GestionnaireVelopack(canal));
            set => _instance = value;   // tests
        }

        private readonly Func<string, IGestionnaireMiseAJour> _fabrique;
        private readonly SemaphoreSlim _verrou = new(1, 1);
        private IGestionnaireMiseAJour? _gestionnaire;
        private string _canalCourant;
        private InfoMiseAJour? _prete;
        private System.Threading.Timer? _minuteur;
        private string? _annonce;

        /// <summary>Levé (sur un fil d'arrière-plan) quand une mise à jour est téléchargée et prête.</summary>
        public event Action<string>? MiseAJourPrete;

        public MiseAJourService(Func<string, IGestionnaireMiseAJour> fabrique)
        {
            _fabrique = fabrique;
            _canalCourant = CanalReglage();
        }

        private static string CanalReglage()
            => string.Equals(ParametresApp.Actuels.CanalMiseAJour, CanalTest, StringComparison.OrdinalIgnoreCase) ? CanalTest : CanalStable;

        private IGestionnaireMiseAJour Gestionnaire
        {
            get
            {
                string canal = CanalReglage();
                if (_gestionnaire == null || canal != _canalCourant)
                {
                    _gestionnaire = _fabrique(canal);
                    if (canal != _canalCourant) _prete = null;   // une mise à jour d'un autre canal n'est plus valable
                    _canalCourant = canal;
                }
                return _gestionnaire;
            }
        }

        public string Canal => CanalReglage();

        public bool EstInstalle
        {
            get { try { return Gestionnaire.EstInstalle; } catch { return false; } }
        }

        public string VersionActuelle
        {
            get { try { return Gestionnaire.VersionActuelle; } catch { return AppInfo.Version; } }
        }

        public string? VersionPrete => _prete?.Version;

        // ───────────── choix du canal (Administration, réservé à l'Administrateur) ─────────────

        public void ChangerCanal(string canal)
        {
            canal = string.Equals(canal, CanalTest, StringComparison.OrdinalIgnoreCase) ? CanalTest : CanalStable;
            ParametresApp.Modifier(p => p.CanalMiseAJour = canal);
            _prete = null;
            Journal.Info("Mises à jour : canal « " + canal + " » choisi.");
        }

        // ───────────── vérification en arrière-plan ─────────────

        /// <summary>Démarre la vérification périodique (rien hors d'une vraie installation).</summary>
        public void Demarrer(TimeSpan? premiere = null, TimeSpan? intervalle = null)
        {
            if (!EstInstalle)
            {
                Journal.Info("Mises à jour : application non installée (Visual Studio ou tests) : aucune vérification.");
                return;
            }
            _minuteur?.Dispose();
            _minuteur = new System.Threading.Timer(_ => { _ = VerifierEtTelechargerAsync(); },
                null, premiere ?? DelaiPremiereVerification, intervalle ?? IntervalleVerification);
        }

        /// <summary>
        /// Cherche une mise à jour et, s'il y en a une, la télécharge. NE LÈVE JAMAIS D'EXCEPTION : tout échec (réseau coupé,
        /// source introuvable…) est journalisé et ignoré. Une seule vérification à la fois.
        /// </summary>
        public async Task<ResultatRecherche> VerifierEtTelechargerAsync()
        {
            string actuelle = VersionActuelle;
            try
            {
                if (!EstInstalle)
                    return new ResultatRecherche(actuelle, null, EtatMiseAJour.NonInstallee, "Cette copie du logiciel n'est pas installée (lancée depuis Visual Studio) : pas de mise à jour automatique.");

                if (!await _verrou.WaitAsync(0).ConfigureAwait(false))
                    return new ResultatRecherche(actuelle, VersionPrete, VersionPrete != null ? EtatMiseAJour.Prete : EtatMiseAJour.Aucune, "Une vérification est déjà en cours.");
                try
                {
                    var gestionnaire = Gestionnaire;
                    if (_prete != null)
                        return new ResultatRecherche(actuelle, _prete.Version, EtatMiseAJour.Prete, $"La version {_prete.Version} est prête : elle s'installera à la prochaine ouverture.");

                    var info = await gestionnaire.VerifierAsync().ConfigureAwait(false);
                    if (info == null)
                    {
                        Journal.Info($"Mises à jour : aucune nouvelle version (canal {Canal}, version actuelle {actuelle}).");
                        return new ResultatRecherche(actuelle, null, EtatMiseAJour.Aucune, "Le logiciel est à jour.");
                    }

                    Journal.Info($"Mises à jour : version {info.Version} trouvée, téléchargement en arrière-plan.");
                    await gestionnaire.TelechargerAsync(info).ConfigureAwait(false);
                    _prete = info;
                    Journal.Info($"Mises à jour : version {info.Version} téléchargée, prête à être installée.");
                    try { MiseAJourPrete?.Invoke(info.Version); }
                    catch (Exception ex) { Journal.Erreur("Mises à jour : abonné à l'événement", ex); }
                    return new ResultatRecherche(actuelle, info.Version, EtatMiseAJour.Prete, $"La version {info.Version} est téléchargée : elle s'installera à la prochaine ouverture.");
                }
                finally { _verrou.Release(); }
            }
            catch (Exception ex)
            {
                // Connexion instable à Mohéli : on journalise (sans trace d'erreur bruyante) et on réessaiera plus tard
                Journal.Info($"Mises à jour : vérification impossible pour le moment ({ex.GetType().Name} : {ex.Message}). Nouvel essai plus tard.");
                return new ResultatRecherche(actuelle, null, EtatMiseAJour.Erreur, "La vérification n'a pas pu se faire (connexion absente ou instable). Le logiciel réessaiera tout seul.");
            }
        }

        // ───────────── installation ─────────────

        /// <summary>
        /// « Installer maintenant » : refusé pendant une vente en cours de saisie ; sinon, après confirmation, la mise à jour est appliquée
        /// et l'application redémarre. <paramref name="confirmer"/> n'est appelé que si aucune vente n'est en cours.
        /// </summary>
        public ResultatInstallation TenterInstallation(Func<bool> venteEnCours, Func<bool> confirmer)
        {
            if (_prete == null) return new ResultatInstallation(false, "Aucune mise à jour n'est prête.");
            if (venteEnCours()) return new ResultatInstallation(false, MessageVenteEnCours);
            if (!confirmer()) return new ResultatInstallation(false, "Installation annulée.");
            try
            {
                Journal.Info($"Mises à jour : installation immédiate de la version {_prete.Version}.");
                Gestionnaire.AppliquerEtRedemarrer(_prete);
                return new ResultatInstallation(true, "");
            }
            catch (Exception ex)
            {
                Journal.Erreur("Mises à jour : installation impossible", ex);
                return new ResultatInstallation(false, "L'installation n'a pas pu démarrer. La mise à jour s'installera à la prochaine ouverture.");
            }
        }

        /// <summary>Fermeture normale : une mise à jour déjà téléchargée est appliquée, sans redémarrage forcé.</summary>
        public void AppliquerALaFermeture()
        {
            try
            {
                _minuteur?.Dispose();
                _minuteur = null;
                if (_prete == null || !EstInstalle) return;
                Journal.Info($"Mises à jour : la version {_prete.Version} s'installe à la fermeture.");
                Gestionnaire.AppliquerALaFermeture(_prete);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Mises à jour : application à la fermeture impossible", ex);
            }
        }

        // ───────────── annonce après une mise à jour ─────────────

        /// <summary>
        /// Au démarrage : si la version a changé depuis la dernière ouverture, prépare l'annonce « Application mise à jour en version X »
        /// (jamais à la toute première ouverture). La nouvelle version est mémorisée tout de suite.
        /// </summary>
        public void DetecterMiseAJourInstallee()
        {
            try
            {
                if (!EstInstalle) return;   // hors installation : aucune annonce
                string version = VersionActuelle;
                string? precedente = ParametresApp.Actuels.DerniereVersionLancee;
                if (precedente != version)
                {
                    if (!string.IsNullOrEmpty(precedente)) _annonce = $"Application mise à jour en version {version}";
                    ParametresApp.Modifier(p => p.DerniereVersionLancee = version);
                }
            }
            catch (Exception ex) { Journal.Erreur("Mises à jour : lecture de la dernière version lancée", ex); }
        }

        /// <summary>L'annonce à afficher une seule fois (null s'il n'y en a pas).</summary>
        public string? PrendreAnnonce()
        {
            var a = _annonce;
            _annonce = null;
            return a;
        }

        public void Dispose()
        {
            _minuteur?.Dispose();
            _verrou.Dispose();
        }
    }
}
