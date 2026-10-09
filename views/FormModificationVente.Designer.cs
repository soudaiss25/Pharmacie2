namespace Pharmacie2.views
{
    partial class FormModificationVente
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
            this.tlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPanier = new System.Windows.Forms.TableLayoutPanel();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
            this.tlpTotal = new System.Windows.Forms.TableLayoutPanel();
            this.btnAjouter = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblMontantTotal = new System.Windows.Forms.Label();
            this.panelDroit = new System.Windows.Forms.Panel();
            this.tlpDroite = new System.Windows.Forms.TableLayoutPanel();
            this.lblClient = new System.Windows.Forms.Label();
            this.tlpClient = new System.Windows.Forms.TableLayoutPanel();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblPrenom = new System.Windows.Forms.Label();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.lblTelephone = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.lblMotif = new System.Windows.Forms.Label();
            this.txtMotif = new System.Windows.Forms.TextBox();
            this.lblSectionPaiement = new System.Windows.Forms.Label();
            this.tlpMode = new System.Windows.Forms.TableLayoutPanel();
            this.lblPaiement = new System.Windows.Forms.Label();
            this.cbPaiement = new System.Windows.Forms.ComboBox();
            this.pnlMatricule = new System.Windows.Forms.TableLayoutPanel();
            this.lblMatriculeLabel = new System.Windows.Forms.Label();
            this.txtMatricule = new System.Windows.Forms.TextBox();
            this.pnlMutuelle = new System.Windows.Forms.TableLayoutPanel();
            this.lblMutuelle = new System.Windows.Forms.Label();
            this.cbMutuelle = new System.Windows.Forms.ComboBox();
            this.lblNotePaiement = new System.Windows.Forms.Label();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixUnitaire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSousTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpPrincipal.SuspendLayout();
            this.tlpPanier.SuspendLayout();
            this.tlpTotal.SuspendLayout();
            this.panelDroit.SuspendLayout();
            this.tlpDroite.SuspendLayout();
            this.tlpClient.SuspendLayout();
            this.tlpMode.SuspendLayout();
            this.pnlMatricule.SuspendLayout();
            this.pnlMutuelle.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpPrincipal, 0, 1);
            this.tlpPrincipal.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 2);
            this.flpBoutons.TabIndex = 2;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Modifier la vente";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpPrincipal
            // 
            this.tlpPrincipal.ColumnCount = 2;
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.0F));
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.0F));
            this.tlpPrincipal.RowCount = 1;
            this.tlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPrincipal.Controls.Add(this.tlpPanier, 0, 0);
            this.tlpPanier.TabIndex = 0;
            this.tlpPrincipal.Controls.Add(this.panelDroit, 1, 0);
            this.panelDroit.TabIndex = 1;
            this.tlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrincipal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpPrincipal.Name = "tlpPrincipal";
            // 
            // tlpPanier
            // 
            this.tlpPanier.ColumnCount = 1;
            this.tlpPanier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPanier.RowCount = 2;
            this.tlpPanier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPanier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPanier.Controls.Add(this.dgvProduits, 0, 0);
            this.dgvProduits.TabIndex = 0;
            this.tlpPanier.Controls.Add(this.tlpTotal, 0, 1);
            this.tlpTotal.TabIndex = 1;
            this.tlpPanier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPanier.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tlpPanier.Name = "tlpPanier";
            // 
            // dgvProduits
            // 
            this.dgvProduits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduit,
            this.colUnite,
            this.colQuantite,
            this.colPrixUnitaire,
            this.colSousTotal});
            this.dgvProduits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduits.AutoGenerateColumns = false;
            this.dgvProduits.ReadOnly = true;
            this.dgvProduits.Name = "dgvProduits";
            // 
            // tlpTotal
            // 
            this.tlpTotal.ColumnCount = 5;
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.RowCount = 1;
            this.tlpTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.Controls.Add(this.btnAjouter, 0, 0);
            this.btnAjouter.TabIndex = 0;
            this.tlpTotal.Controls.Add(this.btnSupprimer, 1, 0);
            this.btnSupprimer.TabIndex = 1;
            this.tlpTotal.Controls.Add(this.lblTotal, 3, 0);
            this.lblTotal.TabIndex = 2;
            this.tlpTotal.Controls.Add(this.lblMontantTotal, 4, 0);
            this.lblMontantTotal.TabIndex = 3;
            this.tlpTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTotal.AutoSize = true;
            this.tlpTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTotal.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpTotal.Name = "tlpTotal";
            // 
            // btnAjouter
            // 
            this.btnAjouter.Text = "Ajouter un produit";
            this.btnAjouter.Tag = "primaire";
            this.btnAjouter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Text = "Retirer la ligne";
            this.btnSupprimer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Text = "Total";
            this.lblTotal.AutoSize = true;
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotal.Tag = "section";
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblTotal.Name = "lblTotal";
            // 
            // lblMontantTotal
            // 
            this.lblMontantTotal.Text = "0 KMF";
            this.lblMontantTotal.AutoSize = true;
            this.lblMontantTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMontantTotal.Tag = "kpi";
            this.lblMontantTotal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblMontantTotal.Name = "lblMontantTotal";
            // 
            // panelDroit
            // 
            this.panelDroit.Controls.Add(this.tlpDroite);
            this.tlpDroite.TabIndex = 0;
            this.panelDroit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDroit.AutoScroll = true;
            this.panelDroit.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.panelDroit.Name = "panelDroit";
            // 
            // tlpDroite
            // 
            this.tlpDroite.ColumnCount = 1;
            this.tlpDroite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpDroite.RowCount = 7;
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDroite.Controls.Add(this.lblClient, 0, 0);
            this.lblClient.TabIndex = 0;
            this.tlpDroite.Controls.Add(this.tlpClient, 0, 1);
            this.tlpClient.TabIndex = 1;
            this.tlpDroite.Controls.Add(this.lblSectionPaiement, 0, 2);
            this.lblSectionPaiement.TabIndex = 2;
            this.tlpDroite.Controls.Add(this.tlpMode, 0, 3);
            this.tlpMode.TabIndex = 3;
            this.tlpDroite.Controls.Add(this.pnlMatricule, 0, 4);
            this.pnlMatricule.TabIndex = 4;
            this.tlpDroite.Controls.Add(this.pnlMutuelle, 0, 5);
            this.pnlMutuelle.TabIndex = 5;
            this.tlpDroite.Controls.Add(this.lblNotePaiement, 0, 6);
            this.lblNotePaiement.TabIndex = 6;
            this.tlpDroite.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDroite.AutoSize = true;
            this.tlpDroite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpDroite.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.tlpDroite.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpDroite.Name = "tlpDroite";
            // 
            // lblClient
            // 
            this.lblClient.Text = "Client";
            this.lblClient.AutoSize = true;
            this.lblClient.Tag = "section";
            this.lblClient.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblClient.Name = "lblClient";
            // 
            // tlpClient
            // 
            this.tlpClient.ColumnCount = 2;
            this.tlpClient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpClient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpClient.RowCount = 4;
            this.tlpClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpClient.Controls.Add(this.lblNom, 0, 0);
            this.lblNom.TabIndex = 0;
            this.tlpClient.Controls.Add(this.txtNom, 1, 0);
            this.txtNom.TabIndex = 1;
            this.tlpClient.Controls.Add(this.lblPrenom, 0, 1);
            this.lblPrenom.TabIndex = 2;
            this.tlpClient.Controls.Add(this.txtPrenom, 1, 1);
            this.txtPrenom.TabIndex = 3;
            this.tlpClient.Controls.Add(this.lblTelephone, 0, 2);
            this.lblTelephone.TabIndex = 4;
            this.tlpClient.Controls.Add(this.txtTelephone, 1, 2);
            this.txtTelephone.TabIndex = 5;
            this.tlpClient.Controls.Add(this.lblMotif, 0, 3);
            this.lblMotif.TabIndex = 6;
            this.tlpClient.Controls.Add(this.txtMotif, 1, 3);
            this.txtMotif.TabIndex = 7;
            this.tlpClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpClient.AutoSize = true;
            this.tlpClient.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpClient.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpClient.Name = "tlpClient";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom";
            this.lblNom.AutoSize = true;
            this.lblNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNom.Name = "lblNom";
            // 
            // txtNom
            // 
            this.txtNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNom.Width = 400;
            this.txtNom.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNom.Name = "txtNom";
            // 
            // lblPrenom
            // 
            this.lblPrenom.Text = "Prénom";
            this.lblPrenom.AutoSize = true;
            this.lblPrenom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrenom.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPrenom.Name = "lblPrenom";
            // 
            // txtPrenom
            // 
            this.txtPrenom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPrenom.Width = 400;
            this.txtPrenom.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtPrenom.Name = "txtPrenom";
            // 
            // lblTelephone
            // 
            this.lblTelephone.Text = "Téléphone";
            this.lblTelephone.AutoSize = true;
            this.lblTelephone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelephone.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblTelephone.Name = "lblTelephone";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTelephone.Width = 400;
            this.txtTelephone.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtTelephone.Name = "txtTelephone";
            // 
            // lblMotif
            // 
            this.lblMotif.Text = "Motif ou ordonnance";
            this.lblMotif.AutoSize = true;
            this.lblMotif.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMotif.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMotif.Name = "lblMotif";
            // 
            // txtMotif
            // 
            this.txtMotif.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMotif.Width = 400;
            this.txtMotif.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtMotif.Name = "txtMotif";
            // 
            // lblSectionPaiement
            // 
            this.lblSectionPaiement.Text = "Paiement";
            this.lblSectionPaiement.AutoSize = true;
            this.lblSectionPaiement.Tag = "section";
            this.lblSectionPaiement.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblSectionPaiement.Name = "lblSectionPaiement";
            // 
            // tlpMode
            // 
            this.tlpMode.ColumnCount = 2;
            this.tlpMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMode.RowCount = 1;
            this.tlpMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMode.Controls.Add(this.lblPaiement, 0, 0);
            this.lblPaiement.TabIndex = 0;
            this.tlpMode.Controls.Add(this.cbPaiement, 1, 0);
            this.cbPaiement.TabIndex = 1;
            this.tlpMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMode.AutoSize = true;
            this.tlpMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.tlpMode.Name = "tlpMode";
            // 
            // lblPaiement
            // 
            this.lblPaiement.Text = "Moyen de paiement";
            this.lblPaiement.AutoSize = true;
            this.lblPaiement.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPaiement.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPaiement.Name = "lblPaiement";
            // 
            // cbPaiement
            // 
            this.cbPaiement.Width = 220;
            this.cbPaiement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaiement.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbPaiement.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbPaiement.Name = "cbPaiement";
            this.cbPaiement.SelectedIndexChanged += new System.EventHandler(this.cbPaiement_SelectedIndexChanged);
            // 
            // pnlMatricule
            // 
            this.pnlMatricule.ColumnCount = 2;
            this.pnlMatricule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMatricule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlMatricule.RowCount = 1;
            this.pnlMatricule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMatricule.Controls.Add(this.lblMatriculeLabel, 0, 0);
            this.lblMatriculeLabel.TabIndex = 0;
            this.pnlMatricule.Controls.Add(this.txtMatricule, 1, 0);
            this.txtMatricule.TabIndex = 1;
            this.pnlMatricule.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMatricule.AutoSize = true;
            this.pnlMatricule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMatricule.Visible = false;
            this.pnlMatricule.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlMatricule.Name = "pnlMatricule";
            // 
            // lblMatriculeLabel
            // 
            this.lblMatriculeLabel.Text = "Matricule de l'employé";
            this.lblMatriculeLabel.AutoSize = true;
            this.lblMatriculeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMatriculeLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMatriculeLabel.Name = "lblMatriculeLabel";
            // 
            // txtMatricule
            // 
            this.txtMatricule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMatricule.Width = 400;
            this.txtMatricule.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtMatricule.Name = "txtMatricule";
            // 
            // pnlMutuelle
            // 
            this.pnlMutuelle.ColumnCount = 2;
            this.pnlMutuelle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlMutuelle.RowCount = 1;
            this.pnlMutuelle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.Controls.Add(this.lblMutuelle, 0, 0);
            this.lblMutuelle.TabIndex = 0;
            this.pnlMutuelle.Controls.Add(this.cbMutuelle, 1, 0);
            this.cbMutuelle.TabIndex = 1;
            this.pnlMutuelle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMutuelle.AutoSize = true;
            this.pnlMutuelle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMutuelle.Visible = false;
            this.pnlMutuelle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pnlMutuelle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlMutuelle.Tag = "carte";
            this.pnlMutuelle.Name = "pnlMutuelle";
            // 
            // lblMutuelle
            // 
            this.lblMutuelle.Text = "Mutuelle";
            this.lblMutuelle.AutoSize = true;
            this.lblMutuelle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMutuelle.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMutuelle.Name = "lblMutuelle";
            // 
            // cbMutuelle
            // 
            this.cbMutuelle.Width = 260;
            this.cbMutuelle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMutuelle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbMutuelle.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbMutuelle.Name = "cbMutuelle";
            // 
            // lblNotePaiement
            // 
            this.lblNotePaiement.Text = "Les sommes déjà versées par le client ne changent pas.";
            this.lblNotePaiement.AutoSize = true;
            this.lblNotePaiement.Tag = "note";
            this.lblNotePaiement.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblNotePaiement.Name = "lblNotePaiement";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnEnregistrer);
            this.btnEnregistrer.TabIndex = 0;
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 1;
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
            this.btnEnregistrer.Text = "Enregistrer les modifications";
            this.btnEnregistrer.Tag = "primaire";
            this.btnEnregistrer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler (Échap)";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "Produit";
            this.colProduit.DataPropertyName = "Produit";
            this.colProduit.FillWeight = 40F;
            this.colProduit.MinimumWidth = 140;
            this.colProduit.ReadOnly = true;
            // 
            // colUnite
            // 
            this.colUnite.HeaderText = "Unité";
            this.colUnite.Name = "Unite";
            this.colUnite.DataPropertyName = "Unite";
            this.colUnite.FillWeight = 14F;
            this.colUnite.MinimumWidth = 80;
            this.colUnite.ReadOnly = true;
            // 
            // colQuantite
            // 
            this.colQuantite.HeaderText = "Qté";
            this.colQuantite.Name = "Quantite";
            this.colQuantite.DataPropertyName = "Quantite";
            this.colQuantite.FillWeight = 10F;
            this.colQuantite.MinimumWidth = 50;
            this.colQuantite.ReadOnly = true;
            // 
            // colPrixUnitaire
            // 
            this.colPrixUnitaire.HeaderText = "Prix unitaire";
            this.colPrixUnitaire.Name = "PrixUnitaire";
            this.colPrixUnitaire.DataPropertyName = "PrixUnitaire";
            this.colPrixUnitaire.FillWeight = 18F;
            this.colPrixUnitaire.MinimumWidth = 100;
            this.colPrixUnitaire.ReadOnly = true;
            this.colPrixUnitaire.Tag = "montant";
            // 
            // colSousTotal
            // 
            this.colSousTotal.HeaderText = "Sous-total";
            this.colSousTotal.Name = "SousTotal";
            this.colSousTotal.DataPropertyName = "SousTotal";
            this.colSousTotal.FillWeight = 18F;
            this.colSousTotal.MinimumWidth = 100;
            this.colSousTotal.ReadOnly = true;
            this.colSousTotal.Tag = "montant";
            // 
            // FormModificationVente
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1180, 640);
            this.MinimumSize = new System.Drawing.Size(1000, 580);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modifier la vente";
            this.AcceptButton = this.btnEnregistrer;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormModificationVente";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.pnlMutuelle.ResumeLayout(false);
            this.pnlMutuelle.PerformLayout();
            this.pnlMatricule.ResumeLayout(false);
            this.pnlMatricule.PerformLayout();
            this.tlpMode.ResumeLayout(false);
            this.tlpMode.PerformLayout();
            this.tlpClient.ResumeLayout(false);
            this.tlpClient.PerformLayout();
            this.tlpDroite.ResumeLayout(false);
            this.tlpDroite.PerformLayout();
            this.panelDroit.ResumeLayout(false);
            this.tlpTotal.ResumeLayout(false);
            this.tlpTotal.PerformLayout();
            this.tlpPanier.ResumeLayout(false);
            this.tlpPanier.PerformLayout();
            this.tlpPrincipal.ResumeLayout(false);
            this.tlpPrincipal.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TableLayoutPanel tlpPrincipal;
        private System.Windows.Forms.TableLayoutPanel tlpPanier;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.TableLayoutPanel tlpTotal;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblMontantTotal;
        private System.Windows.Forms.Panel panelDroit;
        private System.Windows.Forms.TableLayoutPanel tlpDroite;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.TableLayoutPanel tlpClient;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblPrenom;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.Label lblTelephone;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Label lblMotif;
        private System.Windows.Forms.TextBox txtMotif;
        private System.Windows.Forms.Label lblSectionPaiement;
        private System.Windows.Forms.TableLayoutPanel tlpMode;
        private System.Windows.Forms.Label lblPaiement;
        private System.Windows.Forms.ComboBox cbPaiement;
        private System.Windows.Forms.TableLayoutPanel pnlMatricule;
        private System.Windows.Forms.Label lblMatriculeLabel;
        private System.Windows.Forms.TextBox txtMatricule;
        private System.Windows.Forms.TableLayoutPanel pnlMutuelle;
        private System.Windows.Forms.Label lblMutuelle;
        private System.Windows.Forms.ComboBox cbMutuelle;
        private System.Windows.Forms.Label lblNotePaiement;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixUnitaire;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSousTotal;
    }
}
