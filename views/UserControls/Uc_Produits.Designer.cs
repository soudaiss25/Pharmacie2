namespace Pharmacie2.views.UserControls
{
    partial class Uc_Produits
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
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtRecherche = new System.Windows.Forms.TextBox();
            this.lblTypeFiltre = new System.Windows.Forms.Label();
            this.cmbTypeFiltre = new System.Windows.Forms.ComboBox();
            this.btnEffacer = new System.Windows.Forms.Button();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouveauProduit = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.lblCompteur = new System.Windows.Forms.Label();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixAchat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeuil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUniteVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpiration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFournisseur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpFiltres.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 4;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpFiltres, 0, 1);
            this.flpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.dgvProduits, 0, 2);
            this.dgvProduits.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.pnlActions, 0, 3);
            this.pnlActions.TabIndex = 3;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Catalogue produits";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpFiltres
            // 
            this.flpFiltres.Controls.Add(this.lblRecherche);
            this.lblRecherche.TabIndex = 0;
            this.flpFiltres.Controls.Add(this.txtRecherche);
            this.txtRecherche.TabIndex = 1;
            this.flpFiltres.Controls.Add(this.lblTypeFiltre);
            this.lblTypeFiltre.TabIndex = 2;
            this.flpFiltres.Controls.Add(this.cmbTypeFiltre);
            this.cmbTypeFiltre.TabIndex = 3;
            this.flpFiltres.Controls.Add(this.btnEffacer);
            this.btnEffacer.TabIndex = 4;
            this.flpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFiltres.AutoSize = true;
            this.flpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFiltres.WrapContents = true;
            this.flpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpFiltres.Name = "flpFiltres";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Rechercher";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtRecherche
            // 
            this.txtRecherche.Width = 280;
            this.txtRecherche.MaximumSize = new System.Drawing.Size(400, 0);
            this.txtRecherche.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.txtRecherche.PlaceholderText = "Nom, type ou fournisseur";
            this.txtRecherche.Name = "txtRecherche";
            // 
            // lblTypeFiltre
            // 
            this.lblTypeFiltre.Text = "Type";
            this.lblTypeFiltre.AutoSize = true;
            this.lblTypeFiltre.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblTypeFiltre.Name = "lblTypeFiltre";
            // 
            // cmbTypeFiltre
            // 
            this.cmbTypeFiltre.Width = 190;
            this.cmbTypeFiltre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeFiltre.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.cmbTypeFiltre.Items.AddRange(new object[] {
            "Tous",
            "Médicament",
            "Matériel",
            "Produit d'hygiène",
            "Autre"});
            this.cmbTypeFiltre.Name = "cmbTypeFiltre";
            // 
            // btnEffacer
            // 
            this.btnEffacer.Text = "Effacer les filtres";
            this.btnEffacer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnEffacer.Name = "btnEffacer";
            // 
            // dgvProduits
            // 
            this.dgvProduits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNom,
            this.colType,
            this.colPrixAchat,
            this.colPrixVente,
            this.colMarge,
            this.colStock,
            this.colSeuil,
            this.colUniteVente,
            this.colExpiration,
            this.colFournisseur,
            this.colEtat});
            this.dgvProduits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduits.AutoGenerateColumns = false;
            this.dgvProduits.ReadOnly = true;
            this.dgvProduits.Name = "dgvProduits";
            this.dgvProduits.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProduits_CellFormatting);
            this.dgvProduits.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduits_CellDoubleClick);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnNouveauProduit);
            this.btnNouveauProduit.TabIndex = 0;
            this.pnlActions.Controls.Add(this.btnModifier);
            this.btnModifier.TabIndex = 1;
            this.pnlActions.Controls.Add(this.btnSupprimer);
            this.btnSupprimer.TabIndex = 2;
            this.pnlActions.Controls.Add(this.lblCompteur);
            this.lblCompteur.TabIndex = 3;
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.AutoSize = true;
            this.pnlActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlActions.WrapContents = true;
            this.pnlActions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlActions.Name = "pnlActions";
            // 
            // btnNouveauProduit
            // 
            this.btnNouveauProduit.Text = "Nouveau produit";
            this.btnNouveauProduit.Tag = "primaire";
            this.btnNouveauProduit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouveauProduit.Name = "btnNouveauProduit";
            this.btnNouveauProduit.Click += new System.EventHandler(this.btnNouveauProduit_Click);
            // 
            // btnModifier
            // 
            this.btnModifier.Text = "Modifier";
            this.btnModifier.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnModifier.Name = "btnModifier";
            this.btnModifier.Click += new System.EventHandler(this.btnModifier_Click);
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Text = "Archiver";
            this.btnSupprimer.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // lblCompteur
            // 
            this.lblCompteur.Text = "";
            this.lblCompteur.AutoSize = true;
            this.lblCompteur.Tag = "note";
            this.lblCompteur.Margin = new System.Windows.Forms.Padding(12, 8, 0, 0);
            this.lblCompteur.Name = "lblCompteur";
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "Id";
            this.colId.DataPropertyName = "Id";
            this.colId.FillWeight = 100F;
            this.colId.MinimumWidth = 60;
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colNom
            // 
            this.colNom.HeaderText = "Produit";
            this.colNom.Name = "Nom";
            this.colNom.DataPropertyName = "Nom";
            this.colNom.FillWeight = 20F;
            this.colNom.MinimumWidth = 140;
            this.colNom.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "Type";
            this.colType.DataPropertyName = "Type";
            this.colType.FillWeight = 9F;
            this.colType.MinimumWidth = 80;
            this.colType.ReadOnly = true;
            // 
            // colPrixAchat
            // 
            this.colPrixAchat.HeaderText = "Prix d'achat";
            this.colPrixAchat.Name = "PrixAchat";
            this.colPrixAchat.DataPropertyName = "PrixAchat";
            this.colPrixAchat.FillWeight = 9F;
            this.colPrixAchat.MinimumWidth = 90;
            this.colPrixAchat.ReadOnly = true;
            this.colPrixAchat.Tag = "montant";
            // 
            // colPrixVente
            // 
            this.colPrixVente.HeaderText = "Prix de vente";
            this.colPrixVente.Name = "PrixVente";
            this.colPrixVente.DataPropertyName = "PrixVente";
            this.colPrixVente.FillWeight = 9F;
            this.colPrixVente.MinimumWidth = 90;
            this.colPrixVente.ReadOnly = true;
            this.colPrixVente.Tag = "montant";
            // 
            // colMarge
            // 
            this.colMarge.HeaderText = "Marge";
            this.colMarge.Name = "Marge";
            this.colMarge.DataPropertyName = "Marge";
            this.colMarge.FillWeight = 6F;
            this.colMarge.MinimumWidth = 60;
            this.colMarge.ReadOnly = true;
            // 
            // colStock
            // 
            this.colStock.HeaderText = "En stock";
            this.colStock.Name = "Stock";
            this.colStock.DataPropertyName = "Stock";
            this.colStock.FillWeight = 14F;
            this.colStock.MinimumWidth = 120;
            this.colStock.ReadOnly = true;
            // 
            // colSeuil
            // 
            this.colSeuil.HeaderText = "Seuil";
            this.colSeuil.Name = "Seuil";
            this.colSeuil.DataPropertyName = "Seuil";
            this.colSeuil.FillWeight = 7F;
            this.colSeuil.MinimumWidth = 70;
            this.colSeuil.ReadOnly = true;
            // 
            // colUniteVente
            // 
            this.colUniteVente.HeaderText = "Unité";
            this.colUniteVente.Name = "UniteVente";
            this.colUniteVente.DataPropertyName = "UniteVente";
            this.colUniteVente.FillWeight = 7F;
            this.colUniteVente.MinimumWidth = 70;
            this.colUniteVente.ReadOnly = true;
            // 
            // colExpiration
            // 
            this.colExpiration.HeaderText = "Expiration";
            this.colExpiration.Name = "Expiration";
            this.colExpiration.DataPropertyName = "Expiration";
            this.colExpiration.FillWeight = 8F;
            this.colExpiration.MinimumWidth = 85;
            this.colExpiration.ReadOnly = true;
            // 
            // colFournisseur
            // 
            this.colFournisseur.HeaderText = "Fournisseur";
            this.colFournisseur.Name = "Fournisseur";
            this.colFournisseur.DataPropertyName = "Fournisseur";
            this.colFournisseur.FillWeight = 11F;
            this.colFournisseur.MinimumWidth = 90;
            this.colFournisseur.ReadOnly = true;
            // 
            // colEtat
            // 
            this.colEtat.HeaderText = "État";
            this.colEtat.Name = "Etat";
            this.colEtat.DataPropertyName = "Etat";
            this.colEtat.FillWeight = 7F;
            this.colEtat.MinimumWidth = 70;
            this.colEtat.ReadOnly = true;
            // 
            // Uc_Produits
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Produits";
            this.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.flpFiltres.ResumeLayout(false);
            this.flpFiltres.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtRecherche;
        private System.Windows.Forms.Label lblTypeFiltre;
        private System.Windows.Forms.ComboBox cmbTypeFiltre;
        private System.Windows.Forms.Button btnEffacer;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnNouveauProduit;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Label lblCompteur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixAchat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeuil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUniteVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpiration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFournisseur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtat;
    }
}
