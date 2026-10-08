using System;
using System.Windows.Forms;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class PageAccueil : Form
    {
        private readonly User _user;

        public PageAccueil(User user)
        {
            _user = user;
            InitializeComponent();
        }

        private void LoadUserControl(UserControl uc)
        {
            panelContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
        }

        private void btnProduits_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Produits());

        private void btnFournisseurs_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Fournisser());

        private void btnVentes_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Vente());

        private void btn_stock_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Stock());

        private void btnMutuelles_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Mutuelle());

        private void btnStatistiques_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Statistique());

        private void btnDepenses_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Depenses());

        private void BtnCaisse_Click_1(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Caisse());

        private void BtnGestionUtilisateur_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Utilisateurs());

        // ✅ NOUVEAU — Vue globale des commandes fournisseurs
        private void btnCommandes_Click(object sender, EventArgs e)
            => LoadUserControl(new views.UserControls.Uc_Commande());

        // ✅ Sauvegarde complète CSV
        private void btnSauvegarde_Click(object sender, EventArgs e)
        {
            using var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Choisissez le dossier de sauvegarde";
            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            string dossier = System.IO.Path.Combine(
                fbd.SelectedPath,
                $"Pharmacie2_backup_{System.DateTime.Now:yyyy-MM-dd_HHmmss}");
            System.IO.Directory.CreateDirectory(dossier);

            Services.SauvegardeService.ExporterTout(dossier);

            System.Windows.Forms.MessageBox.Show(
                $"Sauvegarde effectuée dans :\n{dossier}",
                "Sauvegarde réussie",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }

        private void btnOuvrirDossierSauvegardes_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Services.CheminsApp.DossierSauvegardes,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Services.Journal.Erreur("Ouverture du dossier des sauvegardes", ex);
                MessageBox.Show("Impossible d'ouvrir le dossier des sauvegardes :\n" + Services.CheminsApp.DossierSauvegardes,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeconnexion_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new Form1();
            login.Show();
        }

        private void btnDeconnexion_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            var login = new Form1();
            login.Show();
        }

        private void lblTitre_Click(object sender, EventArgs e) { }
        private void panelContent_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void panelMenu_Paint(object sender, PaintEventArgs e) { }
    }
}