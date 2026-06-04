using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormCommandesFournisseur
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitre;
        private Label lblNbCommandes;
        private Label lblFiltreStatut;
        private ComboBox cbFiltreStatut;
        private DataGridView dgvCommandes;
        private Label lblDetailTitre;
        private DataGridView dgvLignes;
        private Button btnMarquerRecu;
        private Button btnMarquerPartiel;
        private Button btnAnnuler;
        private Button btnFermer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            lblNbCommandes = new Label();
            lblFiltreStatut = new Label();
            cbFiltreStatut = new ComboBox();
            dgvCommandes = new DataGridView();
            lblDetailTitre = new Label();
            dgvLignes = new DataGridView();
            btnMarquerRecu = new Button();
            btnMarquerPartiel = new Button();
            btnAnnuler = new Button();
            btnFermer = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvCommandes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLignes).BeginInit();
            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Height = 55;
            lblTitre.Text = "📋  Commandes fournisseur";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.BackColor = Color.FromArgb(0, 150, 136);
            lblTitre.ForeColor = Color.White;

            // ── Barre filtre + compteur ────────────────────────────────────
            lblFiltreStatut.Text = "Statut :";
            lblFiltreStatut.Location = new Point(12, 66);
            lblFiltreStatut.Size = new Size(55, 22);
            lblFiltreStatut.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cbFiltreStatut.Location = new Point(70, 63);
            cbFiltreStatut.Size = new Size(170, 28);
            cbFiltreStatut.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFiltreStatut.Font = new Font("Segoe UI", 9F);
            cbFiltreStatut.Items.AddRange(new object[] {
                "Tous", "En attente", "Reçu partiellement", "Reçu", "Annulée" });

            lblNbCommandes.Location = new Point(260, 66);
            lblNbCommandes.Size = new Size(680, 22);
            lblNbCommandes.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblNbCommandes.ForeColor = Color.DimGray;

            // ── Grille commandes ───────────────────────────────────────────
            void StyleGrid(DataGridView g)
            {
                g.ReadOnly = true;
                g.AllowUserToAddRows = false;
                g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                g.MultiSelect = false;
                g.RowHeadersVisible = false;
                g.BackgroundColor = Color.White;
                g.BorderStyle = BorderStyle.None;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                g.Font = new Font("Segoe UI", 9F);
                g.RowTemplate.Height = 32;
                g.ColumnHeadersHeight = 36;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 110);
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                g.EnableHeadersVisualStyles = false;
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 243);
                g.ScrollBars = ScrollBars.Both;
            }

            StyleGrid(dgvCommandes);
            dgvCommandes.Location = new Point(12, 95);
            dgvCommandes.Size = new Size(956, 220);
            dgvCommandes.SelectionChanged += dgvCommandes_SelectionChanged;

            // Colonnes commandes
            dgvCommandes.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colCmdId", HeaderText = "ID", Visible = false },
                new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date commande", FillWeight = 80F },
                new DataGridViewTextBoxColumn { Name = "colStatut", HeaderText = "Statut", FillWeight = 70F },
                new DataGridViewTextBoxColumn { Name = "colNbLig", HeaderText = "Produits", FillWeight = 40F },
                new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total (KMF)", FillWeight = 60F },
                new DataGridViewTextBoxColumn { Name = "colLivraison", HeaderText = "Livraison prévue", FillWeight = 70F },
                new DataGridViewTextBoxColumn { Name = "colReception", HeaderText = "Reçue le", FillWeight = 70F },
                new DataGridViewTextBoxColumn { Name = "colNote", HeaderText = "Note", FillWeight = 100F }
            );

            // ── Boutons d'action ───────────────────────────────────────────
            void StyleActionBtn(Button b, string txt, Color bg, int x)
            {
                b.Text = txt;
                b.BackColor = bg;
                b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.Location = new Point(x, 323);
                b.Size = new Size(195, 34);
                b.Enabled = false;
                b.UseVisualStyleBackColor = false;
            }

            StyleActionBtn(btnMarquerRecu, "✅ Marquer comme reçue", Color.FromArgb(46, 125, 50), 12);
            StyleActionBtn(btnMarquerPartiel, "📦 Réception partielle", Color.FromArgb(230, 120, 0), 217);
            StyleActionBtn(btnAnnuler, "❌ Annuler la commande", Color.FromArgb(198, 40, 40), 422);

            btnMarquerRecu.Click += btnMarquerRecu_Click;
            btnMarquerPartiel.Click += btnMarquerPartiel_Click;
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Détail lignes ──────────────────────────────────────────────
            lblDetailTitre.Text = "Détail de la commande sélectionnée :";
            lblDetailTitre.Location = new Point(12, 368);
            lblDetailTitre.Size = new Size(400, 22);
            lblDetailTitre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetailTitre.ForeColor = Color.FromArgb(0, 100, 90);

            StyleGrid(dgvLignes);
            dgvLignes.Location = new Point(12, 393);
            dgvLignes.Size = new Size(956, 190);

            dgvLignes.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colLigId", HeaderText = "ID", Visible = false },
                new DataGridViewTextBoxColumn { Name = "colProduit", HeaderText = "Produit", FillWeight = 120F },
                new DataGridViewTextBoxColumn { Name = "colQte", HeaderText = "Qté commandée", FillWeight = 60F },
                new DataGridViewTextBoxColumn { Name = "colPrixU", HeaderText = "Prix unitaire", FillWeight = 70F },
                new DataGridViewTextBoxColumn { Name = "colSousTotal", HeaderText = "Total ligne", FillWeight = 70F },
                new DataGridViewTextBoxColumn { Name = "colStockActuel", HeaderText = "Stock actuel", FillWeight = 60F }
            );

            // ── Bouton Fermer ──────────────────────────────────────────────
            btnFermer.Text = "Fermer";
            btnFermer.Location = new Point(848, 596);
            btnFermer.Size = new Size(120, 36);
            btnFermer.FlatStyle = FlatStyle.Flat;
            btnFermer.BackColor = Color.FromArgb(69, 90, 100);
            btnFermer.ForeColor = Color.White;
            btnFermer.FlatAppearance.BorderSize = 0;
            btnFermer.Font = new Font("Segoe UI", 9.5F);
            btnFermer.Click += btnFermer_Click;

            // ── Form ───────────────────────────────────────────────────────
            ClientSize = new Size(980, 646);
            Text = "Commandes fournisseur";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            Controls.AddRange(new Control[] {
                lblTitre,
                lblFiltreStatut, cbFiltreStatut, lblNbCommandes,
                dgvCommandes,
                btnMarquerRecu, btnMarquerPartiel, btnAnnuler,
                lblDetailTitre, dgvLignes,
                btnFermer
            });

            ((System.ComponentModel.ISupportInitialize)dgvCommandes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLignes).EndInit();
            ResumeLayout(false);
        }
    }
}