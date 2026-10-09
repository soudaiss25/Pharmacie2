using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    /// <summary>Page « Administration » : utilisateurs, sauvegarde des données, dossier des sauvegardes.</summary>
    public partial class Uc_Administration : UserControl
    {
        public event Action? UtilisateursDemande;
        public event Action? SauvegardeDemande;
        public event Action? DossierDemande;

        public Uc_Administration()
        {
            InitializeComponent();
            Theme.Appliquer(this);
        }

        private void btnUtilisateurs_Click(object sender, EventArgs e) => UtilisateursDemande?.Invoke();
        private void btnSauvegarde_Click(object sender, EventArgs e) => SauvegardeDemande?.Invoke();
        private void btnDossier_Click(object sender, EventArgs e) => DossierDemande?.Invoke();
    }
}
