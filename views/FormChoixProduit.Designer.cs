using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormChoixProduit
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitre, lblPrixUnit;
        private TextBox txtRecherche;
        private DataGridView dgvProduits;
        private Label lblUnite, lblQte;
        private ComboBox cbUnite;
        private NumericUpDown numQuantite;
        private Button btnValider, btnAnnuler;

        // ── Panneau posologie ─────────────────────────────────────────────
        private Panel pnlPosologie;
        private Label lblPosologieTitre;
        private Label lblIndicationVal;
        private Label lblPosologieVal;
        private Label lblNbFoisVal;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            txtRecherche = new TextBox();
            dgvProduits = new DataGridView();
            lblUnite = new Label(); cbUnite = new ComboBox();
            lblQte = new Label(); numQuantite = new NumericUpDown();
            lblPrixUnit = new Label();
            btnValider = new Button(); btnAnnuler = new Button();
            pnlPosologie = new Panel();
            lblPosologieTitre = new Label();
            lblIndicationVal = new Label();
            lblPosologieVal = new Label();
            lblNbFoisVal = new Label();

            ((System.ComponentModel.ISupportInitialize)dgvProduits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numQuantite).BeginInit();
            SuspendLayout();

            // ── En-tête ───────────────────────────────────────────────────
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Height = 55;
            lblTitre.Text = "CHOISIR UN MÉDICAMENT";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.BackColor = Color.ForestGreen;
            lblTitre.ForeColor = Color.White;

            // ── Recherche ─────────────────────────────────────────────────
            txtRecherche.Location = new Point(12, 65);
            txtRecherche.Size = new Size(756, 26);
            txtRecherche.PlaceholderText = "🔍 Rechercher par nom…";
            txtRecherche.TextChanged += txtRecherche_TextChanged;

            // ── Grille produits ───────────────────────────────────────────
            dgvProduits.Location = new Point(12, 100);
            dgvProduits.Size = new Size(756, 220);
            dgvProduits.ReadOnly = true;
            dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduits.MultiSelect = false;
            dgvProduits.AllowUserToAddRows = false;
            dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduits.BackgroundColor = Color.White;
            dgvProduits.BorderStyle = BorderStyle.None;
            dgvProduits.RowTemplate.Height = 30;
            dgvProduits.ColumnHeadersHeight = 34;
            dgvProduits.ColumnHeadersDefaultCellStyle.BackColor = Color.ForestGreen;
            dgvProduits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduits.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProduits.EnableHeadersVisualStyles = false;
            dgvProduits.Font = new Font("Segoe UI", 9.5F);
            dgvProduits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            dgvProduits.SelectionChanged += dgvProduits_SelectionChanged;

            // ── Panneau posologie — s'affiche quand un produit est sélectionné
            pnlPosologie.Location = new Point(12, 330);
            pnlPosologie.Size = new Size(756, 110);
            pnlPosologie.BackColor = Color.FromArgb(232, 245, 233);
            pnlPosologie.BorderStyle = BorderStyle.FixedSingle;
            pnlPosologie.Visible = false;

            lblPosologieTitre.Text = "💊  Informations sur ce médicament";
            lblPosologieTitre.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPosologieTitre.ForeColor = Color.FromArgb(27, 94, 32);
            lblPosologieTitre.Location = new Point(10, 8);
            lblPosologieTitre.Size = new Size(730, 20);

            lblIndicationVal.Text = "";
            lblIndicationVal.Font = new Font("Segoe UI", 9.5F);
            lblIndicationVal.ForeColor = Color.FromArgb(40, 40, 40);
            lblIndicationVal.Location = new Point(10, 32);
            lblIndicationVal.Size = new Size(730, 20);

            lblPosologieVal.Text = "";
            lblPosologieVal.Font = new Font("Segoe UI", 9.5F);
            lblPosologieVal.ForeColor = Color.FromArgb(40, 40, 40);
            lblPosologieVal.Location = new Point(10, 56);
            lblPosologieVal.Size = new Size(730, 20);

            lblNbFoisVal.Text = "";
            lblNbFoisVal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblNbFoisVal.ForeColor = Color.FromArgb(25, 118, 210);
            lblNbFoisVal.Location = new Point(10, 80);
            lblNbFoisVal.Size = new Size(730, 20);

            pnlPosologie.Controls.AddRange(new Control[] {
                lblPosologieTitre, lblIndicationVal, lblPosologieVal, lblNbFoisVal });

            // ── Unité + quantité + prix ───────────────────────────────────
            int yCtrl = 452;

            lblUnite.Text = "Unité :";
            lblUnite.Location = new Point(12, yCtrl + 4);
            lblUnite.Size = new Size(50, 22);

            cbUnite.Location = new Point(65, yCtrl);
            cbUnite.Size = new Size(140, 28);
            cbUnite.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUnite.SelectedIndexChanged += cbUnite_SelectedIndexChanged;

            lblQte.Text = "Quantité :";
            lblQte.Location = new Point(220, yCtrl + 4);
            lblQte.Size = new Size(70, 22);

            numQuantite.Location = new Point(295, yCtrl);
            numQuantite.Size = new Size(90, 28);
            numQuantite.Minimum = 1;
            numQuantite.Maximum = 10000;

            lblPrixUnit.Text = "Prix unitaire : —";
            lblPrixUnit.Location = new Point(12, yCtrl + 36);
            lblPrixUnit.Size = new Size(756, 22);
            lblPrixUnit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrixUnit.ForeColor = Color.ForestGreen;

            // ── Boutons ───────────────────────────────────────────────────
            int yBtn = yCtrl + 68;

            btnValider.Text = "✔ Ajouter à la vente";
            btnValider.Location = new Point(12, yBtn);
            btnValider.Size = new Size(200, 40);
            btnValider.BackColor = Color.ForestGreen;
            btnValider.ForeColor = Color.White;
            btnValider.FlatStyle = FlatStyle.Flat;
            btnValider.FlatAppearance.BorderSize = 0;
            btnValider.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnValider.Click += btnValider_Click;

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(222, yBtn);
            btnAnnuler.Size = new Size(110, 40);
            btnAnnuler.BackColor = Color.Tomato;
            btnAnnuler.ForeColor = Color.White;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.FlatAppearance.BorderSize = 0;
            btnAnnuler.Font = new Font("Segoe UI", 10F);
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Assemblage ────────────────────────────────────────────────
            ((System.ComponentModel.ISupportInitialize)dgvProduits).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuantite).EndInit();

            ClientSize = new Size(780, yBtn + 56);
            Controls.AddRange(new Control[] {
                lblTitre, txtRecherche, dgvProduits,
                pnlPosologie,
                lblUnite, cbUnite, lblQte, numQuantite,
                lblPrixUnit, btnValider, btnAnnuler });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Choix médicament";
            ResumeLayout(false);
        }
    }
}