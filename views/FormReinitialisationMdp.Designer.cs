namespace Pharmacie2.views
{
    partial class FormReinitialisationMdp
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
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblCle = new System.Windows.Forms.Label();
            this.txtCle = new System.Windows.Forms.TextBox();
            this.lblMdp = new System.Windows.Forms.Label();
            this.txtMdp = new System.Windows.Forms.TextBox();
            this.lblMdp2 = new System.Windows.Forms.Label();
            this.txtMdp2 = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.tlpRoot.SuspendLayout();
            this.panelEspace.SuspendLayout();
            this.flpBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 11;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.lblLogin, 0, 1);
            this.lblLogin.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.txtLogin, 0, 2);
            this.txtLogin.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.lblCle, 0, 3);
            this.lblCle.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.txtCle, 0, 4);
            this.txtCle.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.lblMdp, 0, 5);
            this.lblMdp.TabIndex = 5;
            this.tlpRoot.Controls.Add(this.txtMdp, 0, 6);
            this.txtMdp.TabIndex = 6;
            this.tlpRoot.Controls.Add(this.lblMdp2, 0, 7);
            this.lblMdp2.TabIndex = 7;
            this.tlpRoot.Controls.Add(this.txtMdp2, 0, 8);
            this.txtMdp2.TabIndex = 8;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 9);
            this.panelEspace.TabIndex = 9;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 10);
            this.flpBoutons.TabIndex = 10;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Mot de passe oublié";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblLogin
            // 
            this.lblLogin.Text = "Identifiant de l'administrateur";
            this.lblLogin.AutoSize = true;
            this.lblLogin.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblLogin.Name = "lblLogin";
            // 
            // txtLogin
            // 
            this.txtLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogin.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtLogin.Name = "txtLogin";
            // 
            // lblCle
            // 
            this.lblCle.Text = "Clé de secours (exemple : K7MQ-4XRT-9WPA-H3ZD)";
            this.lblCle.AutoSize = true;
            this.lblCle.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblCle.Name = "lblCle";
            // 
            // txtCle
            // 
            this.txtCle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCle.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtCle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtCle.Name = "txtCle";
            // 
            // lblMdp
            // 
            this.lblMdp.Text = "Nouveau mot de passe";
            this.lblMdp.AutoSize = true;
            this.lblMdp.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblMdp.Name = "lblMdp";
            // 
            // txtMdp
            // 
            this.txtMdp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMdp.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtMdp.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtMdp.UseSystemPasswordChar = true;
            this.txtMdp.Name = "txtMdp";
            // 
            // lblMdp2
            // 
            this.lblMdp2.Text = "Confirmer le nouveau mot de passe";
            this.lblMdp2.AutoSize = true;
            this.lblMdp2.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblMdp2.Name = "lblMdp2";
            // 
            // txtMdp2
            // 
            this.txtMdp2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMdp2.MaximumSize = new System.Drawing.Size(420, 0);
            this.txtMdp2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtMdp2.UseSystemPasswordChar = true;
            this.txtMdp2.Name = "txtMdp2";
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
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnValider
            // 
            this.btnValider.Text = "Valider";
            this.btnValider.Tag = "primaire";
            this.btnValider.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnValider.Name = "btnValider";
            this.btnValider.Click += new System.EventHandler(this.btnValider_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.Name = "btnAnnuler";
            // 
            // FormReinitialisationMdp
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(480, 460);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mot de passe oublié";
            this.AcceptButton = this.btnValider;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormReinitialisationMdp";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblCle;
        private System.Windows.Forms.TextBox txtCle;
        private System.Windows.Forms.Label lblMdp;
        private System.Windows.Forms.TextBox txtMdp;
        private System.Windows.Forms.Label lblMdp2;
        private System.Windows.Forms.TextBox txtMdp2;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
