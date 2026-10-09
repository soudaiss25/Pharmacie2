namespace Pharmacie2.views
{
    partial class FormPeriodeExportMutuelle
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
            this.lblNomMutuelle = new System.Windows.Forms.Label();
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblChoixPeriode = new System.Windows.Forms.Label();
            this.cbPeriode = new System.Windows.Forms.ComboBox();
            this.lblDebut = new System.Windows.Forms.Label();
            this.dtpDebut = new System.Windows.Forms.DateTimePicker();
            this.lblFin = new System.Windows.Forms.Label();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExporter = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
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
            this.tlpRoot.Controls.Add(this.lblNomMutuelle, 0, 1);
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 2);
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 3);
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 4);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Export Excel : choix de la période";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblNomMutuelle
            // 
            this.lblNomMutuelle.Text = "";
            this.lblNomMutuelle.AutoSize = true;
            this.lblNomMutuelle.Tag = "section";
            this.lblNomMutuelle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblNomMutuelle.Name = "lblNomMutuelle";
            // 
            // tlpChamps
            // 
            this.tlpChamps.ColumnCount = 2;
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpChamps.RowCount = 3;
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.Controls.Add(this.lblChoixPeriode, 0, 0);
            this.tlpChamps.Controls.Add(this.cbPeriode, 1, 0);
            this.tlpChamps.Controls.Add(this.lblDebut, 0, 1);
            this.tlpChamps.Controls.Add(this.dtpDebut, 1, 1);
            this.tlpChamps.Controls.Add(this.lblFin, 0, 2);
            this.tlpChamps.Controls.Add(this.dtpFin, 1, 2);
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblChoixPeriode
            // 
            this.lblChoixPeriode.Text = "Raccourci";
            this.lblChoixPeriode.AutoSize = true;
            this.lblChoixPeriode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblChoixPeriode.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblChoixPeriode.Name = "lblChoixPeriode";
            // 
            // cbPeriode
            // 
            this.cbPeriode.Width = 220;
            this.cbPeriode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPeriode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbPeriode.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cbPeriode.Items.AddRange(new object[] {
            "Mois en cours",
            "Mois précédent",
            "Trimestre en cours",
            "Année en cours",
            "Personnalisé"});
            this.cbPeriode.Name = "cbPeriode";
            this.cbPeriode.SelectedIndexChanged += new System.EventHandler(this.CbPeriode_SelectedIndexChanged);
            // 
            // lblDebut
            // 
            this.lblDebut.Text = "Du";
            this.lblDebut.AutoSize = true;
            this.lblDebut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDebut.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblDebut.Name = "lblDebut";
            // 
            // dtpDebut
            // 
            this.dtpDebut.Width = 140;
            this.dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDebut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpDebut.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpDebut.Name = "dtpDebut";
            // 
            // lblFin
            // 
            this.lblFin.Text = "Au";
            this.lblFin.AutoSize = true;
            this.lblFin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFin.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblFin.Name = "lblFin";
            // 
            // dtpFin
            // 
            this.dtpFin.Width = 140;
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpFin.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpFin.Name = "dtpFin";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnExporter);
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnExporter
            // 
            this.btnExporter.Text = "Exporter";
            this.btnExporter.Tag = "primaire";
            this.btnExporter.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnExporter.Name = "btnExporter";
            this.btnExporter.Click += new System.EventHandler(this.btnExporter_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // FormPeriodeExportMutuelle
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(520, 330);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Période d'export";
            this.AcceptButton = this.btnExporter;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormPeriodeExportMutuelle";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpChamps.ResumeLayout(false);
            this.tlpChamps.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblNomMutuelle;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblChoixPeriode;
        private System.Windows.Forms.ComboBox cbPeriode;
        private System.Windows.Forms.Label lblDebut;
        private System.Windows.Forms.DateTimePicker dtpDebut;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnExporter;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
