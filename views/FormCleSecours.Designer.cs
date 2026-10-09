namespace Pharmacie2.views
{
    partial class FormCleSecours
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
            this.lblCle = new System.Windows.Forms.Label();
            this.lblConsigne = new System.Windows.Forms.Label();
            this.btnImprimer = new System.Windows.Forms.Button();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.tlpBas = new System.Windows.Forms.TableLayoutPanel();
            this.chkNote = new System.Windows.Forms.CheckBox();
            this.btnContinuer = new System.Windows.Forms.Button();
            this.tlpRoot.SuspendLayout();
            this.panelEspace.SuspendLayout();
            this.tlpBas.SuspendLayout();
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
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.lblCle, 0, 1);
            this.lblCle.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.lblConsigne, 0, 2);
            this.lblConsigne.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.btnImprimer, 0, 3);
            this.btnImprimer.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 4);
            this.panelEspace.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.tlpBas, 0, 5);
            this.tlpBas.TabIndex = 5;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Votre clé de secours";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblCle
            // 
            this.lblCle.Text = "";
            this.lblCle.AutoSize = true;
            this.lblCle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCle.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.lblCle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCle.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblCle.Name = "lblCle";
            // 
            // lblConsigne
            // 
            this.lblConsigne.Text = "Notez cette clé sur papier et rangez-la en lieu sûr. Elle permet de retrouver l'accès si vous oubliez votre mot de passe.\n\nElle ne sera plus jamais affichée.";
            this.lblConsigne.AutoSize = true;
            this.lblConsigne.MaximumSize = new System.Drawing.Size(560, 0);
            this.lblConsigne.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblConsigne.Name = "lblConsigne";
            // 
            // btnImprimer
            // 
            this.btnImprimer.Text = "Imprimer la clé";
            this.btnImprimer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnImprimer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnImprimer.Name = "btnImprimer";
            this.btnImprimer.Click += new System.EventHandler(this.btnImprimer_Click);
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // tlpBas
            // 
            this.tlpBas.ColumnCount = 2;
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.RowCount = 1;
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.Controls.Add(this.chkNote, 0, 0);
            this.chkNote.TabIndex = 0;
            this.tlpBas.Controls.Add(this.btnContinuer, 1, 0);
            this.btnContinuer.TabIndex = 1;
            this.tlpBas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBas.AutoSize = true;
            this.tlpBas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBas.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpBas.Name = "tlpBas";
            // 
            // chkNote
            // 
            this.chkNote.Text = "J'ai noté la clé";
            this.chkNote.AutoSize = true;
            this.chkNote.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkNote.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.chkNote.Name = "chkNote";
            this.chkNote.CheckedChanged += new System.EventHandler(this.chkNote_CheckedChanged);
            // 
            // btnContinuer
            // 
            this.btnContinuer.Text = "Continuer";
            this.btnContinuer.Enabled = false;
            this.btnContinuer.Tag = "primaire";
            this.btnContinuer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnContinuer.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinuer.Name = "btnContinuer";
            // 
            // FormCleSecours
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(620, 440);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clé de secours";
            this.AcceptButton = this.btnContinuer;
            this.Name = "FormCleSecours";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.tlpBas.ResumeLayout(false);
            this.tlpBas.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblCle;
        private System.Windows.Forms.Label lblConsigne;
        private System.Windows.Forms.Button btnImprimer;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.TableLayoutPanel tlpBas;
        private System.Windows.Forms.CheckBox chkNote;
        private System.Windows.Forms.Button btnContinuer;
    }
}
