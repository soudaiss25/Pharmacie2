using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Saisie de la phrase secrète des sauvegardes externes. Avec confirmation (première configuration) ou simple (restauration).
    /// La phrase n'est jamais enregistrée ni journalisée par cette fenêtre.
    /// </summary>
    public partial class FormPhraseSecrete : Form
    {
        private readonly bool _confirmer;

        public string Phrase => txtPhrase.Text;

        public FormPhraseSecrete(bool confirmer)
        {
            _confirmer = confirmer;
            InitializeComponent();
            Theme.Appliquer(this);
            tlpConfirmation.Visible = confirmer;
            if (confirmer)
            {
                lblTitre.Text = "Choisissez votre phrase secrète";
                lblInfo.Text = "Elle protège vos copies de sauvegarde : sans elle, personne (pas même le développeur) ne peut les lire. " +
                               "Notez-la sur papier et rangez-la en lieu sûr. Au moins 8 caractères ; une phrase de plusieurs mots est idéale.";
            }
            else
            {
                lblTitre.Text = "Phrase secrète de la sauvegarde";
                lblInfo.Text = "Saisissez la phrase secrète choisie lors de la configuration des sauvegardes externes.";
                ClientSize = new Size(ClientSize.Width, ClientSize.Height - 80);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtPhrase.Text.Length < (_confirmer ? 8 : 1))
            {
                MessageBox.Show(_confirmer ? "La phrase secrète doit contenir au moins 8 caractères." : "Saisissez la phrase secrète.",
                    "Phrase secrète", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhrase.Focus();
                return;
            }
            if (_confirmer && txtPhrase.Text != txtConfirmation.Text)
            {
                MessageBox.Show("Les deux saisies ne sont pas identiques.", "Phrase secrète", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmation.Clear();
                txtConfirmation.Focus();
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
