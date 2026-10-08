namespace Pharmacie2.views.UserControls
{
    partial class Uc_Statistique
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
            this.flpPeriode = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPeriode = new System.Windows.Forms.Label();
            this.cbPeriode = new System.Windows.Forms.ComboBox();
            this.dtpDebut = new System.Windows.Forms.DateTimePicker();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.lblPeriodeAffichee = new System.Windows.Forms.Label();
            this.lblAlertes = new System.Windows.Forms.Label();
            this.listeAlertes = new Pharmacie2.views.Composants.ListeActions();
            this.tlpKpi = new System.Windows.Forms.TableLayoutPanel();
            this.carteCA = new Pharmacie2.views.Composants.CarteKpi();
            this.carteBenefice = new Pharmacie2.views.Composants.CarteKpi();
            this.carteVentes = new Pharmacie2.views.Composants.CarteKpi();
            this.carteASolder = new Pharmacie2.views.Composants.CarteKpi();
            this.carteARecuperer = new Pharmacie2.views.Composants.CarteKpi();
            this.tlpTables = new System.Windows.Forms.TableLayoutPanel();
            this.tlpModes = new System.Windows.Forms.TableLayoutPanel();
            this.lblModes = new System.Windows.Forms.Label();
            this.dgvVentilation = new System.Windows.Forms.DataGridView();
            this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblTop = new System.Windows.Forms.Label();
            this.dgvTopProduits = new System.Windows.Forms.DataGridView();
            this.tlpCredits = new System.Windows.Forms.TableLayoutPanel();
            this.lblCredits = new System.Windows.Forms.Label();
            this.dgvCreditsClients = new System.Windows.Forms.DataGridView();
            this.tlpMutuelles = new System.Windows.Forms.TableLayoutPanel();
            this.lblMutuelles = new System.Windows.Forms.Label();
            this.dgvMutuelleImpayes = new System.Windows.Forms.DataGridView();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNbVentes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRecu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPourcentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTelephone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditNbVentes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRestant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutuelleNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutuelleNbVentes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalDu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentilation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProduits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditsClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutuelleImpayes)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpPeriode.SuspendLayout();
            this.tlpKpi.SuspendLayout();
            this.tlpTables.SuspendLayout();
            this.tlpModes.SuspendLayout();
            this.tlpTop.SuspendLayout();
            this.tlpCredits.SuspendLayout();
            this.tlpMutuelles.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 7;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.tlpRoot.Controls.Add(this.flpPeriode, 0, 1);
            this.tlpRoot.Controls.Add(this.lblAlertes, 0, 2);
            this.tlpRoot.Controls.Add(this.listeAlertes, 0, 3);
            this.tlpRoot.Controls.Add(this.tlpKpi, 0, 4);
            this.tlpRoot.Controls.Add(this.tlpTables, 0, 5);
            this.tlpRoot.Controls.Add(this.lblLastUpdate, 0, 6);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Statistiques";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpPeriode
            // 
            this.flpPeriode.Controls.Add(this.lblPeriode);
            this.flpPeriode.Controls.Add(this.cbPeriode);
            this.flpPeriode.Controls.Add(this.dtpDebut);
            this.flpPeriode.Controls.Add(this.dtpFin);
            this.flpPeriode.Controls.Add(this.btnActualiser);
            this.flpPeriode.Controls.Add(this.btnExportExcel);
            this.flpPeriode.Controls.Add(this.btnExportPDF);
            this.flpPeriode.Controls.Add(this.lblPeriodeAffichee);
            this.flpPeriode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPeriode.AutoSize = true;
            this.flpPeriode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPeriode.WrapContents = true;
            this.flpPeriode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flpPeriode.Name = "flpPeriode";
            // 
            // lblPeriode
            // 
            this.lblPeriode.Text = "Période";
            this.lblPeriode.AutoSize = true;
            this.lblPeriode.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblPeriode.Name = "lblPeriode";
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
            "Cette année",
            "Personnalisé"});
            this.cbPeriode.Name = "cbPeriode";
            this.cbPeriode.SelectedIndexChanged += new System.EventHandler(this.cbPeriode_SelectedIndexChanged);
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
            // btnExportExcel
            // 
            this.btnExportExcel.Text = "Exporter en Excel";
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Text = "Exporter en PDF";
            this.btnExportPDF.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // lblPeriodeAffichee
            // 
            this.lblPeriodeAffichee.Text = "";
            this.lblPeriodeAffichee.AutoSize = true;
            this.lblPeriodeAffichee.Tag = "note";
            this.lblPeriodeAffichee.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblPeriodeAffichee.Name = "lblPeriodeAffichee";
            // 
            // lblAlertes
            // 
            this.lblAlertes.Text = "À surveiller";
            this.lblAlertes.AutoSize = true;
            this.lblAlertes.Tag = "section";
            this.lblAlertes.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblAlertes.Name = "lblAlertes";
            // 
            // listeAlertes
            // 
            this.listeAlertes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listeAlertes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.listeAlertes.Name = "listeAlertes";
            // 
            // tlpKpi
            // 
            this.tlpKpi.ColumnCount = 5;
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0F));
            this.tlpKpi.RowCount = 1;
            this.tlpKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpKpi.Controls.Add(this.carteCA, 0, 0);
            this.tlpKpi.Controls.Add(this.carteBenefice, 1, 0);
            this.tlpKpi.Controls.Add(this.carteVentes, 2, 0);
            this.tlpKpi.Controls.Add(this.carteASolder, 3, 0);
            this.tlpKpi.Controls.Add(this.carteARecuperer, 4, 0);
            this.tlpKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpKpi.Name = "tlpKpi";
            // 
            // carteCA
            // 
            this.carteCA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteCA.Titre = "Chiffre d'affaires";
            this.carteCA.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteCA.Name = "carteCA";
            // 
            // carteBenefice
            // 
            this.carteBenefice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteBenefice.Titre = "Bénéfice (après dépenses)";
            this.carteBenefice.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteBenefice.Name = "carteBenefice";
            // 
            // carteVentes
            // 
            this.carteVentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteVentes.Titre = "Ventes";
            this.carteVentes.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteVentes.Name = "carteVentes";
            // 
            // carteASolder
            // 
            this.carteASolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteASolder.Titre = "Ventes à solder";
            this.carteASolder.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteASolder.Name = "carteASolder";
            // 
            // carteARecuperer
            // 
            this.carteARecuperer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteARecuperer.Titre = "Argent à récupérer";
            this.carteARecuperer.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.carteARecuperer.Name = "carteARecuperer";
            // 
            // tlpTables
            // 
            this.tlpTables.ColumnCount = 2;
            this.tlpTables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpTables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpTables.RowCount = 2;
            this.tlpTables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpTables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpTables.Controls.Add(this.tlpModes, 0, 0);
            this.tlpTables.Controls.Add(this.tlpTop, 1, 0);
            this.tlpTables.Controls.Add(this.tlpCredits, 0, 1);
            this.tlpTables.Controls.Add(this.tlpMutuelles, 1, 1);
            this.tlpTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTables.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpTables.Name = "tlpTables";
            // 
            // tlpModes
            // 
            this.tlpModes.ColumnCount = 1;
            this.tlpModes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpModes.RowCount = 2;
            this.tlpModes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpModes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpModes.Controls.Add(this.lblModes, 0, 0);
            this.tlpModes.Controls.Add(this.dgvVentilation, 0, 1);
            this.tlpModes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpModes.Margin = new System.Windows.Forms.Padding(0, 0, 8, 4);
            this.tlpModes.Name = "tlpModes";
            // 
            // lblModes
            // 
            this.lblModes.Text = "Argent reçu par moyen de paiement";
            this.lblModes.AutoSize = true;
            this.lblModes.Tag = "section";
            this.lblModes.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblModes.Name = "lblModes";
            // 
            // dgvVentilation
            // 
            this.dgvVentilation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMode,
            this.colNbVentes,
            this.colRecu,
            this.colPourcentage});
            this.dgvVentilation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentilation.AutoGenerateColumns = false;
            this.dgvVentilation.ReadOnly = true;
            this.dgvVentilation.Name = "dgvVentilation";
            // 
            // tlpTop
            // 
            this.tlpTop.ColumnCount = 1;
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpTop.RowCount = 2;
            this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpTop.Controls.Add(this.lblTop, 0, 0);
            this.tlpTop.Controls.Add(this.dgvTopProduits, 0, 1);
            this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTop.Margin = new System.Windows.Forms.Padding(8, 0, 0, 4);
            this.tlpTop.Name = "tlpTop";
            // 
            // lblTop
            // 
            this.lblTop.Text = "Produits les plus vendus";
            this.lblTop.AutoSize = true;
            this.lblTop.Tag = "section";
            this.lblTop.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblTop.Name = "lblTop";
            // 
            // dgvTopProduits
            // 
            this.dgvTopProduits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduit,
            this.colVendu,
            this.colCA});
            this.dgvTopProduits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopProduits.AutoGenerateColumns = false;
            this.dgvTopProduits.ReadOnly = true;
            this.dgvTopProduits.Name = "dgvTopProduits";
            // 
            // tlpCredits
            // 
            this.tlpCredits.ColumnCount = 1;
            this.tlpCredits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpCredits.RowCount = 2;
            this.tlpCredits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCredits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpCredits.Controls.Add(this.lblCredits, 0, 0);
            this.tlpCredits.Controls.Add(this.dgvCreditsClients, 0, 1);
            this.tlpCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCredits.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.tlpCredits.Name = "tlpCredits";
            // 
            // lblCredits
            // 
            this.lblCredits.Text = "Crédits clients";
            this.lblCredits.AutoSize = true;
            this.lblCredits.Tag = "section";
            this.lblCredits.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblCredits.Name = "lblCredits";
            // 
            // dgvCreditsClients
            // 
            this.dgvCreditsClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClient,
            this.colTelephone,
            this.colCreditNbVentes,
            this.colRestant});
            this.dgvCreditsClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCreditsClients.AutoGenerateColumns = false;
            this.dgvCreditsClients.ReadOnly = true;
            this.dgvCreditsClients.Name = "dgvCreditsClients";
            // 
            // tlpMutuelles
            // 
            this.tlpMutuelles.ColumnCount = 1;
            this.tlpMutuelles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMutuelles.RowCount = 2;
            this.tlpMutuelles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMutuelles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMutuelles.Controls.Add(this.lblMutuelles, 0, 0);
            this.tlpMutuelles.Controls.Add(this.dgvMutuelleImpayes, 0, 1);
            this.tlpMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMutuelles.Margin = new System.Windows.Forms.Padding(8, 4, 0, 0);
            this.tlpMutuelles.Name = "tlpMutuelles";
            // 
            // lblMutuelles
            // 
            this.lblMutuelles.Text = "Mutuelles qui doivent de l'argent";
            this.lblMutuelles.AutoSize = true;
            this.lblMutuelles.Tag = "section";
            this.lblMutuelles.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblMutuelles.Name = "lblMutuelles";
            // 
            // dgvMutuelleImpayes
            // 
            this.dgvMutuelleImpayes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMutuelleNom,
            this.colMutuelleNbVentes,
            this.colTotalDu});
            this.dgvMutuelleImpayes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMutuelleImpayes.AutoGenerateColumns = false;
            this.dgvMutuelleImpayes.ReadOnly = true;
            this.dgvMutuelleImpayes.Name = "dgvMutuelleImpayes";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Text = "";
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Tag = "note";
            this.lblLastUpdate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblLastUpdate.Name = "lblLastUpdate";
            // 
            // colMode
            // 
            this.colMode.HeaderText = "Moyen de paiement";
            this.colMode.Name = "Mode";
            this.colMode.DataPropertyName = "Mode";
            this.colMode.FillWeight = 46F;
            this.colMode.MinimumWidth = 150;
            this.colMode.ReadOnly = true;
            // 
            // colNbVentes
            // 
            this.colNbVentes.HeaderText = "Ventes";
            this.colNbVentes.Name = "NbVentes";
            this.colNbVentes.DataPropertyName = "NbVentes";
            this.colNbVentes.FillWeight = 14F;
            this.colNbVentes.MinimumWidth = 60;
            this.colNbVentes.ReadOnly = true;
            // 
            // colRecu
            // 
            this.colRecu.HeaderText = "Argent reçu";
            this.colRecu.Name = "Recu";
            this.colRecu.DataPropertyName = "Recu";
            this.colRecu.FillWeight = 26F;
            this.colRecu.MinimumWidth = 100;
            this.colRecu.ReadOnly = true;
            this.colRecu.Tag = "montant";
            // 
            // colPourcentage
            // 
            this.colPourcentage.HeaderText = "Part (%)";
            this.colPourcentage.Name = "Pourcentage";
            this.colPourcentage.DataPropertyName = "Pourcentage";
            this.colPourcentage.FillWeight = 14F;
            this.colPourcentage.MinimumWidth = 60;
            this.colPourcentage.ReadOnly = true;
            // 
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "Produit";
            this.colProduit.DataPropertyName = "Produit";
            this.colProduit.FillWeight = 40F;
            this.colProduit.MinimumWidth = 120;
            this.colProduit.ReadOnly = true;
            // 
            // colVendu
            // 
            this.colVendu.HeaderText = "Quantité vendue";
            this.colVendu.Name = "Vendu";
            this.colVendu.DataPropertyName = "Vendu";
            this.colVendu.FillWeight = 35F;
            this.colVendu.MinimumWidth = 120;
            this.colVendu.ReadOnly = true;
            // 
            // colCA
            // 
            this.colCA.HeaderText = "Ventes";
            this.colCA.Name = "CA";
            this.colCA.DataPropertyName = "CA";
            this.colCA.FillWeight = 25F;
            this.colCA.MinimumWidth = 90;
            this.colCA.ReadOnly = true;
            this.colCA.Tag = "montant";
            // 
            // colClient
            // 
            this.colClient.HeaderText = "Client";
            this.colClient.Name = "Client";
            this.colClient.DataPropertyName = "Client";
            this.colClient.FillWeight = 34F;
            this.colClient.MinimumWidth = 110;
            this.colClient.ReadOnly = true;
            // 
            // colTelephone
            // 
            this.colTelephone.HeaderText = "Téléphone";
            this.colTelephone.Name = "Telephone";
            this.colTelephone.DataPropertyName = "Telephone";
            this.colTelephone.FillWeight = 22F;
            this.colTelephone.MinimumWidth = 90;
            this.colTelephone.ReadOnly = true;
            // 
            // colCreditNbVentes
            // 
            this.colCreditNbVentes.HeaderText = "Ventes";
            this.colCreditNbVentes.Name = "NbVentes";
            this.colCreditNbVentes.DataPropertyName = "NbVentes";
            this.colCreditNbVentes.FillWeight = 12F;
            this.colCreditNbVentes.MinimumWidth = 60;
            this.colCreditNbVentes.ReadOnly = true;
            // 
            // colRestant
            // 
            this.colRestant.HeaderText = "Reste à payer";
            this.colRestant.Name = "Restant";
            this.colRestant.DataPropertyName = "Restant";
            this.colRestant.FillWeight = 26F;
            this.colRestant.MinimumWidth = 100;
            this.colRestant.ReadOnly = true;
            this.colRestant.Tag = "montant";
            // 
            // colMutuelleNom
            // 
            this.colMutuelleNom.HeaderText = "Mutuelle";
            this.colMutuelleNom.Name = "Mutuelle";
            this.colMutuelleNom.DataPropertyName = "Mutuelle";
            this.colMutuelleNom.FillWeight = 45F;
            this.colMutuelleNom.MinimumWidth = 120;
            this.colMutuelleNom.ReadOnly = true;
            // 
            // colMutuelleNbVentes
            // 
            this.colMutuelleNbVentes.HeaderText = "Ventes";
            this.colMutuelleNbVentes.Name = "NbVentes";
            this.colMutuelleNbVentes.DataPropertyName = "NbVentes";
            this.colMutuelleNbVentes.FillWeight = 15F;
            this.colMutuelleNbVentes.MinimumWidth = 60;
            this.colMutuelleNbVentes.ReadOnly = true;
            // 
            // colTotalDu
            // 
            this.colTotalDu.HeaderText = "Montant dû";
            this.colTotalDu.Name = "TotalDu";
            this.colTotalDu.DataPropertyName = "TotalDu";
            this.colTotalDu.FillWeight = 40F;
            this.colTotalDu.MinimumWidth = 100;
            this.colTotalDu.ReadOnly = true;
            this.colTotalDu.Tag = "montant";
            // 
            // Uc_Statistique
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Statistique";
            this.ResumeLayout(false);
            this.tlpMutuelles.ResumeLayout(false);
            this.tlpMutuelles.PerformLayout();
            this.tlpCredits.ResumeLayout(false);
            this.tlpCredits.PerformLayout();
            this.tlpTop.ResumeLayout(false);
            this.tlpTop.PerformLayout();
            this.tlpModes.ResumeLayout(false);
            this.tlpModes.PerformLayout();
            this.tlpTables.ResumeLayout(false);
            this.tlpTables.PerformLayout();
            this.tlpKpi.ResumeLayout(false);
            this.tlpKpi.PerformLayout();
            this.flpPeriode.ResumeLayout(false);
            this.flpPeriode.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutuelleImpayes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditsClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProduits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentilation)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpPeriode;
        private System.Windows.Forms.Label lblPeriode;
        private System.Windows.Forms.ComboBox cbPeriode;
        private System.Windows.Forms.DateTimePicker dtpDebut;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Label lblPeriodeAffichee;
        private System.Windows.Forms.Label lblAlertes;
        private Pharmacie2.views.Composants.ListeActions listeAlertes;
        private System.Windows.Forms.TableLayoutPanel tlpKpi;
        private Pharmacie2.views.Composants.CarteKpi carteCA;
        private Pharmacie2.views.Composants.CarteKpi carteBenefice;
        private Pharmacie2.views.Composants.CarteKpi carteVentes;
        private Pharmacie2.views.Composants.CarteKpi carteASolder;
        private Pharmacie2.views.Composants.CarteKpi carteARecuperer;
        private System.Windows.Forms.TableLayoutPanel tlpTables;
        private System.Windows.Forms.TableLayoutPanel tlpModes;
        private System.Windows.Forms.Label lblModes;
        private System.Windows.Forms.DataGridView dgvVentilation;
        private System.Windows.Forms.TableLayoutPanel tlpTop;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.DataGridView dgvTopProduits;
        private System.Windows.Forms.TableLayoutPanel tlpCredits;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.DataGridView dgvCreditsClients;
        private System.Windows.Forms.TableLayoutPanel tlpMutuelles;
        private System.Windows.Forms.Label lblMutuelles;
        private System.Windows.Forms.DataGridView dgvMutuelleImpayes;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNbVentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPourcentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditNbVentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRestant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutuelleNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutuelleNbVentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalDu;
    }
}
