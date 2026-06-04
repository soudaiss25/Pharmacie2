using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Vente
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTop, pnlSearch, pnlActions, pnlInfo;
        private Label lblTitle, lblNombreVentes, lblTotalVentes, lblVentesCredit;
        private TextBox txtSearchVente;
        private Button btnSearch;
        private Button btnNouvelleVente, btnDetail, btnEnregistrerPaiement,
                       btnModifierVente, btnAnnuler;
        private DataGridView dgvVentes;

        private DataGridViewTextBoxColumn colIdVente, colNumeroVente, colClient, colTel;
        private DataGridViewTextBoxColumn colDate, colMontantTotal, colMontantVerse;
        private DataGridViewTextBoxColumn colMontantRestant, colMoyenPaiement,
                                          colVendeur, colStatut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            pnlSearch = new Panel();
            pnlActions = new Panel();
            pnlInfo = new Panel();
            lblTitle = new Label();
            lblNombreVentes = new Label();
            lblTotalVentes = new Label();
            lblVentesCredit = new Label();
            txtSearchVente = new TextBox();
            btnSearch = new Button();
            btnNouvelleVente = new Button();
            btnDetail = new Button();
            btnEnregistrerPaiement = new Button();
            btnModifierVente = new Button();
            btnAnnuler = new Button();
            dgvVentes = new DataGridView();
            colIdVente = new DataGridViewTextBoxColumn();
            colNumeroVente = new DataGridViewTextBoxColumn();
            colClient = new DataGridViewTextBoxColumn();
            colTel = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colMontantTotal = new DataGridViewTextBoxColumn();
            colMontantVerse = new DataGridViewTextBoxColumn();
            colMontantRestant = new DataGridViewTextBoxColumn();
            colMoyenPaiement = new DataGridViewTextBoxColumn();
            colVendeur = new DataGridViewTextBoxColumn();
            colStatut = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dgvVentes).BeginInit();
            this.SuspendLayout();

            // Top
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 52;
            pnlTop.BackColor = Color.FromArgb(0, 112, 175);
            lblTitle.Text = "🛒  Gestion des Ventes";
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(14, 12);
            lblTitle.Size = new Size(400, 28);
            pnlTop.Controls.Add(lblTitle);

            // Search
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Height = 44;
            pnlSearch.BackColor = Color.FromArgb(240, 245, 255);
            txtSearchVente.Location = new Point(14, 10);
            txtSearchVente.Size = new Size(300, 26);
            txtSearchVente.PlaceholderText = "🔍 Nom, prénom, téléphone, n° vente…";
            txtSearchVente.BorderStyle = BorderStyle.FixedSingle;
            txtSearchVente.TextChanged += txtSearchVente_TextChanged;
            btnSearch.Text = "Rechercher";
            btnSearch.Location = new Point(322, 9);
            btnSearch.Size = new Size(100, 28);
            btnSearch.BackColor = Color.FromArgb(0, 112, 175);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += btnSearch_Click;
            pnlSearch.Controls.AddRange(new Control[] { txtSearchVente, btnSearch });

            // Actions
            pnlActions.Dock = DockStyle.Top;
            pnlActions.Height = 46;
            pnlActions.BackColor = Color.White;

            void Btn(Button b, string t, Color c, int x, int w = 150)
            {
                b.Text = t;
                b.Location = new Point(x, 8);
                b.Size = new Size(w, 30);
                b.BackColor = c;
                b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            }

            Btn(btnNouvelleVente, "➕ Nouvelle vente", Color.FromArgb(0, 150, 136), 14, 145);
            Btn(btnDetail, "🔍 Détail", Color.FromArgb(69, 90, 100), 167, 100);
            Btn(btnEnregistrerPaiement, "💵 Enregistrer paiement", Color.FromArgb(33, 150, 243), 275, 180);
            Btn(btnModifierVente, "✏️ Modifier vente", Color.FromArgb(25, 118, 210), 463, 140);
            Btn(btnAnnuler, "⛔ Annuler vente", Color.FromArgb(198, 40, 40), 611, 130);

            btnNouvelleVente.Click += btnNouvelleVente_Click;
            btnDetail.Click += btnDetail_Click;
            btnEnregistrerPaiement.Click += btnEnregistrerPaiement_Click;
            btnModifierVente.Click += btnModifierVente_Click;
            btnAnnuler.Click += btnAnnuler_Click;

            pnlActions.Controls.AddRange(new Control[] {
                btnNouvelleVente, btnDetail, btnEnregistrerPaiement,
                btnModifierVente, btnAnnuler });

            // Info bas
            pnlInfo.Dock = DockStyle.Bottom;
            pnlInfo.Height = 36;
            pnlInfo.BackColor = Color.FromArgb(240, 245, 255);
            pnlInfo.BorderStyle = BorderStyle.FixedSingle;

            lblTotalVentes.Text = "CA : 0 KMF";
            lblTotalVentes.AutoSize = true;
            lblTotalVentes.Location = new Point(14, 9);
            lblTotalVentes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalVentes.ForeColor = Color.FromArgb(0, 112, 175);

            lblNombreVentes.Text = "Total : 0 vente(s)";
            lblNombreVentes.AutoSize = true;
            lblNombreVentes.Location = new Point(220, 9);
            lblNombreVentes.Font = new Font("Segoe UI", 9F);

            lblVentesCredit.Text = "Crédit en cours : 0 KMF";
            lblVentesCredit.AutoSize = true;
            lblVentesCredit.Location = new Point(430, 9);
            lblVentesCredit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVentesCredit.ForeColor = Color.OrangeRed;
            pnlInfo.Controls.AddRange(new Control[] {
                lblTotalVentes, lblNombreVentes, lblVentesCredit });

            // Colonnes DataGridView
            colIdVente.Name = "colIdVente"; colIdVente.HeaderText = "ID"; colIdVente.Visible = false;
            colNumeroVente.Name = "colNumeroVente"; colNumeroVente.HeaderText = "N° Vente";
            colClient.Name = "colClient"; colClient.HeaderText = "Client";
            colTel.Name = "colTel"; colTel.HeaderText = "Téléphone";
            colDate.Name = "colDate"; colDate.HeaderText = "Date";
            colMontantTotal.Name = "colMontantTotal"; colMontantTotal.HeaderText = "Total";
            colMontantVerse.Name = "colMontantVerse"; colMontantVerse.HeaderText = "Versé";
            colMontantRestant.Name = "colMontantRestant"; colMontantRestant.HeaderText = "Reste";
            colMoyenPaiement.Name = "colMoyenPaiement"; colMoyenPaiement.HeaderText = "Paiement";
            colVendeur.Name = "colVendeur"; colVendeur.HeaderText = "Vendeur";
            colStatut.Name = "colStatut"; colStatut.HeaderText = "Statut";

            dgvVentes.Columns.AddRange(new DataGridViewColumn[] {
                colIdVente, colNumeroVente, colClient, colTel, colDate,
                colMontantTotal, colMontantVerse, colMontantRestant,
                colMoyenPaiement, colVendeur, colStatut });

            dgvVentes.Dock = DockStyle.Fill;
            dgvVentes.ReadOnly = true;
            dgvVentes.AllowUserToAddRows = false;
            dgvVentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentes.MultiSelect = false;
            dgvVentes.RowHeadersVisible = false;
            dgvVentes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentes.BackgroundColor = Color.White;
            dgvVentes.BorderStyle = BorderStyle.None;
            dgvVentes.RowTemplate.Height = 34;
            dgvVentes.ColumnHeadersHeight = 38;
            dgvVentes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 112, 175);
            dgvVentes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVentes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVentes.EnableHeadersVisualStyles = false;
            dgvVentes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);
            dgvVentes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(179, 229, 252);
            dgvVentes.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVentes.GridColor = Color.FromArgb(220, 230, 245);
            dgvVentes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVentes.Font = new Font("Segoe UI", 9F);

            ((System.ComponentModel.ISupportInitialize)dgvVentes).EndInit();

            this.Controls.Add(dgvVentes);
            this.Controls.Add(pnlInfo);
            this.Controls.Add(pnlActions);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlTop);
            this.Size = new Size(1100, 680);
            this.BackColor = Color.White;
            this.ResumeLayout(false);
        }
    }
}