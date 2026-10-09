namespace Pharmacie2.views
{
    partial class FormDetailVente
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
            this.tlpEntete = new System.Windows.Forms.TableLayoutPanel();
            this.lblT1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblT2 = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblT3 = new System.Windows.Forms.Label();
            this.lblTel = new System.Windows.Forms.Label();
            this.lblT4 = new System.Windows.Forms.Label();
            this.lblVendeur = new System.Windows.Forms.Label();
            this.lblT5 = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblT6 = new System.Windows.Forms.Label();
            this.lblStatut = new System.Windows.Forms.Label();
            this.lblMotif = new System.Windows.Forms.Label();
            this.dgvLignes = new System.Windows.Forms.DataGridView();
            this.flpTotaux = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblVerse = new System.Windows.Forms.Label();
            this.lblRestant = new System.Windows.Forms.Label();
            this.lblMutuelle = new System.Windows.Forms.Label();
            this.dgvPaiements = new System.Windows.Forms.DataGridView();
            this.btnFermer = new System.Windows.Forms.Button();
            this.colProduit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrixU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSousTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaiementNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaiementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaiementMontant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaiementCaissier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaiements)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpEntete.SuspendLayout();
            this.flpTotaux.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 8;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblNumero, 0, 0);
            this.lblNumero.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpEntete, 0, 1);
            this.tlpEntete.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblMotif, 0, 2);
            this.lblMotif.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.dgvLignes, 0, 3);
            this.dgvLignes.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpTotaux, 0, 4);
            this.flpTotaux.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.lblMutuelle, 0, 5);
            this.lblMutuelle.TabIndex = 5;
            this.tlpRoot.Controls.Add(this.dgvPaiements, 0, 6);
            this.dgvPaiements.TabIndex = 6;
            this.tlpRoot.Controls.Add(this.btnFermer, 0, 7);
            this.btnFermer.TabIndex = 7;
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
            // tlpEntete
            // 
            this.tlpEntete.ColumnCount = 4;
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpEntete.RowCount = 3;
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.Controls.Add(this.lblT1, 0, 0);
            this.lblT1.TabIndex = 0;
            this.tlpEntete.Controls.Add(this.lblDate, 1, 0);
            this.lblDate.TabIndex = 1;
            this.tlpEntete.Controls.Add(this.lblT2, 2, 0);
            this.lblT2.TabIndex = 2;
            this.tlpEntete.Controls.Add(this.lblClient, 3, 0);
            this.lblClient.TabIndex = 3;
            this.tlpEntete.Controls.Add(this.lblT3, 0, 1);
            this.lblT3.TabIndex = 4;
            this.tlpEntete.Controls.Add(this.lblTel, 1, 1);
            this.lblTel.TabIndex = 5;
            this.tlpEntete.Controls.Add(this.lblT4, 2, 1);
            this.lblT4.TabIndex = 6;
            this.tlpEntete.Controls.Add(this.lblVendeur, 3, 1);
            this.lblVendeur.TabIndex = 7;
            this.tlpEntete.Controls.Add(this.lblT5, 0, 2);
            this.lblT5.TabIndex = 8;
            this.tlpEntete.Controls.Add(this.lblMode, 1, 2);
            this.lblMode.TabIndex = 9;
            this.tlpEntete.Controls.Add(this.lblT6, 2, 2);
            this.lblT6.TabIndex = 10;
            this.tlpEntete.Controls.Add(this.lblStatut, 3, 2);
            this.lblStatut.TabIndex = 11;
            this.tlpEntete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntete.AutoSize = true;
            this.tlpEntete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEntete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpEntete.Name = "tlpEntete";
            // 
            // lblT1
            // 
            this.lblT1.Text = "Date";
            this.lblT1.AutoSize = true;
            this.lblT1.Tag = "note";
            this.lblT1.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT1.Name = "lblT1";
            // 
            // lblDate
            // 
            this.lblDate.Text = "";
            this.lblDate.AutoSize = true;
            this.lblDate.Margin = new System.Windows.Forms.Padding(0, 2, 24, 2);
            this.lblDate.Name = "lblDate";
            // 
            // lblT2
            // 
            this.lblT2.Text = "Client";
            this.lblT2.AutoSize = true;
            this.lblT2.Tag = "note";
            this.lblT2.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT2.Name = "lblT2";
            // 
            // lblClient
            // 
            this.lblClient.Text = "";
            this.lblClient.AutoSize = true;
            this.lblClient.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblClient.Name = "lblClient";
            // 
            // lblT3
            // 
            this.lblT3.Text = "Téléphone";
            this.lblT3.AutoSize = true;
            this.lblT3.Tag = "note";
            this.lblT3.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT3.Name = "lblT3";
            // 
            // lblTel
            // 
            this.lblTel.Text = "";
            this.lblTel.AutoSize = true;
            this.lblTel.Margin = new System.Windows.Forms.Padding(0, 2, 24, 2);
            this.lblTel.Name = "lblTel";
            // 
            // lblT4
            // 
            this.lblT4.Text = "Vendeur";
            this.lblT4.AutoSize = true;
            this.lblT4.Tag = "note";
            this.lblT4.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT4.Name = "lblT4";
            // 
            // lblVendeur
            // 
            this.lblVendeur.Text = "";
            this.lblVendeur.AutoSize = true;
            this.lblVendeur.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblVendeur.Name = "lblVendeur";
            // 
            // lblT5
            // 
            this.lblT5.Text = "Paiement";
            this.lblT5.AutoSize = true;
            this.lblT5.Tag = "note";
            this.lblT5.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT5.Name = "lblT5";
            // 
            // lblMode
            // 
            this.lblMode.Text = "";
            this.lblMode.AutoSize = true;
            this.lblMode.Margin = new System.Windows.Forms.Padding(0, 2, 24, 2);
            this.lblMode.Name = "lblMode";
            // 
            // lblT6
            // 
            this.lblT6.Text = "Statut";
            this.lblT6.AutoSize = true;
            this.lblT6.Tag = "note";
            this.lblT6.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            this.lblT6.Name = "lblT6";
            // 
            // lblStatut
            // 
            this.lblStatut.Text = "";
            this.lblStatut.AutoSize = true;
            this.lblStatut.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatut.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblStatut.Name = "lblStatut";
            // 
            // lblMotif
            // 
            this.lblMotif.Text = "";
            this.lblMotif.AutoSize = true;
            this.lblMotif.Tag = "note";
            this.lblMotif.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblMotif.Name = "lblMotif";
            // 
            // dgvLignes
            // 
            this.dgvLignes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduit,
            this.colUnite,
            this.colQuantite,
            this.colPrixU,
            this.colSousTotal});
            this.dgvLignes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLignes.AutoGenerateColumns = false;
            this.dgvLignes.ReadOnly = true;
            this.dgvLignes.Name = "dgvLignes";
            // 
            // flpTotaux
            // 
            this.flpTotaux.Controls.Add(this.lblTotal);
            this.lblTotal.TabIndex = 0;
            this.flpTotaux.Controls.Add(this.lblVerse);
            this.lblVerse.TabIndex = 1;
            this.flpTotaux.Controls.Add(this.lblRestant);
            this.lblRestant.TabIndex = 2;
            this.flpTotaux.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTotaux.AutoSize = true;
            this.flpTotaux.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTotaux.WrapContents = true;
            this.flpTotaux.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.flpTotaux.Name = "flpTotaux";
            // 
            // lblTotal
            // 
            this.lblTotal.Text = "";
            this.lblTotal.AutoSize = true;
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // lblMutuelle
            // 
            this.lblMutuelle.Text = "";
            this.lblMutuelle.AutoSize = true;
            this.lblMutuelle.Tag = "attention";
            this.lblMutuelle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblMutuelle.Name = "lblMutuelle";
            // 
            // dgvPaiements
            // 
            this.dgvPaiements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPaiementNumero,
            this.colPaiementDate,
            this.colPaiementMontant,
            this.colPaiementCaissier});
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
            // colProduit
            // 
            this.colProduit.HeaderText = "Produit";
            this.colProduit.Name = "Produit";
            this.colProduit.DataPropertyName = "Produit";
            this.colProduit.FillWeight = 40F;
            this.colProduit.MinimumWidth = 140;
            this.colProduit.ReadOnly = true;
            // 
            // colUnite
            // 
            this.colUnite.HeaderText = "Unité";
            this.colUnite.Name = "Unite";
            this.colUnite.DataPropertyName = "Unite";
            this.colUnite.FillWeight = 14F;
            this.colUnite.MinimumWidth = 80;
            this.colUnite.ReadOnly = true;
            // 
            // colQuantite
            // 
            this.colQuantite.HeaderText = "Quantité";
            this.colQuantite.Name = "Quantite";
            this.colQuantite.DataPropertyName = "Quantite";
            this.colQuantite.FillWeight = 12F;
            this.colQuantite.MinimumWidth = 70;
            this.colQuantite.ReadOnly = true;
            // 
            // colPrixU
            // 
            this.colPrixU.HeaderText = "Prix unitaire";
            this.colPrixU.Name = "PrixU";
            this.colPrixU.DataPropertyName = "PrixU";
            this.colPrixU.FillWeight = 17F;
            this.colPrixU.MinimumWidth = 100;
            this.colPrixU.ReadOnly = true;
            this.colPrixU.Tag = "montant";
            // 
            // colSousTotal
            // 
            this.colSousTotal.HeaderText = "Sous-total";
            this.colSousTotal.Name = "SousTotal";
            this.colSousTotal.DataPropertyName = "SousTotal";
            this.colSousTotal.FillWeight = 17F;
            this.colSousTotal.MinimumWidth = 100;
            this.colSousTotal.ReadOnly = true;
            this.colSousTotal.Tag = "montant";
            // 
            // colPaiementNumero
            // 
            this.colPaiementNumero.HeaderText = "Paiement";
            this.colPaiementNumero.Name = "Numero";
            this.colPaiementNumero.DataPropertyName = "Numero";
            this.colPaiementNumero.FillWeight = 26F;
            this.colPaiementNumero.MinimumWidth = 100;
            this.colPaiementNumero.ReadOnly = true;
            // 
            // colPaiementDate
            // 
            this.colPaiementDate.HeaderText = "Date";
            this.colPaiementDate.Name = "Date";
            this.colPaiementDate.DataPropertyName = "Date";
            this.colPaiementDate.FillWeight = 30F;
            this.colPaiementDate.MinimumWidth = 120;
            this.colPaiementDate.ReadOnly = true;
            // 
            // colPaiementMontant
            // 
            this.colPaiementMontant.HeaderText = "Montant";
            this.colPaiementMontant.Name = "Montant";
            this.colPaiementMontant.DataPropertyName = "Montant";
            this.colPaiementMontant.FillWeight = 24F;
            this.colPaiementMontant.MinimumWidth = 100;
            this.colPaiementMontant.ReadOnly = true;
            this.colPaiementMontant.Tag = "montant";
            // 
            // colPaiementCaissier
            // 
            this.colPaiementCaissier.HeaderText = "Encaissé par";
            this.colPaiementCaissier.Name = "Caissier";
            this.colPaiementCaissier.DataPropertyName = "Caissier";
            this.colPaiementCaissier.FillWeight = 26F;
            this.colPaiementCaissier.MinimumWidth = 100;
            this.colPaiementCaissier.ReadOnly = true;
            // 
            // FormDetailVente
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(840, 540);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Détail de la vente";
            this.AcceptButton = this.btnFermer;
            this.CancelButton = this.btnFermer;
            this.Name = "FormDetailVente";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpTotaux.ResumeLayout(false);
            this.flpTotaux.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaiements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLignes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblT1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblT2;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label lblT3;
        private System.Windows.Forms.Label lblTel;
        private System.Windows.Forms.Label lblT4;
        private System.Windows.Forms.Label lblVendeur;
        private System.Windows.Forms.Label lblT5;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblT6;
        private System.Windows.Forms.Label lblStatut;
        private System.Windows.Forms.Label lblMotif;
        private System.Windows.Forms.DataGridView dgvLignes;
        private System.Windows.Forms.FlowLayoutPanel flpTotaux;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblVerse;
        private System.Windows.Forms.Label lblRestant;
        private System.Windows.Forms.Label lblMutuelle;
        private System.Windows.Forms.DataGridView dgvPaiements;
        private System.Windows.Forms.Button btnFermer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrixU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSousTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaiementNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaiementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaiementMontant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaiementCaissier;
    }
}
