using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class PageCaissier
    {
        private System.ComponentModel.IContainer components = null;

        // Panels
        private Panel pnlTop, pnlActions, pnlSessionBandeau;

        // Labels
        private Label lblTitre, lblBienvenue, lblStats, lblSessionInfo;

        // Boutons
        private Button btnNouvelleVente, btnDetail, btnPaiement,
                       btnActualiser, btnCloturerSession, btnDeconnexion;

        // Grille ventes
        private DataGridView dgvVentes;
        private DataGridViewTextBoxColumn colId, colNumero, colClient,
                                          colHeure, colTotal, colVerse,
                                          colRestant, colMode, colStatut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            pnlActions = new Panel();
            pnlSessionBandeau = new Panel();
            lblTitre = new Label();
            lblBienvenue = new Label();
            lblStats = new Label();
            lblSessionInfo = new Label();
            btnNouvelleVente = new Button();
            btnDetail = new Button();
            btnPaiement = new Button();
            btnActualiser = new Button();
            btnCloturerSession = new Button();
            btnDeconnexion = new Button();
            dgvVentes = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNumero = new DataGridViewTextBoxColumn();
            colClient = new DataGridViewTextBoxColumn();
            colHeure = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colVerse = new DataGridViewTextBoxColumn();
            colRestant = new DataGridViewTextBoxColumn();
            colMode = new DataGridViewTextBoxColumn();
            colStatut = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dgvVentes).BeginInit();
            SuspendLayout();

            // ══════════════════════════════════════════════════════════════
            // PANEL TOP — en-tête vert
            // ══════════════════════════════════════════════════════════════
            pnlTop.BackColor = Color.ForestGreen;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 70;

            lblTitre.Text = "🏥 PharmaSoft — Mes ventes du jour";
            lblTitre.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Location = new Point(12, 6);
            lblTitre.Size = new Size(500, 30);

            lblBienvenue.Text = "Bonjour";
            lblBienvenue.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblBienvenue.ForeColor = Color.LightGreen;
            lblBienvenue.Location = new Point(14, 40);
            lblBienvenue.Size = new Size(600, 20);

            btnDeconnexion.Text = "🚪 Déconnexion";
            btnDeconnexion.Location = new Point(880, 16);
            btnDeconnexion.Size = new Size(130, 36);
            btnDeconnexion.BackColor = Color.OrangeRed;
            btnDeconnexion.ForeColor = Color.White;
            btnDeconnexion.FlatStyle = FlatStyle.Flat;
            btnDeconnexion.FlatAppearance.BorderSize = 0;
            btnDeconnexion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeconnexion.Click += btnDeconnexion_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitre, lblBienvenue, btnDeconnexion });

            // ══════════════════════════════════════════════════════════════
            // PANEL ACTIONS — boutons opérations
            // ══════════════════════════════════════════════════════════════
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
            Btn(btnPaiement, "💵 Paiement crédit", Color.FromArgb(33, 150, 243), 275, 145);
            Btn(btnActualiser, "🔄 Actualiser", Color.FromArgb(117, 117, 117), 428, 110);
            Btn(btnCloturerSession, "🔒 Clôturer caisse", Color.FromArgb(180, 60, 0), 546, 145);

            btnNouvelleVente.Click += btnNouvelleVente_Click;
            btnDetail.Click += btnDetail_Click;
            btnPaiement.Click += btnPaiement_Click;
            btnActualiser.Click += btnActualiser_Click;
            btnCloturerSession.Click += btnCloturerSession_Click;

            pnlActions.Controls.AddRange(new Control[] {
                btnNouvelleVente, btnDetail, btnPaiement,
                btnActualiser, btnCloturerSession });

            // ══════════════════════════════════════════════════════════════
            // BANDEAU SESSION — état de la session caisse (sous actions)
            // ══════════════════════════════════════════════════════════════
            pnlSessionBandeau.Dock = DockStyle.Top;
            pnlSessionBandeau.Height = 28;
            pnlSessionBandeau.BackColor = Color.FromArgb(232, 245, 233);

            lblSessionInfo.Text = "⚪ Vérification de la session...";
            lblSessionInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSessionInfo.ForeColor = Color.FromArgb(27, 94, 32);
            lblSessionInfo.Dock = DockStyle.Fill;
            lblSessionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            pnlSessionBandeau.Controls.Add(lblSessionInfo);

            // ══════════════════════════════════════════════════════════════
            // STATS — barre du bas
            // ══════════════════════════════════════════════════════════════
            lblStats.Dock = DockStyle.Bottom;
            lblStats.Height = 32;
            lblStats.BackColor = Color.FromArgb(232, 245, 233);
            lblStats.Text = "Ventes aujourd'hui : 0  |  Total : 0 KMF";
            lblStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(27, 94, 32);
            lblStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ══════════════════════════════════════════════════════════════
            // COLONNES DATAGRIDVIEW
            // ══════════════════════════════════════════════════════════════
            colId.Name = "colId"; colId.HeaderText = "ID"; colId.Visible = false;
            colNumero.Name = "colNumero"; colNumero.HeaderText = "N° Vente";
            colClient.Name = "colClient"; colClient.HeaderText = "Client";
            colHeure.Name = "colHeure"; colHeure.HeaderText = "Heure";
            colTotal.Name = "colTotal"; colTotal.HeaderText = "Total";
            colVerse.Name = "colVerse"; colVerse.HeaderText = "Versé";
            colRestant.Name = "colRestant"; colRestant.HeaderText = "Reste";
            colMode.Name = "colMode"; colMode.HeaderText = "Mode";
            colStatut.Name = "colStatut"; colStatut.HeaderText = "Statut";

            dgvVentes.Columns.AddRange(new DataGridViewColumn[] {
                colId, colNumero, colClient, colHeure,
                colTotal, colVerse, colRestant, colMode, colStatut });

            // ══════════════════════════════════════════════════════════════
            // STYLE DATAGRIDVIEW
            // ══════════════════════════════════════════════════════════════
            dgvVentes.Dock = DockStyle.Fill;
            dgvVentes.ReadOnly = true;
            dgvVentes.AllowUserToAddRows = false;
            dgvVentes.RowHeadersVisible = false;
            dgvVentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentes.MultiSelect = false;
            dgvVentes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentes.BackgroundColor = Color.White;
            dgvVentes.BorderStyle = BorderStyle.None;
            dgvVentes.RowTemplate.Height = 36;
            dgvVentes.ColumnHeadersHeight = 38;
            dgvVentes.ColumnHeadersDefaultCellStyle.BackColor = Color.ForestGreen;
            dgvVentes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVentes.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVentes.EnableHeadersVisualStyles = false;
            dgvVentes.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(232, 245, 233);
            dgvVentes.Font = new Font("Segoe UI", 9.5F);

            ((System.ComponentModel.ISupportInitialize)dgvVentes).EndInit();

            // ══════════════════════════════════════════════════════════════
            // ASSEMBLAGE FORM
            // IMPORTANT : l'ordre d'ajout des contrôles Dock:Top est inversé
            // (le dernier ajouté est affiché en haut)
            // ══════════════════════════════════════════════════════════════
            Controls.Add(dgvVentes);          // Fill  → centre
            Controls.Add(lblStats);           // Bottom
            Controls.Add(pnlSessionBandeau); // Top (ajouté 3e → affiché sous pnlActions)
            Controls.Add(pnlActions);        // Top (ajouté 2e → affiché sous pnlTop)
            Controls.Add(pnlTop);            // Top (ajouté 1er → reste en haut)

            ClientSize = new Size(1024, 680);
            Text = "PharmaSoft — Caisse";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 600);
            ResumeLayout(false);
        }
    }
}