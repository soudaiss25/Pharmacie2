namespace Pharmacie2.views
{
    partial class FormCommandesFournisseur
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
            this.flpFiltre = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFiltreStatut = new System.Windows.Forms.Label();
            this.cbFiltreStatut = new System.Windows.Forms.ComboBox();
            this.lblNbCommandes = new System.Windows.Forms.Label();
            this.dgvCommandes = new System.Windows.Forms.DataGridView();
            this.lblDetailTitre = new System.Windows.Forms.Label();
            this.dgvLignes = new System.Windows.Forms.DataGridView();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnMarquerRecu = new System.Windows.Forms.Button();
            this.btnMarquerPartiel = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.btnFermer = new System.Windows.Forms.Button();
            this.colCmdId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNbLig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLivraison = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReception = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLigId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSousTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockActuel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpFiltre.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 6;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpFiltre, 0, 1);
            this.flpFiltre.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.dgvCommandes, 0, 2);
            this.dgvCommandes.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.lblDetailTitre, 0, 3);
            this.lblDetailTitre.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 4);
            this.dgvLignes.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 5);
            this.flpBoutons.TabIndex = 5;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Commandes";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpFiltre
            // 
            this.flpFiltre.Controls.Add(this.lblFiltreStatut);
            this.lblFiltreStatut.TabIndex = 0;
            this.flpFiltre.Controls.Add(this.cbFiltreStatut);
            this.cbFiltreStatut.TabIndex = 1;
            this.flpFiltre.Controls.Add(this.lblNbCommandes);
            this.lblNbCommandes.TabIndex = 2;
            this.flpFiltre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFiltre.AutoSize = true;
            this.flpFiltre.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFiltre.WrapContents = true;
            this.flpFiltre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpFiltre.Name = "flpFiltre";
            // 
            // lblFiltreStatut
            // 
            this.lblFiltreStatut.Text = "Statut";
            this.lblFiltreStatut.AutoSize = true;
            this.lblFiltreStatut.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFiltreStatut.Name = "lblFiltreStatut";
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
            // lblNbCommandes
            // 
            this.lblNbCommandes.Text = "";
            this.lblNbCommandes.AutoSize = true;
            this.lblNbCommandes.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.lblNbCommandes.Name = "lblNbCommandes";
            // 
            // dgvCommandes
            // 
            this.dgvCommandes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCmdId,
            this.colDate,
            this.colStatut,
            this.colNbLig,
            this.colTotal,
            this.colLivraison,
            this.colReception,
            this.colNote});
            this.dgvCommandes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCommandes.AutoGenerateColumns = false;
            this.dgvCommandes.ReadOnly = true;
            this.dgvCommandes.Name = "dgvCommandes";
            this.dgvCommandes.SelectionChanged += new System.EventHandler(this.dgvCommandes_SelectionChanged);
            // 
            // lblDetailTitre
            // 
            this.lblDetailTitre.Text = "Produits de la commande sélectionnée";
            this.lblDetailTitre.AutoSize = true;
            this.lblDetailTitre.Tag = "section";
            this.lblDetailTitre.Margin = new System.Windows.Forms.Padding(0, 8, 0, 6);
            this.lblDetailTitre.Name = "lblDetailTitre";
            // 
            // dgvLignes
            // 
            this.dgvLignes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLigId,
            this.colProduit,
            this.colQte,
            this.colPrixU,
            this.colSousTotal,
            this.colStockActuel});
            this.dgvLignes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLignes.AutoGenerateColumns = false;
            this.dgvLignes.ReadOnly = true;
            this.dgvLignes.Name = "dgvLignes";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnMarquerRecu);
            this.btnMarquerRecu.TabIndex = 0;
            this.flpBoutons.Controls.Add(this.btnMarquerPartiel);
            this.btnMarquerPartiel.TabIndex = 1;
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 2;
            this.flpBoutons.Controls.Add(this.btnFermer);
            this.btnFermer.TabIndex = 3;
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.WrapContents = true;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
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
            // btnFermer
            // 
            this.btnFermer.Text = "Fermer";
            this.btnFermer.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnFermer.Name = "btnFermer";
            this.btnFermer.Click += new System.EventHandler(this.btnFermer_Click);
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
            this.colDate.FillWeight = 16F;
            this.colDate.MinimumWidth = 110;
            this.colDate.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Statut";
            this.colStatut.Name = "colStatut";
            this.colStatut.DataPropertyName = "colStatut";
            this.colStatut.FillWeight = 16F;
            this.colStatut.MinimumWidth = 110;
            this.colStatut.ReadOnly = true;
            // 
            // colNbLig
            // 
            this.colNbLig.HeaderText = "Produits";
            this.colNbLig.Name = "colNbLig";
            this.colNbLig.DataPropertyName = "colNbLig";
            this.colNbLig.FillWeight = 8F;
            this.colNbLig.MinimumWidth = 70;
            this.colNbLig.ReadOnly = true;
            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.DataPropertyName = "colTotal";
            this.colTotal.FillWeight = 13F;
            this.colTotal.MinimumWidth = 100;
            this.colTotal.ReadOnly = true;
            this.colTotal.Tag = "montant";
            // 
            // colLivraison
            // 
            this.colLivraison.HeaderText = "Livraison prévue";
            this.colLivraison.Name = "colLivraison";
            this.colLivraison.DataPropertyName = "colLivraison";
            this.colLivraison.FillWeight = 12F;
            this.colLivraison.MinimumWidth = 100;
            this.colLivraison.ReadOnly = true;
            // 
            // colReception
            // 
            this.colReception.HeaderText = "Reçue le";
            this.colReception.Name = "colReception";
            this.colReception.DataPropertyName = "colReception";
            this.colReception.FillWeight = 14F;
            this.colReception.MinimumWidth = 110;
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
            // colLigId
            // 
            this.colLigId.HeaderText = "Id";
            this.colLigId.Name = "colLigId";
            this.colLigId.DataPropertyName = "colLigId";
            this.colLigId.FillWeight = 100F;
            this.colLigId.MinimumWidth = 60;
            this.colLigId.ReadOnly = true;
            this.colLigId.Visible = false;
            // 
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "colProduit";
            this.colProduit.DataPropertyName = "colProduit";
            this.colProduit.FillWeight = 34F;
            this.colProduit.MinimumWidth = 140;
            this.colProduit.ReadOnly = true;
            // 
            // colQte
            // 
            this.colQte.HeaderText = "Commandé (boîtes)";
            this.colQte.Name = "colQte";
            this.colQte.DataPropertyName = "colQte";
            this.colQte.FillWeight = 14F;
            this.colQte.MinimumWidth = 110;
            this.colQte.ReadOnly = true;
            // 
            // colPrixU
            // 
            this.colPrixU.HeaderText = "Prix unitaire";
            this.colPrixU.Name = "colPrixU";
            this.colPrixU.DataPropertyName = "colPrixU";
            this.colPrixU.FillWeight = 16F;
            this.colPrixU.MinimumWidth = 100;
            this.colPrixU.ReadOnly = true;
            this.colPrixU.Tag = "montant";
            // 
            // colSousTotal
            // 
            this.colSousTotal.HeaderText = "Total ligne";
            this.colSousTotal.Name = "colSousTotal";
            this.colSousTotal.DataPropertyName = "colSousTotal";
            this.colSousTotal.FillWeight = 16F;
            this.colSousTotal.MinimumWidth = 100;
            this.colSousTotal.ReadOnly = true;
            this.colSousTotal.Tag = "montant";
            // 
            // colStockActuel
            // 
            this.colStockActuel.HeaderText = "Stock actuel";
            this.colStockActuel.Name = "colStockActuel";
            this.colStockActuel.DataPropertyName = "colStockActuel";
            this.colStockActuel.FillWeight = 20F;
            this.colStockActuel.MinimumWidth = 120;
            this.colStockActuel.ReadOnly = true;
            // 
            // FormCommandesFournisseur
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.MinimumSize = new System.Drawing.Size(800, 520);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Commandes du fournisseur";
            this.CancelButton = this.btnFermer;
            this.Name = "FormCommandesFournisseur";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.flpFiltre.ResumeLayout(false);
            this.flpFiltre.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpFiltre;
        private System.Windows.Forms.Label lblFiltreStatut;
        private System.Windows.Forms.ComboBox cbFiltreStatut;
        private System.Windows.Forms.Label lblNbCommandes;
        private System.Windows.Forms.DataGridView dgvCommandes;
        private System.Windows.Forms.Label lblDetailTitre;
        private System.Windows.Forms.DataGridView dgvLignes;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnMarquerRecu;
        private System.Windows.Forms.Button btnMarquerPartiel;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.Button btnFermer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCmdId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNbLig;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLivraison;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReception;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLigId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSousTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockActuel;
    }
}
