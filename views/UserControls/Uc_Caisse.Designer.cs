using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Caisse
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTop;
        private Label lblTitre;
        private TabControl tabMain;
        private TabPage tabRapport, tabSessions;

        private Panel pnlFiltres;
        private Label lblPeriodeLabel, lblPeriodeAffichee;
        private ComboBox cbPeriode;
        private DateTimePicker dtpDebut, dtpFin;
        private Button btnActualiser;

        private Panel pnlCartes;
        private Panel cardVentes, cardComptant, cardCredit, cardMutuelle, cardCB, cardCheque;
        private Label lblNbVentesTitle, lblNbVentes;
        private Label lblCaComptantTitle, lblCaComptant;
        private Label lblCaCreditTitle, lblCaCredit;
        private Label lblCaMutuelleTitle, lblCaMutuelle;
        private Label lblCaCBTitle, lblCaCB;
        private Label lblCaChequeTitle, lblCaCheque;

        private Panel pnlCaisse;
        private Label lblEspecesTitle, lblEspecesRecues;
        private Label lblRenduTitle, lblRendu;
        private Label lblCreditRecuTitle, lblCreditRecu;
        private Label lblMutuellePatientTitle, lblMutuellePatient;
        private Label lblSeparateur;
        private Label lblTotalTitle, lblTotalCaisse;

        // NOUVEAU - ventilation
        private GroupBox gbVentilation;
        private DataGridView dgvVentilation;

        private DataGridView dgvDetail;

        private Panel pnlFiltresSessions;
        private Label lblFiltreAnnee, lblFiltreMois, lblFiltreJour, lblFiltreUser;
        private ComboBox cbFiltreAnnee, cbFiltreMois, cbFiltreJour, cbFiltreUser;
        private Button btnActualiserSessions;
        private Label lblNbSessions;

        private SplitContainer splitSessions;
        private DataGridView dgvSessions;
        private DataGridViewTextBoxColumn
            colSessionId, colCaissier, colOuverture, colCloture, colStatutSession,
            colFond, colEncaisse, colTheorique, colCompte, colEcart, colNbVentes;

        private Label lblDetailSession;
        private Label lblVentilationSession;  // NOUVEAU
        private DataGridView dgvDetailSession;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel(); lblTitre = new Label();
            tabMain = new TabControl(); tabRapport = new TabPage(); tabSessions = new TabPage();
            pnlFiltres = new Panel(); lblPeriodeLabel = new Label(); lblPeriodeAffichee = new Label();
            cbPeriode = new ComboBox(); dtpDebut = new DateTimePicker(); dtpFin = new DateTimePicker();
            btnActualiser = new Button();
            pnlCartes = new Panel();
            cardVentes = new Panel(); cardComptant = new Panel(); cardCredit = new Panel(); cardMutuelle = new Panel();
            cardCB = new Panel(); cardCheque = new Panel();
            lblNbVentesTitle = new Label(); lblNbVentes = new Label();
            lblCaComptantTitle = new Label(); lblCaComptant = new Label();
            lblCaCreditTitle = new Label(); lblCaCredit = new Label();
            lblCaMutuelleTitle = new Label(); lblCaMutuelle = new Label();
            lblCaCBTitle = new Label(); lblCaCB = new Label();
            lblCaChequeTitle = new Label(); lblCaCheque = new Label();
            pnlCaisse = new Panel();
            lblEspecesTitle = new Label(); lblEspecesRecues = new Label();
            lblRenduTitle = new Label(); lblRendu = new Label();
            lblCreditRecuTitle = new Label(); lblCreditRecu = new Label();
            lblMutuellePatientTitle = new Label(); lblMutuellePatient = new Label();
            lblSeparateur = new Label(); lblTotalTitle = new Label(); lblTotalCaisse = new Label();
            gbVentilation = new GroupBox(); dgvVentilation = new DataGridView();
            dgvDetail = new DataGridView();
            pnlFiltresSessions = new Panel();
            lblFiltreAnnee = new Label(); cbFiltreAnnee = new ComboBox();
            lblFiltreMois = new Label(); cbFiltreMois = new ComboBox();
            lblFiltreJour = new Label(); cbFiltreJour = new ComboBox();
            lblFiltreUser = new Label(); cbFiltreUser = new ComboBox();
            btnActualiserSessions = new Button(); lblNbSessions = new Label();
            splitSessions = new SplitContainer(); dgvSessions = new DataGridView();
            colSessionId = new DataGridViewTextBoxColumn(); colCaissier = new DataGridViewTextBoxColumn();
            colOuverture = new DataGridViewTextBoxColumn(); colCloture = new DataGridViewTextBoxColumn();
            colStatutSession = new DataGridViewTextBoxColumn(); colFond = new DataGridViewTextBoxColumn();
            colEncaisse = new DataGridViewTextBoxColumn(); colTheorique = new DataGridViewTextBoxColumn();
            colCompte = new DataGridViewTextBoxColumn(); colEcart = new DataGridViewTextBoxColumn();
            colNbVentes = new DataGridViewTextBoxColumn();
            lblDetailSession = new Label(); lblVentilationSession = new Label();
            dgvDetailSession = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvVentilation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetail).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSessions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetailSession).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitSessions).BeginInit();
            splitSessions.Panel1.SuspendLayout();
            splitSessions.Panel2.SuspendLayout();
            gbVentilation.SuspendLayout();
            tabMain.SuspendLayout();
            SuspendLayout();

            // PANEL TOP
            pnlTop.Dock = DockStyle.Top; pnlTop.Height = 55; pnlTop.BackColor = Color.FromArgb(34, 85, 34);
            lblTitre.Text = "  Caisse & Sessions"; lblTitre.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White; lblTitre.Location = new Point(14, 12); lblTitre.Size = new Size(400, 32);
            pnlTop.Controls.Add(lblTitre);

            // TABCONTROL
            tabMain.Dock = DockStyle.Fill; tabMain.Font = new Font("Segoe UI", 10F);
            tabRapport.Text = "Rapport de caisse"; tabSessions.Text = "Sessions caisse";
            tabMain.TabPages.AddRange(new TabPage[] { tabRapport, tabSessions });

            // FILTRES
            pnlFiltres.Dock = DockStyle.Top; pnlFiltres.Height = 52; pnlFiltres.BackColor = Color.FromArgb(245, 250, 245);
            lblPeriodeLabel.Text = "Periode :"; lblPeriodeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeriodeLabel.Location = new Point(14, 16); lblPeriodeLabel.Size = new Size(65, 22);
            cbPeriode.Location = new Point(82, 13); cbPeriode.Size = new Size(150, 28);
            cbPeriode.DropDownStyle = ComboBoxStyle.DropDownList; cbPeriode.Font = new Font("Segoe UI", 9.5F);
            cbPeriode.Items.AddRange(new object[] { "Aujourd'hui", "Cette semaine", "Ce mois", "Personnalisé" });
            dtpDebut.Location = new Point(244, 13); dtpDebut.Size = new Size(140, 28); dtpDebut.Format = DateTimePickerFormat.Short; dtpDebut.Visible = false;
            dtpFin.Location = new Point(394, 13); dtpFin.Size = new Size(140, 28); dtpFin.Format = DateTimePickerFormat.Short; dtpFin.Visible = false;
            cbPeriode.SelectedIndexChanged += (s, e) => { bool c = cbPeriode.SelectedItem?.ToString() == "Personnalisé"; dtpDebut.Visible = c; dtpFin.Visible = c; };
            btnActualiser.Text = "Actualiser"; btnActualiser.Location = new Point(546, 12); btnActualiser.Size = new Size(120, 30);
            btnActualiser.BackColor = Color.FromArgb(46, 125, 50); btnActualiser.ForeColor = Color.White;
            btnActualiser.FlatStyle = FlatStyle.Flat; btnActualiser.FlatAppearance.BorderSize = 0;
            btnActualiser.Font = new Font("Segoe UI", 9F, FontStyle.Bold); btnActualiser.Click += btnActualiser_Click;
            lblPeriodeAffichee.Text = ""; lblPeriodeAffichee.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPeriodeAffichee.ForeColor = Color.DimGray; lblPeriodeAffichee.Location = new Point(680, 17); lblPeriodeAffichee.Size = new Size(400, 20);
            pnlFiltres.Controls.AddRange(new Control[] { lblPeriodeLabel, cbPeriode, dtpDebut, dtpFin, btnActualiser, lblPeriodeAffichee });

            // CARTES
            pnlCartes.Dock = DockStyle.Top; pnlCartes.Height = 120; pnlCartes.BackColor = Color.White;
            // 6 cartes sur une seule ligne, taille réduite pour tenir
            void MakeCard(Panel card, Label title, Label value, string titleText, Color bg, int x)
            {
                card.Size = new Size(196, 100); card.Location = new Point(x, 8); card.BackColor = bg;
                title.Text = titleText; title.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                title.ForeColor = Color.White; title.Location = new Point(8, 8); title.Size = new Size(180, 18);
                value.Text = "0"; value.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                value.ForeColor = Color.White; value.Location = new Point(4, 30); value.Size = new Size(188, 28); value.TextAlign = ContentAlignment.MiddleCenter;
                card.Controls.Add(title); card.Controls.Add(value); pnlCartes.Controls.Add(card);
            }
            int cw = 196, cg = 8, cx0 = 8;
            MakeCard(cardVentes, lblNbVentesTitle, lblNbVentes, "Ventes", Color.FromArgb(69, 90, 100), cx0 + 0 * (cw + cg));
            MakeCard(cardComptant, lblCaComptantTitle, lblCaComptant, "CA Espèces", Color.FromArgb(46, 125, 50), cx0 + 1 * (cw + cg));
            MakeCard(cardCredit, lblCaCreditTitle, lblCaCredit, "CA Crédit", Color.FromArgb(211, 84, 0), cx0 + 2 * (cw + cg));
            MakeCard(cardMutuelle, lblCaMutuelleTitle, lblCaMutuelle, "CA Mutuelle", Color.FromArgb(25, 118, 210), cx0 + 3 * (cw + cg));
            MakeCard(cardCB, lblCaCBTitle, lblCaCB, "CA Carte/CB", Color.FromArgb(106, 27, 154), cx0 + 4 * (cw + cg));
            MakeCard(cardCheque, lblCaChequeTitle, lblCaCheque, "CA Chèque", Color.FromArgb(0, 131, 143), cx0 + 5 * (cw + cg));

            // PANNEAU CAISSE (droite)
            pnlCaisse.Dock = DockStyle.Right; pnlCaisse.Width = 310; pnlCaisse.BackColor = Color.FromArgb(250, 253, 250); pnlCaisse.Padding = new Padding(16);
            pnlCaisse.Paint += (s, e) => e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(46, 125, 50)), 0, 0, 4, pnlCaisse.Height);
            var lblTitreCaisse = new Label { Text = "Ce qui doit etre en caisse", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(34, 85, 34), Location = new Point(16, 14), Size = new Size(280, 24) };
            pnlCaisse.Controls.Add(lblTitreCaisse);
            int cy = 50, ch = 34;
            void LigneCaisse(Label lt, string t, Label lv, string dv, int row, Color col)
            {
                lt.Text = t; lt.Font = new Font("Segoe UI", 8.5F); lt.ForeColor = Color.FromArgb(80, 80, 80); lt.Location = new Point(16, cy + row * ch); lt.Size = new Size(160, 22);
                lv.Text = dv; lv.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); lv.ForeColor = col; lv.Location = new Point(176, cy + row * ch); lv.Size = new Size(120, 22); lv.TextAlign = ContentAlignment.MiddleRight;
                pnlCaisse.Controls.Add(lt); pnlCaisse.Controls.Add(lv);
            }
            LigneCaisse(lblEspecesTitle, "💵 Espèces comptant :", lblEspecesRecues, "0 KMF", 0, Color.ForestGreen);
            LigneCaisse(lblRenduTitle, "🏥 Part patient mutuelle :", lblRendu, "0 KMF", 1, Color.FromArgb(25, 118, 210));
            LigneCaisse(lblCreditRecuTitle, "📋 Avances crédit reçues :", lblCreditRecu, "0 KMF", 2, Color.FromArgb(211, 84, 0));
            LigneCaisse(lblMutuellePatientTitle, "⏳ Mutuelle entreprise due :", lblMutuellePatient, "—", 3, Color.Gray);
            lblSeparateur.Text = ""; lblSeparateur.BorderStyle = BorderStyle.Fixed3D; lblSeparateur.Location = new Point(16, cy + 4 * ch + 8); lblSeparateur.Size = new Size(278, 2); pnlCaisse.Controls.Add(lblSeparateur);
            lblTotalTitle.Text = "TOTAL ATTENDU EN CAISSE"; lblTotalTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblTotalTitle.ForeColor = Color.FromArgb(34, 85, 34); lblTotalTitle.Location = new Point(16, cy + 4 * ch + 18); lblTotalTitle.Size = new Size(278, 20); pnlCaisse.Controls.Add(lblTotalTitle);
            lblTotalCaisse.Text = "0 KMF"; lblTotalCaisse.Font = new Font("Segoe UI", 18F, FontStyle.Bold); lblTotalCaisse.ForeColor = Color.ForestGreen; lblTotalCaisse.Location = new Point(16, cy + 4 * ch + 42); lblTotalCaisse.Size = new Size(278, 66); lblTotalCaisse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; pnlCaisse.Controls.Add(lblTotalCaisse);

            // GRILLE VENTILATION (NOUVEAU)
            dgvVentilation.AllowUserToAddRows = false; dgvVentilation.AllowUserToDeleteRows = false; dgvVentilation.ReadOnly = true;
            dgvVentilation.Dock = DockStyle.Fill; dgvVentilation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentilation.RowHeadersVisible = false; dgvVentilation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentilation.BackgroundColor = Color.White; dgvVentilation.BorderStyle = BorderStyle.None;
            dgvVentilation.Font = new Font("Segoe UI", 9.5F); dgvVentilation.RowTemplate.Height = 34; dgvVentilation.ColumnHeadersHeight = 36;
            dgvVentilation.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 85, 34); dgvVentilation.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVentilation.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); dgvVentilation.EnableHeadersVisualStyles = false;
            dgvVentilation.Name = "dgvVentilation";
            gbVentilation.Controls.Add(dgvVentilation);
            gbVentilation.Dock = DockStyle.Top; gbVentilation.Height = 260;
            gbVentilation.Text = "Ventilation par mode de paiement"; gbVentilation.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbVentilation.ForeColor = Color.FromArgb(34, 85, 34); gbVentilation.Name = "gbVentilation";

            // GRILLE DETAIL
            dgvDetail.Dock = DockStyle.Fill; dgvDetail.ReadOnly = true; dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetail.AllowUserToAddRows = false; dgvDetail.RowHeadersVisible = false; dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetail.BackgroundColor = Color.White; dgvDetail.BorderStyle = BorderStyle.None; dgvDetail.Font = new Font("Segoe UI", 9F); dgvDetail.RowTemplate.Height = 32; dgvDetail.ColumnHeadersHeight = 36;
            dgvDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46); dgvDetail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); dgvDetail.EnableHeadersVisualStyles = false;
            dgvDetail.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);

            // Ordre important pour Dock
            tabRapport.Controls.Add(dgvDetail);
            tabRapport.Controls.Add(pnlCaisse);
            tabRapport.Controls.Add(gbVentilation);
            tabRapport.Controls.Add(pnlCartes);
            tabRapport.Controls.Add(pnlFiltres);

            // SESSIONS - FILTRES
            pnlFiltresSessions.Dock = DockStyle.Top; pnlFiltresSessions.Height = 52; pnlFiltresSessions.BackColor = Color.FromArgb(245, 250, 245);
            void LblCombo(Label l, string t, ComboBox cb, int x)
            {
                l.Text = t; l.Location = new Point(x, 16); l.Size = new Size(55, 20); l.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                cb.Location = new Point(x + 58, 13); cb.Size = new Size(120, 28); cb.DropDownStyle = ComboBoxStyle.DropDownList; cb.Font = new Font("Segoe UI", 9F);
                pnlFiltresSessions.Controls.Add(l); pnlFiltresSessions.Controls.Add(cb);
            }
            LblCombo(lblFiltreAnnee, "Annee :", cbFiltreAnnee, 10);
            LblCombo(lblFiltreMois, "Mois :", cbFiltreMois, 200);
            LblCombo(lblFiltreJour, "Jour :", cbFiltreJour, 390);
            LblCombo(lblFiltreUser, "Caissier :", cbFiltreUser, 530); cbFiltreUser.Size = new Size(180, 28);
            btnActualiserSessions.Text = "Actualiser"; btnActualiserSessions.Location = new Point(730, 12); btnActualiserSessions.Size = new Size(120, 30);
            btnActualiserSessions.BackColor = Color.FromArgb(46, 125, 50); btnActualiserSessions.ForeColor = Color.White;
            btnActualiserSessions.FlatStyle = FlatStyle.Flat; btnActualiserSessions.FlatAppearance.BorderSize = 0;
            btnActualiserSessions.Font = new Font("Segoe UI", 9F, FontStyle.Bold); btnActualiserSessions.Click += btnActualiserSessions_Click;
            lblNbSessions.Text = "0 session(s)"; lblNbSessions.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic); lblNbSessions.ForeColor = Color.DimGray; lblNbSessions.Location = new Point(862, 17); lblNbSessions.Size = new Size(200, 20);
            pnlFiltresSessions.Controls.Add(btnActualiserSessions); pnlFiltresSessions.Controls.Add(lblNbSessions);

            // SPLIT SESSIONS - PAS de SplitterDistance ici
            splitSessions.Dock = DockStyle.Fill; splitSessions.Orientation = Orientation.Horizontal;
            splitSessions.Panel1MinSize = 150; splitSessions.Panel2MinSize = 120;

            // Colonnes sessions
            colSessionId.Name = "colSessionId"; colSessionId.HeaderText = "ID"; colSessionId.Visible = false;
            colCaissier.Name = "colCaissier"; colCaissier.HeaderText = "Caissier";
            colOuverture.Name = "colOuverture"; colOuverture.HeaderText = "Ouverture";
            colCloture.Name = "colCloture"; colCloture.HeaderText = "Cloture";
            colStatutSession.Name = "colStatutSession"; colStatutSession.HeaderText = "Statut";
            colFond.Name = "colFond"; colFond.HeaderText = "Fond (KMF)";
            colEncaisse.Name = "colEncaisse"; colEncaisse.HeaderText = "Encaisse (KMF)";
            colTheorique.Name = "colTheorique"; colTheorique.HeaderText = "Theorique (KMF)";
            colCompte.Name = "colCompte"; colCompte.HeaderText = "Compte (KMF)";
            colEcart.Name = "colEcart"; colEcart.HeaderText = "Ecart (KMF)";
            colNbVentes.Name = "colNbVentes"; colNbVentes.HeaderText = "Nb Ventes";
            dgvSessions.Columns.AddRange(new DataGridViewColumn[] { colSessionId, colCaissier, colOuverture, colCloture, colStatutSession, colFond, colEncaisse, colTheorique, colCompte, colEcart, colNbVentes });
            dgvSessions.Dock = DockStyle.Fill; dgvSessions.ReadOnly = true; dgvSessions.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvSessions.MultiSelect = false;
            dgvSessions.AllowUserToAddRows = false; dgvSessions.RowHeadersVisible = false; dgvSessions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSessions.BackgroundColor = Color.White; dgvSessions.BorderStyle = BorderStyle.None; dgvSessions.Font = new Font("Segoe UI", 9F); dgvSessions.RowTemplate.Height = 34; dgvSessions.ColumnHeadersHeight = 36;
            dgvSessions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 85, 34); dgvSessions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSessions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); dgvSessions.EnableHeadersVisualStyles = false;
            dgvSessions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            splitSessions.Panel1.Controls.Add(dgvSessions);

            // Recaps session
            lblDetailSession.Dock = DockStyle.Top; lblDetailSession.Height = 30;
            lblDetailSession.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblDetailSession.ForeColor = Color.FromArgb(27, 94, 32);
            lblDetailSession.BackColor = Color.FromArgb(232, 245, 233); lblDetailSession.Text = "Selectionnez une session pour voir le detail";
            lblDetailSession.TextAlign = ContentAlignment.MiddleLeft; lblDetailSession.Padding = new Padding(8, 0, 0, 0);

            // NOUVEAU - ligne ventilation session
            lblVentilationSession.Dock = DockStyle.Top; lblVentilationSession.Height = 26;
            lblVentilationSession.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold); lblVentilationSession.ForeColor = Color.FromArgb(25, 118, 210);
            lblVentilationSession.BackColor = Color.FromArgb(227, 242, 253); lblVentilationSession.Text = "Especes : -  |  Mobile : -  |  CB : -  |  Cheque : -  |  Mutuelle : -  |  Credit : -  (KMF)";
            lblVentilationSession.TextAlign = ContentAlignment.MiddleLeft; lblVentilationSession.Padding = new Padding(8, 0, 0, 0); lblVentilationSession.Name = "lblVentilationSession";

            dgvDetailSession.Dock = DockStyle.Fill; dgvDetailSession.ReadOnly = true; dgvDetailSession.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetailSession.AllowUserToAddRows = false; dgvDetailSession.RowHeadersVisible = false; dgvDetailSession.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetailSession.BackgroundColor = Color.White; dgvDetailSession.BorderStyle = BorderStyle.None; dgvDetailSession.Font = new Font("Segoe UI", 9F); dgvDetailSession.RowTemplate.Height = 32; dgvDetailSession.ColumnHeadersHeight = 34;
            dgvDetailSession.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(69, 90, 100); dgvDetailSession.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetailSession.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); dgvDetailSession.EnableHeadersVisualStyles = false;
            dgvDetailSession.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);

            // Ordre : Fill en dernier dans Panel2
            splitSessions.Panel2.Controls.Add(dgvDetailSession);
            splitSessions.Panel2.Controls.Add(lblVentilationSession);
            splitSessions.Panel2.Controls.Add(lblDetailSession);

            tabSessions.Controls.Add(splitSessions);
            tabSessions.Controls.Add(pnlFiltresSessions);

            // ASSEMBLAGE
            gbVentilation.ResumeLayout(false);
            splitSessions.Panel1.ResumeLayout(false);
            splitSessions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVentilation).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetail).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSessions).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetailSession).EndInit();
            ((System.ComponentModel.ISupportInitialize)splitSessions).EndInit();
            tabMain.ResumeLayout(false);

            Controls.Add(tabMain);
            Controls.Add(pnlTop);

            Size = new Size(1200, 780);
            BackColor = Color.White;

            // SplitterDistance au Load pour eviter l'exception
            this.Load += (s, e) =>
            {
                try
                {
                    int dist = (int)(splitSessions.Height * 0.45);
                    if (dist > splitSessions.Panel1MinSize && dist < splitSessions.Height - splitSessions.Panel2MinSize)
                        splitSessions.SplitterDistance = dist;
                }
                catch { }
            };

            ResumeLayout(false);
        }
    }
}