namespace Pharmacie2.views.UserControls
{
    partial class Uc_Depenses
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
            this.tabMensuel = new System.Windows.Forms.TabPage();
            this.tlpMensuel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltreMensuel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMoisLabel = new System.Windows.Forms.Label();
            this.cbMois = new System.Windows.Forms.ComboBox();
            this.lblAnneeLabel = new System.Windows.Forms.Label();
            this.cbAnnee = new System.Windows.Forms.ComboBox();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.btnNouvelleDepense = new System.Windows.Forms.Button();
            this.lblPeriodeMensuel = new System.Windows.Forms.Label();
            this.tlpCorps = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGauche = new System.Windows.Forms.TableLayoutPanel();
            this.lblRevTitre = new System.Windows.Forms.Label();
            this.lblEspecesT = new System.Windows.Forms.Label();
            this.lblEspeces = new System.Windows.Forms.Label();
            this.lblCBT = new System.Windows.Forms.Label();
            this.lblCB = new System.Windows.Forms.Label();
            this.lblAvancesT = new System.Windows.Forms.Label();
            this.lblAvancesCredit = new System.Windows.Forms.Label();
            this.lblMutuellePT = new System.Windows.Forms.Label();
            this.lblMutuelleP = new System.Windows.Forms.Label();
            this.lblMutuelleET = new System.Windows.Forms.Label();
            this.lblMutuelleE = new System.Windows.Forms.Label();
            this.lblTotalRevenusT = new System.Windows.Forms.Label();
            this.lblTotalRevenus = new System.Windows.Forms.Label();
            this.pnlDroite = new System.Windows.Forms.TableLayoutPanel();
            this.lblChargeTitre = new System.Windows.Forms.Label();
            this.lblCOGST = new System.Windows.Forms.Label();
            this.lblCOGS = new System.Windows.Forms.Label();
            this.lblSalairesT = new System.Windows.Forms.Label();
            this.lblSalaires = new System.Windows.Forms.Label();
            this.lblLoyerT = new System.Windows.Forms.Label();
            this.lblLoyer = new System.Windows.Forms.Label();
            this.lblFacturesT = new System.Windows.Forms.Label();
            this.lblFactures = new System.Windows.Forms.Label();
            this.lblFournituresT = new System.Windows.Forms.Label();
            this.lblFournitures = new System.Windows.Forms.Label();
            this.lblAutresT = new System.Windows.Forms.Label();
            this.lblAutres = new System.Windows.Forms.Label();
            this.lblTotalChargesT = new System.Windows.Forms.Label();
            this.lblTotalCharges = new System.Windows.Forms.Label();
            this.pnlResultat = new System.Windows.Forms.TableLayoutPanel();
            this.lblBeneficeLabel = new System.Windows.Forms.Label();
            this.lblBeneficeNet = new System.Windows.Forms.Label();
            this.lblMarge = new System.Windows.Forms.Label();
            this.tabAnnuel = new System.Windows.Forms.TabPage();
            this.tlpAnnuel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltreAnnuel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAnneeAnnuelLabel = new System.Windows.Forms.Label();
            this.cbAnneeAnnuel = new System.Windows.Forms.ComboBox();
            this.dgvAnnuel = new System.Windows.Forms.DataGridView();
            this.tabHistorique = new System.Windows.Forms.TabPage();
            this.tlpHisto = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltreHisto = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFiltreHistoLabel = new System.Windows.Forms.Label();
            this.cbFiltreHistoCat = new System.Windows.Forms.ComboBox();
            this.btnSupprimerDep = new System.Windows.Forms.Button();
            this.dgvHistorique = new System.Windows.Forms.DataGridView();
            this.lblTotalHistorique = new System.Windows.Forms.Label();
            this.colMois = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCOGS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepenses = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBenefice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoMontant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnnuel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorique)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabMensuel.SuspendLayout();
            this.tlpMensuel.SuspendLayout();
            this.pnlFiltreMensuel.SuspendLayout();
            this.tlpCorps.SuspendLayout();
            this.pnlGauche.SuspendLayout();
            this.pnlDroite.SuspendLayout();
            this.pnlResultat.SuspendLayout();
            this.tabAnnuel.SuspendLayout();
            this.tlpAnnuel.SuspendLayout();
            this.pnlFiltreAnnuel.SuspendLayout();
            this.tabHistorique.SuspendLayout();
            this.tlpHisto.SuspendLayout();
            this.pnlFiltreHisto.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.tabMain, 0, 1);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Dépenses et résultat";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabMensuel);
            this.tabMain.Controls.Add(this.tabAnnuel);
            this.tabMain.Controls.Add(this.tabHistorique);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tabMain.Name = "tabMain";
            // 
            // tabMensuel
            // 
            this.tabMensuel.Controls.Add(this.tlpMensuel);
            this.tabMensuel.Text = "Résultat du mois";
            this.tabMensuel.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tabMensuel.UseVisualStyleBackColor = true;
            this.tabMensuel.Name = "tabMensuel";
            // 
            // tlpMensuel
            // 
            this.tlpMensuel.ColumnCount = 1;
            this.tlpMensuel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMensuel.RowCount = 2;
            this.tlpMensuel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMensuel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMensuel.Controls.Add(this.pnlFiltreMensuel, 0, 0);
            this.tlpMensuel.Controls.Add(this.tlpCorps, 0, 1);
            this.tlpMensuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMensuel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpMensuel.Name = "tlpMensuel";
            // 
            // pnlFiltreMensuel
            // 
            this.pnlFiltreMensuel.Controls.Add(this.lblMoisLabel);
            this.pnlFiltreMensuel.Controls.Add(this.cbMois);
            this.pnlFiltreMensuel.Controls.Add(this.lblAnneeLabel);
            this.pnlFiltreMensuel.Controls.Add(this.cbAnnee);
            this.pnlFiltreMensuel.Controls.Add(this.btnActualiser);
            this.pnlFiltreMensuel.Controls.Add(this.btnNouvelleDepense);
            this.pnlFiltreMensuel.Controls.Add(this.lblPeriodeMensuel);
            this.pnlFiltreMensuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltreMensuel.AutoSize = true;
            this.pnlFiltreMensuel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFiltreMensuel.WrapContents = true;
            this.pnlFiltreMensuel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlFiltreMensuel.Name = "pnlFiltreMensuel";
            // 
            // lblMoisLabel
            // 
            this.lblMoisLabel.Text = "Mois";
            this.lblMoisLabel.AutoSize = true;
            this.lblMoisLabel.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblMoisLabel.Name = "lblMoisLabel";
            // 
            // cbMois
            // 
            this.cbMois.Width = 140;
            this.cbMois.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMois.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbMois.Name = "cbMois";
            // 
            // lblAnneeLabel
            // 
            this.lblAnneeLabel.Text = "Année";
            this.lblAnneeLabel.AutoSize = true;
            this.lblAnneeLabel.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblAnneeLabel.Name = "lblAnneeLabel";
            // 
            // cbAnnee
            // 
            this.cbAnnee.Width = 90;
            this.cbAnnee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAnnee.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbAnnee.Name = "cbAnnee";
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiser.Name = "btnActualiser";
            // 
            // btnNouvelleDepense
            // 
            this.btnNouvelleDepense.Text = "Nouvelle dépense";
            this.btnNouvelleDepense.Tag = "primaire";
            this.btnNouvelleDepense.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouvelleDepense.Name = "btnNouvelleDepense";
            // 
            // lblPeriodeMensuel
            // 
            this.lblPeriodeMensuel.Text = "";
            this.lblPeriodeMensuel.AutoSize = true;
            this.lblPeriodeMensuel.Tag = "note";
            this.lblPeriodeMensuel.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblPeriodeMensuel.Name = "lblPeriodeMensuel";
            // 
            // tlpCorps
            // 
            this.tlpCorps.ColumnCount = 3;
            this.tlpCorps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.0F));
            this.tlpCorps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.0F));
            this.tlpCorps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.0F));
            this.tlpCorps.RowCount = 1;
            this.tlpCorps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCorps.Controls.Add(this.pnlGauche, 0, 0);
            this.tlpCorps.Controls.Add(this.pnlDroite, 1, 0);
            this.tlpCorps.Controls.Add(this.pnlResultat, 2, 0);
            this.tlpCorps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCorps.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpCorps.Name = "tlpCorps";
            // 
            // pnlGauche
            // 
            this.pnlGauche.ColumnCount = 2;
            this.pnlGauche.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlGauche.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowCount = 7;
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlGauche.Controls.Add(this.lblRevTitre, 0, 0);
            this.pnlGauche.SetColumnSpan(this.lblRevTitre, 2);
            this.pnlGauche.Controls.Add(this.lblEspecesT, 0, 1);
            this.pnlGauche.Controls.Add(this.lblEspeces, 1, 1);
            this.pnlGauche.Controls.Add(this.lblCBT, 0, 2);
            this.pnlGauche.Controls.Add(this.lblCB, 1, 2);
            this.pnlGauche.Controls.Add(this.lblAvancesT, 0, 3);
            this.pnlGauche.Controls.Add(this.lblAvancesCredit, 1, 3);
            this.pnlGauche.Controls.Add(this.lblMutuellePT, 0, 4);
            this.pnlGauche.Controls.Add(this.lblMutuelleP, 1, 4);
            this.pnlGauche.Controls.Add(this.lblMutuelleET, 0, 5);
            this.pnlGauche.Controls.Add(this.lblMutuelleE, 1, 5);
            this.pnlGauche.Controls.Add(this.lblTotalRevenusT, 0, 6);
            this.pnlGauche.Controls.Add(this.lblTotalRevenus, 1, 6);
            this.pnlGauche.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGauche.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlGauche.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlGauche.Tag = "carte";
            this.pnlGauche.Name = "pnlGauche";
            // 
            // lblRevTitre
            // 
            this.lblRevTitre.Text = "Argent reçu";
            this.lblRevTitre.AutoSize = true;
            this.lblRevTitre.Tag = "section";
            this.lblRevTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblRevTitre.Name = "lblRevTitre";
            // 
            // lblEspecesT
            // 
            this.lblEspecesT.Text = "Espèces (comptant)";
            this.lblEspecesT.AutoSize = true;
            this.lblEspecesT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEspecesT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblEspecesT.Name = "lblEspecesT";
            // 
            // lblEspeces
            // 
            this.lblEspeces.Text = "";
            this.lblEspeces.AutoSize = true;
            this.lblEspeces.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEspeces.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblEspeces.Name = "lblEspeces";
            // 
            // lblCBT
            // 
            this.lblCBT.Text = "Carte, chèque et mobile";
            this.lblCBT.AutoSize = true;
            this.lblCBT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCBT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblCBT.Name = "lblCBT";
            // 
            // lblCB
            // 
            this.lblCB.Text = "";
            this.lblCB.AutoSize = true;
            this.lblCB.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCB.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblCB.Name = "lblCB";
            // 
            // lblAvancesT
            // 
            this.lblAvancesT.Text = "Versements sur crédits";
            this.lblAvancesT.AutoSize = true;
            this.lblAvancesT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAvancesT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblAvancesT.Name = "lblAvancesT";
            // 
            // lblAvancesCredit
            // 
            this.lblAvancesCredit.Text = "";
            this.lblAvancesCredit.AutoSize = true;
            this.lblAvancesCredit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAvancesCredit.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblAvancesCredit.Name = "lblAvancesCredit";
            // 
            // lblMutuellePT
            // 
            this.lblMutuellePT.Text = "Part des patients (mutuelle)";
            this.lblMutuellePT.AutoSize = true;
            this.lblMutuellePT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMutuellePT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblMutuellePT.Name = "lblMutuellePT";
            // 
            // lblMutuelleP
            // 
            this.lblMutuelleP.Text = "";
            this.lblMutuelleP.AutoSize = true;
            this.lblMutuelleP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMutuelleP.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblMutuelleP.Name = "lblMutuelleP";
            // 
            // lblMutuelleET
            // 
            this.lblMutuelleET.Text = "Part réglée par les mutuelles";
            this.lblMutuelleET.AutoSize = true;
            this.lblMutuelleET.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMutuelleET.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblMutuelleET.Name = "lblMutuelleET";
            // 
            // lblMutuelleE
            // 
            this.lblMutuelleE.Text = "";
            this.lblMutuelleE.AutoSize = true;
            this.lblMutuelleE.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMutuelleE.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblMutuelleE.Name = "lblMutuelleE";
            // 
            // lblTotalRevenusT
            // 
            this.lblTotalRevenusT.Text = "Total reçu";
            this.lblTotalRevenusT.AutoSize = true;
            this.lblTotalRevenusT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalRevenusT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblTotalRevenusT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRevenusT.Name = "lblTotalRevenusT";
            // 
            // lblTotalRevenus
            // 
            this.lblTotalRevenus.Text = "";
            this.lblTotalRevenus.AutoSize = true;
            this.lblTotalRevenus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalRevenus.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblTotalRevenus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRevenus.Name = "lblTotalRevenus";
            // 
            // pnlDroite
            // 
            this.pnlDroite.ColumnCount = 2;
            this.pnlDroite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlDroite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowCount = 8;
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlDroite.Controls.Add(this.lblChargeTitre, 0, 0);
            this.pnlDroite.SetColumnSpan(this.lblChargeTitre, 2);
            this.pnlDroite.Controls.Add(this.lblCOGST, 0, 1);
            this.pnlDroite.Controls.Add(this.lblCOGS, 1, 1);
            this.pnlDroite.Controls.Add(this.lblSalairesT, 0, 2);
            this.pnlDroite.Controls.Add(this.lblSalaires, 1, 2);
            this.pnlDroite.Controls.Add(this.lblLoyerT, 0, 3);
            this.pnlDroite.Controls.Add(this.lblLoyer, 1, 3);
            this.pnlDroite.Controls.Add(this.lblFacturesT, 0, 4);
            this.pnlDroite.Controls.Add(this.lblFactures, 1, 4);
            this.pnlDroite.Controls.Add(this.lblFournituresT, 0, 5);
            this.pnlDroite.Controls.Add(this.lblFournitures, 1, 5);
            this.pnlDroite.Controls.Add(this.lblAutresT, 0, 6);
            this.pnlDroite.Controls.Add(this.lblAutres, 1, 6);
            this.pnlDroite.Controls.Add(this.lblTotalChargesT, 0, 7);
            this.pnlDroite.Controls.Add(this.lblTotalCharges, 1, 7);
            this.pnlDroite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDroite.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlDroite.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlDroite.Tag = "carte";
            this.pnlDroite.Name = "pnlDroite";
            // 
            // lblChargeTitre
            // 
            this.lblChargeTitre.Text = "Dépenses";
            this.lblChargeTitre.AutoSize = true;
            this.lblChargeTitre.Tag = "section";
            this.lblChargeTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblChargeTitre.Name = "lblChargeTitre";
            // 
            // lblCOGST
            // 
            this.lblCOGST.Text = "Achat des produits vendus";
            this.lblCOGST.AutoSize = true;
            this.lblCOGST.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCOGST.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblCOGST.Name = "lblCOGST";
            // 
            // lblCOGS
            // 
            this.lblCOGS.Text = "";
            this.lblCOGS.AutoSize = true;
            this.lblCOGS.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCOGS.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblCOGS.Name = "lblCOGS";
            // 
            // lblSalairesT
            // 
            this.lblSalairesT.Text = "Salaires";
            this.lblSalairesT.AutoSize = true;
            this.lblSalairesT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSalairesT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblSalairesT.Name = "lblSalairesT";
            // 
            // lblSalaires
            // 
            this.lblSalaires.Text = "";
            this.lblSalaires.AutoSize = true;
            this.lblSalaires.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSalaires.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblSalaires.Name = "lblSalaires";
            // 
            // lblLoyerT
            // 
            this.lblLoyerT.Text = "Loyer";
            this.lblLoyerT.AutoSize = true;
            this.lblLoyerT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLoyerT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblLoyerT.Name = "lblLoyerT";
            // 
            // lblLoyer
            // 
            this.lblLoyer.Text = "";
            this.lblLoyer.AutoSize = true;
            this.lblLoyer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLoyer.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblLoyer.Name = "lblLoyer";
            // 
            // lblFacturesT
            // 
            this.lblFacturesT.Text = "Factures";
            this.lblFacturesT.AutoSize = true;
            this.lblFacturesT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFacturesT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblFacturesT.Name = "lblFacturesT";
            // 
            // lblFactures
            // 
            this.lblFactures.Text = "";
            this.lblFactures.AutoSize = true;
            this.lblFactures.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFactures.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblFactures.Name = "lblFactures";
            // 
            // lblFournituresT
            // 
            this.lblFournituresT.Text = "Fournitures";
            this.lblFournituresT.AutoSize = true;
            this.lblFournituresT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFournituresT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblFournituresT.Name = "lblFournituresT";
            // 
            // lblFournitures
            // 
            this.lblFournitures.Text = "";
            this.lblFournitures.AutoSize = true;
            this.lblFournitures.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFournitures.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblFournitures.Name = "lblFournitures";
            // 
            // lblAutresT
            // 
            this.lblAutresT.Text = "Autres";
            this.lblAutresT.AutoSize = true;
            this.lblAutresT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAutresT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblAutresT.Name = "lblAutresT";
            // 
            // lblAutres
            // 
            this.lblAutres.Text = "";
            this.lblAutres.AutoSize = true;
            this.lblAutres.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAutres.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblAutres.Name = "lblAutres";
            // 
            // lblTotalChargesT
            // 
            this.lblTotalChargesT.Text = "Total des dépenses";
            this.lblTotalChargesT.AutoSize = true;
            this.lblTotalChargesT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalChargesT.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.lblTotalChargesT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalChargesT.Name = "lblTotalChargesT";
            // 
            // lblTotalCharges
            // 
            this.lblTotalCharges.Text = "";
            this.lblTotalCharges.AutoSize = true;
            this.lblTotalCharges.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalCharges.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblTotalCharges.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCharges.Name = "lblTotalCharges";
            // 
            // pnlResultat
            // 
            this.pnlResultat.ColumnCount = 1;
            this.pnlResultat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlResultat.RowCount = 3;
            this.pnlResultat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlResultat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlResultat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlResultat.Controls.Add(this.lblBeneficeLabel, 0, 0);
            this.pnlResultat.Controls.Add(this.lblBeneficeNet, 0, 1);
            this.pnlResultat.Controls.Add(this.lblMarge, 0, 2);
            this.pnlResultat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultat.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlResultat.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.pnlResultat.Tag = "carte";
            this.pnlResultat.Name = "pnlResultat";
            // 
            // lblBeneficeLabel
            // 
            this.lblBeneficeLabel.Text = "Bénéfice du mois";
            this.lblBeneficeLabel.AutoSize = true;
            this.lblBeneficeLabel.Tag = "section";
            this.lblBeneficeLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblBeneficeLabel.Name = "lblBeneficeLabel";
            // 
            // lblBeneficeNet
            // 
            this.lblBeneficeNet.Text = "";
            this.lblBeneficeNet.AutoSize = true;
            this.lblBeneficeNet.Tag = "kpi";
            this.lblBeneficeNet.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblBeneficeNet.Name = "lblBeneficeNet";
            // 
            // lblMarge
            // 
            this.lblMarge.Text = "";
            this.lblMarge.AutoSize = true;
            this.lblMarge.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblMarge.Name = "lblMarge";
            // 
            // tabAnnuel
            // 
            this.tabAnnuel.Controls.Add(this.tlpAnnuel);
            this.tabAnnuel.Text = "Résultat de l'année";
            this.tabAnnuel.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tabAnnuel.UseVisualStyleBackColor = true;
            this.tabAnnuel.Name = "tabAnnuel";
            // 
            // tlpAnnuel
            // 
            this.tlpAnnuel.ColumnCount = 1;
            this.tlpAnnuel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpAnnuel.RowCount = 2;
            this.tlpAnnuel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpAnnuel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpAnnuel.Controls.Add(this.pnlFiltreAnnuel, 0, 0);
            this.tlpAnnuel.Controls.Add(this.dgvAnnuel, 0, 1);
            this.tlpAnnuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAnnuel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpAnnuel.Name = "tlpAnnuel";
            // 
            // pnlFiltreAnnuel
            // 
            this.pnlFiltreAnnuel.Controls.Add(this.lblAnneeAnnuelLabel);
            this.pnlFiltreAnnuel.Controls.Add(this.cbAnneeAnnuel);
            this.pnlFiltreAnnuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltreAnnuel.AutoSize = true;
            this.pnlFiltreAnnuel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFiltreAnnuel.WrapContents = true;
            this.pnlFiltreAnnuel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlFiltreAnnuel.Name = "pnlFiltreAnnuel";
            // 
            // lblAnneeAnnuelLabel
            // 
            this.lblAnneeAnnuelLabel.Text = "Année";
            this.lblAnneeAnnuelLabel.AutoSize = true;
            this.lblAnneeAnnuelLabel.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblAnneeAnnuelLabel.Name = "lblAnneeAnnuelLabel";
            // 
            // cbAnneeAnnuel
            // 
            this.cbAnneeAnnuel.Width = 90;
            this.cbAnneeAnnuel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAnneeAnnuel.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbAnneeAnnuel.Name = "cbAnneeAnnuel";
            // 
            // dgvAnnuel
            // 
            this.dgvAnnuel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMois,
            this.colCA,
            this.colCOGS,
            this.colDepenses,
            this.colBenefice,
            this.colMarge});
            this.dgvAnnuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAnnuel.AutoGenerateColumns = false;
            this.dgvAnnuel.ReadOnly = true;
            this.dgvAnnuel.Name = "dgvAnnuel";
            this.dgvAnnuel.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAnnuel_CellFormatting);
            // 
            // tabHistorique
            // 
            this.tabHistorique.Controls.Add(this.tlpHisto);
            this.tabHistorique.Text = "Dépenses saisies";
            this.tabHistorique.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tabHistorique.UseVisualStyleBackColor = true;
            this.tabHistorique.Name = "tabHistorique";
            // 
            // tlpHisto
            // 
            this.tlpHisto.ColumnCount = 1;
            this.tlpHisto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpHisto.RowCount = 3;
            this.tlpHisto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpHisto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpHisto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpHisto.Controls.Add(this.pnlFiltreHisto, 0, 0);
            this.tlpHisto.Controls.Add(this.dgvHistorique, 0, 1);
            this.tlpHisto.Controls.Add(this.lblTotalHistorique, 0, 2);
            this.tlpHisto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHisto.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpHisto.Name = "tlpHisto";
            // 
            // pnlFiltreHisto
            // 
            this.pnlFiltreHisto.Controls.Add(this.lblFiltreHistoLabel);
            this.pnlFiltreHisto.Controls.Add(this.cbFiltreHistoCat);
            this.pnlFiltreHisto.Controls.Add(this.btnSupprimerDep);
            this.pnlFiltreHisto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltreHisto.AutoSize = true;
            this.pnlFiltreHisto.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFiltreHisto.WrapContents = true;
            this.pnlFiltreHisto.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlFiltreHisto.Name = "pnlFiltreHisto";
            // 
            // lblFiltreHistoLabel
            // 
            this.lblFiltreHistoLabel.Text = "Catégorie";
            this.lblFiltreHistoLabel.AutoSize = true;
            this.lblFiltreHistoLabel.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreHistoLabel.Name = "lblFiltreHistoLabel";
            // 
            // cbFiltreHistoCat
            // 
            this.cbFiltreHistoCat.Width = 160;
            this.cbFiltreHistoCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreHistoCat.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cbFiltreHistoCat.Name = "cbFiltreHistoCat";
            // 
            // btnSupprimerDep
            // 
            this.btnSupprimerDep.Text = "Supprimer la dépense";
            this.btnSupprimerDep.Tag = "danger";
            this.btnSupprimerDep.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnSupprimerDep.Name = "btnSupprimerDep";
            // 
            // dgvHistorique
            // 
            this.dgvHistorique.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHistoId,
            this.colHistoDate,
            this.colHistoCat,
            this.colHistoDesc,
            this.colHistoMontant,
            this.colHistoUser});
            this.dgvHistorique.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorique.AutoGenerateColumns = false;
            this.dgvHistorique.ReadOnly = true;
            this.dgvHistorique.Name = "dgvHistorique";
            // 
            // lblTotalHistorique
            // 
            this.lblTotalHistorique.Text = "";
            this.lblTotalHistorique.AutoSize = true;
            this.lblTotalHistorique.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHistorique.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTotalHistorique.Name = "lblTotalHistorique";
            // 
            // colMois
            // 
            this.colMois.HeaderText = "Mois";
            this.colMois.Name = "Mois";
            this.colMois.DataPropertyName = "Mois";
            this.colMois.FillWeight = 14F;
            this.colMois.MinimumWidth = 70;
            this.colMois.ReadOnly = true;
            // 
            // colCA
            // 
            this.colCA.HeaderText = "Ventes";
            this.colCA.Name = "CA";
            this.colCA.DataPropertyName = "CA";
            this.colCA.FillWeight = 20F;
            this.colCA.MinimumWidth = 100;
            this.colCA.ReadOnly = true;
            this.colCA.Tag = "montant";
            // 
            // colCOGS
            // 
            this.colCOGS.HeaderText = "Achat des produits vendus";
            this.colCOGS.Name = "Cogs";
            this.colCOGS.DataPropertyName = "Cogs";
            this.colCOGS.FillWeight = 22F;
            this.colCOGS.MinimumWidth = 120;
            this.colCOGS.ReadOnly = true;
            this.colCOGS.Tag = "montant";
            // 
            // colDepenses
            // 
            this.colDepenses.HeaderText = "Dépenses";
            this.colDepenses.Name = "Depenses";
            this.colDepenses.DataPropertyName = "Depenses";
            this.colDepenses.FillWeight = 18F;
            this.colDepenses.MinimumWidth = 100;
            this.colDepenses.ReadOnly = true;
            this.colDepenses.Tag = "montant";
            // 
            // colBenefice
            // 
            this.colBenefice.HeaderText = "Bénéfice";
            this.colBenefice.Name = "Benefice";
            this.colBenefice.DataPropertyName = "Benefice";
            this.colBenefice.FillWeight = 18F;
            this.colBenefice.MinimumWidth = 100;
            this.colBenefice.ReadOnly = true;
            this.colBenefice.Tag = "montant";
            // 
            // colMarge
            // 
            this.colMarge.HeaderText = "Marge";
            this.colMarge.Name = "Marge";
            this.colMarge.DataPropertyName = "Marge";
            this.colMarge.FillWeight = 8F;
            this.colMarge.MinimumWidth = 70;
            this.colMarge.ReadOnly = true;
            // 
            // colHistoId
            // 
            this.colHistoId.HeaderText = "Id";
            this.colHistoId.Name = "Id";
            this.colHistoId.DataPropertyName = "Id";
            this.colHistoId.FillWeight = 100F;
            this.colHistoId.MinimumWidth = 60;
            this.colHistoId.ReadOnly = true;
            this.colHistoId.Visible = false;
            // 
            // colHistoDate
            // 
            this.colHistoDate.HeaderText = "Date";
            this.colHistoDate.Name = "Date";
            this.colHistoDate.DataPropertyName = "Date";
            this.colHistoDate.FillWeight = 14F;
            this.colHistoDate.MinimumWidth = 90;
            this.colHistoDate.ReadOnly = true;
            // 
            // colHistoCat
            // 
            this.colHistoCat.HeaderText = "Catégorie";
            this.colHistoCat.Name = "Categorie";
            this.colHistoCat.DataPropertyName = "Categorie";
            this.colHistoCat.FillWeight = 16F;
            this.colHistoCat.MinimumWidth = 100;
            this.colHistoCat.ReadOnly = true;
            // 
            // colHistoDesc
            // 
            this.colHistoDesc.HeaderText = "Description";
            this.colHistoDesc.Name = "Description";
            this.colHistoDesc.DataPropertyName = "Description";
            this.colHistoDesc.FillWeight = 36F;
            this.colHistoDesc.MinimumWidth = 140;
            this.colHistoDesc.ReadOnly = true;
            // 
            // colHistoMontant
            // 
            this.colHistoMontant.HeaderText = "Montant";
            this.colHistoMontant.Name = "Montant";
            this.colHistoMontant.DataPropertyName = "Montant";
            this.colHistoMontant.FillWeight = 18F;
            this.colHistoMontant.MinimumWidth = 100;
            this.colHistoMontant.ReadOnly = true;
            this.colHistoMontant.Tag = "montant";
            // 
            // colHistoUser
            // 
            this.colHistoUser.HeaderText = "Saisi par";
            this.colHistoUser.Name = "Utilisateur";
            this.colHistoUser.DataPropertyName = "Utilisateur";
            this.colHistoUser.FillWeight = 16F;
            this.colHistoUser.MinimumWidth = 100;
            this.colHistoUser.ReadOnly = true;
            // 
            // Uc_Depenses
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Depenses";
            this.ResumeLayout(false);
            this.pnlFiltreHisto.ResumeLayout(false);
            this.pnlFiltreHisto.PerformLayout();
            this.tlpHisto.ResumeLayout(false);
            this.tlpHisto.PerformLayout();
            this.tabHistorique.ResumeLayout(false);
            this.pnlFiltreAnnuel.ResumeLayout(false);
            this.pnlFiltreAnnuel.PerformLayout();
            this.tlpAnnuel.ResumeLayout(false);
            this.tlpAnnuel.PerformLayout();
            this.tabAnnuel.ResumeLayout(false);
            this.pnlResultat.ResumeLayout(false);
            this.pnlResultat.PerformLayout();
            this.pnlDroite.ResumeLayout(false);
            this.pnlDroite.PerformLayout();
            this.pnlGauche.ResumeLayout(false);
            this.pnlGauche.PerformLayout();
            this.tlpCorps.ResumeLayout(false);
            this.tlpCorps.PerformLayout();
            this.pnlFiltreMensuel.ResumeLayout(false);
            this.pnlFiltreMensuel.PerformLayout();
            this.tlpMensuel.ResumeLayout(false);
            this.tlpMensuel.PerformLayout();
            this.tabMensuel.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorique)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnnuel)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabMensuel;
        private System.Windows.Forms.TableLayoutPanel tlpMensuel;
        private System.Windows.Forms.FlowLayoutPanel pnlFiltreMensuel;
        private System.Windows.Forms.Label lblMoisLabel;
        private System.Windows.Forms.ComboBox cbMois;
        private System.Windows.Forms.Label lblAnneeLabel;
        private System.Windows.Forms.ComboBox cbAnnee;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Button btnNouvelleDepense;
        private System.Windows.Forms.Label lblPeriodeMensuel;
        private System.Windows.Forms.TableLayoutPanel tlpCorps;
        private System.Windows.Forms.TableLayoutPanel pnlGauche;
        private System.Windows.Forms.Label lblRevTitre;
        private System.Windows.Forms.Label lblEspecesT;
        private System.Windows.Forms.Label lblEspeces;
        private System.Windows.Forms.Label lblCBT;
        private System.Windows.Forms.Label lblCB;
        private System.Windows.Forms.Label lblAvancesT;
        private System.Windows.Forms.Label lblAvancesCredit;
        private System.Windows.Forms.Label lblMutuellePT;
        private System.Windows.Forms.Label lblMutuelleP;
        private System.Windows.Forms.Label lblMutuelleET;
        private System.Windows.Forms.Label lblMutuelleE;
        private System.Windows.Forms.Label lblTotalRevenusT;
        private System.Windows.Forms.Label lblTotalRevenus;
        private System.Windows.Forms.TableLayoutPanel pnlDroite;
        private System.Windows.Forms.Label lblChargeTitre;
        private System.Windows.Forms.Label lblCOGST;
        private System.Windows.Forms.Label lblCOGS;
        private System.Windows.Forms.Label lblSalairesT;
        private System.Windows.Forms.Label lblSalaires;
        private System.Windows.Forms.Label lblLoyerT;
        private System.Windows.Forms.Label lblLoyer;
        private System.Windows.Forms.Label lblFacturesT;
        private System.Windows.Forms.Label lblFactures;
        private System.Windows.Forms.Label lblFournituresT;
        private System.Windows.Forms.Label lblFournitures;
        private System.Windows.Forms.Label lblAutresT;
        private System.Windows.Forms.Label lblAutres;
        private System.Windows.Forms.Label lblTotalChargesT;
        private System.Windows.Forms.Label lblTotalCharges;
        private System.Windows.Forms.TableLayoutPanel pnlResultat;
        private System.Windows.Forms.Label lblBeneficeLabel;
        private System.Windows.Forms.Label lblBeneficeNet;
        private System.Windows.Forms.Label lblMarge;
        private System.Windows.Forms.TabPage tabAnnuel;
        private System.Windows.Forms.TableLayoutPanel tlpAnnuel;
        private System.Windows.Forms.FlowLayoutPanel pnlFiltreAnnuel;
        private System.Windows.Forms.Label lblAnneeAnnuelLabel;
        private System.Windows.Forms.ComboBox cbAnneeAnnuel;
        private System.Windows.Forms.DataGridView dgvAnnuel;
        private System.Windows.Forms.TabPage tabHistorique;
        private System.Windows.Forms.TableLayoutPanel tlpHisto;
        private System.Windows.Forms.FlowLayoutPanel pnlFiltreHisto;
        private System.Windows.Forms.Label lblFiltreHistoLabel;
        private System.Windows.Forms.ComboBox cbFiltreHistoCat;
        private System.Windows.Forms.Button btnSupprimerDep;
        private System.Windows.Forms.DataGridView dgvHistorique;
        private System.Windows.Forms.Label lblTotalHistorique;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMois;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCOGS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepenses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBenefice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoMontant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoUser;
    }
}
