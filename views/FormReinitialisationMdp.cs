using System;
using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>« Mot de passe oublié ? » : login + clé de secours → nouveau mot de passe → nouvelle clé de secours.</summary>
    public partial class FormReinitialisationMdp : Form
    {
        public FormReinitialisationMdp(string loginInitial = "")
        {
            InitializeComponent();
            Theme.Appliquer(this);
            txtLogin.Text = loginInitial;
        }

        private void btnValider_Click(object sender, EventArgs e) => Valider();

        private void Valider()
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtCle.Text) || string.IsNullOrWhiteSpace(txtMdp.Text))
            {
                Messages.Afficher("Remplissez tous les champs.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMdp.Text.Trim() != txtMdp2.Text.Trim())
            {
                Messages.Afficher("Les deux mots de passe ne sont pas identiques.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMdp2.Focus();
                return;
            }

            try
            {
                int userId = CleSecoursService.Reinitialiser(txtLogin.Text, txtCle.Text, txtMdp.Text);

                Messages.Afficher("Votre mot de passe a été modifié.\n\nUne nouvelle clé de secours va s'afficher : l'ancienne n'est plus valable.",
                    "Mot de passe modifié", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormCleSecours.GenererAfficherEtEnregistrer(this, userId);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (CleSecoursBloqueeException ex)
            {
                Messages.Afficher(ex.Message, "Fonction bloquée", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CleSecoursInvalideException ex)
            {
                Messages.Afficher(ex.Message, "Clé incorrecte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCle.Clear();
                txtCle.Focus();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Réinitialisation du mot de passe par clé de secours", ex);
                Messages.Afficher("La réinitialisation n'a pas pu être faite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
