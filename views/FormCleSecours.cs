using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Affiche UNE SEULE FOIS la clé de secours. Impossible de fermer sans avoir coché « J'ai noté la clé ».
    /// </summary>
    public class FormCleSecours : Form
    {
        private readonly string _cle;
        private readonly CheckBox _chkNote;
        private readonly Button _btnContinuer;

        public FormCleSecours(string cle)
        {
            _cle = cle;

            Text = "Clé de secours";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(620, 400);
            Font = new Font("Segoe UI", 10F);

            var titre = new Label
            {
                Text = "🔑 Votre clé de secours",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location = new Point(20, 16),
                Size = new Size(580, 36)
            };

            var lblCle = new Label
            {
                Text = cle,
                Font = new Font("Consolas", 30F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(255, 249, 196),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(20, 64),
                Size = new Size(580, 80)
            };

            var consigne = new Label
            {
                Text = "Notez cette clé sur papier et rangez-la en lieu sûr. " +
                       "Elle permet de retrouver l'accès si vous oubliez votre mot de passe.\n\n" +
                       "Elle ne sera plus jamais affichée.",
                Location = new Point(20, 160),
                Size = new Size(580, 90)
            };

            var btnImprimer = new Button { Text = "🖨 Imprimer", Location = new Point(20, 262), Size = new Size(160, 36) };
            btnImprimer.Click += (s, e) => Imprimer();

            _chkNote = new CheckBox
            {
                Text = "J'ai noté la clé",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 316)
            };
            _chkNote.CheckedChanged += (s, e) => _btnContinuer.Enabled = _chkNote.Checked;

            _btnContinuer = new Button
            {
                Text = "Continuer",
                Enabled = false,
                Location = new Point(440, 348),
                Size = new Size(160, 36),
                DialogResult = DialogResult.OK
            };

            Controls.AddRange(new Control[] { titre, lblCle, consigne, btnImprimer, _chkNote, _btnContinuer });
            AcceptButton = _btnContinuer;

            // Pas de fermeture possible tant que la case n'est pas cochée
            FormClosing += (s, e) =>
            {
                if (DialogResult != DialogResult.OK)
                    e.Cancel = true;
            };
        }

        private void Imprimer()
        {
            try
            {
                using var pd = new PrintDocument();
                pd.PrintPage += (s, ev) =>
                {
                    using var fTitre = new Font("Segoe UI", 20F, FontStyle.Bold);
                    using var fCle = new Font("Consolas", 34F, FontStyle.Bold);
                    using var fTexte = new Font("Segoe UI", 12F);
                    var g = ev.Graphics;
                    g.DrawString("Pharmacie — clé de secours", fTitre, Brushes.Black, 60, 60);
                    g.DrawString(_cle, fCle, Brushes.Black, 60, 140);
                    g.DrawString("Rangez cette feuille en lieu sûr.\nElle permet de retrouver l'accès si vous oubliez votre mot de passe.\n" +
                                 $"Imprimée le {DateTime.Now:dd/MM/yyyy}.", fTexte, Brushes.Black, 60, 230);
                };
                using var dlg = new PrintDialog { Document = pd };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    pd.Print();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Impression de la clé de secours", ex);   // la clé elle-même n'est jamais journalisée
                MessageBox.Show("Impossible d'imprimer : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Génère une clé, l'affiche, et ne l'enregistre qu'une fois la case cochée.</summary>
        public static void GenererAfficherEtEnregistrer(IWin32Window? parent, int userId)
        {
            string cle = CleSecoursService.Generer();
            using var f = new FormCleSecours(cle);
            if (f.ShowDialog(parent) == DialogResult.OK)
                CleSecoursService.Definir(userId, cle);
        }
    }
}
