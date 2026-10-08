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
            this.lblTitre = new System.Windows.Forms.Label();
            this.flpFiltres = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtSearchProduit = new System.Windows.Forms.TextBox();
            this.lblFiltre = new System.Windows.Forms.Label();
            this.cbSeuil = new System.Windows.Forms.ComboBox();
            this.lblDisponibilite = new System.Windows.Forms.Label();
            this.cbDisponibilite = new System.Windows.Forms.ComboBox();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAjouter = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.btnCommander = new System.Windows.Forms.Button();
            this.btnInventaire = new System.Windows.Forms.Button();
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
            this.flpFiltres.SuspendLayout();
            this.flpActions.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.flpFiltres, 0, 1);
            this.tlpRoot.Controls.Add(this.dgvStock, 0, 2);
            this.tlpRoot.Controls.Add(this.flpActions, 0, 3);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Stock";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpFiltres
            // 
            this.flpFiltres.Controls.Add(this.lblRecherche);
            this.flpFiltres.Controls.Add(this.txtSearchProduit);
            this.flpFiltres.Controls.Add(this.lblFiltre);
            this.flpFiltres.Controls.Add(this.cbSeuil);
            this.flpFiltres.Controls.Add(this.lblDisponibilite);
            this.flpFiltres.Controls.Add(this.cbDisponibilite);
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
            // txtSearchProduit
            // 
            this.txtSearchProduit.Width = 260;
            this.txtSearchProduit.MaximumSize = new System.Drawing.Size(400, 0);
            this.txtSearchProduit.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.txtSearchProduit.Name = "txtSearchProduit";
            this.txtSearchProduit.TextChanged += new System.EventHandler(this.txtSearchProduit_TextChanged);
            // 
            // lblFiltre
            // 
            this.lblFiltre.Text = "Afficher";
            this.lblFiltre.AutoSize = true;
            this.lblFiltre.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltre.Name = "lblFiltre";
            // 
            // cbSeuil
            // 
            this.cbSeuil.Width = 190;
            this.cbSeuil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeuil.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
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
            this.lblDisponibilite.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblDisponibilite.Name = "lblDisponibilite";
            // 
            // cbDisponibilite
            // 
            this.cbDisponibilite.Width = 150;
            this.cbDisponibilite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisponibilite.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
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
            // flpActions
            // 
            this.flpActions.Controls.Add(this.btnAjouter);
            this.flpActions.Controls.Add(this.btnModifier);
            this.flpActions.Controls.Add(this.btnSupprimer);
            this.flpActions.Controls.Add(this.btnCommander);
            this.flpActions.Controls.Add(this.btnInventaire);
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.WrapContents = true;
            this.flpActions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpActions.Name = "flpActions";
            // 
            // btnAjouter
            // 
            this.btnAjouter.Text = "Ajouter un produit";
            this.btnAjouter.Tag = "primaire";
            this.btnAjouter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
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
            this.btnCommander.Text = "Commander au fournisseur";
            this.btnCommander.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCommander.Name = "btnCommander";
            this.btnCommander.Click += new System.EventHandler(this.btnCommander_Click);
            // 
            // btnInventaire
            // 
            this.btnInventaire.Text = "Feuille d'inventaire";
            this.btnInventaire.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnInventaire.Name = "btnInventaire";
            this.btnInventaire.Click += new System.EventHandler(this.btnInventaire_Click);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Stock";
            this.ResumeLayout(false);
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.flpFiltres.ResumeLayout(false);
            this.flpFiltres.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtSearchProduit;
        private System.Windows.Forms.Label lblFiltre;
        private System.Windows.Forms.ComboBox cbSeuil;
        private System.Windows.Forms.Label lblDisponibilite;
        private System.Windows.Forms.ComboBox cbDisponibilite;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Button btnCommander;
        private System.Windows.Forms.Button btnInventaire;
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
