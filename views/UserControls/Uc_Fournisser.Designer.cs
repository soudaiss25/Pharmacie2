namespace Pharmacie2.views.UserControls
{
    partial class Uc_Fournisser
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvFournisseurs = new System.Windows.Forms.DataGridView();
            this.flpActionsF = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnVoirCommandes = new System.Windows.Forms.Button();
            this.lblProduitsTitre = new System.Windows.Forms.Label();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
            this.flpActionsP = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCommanderProduit = new System.Windows.Forms.Button();
            this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitQte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitSeuil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitPrixAchat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitPrixVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduitEtat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFournisseurs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpFiltres.SuspendLayout();
            this.flpActionsF.SuspendLayout();
            this.flpActionsP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 7;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpFiltres, 0, 1);
            this.flpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.dgvFournisseurs, 0, 2);
            this.dgvFournisseurs.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.flpActionsF, 0, 3);
            this.flpActionsF.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.lblProduitsTitre, 0, 4);
            this.lblProduitsTitre.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.dgvProduits, 0, 5);
            this.dgvProduits.TabIndex = 5;
            this.tlpRoot.Controls.Add(this.flpActionsP, 0, 6);
            this.flpActionsP.TabIndex = 6;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Fournisseurs";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpFiltres
            // 
            this.flpFiltres.Controls.Add(this.lblRecherche);
            this.lblRecherche.TabIndex = 0;
            this.flpFiltres.Controls.Add(this.txtSearch);
            this.txtSearch.TabIndex = 1;
            this.flpFiltres.Controls.Add(this.btnClear);
            this.btnClear.TabIndex = 2;
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
            // txtSearch
            // 
            this.txtSearch.Width = 280;
            this.txtSearch.MaximumSize = new System.Drawing.Size(400, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.txtSearch.PlaceholderText = "Nom ou contact";
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnClear
            // 
            this.btnClear.Text = "Effacer";
            this.btnClear.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dgvFournisseurs
            // 
            this.dgvFournisseurs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNom,
            this.colContact});
            this.dgvFournisseurs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFournisseurs.AutoGenerateColumns = false;
            this.dgvFournisseurs.ReadOnly = true;
            this.dgvFournisseurs.Name = "dgvFournisseurs";
            this.dgvFournisseurs.SelectionChanged += new System.EventHandler(this.dgvFournisseurs_SelectionChanged);
            this.dgvFournisseurs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFournisseurs_CellFormatting);
            // 
            // flpActionsF
            // 
            this.flpActionsF.Controls.Add(this.btnAdd);
            this.btnAdd.TabIndex = 0;
            this.flpActionsF.Controls.Add(this.btnEdit);
            this.btnEdit.TabIndex = 1;
            this.flpActionsF.Controls.Add(this.btnDelete);
            this.btnDelete.TabIndex = 2;
            this.flpActionsF.Controls.Add(this.btnVoirCommandes);
            this.btnVoirCommandes.TabIndex = 3;
            this.flpActionsF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActionsF.AutoSize = true;
            this.flpActionsF.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActionsF.WrapContents = true;
            this.flpActionsF.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.flpActionsF.Name = "flpActionsF";
            // 
            // btnAdd
            // 
            this.btnAdd.Text = "Nouveau fournisseur";
            this.btnAdd.Tag = "primaire";
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Text = "Modifier";
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Text = "Archiver";
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnVoirCommandes
            // 
            this.btnVoirCommandes.Text = "Voir ses commandes";
            this.btnVoirCommandes.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnVoirCommandes.Enabled = false;
            this.btnVoirCommandes.Name = "btnVoirCommandes";
            this.btnVoirCommandes.Click += new System.EventHandler(this.btnVoirCommandes_Click);
            // 
            // lblProduitsTitre
            // 
            this.lblProduitsTitre.Text = "Sélectionnez un fournisseur pour voir ses produits";
            this.lblProduitsTitre.AutoSize = true;
            this.lblProduitsTitre.Tag = "section";
            this.lblProduitsTitre.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.lblProduitsTitre.Name = "lblProduitsTitre";
            // 
            // dgvProduits
            // 
            this.dgvProduits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduitId,
            this.colProduitNom,
            this.colProduitType,
            this.colProduitQte,
            this.colProduitSeuil,
            this.colProduitPrixAchat,
            this.colProduitPrixVente,
            this.colProduitEtat});
            this.dgvProduits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduits.AutoGenerateColumns = false;
            this.dgvProduits.ReadOnly = true;
            this.dgvProduits.Name = "dgvProduits";
            this.dgvProduits.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProduits_CellFormatting);
            // 
            // flpActionsP
            // 
            this.flpActionsP.Controls.Add(this.btnCommanderProduit);
            this.btnCommanderProduit.TabIndex = 0;
            this.flpActionsP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActionsP.AutoSize = true;
            this.flpActionsP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActionsP.WrapContents = true;
            this.flpActionsP.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpActionsP.Name = "flpActionsP";
            // 
            // btnCommanderProduit
            // 
            this.btnCommanderProduit.Text = "Commander ce produit";
            this.btnCommanderProduit.Tag = "primaire";
            this.btnCommanderProduit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCommanderProduit.Enabled = false;
            this.btnCommanderProduit.Name = "btnCommanderProduit";
            this.btnCommanderProduit.Click += new System.EventHandler(this.btnCommanderProduit_Click);
            // 
            // colNom
            // 
            this.colNom.HeaderText = "Fournisseur";
            this.colNom.Name = "Nom";
            this.colNom.DataPropertyName = "Nom";
            this.colNom.FillWeight = 55F;
            this.colNom.MinimumWidth = 160;
            this.colNom.ReadOnly = true;
            // 
            // colContact
            // 
            this.colContact.HeaderText = "Contact";
            this.colContact.Name = "Contact";
            this.colContact.DataPropertyName = "Contact";
            this.colContact.FillWeight = 45F;
            this.colContact.MinimumWidth = 120;
            this.colContact.ReadOnly = true;
            // 
            // colProduitId
            // 
            this.colProduitId.HeaderText = "Id";
            this.colProduitId.Name = "Id";
            this.colProduitId.DataPropertyName = "Id";
            this.colProduitId.FillWeight = 100F;
            this.colProduitId.MinimumWidth = 60;
            this.colProduitId.ReadOnly = true;
            this.colProduitId.Visible = false;
            // 
            // colProduitNom
            // 
            this.colProduitNom.HeaderText = "Produit";
            this.colProduitNom.Name = "Nom";
            this.colProduitNom.DataPropertyName = "Nom";
            this.colProduitNom.FillWeight = 30F;
            this.colProduitNom.MinimumWidth = 140;
            this.colProduitNom.ReadOnly = true;
            // 
            // colProduitType
            // 
            this.colProduitType.HeaderText = "Type";
            this.colProduitType.Name = "Type";
            this.colProduitType.DataPropertyName = "Type";
            this.colProduitType.FillWeight = 12F;
            this.colProduitType.MinimumWidth = 80;
            this.colProduitType.ReadOnly = true;
            // 
            // colProduitQte
            // 
            this.colProduitQte.HeaderText = "En stock";
            this.colProduitQte.Name = "Stock";
            this.colProduitQte.DataPropertyName = "Stock";
            this.colProduitQte.FillWeight = 22F;
            this.colProduitQte.MinimumWidth = 120;
            this.colProduitQte.ReadOnly = true;
            // 
            // colProduitSeuil
            // 
            this.colProduitSeuil.HeaderText = "Seuil";
            this.colProduitSeuil.Name = "Seuil";
            this.colProduitSeuil.DataPropertyName = "Seuil";
            this.colProduitSeuil.FillWeight = 9F;
            this.colProduitSeuil.MinimumWidth = 70;
            this.colProduitSeuil.ReadOnly = true;
            // 
            // colProduitPrixAchat
            // 
            this.colProduitPrixAchat.HeaderText = "Prix d'achat";
            this.colProduitPrixAchat.Name = "PrixAchat";
            this.colProduitPrixAchat.DataPropertyName = "PrixAchat";
            this.colProduitPrixAchat.FillWeight = 11F;
            this.colProduitPrixAchat.MinimumWidth = 90;
            this.colProduitPrixAchat.ReadOnly = true;
            this.colProduitPrixAchat.Tag = "montant";
            // 
            // colProduitPrixVente
            // 
            this.colProduitPrixVente.HeaderText = "Prix de vente";
            this.colProduitPrixVente.Name = "PrixVente";
            this.colProduitPrixVente.DataPropertyName = "PrixVente";
            this.colProduitPrixVente.FillWeight = 11F;
            this.colProduitPrixVente.MinimumWidth = 90;
            this.colProduitPrixVente.ReadOnly = true;
            this.colProduitPrixVente.Tag = "montant";
            // 
            // colProduitEtat
            // 
            this.colProduitEtat.HeaderText = "État";
            this.colProduitEtat.Name = "Etat";
            this.colProduitEtat.DataPropertyName = "Etat";
            this.colProduitEtat.FillWeight = 9F;
            this.colProduitEtat.MinimumWidth = 70;
            this.colProduitEtat.ReadOnly = true;
            // 
            // Uc_Fournisser
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Fournisser";
            this.ResumeLayout(false);
            this.flpActionsP.ResumeLayout(false);
            this.flpActionsP.PerformLayout();
            this.flpActionsF.ResumeLayout(false);
            this.flpActionsF.PerformLayout();
            this.flpFiltres.ResumeLayout(false);
            this.flpFiltres.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFournisseurs)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvFournisseurs;
        private System.Windows.Forms.FlowLayoutPanel flpActionsF;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnVoirCommandes;
        private System.Windows.Forms.Label lblProduitsTitre;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.FlowLayoutPanel flpActionsP;
        private System.Windows.Forms.Button btnCommanderProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitQte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitSeuil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitPrixAchat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitPrixVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitEtat;
    }
}
