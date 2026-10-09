namespace Pharmacie2.views
{
    partial class FormChoixProduit
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
            this.flpRecherche = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtRecherche = new System.Windows.Forms.TextBox();
            this.dgvProduits = new System.Windows.Forms.DataGridView();
            this.flpChoix = new System.Windows.Forms.FlowLayoutPanel();
            this.lblUnite = new System.Windows.Forms.Label();
            this.cbUnite = new System.Windows.Forms.ComboBox();
            this.lblQuantite = new System.Windows.Forms.Label();
            this.numQuantite = new System.Windows.Forms.NumericUpDown();
            this.lblPrixUnit = new System.Windows.Forms.Label();
            this.pnlPosologie = new System.Windows.Forms.TableLayoutPanel();
            this.lblIndicationVal = new System.Windows.Forms.Label();
            this.lblPosologieVal = new System.Windows.Forms.Label();
            this.lblNbFoisVal = new System.Windows.Forms.Label();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUniteVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParBoite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpRecherche.SuspendLayout();
            this.flpChoix.SuspendLayout();
            this.pnlPosologie.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 5;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.flpRecherche, 0, 0);
            this.flpRecherche.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.dgvProduits, 0, 1);
            this.dgvProduits.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.flpChoix, 0, 2);
            this.flpChoix.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.pnlPosologie, 0, 3);
            this.pnlPosologie.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 4);
            this.flpBoutons.TabIndex = 4;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // flpRecherche
            // 
            this.flpRecherche.Controls.Add(this.lblRecherche);
            this.lblRecherche.TabIndex = 0;
            this.flpRecherche.Controls.Add(this.txtRecherche);
            this.txtRecherche.TabIndex = 1;
            this.flpRecherche.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRecherche.AutoSize = true;
            this.flpRecherche.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpRecherche.WrapContents = true;
            this.flpRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpRecherche.Name = "flpRecherche";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Rechercher un produit";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtRecherche
            // 
            this.txtRecherche.Width = 320;
            this.txtRecherche.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtRecherche.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.txtRecherche.Name = "txtRecherche";
            this.txtRecherche.TextChanged += new System.EventHandler(this.txtRecherche_TextChanged);
            // 
            // dgvProduits
            // 
            this.dgvProduits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNom,
            this.colType,
            this.colPrixVente,
            this.colStock,
            this.colUniteVente,
            this.colParBoite});
            this.dgvProduits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduits.AutoGenerateColumns = false;
            this.dgvProduits.ReadOnly = true;
            this.dgvProduits.Name = "dgvProduits";
            this.dgvProduits.SelectionChanged += new System.EventHandler(this.dgvProduits_SelectionChanged);
            this.dgvProduits.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduits_CellDoubleClick);
            // 
            // flpChoix
            // 
            this.flpChoix.Controls.Add(this.lblUnite);
            this.lblUnite.TabIndex = 0;
            this.flpChoix.Controls.Add(this.cbUnite);
            this.cbUnite.TabIndex = 1;
            this.flpChoix.Controls.Add(this.lblQuantite);
            this.lblQuantite.TabIndex = 2;
            this.flpChoix.Controls.Add(this.numQuantite);
            this.numQuantite.TabIndex = 3;
            this.flpChoix.Controls.Add(this.lblPrixUnit);
            this.lblPrixUnit.TabIndex = 4;
            this.flpChoix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpChoix.AutoSize = true;
            this.flpChoix.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpChoix.WrapContents = true;
            this.flpChoix.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.flpChoix.Name = "flpChoix";
            // 
            // lblUnite
            // 
            this.lblUnite.Text = "Vendre en";
            this.lblUnite.AutoSize = true;
            this.lblUnite.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblUnite.Name = "lblUnite";
            // 
            // cbUnite
            // 
            this.cbUnite.Width = 160;
            this.cbUnite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnite.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.cbUnite.Name = "cbUnite";
            this.cbUnite.SelectedIndexChanged += new System.EventHandler(this.cbUnite_SelectedIndexChanged);
            // 
            // lblQuantite
            // 
            this.lblQuantite.Text = "Quantité";
            this.lblQuantite.AutoSize = true;
            this.lblQuantite.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblQuantite.Name = "lblQuantite";
            // 
            // numQuantite
            // 
            this.numQuantite.Width = 90;
            this.numQuantite.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numQuantite.Maximum = new decimal(new int[] { 10000, 0, 0, 0});
            this.numQuantite.Value = new decimal(new int[] { 1, 0, 0, 0});
            this.numQuantite.ThousandsSeparator = true;
            this.numQuantite.DecimalPlaces = 0;
            this.numQuantite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numQuantite.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.numQuantite.Name = "numQuantite";
            // 
            // lblPrixUnit
            // 
            this.lblPrixUnit.Text = "";
            this.lblPrixUnit.AutoSize = true;
            this.lblPrixUnit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrixUnit.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblPrixUnit.Name = "lblPrixUnit";
            // 
            // pnlPosologie
            // 
            this.pnlPosologie.ColumnCount = 1;
            this.pnlPosologie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlPosologie.RowCount = 3;
            this.pnlPosologie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlPosologie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlPosologie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlPosologie.Controls.Add(this.lblIndicationVal, 0, 0);
            this.lblIndicationVal.TabIndex = 0;
            this.pnlPosologie.Controls.Add(this.lblPosologieVal, 0, 1);
            this.lblPosologieVal.TabIndex = 1;
            this.pnlPosologie.Controls.Add(this.lblNbFoisVal, 0, 2);
            this.lblNbFoisVal.TabIndex = 2;
            this.pnlPosologie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPosologie.AutoSize = true;
            this.pnlPosologie.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPosologie.Visible = false;
            this.pnlPosologie.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pnlPosologie.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pnlPosologie.Tag = "carte";
            this.pnlPosologie.Name = "pnlPosologie";
            // 
            // lblIndicationVal
            // 
            this.lblIndicationVal.Text = "";
            this.lblIndicationVal.AutoSize = true;
            this.lblIndicationVal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblIndicationVal.Name = "lblIndicationVal";
            // 
            // lblPosologieVal
            // 
            this.lblPosologieVal.Text = "";
            this.lblPosologieVal.AutoSize = true;
            this.lblPosologieVal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblPosologieVal.Name = "lblPosologieVal";
            // 
            // lblNbFoisVal
            // 
            this.lblNbFoisVal.Text = "";
            this.lblNbFoisVal.AutoSize = true;
            this.lblNbFoisVal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblNbFoisVal.Name = "lblNbFoisVal";
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
            this.btnValider.Text = "Choisir ce produit";
            this.btnValider.Tag = "primaire";
            this.btnValider.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
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
            this.colNom.FillWeight = 44F;
            this.colNom.MinimumWidth = 160;
            this.colNom.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "Type";
            this.colType.DataPropertyName = "Type";
            this.colType.FillWeight = 16F;
            this.colType.MinimumWidth = 90;
            this.colType.ReadOnly = true;
            // 
            // colPrixVente
            // 
            this.colPrixVente.HeaderText = "Prix (boîte)";
            this.colPrixVente.Name = "PrixVente";
            this.colPrixVente.DataPropertyName = "PrixVente";
            this.colPrixVente.FillWeight = 18F;
            this.colPrixVente.MinimumWidth = 100;
            this.colPrixVente.ReadOnly = true;
            this.colPrixVente.Tag = "montant";
            // 
            // colStock
            // 
            this.colStock.HeaderText = "En stock";
            this.colStock.Name = "Stock";
            this.colStock.DataPropertyName = "Stock";
            this.colStock.FillWeight = 22F;
            this.colStock.MinimumWidth = 130;
            this.colStock.ReadOnly = true;
            // 
            // colUniteVente
            // 
            this.colUniteVente.HeaderText = "Unité";
            this.colUniteVente.Name = "UniteVente";
            this.colUniteVente.DataPropertyName = "UniteVente";
            this.colUniteVente.FillWeight = 100F;
            this.colUniteVente.MinimumWidth = 60;
            this.colUniteVente.ReadOnly = true;
            this.colUniteVente.Visible = false;
            // 
            // colParBoite
            // 
            this.colParBoite.HeaderText = "Par boîte";
            this.colParBoite.Name = "ParBoite";
            this.colParBoite.DataPropertyName = "ParBoite";
            this.colParBoite.FillWeight = 100F;
            this.colParBoite.MinimumWidth = 60;
            this.colParBoite.ReadOnly = true;
            this.colParBoite.Visible = false;
            // 
            // FormChoixProduit
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.MinimumSize = new System.Drawing.Size(760, 520);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choisir un produit";
            this.AcceptButton = this.btnValider;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormChoixProduit";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.pnlPosologie.ResumeLayout(false);
            this.pnlPosologie.PerformLayout();
            this.flpChoix.ResumeLayout(false);
            this.flpChoix.PerformLayout();
            this.flpRecherche.ResumeLayout(false);
            this.flpRecherche.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduits)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.FlowLayoutPanel flpRecherche;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtRecherche;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.FlowLayoutPanel flpChoix;
        private System.Windows.Forms.Label lblUnite;
        private System.Windows.Forms.ComboBox cbUnite;
        private System.Windows.Forms.Label lblQuantite;
        private System.Windows.Forms.NumericUpDown numQuantite;
        private System.Windows.Forms.Label lblPrixUnit;
        private System.Windows.Forms.TableLayoutPanel pnlPosologie;
        private System.Windows.Forms.Label lblIndicationVal;
        private System.Windows.Forms.Label lblPosologieVal;
        private System.Windows.Forms.Label lblNbFoisVal;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUniteVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParBoite;
    }
}
