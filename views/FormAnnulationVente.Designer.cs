namespace Pharmacie2.views
{
    partial class FormAnnulationVente
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
            this.dgvLignes = new System.Windows.Forms.DataGridView();
            this.lblMotifLabel = new System.Windows.Forms.Label();
            this.txtMotif = new System.Windows.Forms.TextBox();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnConfirmer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).BeginInit();
            this.tlpRoot.SuspendLayout();
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
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.tlpRoot.Controls.Add(this.lblInfo, 0, 1);
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 2);
            this.tlpRoot.Controls.Add(this.lblMotifLabel, 0, 3);
            this.tlpRoot.Controls.Add(this.txtMotif, 0, 4);
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 5);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Annuler la vente";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblInfo
            // 
            this.lblInfo.Text = "";
            this.lblInfo.AutoSize = true;
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblInfo.Name = "lblInfo";
            // 
            // dgvLignes
            // 
            this.dgvLignes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduit,
            this.colUnite,
            this.colQuantite});
            this.dgvLignes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLignes.AutoGenerateColumns = false;
            this.dgvLignes.ReadOnly = true;
            this.dgvLignes.Name = "dgvLignes";
            // 
            // lblMotifLabel
            // 
            this.lblMotifLabel.Text = "Motif de l'annulation (obligatoire)";
            this.lblMotifLabel.AutoSize = true;
            this.lblMotifLabel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblMotifLabel.Name = "lblMotifLabel";
            // 
            // txtMotif
            // 
            this.txtMotif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMotif.Multiline = true;
            this.txtMotif.Height = 60;
            this.txtMotif.MaximumSize = new System.Drawing.Size(0, 90);
            this.txtMotif.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.txtMotif.Name = "txtMotif";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnConfirmer);
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnConfirmer
            // 
            this.btnConfirmer.Text = "Confirmer l'annulation";
            this.btnConfirmer.Tag = "danger";
            this.btnConfirmer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnConfirmer.Name = "btnConfirmer";
            this.btnConfirmer.Click += new System.EventHandler(this.btnConfirmer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Retour";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "Produit";
            this.colProduit.DataPropertyName = "Produit";
            this.colProduit.FillWeight = 60F;
            this.colProduit.MinimumWidth = 140;
            this.colProduit.ReadOnly = true;
            // 
            // colUnite
            // 
            this.colUnite.HeaderText = "Unité";
            this.colUnite.Name = "Unite";
            this.colUnite.DataPropertyName = "Unite";
            this.colUnite.FillWeight = 20F;
            this.colUnite.MinimumWidth = 80;
            this.colUnite.ReadOnly = true;
            // 
            // colQuantite
            // 
            this.colQuantite.HeaderText = "Quantité";
            this.colQuantite.Name = "Quantite";
            this.colQuantite.DataPropertyName = "Quantite";
            this.colQuantite.FillWeight = 20F;
            this.colQuantite.MinimumWidth = 70;
            this.colQuantite.ReadOnly = true;
            // 
            // FormAnnulationVente
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(600, 520);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Annulation de vente";
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormAnnulationVente";
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
        private System.Windows.Forms.DataGridView dgvLignes;
        private System.Windows.Forms.Label lblMotifLabel;
        private System.Windows.Forms.TextBox txtMotif;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnConfirmer;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantite;
    }
}
