namespace Pharmacie2.views.UserControls
{
    partial class Uc_Commande
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
            this.tlpEnteteActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvelleCommande = new System.Windows.Forms.Button();
            this.btnMarquerRecu = new System.Windows.Forms.Button();
            this.btnMarquerPartiel = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.tlpFiltres = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatut = new System.Windows.Forms.Label();
            this.cbFiltreStatut = new System.Windows.Forms.ComboBox();
            this.lblFournisseur = new System.Windows.Forms.Label();
            this.cbFiltreFournisseur = new System.Windows.Forms.ComboBox();
            this.chkPeriode = new System.Windows.Forms.CheckBox();
            this.dtpDebut = new System.Windows.Forms.DateTimePicker();
            this.lblAu = new System.Windows.Forms.Label();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.lblRecap = new System.Windows.Forms.Label();
            this.dgvCommandes = new System.Windows.Forms.DataGridView();
            this.lblLignesTitre = new System.Windows.Forms.Label();
            this.dgvLignes = new System.Windows.Forms.DataGridView();
            this.colCmdId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFourn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNbProd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReception = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLQte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLPrixU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpEntete.SuspendLayout();
            this.tlpEnteteActions.SuspendLayout();
            this.tlpFiltres.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 6;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0F));
            this.tlpRoot.Controls.Add(this.tlpEntete, 0, 0);
            this.tlpEntete.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpFiltres, 0, 1);
            this.tlpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblRecap, 0, 2);
            this.lblRecap.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.dgvCommandes, 0, 3);
            this.dgvCommandes.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.lblLignesTitre, 0, 4);
            this.lblLignesTitre.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 5);
            this.dgvLignes.TabIndex = 5;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // tlpEntete
            // 
            this.tlpEntete.ColumnCount = 3;
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowCount = 1;
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpEntete.Controls.Add(this.tlpEnteteActions, 2, 0);
            this.tlpEnteteActions.TabIndex = 1;
            this.tlpEntete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntete.AutoSize = true;
            this.tlpEntete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEntete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpEntete.Name = "tlpEntete";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Commandes aux fournisseurs";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpEnteteActions
            // 
            this.tlpEnteteActions.Controls.Add(this.btnNouvelleCommande);
            this.btnNouvelleCommande.TabIndex = 0;
            this.tlpEnteteActions.Controls.Add(this.btnMarquerRecu);
            this.btnMarquerRecu.TabIndex = 1;
            this.tlpEnteteActions.Controls.Add(this.btnMarquerPartiel);
            this.btnMarquerPartiel.TabIndex = 2;
            this.tlpEnteteActions.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 3;
            this.tlpEnteteActions.AutoSize = true;
            this.tlpEnteteActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEnteteActions.WrapContents = false;
            this.tlpEnteteActions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpEnteteActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpEnteteActions.Name = "tlpEnteteActions";
            // 
            // btnNouvelleCommande
            // 
            this.btnNouvelleCommande.Text = "Nouvelle commande";
            this.btnNouvelleCommande.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouvelleCommande.Tag = "primaire";
            this.btnNouvelleCommande.Name = "btnNouvelleCommande";
            this.btnNouvelleCommande.Click += new System.EventHandler(this.btnNouvelleCommande_Click);
            // 
            // btnMarquerRecu
            // 
            this.btnMarquerRecu.Text = "Tout recevoir";
            this.btnMarquerRecu.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnMarquerRecu.Enabled = false;
            this.btnMarquerRecu.Name = "btnMarquerRecu";
            this.btnMarquerRecu.Click += new System.EventHandler(this.btnMarquerRecu_Click);
            // 
            // btnMarquerPartiel
            // 
            this.btnMarquerPartiel.Text = "Réception partielle…";
            this.btnMarquerPartiel.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnMarquerPartiel.Enabled = false;
            this.btnMarquerPartiel.Name = "btnMarquerPartiel";
            this.btnMarquerPartiel.Click += new System.EventHandler(this.btnMarquerPartiel_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler la commande";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAnnuler.Tag = "danger";
            this.btnAnnuler.Enabled = false;
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // tlpFiltres
            // 
            this.tlpFiltres.ColumnCount = 10;
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpFiltres.RowCount = 1;
            this.tlpFiltres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.Controls.Add(this.lblStatut, 0, 0);
            this.lblStatut.TabIndex = 0;
            this.tlpFiltres.Controls.Add(this.cbFiltreStatut, 1, 0);
            this.cbFiltreStatut.TabIndex = 1;
            this.tlpFiltres.Controls.Add(this.lblFournisseur, 2, 0);
            this.lblFournisseur.TabIndex = 2;
            this.tlpFiltres.Controls.Add(this.cbFiltreFournisseur, 3, 0);
            this.cbFiltreFournisseur.TabIndex = 3;
            this.tlpFiltres.Controls.Add(this.chkPeriode, 4, 0);
            this.chkPeriode.TabIndex = 4;
            this.tlpFiltres.Controls.Add(this.dtpDebut, 5, 0);
            this.dtpDebut.TabIndex = 5;
            this.tlpFiltres.Controls.Add(this.lblAu, 6, 0);
            this.lblAu.TabIndex = 6;
            this.tlpFiltres.Controls.Add(this.dtpFin, 7, 0);
            this.dtpFin.TabIndex = 7;
            this.tlpFiltres.Controls.Add(this.btnActualiser, 8, 0);
            this.btnActualiser.TabIndex = 8;
            this.tlpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltres.AutoSize = true;
            this.tlpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpFiltres.Name = "tlpFiltres";
            // 
            // lblStatut
            // 
            this.lblStatut.Text = "Statut";
            this.lblStatut.AutoSize = true;
            this.lblStatut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatut.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblStatut.Name = "lblStatut";
            // 
            // cbFiltreStatut
            // 
            this.cbFiltreStatut.Width = 170;
            this.cbFiltreStatut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreStatut.Items.AddRange(new object[] {
            "Tous",
            "En attente",
            "Reçu partiellement",
            "Reçu",
            "Annulée"});
            this.cbFiltreStatut.SelectedIndex = 0;
            this.cbFiltreStatut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFiltreStatut.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cbFiltreStatut.Name = "cbFiltreStatut";
            // 
            // lblFournisseur
            // 
            this.lblFournisseur.Text = "Fournisseur";
            this.lblFournisseur.AutoSize = true;
            this.lblFournisseur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFournisseur.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblFournisseur.Name = "lblFournisseur";
            // 
            // cbFiltreFournisseur
            // 
            this.cbFiltreFournisseur.Width = 220;
            this.cbFiltreFournisseur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreFournisseur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFiltreFournisseur.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cbFiltreFournisseur.Name = "cbFiltreFournisseur";
            // 
            // chkPeriode
            // 
            this.chkPeriode.Text = "Période du";
            this.chkPeriode.AutoSize = true;
            this.chkPeriode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkPeriode.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.chkPeriode.Name = "chkPeriode";
            this.chkPeriode.CheckedChanged += new System.EventHandler(this.chkPeriode_CheckedChanged);
            // 
            // dtpDebut
            // 
            this.dtpDebut.Width = 120;
            this.dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDebut.Enabled = false;
            this.dtpDebut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpDebut.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.dtpDebut.Name = "dtpDebut";
            // 
            // lblAu
            // 
            this.lblAu.Text = "au";
            this.lblAu.AutoSize = true;
            this.lblAu.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAu.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblAu.Name = "lblAu";
            // 
            // dtpFin
            // 
            this.dtpFin.Width = 120;
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFin.Enabled = false;
            this.dtpFin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpFin.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.dtpFin.Name = "dtpFin";
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnActualiser.Name = "btnActualiser";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);
            // 
            // lblRecap
            // 
            this.lblRecap.Text = "";
            this.lblRecap.AutoSize = true;
            this.lblRecap.Tag = "note";
            this.lblRecap.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblRecap.Name = "lblRecap";
            // 
            // dgvCommandes
            // 
            this.dgvCommandes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCmdId,
            this.colDate,
            this.colFourn,
            this.colStatut,
            this.colNbProd,
            this.colMontant,
            this.colReception,
            this.colNote});
            this.dgvCommandes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCommandes.AutoGenerateColumns = false;
            this.dgvCommandes.ReadOnly = true;
            this.dgvCommandes.Name = "dgvCommandes";
            this.dgvCommandes.SelectionChanged += new System.EventHandler(this.DgvCommandes_SelectionChanged);
            // 
            // lblLignesTitre
            // 
            this.lblLignesTitre.Text = "Produits de la commande sélectionnée";
            this.lblLignesTitre.AutoSize = true;
            this.lblLignesTitre.Tag = "section";
            this.lblLignesTitre.Margin = new System.Windows.Forms.Padding(0, 8, 0, 6);
            this.lblLignesTitre.Name = "lblLignesTitre";
            // 
            // dgvLignes
            // 
            this.dgvLignes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLProduit,
            this.colLQte,
            this.colLPrixU,
            this.colLTotal,
            this.colLStock});
            this.dgvLignes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLignes.AutoGenerateColumns = false;
            this.dgvLignes.ReadOnly = true;
            this.dgvLignes.Name = "dgvLignes";
            // 
            // colCmdId
            // 
            this.colCmdId.HeaderText = "Id";
            this.colCmdId.Name = "colCmdId";
            this.colCmdId.DataPropertyName = "colCmdId";
            this.colCmdId.FillWeight = 100F;
            this.colCmdId.MinimumWidth = 60;
            this.colCmdId.ReadOnly = true;
            this.colCmdId.Visible = false;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.DataPropertyName = "colDate";
            this.colDate.FillWeight = 14F;
            this.colDate.MinimumWidth = 110;
            this.colDate.ReadOnly = true;
            // 
            // colFourn
            // 
            this.colFourn.HeaderText = "Fournisseur";
            this.colFourn.Name = "colFourn";
            this.colFourn.DataPropertyName = "colFourn";
            this.colFourn.FillWeight = 20F;
            this.colFourn.MinimumWidth = 120;
            this.colFourn.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Statut";
            this.colStatut.Name = "colStatut";
            this.colStatut.DataPropertyName = "colStatut";
            this.colStatut.FillWeight = 14F;
            this.colStatut.MinimumWidth = 110;
            this.colStatut.ReadOnly = true;
            // 
            // colNbProd
            // 
            this.colNbProd.HeaderText = "Produits";
            this.colNbProd.Name = "colNbProd";
            this.colNbProd.DataPropertyName = "colNbProd";
            this.colNbProd.FillWeight = 8F;
            this.colNbProd.MinimumWidth = 70;
            this.colNbProd.ReadOnly = true;
            // 
            // colMontant
            // 
            this.colMontant.HeaderText = "Montant";
            this.colMontant.Name = "colMontant";
            this.colMontant.DataPropertyName = "colMontant";
            this.colMontant.FillWeight = 12F;
            this.colMontant.MinimumWidth = 100;
            this.colMontant.ReadOnly = true;
            this.colMontant.Tag = "montant";
            // 
            // colReception
            // 
            this.colReception.HeaderText = "Reçue le";
            this.colReception.Name = "colReception";
            this.colReception.DataPropertyName = "colReception";
            this.colReception.FillWeight = 11F;
            this.colReception.MinimumWidth = 90;
            this.colReception.ReadOnly = true;
            // 
            // colNote
            // 
            this.colNote.HeaderText = "Note";
            this.colNote.Name = "colNote";
            this.colNote.DataPropertyName = "colNote";
            this.colNote.FillWeight = 21F;
            this.colNote.MinimumWidth = 100;
            this.colNote.ReadOnly = true;
            // 
            // colLProduit
            // 
            this.colLProduit.HeaderText = "Produit";
            this.colLProduit.Name = "colLProduit";
            this.colLProduit.DataPropertyName = "colLProduit";
            this.colLProduit.FillWeight = 34F;
            this.colLProduit.MinimumWidth = 140;
            this.colLProduit.ReadOnly = true;
            // 
            // colLQte
            // 
            this.colLQte.HeaderText = "Commandé (boîtes)";
            this.colLQte.Name = "colLQte";
            this.colLQte.DataPropertyName = "colLQte";
            this.colLQte.FillWeight = 16F;
            this.colLQte.MinimumWidth = 110;
            this.colLQte.ReadOnly = true;
            // 
            // colLPrixU
            // 
            this.colLPrixU.HeaderText = "Prix unitaire";
            this.colLPrixU.Name = "colLPrixU";
            this.colLPrixU.DataPropertyName = "colLPrixU";
            this.colLPrixU.FillWeight = 16F;
            this.colLPrixU.MinimumWidth = 100;
            this.colLPrixU.ReadOnly = true;
            this.colLPrixU.Tag = "montant";
            // 
            // colLTotal
            // 
            this.colLTotal.HeaderText = "Total ligne";
            this.colLTotal.Name = "colLTotal";
            this.colLTotal.DataPropertyName = "colLTotal";
            this.colLTotal.FillWeight = 16F;
            this.colLTotal.MinimumWidth = 100;
            this.colLTotal.ReadOnly = true;
            this.colLTotal.Tag = "montant";
            // 
            // colLStock
            // 
            this.colLStock.HeaderText = "Stock actuel";
            this.colLStock.Name = "colLStock";
            this.colLStock.DataPropertyName = "colLStock";
            this.colLStock.FillWeight = 18F;
            this.colLStock.MinimumWidth = 120;
            this.colLStock.ReadOnly = true;
            // 
            // Uc_Commande
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1000, 560);
            this.MinimumSize = new System.Drawing.Size(840, 520);
            this.Name = "Uc_Commande";
            this.ResumeLayout(false);
            this.tlpFiltres.ResumeLayout(false);
            this.tlpFiltres.PerformLayout();
            this.tlpEnteteActions.ResumeLayout(false);
            this.tlpEnteteActions.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel tlpEnteteActions;
        private System.Windows.Forms.Button btnNouvelleCommande;
        private System.Windows.Forms.Button btnMarquerRecu;
        private System.Windows.Forms.Button btnMarquerPartiel;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.TableLayoutPanel tlpFiltres;
        private System.Windows.Forms.Label lblStatut;
        private System.Windows.Forms.ComboBox cbFiltreStatut;
        private System.Windows.Forms.Label lblFournisseur;
        private System.Windows.Forms.ComboBox cbFiltreFournisseur;
        private System.Windows.Forms.CheckBox chkPeriode;
        private System.Windows.Forms.DateTimePicker dtpDebut;
        private System.Windows.Forms.Label lblAu;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Label lblRecap;
        private System.Windows.Forms.DataGridView dgvCommandes;
        private System.Windows.Forms.Label lblLignesTitre;
        private System.Windows.Forms.DataGridView dgvLignes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCmdId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFourn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNbProd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReception;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLQte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLPrixU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLStock;
    }
}
