using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>Messages clavier et souris : chaque action de l'utilisateur remet à zéro le compteur d'inactivité.</summary>
    internal sealed class FiltreActivite : IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100, WM_LBUTTONDOWN = 0x0201, WM_RBUTTONDOWN = 0x0204, WM_MOUSEWHEEL = 0x020A, WM_MOUSEMOVE = 0x0200;
        private readonly Action _activite;
        private Point _dernierePosition = Cursor.Position;

        public FiltreActivite(Action activite) => _activite = activite;

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MOUSEWHEEL:
                    _activite();
                    break;
                case WM_MOUSEMOVE:
                    // Windows renvoie parfois des déplacements sans mouvement réel : on ne compte que les vrais déplacements
                    var p = Cursor.Position;
                    if (p != _dernierePosition) { _dernierePosition = p; _activite(); }
                    break;
            }
            return false;
        }
    }

    /// <summary>
    /// Verrouillage automatique d'une page principale après inactivité (délai réglable dans Administration, 0 = désactivé)
    /// et verrouillage volontaire (Ctrl+L). L'écran de verrouillage couvre tout l'écran, y compris une vente en cours de saisie,
    /// qui est conservée telle quelle dessous.
    /// </summary>
    public sealed class VerrouillageAuto : IDisposable
    {
        /// <summary>Verrouillage de la page ouverte (null hors des pages principales).</summary>
        public static VerrouillageAuto? Courant { get; private set; }

        private readonly Form _page;
        private readonly IPageSession _session;
        private readonly System.Windows.Forms.Timer _minuteur = new() { Interval = 1000 };
        private readonly FiltreActivite _filtre;
        private FormVerrouillage? _ecran;

        public SuiviInactivite Suivi { get; } = new();

        /// <summary>Faux uniquement pour les tests : l'écran de verrouillage s'affiche sans bloquer.</summary>
        public bool Modal { get; set; } = true;
        public bool EstVerrouille => _ecran != null;
        public FormVerrouillage? EcranActuel => _ecran;

        /// <summary>Verrouillage volontaire (bouton du menu, Ctrl+L).</summary>
        public void VerrouillerMaintenant() => Verrouiller(Modal);

        public VerrouillageAuto(Form page, IPageSession session)
        {
            _page = page;
            _session = session;
            _filtre = new FiltreActivite(Suivi.Activite);
            Application.AddMessageFilter(_filtre);
            _minuteur.Tick += (s, e) => Verifier();
            _minuteur.Start();
            Courant = this;
            page.FormClosed += (s, e) => Dispose();
            page.Disposed += (s, e) => Dispose();
        }

        /// <summary>Appelé chaque seconde : verrouille si le délai d'inactivité est dépassé.</summary>
        public void Verifier()
        {
            if (EstVerrouille || _page.IsDisposed) return;
            if (Suivi.DoitVerrouiller(ParametresApp.Actuels.DelaiVerrouillageMinutes))
                Verrouiller(Modal);
        }

        /// <summary>Verrouille maintenant. <paramref name="modal"/> = faux pour les tests (l'écran s'affiche sans bloquer).</summary>
        public FormVerrouillage? Verrouiller(bool modal = true)
        {
            if (EstVerrouille || SessionUtilisateur.Courant == null) return _ecran;
            var proprietaire = Form.ActiveForm ?? _page;
            _ecran = new FormVerrouillage(SessionUtilisateur.Courant);
            _ecran.Verrouiller(proprietaire);
            Journal.Info("Écran verrouillé.");

            if (!modal)
            {
                _ecran.Show(proprietaire);
                return _ecran;
            }

            _ecran.ShowDialog(proprietaire);
            Terminer();
            return null;
        }

        /// <summary>Après fermeture de l'écran de verrouillage : déverrouillage, ou changement d'utilisateur.</summary>
        public void Terminer()
        {
            var ecran = _ecran;
            _ecran = null;
            Suivi.Activite();
            if (ecran == null) return;
            bool changer = ecran.Resultat == FormVerrouillage.Issue.ChangerUtilisateur;
            ecran.Dispose();
            if (changer) ChangerUtilisateur();
        }

        /// <summary>Abandonne la vente en cours (confirmée à l'écran de verrouillage) puis déconnecte.</summary>
        private void ChangerUtilisateur()
        {
            foreach (var f in Application.OpenForms.OfType<FormVente>().ToList())
                f.AbandonnerEtFermer();
            _session.Deconnecter();
        }

        /// <summary>Vrai si une fenêtre de vente contient des produits non validés.</summary>
        public static bool VenteEnCours() => Application.OpenForms.OfType<FormVente>().Any(f => f.AVenteEnCours);

        public void Dispose()
        {
            _minuteur.Stop();
            _minuteur.Dispose();
            Application.RemoveMessageFilter(_filtre);
            if (ReferenceEquals(Courant, this)) Courant = null;
        }
    }
}
