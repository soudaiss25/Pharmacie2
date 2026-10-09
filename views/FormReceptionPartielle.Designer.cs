namespace Pharmacie2.views
{
    partial class FormReceptionPartielle
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblAvertissement = new System.Windows.Forms.Label();
            this.dgvLignes = new System.Windows.Forms.DataGridView();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnToutRecevoir = new System.Windows.Forms.Button();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.colLigneId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCommande = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDejaRecu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRecu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 5;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.lblInfo, 0, 1);
            this.lblInfo.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblAvertissement, 0, 2);
            this.lblAvertissement.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 3);
            this.dgvLignes.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 4);
            this.flpBoutons.TabIndex = 4;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Réception de commande";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblInfo
            // 
            this.lblInfo.Text = "Indiquez, pour chaque produit, le nombre de boîtes reçues aujourd'hui.";
            this.lblInfo.AutoSize = true;
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblInfo.Name = "lblInfo";
            // 
            // lblAvertissement
            // 
            this.lblAvertissement.Text = "";
            this.lblAvertissement.AutoSize = true;
            this.lblAvertissement.Tag = "attention";
            this.lblAvertissement.Visible = false;
            this.lblAvertissement.MaximumSize = new System.Drawing.Size(900, 0);
            this.lblAvertissement.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblAvertissement.Name = "lblAvertissement";
            // 
            // dgvLignes
            // 
            this.dgvLignes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLigneId,
            this.colProduit,
            this.colCommande,
            this.colDejaRecu,
            this.colReste,
            this.colRecu});
            this.dgvLignes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLignes.AutoGenerateColumns = false;
            this.dgvLignes.AllowUserToAddRows = false;
            this.dgvLignes.AllowUserToDeleteRows = false;
            this.dgvLignes.Name = "dgvLignes";
            this.dgvLignes.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvLignes_CellValidating);
            this.dgvLignes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLignes_DataError);
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnToutRecevoir);
            this.btnToutRecevoir.TabIndex = 0;
            this.flpBoutons.Controls.Add(this.btnValider);
            this.btnValider.TabIndex = 1;
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 2;
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.WrapContents = true;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnToutRecevoir
            // 
            this.btnToutRecevoir.Text = "Tout recevoir";
            this.btnToutRecevoir.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnToutRecevoir.Name = "btnToutRecevoir";
            this.btnToutRecevoir.Click += new System.EventHandler(this.btnToutRecevoir_Click);
            // 
            // btnValider
            // 
            this.btnValider.Text = "Valider la réception";
            this.btnValider.Tag = "primaire";
            this.btnValider.Margin = new System.Windows.Forms.Padding(16, 0, 8, 0);
            this.btnValider.Name = "btnValider";
            this.btnValider.Click += new System.EventHandler(this.btnValider_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // colLigneId
            // 
            this.colLigneId.HeaderText = "Id";
            this.colLigneId.Name = "colLigneId";
            this.colLigneId.DataPropertyName = "colLigneId";
            this.colLigneId.FillWeight = 100F;
            this.colLigneId.MinimumWidth = 60;
            this.colLigneId.ReadOnly = true;
            this.colLigneId.Visible = false;
            // 
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "colProduit";
            this.colProduit.DataPropertyName = "colProduit";
            this.colProduit.FillWeight = 36F;
            this.colProduit.MinimumWidth = 160;
            this.colProduit.ReadOnly = true;
            // 
            // colCommande
            // 
            this.colCommande.HeaderText = "Commandé";
            this.colCommande.Name = "colCommande";
            this.colCommande.DataPropertyName = "colCommande";
            this.colCommande.FillWeight = 13F;
            this.colCommande.MinimumWidth = 80;
            this.colCommande.ReadOnly = true;
            // 
            // colDejaRecu
            // 
            this.colDejaRecu.HeaderText = "Déjà reçu";
            this.colDejaRecu.Name = "colDejaRecu";
            this.colDejaRecu.DataPropertyName = "colDejaRecu";
            this.colDejaRecu.FillWeight = 13F;
            this.colDejaRecu.MinimumWidth = 80;
            this.colDejaRecu.ReadOnly = true;
            // 
            // colReste
            // 
            this.colReste.HeaderText = "Reste à recevoir";
            this.colReste.Name = "colReste";
            this.colReste.DataPropertyName = "colReste";
            this.colReste.FillWeight = 14F;
            this.colReste.MinimumWidth = 100;
            this.colReste.ReadOnly = true;
            // 
            // colRecu
            // 
            this.colRecu.HeaderText = "Reçu maintenant";
            this.colRecu.Name = "colRecu";
            this.colRecu.DataPropertyName = "colRecu";
            this.colRecu.FillWeight = 14F;
            this.colRecu.MinimumWidth = 110;
            this.colRecu.ReadOnly = false;
            // 
            // FormReceptionPartielle
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(860, 480);
            this.MinimumSize = new System.Drawing.Size(680, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Réception de commande";
            this.AcceptButton = this.btnValider;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormReceptionPartielle";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblAvertissement;
        private System.Windows.Forms.DataGridView dgvLignes;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnToutRecevoir;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLigneId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommande;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDejaRecu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReste;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecu;
    }
}
