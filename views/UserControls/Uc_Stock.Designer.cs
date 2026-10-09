namespace Pharmacie2.views.UserControls
{
    partial class Uc_Stock
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
            this.btnAjouter = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.btnCommander = new System.Windows.Forms.Button();
            this.btnInventaire = new System.Windows.Forms.Button();
            this.tlpFiltres = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtSearchProduit = new System.Windows.Forms.TextBox();
            this.lblFiltre = new System.Windows.Forms.Label();
            this.cbSeuil = new System.Windows.Forms.ComboBox();
            this.lblDisponibilite = new System.Windows.Forms.Label();
            this.cbDisponibilite = new System.Windows.Forms.ComboBox();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFournisseur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeuil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpiration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVerification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
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
            this.tlpRoot.Controls.Add(this.dgvStock, 0, 2);
            this.dgvStock.TabIndex = 2;
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
            this.lblTitre.Text = "Stock";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpEnteteActions
            // 
            this.tlpEnteteActions.Controls.Add(this.btnAjouter);
            this.btnAjouter.TabIndex = 0;
            this.tlpEnteteActions.Controls.Add(this.btnModifier);
            this.btnModifier.TabIndex = 1;
            this.tlpEnteteActions.Controls.Add(this.btnSupprimer);
            this.btnSupprimer.TabIndex = 2;
            this.tlpEnteteActions.Controls.Add(this.btnCommander);
            this.btnCommander.TabIndex = 3;
            this.tlpEnteteActions.Controls.Add(this.btnInventaire);
            this.btnInventaire.TabIndex = 4;
            this.tlpEnteteActions.AutoSize = true;
            this.tlpEnteteActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEnteteActions.WrapContents = false;
            this.tlpEnteteActions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpEnteteActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpEnteteActions.Name = "tlpEnteteActions";
            // 
            // btnAjouter
            // 
            this.btnAjouter.Text = "Ajouter";
            this.btnAjouter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAjouter.Tag = "primaire";
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);
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
            // btnCommander
            // 
            this.btnCommander.Text = "Commander";
            this.btnCommander.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCommander.Name = "btnCommander";
            this.btnCommander.Click += new System.EventHandler(this.btnCommander_Click);
            // 
            // btnInventaire
            // 
            this.btnInventaire.Text = "Inventaire";
            this.btnInventaire.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnInventaire.Name = "btnInventaire";
            this.btnInventaire.Click += new System.EventHandler(this.btnInventaire_Click);
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
            this.tlpFiltres.Controls.Add(this.txtSearchProduit, 1, 0);
            this.txtSearchProduit.TabIndex = 1;
            this.tlpFiltres.Controls.Add(this.lblFiltre, 2, 0);
            this.lblFiltre.TabIndex = 2;
            this.tlpFiltres.Controls.Add(this.cbSeuil, 3, 0);
            this.cbSeuil.TabIndex = 3;
            this.tlpFiltres.Controls.Add(this.lblDisponibilite, 4, 0);
            this.lblDisponibilite.TabIndex = 4;
            this.tlpFiltres.Controls.Add(this.cbDisponibilite, 5, 0);
            this.cbDisponibilite.TabIndex = 5;
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
            // txtSearchProduit
            // 
            this.txtSearchProduit.Width = 280;
            this.txtSearchProduit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSearchProduit.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.txtSearchProduit.PlaceholderText = "Nom du produit";
            this.txtSearchProduit.Name = "txtSearchProduit";
            this.txtSearchProduit.TextChanged += new System.EventHandler(this.txtSearchProduit_TextChanged);
            // 
            // lblFiltre
            // 
            this.lblFiltre.Text = "Afficher";
            this.lblFiltre.AutoSize = true;
            this.lblFiltre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltre.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblFiltre.Name = "lblFiltre";
            // 
            // cbSeuil
            // 
            this.cbSeuil.Width = 190;
            this.cbSeuil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeuil.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbSeuil.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.cbSeuil.Items.AddRange(new object[] {
            "Tous",
            "Sous le seuil",
            "Normal",
            "À vérifier",
            "Périmés",
            "Péremption proche"});
            this.cbSeuil.Name = "cbSeuil";
            this.cbSeuil.SelectedIndexChanged += new System.EventHandler(this.Filtre_Changed);
            // 
            // lblDisponibilite
            // 
            this.lblDisponibilite.Text = "Disponibilité";
            this.lblDisponibilite.AutoSize = true;
            this.lblDisponibilite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDisponibilite.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblDisponibilite.Name = "lblDisponibilite";
            // 
            // cbDisponibilite
            // 
            this.cbDisponibilite.Width = 150;
            this.cbDisponibilite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisponibilite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbDisponibilite.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.cbDisponibilite.Items.AddRange(new object[] {
            "Tous",
            "En rupture",
            "Disponible"});
            this.cbDisponibilite.Name = "cbDisponibilite";
            this.cbDisponibilite.SelectedIndexChanged += new System.EventHandler(this.Filtre_Changed);
            // 
            // dgvStock
            // 
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colProduit,
            this.colType,
            this.colFournisseur,
            this.colQuantite,
            this.colSeuil,
            this.colExpiration,
            this.colVerification,
            this.colEtat});
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.AutoGenerateColumns = false;
            this.dgvStock.ReadOnly = true;
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvStock_CellFormatting);
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
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "Produit";
            this.colProduit.DataPropertyName = "Produit";
            this.colProduit.FillWeight = 34F;
            this.colProduit.MinimumWidth = 140;
            this.colProduit.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "Type";
            this.colType.DataPropertyName = "Type";
            this.colType.FillWeight = 12F;
            this.colType.MinimumWidth = 80;
            this.colType.ReadOnly = true;
            // 
            // colFournisseur
            // 
            this.colFournisseur.HeaderText = "Fournisseur";
            this.colFournisseur.Name = "Fournisseur";
            this.colFournisseur.DataPropertyName = "Fournisseur";
            this.colFournisseur.FillWeight = 18F;
            this.colFournisseur.MinimumWidth = 100;
            this.colFournisseur.ReadOnly = true;
            // 
            // colQuantite
            // 
            this.colQuantite.HeaderText = "En stock";
            this.colQuantite.Name = "Quantite";
            this.colQuantite.DataPropertyName = "Quantite";
            this.colQuantite.FillWeight = 24F;
            this.colQuantite.MinimumWidth = 130;
            this.colQuantite.ReadOnly = true;
            // 
            // colSeuil
            // 
            this.colSeuil.HeaderText = "Seuil d'alerte";
            this.colSeuil.Name = "Seuil";
            this.colSeuil.DataPropertyName = "Seuil";
            this.colSeuil.FillWeight = 10F;
            this.colSeuil.MinimumWidth = 90;
            this.colSeuil.ReadOnly = true;
            // 
            // colExpiration
            // 
            this.colExpiration.HeaderText = "Expiration";
            this.colExpiration.Name = "Expiration";
            this.colExpiration.DataPropertyName = "Expiration";
            this.colExpiration.FillWeight = 11F;
            this.colExpiration.MinimumWidth = 90;
            this.colExpiration.ReadOnly = true;
            // 
            // colVerification
            // 
            this.colVerification.HeaderText = "Vérification";
            this.colVerification.Name = "Verification";
            this.colVerification.DataPropertyName = "Verification";
            this.colVerification.FillWeight = 11F;
            this.colVerification.MinimumWidth = 90;
            this.colVerification.ReadOnly = true;
            // 
            // colEtat
            // 
            this.colEtat.HeaderText = "État";
            this.colEtat.Name = "Etat";
            this.colEtat.DataPropertyName = "Etat";
            this.colEtat.FillWeight = 9F;
            this.colEtat.MinimumWidth = 70;
            this.colEtat.ReadOnly = true;
            // 
            // Uc_Stock
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1000, 520);
            this.MinimumSize = new System.Drawing.Size(900, 480);
            this.Name = "Uc_Stock";
            this.ResumeLayout(false);
            this.tlpFiltres.ResumeLayout(false);
            this.tlpFiltres.PerformLayout();
            this.tlpEnteteActions.ResumeLayout(false);
            this.tlpEnteteActions.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel tlpEnteteActions;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Button btnCommander;
        private System.Windows.Forms.Button btnInventaire;
        private System.Windows.Forms.TableLayoutPanel tlpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtSearchProduit;
        private System.Windows.Forms.Label lblFiltre;
        private System.Windows.Forms.ComboBox cbSeuil;
        private System.Windows.Forms.Label lblDisponibilite;
        private System.Windows.Forms.ComboBox cbDisponibilite;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFournisseur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeuil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpiration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVerification;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtat;
    }
}
