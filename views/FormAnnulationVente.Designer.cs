using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormAnnulationVente
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitre, lblInfo, lblMotifLabel;
        private ListView lvLignes;
        private TextBox txtMotif;
        private Button btnConfirmer, btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            lblInfo = new Label();
            lvLignes = new ListView();
            lblMotifLabel = new Label();
            txtMotif = new TextBox();
            btnConfirmer = new Button();
            btnAnnuler = new Button();

            this.SuspendLayout();

            lblTitre.Dock = DockStyle.Top; lblTitre.Height = 50;
            lblTitre.Text = "⚠️  ANNULATION DE VENTE"; lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.BackColor = Color.OrangeRed; lblTitre.ForeColor = Color.White;

            lblInfo.Location = new Point(12, 60); lblInfo.Size = new Size(560, 80);
            lblInfo.Font = new Font("Segoe UI", 9F); lblInfo.ForeColor = Color.FromArgb(50, 50, 50);

            lvLignes.Location = new Point(12, 148); lvLignes.Size = new Size(560, 130);
            lvLignes.View = View.Details; lvLignes.FullRowSelect = true; lvLignes.GridLines = true;
            lvLignes.Columns.Add("Produit", 260);
            lvLignes.Columns.Add("Unité", 100);
            lvLignes.Columns.Add("Quantité remise", 130);

            lblMotifLabel.Text = "Motif d'annulation * :";
            lblMotifLabel.Location = new Point(12, 290); lblMotifLabel.Size = new Size(180, 22);
            lblMotifLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            txtMotif.Location = new Point(12, 314); txtMotif.Size = new Size(560, 26);
            txtMotif.PlaceholderText = "Ex: Erreur de saisie, retour client, produit indisponible…";

            btnConfirmer.Text = "✔ Confirmer l'annulation";
            btnConfirmer.Location = new Point(320, 356); btnConfirmer.Size = new Size(200, 36);
            btnConfirmer.BackColor = Color.OrangeRed; btnConfirmer.ForeColor = Color.White;
            btnConfirmer.FlatStyle = FlatStyle.Flat; btnConfirmer.FlatAppearance.BorderSize = 0;
            btnConfirmer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConfirmer.Click += btnConfirmer_Click;

            btnAnnuler.Text = "Fermer";
            btnAnnuler.Location = new Point(210, 356); btnAnnuler.Size = new Size(100, 36);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Click += btnAnnuler_Click;

            this.ClientSize = new Size(586, 406);
            this.Text = "Annulation de vente";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.AddRange(new Control[] {
                lblTitre, lblInfo, lvLignes, lblMotifLabel, txtMotif,
                btnConfirmer, btnAnnuler });
            this.ResumeLayout(false);
        }
    }
}