using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Commande
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        // ── Contrôles ─────────────────────────────────────────────────────
        private Panel pnlHeader;
        private Label lblTitre;
        private Panel pnlFiltres;
        private Label lblStatut, lblFournisseur;
        private ComboBox cbFiltreStatut, cbFiltreFournisseur;
        private CheckBox chkPeriode;
        private DateTimePicker dtpDebut, dtpFin;
        private Button btnActualiser, btnNouvelleCommande, btnMarquerRecu, btnAnnuler;
        private Label lblRecap;

        private Panel pnlHaut;
        private DataGridView dgvCommandes;
        private DataGridViewTextBoxColumn colCmdId, colDate, colFourn, colStatut,
                                          colNbProd, colMontant, colReception, colNote;

        private Splitter splitter;

        private Panel pnlBas;
        private Label lblLignesTitre;
        private DataGridView dgvLignes;
        private DataGridViewTextBoxColumn colLProduit, colLQte, colLPrixU, colLTotal, colLStock;

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitre = new Label();
            pnlFiltres = new Panel();
            lblStatut = new Label();
            cbFiltreStatut = new ComboBox();
            lblFournisseur = new Label();
            cbFiltreFournisseur = new ComboBox();
            chkPeriode = new CheckBox();
            dtpDebut = new DateTimePicker();
            dtpFin = new DateTimePicker();
            btnActualiser = new Button();
            btnNouvelleCommande = new Button();
            btnMarquerRecu = new Button();
            btnAnnuler = new Button();
            lblRecap = new Label();
            pnlHaut = new Panel();
            dgvCommandes = new DataGridView();
            colCmdId = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colFourn = new DataGridViewTextBoxColumn();
            colStatut = new DataGridViewTextBoxColumn();
            colNbProd = new DataGridViewTextBoxColumn();
            colMontant = new DataGridViewTextBoxColumn();
            colReception = new DataGridViewTextBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            splitter = new Splitter();
            pnlBas = new Panel();
            lblLignesTitre = new Label();
            dgvLignes = new DataGridView();
            colLProduit = new DataGridViewTextBoxColumn();
            colLQte = new DataGridViewTextBoxColumn();
            colLPrixU = new DataGridViewTextBoxColumn();
            colLTotal = new DataGridViewTextBoxColumn();
            colLStock = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dgvCommandes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLignes).BeginInit();
            pnlHeader.SuspendLayout();
            pnlFiltres.SuspendLayout();
            pnlHaut.SuspendLayout();
            pnlBas.SuspendLayout();
            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitre.Text = "📦  Commandes Fournisseurs";
            lblTitre.Dock = DockStyle.Fill;
            lblTitre.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblTitre.Padding = new Padding(14, 0, 0, 0);

            pnlHeader.BackColor = Color.FromArgb(46, 100, 46);
            pnlHeader.Controls.Add(lblTitre);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 55;

            // ── Filtres ────────────────────────────────────────────────────
            void StyleLabel(Label l, string t, int x)
            {
                l.Text = t; l.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                l.Location = new Point(x, 16); l.Size = new Size(70, 20);
            }
            void StyleCombo(ComboBox cb, int x, int w = 140)
            {
                cb.Location = new Point(x, 13); cb.Size = new Size(w, 28);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Font = new Font("Segoe UI", 9F);
            }

            StyleLabel(lblStatut, "Statut :", 10);
            StyleCombo(cbFiltreStatut, 80, 150);
            cbFiltreStatut.Items.AddRange(new object[] {
                "Tous", "En attente", "Reçu partiellement", "Reçu", "Annulée" });
            cbFiltreStatut.SelectedIndex = 0;

            StyleLabel(lblFournisseur, "Fournisseur :", 242);
            cbFiltreFournisseur.Location = new Point(325, 13);
            cbFiltreFournisseur.Size = new Size(200, 28);
            cbFiltreFournisseur.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFiltreFournisseur.Font = new Font("Segoe UI", 9F);

            chkPeriode.Text = "Période :";
            chkPeriode.Location = new Point(538, 14);
            chkPeriode.Size = new Size(80, 20);
            chkPeriode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkPeriode.CheckedChanged += chkPeriode_CheckedChanged;

            dtpDebut.Location = new Point(622, 12); dtpDebut.Size = new Size(130, 28);
            dtpDebut.Format = DateTimePickerFormat.Short; dtpDebut.Enabled = false;

            dtpFin.Location = new Point(760, 12); dtpFin.Size = new Size(130, 28);
            dtpFin.Format = DateTimePickerFormat.Short; dtpFin.Enabled = false;

            void StyleBtn(Button b, string t, Color bg, int x, int w = 130)
            {
                b.Text = t; b.Location = new Point(x, 10); b.Size = new Size(w, 32);
                b.BackColor = bg; b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.UseVisualStyleBackColor = false;
            }

            StyleBtn(btnActualiser, "🔄 Actualiser", Color.FromArgb(69, 90, 100), 900, 120);
            StyleBtn(btnNouvelleCommande, "➕ Nouvelle commande", Color.FromArgb(0, 150, 136), 1030, 170);

            lblRecap.AutoSize = true;
            lblRecap.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblRecap.ForeColor = Color.DimGray;
            lblRecap.Location = new Point(10, 50);

            pnlFiltres.BackColor = Color.FromArgb(245, 250, 245);
            pnlFiltres.Controls.AddRange(new Control[] {
                lblStatut, cbFiltreStatut, lblFournisseur, cbFiltreFournisseur,
                chkPeriode, dtpDebut, dtpFin,
                btnActualiser, btnNouvelleCommande,
                lblRecap });
            pnlFiltres.Dock = DockStyle.Top;
            pnlFiltres.Height = 72;

            // ── Grille commandes ───────────────────────────────────────────
            void StyleGrid(DataGridView g)
            {
                g.AllowUserToAddRows = false;
                g.AllowUserToDeleteRows = false;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                g.BackgroundColor = Color.White;
                g.BorderStyle = BorderStyle.None;
                g.ColumnHeadersHeight = 36;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                g.Dock = DockStyle.Fill;
                g.EnableHeadersVisualStyles = false;
                g.Font = new Font("Segoe UI", 9F);
                g.MultiSelect = false;
                g.ReadOnly = true;
                g.RowHeadersVisible = false;
                g.RowTemplate.Height = 32;
                g.ScrollBars = ScrollBars.Both;
                g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            }

            colCmdId.Name = "colCmdId"; colCmdId.Visible = false;
            colDate.Name = "colDate"; colDate.HeaderText = "Date"; colDate.FillWeight = 80F;
            colFourn.Name = "colFourn"; colFourn.HeaderText = "Fournisseur"; colFourn.FillWeight = 100F;
            colStatut.Name = "colStatut"; colStatut.HeaderText = "Statut"; colStatut.FillWeight = 70F;
            colNbProd.Name = "colNbProd"; colNbProd.HeaderText = "Produits"; colNbProd.FillWeight = 40F;
            colMontant.Name = "colMontant"; colMontant.HeaderText = "Montant"; colMontant.FillWeight = 70F;
            colReception.Name = "colReception"; colReception.HeaderText = "Reçue le"; colReception.FillWeight = 70F;
            colNote.Name = "colNote"; colNote.HeaderText = "Note"; colNote.FillWeight = 100F;

            dgvCommandes.Columns.AddRange(new DataGridViewColumn[] {
                colCmdId, colDate, colFourn, colStatut, colNbProd, colMontant, colReception, colNote });
            StyleGrid(dgvCommandes);

            // Boutons action (dans pnlHaut en bas)
            var pnlActions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(245, 250, 245)
            };
            btnMarquerRecu.Text = "✅ Marquer comme reçue";
            btnMarquerRecu.Location = new Point(10, 10);
            btnMarquerRecu.Size = new Size(200, 32);
            btnMarquerRecu.BackColor = Color.FromArgb(46, 125, 50);
            btnMarquerRecu.ForeColor = Color.White;
            btnMarquerRecu.FlatStyle = FlatStyle.Flat;
            btnMarquerRecu.FlatAppearance.BorderSize = 0;
            btnMarquerRecu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMarquerRecu.Enabled = false;

            btnAnnuler.Text = "❌ Annuler la commande";
            btnAnnuler.Location = new Point(220, 10);
            btnAnnuler.Size = new Size(180, 32);
            btnAnnuler.BackColor = Color.FromArgb(198, 40, 40);
            btnAnnuler.ForeColor = Color.White;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.FlatAppearance.BorderSize = 0;
            btnAnnuler.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAnnuler.Enabled = false;

            pnlActions.Controls.Add(btnMarquerRecu);
            pnlActions.Controls.Add(btnAnnuler);

            pnlHaut.Controls.Add(dgvCommandes);
            pnlHaut.Controls.Add(pnlActions);
            pnlHaut.Dock = DockStyle.Top;
            pnlHaut.Height = 280;

            // ── Splitter ───────────────────────────────────────────────────
            splitter.Dock = DockStyle.Top;
            splitter.Height = 5;
            splitter.BackColor = Color.FromArgb(200, 220, 200);

            // ── Détail lignes ──────────────────────────────────────────────
            lblLignesTitre.Text = "Détail de la commande sélectionnée :";
            lblLignesTitre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLignesTitre.ForeColor = Color.FromArgb(46, 100, 46);
            lblLignesTitre.Dock = DockStyle.Top;
            lblLignesTitre.Height = 26;
            lblLignesTitre.Padding = new Padding(6, 4, 0, 0);
            lblLignesTitre.BackColor = Color.FromArgb(232, 245, 233);

            colLProduit.Name = "colLProduit"; colLProduit.HeaderText = "Produit"; colLProduit.FillWeight = 120F;
            colLQte.Name = "colLQte"; colLQte.HeaderText = "Qté commandée"; colLQte.FillWeight = 60F;
            colLPrixU.Name = "colLPrixU"; colLPrixU.HeaderText = "Prix unitaire"; colLPrixU.FillWeight = 70F;
            colLTotal.Name = "colLTotal"; colLTotal.HeaderText = "Total ligne"; colLTotal.FillWeight = 70F;
            colLStock.Name = "colLStock"; colLStock.HeaderText = "Stock actuel"; colLStock.FillWeight = 60F;

            dgvLignes.Columns.AddRange(new DataGridViewColumn[] {
                colLProduit, colLQte, colLPrixU, colLTotal, colLStock });
            StyleGrid(dgvLignes);
            dgvLignes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(69, 90, 100);

            pnlBas.Controls.Add(dgvLignes);
            pnlBas.Controls.Add(lblLignesTitre);
            pnlBas.Dock = DockStyle.Fill;

            // ── Assemblage ─────────────────────────────────────────────────
            Controls.Add(pnlBas);
            Controls.Add(splitter);
            Controls.Add(pnlHaut);
            Controls.Add(pnlFiltres);
            Controls.Add(pnlHeader);

            BackColor = Color.White;
            Size = new Size(1200, 700);

            pnlHeader.ResumeLayout(false);
            pnlFiltres.ResumeLayout(false);
            pnlHaut.ResumeLayout(false);
            pnlBas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCommandes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLignes).EndInit();
            ResumeLayout(false);
        }
    }
}