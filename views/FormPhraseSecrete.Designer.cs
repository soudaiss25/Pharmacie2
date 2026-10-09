namespace Pharmacie2.views
{
    partial class FormPhraseSecrete
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
            this.lblPhrase = new System.Windows.Forms.Label();
            this.txtPhrase = new System.Windows.Forms.TextBox();
            this.tlpConfirmation = new System.Windows.Forms.TableLayoutPanel();
            this.lblConfirmation = new System.Windows.Forms.Label();
            this.txtConfirmation = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.tlpRoot.SuspendLayout();
            this.tlpConfirmation.SuspendLayout();
            this.panelEspace.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 7;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.lblInfo, 0, 1);
            this.lblInfo.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblPhrase, 0, 2);
            this.lblPhrase.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.txtPhrase, 0, 3);
            this.txtPhrase.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.tlpConfirmation, 0, 4);
            this.tlpConfirmation.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 5);
            this.panelEspace.TabIndex = 5;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 6);
            this.flpBoutons.TabIndex = 6;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Phrase secrète";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblInfo
            // 
            this.lblInfo.Text = "";
            this.lblInfo.AutoSize = true;
            this.lblInfo.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblInfo.Name = "lblInfo";
            // 
            // lblPhrase
            // 
            this.lblPhrase.Text = "Phrase secrète";
            this.lblPhrase.AutoSize = true;
            this.lblPhrase.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.lblPhrase.Name = "lblPhrase";
            // 
            // txtPhrase
            // 
            this.txtPhrase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhrase.UseSystemPasswordChar = true;
            this.txtPhrase.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.txtPhrase.Name = "txtPhrase";
            // 
            // tlpConfirmation
            // 
            this.tlpConfirmation.ColumnCount = 1;
            this.tlpConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpConfirmation.RowCount = 2;
            this.tlpConfirmation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpConfirmation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpConfirmation.Controls.Add(this.lblConfirmation, 0, 0);
            this.lblConfirmation.TabIndex = 0;
            this.tlpConfirmation.Controls.Add(this.txtConfirmation, 0, 1);
            this.txtConfirmation.TabIndex = 1;
            this.tlpConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpConfirmation.AutoSize = true;
            this.tlpConfirmation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpConfirmation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpConfirmation.Name = "tlpConfirmation";
            // 
            // lblConfirmation
            // 
            this.lblConfirmation.Text = "Confirmez la phrase secrète";
            this.lblConfirmation.AutoSize = true;
            this.lblConfirmation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.lblConfirmation.Name = "lblConfirmation";
            // 
            // txtConfirmation
            // 
            this.txtConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfirmation.UseSystemPasswordChar = true;
            this.txtConfirmation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.txtConfirmation.Name = "txtConfirmation";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnOk);
            this.btnOk.TabIndex = 0;
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
            // btnOk
            // 
            this.btnOk.Text = "Valider";
            this.btnOk.Tag = "primaire";
            this.btnOk.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.Name = "btnAnnuler";
            // 
            // FormPhraseSecrete
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(560, 330);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phrase secrète";
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormPhraseSecrete";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpConfirmation.ResumeLayout(false);
            this.tlpConfirmation.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblPhrase;
        private System.Windows.Forms.TextBox txtPhrase;
        private System.Windows.Forms.TableLayoutPanel tlpConfirmation;
        private System.Windows.Forms.Label lblConfirmation;
        private System.Windows.Forms.TextBox txtConfirmation;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
