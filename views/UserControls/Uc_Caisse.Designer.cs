namespace Pharmacie2.views.UserControls
{
    partial class Uc_Caisse
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabRapport = new System.Windows.Forms.TabPage();
            this.tlpRapport = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltres = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPeriodeLabel = new System.Windows.Forms.Label();
            this.cbPeriode = new System.Windows.Forms.ComboBox();
            this.dtpDebut = new System.Windows.Forms.DateTimePicker();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.lblPeriodeAffichee = new System.Windows.Forms.Label();
            this.pnlCartes = new System.Windows.Forms.TableLayoutPanel();
            this.cardVentes = new Pharmacie2.views.Composants.CarteKpi();
            this.cardComptant = new Pharmacie2.views.Composants.CarteKpi();
            this.cardCredit = new Pharmacie2.views.Composants.CarteKpi();
            this.cardMutuelle = new Pharmacie2.views.Composants.CarteKpi();
            this.cardCB = new Pharmacie2.views.Composants.CarteKpi();
            this.cardCheque = new Pharmacie2.views.Composants.CarteKpi();
            this.pnlCaisse = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitreCaisse = new System.Windows.Forms.Label();
            this.lblEspecesTitle = new System.Windows.Forms.Label();
            this.lblEspecesRecues = new System.Windows.Forms.Label();
            this.lblRenduTitle = new System.Windows.Forms.Label();
            this.lblRendu = new System.Windows.Forms.Label();
            this.lblCreditRecuTitle = new System.Windows.Forms.Label();
            this.lblCreditRecu = new System.Windows.Forms.Label();
            this.lblMutuellePatientTitle = new System.Windows.Forms.Label();
            this.lblMutuellePatient = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.lblTotalCaisse = new System.Windows.Forms.Label();
            this.lblElectronique = new System.Windows.Forms.Label();
            this.tlpVentilation = new System.Windows.Forms.TableLayoutPanel();
            this.lblVentilation = new System.Windows.Forms.Label();
            this.dgvVentilation = new System.Windows.Forms.DataGridView();
            this.lblDetailVentes = new System.Windows.Forms.Label();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.tabSessions = new System.Windows.Forms.TabPage();
            this.tlpSessions = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltresSessions = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFiltreAnnee = new System.Windows.Forms.Label();
            this.cbFiltreAnnee = new System.Windows.Forms.ComboBox();
            this.lblFiltreMois = new System.Windows.Forms.Label();
            this.cbFiltreMois = new System.Windows.Forms.ComboBox();
            this.lblFiltreJour = new System.Windows.Forms.Label();
            this.cbFiltreJour = new System.Windows.Forms.ComboBox();
            this.lblFiltreUser = new System.Windows.Forms.Label();
            this.cbFiltreUser = new System.Windows.Forms.ComboBox();
            this.btnActualiserSessions = new System.Windows.Forms.Button();
            this.lblNbSessions = new System.Windows.Forms.Label();
            this.dgvSessions = new System.Windows.Forms.DataGridView();
            this.lblDetailSession = new System.Windows.Forms.Label();
            this.lblVentilationSession = new System.Windows.Forms.Label();
            this.dgvDetailSession = new System.Windows.Forms.DataGridView();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNbVentes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPctCA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailMotif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailEspeces = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailRendu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailVendeur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCaissier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOuverture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCloture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessionStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEncaisse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTheorique = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEcart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessionNbVentes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessHeure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessVerse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessReste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentilation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSessions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailSession)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabRapport.SuspendLayout();
            this.tlpRapport.SuspendLayout();
            this.pnlFiltres.SuspendLayout();
            this.pnlCartes.SuspendLayout();
            this.pnlCaisse.SuspendLayout();
            this.tlpVentilation.SuspendLayout();
            this.tabSessions.SuspendLayout();
            this.tlpSessions.SuspendLayout();
            this.pnlFiltresSessions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 2;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tabMain, 0, 1);
            this.tabMain.TabIndex = 1;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Caisse";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabRapport);
            this.tabRapport.TabIndex = 0;
            this.tabMain.Controls.Add(this.tabSessions);
            this.tabSessions.TabIndex = 1;
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tabMain.Name = "tabMain";
            // 
            // tabRapport
            // 
            this.tabRapport.Controls.Add(this.tlpRapport);
            this.tlpRapport.TabIndex = 0;
            this.tabRapport.Text = "Rapport de caisse";
            this.tabRapport.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabRapport.UseVisualStyleBackColor = true;
            this.tabRapport.Name = "tabRapport";
            // 
            // tlpRapport
            // 
            this.tlpRapport.ColumnCount = 2;
            this.tlpRapport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlpRapport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64F));
            this.tlpRapport.RowCount = 5;
            this.tlpRapport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRapport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRapport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpRapport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRapport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpRapport.Controls.Add(this.pnlFiltres, 0, 0);
            this.pnlFiltres.TabIndex = 0;
            this.tlpRapport.SetColumnSpan(this.pnlFiltres, 2);
            this.tlpRapport.Controls.Add(this.pnlCartes, 0, 1);
            this.pnlCartes.TabIndex = 1;
            this.tlpRapport.SetColumnSpan(this.pnlCartes, 2);
            this.tlpRapport.Controls.Add(this.pnlCaisse, 0, 2);
            this.pnlCaisse.TabIndex = 2;
            this.tlpRapport.SetRowSpan(this.pnlCaisse, 3);
            this.tlpRapport.Controls.Add(this.tlpVentilation, 1, 2);
            this.tlpVentilation.TabIndex = 3;
            this.tlpRapport.Controls.Add(this.lblDetailVentes, 1, 3);
            this.lblDetailVentes.TabIndex = 4;
            this.tlpRapport.Controls.Add(this.dgvDetail, 1, 4);
            this.dgvDetail.TabIndex = 5;
            this.tlpRapport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRapport.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRapport.Name = "tlpRapport";
            // 
            // pnlFiltres
            // 
            this.pnlFiltres.Controls.Add(this.lblPeriodeLabel);
            this.lblPeriodeLabel.TabIndex = 0;
            this.pnlFiltres.Controls.Add(this.cbPeriode);
            this.cbPeriode.TabIndex = 1;
            this.pnlFiltres.Controls.Add(this.dtpDebut);
            this.dtpDebut.TabIndex = 2;
            this.pnlFiltres.Controls.Add(this.dtpFin);
            this.dtpFin.TabIndex = 3;
            this.pnlFiltres.Controls.Add(this.btnActualiser);
            this.btnActualiser.TabIndex = 4;
            this.pnlFiltres.Controls.Add(this.lblPeriodeAffichee);
            this.lblPeriodeAffichee.TabIndex = 5;
            this.pnlFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltres.AutoSize = true;
            this.pnlFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFiltres.WrapContents = true;
            this.pnlFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlFiltres.Name = "pnlFiltres";
            // 
            // lblPeriodeLabel
            // 
            this.lblPeriodeLabel.Text = "Période";
            this.lblPeriodeLabel.AutoSize = true;
            this.lblPeriodeLabel.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblPeriodeLabel.Name = "lblPeriodeLabel";
            // 
            // cbPeriode
            // 
            this.cbPeriode.Width = 170;
            this.cbPeriode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPeriode.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbPeriode.Items.AddRange(new object[] {
            "Aujourd'hui",
            "Cette semaine",
            "Ce mois",
            "Personnalisé"});
            this.cbPeriode.Name = "cbPeriode";
            // 
            // dtpDebut
            // 
            this.dtpDebut.Width = 130;
            this.dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDebut.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.dtpDebut.Name = "dtpDebut";
            // 
            // dtpFin
            // 
            this.dtpFin.Width = 130;
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFin.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.dtpFin.Name = "dtpFin";
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Tag = "primaire";
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiser.Name = "btnActualiser";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);
            // 
            // lblPeriodeAffichee
            // 
            this.lblPeriodeAffichee.Text = "";
            this.lblPeriodeAffichee.AutoSize = true;
            this.lblPeriodeAffichee.Tag = "note";
            this.lblPeriodeAffichee.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblPeriodeAffichee.Name = "lblPeriodeAffichee";
            // 
            // pnlCartes
            // 
            this.pnlCartes.ColumnCount = 6;
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.0F));
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.0F));
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.0F));
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.0F));
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.0F));
            this.pnlCartes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.0F));
            this.pnlCartes.RowCount = 1;
            this.pnlCartes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCartes.Controls.Add(this.cardVentes, 0, 0);
            this.cardVentes.TabIndex = 0;
            this.pnlCartes.Controls.Add(this.cardComptant, 1, 0);
            this.cardComptant.TabIndex = 1;
            this.pnlCartes.Controls.Add(this.cardCredit, 2, 0);
            this.cardCredit.TabIndex = 2;
            this.pnlCartes.Controls.Add(this.cardMutuelle, 3, 0);
            this.cardMutuelle.TabIndex = 3;
            this.pnlCartes.Controls.Add(this.cardCB, 4, 0);
            this.cardCB.TabIndex = 4;
            this.pnlCartes.Controls.Add(this.cardCheque, 5, 0);
            this.cardCheque.TabIndex = 5;
            this.pnlCartes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCartes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.pnlCartes.Name = "pnlCartes";
            // 
            // cardVentes
            // 
            this.cardVentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardVentes.Titre = "Ventes";
            this.cardVentes.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.cardVentes.Name = "cardVentes";
            // 
            // cardComptant
            // 
            this.cardComptant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardComptant.Titre = "Espèces (comptant)";
            this.cardComptant.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.cardComptant.Name = "cardComptant";
            // 
            // cardCredit
            // 
            this.cardCredit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardCredit.Titre = "Crédits";
            this.cardCredit.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.cardCredit.Name = "cardCredit";
            // 
            // cardMutuelle
            // 
            this.cardMutuelle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardMutuelle.Titre = "Mutuelles";
            this.cardMutuelle.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.cardMutuelle.Name = "cardMutuelle";
            // 
            // cardCB
            // 
            this.cardCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardCB.Titre = "Carte bancaire";
            this.cardCB.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.cardCB.Name = "cardCB";
            // 
            // cardCheque
            // 
            this.cardCheque.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardCheque.Titre = "Chèques";
            this.cardCheque.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cardCheque.Name = "cardCheque";
            // 
            // pnlCaisse
            // 
            this.pnlCaisse.ColumnCount = 2;
            this.pnlCaisse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlCaisse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowCount = 7;
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlCaisse.Controls.Add(this.lblTitreCaisse, 0, 0);
            this.lblTitreCaisse.TabIndex = 0;
            this.pnlCaisse.SetColumnSpan(this.lblTitreCaisse, 2);
            this.pnlCaisse.Controls.Add(this.lblEspecesTitle, 0, 1);
            this.lblEspecesTitle.TabIndex = 1;
            this.pnlCaisse.Controls.Add(this.lblEspecesRecues, 1, 1);
            this.lblEspecesRecues.TabIndex = 2;
            this.pnlCaisse.Controls.Add(this.lblRenduTitle, 0, 2);
            this.lblRenduTitle.TabIndex = 3;
            this.pnlCaisse.Controls.Add(this.lblRendu, 1, 2);
            this.lblRendu.TabIndex = 4;
            this.pnlCaisse.Controls.Add(this.lblCreditRecuTitle, 0, 3);
            this.lblCreditRecuTitle.TabIndex = 5;
            this.pnlCaisse.Controls.Add(this.lblCreditRecu, 1, 3);
            this.lblCreditRecu.TabIndex = 6;
            this.pnlCaisse.Controls.Add(this.lblMutuellePatientTitle, 0, 4);
            this.lblMutuellePatientTitle.TabIndex = 7;
            this.pnlCaisse.Controls.Add(this.lblMutuellePatient, 1, 4);
            this.lblMutuellePatient.TabIndex = 8;
            this.pnlCaisse.Controls.Add(this.lblTotalTitle, 0, 5);
            this.lblTotalTitle.TabIndex = 9;
            this.pnlCaisse.Controls.Add(this.lblTotalCaisse, 1, 5);
            this.lblTotalCaisse.TabIndex = 10;
            this.pnlCaisse.Controls.Add(this.lblElectronique, 0, 6);
            this.lblElectronique.TabIndex = 11;
            this.pnlCaisse.SetColumnSpan(this.lblElectronique, 2);
            this.pnlCaisse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCaisse.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlCaisse.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlCaisse.Tag = "carte";
            this.pnlCaisse.Name = "pnlCaisse";
            // 
            // lblTitreCaisse
            // 
            this.lblTitreCaisse.Text = "Ce qui doit être dans le tiroir";
            this.lblTitreCaisse.AutoSize = true;
            this.lblTitreCaisse.Tag = "section";
            this.lblTitreCaisse.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblTitreCaisse.Name = "lblTitreCaisse";
            // 
            // lblEspecesTitle
            // 
            this.lblEspecesTitle.Text = "Espèces des ventes au comptant";
            this.lblEspecesTitle.AutoSize = true;
            this.lblEspecesTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEspecesTitle.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblEspecesTitle.Name = "lblEspecesTitle";
            // 
            // lblEspecesRecues
            // 
            this.lblEspecesRecues.Text = "";
            this.lblEspecesRecues.AutoSize = true;
            this.lblEspecesRecues.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEspecesRecues.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEspecesRecues.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblEspecesRecues.Name = "lblEspecesRecues";
            // 
            // lblRenduTitle
            // 
            this.lblRenduTitle.Text = "Part des patients (mutuelle)";
            this.lblRenduTitle.AutoSize = true;
            this.lblRenduTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRenduTitle.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblRenduTitle.Name = "lblRenduTitle";
            // 
            // lblRendu
            // 
            this.lblRendu.Text = "";
            this.lblRendu.AutoSize = true;
            this.lblRendu.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRendu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRendu.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblRendu.Name = "lblRendu";
            // 
            // lblCreditRecuTitle
            // 
            this.lblCreditRecuTitle.Text = "Versements sur crédits";
            this.lblCreditRecuTitle.AutoSize = true;
            this.lblCreditRecuTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCreditRecuTitle.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblCreditRecuTitle.Name = "lblCreditRecuTitle";
            // 
            // lblCreditRecu
            // 
            this.lblCreditRecu.Text = "";
            this.lblCreditRecu.AutoSize = true;
            this.lblCreditRecu.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCreditRecu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditRecu.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblCreditRecu.Name = "lblCreditRecu";
            // 
            // lblMutuellePatientTitle
            // 
            this.lblMutuellePatientTitle.Text = "Part des mutuelles";
            this.lblMutuellePatientTitle.AutoSize = true;
            this.lblMutuellePatientTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMutuellePatientTitle.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblMutuellePatientTitle.Name = "lblMutuellePatientTitle";
            // 
            // lblMutuellePatient
            // 
            this.lblMutuellePatient.Text = "";
            this.lblMutuellePatient.AutoSize = true;
            this.lblMutuellePatient.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMutuellePatient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMutuellePatient.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblMutuellePatient.Name = "lblMutuellePatient";
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Text = "Total attendu en espèces";
            this.lblTotalTitle.AutoSize = true;
            this.lblTotalTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTitle.Margin = new System.Windows.Forms.Padding(0, 8, 12, 0);
            this.lblTotalTitle.Name = "lblTotalTitle";
            // 
            // lblTotalCaisse
            // 
            this.lblTotalCaisse.Text = "";
            this.lblTotalCaisse.AutoSize = true;
            this.lblTotalCaisse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalCaisse.Tag = "kpi";
            this.lblTotalCaisse.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTotalCaisse.Name = "lblTotalCaisse";
            // 
            // lblElectronique
            // 
            this.lblElectronique.Text = "";
            this.lblElectronique.AutoSize = true;
            this.lblElectronique.Tag = "note";
            this.lblElectronique.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblElectronique.Name = "lblElectronique";
            // 
            // tlpVentilation
            // 
            this.tlpVentilation.ColumnCount = 1;
            this.tlpVentilation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpVentilation.RowCount = 2;
            this.tlpVentilation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpVentilation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpVentilation.Controls.Add(this.lblVentilation, 0, 0);
            this.lblVentilation.TabIndex = 0;
            this.tlpVentilation.Controls.Add(this.dgvVentilation, 0, 1);
            this.dgvVentilation.TabIndex = 1;
            this.tlpVentilation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVentilation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpVentilation.Name = "tlpVentilation";
            // 
            // lblVentilation
            // 
            this.lblVentilation.Text = "Ventes par moyen de paiement";
            this.lblVentilation.AutoSize = true;
            this.lblVentilation.Tag = "section";
            this.lblVentilation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblVentilation.Name = "lblVentilation";
            // 
            // dgvVentilation
            // 
            this.dgvVentilation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMode,
            this.colNbVentes,
            this.colMontant,
            this.colPctCA,
            this.colStatut});
            this.dgvVentilation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentilation.AutoGenerateColumns = false;
            this.dgvVentilation.ReadOnly = true;
            this.dgvVentilation.Name = "dgvVentilation";
            // 
            // lblDetailVentes
            // 
            this.lblDetailVentes.Text = "Détail des ventes de la période";
            this.lblDetailVentes.AutoSize = true;
            this.lblDetailVentes.Tag = "section";
            this.lblDetailVentes.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblDetailVentes.Name = "lblDetailVentes";
            // 
            // dgvDetail
            // 
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailDate,
            this.colDetailNumero,
            this.colDetailClient,
            this.colDetailMotif,
            this.colDetailType,
            this.colDetailStatut,
            this.colDetailTotal,
            this.colDetailEspeces,
            this.colDetailRendu,
            this.colDetailVendeur});
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.AutoGenerateColumns = false;
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDetail_CellFormatting);
            // 
            // tabSessions
            // 
            this.tabSessions.Controls.Add(this.tlpSessions);
            this.tlpSessions.TabIndex = 0;
            this.tabSessions.Text = "Sessions de caisse";
            this.tabSessions.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabSessions.UseVisualStyleBackColor = true;
            this.tabSessions.Name = "tabSessions";
            // 
            // tlpSessions
            // 
            this.tlpSessions.ColumnCount = 1;
            this.tlpSessions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpSessions.RowCount = 5;
            this.tlpSessions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSessions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0F));
            this.tlpSessions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSessions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSessions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0F));
            this.tlpSessions.Controls.Add(this.pnlFiltresSessions, 0, 0);
            this.pnlFiltresSessions.TabIndex = 0;
            this.tlpSessions.Controls.Add(this.dgvSessions, 0, 1);
            this.dgvSessions.TabIndex = 1;
            this.tlpSessions.Controls.Add(this.lblDetailSession, 0, 2);
            this.lblDetailSession.TabIndex = 2;
            this.tlpSessions.Controls.Add(this.lblVentilationSession, 0, 3);
            this.lblVentilationSession.TabIndex = 3;
            this.tlpSessions.Controls.Add(this.dgvDetailSession, 0, 4);
            this.dgvDetailSession.TabIndex = 4;
            this.tlpSessions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSessions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpSessions.Name = "tlpSessions";
            // 
            // pnlFiltresSessions
            // 
            this.pnlFiltresSessions.Controls.Add(this.lblFiltreAnnee);
            this.lblFiltreAnnee.TabIndex = 0;
            this.pnlFiltresSessions.Controls.Add(this.cbFiltreAnnee);
            this.cbFiltreAnnee.TabIndex = 1;
            this.pnlFiltresSessions.Controls.Add(this.lblFiltreMois);
            this.lblFiltreMois.TabIndex = 2;
            this.pnlFiltresSessions.Controls.Add(this.cbFiltreMois);
            this.cbFiltreMois.TabIndex = 3;
            this.pnlFiltresSessions.Controls.Add(this.lblFiltreJour);
            this.lblFiltreJour.TabIndex = 4;
            this.pnlFiltresSessions.Controls.Add(this.cbFiltreJour);
            this.cbFiltreJour.TabIndex = 5;
            this.pnlFiltresSessions.Controls.Add(this.lblFiltreUser);
            this.lblFiltreUser.TabIndex = 6;
            this.pnlFiltresSessions.Controls.Add(this.cbFiltreUser);
            this.cbFiltreUser.TabIndex = 7;
            this.pnlFiltresSessions.Controls.Add(this.btnActualiserSessions);
            this.btnActualiserSessions.TabIndex = 8;
            this.pnlFiltresSessions.Controls.Add(this.lblNbSessions);
            this.lblNbSessions.TabIndex = 9;
            this.pnlFiltresSessions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltresSessions.AutoSize = true;
            this.pnlFiltresSessions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFiltresSessions.WrapContents = true;
            this.pnlFiltresSessions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlFiltresSessions.Name = "pnlFiltresSessions";
            // 
            // lblFiltreAnnee
            // 
            this.lblFiltreAnnee.Text = "Année";
            this.lblFiltreAnnee.AutoSize = true;
            this.lblFiltreAnnee.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreAnnee.Name = "lblFiltreAnnee";
            // 
            // cbFiltreAnnee
            // 
            this.cbFiltreAnnee.Width = 90;
            this.cbFiltreAnnee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreAnnee.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbFiltreAnnee.Name = "cbFiltreAnnee";
            // 
            // lblFiltreMois
            // 
            this.lblFiltreMois.Text = "Mois";
            this.lblFiltreMois.AutoSize = true;
            this.lblFiltreMois.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreMois.Name = "lblFiltreMois";
            // 
            // cbFiltreMois
            // 
            this.cbFiltreMois.Width = 120;
            this.cbFiltreMois.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreMois.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbFiltreMois.Name = "cbFiltreMois";
            // 
            // lblFiltreJour
            // 
            this.lblFiltreJour.Text = "Jour";
            this.lblFiltreJour.AutoSize = true;
            this.lblFiltreJour.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreJour.Name = "lblFiltreJour";
            // 
            // cbFiltreJour
            // 
            this.cbFiltreJour.Width = 80;
            this.cbFiltreJour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreJour.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbFiltreJour.Name = "cbFiltreJour";
            // 
            // lblFiltreUser
            // 
            this.lblFiltreUser.Text = "Caissier";
            this.lblFiltreUser.AutoSize = true;
            this.lblFiltreUser.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreUser.Name = "lblFiltreUser";
            // 
            // cbFiltreUser
            // 
            this.cbFiltreUser.Width = 180;
            this.cbFiltreUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreUser.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbFiltreUser.Name = "cbFiltreUser";
            // 
            // btnActualiserSessions
            // 
            this.btnActualiserSessions.Text = "Actualiser";
            this.btnActualiserSessions.Tag = "primaire";
            this.btnActualiserSessions.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiserSessions.Name = "btnActualiserSessions";
            this.btnActualiserSessions.Click += new System.EventHandler(this.btnActualiserSessions_Click);
            // 
            // lblNbSessions
            // 
            this.lblNbSessions.Text = "";
            this.lblNbSessions.AutoSize = true;
            this.lblNbSessions.Tag = "note";
            this.lblNbSessions.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblNbSessions.Name = "lblNbSessions";
            // 
            // dgvSessions
            // 
            this.dgvSessions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSessionId,
            this.colCaissier,
            this.colOuverture,
            this.colCloture,
            this.colSessionStatut,
            this.colFond,
            this.colEncaisse,
            this.colTheorique,
            this.colCompte,
            this.colEcart,
            this.colSessionNbVentes});
            this.dgvSessions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSessions.AutoGenerateColumns = false;
            this.dgvSessions.ReadOnly = true;
            this.dgvSessions.Name = "dgvSessions";
            this.dgvSessions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSessions_CellFormatting);
            this.dgvSessions.SelectionChanged += new System.EventHandler(this.DgvSessions_SelectionChanged);
            // 
            // lblDetailSession
            // 
            this.lblDetailSession.Text = "Sélectionnez une session pour voir son détail";
            this.lblDetailSession.AutoSize = true;
            this.lblDetailSession.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailSession.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblDetailSession.Name = "lblDetailSession";
            // 
            // lblVentilationSession
            // 
            this.lblVentilationSession.Text = "";
            this.lblVentilationSession.AutoSize = true;
            this.lblVentilationSession.Tag = "info";
            this.lblVentilationSession.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblVentilationSession.Name = "lblVentilationSession";
            // 
            // dgvDetailSession
            // 
            this.dgvDetailSession.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSessHeure,
            this.colSessNumero,
            this.colSessClient,
            this.colSessType,
            this.colSessStatut,
            this.colSessTotal,
            this.colSessVerse,
            this.colSessReste});
            this.dgvDetailSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetailSession.AutoGenerateColumns = false;
            this.dgvDetailSession.ReadOnly = true;
            this.dgvDetailSession.Name = "dgvDetailSession";
            this.dgvDetailSession.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDetailSession_CellFormatting);
            // 
            // colMode
            // 
            this.colMode.HeaderText = "Moyen de paiement";
            this.colMode.Name = "Mode";
            this.colMode.DataPropertyName = "Mode";
            this.colMode.FillWeight = 34F;
            this.colMode.MinimumWidth = 140;
            this.colMode.ReadOnly = true;
            // 
            // colNbVentes
            // 
            this.colNbVentes.HeaderText = "Ventes";
            this.colNbVentes.Name = "NbVentes";
            this.colNbVentes.DataPropertyName = "NbVentes";
            this.colNbVentes.FillWeight = 10F;
            this.colNbVentes.MinimumWidth = 60;
            this.colNbVentes.ReadOnly = true;
            // 
            // colMontant
            // 
            this.colMontant.HeaderText = "Montant";
            this.colMontant.Name = "Montant";
            this.colMontant.DataPropertyName = "Montant";
            this.colMontant.FillWeight = 20F;
            this.colMontant.MinimumWidth = 100;
            this.colMontant.ReadOnly = true;
            this.colMontant.Tag = "montant";
            // 
            // colPctCA
            // 
            this.colPctCA.HeaderText = "Part (%)";
            this.colPctCA.Name = "PctCA";
            this.colPctCA.DataPropertyName = "PctCA";
            this.colPctCA.FillWeight = 10F;
            this.colPctCA.MinimumWidth = 60;
            this.colPctCA.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Situation";
            this.colStatut.Name = "Statut";
            this.colStatut.DataPropertyName = "Statut";
            this.colStatut.FillWeight = 26F;
            this.colStatut.MinimumWidth = 140;
            this.colStatut.ReadOnly = true;
            // 
            // colDetailDate
            // 
            this.colDetailDate.HeaderText = "Date";
            this.colDetailDate.Name = "Date";
            this.colDetailDate.DataPropertyName = "Date";
            this.colDetailDate.FillWeight = 13F;
            this.colDetailDate.MinimumWidth = 110;
            this.colDetailDate.ReadOnly = true;
            // 
            // colDetailNumero
            // 
            this.colDetailNumero.HeaderText = "N° de vente";
            this.colDetailNumero.Name = "Numero";
            this.colDetailNumero.DataPropertyName = "Numero";
            this.colDetailNumero.FillWeight = 10F;
            this.colDetailNumero.MinimumWidth = 90;
            this.colDetailNumero.ReadOnly = true;
            // 
            // colDetailClient
            // 
            this.colDetailClient.HeaderText = "Client";
            this.colDetailClient.Name = "Client";
            this.colDetailClient.DataPropertyName = "Client";
            this.colDetailClient.FillWeight = 17F;
            this.colDetailClient.MinimumWidth = 110;
            this.colDetailClient.ReadOnly = true;
            // 
            // colDetailMotif
            // 
            this.colDetailMotif.HeaderText = "Motif";
            this.colDetailMotif.Name = "Motif";
            this.colDetailMotif.DataPropertyName = "Motif";
            this.colDetailMotif.FillWeight = 16F;
            this.colDetailMotif.MinimumWidth = 100;
            this.colDetailMotif.ReadOnly = true;
            // 
            // colDetailType
            // 
            this.colDetailType.HeaderText = "Paiement";
            this.colDetailType.Name = "Type";
            this.colDetailType.DataPropertyName = "Type";
            this.colDetailType.FillWeight = 11F;
            this.colDetailType.MinimumWidth = 90;
            this.colDetailType.ReadOnly = true;
            // 
            // colDetailStatut
            // 
            this.colDetailStatut.HeaderText = "Statut";
            this.colDetailStatut.Name = "Statut";
            this.colDetailStatut.DataPropertyName = "Statut";
            this.colDetailStatut.FillWeight = 8F;
            this.colDetailStatut.MinimumWidth = 70;
            this.colDetailStatut.ReadOnly = true;
            // 
            // colDetailTotal
            // 
            this.colDetailTotal.HeaderText = "Total";
            this.colDetailTotal.Name = "Total";
            this.colDetailTotal.DataPropertyName = "Total";
            this.colDetailTotal.FillWeight = 9F;
            this.colDetailTotal.MinimumWidth = 90;
            this.colDetailTotal.ReadOnly = true;
            this.colDetailTotal.Tag = "montant";
            // 
            // colDetailEspeces
            // 
            this.colDetailEspeces.HeaderText = "Espèces reçues";
            this.colDetailEspeces.Name = "Especes";
            this.colDetailEspeces.DataPropertyName = "Especes";
            this.colDetailEspeces.FillWeight = 9F;
            this.colDetailEspeces.MinimumWidth = 90;
            this.colDetailEspeces.ReadOnly = true;
            this.colDetailEspeces.Tag = "montant";
            // 
            // colDetailRendu
            // 
            this.colDetailRendu.HeaderText = "Rendu";
            this.colDetailRendu.Name = "Rendu";
            this.colDetailRendu.DataPropertyName = "Rendu";
            this.colDetailRendu.FillWeight = 7F;
            this.colDetailRendu.MinimumWidth = 80;
            this.colDetailRendu.ReadOnly = true;
            this.colDetailRendu.Tag = "montant";
            // 
            // colDetailVendeur
            // 
            this.colDetailVendeur.HeaderText = "Vendeur";
            this.colDetailVendeur.Name = "Vendeur";
            this.colDetailVendeur.DataPropertyName = "Vendeur";
            this.colDetailVendeur.FillWeight = 12F;
            this.colDetailVendeur.MinimumWidth = 90;
            this.colDetailVendeur.ReadOnly = true;
            // 
            // colSessionId
            // 
            this.colSessionId.HeaderText = "Id";
            this.colSessionId.Name = "SessionId";
            this.colSessionId.DataPropertyName = "SessionId";
            this.colSessionId.FillWeight = 100F;
            this.colSessionId.MinimumWidth = 60;
            this.colSessionId.ReadOnly = true;
            this.colSessionId.Visible = false;
            // 
            // colCaissier
            // 
            this.colCaissier.HeaderText = "Caissier";
            this.colCaissier.Name = "Caissier";
            this.colCaissier.DataPropertyName = "Caissier";
            this.colCaissier.FillWeight = 16F;
            this.colCaissier.MinimumWidth = 100;
            this.colCaissier.ReadOnly = true;
            // 
            // colOuverture
            // 
            this.colOuverture.HeaderText = "Ouverture";
            this.colOuverture.Name = "Ouverture";
            this.colOuverture.DataPropertyName = "Ouverture";
            this.colOuverture.FillWeight = 13F;
            this.colOuverture.MinimumWidth = 110;
            this.colOuverture.ReadOnly = true;
            // 
            // colCloture
            // 
            this.colCloture.HeaderText = "Clôture";
            this.colCloture.Name = "Cloture";
            this.colCloture.DataPropertyName = "Cloture";
            this.colCloture.FillWeight = 13F;
            this.colCloture.MinimumWidth = 110;
            this.colCloture.ReadOnly = true;
            // 
            // colSessionStatut
            // 
            this.colSessionStatut.HeaderText = "Statut";
            this.colSessionStatut.Name = "Statut";
            this.colSessionStatut.DataPropertyName = "Statut";
            this.colSessionStatut.FillWeight = 8F;
            this.colSessionStatut.MinimumWidth = 70;
            this.colSessionStatut.ReadOnly = true;
            // 
            // colFond
            // 
            this.colFond.HeaderText = "Fond de caisse";
            this.colFond.Name = "Fond";
            this.colFond.DataPropertyName = "Fond";
            this.colFond.FillWeight = 9F;
            this.colFond.MinimumWidth = 90;
            this.colFond.ReadOnly = true;
            this.colFond.Tag = "montant";
            // 
            // colEncaisse
            // 
            this.colEncaisse.HeaderText = "Encaissé";
            this.colEncaisse.Name = "Encaisse";
            this.colEncaisse.DataPropertyName = "Encaisse";
            this.colEncaisse.FillWeight = 9F;
            this.colEncaisse.MinimumWidth = 90;
            this.colEncaisse.ReadOnly = true;
            this.colEncaisse.Tag = "montant";
            // 
            // colTheorique
            // 
            this.colTheorique.HeaderText = "Attendu";
            this.colTheorique.Name = "Theorique";
            this.colTheorique.DataPropertyName = "Theorique";
            this.colTheorique.FillWeight = 9F;
            this.colTheorique.MinimumWidth = 90;
            this.colTheorique.ReadOnly = true;
            this.colTheorique.Tag = "montant";
            // 
            // colCompte
            // 
            this.colCompte.HeaderText = "Compté";
            this.colCompte.Name = "Compte";
            this.colCompte.DataPropertyName = "Compte";
            this.colCompte.FillWeight = 9F;
            this.colCompte.MinimumWidth = 90;
            this.colCompte.ReadOnly = true;
            this.colCompte.Tag = "montant";
            // 
            // colEcart
            // 
            this.colEcart.HeaderText = "Écart";
            this.colEcart.Name = "Ecart";
            this.colEcart.DataPropertyName = "Ecart";
            this.colEcart.FillWeight = 8F;
            this.colEcart.MinimumWidth = 80;
            this.colEcart.ReadOnly = true;
            this.colEcart.Tag = "montant";
            // 
            // colSessionNbVentes
            // 
            this.colSessionNbVentes.HeaderText = "Ventes";
            this.colSessionNbVentes.Name = "NbVentes";
            this.colSessionNbVentes.DataPropertyName = "NbVentes";
            this.colSessionNbVentes.FillWeight = 6F;
            this.colSessionNbVentes.MinimumWidth = 60;
            this.colSessionNbVentes.ReadOnly = true;
            // 
            // colSessHeure
            // 
            this.colSessHeure.HeaderText = "Heure";
            this.colSessHeure.Name = "Heure";
            this.colSessHeure.DataPropertyName = "Heure";
            this.colSessHeure.FillWeight = 10F;
            this.colSessHeure.MinimumWidth = 80;
            this.colSessHeure.ReadOnly = true;
            // 
            // colSessNumero
            // 
            this.colSessNumero.HeaderText = "N° de vente";
            this.colSessNumero.Name = "Numero";
            this.colSessNumero.DataPropertyName = "Numero";
            this.colSessNumero.FillWeight = 12F;
            this.colSessNumero.MinimumWidth = 90;
            this.colSessNumero.ReadOnly = true;
            // 
            // colSessClient
            // 
            this.colSessClient.HeaderText = "Client";
            this.colSessClient.Name = "Client";
            this.colSessClient.DataPropertyName = "Client";
            this.colSessClient.FillWeight = 24F;
            this.colSessClient.MinimumWidth = 110;
            this.colSessClient.ReadOnly = true;
            // 
            // colSessType
            // 
            this.colSessType.HeaderText = "Paiement";
            this.colSessType.Name = "Type";
            this.colSessType.DataPropertyName = "Type";
            this.colSessType.FillWeight = 14F;
            this.colSessType.MinimumWidth = 90;
            this.colSessType.ReadOnly = true;
            // 
            // colSessStatut
            // 
            this.colSessStatut.HeaderText = "Statut";
            this.colSessStatut.Name = "Statut";
            this.colSessStatut.DataPropertyName = "Statut";
            this.colSessStatut.FillWeight = 10F;
            this.colSessStatut.MinimumWidth = 70;
            this.colSessStatut.ReadOnly = true;
            // 
            // colSessTotal
            // 
            this.colSessTotal.HeaderText = "Total";
            this.colSessTotal.Name = "Total";
            this.colSessTotal.DataPropertyName = "Total";
            this.colSessTotal.FillWeight = 10F;
            this.colSessTotal.MinimumWidth = 90;
            this.colSessTotal.ReadOnly = true;
            this.colSessTotal.Tag = "montant";
            // 
            // colSessVerse
            // 
            this.colSessVerse.HeaderText = "Versé";
            this.colSessVerse.Name = "Verse";
            this.colSessVerse.DataPropertyName = "Verse";
            this.colSessVerse.FillWeight = 10F;
            this.colSessVerse.MinimumWidth = 90;
            this.colSessVerse.ReadOnly = true;
            this.colSessVerse.Tag = "montant";
            // 
            // colSessReste
            // 
            this.colSessReste.HeaderText = "Reste";
            this.colSessReste.Name = "Reste";
            this.colSessReste.DataPropertyName = "Reste";
            this.colSessReste.FillWeight = 10F;
            this.colSessReste.MinimumWidth = 90;
            this.colSessReste.ReadOnly = true;
            this.colSessReste.Tag = "montant";
            // 
            // Uc_Caisse
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.MinimumSize = new System.Drawing.Size(960, 560);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Caisse";
            this.ResumeLayout(false);
            this.pnlFiltresSessions.ResumeLayout(false);
            this.pnlFiltresSessions.PerformLayout();
            this.tlpSessions.ResumeLayout(false);
            this.tlpSessions.PerformLayout();
            this.tabSessions.ResumeLayout(false);
            this.tlpVentilation.ResumeLayout(false);
            this.tlpVentilation.PerformLayout();
            this.pnlCaisse.ResumeLayout(false);
            this.pnlCaisse.PerformLayout();
            this.pnlCartes.ResumeLayout(false);
            this.pnlCartes.PerformLayout();
            this.pnlFiltres.ResumeLayout(false);
            this.pnlFiltres.PerformLayout();
            this.tlpRapport.ResumeLayout(false);
            this.tlpRapport.PerformLayout();
            this.tabRapport.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailSession)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSessions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentilation)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabRapport;
        private System.Windows.Forms.TableLayoutPanel tlpRapport;
        private System.Windows.Forms.FlowLayoutPanel pnlFiltres;
        private System.Windows.Forms.Label lblPeriodeLabel;
        private System.Windows.Forms.ComboBox cbPeriode;
        private System.Windows.Forms.DateTimePicker dtpDebut;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Label lblPeriodeAffichee;
        private System.Windows.Forms.TableLayoutPanel pnlCartes;
        private Pharmacie2.views.Composants.CarteKpi cardVentes;
        private Pharmacie2.views.Composants.CarteKpi cardComptant;
        private Pharmacie2.views.Composants.CarteKpi cardCredit;
        private Pharmacie2.views.Composants.CarteKpi cardMutuelle;
        private Pharmacie2.views.Composants.CarteKpi cardCB;
        private Pharmacie2.views.Composants.CarteKpi cardCheque;
        private System.Windows.Forms.TableLayoutPanel pnlCaisse;
        private System.Windows.Forms.Label lblTitreCaisse;
        private System.Windows.Forms.Label lblEspecesTitle;
        private System.Windows.Forms.Label lblEspecesRecues;
        private System.Windows.Forms.Label lblRenduTitle;
        private System.Windows.Forms.Label lblRendu;
        private System.Windows.Forms.Label lblCreditRecuTitle;
        private System.Windows.Forms.Label lblCreditRecu;
        private System.Windows.Forms.Label lblMutuellePatientTitle;
        private System.Windows.Forms.Label lblMutuellePatient;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotalCaisse;
        private System.Windows.Forms.Label lblElectronique;
        private System.Windows.Forms.TableLayoutPanel tlpVentilation;
        private System.Windows.Forms.Label lblVentilation;
        private System.Windows.Forms.DataGridView dgvVentilation;
        private System.Windows.Forms.Label lblDetailVentes;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.TabPage tabSessions;
        private System.Windows.Forms.TableLayoutPanel tlpSessions;
        private System.Windows.Forms.FlowLayoutPanel pnlFiltresSessions;
        private System.Windows.Forms.Label lblFiltreAnnee;
        private System.Windows.Forms.ComboBox cbFiltreAnnee;
        private System.Windows.Forms.Label lblFiltreMois;
        private System.Windows.Forms.ComboBox cbFiltreMois;
        private System.Windows.Forms.Label lblFiltreJour;
        private System.Windows.Forms.ComboBox cbFiltreJour;
        private System.Windows.Forms.Label lblFiltreUser;
        private System.Windows.Forms.ComboBox cbFiltreUser;
        private System.Windows.Forms.Button btnActualiserSessions;
        private System.Windows.Forms.Label lblNbSessions;
        private System.Windows.Forms.DataGridView dgvSessions;
        private System.Windows.Forms.Label lblDetailSession;
        private System.Windows.Forms.Label lblVentilationSession;
        private System.Windows.Forms.DataGridView dgvDetailSession;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNbVentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPctCA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailMotif;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailEspeces;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailRendu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailVendeur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCaissier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOuverture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCloture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEncaisse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTheorique;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEcart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionNbVentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessHeure;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessVerse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessReste;
    }
}
