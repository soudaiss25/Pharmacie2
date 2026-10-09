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
    public partial class FormCleSecours : Form
    {
        private readonly string _cle;

        public FormCleSecours(string cle)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _cle = cle;
            lblCle.Text = cle;
            lblCle.BackColor = Theme.AttentionFond;

            // Pas de fermeture possible tant que la case n'est pas cochée
            FormClosing += (s, e) =>
            {
                if (DialogResult != DialogResult.OK)
                    e.Cancel = true;
            };
        }

        private void chkNote_CheckedChanged(object sender, EventArgs e) => btnContinuer.Enabled = chkNote.Checked;

        private void btnImprimer_Click(object sender, EventArgs e) => Imprimer();

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
                    g.DrawString(AppInfo.NomPharmacie + " : clé de secours", fTitre, Brushes.Black, 60, 60);
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
