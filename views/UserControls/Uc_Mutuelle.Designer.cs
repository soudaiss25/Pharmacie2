using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Mutuelle
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Déclarations ──────────────────────────────────────────────
            pnlHeader = new Panel();
            lblTitre = new Label();
            pnlToolbar = new Panel();
            btnNouvelleMutuelle = new Button();
            btnModifier = new Button();
            btnSupprimer = new Button();
            btnExportExcel = new Button();
            btnActualiser = new Button();

            // Panel haut (liste mutuelles)
            pnlHaut = new Panel();
            dgvMutuelles = new DataGridView();

            // Splitter entre haut et bas
            splitter = new Splitter();

            // Panel bas (ventes impayées)
            pnlBas = new Panel();
            pnlImpayesHeader = new Panel();
            lblRecapImpaye = new Label();
            btnReglertout = new Button();
            btnReglerSelection = new Button();
            dgvVentesImpayees = new DataGridView();

            // Colonnes grille ventes impayées
            colCoche = new DataGridViewCheckBoxColumn();
            colVenteId = new DataGridViewTextBoxColumn();
            colNumeroVente = new DataGridViewTextBoxColumn();
            colDateVente = new DataGridViewTextBoxColumn();
            colClient = new DataGridViewTextBoxColumn();
            colMatricule = new DataGridViewTextBoxColumn();
            colMontantTotal = new DataGridViewTextBoxColumn();
            colMontantMutuelle = new DataGridViewTextBoxColumn();
            colVendeur = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dgvMutuelles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVentesImpayees).BeginInit();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlHaut.SuspendLayout();
            pnlBas.SuspendLayout();
            pnlImpayesHeader.SuspendLayout();
            SuspendLayout();

            // ══════════════════════════════════════════════════════════════
            // EN-TÊTE
            // ══════════════════════════════════════════════════════════════
            lblTitre.AutoSize = false;
            lblTitre.Dock = DockStyle.Fill;
            lblTitre.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Text = "🏢  Gestion des Mutuelles";
            lblTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblTitre.Padding = new Padding(14, 0, 0, 0);

            pnlHeader.BackColor = Color.FromArgb(46, 100, 46);
            pnlHeader.Controls.Add(lblTitre);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 55;
            pnlHeader.Name = "pnlHeader";

            // ══════════════════════════════════════════════════════════════
            // BARRE OUTILS
            // ══════════════════════════════════════════════════════════════
            void StyleBtn(Button b, string txt, Color bg, int x, int w = 130)
            {
                b.Text = txt;
                b.Location = new Point(x, 9);
                b.Size = new Size(w, 32);
                b.BackColor = bg;
                b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.UseVisualStyleBackColor = false;
            }

            StyleBtn(btnNouvelleMutuelle, "➕ Ajouter", Color.FromArgb(0, 150, 136), 10, 120);
            StyleBtn(btnModifier, "✏️ Modifier", Color.FromArgb(33, 150, 243), 140, 110);
            StyleBtn(btnSupprimer, "🗑️ Supprimer", Color.FromArgb(211, 47, 47), 260, 110);
            StyleBtn(btnExportExcel, "📊 Export Excel", Color.FromArgb(56, 142, 60), 380, 140);
            StyleBtn(btnActualiser, "🔄 Actualiser", Color.FromArgb(69, 90, 100), 530, 120);

            btnNouvelleMutuelle.Click += btnNouvelleMutuelle_Click;
            btnModifier.Click += btnModifier_Click;
            btnSupprimer.Click += btnSupprimer_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            pnlToolbar.BackColor = Color.FromArgb(245, 250, 245);
            pnlToolbar.Controls.AddRange(new Control[] {
                btnNouvelleMutuelle, btnModifier, btnSupprimer,
                btnExportExcel, btnActualiser });
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Height = 50;
            pnlToolbar.Name = "pnlToolbar";

            // ══════════════════════════════════════════════════════════════
            // PANEL HAUT — Liste des mutuelles
            // ══════════════════════════════════════════════════════════════
            dgvMutuelles.AllowUserToAddRows = false;
            dgvMutuelles.AllowUserToDeleteRows = false;
            dgvMutuelles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMutuelles.BackgroundColor = Color.White;
            dgvMutuelles.BorderStyle = BorderStyle.None;
            dgvMutuelles.ColumnHeadersHeight = 38;
            dgvMutuelles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
            dgvMutuelles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMutuelles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMutuelles.Dock = DockStyle.Fill;
            dgvMutuelles.EnableHeadersVisualStyles = false;
            dgvMutuelles.Font = new Font("Segoe UI", 9.5F);
            dgvMutuelles.MultiSelect = false;
            dgvMutuelles.Name = "dgvMutuelles";
            dgvMutuelles.ReadOnly = true;
            dgvMutuelles.RowHeadersVisible = false;
            dgvMutuelles.RowTemplate.Height = 34;
            dgvMutuelles.ScrollBars = ScrollBars.Both;
            dgvMutuelles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMutuelles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);

            pnlHaut.Controls.Add(dgvMutuelles);
            pnlHaut.Dock = DockStyle.Top;
            pnlHaut.Height = 250;          // hauteur fixe — pas de SplitterDistance
            pnlHaut.MinimumSize = new Size(0, 120);
            pnlHaut.Name = "pnlHaut";

            // ── Splitter entre haut et bas ────────────────────────────────
            splitter.Dock = DockStyle.Top;
            splitter.Height = 5;
            splitter.BackColor = Color.FromArgb(200, 200, 200);
            splitter.Name = "splitter";

            // ══════════════════════════════════════════════════════════════
            // PANEL BAS — Ventes impayées
            // ══════════════════════════════════════════════════════════════

            // En-tête impayés
            lblRecapImpaye.AutoSize = false;
            lblRecapImpaye.Location = new Point(8, 8);
            lblRecapImpaye.Size = new Size(560, 34);
            lblRecapImpaye.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblRecapImpaye.ForeColor = Color.FromArgb(27, 94, 32);
            lblRecapImpaye.Text = "← Sélectionnez une mutuelle pour voir ses ventes impayées";
            lblRecapImpaye.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblRecapImpaye.Name = "lblRecapImpaye";

            btnReglertout.Text = "✅ Régler tout";
            btnReglertout.Location = new Point(580, 8);
            btnReglertout.Size = new Size(150, 34);
            btnReglertout.BackColor = Color.FromArgb(46, 125, 50);
            btnReglertout.ForeColor = Color.White;
            btnReglertout.FlatStyle = FlatStyle.Flat;
            btnReglertout.FlatAppearance.BorderSize = 0;
            btnReglertout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReglertout.Enabled = false;
            btnReglertout.Name = "btnReglertout";
            btnReglertout.UseVisualStyleBackColor = false;

            btnReglerSelection.Text = "☑️ Régler sélection";
            btnReglerSelection.Location = new Point(740, 8);
            btnReglerSelection.Size = new Size(160, 34);
            btnReglerSelection.BackColor = Color.FromArgb(25, 118, 210);
            btnReglerSelection.ForeColor = Color.White;
            btnReglerSelection.FlatStyle = FlatStyle.Flat;
            btnReglerSelection.FlatAppearance.BorderSize = 0;
            btnReglerSelection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReglerSelection.Enabled = false;
            btnReglerSelection.Name = "btnReglerSelection";
            btnReglerSelection.UseVisualStyleBackColor = false;

            pnlImpayesHeader.BackColor = Color.FromArgb(255, 243, 205);
            pnlImpayesHeader.Controls.Add(lblRecapImpaye);
            pnlImpayesHeader.Controls.Add(btnReglertout);
            pnlImpayesHeader.Controls.Add(btnReglerSelection);
            pnlImpayesHeader.Dock = DockStyle.Top;
            pnlImpayesHeader.Height = 50;
            pnlImpayesHeader.Name = "pnlImpayesHeader";

            // Colonnes grille ventes impayées
            colCoche.Name = "colCoche"; colCoche.HeaderText = "✔"; colCoche.Width = 40; colCoche.ReadOnly = false; colCoche.FillWeight = 20F;
            colVenteId.Name = "colVenteId"; colVenteId.HeaderText = "ID"; colVenteId.Visible = false;
            colNumeroVente.Name = "colNumeroVente"; colNumeroVente.HeaderText = "N° Vente"; colNumeroVente.ReadOnly = true; colNumeroVente.FillWeight = 60F;
            colDateVente.Name = "colDateVente"; colDateVente.HeaderText = "Date"; colDateVente.ReadOnly = true; colDateVente.FillWeight = 80F;
            colClient.Name = "colClient"; colClient.HeaderText = "Client"; colClient.ReadOnly = true; colClient.FillWeight = 100F;
            colMatricule.Name = "colMatricule"; colMatricule.HeaderText = "Matricule"; colMatricule.ReadOnly = true; colMatricule.FillWeight = 60F;
            colMontantTotal.Name = "colMontantTotal"; colMontantTotal.HeaderText = "Total vente"; colMontantTotal.ReadOnly = true; colMontantTotal.FillWeight = 70F;
            colMontantMutuelle.Name = "colMontantMutuelle"; colMontantMutuelle.HeaderText = "Part entreprise (dû)"; colMontantMutuelle.ReadOnly = true; colMontantMutuelle.FillWeight = 80F;
            colMontantMutuelle.DefaultCellStyle.ForeColor = Color.OrangeRed;
            colMontantMutuelle.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colVendeur.Name = "colVendeur"; colVendeur.HeaderText = "Vendeur"; colVendeur.ReadOnly = true; colVendeur.FillWeight = 70F;

            dgvVentesImpayees.AllowUserToAddRows = false;
            dgvVentesImpayees.AllowUserToDeleteRows = false;
            dgvVentesImpayees.AutoGenerateColumns = false;
            dgvVentesImpayees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentesImpayees.BackgroundColor = Color.White;
            dgvVentesImpayees.BorderStyle = BorderStyle.None;
            dgvVentesImpayees.ColumnHeadersHeight = 36;
            dgvVentesImpayees.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(211, 84, 0);
            dgvVentesImpayees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVentesImpayees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVentesImpayees.Columns.AddRange(new DataGridViewColumn[] {
                colCoche, colVenteId, colNumeroVente, colDateVente,
                colClient, colMatricule, colMontantTotal, colMontantMutuelle, colVendeur });
            dgvVentesImpayees.Dock = DockStyle.Fill;
            dgvVentesImpayees.EnableHeadersVisualStyles = false;
            dgvVentesImpayees.Font = new Font("Segoe UI", 9F);
            dgvVentesImpayees.MultiSelect = true;
            dgvVentesImpayees.Name = "dgvVentesImpayees";
            dgvVentesImpayees.ReadOnly = false;
            dgvVentesImpayees.RowHeadersVisible = false;
            dgvVentesImpayees.RowTemplate.Height = 32;
            dgvVentesImpayees.ScrollBars = ScrollBars.Both;
            dgvVentesImpayees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentesImpayees.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 250, 240);

            // Ordre critique : Fill doit être ajouté en dernier dans pnlBas
            pnlBas.Controls.Add(dgvVentesImpayees);
            pnlBas.Controls.Add(pnlImpayesHeader);
            pnlBas.Dock = DockStyle.Fill;
            pnlBas.Name = "pnlBas";

            // ══════════════════════════════════════════════════════════════
            // ASSEMBLAGE FINAL
            // Ordre : Fill en dernier, Top avant, Bottom avant Fill
            // ══════════════════════════════════════════════════════════════
            BackColor = Color.White;
            Size = new Size(1100, 700);
            Name = "Uc_Mutuelle";

            // Ordre d'ajout des contrôles Docked :
            // Top s'empile dans l'ordre d'ajout
            // Fill prend tout l'espace restant → doit être ajouté EN DERNIER
            Controls.Add(pnlBas);        // Fill  → ajouté en premier pour être "réservé" en dernier
            Controls.Add(splitter);      // Top
            Controls.Add(pnlHaut);       // Top
            Controls.Add(pnlToolbar);    // Top
            Controls.Add(pnlHeader);     // Top

            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlImpayesHeader.ResumeLayout(false);
            pnlHaut.ResumeLayout(false);
            pnlBas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMutuelles).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVentesImpayees).EndInit();
            ResumeLayout(false);
        }

        // ── Champs ────────────────────────────────────────────────────────
        private Panel pnlHeader;
        private Label lblTitre;
        private Panel pnlToolbar;
        private Button btnNouvelleMutuelle;
        private Button btnModifier;
        private Button btnSupprimer;
        private Button btnExportExcel;
        private Button btnActualiser;

        // Partie haute
        private Panel pnlHaut;
        private DataGridView dgvMutuelles;

        // Splitter
        private Splitter splitter;

        // Partie basse
        private Panel pnlBas;
        private Panel pnlImpayesHeader;
        private Label lblRecapImpaye;
        private Button btnReglertout;
        private Button btnReglerSelection;
        private DataGridView dgvVentesImpayees;

        // Colonnes grille impayées
        private DataGridViewCheckBoxColumn colCoche;
        private DataGridViewTextBoxColumn colVenteId;
        private DataGridViewTextBoxColumn colNumeroVente;
        private DataGridViewTextBoxColumn colDateVente;
        private DataGridViewTextBoxColumn colClient;
        private DataGridViewTextBoxColumn colMatricule;
        private DataGridViewTextBoxColumn colMontantTotal;
        private DataGridViewTextBoxColumn colMontantMutuelle;
        private DataGridViewTextBoxColumn colVendeur;
    }
}