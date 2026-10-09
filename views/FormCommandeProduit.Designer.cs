namespace Pharmacie2.views
{
    partial class FormCommandeProduit
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
            this.lblProduit = new System.Windows.Forms.Label();
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblFournisseurLabel = new System.Windows.Forms.Label();
            this.txtFournisseur = new System.Windows.Forms.TextBox();
            this.lblFournisseurInfo = new System.Windows.Forms.Label();
            this.lblQuantiteLabel = new System.Windows.Forms.Label();
            this.numQuantite = new System.Windows.Forms.NumericUpDown();
            this.lblPrixUnitaireLabel = new System.Windows.Forms.Label();
            this.numPrixUnitaire = new System.Windows.Forms.NumericUpDown();
            this.chkDateLivraison = new System.Windows.Forms.CheckBox();
            this.dtpLivraisonPrevue = new System.Windows.Forms.DateTimePicker();
            this.lblNoteLabel = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixUnitaire)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpChamps.SuspendLayout();
            this.panelEspace.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.lblProduit, 0, 1);
            this.lblProduit.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 2);
            this.tlpChamps.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 3);
            this.panelEspace.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 4);
            this.flpBoutons.TabIndex = 4;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Passer une commande";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblProduit
            // 
            this.lblProduit.Text = "Produit : —";
            this.lblProduit.AutoSize = true;
            this.lblProduit.Tag = "section";
            this.lblProduit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblProduit.Name = "lblProduit";
            // 
            // tlpChamps
            // 
            this.tlpChamps.ColumnCount = 2;
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpChamps.RowCount = 6;
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.Controls.Add(this.lblFournisseurLabel, 0, 0);
            this.lblFournisseurLabel.TabIndex = 0;
            this.tlpChamps.Controls.Add(this.txtFournisseur, 1, 0);
            this.txtFournisseur.TabIndex = 1;
            this.tlpChamps.Controls.Add(this.lblFournisseurInfo, 1, 1);
            this.lblFournisseurInfo.TabIndex = 2;
            this.tlpChamps.Controls.Add(this.lblQuantiteLabel, 0, 2);
            this.lblQuantiteLabel.TabIndex = 3;
            this.tlpChamps.Controls.Add(this.numQuantite, 1, 2);
            this.numQuantite.TabIndex = 4;
            this.tlpChamps.Controls.Add(this.lblPrixUnitaireLabel, 0, 3);
            this.lblPrixUnitaireLabel.TabIndex = 5;
            this.tlpChamps.Controls.Add(this.numPrixUnitaire, 1, 3);
            this.numPrixUnitaire.TabIndex = 6;
            this.tlpChamps.Controls.Add(this.chkDateLivraison, 0, 4);
            this.chkDateLivraison.TabIndex = 7;
            this.tlpChamps.Controls.Add(this.dtpLivraisonPrevue, 1, 4);
            this.dtpLivraisonPrevue.TabIndex = 8;
            this.tlpChamps.Controls.Add(this.lblNoteLabel, 0, 5);
            this.lblNoteLabel.TabIndex = 9;
            this.tlpChamps.Controls.Add(this.txtNote, 1, 5);
            this.txtNote.TabIndex = 10;
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblFournisseurLabel
            // 
            this.lblFournisseurLabel.Text = "Fournisseur *";
            this.lblFournisseurLabel.AutoSize = true;
            this.lblFournisseurLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFournisseurLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblFournisseurLabel.Name = "lblFournisseurLabel";
            // 
            // txtFournisseur
            // 
            this.txtFournisseur.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtFournisseur.Width = 380;
            this.txtFournisseur.PlaceholderText = "Nom du fournisseur…";
            this.txtFournisseur.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtFournisseur.Name = "txtFournisseur";
            this.txtFournisseur.Leave += new System.EventHandler(this.txtFournisseur_Leave);
            // 
            // lblFournisseurInfo
            // 
            this.lblFournisseurInfo.Text = "";
            this.lblFournisseurInfo.AutoSize = true;
            this.lblFournisseurInfo.Visible = false;
            this.lblFournisseurInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblFournisseurInfo.Name = "lblFournisseurInfo";
            // 
            // lblQuantiteLabel
            // 
            this.lblQuantiteLabel.Text = "Quantité (boîtes) *";
            this.lblQuantiteLabel.AutoSize = true;
            this.lblQuantiteLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblQuantiteLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblQuantiteLabel.Name = "lblQuantiteLabel";
            // 
            // numQuantite
            // 
            this.numQuantite.Width = 100;
            this.numQuantite.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.numQuantite.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numQuantite.Value = new decimal(new int[] { 1, 0, 0, 0});
            this.numQuantite.ThousandsSeparator = true;
            this.numQuantite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numQuantite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numQuantite.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numQuantite.Name = "numQuantite";
            // 
            // lblPrixUnitaireLabel
            // 
            this.lblPrixUnitaireLabel.Text = "Prix d'achat unitaire (facultatif)";
            this.lblPrixUnitaireLabel.AutoSize = true;
            this.lblPrixUnitaireLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrixUnitaireLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblPrixUnitaireLabel.Name = "lblPrixUnitaireLabel";
            // 
            // numPrixUnitaire
            // 
            this.numPrixUnitaire.Width = 130;
            this.numPrixUnitaire.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numPrixUnitaire.Maximum = new decimal(new int[] { 100000000, 0, 0, 0});
            this.numPrixUnitaire.ThousandsSeparator = true;
            this.numPrixUnitaire.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPrixUnitaire.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numPrixUnitaire.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numPrixUnitaire.Name = "numPrixUnitaire";
            // 
            // chkDateLivraison
            // 
            this.chkDateLivraison.Text = "Livraison prévue le";
            this.chkDateLivraison.AutoSize = true;
            this.chkDateLivraison.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDateLivraison.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.chkDateLivraison.Name = "chkDateLivraison";
            this.chkDateLivraison.CheckedChanged += new System.EventHandler(this.chkDateLivraison_CheckedChanged);
            // 
            // dtpLivraisonPrevue
            // 
            this.dtpLivraisonPrevue.Width = 140;
            this.dtpLivraisonPrevue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLivraisonPrevue.Enabled = false;
            this.dtpLivraisonPrevue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpLivraisonPrevue.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpLivraisonPrevue.Name = "dtpLivraisonPrevue";
            // 
            // lblNoteLabel
            // 
            this.lblNoteLabel.Text = "Note";
            this.lblNoteLabel.AutoSize = true;
            this.lblNoteLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNoteLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNoteLabel.Name = "lblNoteLabel";
            // 
            // txtNote
            // 
            this.txtNote.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNote.Width = 380;
            this.txtNote.PlaceholderText = "Ex : urgence, référence du fournisseur…";
            this.txtNote.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNote.Name = "txtNote";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
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
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnValider
            // 
            this.btnValider.Text = "Enregistrer la commande";
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
            // FormCommandeProduit
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(600, 440);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Commande fournisseur";
            this.AcceptButton = this.btnValider;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormCommandeProduit";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpChamps.ResumeLayout(false);
            this.tlpChamps.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrixUnitaire)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantite)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblProduit;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblFournisseurLabel;
        private System.Windows.Forms.TextBox txtFournisseur;
        private System.Windows.Forms.Label lblFournisseurInfo;
        private System.Windows.Forms.Label lblQuantiteLabel;
        private System.Windows.Forms.NumericUpDown numQuantite;
        private System.Windows.Forms.Label lblPrixUnitaireLabel;
        private System.Windows.Forms.NumericUpDown numPrixUnitaire;
        private System.Windows.Forms.CheckBox chkDateLivraison;
        private System.Windows.Forms.DateTimePicker dtpLivraisonPrevue;
        private System.Windows.Forms.Label lblNoteLabel;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
