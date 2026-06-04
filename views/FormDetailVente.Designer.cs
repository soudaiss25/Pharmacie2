using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormDetailVente
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTop;
        private Label lblNumero, lblDate, lblClient, lblTel, lblMotif, lblVendeur, lblMode, lblStatut, lblMutuelle;
        private DataGridView dgvLignes;
        private Label lblTotal, lblVerse, lblRestant;
        private ListView lvPaiements;
        private Button btnFermer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblNumero = new Label(); lblDate = new Label();
            lblClient = new Label(); lblTel = new Label();
            lblMotif = new Label(); lblVendeur = new Label();
            lblMode = new Label(); lblStatut = new Label();
            lblMutuelle = new Label();
            dgvLignes = new DataGridView();
            lblTotal = new Label(); lblVerse = new Label(); lblRestant = new Label();
            lvPaiements = new ListView();
            btnFermer = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvLignes).BeginInit();
            pnlTop.SuspendLayout();
            this.SuspendLayout();

            // ── En-tête ───────────────────────────────────────────────────
            pnlTop.BackColor = Color.FromArgb(34, 85, 34); pnlTop.Dock = DockStyle.Top; pnlTop.Height = 140;

            void AddLbl(Label l, string prefix, int x, int y, int w = 380)
            {
                l.Text = prefix; l.Location = new Point(x, y); l.Size = new Size(w, 22);
                l.Font = new Font("Segoe UI", 9F); l.ForeColor = Color.White;
                pnlTop.Controls.Add(l);
            }

            AddLbl(lblNumero, "N° Vente : —", 12, 10, 200);
            AddLbl(lblDate, "Date : —", 220, 10, 200);
            AddLbl(lblStatut, "Statut : —", 430, 10, 150);
            AddLbl(lblClient, "Client : —", 12, 36, 280);
            AddLbl(lblTel, "Tél : —", 300, 36, 200);
            AddLbl(lblVendeur, "Vendeur : —", 510, 36, 250);
            AddLbl(lblMode, "Paiement : —", 12, 62, 280);
            AddLbl(lblMotif, "Motif : —", 12, 88, 750);
            AddLbl(lblMutuelle, "", 12, 112, 750);
            lblMutuelle.Visible = false;
            lblMutuelle.ForeColor = Color.LightCyan;
            lblStatut.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // ── Grille lignes ─────────────────────────────────────────────
            dgvLignes.Location = new Point(12, 150); dgvLignes.Size = new Size(766, 200);
            dgvLignes.ReadOnly = true; dgvLignes.AllowUserToAddRows = false;
            dgvLignes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLignes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLignes.BackgroundColor = Color.White; dgvLignes.BorderStyle = BorderStyle.None;
            dgvLignes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
            dgvLignes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLignes.EnableHeadersVisualStyles = false;

            // ── Totaux ────────────────────────────────────────────────────
            lblTotal.Text = "Total : —";
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(12, 360); lblTotal.Size = new Size(220, 24);

            lblVerse.Text = "Versé : —";
            lblVerse.Font = new Font("Segoe UI", 10F);
            lblVerse.Location = new Point(240, 360); lblVerse.Size = new Size(220, 24);

            lblRestant.Text = "Reste : —";
            lblRestant.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRestant.Location = new Point(470, 360); lblRestant.Size = new Size(220, 24);

            // ── Historique paiements ──────────────────────────────────────
            lvPaiements.Location = new Point(12, 392); lvPaiements.Size = new Size(766, 130);
            lvPaiements.View = View.Details; lvPaiements.FullRowSelect = true; lvPaiements.GridLines = true;
            lvPaiements.Columns.Add("N° Paiement", 130);
            lvPaiements.Columns.Add("Date", 140);
            lvPaiements.Columns.Add("Montant", 120);
            lvPaiements.Columns.Add("Encaissé par", 180);

            btnFermer.Text = "Fermer";
            btnFermer.Location = new Point(660, 532); btnFermer.Size = new Size(118, 34);
            btnFermer.FlatStyle = FlatStyle.Flat; btnFermer.BackColor = Color.FromArgb(69, 90, 100);
            btnFermer.ForeColor = Color.White; btnFermer.FlatAppearance.BorderSize = 0;
            btnFermer.Click += btnFermer_Click;

            ((System.ComponentModel.ISupportInitialize)dgvLignes).EndInit();
            pnlTop.ResumeLayout(false);

            this.ClientSize = new Size(790, 578);
            this.Text = "Détail de la vente";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.AddRange(new Control[] {
                pnlTop, dgvLignes, lblTotal, lblVerse, lblRestant,
                lvPaiements, btnFermer });
            this.ResumeLayout(false);
        }
    }
}