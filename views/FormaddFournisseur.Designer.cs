namespace Pharmacie2.views
{
    partial class FormaddFournisseur
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblContact = new System.Windows.Forms.Label();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
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
            this.tlpRoot.RowCount = 4;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitle, 0, 0);
            this.lblTitle.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 1);
            this.tlpChamps.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 2);
            this.panelEspace.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 3);
            this.flpBoutons.TabIndex = 3;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Nouveau fournisseur";
            this.lblTitle.AutoSize = true;
            this.lblTitle.Tag = "titre";
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitle.Name = "lblTitle";
            // 
            // tlpChamps
            // 
            this.tlpChamps.ColumnCount = 2;
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpChamps.RowCount = 2;
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.Controls.Add(this.lblNom, 0, 0);
            this.lblNom.TabIndex = 0;
            this.tlpChamps.Controls.Add(this.txtNom, 1, 0);
            this.txtNom.TabIndex = 1;
            this.tlpChamps.Controls.Add(this.lblContact, 0, 1);
            this.lblContact.TabIndex = 2;
            this.tlpChamps.Controls.Add(this.txtContact, 1, 1);
            this.txtContact.TabIndex = 3;
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom du fournisseur *";
            this.lblNom.AutoSize = true;
            this.lblNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNom.Name = "lblNom";
            // 
            // txtNom
            // 
            this.txtNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNom.Width = 380;
            this.txtNom.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNom.Name = "txtNom";
            // 
            // lblContact
            // 
            this.lblContact.Text = "Contact (téléphone, e-mail)";
            this.lblContact.AutoSize = true;
            this.lblContact.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContact.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblContact.Name = "lblContact";
            // 
            // txtContact
            // 
            this.txtContact.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtContact.Width = 380;
            this.txtContact.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtContact.Name = "txtContact";
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
            this.btnEnregistrer.TabIndex = 0;
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
            // FormaddFournisseur
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(560, 280);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fournisseur";
            this.AcceptButton = this.btnEnregistrer;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormaddFournisseur";
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
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtContact;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
