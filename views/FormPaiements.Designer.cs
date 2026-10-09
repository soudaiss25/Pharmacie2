namespace Pharmacie2.views
{
    partial class FormPaiements
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
            this.lblNumero = new System.Windows.Forms.Label();
            this.flpInfos = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblVerse = new System.Windows.Forms.Label();
            this.lblRestant = new System.Windows.Forms.Label();
            this.lblVendeurVente = new System.Windows.Forms.Label();
            this.lblMotif = new System.Windows.Forms.Label();
            this.flpSaisie = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSaisie = new System.Windows.Forms.Label();
            this.numMontant = new System.Windows.Forms.NumericUpDown();
            this.btnAjouterPaiement = new System.Windows.Forms.Button();
            this.lblRetour = new System.Windows.Forms.Label();
            this.dgvPaiements = new System.Windows.Forms.DataGridView();
            this.btnFermer = new System.Windows.Forms.Button();
            this.colNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCaissier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numMontant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaiements)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpInfos.SuspendLayout();
            this.flpSaisie.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 6;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblNumero, 0, 0);
            this.lblNumero.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpInfos, 0, 1);
            this.flpInfos.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.flpSaisie, 0, 2);
            this.flpSaisie.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.lblRetour, 0, 3);
            this.lblRetour.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.dgvPaiements, 0, 4);
            this.dgvPaiements.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.btnFermer, 0, 5);
            this.btnFermer.TabIndex = 5;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblNumero
            // 
            this.lblNumero.Text = "Vente";
            this.lblNumero.AutoSize = true;
            this.lblNumero.Tag = "titre";
            this.lblNumero.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblNumero.Name = "lblNumero";
            // 
            // flpInfos
            // 
            this.flpInfos.Controls.Add(this.lblTotal);
            this.lblTotal.TabIndex = 0;
            this.flpInfos.Controls.Add(this.lblVerse);
            this.lblVerse.TabIndex = 1;
            this.flpInfos.Controls.Add(this.lblRestant);
            this.lblRestant.TabIndex = 2;
            this.flpInfos.Controls.Add(this.lblVendeurVente);
            this.lblVendeurVente.TabIndex = 3;
            this.flpInfos.Controls.Add(this.lblMotif);
            this.lblMotif.TabIndex = 4;
            this.flpInfos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpInfos.AutoSize = true;
            this.flpInfos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpInfos.WrapContents = true;
            this.flpInfos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpInfos.Name = "flpInfos";
            // 
            // lblTotal
            // 
            this.lblTotal.Text = "";
            this.lblTotal.AutoSize = true;
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblTotal.Name = "lblTotal";
            // 
            // lblVerse
            // 
            this.lblVerse.Text = "";
            this.lblVerse.AutoSize = true;
            this.lblVerse.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblVerse.Name = "lblVerse";
            // 
            // lblRestant
            // 
            this.lblRestant.Text = "";
            this.lblRestant.AutoSize = true;
            this.lblRestant.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblRestant.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestant.Name = "lblRestant";
            // 
            // lblVendeurVente
            // 
            this.lblVendeurVente.Text = "";
            this.lblVendeurVente.AutoSize = true;
            this.lblVendeurVente.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblVendeurVente.Tag = "note";
            this.lblVendeurVente.Name = "lblVendeurVente";
            // 
            // lblMotif
            // 
            this.lblMotif.Text = "";
            this.lblMotif.AutoSize = true;
            this.lblMotif.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblMotif.Tag = "note";
            this.lblMotif.Name = "lblMotif";
            // 
            // flpSaisie
            // 
            this.flpSaisie.Controls.Add(this.lblSaisie);
            this.lblSaisie.TabIndex = 0;
            this.flpSaisie.Controls.Add(this.numMontant);
            this.numMontant.TabIndex = 1;
            this.flpSaisie.Controls.Add(this.btnAjouterPaiement);
            this.btnAjouterPaiement.TabIndex = 2;
            this.flpSaisie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSaisie.AutoSize = true;
            this.flpSaisie.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSaisie.WrapContents = true;
            this.flpSaisie.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpSaisie.Name = "flpSaisie";
            // 
            // lblSaisie
            // 
            this.lblSaisie.Text = "Montant reçu";
            this.lblSaisie.AutoSize = true;
            this.lblSaisie.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblSaisie.Name = "lblSaisie";
            // 
            // numMontant
            // 
            this.numMontant.Width = 170;
            this.numMontant.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numMontant.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numMontant.ThousandsSeparator = true;
            this.numMontant.DecimalPlaces = 0;
            this.numMontant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMontant.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.numMontant.Name = "numMontant";
            // 
            // btnAjouterPaiement
            // 
            this.btnAjouterPaiement.Text = "Enregistrer le paiement";
            this.btnAjouterPaiement.Tag = "primaire";
            this.btnAjouterPaiement.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAjouterPaiement.Name = "btnAjouterPaiement";
            this.btnAjouterPaiement.Click += new System.EventHandler(this.btnAjouterPaiement_Click);
            // 
            // lblRetour
            // 
            this.lblRetour.Text = "";
            this.lblRetour.AutoSize = true;
            this.lblRetour.Tag = "succes";
            this.lblRetour.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblRetour.Name = "lblRetour";
            // 
            // dgvPaiements
            // 
            this.dgvPaiements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumero,
            this.colDate,
            this.colMontant,
            this.colCaissier});
            this.dgvPaiements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPaiements.AutoGenerateColumns = false;
            this.dgvPaiements.ReadOnly = true;
            this.dgvPaiements.Name = "dgvPaiements";
            // 
            // btnFermer
            // 
            this.btnFermer.Text = "Fermer";
            this.btnFermer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFermer.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnFermer.Name = "btnFermer";
            this.btnFermer.Click += new System.EventHandler(this.btnFermer_Click);
            // 
            // colNumero
            // 
            this.colNumero.HeaderText = "Paiement";
            this.colNumero.Name = "Numero";
            this.colNumero.DataPropertyName = "Numero";
            this.colNumero.FillWeight = 26F;
            this.colNumero.MinimumWidth = 100;
            this.colNumero.ReadOnly = true;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "Date";
            this.colDate.DataPropertyName = "Date";
            this.colDate.FillWeight = 30F;
            this.colDate.MinimumWidth = 120;
            this.colDate.ReadOnly = true;
            // 
            // colMontant
            // 
            this.colMontant.HeaderText = "Montant";
            this.colMontant.Name = "Montant";
            this.colMontant.DataPropertyName = "Montant";
            this.colMontant.FillWeight = 24F;
            this.colMontant.MinimumWidth = 100;
            this.colMontant.ReadOnly = true;
            this.colMontant.Tag = "montant";
            // 
            // colCaissier
            // 
            this.colCaissier.HeaderText = "Encaissé par";
            this.colCaissier.Name = "Caissier";
            this.colCaissier.DataPropertyName = "Caissier";
            this.colCaissier.FillWeight = 26F;
            this.colCaissier.MinimumWidth = 100;
            this.colCaissier.ReadOnly = true;
            // 
            // FormPaiements
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(640, 520);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paiements de la vente";
            this.AcceptButton = this.btnAjouterPaiement;
            this.CancelButton = this.btnFermer;
            this.Name = "FormPaiements";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpSaisie.ResumeLayout(false);
            this.flpSaisie.PerformLayout();
            this.flpInfos.ResumeLayout(false);
            this.flpInfos.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaiements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMontant)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.FlowLayoutPanel flpInfos;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblVerse;
        private System.Windows.Forms.Label lblRestant;
        private System.Windows.Forms.Label lblVendeurVente;
        private System.Windows.Forms.Label lblMotif;
        private System.Windows.Forms.FlowLayoutPanel flpSaisie;
        private System.Windows.Forms.Label lblSaisie;
        private System.Windows.Forms.NumericUpDown numMontant;
        private System.Windows.Forms.Button btnAjouterPaiement;
        private System.Windows.Forms.Label lblRetour;
        private System.Windows.Forms.DataGridView dgvPaiements;
        private System.Windows.Forms.Button btnFermer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCaissier;
    }
}
