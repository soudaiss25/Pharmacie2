using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormChoixProduit
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitre;
        private Label lblPrixUnit;
        private Label lblInfo;
        private TextBox txtRecherche;
        private DataGridView dgvProduits;
        private Label lblUnite;
        private Label lblQte;
        private ComboBox cbUnite;
        private NumericUpDown numQuantite;
        private Button btnValider;
        private Button btnAnnuler;

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
            lblUnite = new Label();
            cbUnite = new ComboBox();
            lblQte = new Label();
            numQuantite = new NumericUpDown();
            lblPrixUnit = new Label();
            lblInfo = new Label();
            btnValider = new Button();
            btnAnnuler = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvProduits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numQuantite).BeginInit();
            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Height = 50;
            lblTitre.Text = "💊  CHOISIR UN MÉDICAMENT";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.BackColor = Color.ForestGreen;
            lblTitre.ForeColor = Color.White;

            // ── Recherche ──────────────────────────────────────────────────
            txtRecherche.Location = new Point(12, 60);
            txtRecherche.Size = new Size(546, 26);
            txtRecherche.PlaceholderText = "🔍 Rechercher par nom…";
            txtRecherche.Font = new Font("Segoe UI", 9.5F);
            txtRecherche.TextChanged += txtRecherche_TextChanged;

            // ── Grille produits ────────────────────────────────────────────
            dgvProduits.Location = new Point(12, 92);
            dgvProduits.Size = new Size(546, 210);
            dgvProduits.ReadOnly = true;
            dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduits.MultiSelect = false;
            dgvProduits.AllowUserToAddRows = false;
            dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduits.BackgroundColor = Color.White;
            dgvProduits.BorderStyle = BorderStyle.None;
            dgvProduits.Font = new Font("Segoe UI", 9F);
            dgvProduits.RowTemplate.Height = 30;
            dgvProduits.ColumnHeadersHeight = 34;
            dgvProduits.ColumnHeadersDefaultCellStyle.BackColor = Color.ForestGreen;
            dgvProduits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduits.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProduits.EnableHeadersVisualStyles = false;
            dgvProduits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            dgvProduits.SelectionChanged += dgvProduits_SelectionChanged;

            // ── Unité de vente ─────────────────────────────────────────────
            lblUnite.Text = "Unité :";
            lblUnite.Location = new Point(12, 314);
            lblUnite.Size = new Size(55, 22);
            lblUnite.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cbUnite.Location = new Point(70, 311);
            cbUnite.Size = new Size(150, 28);
            cbUnite.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUnite.Font = new Font("Segoe UI", 9F);
            cbUnite.SelectedIndexChanged += cbUnite_SelectedIndexChanged;

            // ── Quantité ───────────────────────────────────────────────────
            lblQte.Text = "Quantité :";
            lblQte.Location = new Point(232, 314);
            lblQte.Size = new Size(70, 22);
            lblQte.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            numQuantite.Location = new Point(310, 311);
            numQuantite.Size = new Size(90, 28);
            numQuantite.Minimum = 1;
            numQuantite.Maximum = 10000;
            numQuantite.Value = 1;
            numQuantite.Font = new Font("Segoe UI", 10F);
            numQuantite.ValueChanged += numQuantite_ValueChanged;

            // ── Prix unitaire ──────────────────────────────────────────────
            lblPrixUnit.Text = "Prix unitaire : —";
            lblPrixUnit.Location = new Point(12, 347);
            lblPrixUnit.Size = new Size(546, 22);
            lblPrixUnit.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPrixUnit.ForeColor = Color.ForestGreen;

            // ── Info stock / boîtes consommées (NOUVEAU) ───────────────────
            lblInfo.Location = new Point(12, 372);
            lblInfo.Size = new Size(546, 42);
            lblInfo.Font = new Font("Segoe UI", 8.5F);
            lblInfo.ForeColor = Color.FromArgb(27, 94, 32);
            lblInfo.Text = "";

            // ── Boutons ────────────────────────────────────────────────────
            btnValider.Text = "✔ Ajouter à la vente";
            btnValider.Location = new Point(12, 422);
            btnValider.Size = new Size(180, 38);
            btnValider.BackColor = Color.ForestGreen;
            btnValider.ForeColor = Color.White;
            btnValider.FlatStyle = FlatStyle.Flat;
            btnValider.FlatAppearance.BorderSize = 0;
            btnValider.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnValider.Click += btnValider_Click;

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(202, 422);
            btnAnnuler.Size = new Size(100, 38);
            btnAnnuler.BackColor = Color.Tomato;
            btnAnnuler.ForeColor = Color.White;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.FlatAppearance.BorderSize = 0;
            btnAnnuler.Font = new Font("Segoe UI", 9F);
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Form ───────────────────────────────────────────────────────
            ClientSize = new Size(572, 476);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            Text = "Choix médicament";
            BackColor = Color.White;

            Controls.AddRange(new Control[] {
                lblTitre, txtRecherche, dgvProduits,
                lblUnite, cbUnite,
                lblQte, numQuantite,
                lblPrixUnit, lblInfo,
                btnValider, btnAnnuler
            });

            ((System.ComponentModel.ISupportInitialize)dgvProduits).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuantite).EndInit();
            ResumeLayout(false);
        }
    }
}