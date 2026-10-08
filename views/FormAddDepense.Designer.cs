namespace Pharmacie2.views
{
    partial class FormAddDepense
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
            this.lblTitreHeader = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblCategorie = new System.Windows.Forms.Label();
            this.cbCategorie = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblMontant = new System.Windows.Forms.Label();
            this.numMontant = new System.Windows.Forms.NumericUpDown();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numMontant)).BeginInit();
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
            this.tlpRoot.Controls.Add(this.lblTitreHeader, 0, 0);
            this.tlpRoot.Controls.Add(this.lblNote, 0, 1);
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 2);
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 3);
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 4);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitreHeader
            // 
            this.lblTitreHeader.Text = "Nouvelle dépense";
            this.lblTitreHeader.AutoSize = true;
            this.lblTitreHeader.Tag = "titre";
            this.lblTitreHeader.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitreHeader.Name = "lblTitreHeader";
            // 
            // lblNote
            // 
            this.lblNote.Text = "Salaires, factures, loyer, fournitures : elles servent à calculer le bénéfice.";
            this.lblNote.AutoSize = true;
            this.lblNote.Tag = "note";
            this.lblNote.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblNote.Name = "lblNote";
            // 
            // tlpChamps
            // 
            this.tlpChamps.ColumnCount = 2;
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpChamps.RowCount = 4;
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.Controls.Add(this.lblCategorie, 0, 0);
            this.tlpChamps.Controls.Add(this.cbCategorie, 1, 0);
            this.tlpChamps.Controls.Add(this.lblDescription, 0, 1);
            this.tlpChamps.Controls.Add(this.txtDescription, 1, 1);
            this.tlpChamps.Controls.Add(this.lblMontant, 0, 2);
            this.tlpChamps.Controls.Add(this.numMontant, 1, 2);
            this.tlpChamps.Controls.Add(this.lblDate, 0, 3);
            this.tlpChamps.Controls.Add(this.dtpDate, 1, 3);
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblCategorie
            // 
            this.lblCategorie.Text = "Catégorie";
            this.lblCategorie.AutoSize = true;
            this.lblCategorie.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCategorie.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblCategorie.Name = "lblCategorie";
            // 
            // cbCategorie
            // 
            this.cbCategorie.Width = 220;
            this.cbCategorie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategorie.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbCategorie.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbCategorie.Items.AddRange(new object[] {
            "Salaire",
            "Facture",
            "Loyer",
            "Fournitures",
            "Autre"});
            this.cbCategorie.Name = "cbCategorie";
            this.cbCategorie.SelectedIndexChanged += new System.EventHandler(this.cbCategorie_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Text = "Description";
            this.lblDescription.AutoSize = true;
            this.lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblDescription.Name = "lblDescription";
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtDescription.Name = "txtDescription";
            // 
            // lblMontant
            // 
            this.lblMontant.Text = "Montant (KMF)";
            this.lblMontant.AutoSize = true;
            this.lblMontant.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMontant.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMontant.Name = "lblMontant";
            // 
            // numMontant
            // 
            this.numMontant.Width = 170;
            this.numMontant.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numMontant.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numMontant.ThousandsSeparator = true;
            this.numMontant.DecimalPlaces = 0;
            this.numMontant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMontant.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMontant.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numMontant.Name = "numMontant";
            // 
            // lblDate
            // 
            this.lblDate.Text = "Date";
            this.lblDate.AutoSize = true;
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDate.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblDate.Name = "lblDate";
            // 
            // dtpDate
            // 
            this.dtpDate.Width = 150;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpDate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpDate.Name = "dtpDate";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnEnregistrer);
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnEnregistrer
            // 
            this.btnEnregistrer.Text = "Enregistrer";
            this.btnEnregistrer.Tag = "primaire";
            this.btnEnregistrer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // FormAddDepense
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(560, 400);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nouvelle dépense";
            this.AcceptButton = this.btnEnregistrer;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormAddDepense";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpChamps.ResumeLayout(false);
            this.tlpChamps.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMontant)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitreHeader;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblCategorie;
        private System.Windows.Forms.ComboBox cbCategorie;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblMontant;
        private System.Windows.Forms.NumericUpDown numMontant;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
