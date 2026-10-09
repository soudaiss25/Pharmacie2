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
            this.tlpEntete = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.tlpEnteteActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouveauProduit = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.tlpFiltres = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtRecherche = new System.Windows.Forms.TextBox();
            this.lblTypeFiltre = new System.Windows.Forms.Label();
            this.cmbTypeFiltre = new System.Windows.Forms.ComboBox();
            this.btnEffacer = new System.Windows.Forms.Button();
            this.lblCompteur = new System.Windows.Forms.Label();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
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
            this.tlpEntete.SuspendLayout();
            this.tlpEnteteActions.SuspendLayout();
            this.tlpFiltres.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.Controls.Add(this.tlpEntete, 0, 0);
            this.tlpEntete.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpFiltres, 0, 1);
            this.tlpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.dgvProduits, 0, 2);
            this.dgvProduits.TabIndex = 2;
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
            this.lblTitre.Text = "Catalogue produits";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpEnteteActions
            // 
            this.tlpEnteteActions.Controls.Add(this.btnNouveauProduit);
            this.btnNouveauProduit.TabIndex = 0;
            this.tlpEnteteActions.Controls.Add(this.btnModifier);
            this.btnModifier.TabIndex = 1;
            this.tlpEnteteActions.Controls.Add(this.btnSupprimer);
            this.btnSupprimer.TabIndex = 2;
            this.tlpEnteteActions.AutoSize = true;
            this.tlpEnteteActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEnteteActions.WrapContents = false;
            this.tlpEnteteActions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpEnteteActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpEnteteActions.Name = "tlpEnteteActions";
            // 
            // btnNouveauProduit
            // 
            this.btnNouveauProduit.Text = "Nouveau produit";
            this.btnNouveauProduit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouveauProduit.Tag = "primaire";
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
            // tlpFiltres
            // 
            this.tlpFiltres.ColumnCount = 7;
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpFiltres.RowCount = 1;
            this.tlpFiltres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.Controls.Add(this.lblRecherche, 0, 0);
            this.lblRecherche.TabIndex = 0;
            this.tlpFiltres.Controls.Add(this.txtRecherche, 1, 0);
            this.txtRecherche.TabIndex = 1;
            this.tlpFiltres.Controls.Add(this.lblTypeFiltre, 2, 0);
            this.lblTypeFiltre.TabIndex = 2;
            this.tlpFiltres.Controls.Add(this.cmbTypeFiltre, 3, 0);
            this.cmbTypeFiltre.TabIndex = 3;
            this.tlpFiltres.Controls.Add(this.btnEffacer, 4, 0);
            this.btnEffacer.TabIndex = 4;
            this.tlpFiltres.Controls.Add(this.lblCompteur, 5, 0);
            this.lblCompteur.TabIndex = 5;
            this.tlpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltres.AutoSize = true;
            this.tlpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpFiltres.Name = "tlpFiltres";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Rechercher";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtRecherche
            // 
            this.txtRecherche.Width = 280;
            this.txtRecherche.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.txtRecherche.PlaceholderText = "Nom, type ou fournisseur";
            this.txtRecherche.Name = "txtRecherche";
            // 
            // lblTypeFiltre
            // 
            this.lblTypeFiltre.Text = "Type";
            this.lblTypeFiltre.AutoSize = true;
            this.lblTypeFiltre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTypeFiltre.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblTypeFiltre.Name = "lblTypeFiltre";
            // 
            // cmbTypeFiltre
            // 
            this.cmbTypeFiltre.Width = 190;
            this.cmbTypeFiltre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeFiltre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbTypeFiltre.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
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
            this.btnEffacer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEffacer.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEffacer.Name = "btnEffacer";
            // 
            // lblCompteur
            // 
            this.lblCompteur.Text = "";
            this.lblCompteur.AutoSize = true;
            this.lblCompteur.Tag = "note";
            this.lblCompteur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCompteur.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCompteur.Name = "lblCompteur";
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
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1000, 560);
            this.MinimumSize = new System.Drawing.Size(840, 480);
            this.Name = "Uc_Produits";
            this.ResumeLayout(false);
            this.tlpFiltres.ResumeLayout(false);
            this.tlpFiltres.PerformLayout();
            this.tlpEnteteActions.ResumeLayout(false);
            this.tlpEnteteActions.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel tlpEnteteActions;
        private System.Windows.Forms.Button btnNouveauProduit;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.TableLayoutPanel tlpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtRecherche;
        private System.Windows.Forms.Label lblTypeFiltre;
        private System.Windows.Forms.ComboBox cmbTypeFiltre;
        private System.Windows.Forms.Button btnEffacer;
        private System.Windows.Forms.Label lblCompteur;
        private System.Windows.Forms.DataGridView dgvProduits;
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
