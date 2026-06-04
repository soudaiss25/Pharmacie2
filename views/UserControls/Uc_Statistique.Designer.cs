using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Statistique
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        // ── Contrôles déclarés ────────────────────────────────────────────
        private Label lblTitre;
        private Label lblPeriodeAffichee;
        private ComboBox cbPeriode;
        private DateTimePicker dtpDebut, dtpFin;
        private Button btnActualiser;
        private Label lblLastUpdate;

        // KPI cards
        private Panel pnlKpiContainer;
        private Panel cardCA, cardMarge, cardVentes, cardImpayes, cardRestant;
        private Label lblCALabel, lblCAValue;
        private Label lblMargeLabel, lblMargeValue;
        private Label lblVentesLabel, lblVentesValue;
        private Label lblImpayesLabel, lblImpayesValue;
        private Label lblRestantLabel, lblRestantValue;

        // Grilles
        private GroupBox gbVentilation;
        private DataGridView dgvVentilation;
        private GroupBox gbTopProduits;
        private DataGridView dgvTopProduits;
        private GroupBox gbCreditsClients;
        private DataGridView dgvCreditsClients;
        private GroupBox gbMutuelleImpayes;
        private DataGridView dgvMutuelleImpayes;
        private GroupBox gbReco;
        private RichTextBox rtbRecommandations;

        // Boutons export
        private Button btnExportPDF;
        private Button btnExportExcel;

        private void InitializeComponent()
        {
            lblTitre = new Label();
            lblPeriodeAffichee = new Label();
            cbPeriode = new ComboBox();
            dtpDebut = new DateTimePicker();
            dtpFin = new DateTimePicker();
            btnActualiser = new Button();
            lblLastUpdate = new Label();
            pnlKpiContainer = new Panel();

            cardCA = new Panel(); lblCALabel = new Label(); lblCAValue = new Label();
            cardMarge = new Panel(); lblMargeLabel = new Label(); lblMargeValue = new Label();
            cardVentes = new Panel(); lblVentesLabel = new Label(); lblVentesValue = new Label();
            cardImpayes = new Panel(); lblImpayesLabel = new Label(); lblImpayesValue = new Label();
            cardRestant = new Panel(); lblRestantLabel = new Label(); lblRestantValue = new Label();

            gbVentilation = new GroupBox(); dgvVentilation = new DataGridView();
            gbTopProduits = new GroupBox(); dgvTopProduits = new DataGridView();
            gbCreditsClients = new GroupBox(); dgvCreditsClients = new DataGridView();
            gbMutuelleImpayes = new GroupBox(); dgvMutuelleImpayes = new DataGridView();
            gbReco = new GroupBox(); rtbRecommandations = new RichTextBox();

            btnExportPDF = new Button();
            btnExportExcel = new Button();

            pnlKpiContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentilation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTopProduits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCreditsClients).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMutuelleImpayes).BeginInit();
            gbVentilation.SuspendLayout();
            gbTopProduits.SuspendLayout();
            gbCreditsClients.SuspendLayout();
            gbMutuelleImpayes.SuspendLayout();
            gbReco.SuspendLayout();
            SuspendLayout();

            // ══════════════════════════════════════════════════════════════
            // EN-TÊTE
            // ══════════════════════════════════════════════════════════════
            lblTitre.AutoSize = true;
            lblTitre.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitre.ForeColor = Color.FromArgb(27, 94, 32);
            lblTitre.Location = new Point(14, 10);
            lblTitre.Text = "📊  Statistiques & Tableau de Bord";

            cbPeriode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPeriode.Items.AddRange(new object[] {
                "Aujourd'hui", "Cette semaine", "Ce mois",
                "Cette année", "Personnalisé" });
            cbPeriode.Location = new Point(500, 14);
            cbPeriode.Size = new Size(170, 30);
            cbPeriode.Font = new Font("Segoe UI", 9.5F);

            dtpDebut.Location = new Point(680, 14);
            dtpDebut.Size = new Size(160, 30);
            dtpDebut.Format = DateTimePickerFormat.Short;
            dtpDebut.Visible = false;

            dtpFin.Location = new Point(850, 14);
            dtpFin.Size = new Size(160, 30);
            dtpFin.Format = DateTimePickerFormat.Short;
            dtpFin.Visible = false;

            btnActualiser.Text = "🔄 Actualiser";
            btnActualiser.Location = new Point(1020, 10);
            btnActualiser.Size = new Size(130, 34);
            btnActualiser.BackColor = Color.FromArgb(0, 122, 204);
            btnActualiser.ForeColor = Color.White;
            btnActualiser.FlatStyle = FlatStyle.Flat;
            btnActualiser.FlatAppearance.BorderSize = 0;
            btnActualiser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActualiser.UseVisualStyleBackColor = false;

            lblPeriodeAffichee.AutoSize = true;
            lblPeriodeAffichee.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblPeriodeAffichee.ForeColor = Color.FromArgb(69, 90, 100);
            lblPeriodeAffichee.Location = new Point(14, 46);
            lblPeriodeAffichee.Text = "Période : —";

            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblLastUpdate.ForeColor = Color.Gray;
            lblLastUpdate.Location = new Point(900, 46);
            lblLastUpdate.Text = "Actualisation : —";

            // ══════════════════════════════════════════════════════════════
            // KPI CARDS — 5 cartes côte à côte
            // ══════════════════════════════════════════════════════════════
            void MakeCard(Panel card, Label lbl, Label val, string titre,
                          Color bgColor, Color valColor, int x)
            {
                card.BackColor = bgColor;
                card.BorderStyle = BorderStyle.None;
                card.Size = new Size(228, 90);
                card.Location = new Point(x, 0);

                // Barre colorée en haut
                var barre = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 4,
                    BackColor = valColor
                };

                lbl.Text = titre;
                lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(80, 80, 80);
                lbl.Location = new Point(12, 14);
                lbl.AutoSize = true;

                val.Text = "0";
                val.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                val.ForeColor = valColor;
                val.Location = new Point(12, 38);
                val.AutoSize = true;

                card.Controls.Add(barre);
                card.Controls.Add(lbl);
                card.Controls.Add(val);
            }

            MakeCard(cardCA, lblCALabel, lblCAValue,
                "💰 Chiffre d'affaires", Color.FromArgb(232, 245, 233),
                Color.FromArgb(27, 94, 32), 0);

            MakeCard(cardMarge, lblMargeLabel, lblMargeValue,
                "📈 Bénéfice (marge)", Color.FromArgb(225, 245, 254),
                Color.FromArgb(1, 87, 155), 238);

            MakeCard(cardVentes, lblVentesLabel, lblVentesValue,
                "🛒 Nb de ventes", Color.FromArgb(243, 229, 245),
                Color.FromArgb(106, 27, 154), 476);

            MakeCard(cardImpayes, lblImpayesLabel, lblImpayesValue,
                "⚠️ Ventes impayées", Color.FromArgb(255, 243, 224),
                Color.FromArgb(230, 81, 0), 714);

            MakeCard(cardRestant, lblRestantLabel, lblRestantValue,
                "💸 Total restant dû", Color.FromArgb(255, 235, 238),
                Color.FromArgb(183, 28, 28), 952);

            pnlKpiContainer.Controls.AddRange(new Control[] {
                cardCA, cardMarge, cardVentes, cardImpayes, cardRestant });
            pnlKpiContainer.Location = new Point(14, 66);
            pnlKpiContainer.Size = new Size(1280, 100);
            pnlKpiContainer.Name = "pnlKpiContainer";

            // ══════════════════════════════════════════════════════════════
            // VENTILATION PAR MODE DE PAIEMENT (NOUVELLE SECTION)
            // ══════════════════════════════════════════════════════════════
            dgvVentilation.AllowUserToAddRows = false;
            dgvVentilation.AllowUserToDeleteRows = false;
            dgvVentilation.ReadOnly = true;
            dgvVentilation.Dock = DockStyle.Fill;
            dgvVentilation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentilation.RowHeadersVisible = false;
            dgvVentilation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentilation.MultiSelect = false;

            gbVentilation.Controls.Add(dgvVentilation);
            gbVentilation.Location = new Point(14, 178);
            gbVentilation.Size = new Size(1280, 220);
            gbVentilation.Text = "💳  Ventilation des encaissements par mode de paiement";
            gbVentilation.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbVentilation.ForeColor = Color.FromArgb(27, 94, 32);

            // ══════════════════════════════════════════════════════════════
            // RANGÉE DU BAS — 4 GroupBox
            // ══════════════════════════════════════════════════════════════

            // Top Produits
            dgvTopProduits.AllowUserToAddRows = false;
            dgvTopProduits.ReadOnly = true;
            dgvTopProduits.Dock = DockStyle.Fill;
            dgvTopProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopProduits.RowHeadersVisible = false;
            dgvTopProduits.ColumnHeadersHeight = 34;

            gbTopProduits.Controls.Add(dgvTopProduits);
            gbTopProduits.Location = new Point(14, 410);
            gbTopProduits.Size = new Size(620, 240);
            gbTopProduits.Text = "🏆  Top 10 produits vendus";
            gbTopProduits.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbTopProduits.ForeColor = Color.FromArgb(27, 94, 32);

            // Crédits clients
            dgvCreditsClients.AllowUserToAddRows = false;
            dgvCreditsClients.ReadOnly = true;
            dgvCreditsClients.Dock = DockStyle.Fill;
            dgvCreditsClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCreditsClients.RowHeadersVisible = false;
            dgvCreditsClients.ColumnHeadersHeight = 34;

            gbCreditsClients.Controls.Add(dgvCreditsClients);
            gbCreditsClients.Location = new Point(648, 410);
            gbCreditsClients.Size = new Size(646, 240);
            gbCreditsClients.Text = "📋  Crédits clients impayés";
            gbCreditsClients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbCreditsClients.ForeColor = Color.FromArgb(183, 28, 28);

            // Mutuelles impayées
            dgvMutuelleImpayes.AllowUserToAddRows = false;
            dgvMutuelleImpayes.ReadOnly = true;
            dgvMutuelleImpayes.Dock = DockStyle.Fill;
            dgvMutuelleImpayes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMutuelleImpayes.RowHeadersVisible = false;
            dgvMutuelleImpayes.ColumnHeadersHeight = 34;

            gbMutuelleImpayes.Controls.Add(dgvMutuelleImpayes);
            gbMutuelleImpayes.Location = new Point(14, 662);
            gbMutuelleImpayes.Size = new Size(620, 200);
            gbMutuelleImpayes.Text = "🏥  Mutuelles impayées";
            gbMutuelleImpayes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbMutuelleImpayes.ForeColor = Color.FromArgb(230, 81, 0);

            // Recommandations
            rtbRecommandations.Dock = DockStyle.Fill;
            rtbRecommandations.ReadOnly = true;
            rtbRecommandations.BackColor = Color.FromArgb(250, 250, 250);
            rtbRecommandations.Font = new Font("Segoe UI", 9F);
            rtbRecommandations.BorderStyle = BorderStyle.None;

            gbReco.Controls.Add(rtbRecommandations);
            gbReco.Location = new Point(648, 662);
            gbReco.Size = new Size(646, 200);
            gbReco.Text = "💡  Recommandations & Alertes";
            gbReco.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbReco.ForeColor = Color.FromArgb(1, 87, 155);

            // ── Boutons export ─────────────────────────────────────────────
            btnExportExcel.Text = "📊 Export Excel";
            btnExportExcel.Location = new Point(14, 876);
            btnExportExcel.Size = new Size(160, 36);
            btnExportExcel.BackColor = Color.FromArgb(56, 142, 60);
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportExcel.Click += btnExportExcel_Click;
            btnExportExcel.UseVisualStyleBackColor = false;

            btnExportPDF.Text = "📄 Export PDF";
            btnExportPDF.Location = new Point(186, 876);
            btnExportPDF.Size = new Size(160, 36);
            btnExportPDF.BackColor = Color.FromArgb(198, 40, 40);
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportPDF.Click += btnExportPDF_Click;
            btnExportPDF.UseVisualStyleBackColor = false;

            // ── Assemblage ─────────────────────────────────────────────────
            BackColor = Color.FromArgb(245, 248, 250);
            Size = new Size(1320, 930);
            Name = "Uc_Statistique";

            Controls.AddRange(new Control[] {
                lblTitre, cbPeriode, dtpDebut, dtpFin,
                btnActualiser, lblPeriodeAffichee, lblLastUpdate,
                pnlKpiContainer,
                gbVentilation,
                gbTopProduits, gbCreditsClients,
                gbMutuelleImpayes, gbReco,
                btnExportExcel, btnExportPDF
            });

            pnlKpiContainer.ResumeLayout(false);
            gbVentilation.ResumeLayout(false);
            gbTopProduits.ResumeLayout(false);
            gbCreditsClients.ResumeLayout(false);
            gbMutuelleImpayes.ResumeLayout(false);
            gbReco.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVentilation).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTopProduits).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCreditsClients).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMutuelleImpayes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}