namespace Pharmacie2.views
{
    partial class FormSessionCaisse
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
            this.lblEtat = new System.Windows.Forms.Label();
            this.lblInfoSession = new System.Windows.Forms.Label();
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblFond = new System.Windows.Forms.Label();
            this.numFond = new System.Windows.Forms.NumericUpDown();
            this.lblMontantReel = new System.Windows.Forms.Label();
            this.numMontantReel = new System.Windows.Forms.NumericUpDown();
            this.lblCommentaireLabel = new System.Windows.Forms.Label();
            this.txtCommentaire = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCloture = new System.Windows.Forms.Button();
            this.btnOuvrir = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numFond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMontantReel)).BeginInit();
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
            this.tlpRoot.RowCount = 6;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.tlpRoot.Controls.Add(this.lblEtat, 0, 1);
            this.tlpRoot.Controls.Add(this.lblInfoSession, 0, 2);
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 3);
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 4);
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 5);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Caisse";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblEtat
            // 
            this.lblEtat.Text = "";
            this.lblEtat.AutoSize = true;
            this.lblEtat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEtat.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblEtat.Name = "lblEtat";
            // 
            // lblInfoSession
            // 
            this.lblInfoSession.Text = "";
            this.lblInfoSession.AutoSize = true;
            this.lblInfoSession.Tag = "note";
            this.lblInfoSession.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblInfoSession.Name = "lblInfoSession";
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
            this.tlpChamps.Controls.Add(this.lblFond, 0, 0);
            this.tlpChamps.Controls.Add(this.numFond, 1, 0);
            this.tlpChamps.Controls.Add(this.lblMontantReel, 0, 1);
            this.tlpChamps.Controls.Add(this.numMontantReel, 1, 1);
            this.tlpChamps.Controls.Add(this.lblCommentaireLabel, 0, 2);
            this.tlpChamps.Controls.Add(this.txtCommentaire, 1, 2);
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblFond
            // 
            this.lblFond.Text = "Fond de caisse au départ";
            this.lblFond.AutoSize = true;
            this.lblFond.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFond.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblFond.Name = "lblFond";
            // 
            // numFond
            // 
            this.numFond.Width = 170;
            this.numFond.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numFond.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numFond.ThousandsSeparator = true;
            this.numFond.DecimalPlaces = 0;
            this.numFond.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFond.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numFond.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numFond.Name = "numFond";
            // 
            // lblMontantReel
            // 
            this.lblMontantReel.Text = "Montant compté en caisse";
            this.lblMontantReel.AutoSize = true;
            this.lblMontantReel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMontantReel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblMontantReel.Name = "lblMontantReel";
            // 
            // numMontantReel
            // 
            this.numMontantReel.Width = 170;
            this.numMontantReel.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numMontantReel.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0});
            this.numMontantReel.ThousandsSeparator = true;
            this.numMontantReel.DecimalPlaces = 0;
            this.numMontantReel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMontantReel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMontantReel.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numMontantReel.Name = "numMontantReel";
            // 
            // lblCommentaireLabel
            // 
            this.lblCommentaireLabel.Text = "Commentaire";
            this.lblCommentaireLabel.AutoSize = true;
            this.lblCommentaireLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCommentaireLabel.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblCommentaireLabel.Name = "lblCommentaireLabel";
            // 
            // txtCommentaire
            // 
            this.txtCommentaire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommentaire.MaximumSize = new System.Drawing.Size(400, 0);
            this.txtCommentaire.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtCommentaire.Name = "txtCommentaire";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnCloture);
            this.flpBoutons.Controls.Add(this.btnOuvrir);
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnCloture
            // 
            this.btnCloture.Text = "Clôturer la caisse";
            this.btnCloture.Tag = "danger";
            this.btnCloture.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnCloture.Name = "btnCloture";
            this.btnCloture.Click += new System.EventHandler(this.btnCloture_Click);
            // 
            // btnOuvrir
            // 
            this.btnOuvrir.Text = "Ouvrir la caisse";
            this.btnOuvrir.Tag = "primaire";
            this.btnOuvrir.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnOuvrir.Name = "btnOuvrir";
            this.btnOuvrir.Click += new System.EventHandler(this.btnOuvrir_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Fermer";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // FormSessionCaisse
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(560, 440);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Session de caisse";
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormSessionCaisse";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpChamps.ResumeLayout(false);
            this.tlpChamps.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMontantReel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFond)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblEtat;
        private System.Windows.Forms.Label lblInfoSession;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblFond;
        private System.Windows.Forms.NumericUpDown numFond;
        private System.Windows.Forms.Label lblMontantReel;
        private System.Windows.Forms.NumericUpDown numMontantReel;
        private System.Windows.Forms.Label lblCommentaireLabel;
        private System.Windows.Forms.TextBox txtCommentaire;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnCloture;
        private System.Windows.Forms.Button btnOuvrir;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
