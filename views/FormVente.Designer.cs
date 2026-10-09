namespace Pharmacie2.views
{
    partial class FormVente
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
            this.tlpEntete = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblVendeur = new System.Windows.Forms.Label();
            this.tlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPanier = new System.Windows.Forms.TableLayoutPanel();
            this.flpRecherche = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtRecherche = new System.Windows.Forms.TextBox();
            this.btnAjouter = new System.Windows.Forms.Button();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
            this.tlpTotal = new System.Windows.Forms.TableLayoutPanel();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblMontantTotal = new System.Windows.Forms.Label();
            this.panelDroit = new System.Windows.Forms.Panel();
            this.tlpDroite = new System.Windows.Forms.TableLayoutPanel();
            this.lblClient = new System.Windows.Forms.LinkLabel();
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
            this.lblMatricule = new System.Windows.Forms.Label();
            this.txtMatricule = new System.Windows.Forms.TextBox();
            this.pnlMutuelle = new System.Windows.Forms.TableLayoutPanel();
            this.lblMutuelle = new System.Windows.Forms.Label();
            this.cbMutuelle = new System.Windows.Forms.ComboBox();
            this.lblTaux = new System.Windows.Forms.Label();
            this.txtTauxMutuelle = new System.Windows.Forms.TextBox();
            this.lblPartMutuelle = new System.Windows.Forms.Label();
            this.lblRestePatient = new System.Windows.Forms.Label();
            this.pnlEspeces = new System.Windows.Forms.TableLayoutPanel();
            this.lblEspeces = new System.Windows.Forms.Label();
            this.numEspeces = new System.Windows.Forms.NumericUpDown();
            this.lblMontantRendu = new System.Windows.Forms.Label();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixUnitaire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSousTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEspeces)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpEntete.SuspendLayout();
            this.tlpPrincipal.SuspendLayout();
            this.tlpPanier.SuspendLayout();
            this.flpRecherche.SuspendLayout();
            this.tlpTotal.SuspendLayout();
            this.panelDroit.SuspendLayout();
            this.tlpDroite.SuspendLayout();
            this.tlpClient.SuspendLayout();
            this.tlpMode.SuspendLayout();
            this.pnlMatricule.SuspendLayout();
            this.pnlMutuelle.SuspendLayout();
            this.pnlEspeces.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 4;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.tlpEntete, 0, 0);
            this.tlpEntete.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpPrincipal, 0, 1);
            this.tlpPrincipal.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 3);
            this.flpBoutons.TabIndex = 2;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // tlpEntete
            // 
            this.tlpEntete.ColumnCount = 2;
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowCount = 1;
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpEntete.Controls.Add(this.lblVendeur, 1, 0);
            this.lblVendeur.TabIndex = 1;
            this.tlpEntete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpEntete.AutoSize = true;
            this.tlpEntete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEntete.Name = "tlpEntete";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Nouvelle vente";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblVendeur
            // 
            this.lblVendeur.Text = "";
            this.lblVendeur.AutoSize = true;
            this.lblVendeur.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVendeur.Tag = "note";
            this.lblVendeur.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblVendeur.Name = "lblVendeur";
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
            this.tlpPanier.RowCount = 3;
            this.tlpPanier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPanier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPanier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPanier.Controls.Add(this.flpRecherche, 0, 0);
            this.flpRecherche.TabIndex = 0;
            this.tlpPanier.Controls.Add(this.dgvProduits, 0, 1);
            this.dgvProduits.TabIndex = 1;
            this.tlpPanier.Controls.Add(this.tlpTotal, 0, 2);
            this.tlpTotal.TabIndex = 2;
            this.tlpPanier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPanier.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tlpPanier.Name = "tlpPanier";
            // 
            // flpRecherche
            // 
            this.flpRecherche.Controls.Add(this.lblRecherche);
            this.lblRecherche.TabIndex = 0;
            this.flpRecherche.Controls.Add(this.txtRecherche);
            this.txtRecherche.TabIndex = 1;
            this.flpRecherche.Controls.Add(this.btnAjouter);
            this.btnAjouter.TabIndex = 2;
            this.flpRecherche.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRecherche.AutoSize = true;
            this.flpRecherche.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpRecherche.WrapContents = false;
            this.flpRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpRecherche.Name = "flpRecherche";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Produit";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtRecherche
            // 
            this.txtRecherche.Width = 300;
            this.txtRecherche.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.txtRecherche.PlaceholderText = "Tapez le nom puis Entrée";
            this.txtRecherche.Name = "txtRecherche";
            this.txtRecherche.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRecherche_KeyDown);
            // 
            // btnAjouter
            // 
            this.btnAjouter.Text = "Ajouter le produit (F3)";
            this.btnAjouter.Tag = "primaire";
            this.btnAjouter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);
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
            this.dgvProduits.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProduits_KeyDown);
            // 
            // tlpTotal
            // 
            this.tlpTotal.ColumnCount = 3;
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.RowCount = 1;
            this.tlpTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotal.Controls.Add(this.btnSupprimer, 0, 0);
            this.btnSupprimer.TabIndex = 0;
            this.tlpTotal.Controls.Add(this.lblTotal, 1, 0);
            this.lblTotal.TabIndex = 1;
            this.tlpTotal.Controls.Add(this.lblMontantTotal, 2, 0);
            this.lblMontantTotal.TabIndex = 2;
            this.tlpTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTotal.AutoSize = true;
            this.tlpTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTotal.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpTotal.Name = "tlpTotal";
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Text = "Retirer la ligne (Suppr)";
            this.btnSupprimer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Text = "Total à payer";
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
            this.tlpDroite.RowCount = 8;
            this.tlpDroite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
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
            this.tlpDroite.Controls.Add(this.pnlEspeces, 0, 6);
            this.pnlEspeces.TabIndex = 6;
            this.tlpDroite.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDroite.AutoSize = true;
            this.tlpDroite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpDroite.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.tlpDroite.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpDroite.Name = "tlpDroite";
            // 
            // lblClient
            // 
            this.lblClient.Text = "Client (facultatif)  +";
            this.lblClient.AutoSize = true;
            this.lblClient.Tag = "entete";
            this.lblClient.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblClient.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblClient_LinkClicked);
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
            this.txtNom.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.txtPrenom.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.txtTelephone.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.txtMotif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMotif.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtMotif.PlaceholderText = "Ex : hypertension, grippe, ordonnance du Dr Ahmed";
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
            this.pnlMatricule.Controls.Add(this.lblMatricule, 0, 0);
            this.lblMatricule.TabIndex = 0;
            this.pnlMatricule.Controls.Add(this.txtMatricule, 1, 0);
            this.txtMatricule.TabIndex = 1;
            this.pnlMatricule.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMatricule.AutoSize = true;
            this.pnlMatricule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMatricule.Visible = false;
            this.pnlMatricule.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlMatricule.Name = "pnlMatricule";
            // 
            // lblMatricule
            // 
            this.lblMatricule.Text = "Matricule de l'employé";
            this.lblMatricule.AutoSize = true;
            this.lblMatricule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMatricule.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMatricule.Name = "lblMatricule";
            // 
            // txtMatricule
            // 
            this.txtMatricule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMatricule.Width = 400;
            this.txtMatricule.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtMatricule.PlaceholderText = "N° matricule";
            this.txtMatricule.Name = "txtMatricule";
            // 
            // pnlMutuelle
            // 
            this.pnlMutuelle.ColumnCount = 2;
            this.pnlMutuelle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlMutuelle.RowCount = 4;
            this.pnlMutuelle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlMutuelle.Controls.Add(this.lblMutuelle, 0, 0);
            this.lblMutuelle.TabIndex = 0;
            this.pnlMutuelle.Controls.Add(this.cbMutuelle, 1, 0);
            this.cbMutuelle.TabIndex = 1;
            this.pnlMutuelle.Controls.Add(this.lblTaux, 0, 1);
            this.lblTaux.TabIndex = 2;
            this.pnlMutuelle.Controls.Add(this.txtTauxMutuelle, 1, 1);
            this.txtTauxMutuelle.TabIndex = 3;
            this.pnlMutuelle.Controls.Add(this.lblPartMutuelle, 0, 2);
            this.lblPartMutuelle.TabIndex = 4;
            this.pnlMutuelle.SetColumnSpan(this.lblPartMutuelle, 2);
            this.pnlMutuelle.Controls.Add(this.lblRestePatient, 0, 3);
            this.lblRestePatient.TabIndex = 5;
            this.pnlMutuelle.SetColumnSpan(this.lblRestePatient, 2);
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
            this.cbMutuelle.Width = 240;
            this.cbMutuelle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMutuelle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbMutuelle.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbMutuelle.Name = "cbMutuelle";
            this.cbMutuelle.SelectedIndexChanged += new System.EventHandler(this.cbMutuelle_SelectedIndexChanged);
            // 
            // lblTaux
            // 
            this.lblTaux.Text = "Taux de prise en charge (%)";
            this.lblTaux.AutoSize = true;
            this.lblTaux.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTaux.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblTaux.Name = "lblTaux";
            // 
            // txtTauxMutuelle
            // 
            this.txtTauxMutuelle.Width = 80;
            this.txtTauxMutuelle.ReadOnly = true;
            this.txtTauxMutuelle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTauxMutuelle.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtTauxMutuelle.Name = "txtTauxMutuelle";
            // 
            // lblPartMutuelle
            // 
            this.lblPartMutuelle.Text = "Pris en charge par la mutuelle : 0 KMF";
            this.lblPartMutuelle.AutoSize = true;
            this.lblPartMutuelle.Tag = "info";
            this.lblPartMutuelle.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblPartMutuelle.Name = "lblPartMutuelle";
            // 
            // lblRestePatient
            // 
            this.lblRestePatient.Text = "À payer par le patient : 0 KMF";
            this.lblRestePatient.AutoSize = true;
            this.lblRestePatient.Tag = "attention";
            this.lblRestePatient.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblRestePatient.Name = "lblRestePatient";
            // 
            // pnlEspeces
            // 
            this.pnlEspeces.ColumnCount = 2;
            this.pnlEspeces.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlEspeces.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlEspeces.RowCount = 2;
            this.pnlEspeces.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlEspeces.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlEspeces.Controls.Add(this.lblEspeces, 0, 0);
            this.lblEspeces.TabIndex = 0;
            this.pnlEspeces.Controls.Add(this.numEspeces, 1, 0);
            this.numEspeces.TabIndex = 1;
            this.pnlEspeces.Controls.Add(this.lblMontantRendu, 0, 1);
            this.lblMontantRendu.TabIndex = 2;
            this.pnlEspeces.SetColumnSpan(this.lblMontantRendu, 2);
            this.pnlEspeces.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEspeces.AutoSize = true;
            this.pnlEspeces.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlEspeces.Visible = false;
            this.pnlEspeces.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pnlEspeces.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlEspeces.Tag = "carte";
            this.pnlEspeces.Name = "pnlEspeces";
            // 
            // lblEspeces
            // 
            this.lblEspeces.Text = "Espèces reçues";
            this.lblEspeces.AutoSize = true;
            this.lblEspeces.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEspeces.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblEspeces.Name = "lblEspeces";
            // 
            // numEspeces
            // 
            this.numEspeces.Width = 170;
            this.numEspeces.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numEspeces.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numEspeces.ThousandsSeparator = true;
            this.numEspeces.DecimalPlaces = 0;
            this.numEspeces.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numEspeces.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numEspeces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEspeces.Name = "numEspeces";
            this.numEspeces.ValueChanged += new System.EventHandler(this.numEspeces_ValueChanged);
            // 
            // lblMontantRendu
            // 
            this.lblMontantRendu.Text = "";
            this.lblMontantRendu.AutoSize = true;
            this.lblMontantRendu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontantRendu.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblMontantRendu.Name = "lblMontantRendu";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnValider);
            this.btnValider.TabIndex = 0;
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
            // btnValider
            // 
            this.btnValider.Text = "Valider la vente (F9)";
            this.btnValider.Tag = "primaire";
            this.btnValider.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnValider.Name = "btnValider";
            this.btnValider.Click += new System.EventHandler(this.btnValider_Click);
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
            // FormVente
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1180, 660);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.KeyPreview = true;
            this.Text = "Nouvelle vente";
            this.AcceptButton = this.btnValider;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormVente";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVente_KeyDown);
            this.Shown += new System.EventHandler(this.FormVente_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.pnlEspeces.ResumeLayout(false);
            this.pnlEspeces.PerformLayout();
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
            this.flpRecherche.ResumeLayout(false);
            this.flpRecherche.PerformLayout();
            this.tlpPanier.ResumeLayout(false);
            this.tlpPanier.PerformLayout();
            this.tlpPrincipal.ResumeLayout(false);
            this.tlpPrincipal.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEspeces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblVendeur;
        private System.Windows.Forms.TableLayoutPanel tlpPrincipal;
        private System.Windows.Forms.TableLayoutPanel tlpPanier;
        private System.Windows.Forms.FlowLayoutPanel flpRecherche;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtRecherche;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.TableLayoutPanel tlpTotal;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblMontantTotal;
        private System.Windows.Forms.Panel panelDroit;
        private System.Windows.Forms.TableLayoutPanel tlpDroite;
        private System.Windows.Forms.LinkLabel lblClient;
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
        private System.Windows.Forms.Label lblMatricule;
        private System.Windows.Forms.TextBox txtMatricule;
        private System.Windows.Forms.TableLayoutPanel pnlMutuelle;
        private System.Windows.Forms.Label lblMutuelle;
        private System.Windows.Forms.ComboBox cbMutuelle;
        private System.Windows.Forms.Label lblTaux;
        private System.Windows.Forms.TextBox txtTauxMutuelle;
        private System.Windows.Forms.Label lblPartMutuelle;
        private System.Windows.Forms.Label lblRestePatient;
        private System.Windows.Forms.TableLayoutPanel pnlEspeces;
        private System.Windows.Forms.Label lblEspeces;
        private System.Windows.Forms.NumericUpDown numEspeces;
        private System.Windows.Forms.Label lblMontantRendu;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixUnitaire;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSousTotal;
    }
}
