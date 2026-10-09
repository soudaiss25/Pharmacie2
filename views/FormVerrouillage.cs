using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    /// <summary>
    /// Écran de verrouillage : plein écran, masque tout le contenu (y compris une vente en cours, conservée dessous).
    /// Mot de passe de l'utilisateur connecté, « Changer d'utilisateur », « Mot de passe oublié ? » (clé de secours).
    /// Cinq mots de passe faux de suite : 30 secondes d'attente entre les essais (journalisé).
    /// </summary>
    public partial class FormVerrouillage : Form
    {
        public enum Issue { Aucune, Deverrouille, ChangerUtilisateur }

        private readonly User _utilisateur;

        public Issue Resultat { get; private set; } = Issue.Aucune;

        /// <summary>Limite des essais (horloge injectable pour les tests).</summary>
        public LimiteEssais Limite { get; } = new();

        public FormVerrouillage(User utilisateur)
        {
            _utilisateur = utilisateur;
            InitializeComponent();
            Theme.Appliquer(this);
            BackColor = Theme.Principal;          // fond sombre : rien du contenu n'est visible
            foreach (var b in new[] { btnDeverrouiller, btnChangerUtilisateur })
            {
                b.AutoSize = false;               // les deux boutons prennent toute la largeur de la carte
                b.Dock = DockStyle.Fill;
                b.Height = Theme.Px(this, 38);
            }
            lnkOubli.LinkColor = Theme.Accent;
            lnkOubli.ActiveLinkColor = Theme.Principal;
            lblUtilisateur.Text = $"{utilisateur.Prenom} {utilisateur.Nom}";
            FormClosing += (s, e) =>
            {
                if (Resultat == Issue.Aucune && e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = true;              // Alt+F4 ne contourne pas le verrouillage
            };
            Shown += (s, e) => txtMotDePasse.Focus();
        }

        /// <summary>Couvre tout l'écran de la fenêtre propriétaire, au premier plan.</summary>
        public void Verrouiller(Form proprietaire)
        {
            StartPosition = FormStartPosition.Manual;
            Bounds = Screen.FromControl(proprietaire).Bounds;
            TopMost = true;
        }

        /// <summary>Teste le mot de passe saisi ; renvoie vrai et ferme l'écran s'il est bon.</summary>
        public bool Essayer(string motDePasse)
        {
            if (!Limite.PeutEssayer(out int secondes))
            {
                lblMessage.Text = $"Trop d'essais. Patientez {Format.Compte(secondes, "seconde")} avant de réessayer.";
                return false;
            }

            var resultat = AuthentificationService.Authentifier(_utilisateur.Login, motDePasse, out var user);
            if (resultat == ResultatConnexion.Succes && user != null && user.Id == _utilisateur.Id)
            {
                Limite.EnregistrerReussite();
                Resultat = Issue.Deverrouille;
                DialogResult = DialogResult.OK;
                Close();
                return true;
            }

            Limite.EnregistrerEchec(_utilisateur.Login);
            txtMotDePasse.Clear();
            txtMotDePasse.Focus();
            lblMessage.Text = Limite.PeutEssayer(out int attente)
                ? "Mot de passe incorrect."
                : $"Mot de passe incorrect. Patientez {Format.Compte(attente, "seconde")} avant de réessayer.";
            return false;
        }

        private void btnDeverrouiller_Click(object sender, EventArgs e) => Essayer(txtMotDePasse.Text);

        private void btnChangerUtilisateur_Click(object sender, EventArgs e)
        {
            if (VerrouillageAuto.VenteEnCours() &&
                MessageBox.Show(this, "Une vente est en cours : elle sera abandonnée. Continuer ?", "Changer d'utilisateur",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Resultat = Issue.ChangerUtilisateur;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lnkOubli_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var f = new FormReinitialisationMdp(_utilisateur.Login);
            TopMost = false;
            try
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    lblMessage.Text = "Mot de passe modifié : saisissez-le pour reprendre.";
                    txtMotDePasse.Clear();
                    txtMotDePasse.Focus();
                }
            }
            finally { TopMost = true; }
        }
    }
}
