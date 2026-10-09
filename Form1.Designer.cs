namespace Pharmacie2
{
    partial class Form1
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
            this.panelLogin = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lnkOubli = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 3;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.Controls.Add(this.panelLogin, 1, 1);
            this.panelLogin.TabIndex = 0;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // panelLogin
            // 
            this.panelLogin.ColumnCount = 1;
            this.panelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowCount = 8;
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelLogin.Controls.Add(this.pictureBoxLogo, 0, 0);
            this.pictureBoxLogo.TabIndex = 0;
            this.panelLogin.Controls.Add(this.lblWelcome, 0, 1);
            this.lblWelcome.TabIndex = 1;
            this.panelLogin.Controls.Add(this.lblUsername, 0, 2);
            this.lblUsername.TabIndex = 2;
            this.panelLogin.Controls.Add(this.txtUsername, 0, 3);
            this.txtUsername.TabIndex = 3;
            this.panelLogin.Controls.Add(this.lblPassword, 0, 4);
            this.lblPassword.TabIndex = 4;
            this.panelLogin.Controls.Add(this.txtPassword, 0, 5);
            this.txtPassword.TabIndex = 5;
            this.panelLogin.Controls.Add(this.btnLogin, 0, 6);
            this.btnLogin.TabIndex = 6;
            this.panelLogin.Controls.Add(this.lnkOubli, 0, 7);
            this.lnkOubli.TabIndex = 7;
            this.panelLogin.AutoSize = true;
            this.panelLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelLogin.Padding = new System.Windows.Forms.Padding(24, 24, 24, 24);
            this.panelLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelLogin.Tag = "carte";
            this.panelLogin.Name = "panelLogin";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Size = new System.Drawing.Size(120, 80);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            // 
            // lblWelcome
            // 
            this.lblWelcome.Text = "Connexion";
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Tag = "titre";
            this.lblWelcome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblWelcome.Name = "lblWelcome";
            // 
            // lblUsername
            // 
            this.lblUsername.Text = "Nom d'utilisateur";
            this.lblUsername.AutoSize = true;
            this.lblUsername.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblUsername.Name = "lblUsername";
            // 
            // txtUsername
            // 
            this.txtUsername.Width = 300;
            this.txtUsername.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtUsername.Name = "txtUsername";
            // 
            // lblPassword
            // 
            this.lblPassword.Text = "Mot de passe";
            this.lblPassword.AutoSize = true;
            this.lblPassword.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblPassword.Name = "lblPassword";
            // 
            // txtPassword
            // 
            this.txtPassword.Width = 300;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.txtPassword.Name = "txtPassword";
            // 
            // btnLogin
            // 
            this.btnLogin.Text = "Se connecter";
            this.btnLogin.Tag = "primaire";
            this.btnLogin.Width = 300;
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lnkOubli
            // 
            this.lnkOubli.Text = "Mot de passe oublié ?";
            this.lnkOubli.AutoSize = true;
            this.lnkOubli.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkOubli.Name = "lnkOubli";
            this.lnkOubli.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOubli_LinkClicked);
            // 
            // Form1
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(480, 520);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connexion";
            this.AcceptButton = this.btnLogin;
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel panelLogin;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkOubli;
    }
}
