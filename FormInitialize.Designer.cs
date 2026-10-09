namespace Pharmacie2
{
    partial class FormInitialize
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
            this.panelCreation = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblPrenom = new System.Windows.Forms.Label();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.tlpRoot.SuspendLayout();
            this.panelCreation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 3;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowCount = 4;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.Controls.Add(this.panelCreation, 1, 1);
            this.panelCreation.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.btnCreate, 1, 2);
            this.btnCreate.TabIndex = 1;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // panelCreation
            // 
            this.panelCreation.ColumnCount = 1;
            this.panelCreation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowCount = 10;
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCreation.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.panelCreation.Controls.Add(this.lblInfo, 0, 1);
            this.lblInfo.TabIndex = 1;
            this.panelCreation.Controls.Add(this.lblNom, 0, 2);
            this.lblNom.TabIndex = 2;
            this.panelCreation.Controls.Add(this.txtNom, 0, 3);
            this.txtNom.TabIndex = 3;
            this.panelCreation.Controls.Add(this.lblPrenom, 0, 4);
            this.lblPrenom.TabIndex = 4;
            this.panelCreation.Controls.Add(this.txtPrenom, 0, 5);
            this.txtPrenom.TabIndex = 5;
            this.panelCreation.Controls.Add(this.lblLogin, 0, 6);
            this.lblLogin.TabIndex = 6;
            this.panelCreation.Controls.Add(this.txtLogin, 0, 7);
            this.txtLogin.TabIndex = 7;
            this.panelCreation.Controls.Add(this.lblPassword, 0, 8);
            this.lblPassword.TabIndex = 8;
            this.panelCreation.Controls.Add(this.txtPassword, 0, 9);
            this.txtPassword.TabIndex = 9;
            this.panelCreation.AutoSize = true;
            this.panelCreation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelCreation.Padding = new System.Windows.Forms.Padding(24, 24, 24, 24);
            this.panelCreation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelCreation.Tag = "carte";
            this.panelCreation.Name = "panelCreation";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Création du compte administrateur";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblInfo
            // 
            this.lblInfo.Text = "C'est le premier lancement : créez le compte qui gérera le logiciel.";
            this.lblInfo.AutoSize = true;
            this.lblInfo.MaximumSize = new System.Drawing.Size(320, 0);
            this.lblInfo.Tag = "note";
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblInfo.Name = "lblInfo";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom";
            this.lblNom.AutoSize = true;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblNom.Name = "lblNom";
            // 
            // txtNom
            // 
            this.txtNom.Width = 320;
            this.txtNom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtNom.Name = "txtNom";
            // 
            // lblPrenom
            // 
            this.lblPrenom.Text = "Prénom";
            this.lblPrenom.AutoSize = true;
            this.lblPrenom.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblPrenom.Name = "lblPrenom";
            // 
            // txtPrenom
            // 
            this.txtPrenom.Width = 320;
            this.txtPrenom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtPrenom.Name = "txtPrenom";
            // 
            // lblLogin
            // 
            this.lblLogin.Text = "Identifiant de connexion";
            this.lblLogin.AutoSize = true;
            this.lblLogin.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblLogin.Name = "lblLogin";
            // 
            // txtLogin
            // 
            this.txtLogin.Width = 320;
            this.txtLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtLogin.Name = "txtLogin";
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
            this.txtPassword.Width = 320;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.txtPassword.Name = "txtPassword";
            // 
            // btnCreate
            // 
            this.btnCreate.Text = "Créer l'administrateur";
            this.btnCreate.Tag = "primaire";
            this.btnCreate.Width = 320;
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCreate.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // FormInitialize
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(480, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Premier lancement";
            this.AcceptButton = this.btnCreate;
            this.Name = "FormInitialize";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.panelCreation.ResumeLayout(false);
            this.panelCreation.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel panelCreation;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblPrenom;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCreate;
    }
}
