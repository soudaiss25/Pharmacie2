namespace Pharmacie2.views
{
    partial class FormAddProduit
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
            this.pnlVerification = new System.Windows.Forms.TableLayoutPanel();
            this.lblVerification = new System.Windows.Forms.Label();
            this.flpVerification = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCorrection = new System.Windows.Forms.Button();
            this.btnStockCorrect = new System.Windows.Forms.Button();
            this.btnCompte = new System.Windows.Forms.Button();
            this.panelContenu = new System.Windows.Forms.Panel();
            this.tlpContenu = new System.Windows.Forms.TableLayoutPanel();
            this.tlpGauche = new System.Windows.Forms.TableLayoutPanel();
            this.lblSectionProduit = new System.Windows.Forms.Label();
            this.tlpProduit = new System.Windows.Forms.TableLayoutPanel();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblPrixAchat = new System.Windows.Forms.Label();
            this.numPrixAchat = new System.Windows.Forms.NumericUpDown();
            this.lblMarge = new System.Windows.Forms.Label();
            this.numMarge = new System.Windows.Forms.NumericUpDown();
            this.lblPrixVente = new System.Windows.Forms.Label();
            this.numPrixVente = new System.Windows.Forms.NumericUpDown();
            this.lblFournisseur = new System.Windows.Forms.Label();
            this.cbFournisseur = new System.Windows.Forms.ComboBox();
            this.lblSectionInfos = new System.Windows.Forms.Label();
            this.tlpInfos = new System.Windows.Forms.TableLayoutPanel();
            this.lblIndication = new System.Windows.Forms.Label();
            this.txtIndication = new System.Windows.Forms.TextBox();
            this.lblPosologie = new System.Windows.Forms.Label();
            this.txtPosologie = new System.Windows.Forms.TextBox();
            this.lblPosologieJour = new System.Windows.Forms.Label();
            this.numPosologieJour = new System.Windows.Forms.NumericUpDown();
            this.tlpDroite = new System.Windows.Forms.TableLayoutPanel();
            this.lblSectionStock = new System.Windows.Forms.Label();
            this.tlpStock = new System.Windows.Forms.TableLayoutPanel();
            this.lblQuantite = new System.Windows.Forms.Label();
            this.flpQuantite = new System.Windows.Forms.FlowLayoutPanel();
            this.numQuantite = new System.Windows.Forms.NumericUpDown();
            this.lblUnitesVrac = new System.Windows.Forms.Label();
            this.numUnitesVrac = new System.Windows.Forms.NumericUpDown();
            this.lblSeuil = new System.Windows.Forms.Label();
            this.numSeuil = new System.Windows.Forms.NumericUpDown();
            this.lblDateExpiration = new System.Windows.Forms.Label();
            this.dtpDateExpiration = new System.Windows.Forms.DateTimePicker();
            this.lblSectionDetail = new System.Windows.Forms.Label();
            this.chkVenteDetail = new System.Windows.Forms.CheckBox();
            this.pnlVenteDetail = new System.Windows.Forms.TableLayoutPanel();
            this.lblUniteVente = new System.Windows.Forms.Label();
            this.cmbUniteVente = new System.Windows.Forms.ComboBox();
            this.lblNbUnite = new System.Windows.Forms.Label();
            this.numNbUniteParBoite = new System.Windows.Forms.NumericUpDown();
            this.lblApercu = new System.Windows.Forms.Label();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixAchat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixVente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosologieJour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitesVrac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeuil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNbUniteParBoite)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.pnlVerification.SuspendLayout();
            this.flpVerification.SuspendLayout();
            this.panelContenu.SuspendLayout();
            this.tlpContenu.SuspendLayout();
            this.tlpGauche.SuspendLayout();
            this.tlpProduit.SuspendLayout();
            this.tlpInfos.SuspendLayout();
            this.tlpDroite.SuspendLayout();
            this.tlpStock.SuspendLayout();
            this.flpQuantite.SuspendLayout();
            this.pnlVenteDetail.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.pnlVerification, 0, 0);
            this.tlpRoot.Controls.Add(this.panelContenu, 0, 1);
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 2);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // pnlVerification
            // 
            this.pnlVerification.ColumnCount = 1;
            this.pnlVerification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlVerification.RowCount = 2;
            this.pnlVerification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVerification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVerification.Controls.Add(this.lblVerification, 0, 0);
            this.pnlVerification.Controls.Add(this.flpVerification, 0, 1);
            this.pnlVerification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVerification.AutoSize = true;
            this.pnlVerification.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlVerification.Visible = false;
            this.pnlVerification.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlVerification.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlVerification.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
            this.pnlVerification.Name = "pnlVerification";
            // 
            // lblVerification
            // 
            this.lblVerification.Text = "";
            this.lblVerification.AutoSize = true;
            this.lblVerification.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerification.ForeColor = System.Drawing.Color.FromArgb(230, 81, 0);
            this.lblVerification.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblVerification.Name = "lblVerification";
            // 
            // flpVerification
            // 
            this.flpVerification.Controls.Add(this.btnCorrection);
            this.flpVerification.Controls.Add(this.btnStockCorrect);
            this.flpVerification.Controls.Add(this.btnCompte);
            this.flpVerification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpVerification.AutoSize = true;
            this.flpVerification.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpVerification.WrapContents = true;
            this.flpVerification.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpVerification.Name = "flpVerification";
            // 
            // btnCorrection
            // 
            this.btnCorrection.Text = "Appliquer la correction";
            this.btnCorrection.Tag = "primaire";
            this.btnCorrection.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCorrection.Name = "btnCorrection";
            // 
            // btnStockCorrect
            // 
            this.btnStockCorrect.Text = "Le stock est correct";
            this.btnStockCorrect.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnStockCorrect.Name = "btnStockCorrect";
            // 
            // btnCompte
            // 
            this.btnCompte.Text = "J'ai compté le stock";
            this.btnCompte.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCompte.Name = "btnCompte";
            // 
            // panelContenu
            // 
            this.panelContenu.Controls.Add(this.tlpContenu);
            this.panelContenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenu.AutoScroll = true;
            this.panelContenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelContenu.Name = "panelContenu";
            // 
            // tlpContenu
            // 
            this.tlpContenu.ColumnCount = 2;
            this.tlpContenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpContenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpContenu.RowCount = 1;
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.Controls.Add(this.tlpGauche, 0, 0);
            this.tlpContenu.Controls.Add(this.tlpDroite, 1, 0);
            this.tlpContenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpContenu.AutoSize = true;
            this.tlpContenu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpContenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpContenu.Name = "tlpContenu";
            // 
            // tlpGauche
            // 
            this.tlpGauche.ColumnCount = 1;
            this.tlpGauche.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpGauche.RowCount = 4;
            this.tlpGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpGauche.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpGauche.Controls.Add(this.lblSectionProduit, 0, 0);
            this.tlpGauche.Controls.Add(this.tlpProduit, 0, 1);
            this.tlpGauche.Controls.Add(this.lblSectionInfos, 0, 2);
            this.tlpGauche.Controls.Add(this.tlpInfos, 0, 3);
            this.tlpGauche.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpGauche.AutoSize = true;
            this.tlpGauche.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpGauche.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tlpGauche.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpGauche.Name = "tlpGauche";
            // 
            // lblSectionProduit
            // 
            this.lblSectionProduit.Text = "Le produit";
            this.lblSectionProduit.AutoSize = true;
            this.lblSectionProduit.Tag = "section";
            this.lblSectionProduit.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblSectionProduit.Name = "lblSectionProduit";
            // 
            // tlpProduit
            // 
            this.tlpProduit.ColumnCount = 2;
            this.tlpProduit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpProduit.RowCount = 6;
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpProduit.Controls.Add(this.lblNom, 0, 0);
            this.tlpProduit.Controls.Add(this.txtNom, 1, 0);
            this.tlpProduit.Controls.Add(this.lblType, 0, 1);
            this.tlpProduit.Controls.Add(this.cmbType, 1, 1);
            this.tlpProduit.Controls.Add(this.lblPrixAchat, 0, 2);
            this.tlpProduit.Controls.Add(this.numPrixAchat, 1, 2);
            this.tlpProduit.Controls.Add(this.lblMarge, 0, 3);
            this.tlpProduit.Controls.Add(this.numMarge, 1, 3);
            this.tlpProduit.Controls.Add(this.lblPrixVente, 0, 4);
            this.tlpProduit.Controls.Add(this.numPrixVente, 1, 4);
            this.tlpProduit.Controls.Add(this.lblFournisseur, 0, 5);
            this.tlpProduit.Controls.Add(this.cbFournisseur, 1, 5);
            this.tlpProduit.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpProduit.AutoSize = true;
            this.tlpProduit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpProduit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpProduit.Name = "tlpProduit";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom du produit *";
            this.lblNom.AutoSize = true;
            this.lblNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNom.Name = "lblNom";
            // 
            // txtNom
            // 
            this.txtNom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNom.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtNom.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNom.PlaceholderText = "Ex : Doliprane 500 mg";
            this.txtNom.Name = "txtNom";
            // 
            // lblType
            // 
            this.lblType.Text = "Type *";
            this.lblType.AutoSize = true;
            this.lblType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblType.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblType.Name = "lblType";
            // 
            // cmbType
            // 
            this.cmbType.Width = 200;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbType.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cmbType.Items.AddRange(new object[] {
            "Médicament",
            "Matériel",
            "Produit d'hygiène",
            "Autre"});
            this.cmbType.Name = "cmbType";
            // 
            // lblPrixAchat
            // 
            this.lblPrixAchat.Text = "Prix d'achat (KMF) *";
            this.lblPrixAchat.AutoSize = true;
            this.lblPrixAchat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrixAchat.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPrixAchat.Name = "lblPrixAchat";
            // 
            // numPrixAchat
            // 
            this.numPrixAchat.Width = 150;
            this.numPrixAchat.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numPrixAchat.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numPrixAchat.Value = new decimal(new int[] { 0, 0, 0, 0});
            this.numPrixAchat.ThousandsSeparator = true;
            this.numPrixAchat.DecimalPlaces = 0;
            this.numPrixAchat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPrixAchat.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numPrixAchat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numPrixAchat.Name = "numPrixAchat";
            // 
            // lblMarge
            // 
            this.lblMarge.Text = "Marge bénéfice (%)";
            this.lblMarge.AutoSize = true;
            this.lblMarge.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMarge.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMarge.ForeColor = System.Drawing.Color.FromArgb(69, 90, 100);
            this.lblMarge.Name = "lblMarge";
            // 
            // numMarge
            // 
            this.numMarge.Width = 100;
            this.numMarge.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numMarge.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numMarge.Value = new decimal(new int[] { 0, 0, 0, 0});
            this.numMarge.ThousandsSeparator = true;
            this.numMarge.DecimalPlaces = 1;
            this.numMarge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMarge.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numMarge.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMarge.Name = "numMarge";
            // 
            // lblPrixVente
            // 
            this.lblPrixVente.Text = "Prix de vente (KMF) *";
            this.lblPrixVente.AutoSize = true;
            this.lblPrixVente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrixVente.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPrixVente.Name = "lblPrixVente";
            // 
            // numPrixVente
            // 
            this.numPrixVente.Width = 150;
            this.numPrixVente.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numPrixVente.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numPrixVente.Value = new decimal(new int[] { 0, 0, 0, 0});
            this.numPrixVente.ThousandsSeparator = true;
            this.numPrixVente.DecimalPlaces = 0;
            this.numPrixVente.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPrixVente.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numPrixVente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numPrixVente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPrixVente.Name = "numPrixVente";
            // 
            // lblFournisseur
            // 
            this.lblFournisseur.Text = "Fournisseur";
            this.lblFournisseur.AutoSize = true;
            this.lblFournisseur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFournisseur.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblFournisseur.Name = "lblFournisseur";
            // 
            // cbFournisseur
            // 
            this.cbFournisseur.Width = 280;
            this.cbFournisseur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFournisseur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFournisseur.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbFournisseur.Name = "cbFournisseur";
            // 
            // lblSectionInfos
            // 
            this.lblSectionInfos.Text = "Informations pour le caissier";
            this.lblSectionInfos.AutoSize = true;
            this.lblSectionInfos.Tag = "section";
            this.lblSectionInfos.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblSectionInfos.Name = "lblSectionInfos";
            // 
            // tlpInfos
            // 
            this.tlpInfos.ColumnCount = 2;
            this.tlpInfos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInfos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpInfos.RowCount = 3;
            this.tlpInfos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInfos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInfos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInfos.Controls.Add(this.lblIndication, 0, 0);
            this.tlpInfos.Controls.Add(this.txtIndication, 1, 0);
            this.tlpInfos.Controls.Add(this.lblPosologie, 0, 1);
            this.tlpInfos.Controls.Add(this.txtPosologie, 1, 1);
            this.tlpInfos.Controls.Add(this.lblPosologieJour, 0, 2);
            this.tlpInfos.Controls.Add(this.numPosologieJour, 1, 2);
            this.tlpInfos.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpInfos.AutoSize = true;
            this.tlpInfos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpInfos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpInfos.Name = "tlpInfos";
            // 
            // lblIndication
            // 
            this.lblIndication.Text = "Pour quoi ?";
            this.lblIndication.AutoSize = true;
            this.lblIndication.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblIndication.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblIndication.Name = "lblIndication";
            // 
            // txtIndication
            // 
            this.txtIndication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIndication.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtIndication.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtIndication.PlaceholderText = "Ex : toux, fièvre, hypertension";
            this.txtIndication.Name = "txtIndication";
            // 
            // lblPosologie
            // 
            this.lblPosologie.Text = "Comment le prendre ?";
            this.lblPosologie.AutoSize = true;
            this.lblPosologie.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPosologie.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPosologie.Name = "lblPosologie";
            // 
            // txtPosologie
            // 
            this.txtPosologie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPosologie.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtPosologie.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtPosologie.PlaceholderText = "Ex : 1 comprimé après le repas";
            this.txtPosologie.Name = "txtPosologie";
            // 
            // lblPosologieJour
            // 
            this.lblPosologieJour.Text = "Prises par jour";
            this.lblPosologieJour.AutoSize = true;
            this.lblPosologieJour.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPosologieJour.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPosologieJour.Name = "lblPosologieJour";
            // 
            // numPosologieJour
            // 
            this.numPosologieJour.Width = 80;
            this.numPosologieJour.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numPosologieJour.Maximum = new decimal(new int[] { 10, 0, 0, 0});
            this.numPosologieJour.Value = new decimal(new int[] { 1, 0, 0, 0});
            this.numPosologieJour.ThousandsSeparator = true;
            this.numPosologieJour.DecimalPlaces = 0;
            this.numPosologieJour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPosologieJour.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numPosologieJour.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numPosologieJour.Name = "numPosologieJour";
            // 
            // tlpDroite
            // 
            this.tlpDroite.ColumnCount = 1;
            this.tlpDroite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpDroite.RowCount = 5;
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.Controls.Add(this.lblSectionStock, 0, 0);
            this.tlpDroite.Controls.Add(this.tlpStock, 0, 1);
            this.tlpDroite.Controls.Add(this.lblSectionDetail, 0, 2);
            this.tlpDroite.Controls.Add(this.chkVenteDetail, 0, 3);
            this.tlpDroite.Controls.Add(this.pnlVenteDetail, 0, 4);
            this.tlpDroite.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDroite.AutoSize = true;
            this.tlpDroite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpDroite.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpDroite.Name = "tlpDroite";
            // 
            // lblSectionStock
            // 
            this.lblSectionStock.Text = "Stock";
            this.lblSectionStock.AutoSize = true;
            this.lblSectionStock.Tag = "section";
            this.lblSectionStock.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblSectionStock.Name = "lblSectionStock";
            // 
            // tlpStock
            // 
            this.tlpStock.ColumnCount = 2;
            this.tlpStock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpStock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpStock.RowCount = 3;
            this.tlpStock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpStock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpStock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpStock.Controls.Add(this.lblQuantite, 0, 0);
            this.tlpStock.Controls.Add(this.flpQuantite, 1, 0);
            this.tlpStock.Controls.Add(this.lblSeuil, 0, 1);
            this.tlpStock.Controls.Add(this.numSeuil, 1, 1);
            this.tlpStock.Controls.Add(this.lblDateExpiration, 0, 2);
            this.tlpStock.Controls.Add(this.dtpDateExpiration, 1, 2);
            this.tlpStock.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpStock.AutoSize = true;
            this.tlpStock.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpStock.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpStock.Name = "tlpStock";
            // 
            // lblQuantite
            // 
            this.lblQuantite.Text = "Boîtes pleines en stock *";
            this.lblQuantite.AutoSize = true;
            this.lblQuantite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblQuantite.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblQuantite.Name = "lblQuantite";
            // 
            // flpQuantite
            // 
            this.flpQuantite.Controls.Add(this.numQuantite);
            this.flpQuantite.Controls.Add(this.lblUnitesVrac);
            this.flpQuantite.Controls.Add(this.numUnitesVrac);
            this.flpQuantite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpQuantite.AutoSize = true;
            this.flpQuantite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpQuantite.WrapContents = true;
            this.flpQuantite.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpQuantite.Name = "flpQuantite";
            // 
            // numQuantite
            // 
            this.numQuantite.Width = 110;
            this.numQuantite.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numQuantite.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numQuantite.ThousandsSeparator = true;
            this.numQuantite.DecimalPlaces = 0;
            this.numQuantite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numQuantite.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numQuantite.Name = "numQuantite";
            // 
            // lblUnitesVrac
            // 
            this.lblUnitesVrac.Text = "+ unités en vrac";
            this.lblUnitesVrac.AutoSize = true;
            this.lblUnitesVrac.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblUnitesVrac.Name = "lblUnitesVrac";
            // 
            // numUnitesVrac
            // 
            this.numUnitesVrac.Width = 90;
            this.numUnitesVrac.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numUnitesVrac.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numUnitesVrac.ThousandsSeparator = true;
            this.numUnitesVrac.DecimalPlaces = 0;
            this.numUnitesVrac.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUnitesVrac.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numUnitesVrac.Enabled = false;
            this.numUnitesVrac.Name = "numUnitesVrac";
            // 
            // lblSeuil
            // 
            this.lblSeuil.Text = "Seuil d'alerte (en boîtes) *";
            this.lblSeuil.AutoSize = true;
            this.lblSeuil.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSeuil.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblSeuil.Name = "lblSeuil";
            // 
            // numSeuil
            // 
            this.numSeuil.Width = 110;
            this.numSeuil.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numSeuil.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numSeuil.Value = new decimal(new int[] { 10, 0, 0, 0});
            this.numSeuil.ThousandsSeparator = true;
            this.numSeuil.DecimalPlaces = 0;
            this.numSeuil.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSeuil.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numSeuil.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numSeuil.Name = "numSeuil";
            // 
            // lblDateExpiration
            // 
            this.lblDateExpiration.Text = "Date d'expiration *";
            this.lblDateExpiration.AutoSize = true;
            this.lblDateExpiration.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDateExpiration.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblDateExpiration.Name = "lblDateExpiration";
            // 
            // dtpDateExpiration
            // 
            this.dtpDateExpiration.Width = 150;
            this.dtpDateExpiration.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateExpiration.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpDateExpiration.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpDateExpiration.Name = "dtpDateExpiration";
            // 
            // lblSectionDetail
            // 
            this.lblSectionDetail.Text = "Vente en détail (facultatif)";
            this.lblSectionDetail.AutoSize = true;
            this.lblSectionDetail.Tag = "section";
            this.lblSectionDetail.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblSectionDetail.Name = "lblSectionDetail";
            // 
            // chkVenteDetail
            // 
            this.chkVenteDetail.Text = "Peut être vendu à l'unité (plaquette, comprimé, flacon…)";
            this.chkVenteDetail.AutoSize = true;
            this.chkVenteDetail.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.chkVenteDetail.Name = "chkVenteDetail";
            // 
            // pnlVenteDetail
            // 
            this.pnlVenteDetail.ColumnCount = 2;
            this.pnlVenteDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVenteDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlVenteDetail.RowCount = 3;
            this.pnlVenteDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVenteDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVenteDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlVenteDetail.Controls.Add(this.lblUniteVente, 0, 0);
            this.pnlVenteDetail.Controls.Add(this.cmbUniteVente, 1, 0);
            this.pnlVenteDetail.Controls.Add(this.lblNbUnite, 0, 1);
            this.pnlVenteDetail.Controls.Add(this.numNbUniteParBoite, 1, 1);
            this.pnlVenteDetail.Controls.Add(this.lblApercu, 0, 2);
            this.pnlVenteDetail.SetColumnSpan(this.lblApercu, 2);
            this.pnlVenteDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlVenteDetail.AutoSize = true;
            this.pnlVenteDetail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlVenteDetail.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pnlVenteDetail.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlVenteDetail.Enabled = false;
            this.pnlVenteDetail.Tag = "carte";
            this.pnlVenteDetail.Name = "pnlVenteDetail";
            // 
            // lblUniteVente
            // 
            this.lblUniteVente.Text = "Unité de vente";
            this.lblUniteVente.AutoSize = true;
            this.lblUniteVente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUniteVente.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblUniteVente.Name = "lblUniteVente";
            // 
            // cmbUniteVente
            // 
            this.cmbUniteVente.Width = 160;
            this.cmbUniteVente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUniteVente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbUniteVente.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cmbUniteVente.Items.AddRange(new object[] {
            "Plaquette",
            "Comprimé",
            "Gélule",
            "Ampoule",
            "Flacon",
            "Sachet",
            "Tube",
            "Autre"});
            this.cmbUniteVente.Name = "cmbUniteVente";
            // 
            // lblNbUnite
            // 
            this.lblNbUnite.Text = "Nombre d'unités par boîte";
            this.lblNbUnite.AutoSize = true;
            this.lblNbUnite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNbUnite.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNbUnite.Name = "lblNbUnite";
            // 
            // numNbUniteParBoite
            // 
            this.numNbUniteParBoite.Width = 90;
            this.numNbUniteParBoite.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numNbUniteParBoite.Maximum = new decimal(new int[] { 1000, 0, 0, 0});
            this.numNbUniteParBoite.Value = new decimal(new int[] { 2, 0, 0, 0});
            this.numNbUniteParBoite.ThousandsSeparator = true;
            this.numNbUniteParBoite.DecimalPlaces = 0;
            this.numNbUniteParBoite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNbUniteParBoite.Margin = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.numNbUniteParBoite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numNbUniteParBoite.Name = "numNbUniteParBoite";
            // 
            // lblApercu
            // 
            this.lblApercu.Text = "";
            this.lblApercu.AutoSize = true;
            this.lblApercu.Tag = "succes";
            this.lblApercu.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblApercu.Name = "lblApercu";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnEnregistrer);
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnEnregistrer
            // 
            this.btnEnregistrer.Text = "Enregistrer";
            this.btnEnregistrer.Tag = "primaire";
            this.btnEnregistrer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // FormAddProduit
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1020, 560);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Produit";
            this.AcceptButton = this.btnEnregistrer;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormAddProduit";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.pnlVenteDetail.ResumeLayout(false);
            this.pnlVenteDetail.PerformLayout();
            this.flpQuantite.ResumeLayout(false);
            this.flpQuantite.PerformLayout();
            this.tlpStock.ResumeLayout(false);
            this.tlpStock.PerformLayout();
            this.tlpDroite.ResumeLayout(false);
            this.tlpDroite.PerformLayout();
            this.tlpInfos.ResumeLayout(false);
            this.tlpInfos.PerformLayout();
            this.tlpProduit.ResumeLayout(false);
            this.tlpProduit.PerformLayout();
            this.tlpGauche.ResumeLayout(false);
            this.tlpGauche.PerformLayout();
            this.tlpContenu.ResumeLayout(false);
            this.tlpContenu.PerformLayout();
            this.panelContenu.ResumeLayout(false);
            this.flpVerification.ResumeLayout(false);
            this.flpVerification.PerformLayout();
            this.pnlVerification.ResumeLayout(false);
            this.pnlVerification.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNbUniteParBoite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeuil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitesVrac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosologieJour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixVente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixAchat)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel pnlVerification;
        private System.Windows.Forms.Label lblVerification;
        private System.Windows.Forms.FlowLayoutPanel flpVerification;
        private System.Windows.Forms.Button btnCorrection;
        private System.Windows.Forms.Button btnStockCorrect;
        private System.Windows.Forms.Button btnCompte;
        private System.Windows.Forms.Panel panelContenu;
        private System.Windows.Forms.TableLayoutPanel tlpContenu;
        private System.Windows.Forms.TableLayoutPanel tlpGauche;
        private System.Windows.Forms.Label lblSectionProduit;
        private System.Windows.Forms.TableLayoutPanel tlpProduit;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblPrixAchat;
        private System.Windows.Forms.NumericUpDown numPrixAchat;
        private System.Windows.Forms.Label lblMarge;
        private System.Windows.Forms.NumericUpDown numMarge;
        private System.Windows.Forms.Label lblPrixVente;
        private System.Windows.Forms.NumericUpDown numPrixVente;
        private System.Windows.Forms.Label lblFournisseur;
        private System.Windows.Forms.ComboBox cbFournisseur;
        private System.Windows.Forms.Label lblSectionInfos;
        private System.Windows.Forms.TableLayoutPanel tlpInfos;
        private System.Windows.Forms.Label lblIndication;
        private System.Windows.Forms.TextBox txtIndication;
        private System.Windows.Forms.Label lblPosologie;
        private System.Windows.Forms.TextBox txtPosologie;
        private System.Windows.Forms.Label lblPosologieJour;
        private System.Windows.Forms.NumericUpDown numPosologieJour;
        private System.Windows.Forms.TableLayoutPanel tlpDroite;
        private System.Windows.Forms.Label lblSectionStock;
        private System.Windows.Forms.TableLayoutPanel tlpStock;
        private System.Windows.Forms.Label lblQuantite;
        private System.Windows.Forms.FlowLayoutPanel flpQuantite;
        private System.Windows.Forms.NumericUpDown numQuantite;
        private System.Windows.Forms.Label lblUnitesVrac;
        private System.Windows.Forms.NumericUpDown numUnitesVrac;
        private System.Windows.Forms.Label lblSeuil;
        private System.Windows.Forms.NumericUpDown numSeuil;
        private System.Windows.Forms.Label lblDateExpiration;
        private System.Windows.Forms.DateTimePicker dtpDateExpiration;
        private System.Windows.Forms.Label lblSectionDetail;
        private System.Windows.Forms.CheckBox chkVenteDetail;
        private System.Windows.Forms.TableLayoutPanel pnlVenteDetail;
        private System.Windows.Forms.Label lblUniteVente;
        private System.Windows.Forms.ComboBox cmbUniteVente;
        private System.Windows.Forms.Label lblNbUnite;
        private System.Windows.Forms.NumericUpDown numNbUniteParBoite;
        private System.Windows.Forms.Label lblApercu;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
