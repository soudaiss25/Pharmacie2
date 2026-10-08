using System;
using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>« Mot de passe oublié ? » : login + clé de secours → nouveau mot de passe → nouvelle clé de secours.</summary>
    public class FormReinitialisationMdp : Form
    {
        private readonly TextBox _txtLogin = new TextBox();
        private readonly TextBox _txtCle = new TextBox();
        private readonly TextBox _txtMdp = new TextBox();
        private readonly TextBox _txtMdp2 = new TextBox();

        public FormReinitialisationMdp(string loginInitial = "")
        {
            Text = "Mot de passe oublié";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 330);
            Font = new Font("Segoe UI", 10F);

            int y = 16;
            void Champ(string libelle, TextBox t, bool secret = false)
            {
                Controls.Add(new Label { Text = libelle, Location = new Point(20, y), Size = new Size(400, 22) });
                t.Location = new Point(20, y + 24);
                t.Size = new Size(400, 28);
                if (secret) t.UseSystemPasswordChar = true;
                Controls.Add(t);
                y += 62;
            }

            Champ("Login administrateur", _txtLogin);
            Champ("Clé de secours (ex : K7MQ-4XRT-9WPA-H3ZD)", _txtCle);
            Champ("Nouveau mot de passe", _txtMdp, true);
            Champ("Confirmer le nouveau mot de passe", _txtMdp2, true);
            _txtLogin.Text = loginInitial;

            var ok = new Button { Text = "Valider", Location = new Point(190, 284), Size = new Size(110, 32) };
            var annuler = new Button { Text = "Annuler", Location = new Point(310, 284), Size = new Size(110, 32), DialogResult = DialogResult.Cancel };
            ok.Click += (s, e) => Valider();
            Controls.AddRange(new Control[] { ok, annuler });
            AcceptButton = ok;
            CancelButton = annuler;
        }

        private void Valider()
        {
            if (string.IsNullOrWhiteSpace(_txtLogin.Text) || string.IsNullOrWhiteSpace(_txtCle.Text) || string.IsNullOrWhiteSpace(_txtMdp.Text))
            {
                MessageBox.Show("Remplissez tous les champs.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_txtMdp.Text.Trim() != _txtMdp2.Text.Trim())
            {
                MessageBox.Show("Les deux mots de passe ne sont pas identiques.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtMdp2.Focus();
                return;
            }

            try
            {
                int userId = CleSecoursService.Reinitialiser(_txtLogin.Text, _txtCle.Text, _txtMdp.Text);

                MessageBox.Show("Votre mot de passe a été modifié.\n\nUne nouvelle clé de secours va s'afficher : l'ancienne n'est plus valable.",
                    "Mot de passe modifié", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormCleSecours.GenererAfficherEtEnregistrer(this, userId);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (CleSecoursBloqueeException ex)
            {
                MessageBox.Show(ex.Message, "Fonction bloquée", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CleSecoursInvalideException ex)
            {
                MessageBox.Show(ex.Message, "Clé incorrecte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCle.Clear();
                _txtCle.Focus();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Réinitialisation du mot de passe par clé de secours", ex);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
