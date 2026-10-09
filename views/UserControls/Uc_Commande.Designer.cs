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
            this.lblTitre = new System.Windows.Forms.Label();
            this.flpFiltres = new System.Windows.Forms.FlowLayoutPanel();
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
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvelleCommande = new System.Windows.Forms.Button();
            this.btnMarquerRecu = new System.Windows.Forms.Button();
            this.btnMarquerPartiel = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
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
            this.flpFiltres.SuspendLayout();
            this.flpActions.SuspendLayout();
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
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0F));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpFiltres, 0, 1);
            this.flpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblRecap, 0, 2);
            this.lblRecap.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.dgvCommandes, 0, 3);
            this.dgvCommandes.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpActions, 0, 4);
            this.flpActions.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.lblLignesTitre, 0, 5);
            this.lblLignesTitre.TabIndex = 5;
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 6);
            this.dgvLignes.TabIndex = 6;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Commandes aux fournisseurs";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpFiltres
            // 
            this.flpFiltres.Controls.Add(this.lblStatut);
            this.lblStatut.TabIndex = 0;
            this.flpFiltres.Controls.Add(this.cbFiltreStatut);
            this.cbFiltreStatut.TabIndex = 1;
            this.flpFiltres.Controls.Add(this.lblFournisseur);
            this.lblFournisseur.TabIndex = 2;
            this.flpFiltres.Controls.Add(this.cbFiltreFournisseur);
            this.cbFiltreFournisseur.TabIndex = 3;
            this.flpFiltres.Controls.Add(this.chkPeriode);
            this.chkPeriode.TabIndex = 4;
            this.flpFiltres.Controls.Add(this.dtpDebut);
            this.dtpDebut.TabIndex = 5;
            this.flpFiltres.Controls.Add(this.lblAu);
            this.lblAu.TabIndex = 6;
            this.flpFiltres.Controls.Add(this.dtpFin);
            this.dtpFin.TabIndex = 7;
            this.flpFiltres.Controls.Add(this.btnActualiser);
            this.btnActualiser.TabIndex = 8;
            this.flpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFiltres.AutoSize = true;
            this.flpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFiltres.WrapContents = true;
            this.flpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpFiltres.Name = "flpFiltres";
            // 
            // lblStatut
            // 
            this.lblStatut.Text = "Statut";
            this.lblStatut.AutoSize = true;
            this.lblStatut.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
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
            this.cbFiltreStatut.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.cbFiltreStatut.Name = "cbFiltreStatut";
            // 
            // lblFournisseur
            // 
            this.lblFournisseur.Text = "Fournisseur";
            this.lblFournisseur.AutoSize = true;
            this.lblFournisseur.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFournisseur.Name = "lblFournisseur";
            // 
            // cbFiltreFournisseur
            // 
            this.cbFiltreFournisseur.Width = 220;
            this.cbFiltreFournisseur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltreFournisseur.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.cbFiltreFournisseur.Name = "cbFiltreFournisseur";
            // 
            // chkPeriode
            // 
            this.chkPeriode.Text = "Période du";
            this.chkPeriode.AutoSize = true;
            this.chkPeriode.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            this.chkPeriode.Name = "chkPeriode";
            this.chkPeriode.CheckedChanged += new System.EventHandler(this.chkPeriode_CheckedChanged);
            // 
            // dtpDebut
            // 
            this.dtpDebut.Width = 120;
            this.dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDebut.Enabled = false;
            this.dtpDebut.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.dtpDebut.Name = "dtpDebut";
            // 
            // lblAu
            // 
            this.lblAu.Text = "au";
            this.lblAu.AutoSize = true;
            this.lblAu.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblAu.Name = "lblAu";
            // 
            // dtpFin
            // 
            this.dtpFin.Width = 120;
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFin.Enabled = false;
            this.dtpFin.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.dtpFin.Name = "dtpFin";
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiser.Name = "btnActualiser";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);
            // 
            // lblRecap
            // 
            this.lblRecap.Text = "";
            this.lblRecap.AutoSize = true;
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
            // flpActions
            // 
            this.flpActions.Controls.Add(this.btnNouvelleCommande);
            this.btnNouvelleCommande.TabIndex = 0;
            this.flpActions.Controls.Add(this.btnMarquerRecu);
            this.btnMarquerRecu.TabIndex = 1;
            this.flpActions.Controls.Add(this.btnMarquerPartiel);
            this.btnMarquerPartiel.TabIndex = 2;
            this.flpActions.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 3;
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.WrapContents = true;
            this.flpActions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.flpActions.Name = "flpActions";
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
            // lblLignesTitre
            // 
            this.lblLignesTitre.Text = "Produits de la commande sélectionnée";
            this.lblLignesTitre.AutoSize = true;
            this.lblLignesTitre.Tag = "section";
            this.lblLignesTitre.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Commande";
            this.ResumeLayout(false);
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.flpFiltres.ResumeLayout(false);
            this.flpFiltres.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpFiltres;
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
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnNouvelleCommande;
        private System.Windows.Forms.Button btnMarquerRecu;
        private System.Windows.Forms.Button btnMarquerPartiel;
        private System.Windows.Forms.Button btnAnnuler;
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
