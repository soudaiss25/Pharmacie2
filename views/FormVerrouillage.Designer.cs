namespace Pharmacie2.views
{
    partial class FormVerrouillage
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
            this.panelCarte = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblUtilisateur = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtMotDePasse = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnDeverrouiller = new System.Windows.Forms.Button();
            this.btnChangerUtilisateur = new System.Windows.Forms.Button();
            this.lnkOubli = new System.Windows.Forms.LinkLabel();
            this.tlpRoot.SuspendLayout();
            this.panelCarte.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.panelCarte, 1, 1);
            this.panelCarte.TabIndex = 0;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // panelCarte
            // 
            this.panelCarte.ColumnCount = 1;
            this.panelCarte.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowCount = 8;
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelCarte.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.panelCarte.Controls.Add(this.lblUtilisateur, 0, 1);
            this.lblUtilisateur.TabIndex = 1;
            this.panelCarte.Controls.Add(this.lblInfo, 0, 2);
            this.lblInfo.TabIndex = 2;
            this.panelCarte.Controls.Add(this.txtMotDePasse, 0, 3);
            this.txtMotDePasse.TabIndex = 3;
            this.panelCarte.Controls.Add(this.lblMessage, 0, 4);
            this.lblMessage.TabIndex = 4;
            this.panelCarte.Controls.Add(this.btnDeverrouiller, 0, 5);
            this.btnDeverrouiller.TabIndex = 5;
            this.panelCarte.Controls.Add(this.btnChangerUtilisateur, 0, 6);
            this.btnChangerUtilisateur.TabIndex = 6;
            this.panelCarte.Controls.Add(this.lnkOubli, 0, 7);
            this.lnkOubli.TabIndex = 7;
            this.panelCarte.AutoSize = true;
            this.panelCarte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelCarte.Padding = new System.Windows.Forms.Padding(28, 28, 28, 28);
            this.panelCarte.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelCarte.Tag = "carte";
            this.panelCarte.Name = "panelCarte";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Écran verrouillé";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblUtilisateur
            // 
            this.lblUtilisateur.Text = "";
            this.lblUtilisateur.AutoSize = true;
            this.lblUtilisateur.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblUtilisateur.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtilisateur.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.lblUtilisateur.Name = "lblUtilisateur";
            // 
            // lblInfo
            // 
            this.lblInfo.Text = "Saisissez votre mot de passe pour reprendre. Rien n'est perdu : le travail en cours est conservé.";
            this.lblInfo.AutoSize = true;
            this.lblInfo.MaximumSize = new System.Drawing.Size(320, 0);
            this.lblInfo.Tag = "note";
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblInfo.Name = "lblInfo";
            // 
            // txtMotDePasse
            // 
            this.txtMotDePasse.Width = 320;
            this.txtMotDePasse.UseSystemPasswordChar = true;
            this.txtMotDePasse.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.txtMotDePasse.Name = "txtMotDePasse";
            // 
            // lblMessage
            // 
            this.lblMessage.Text = "";
            this.lblMessage.AutoSize = true;
            this.lblMessage.MaximumSize = new System.Drawing.Size(320, 0);
            this.lblMessage.Tag = "urgent";
            this.lblMessage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblMessage.Name = "lblMessage";
            // 
            // btnDeverrouiller
            // 
            this.btnDeverrouiller.Text = "Déverrouiller";
            this.btnDeverrouiller.Tag = "primaire";
            this.btnDeverrouiller.Width = 320;
            this.btnDeverrouiller.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnDeverrouiller.Name = "btnDeverrouiller";
            this.btnDeverrouiller.Click += new System.EventHandler(this.btnDeverrouiller_Click);
            // 
            // btnChangerUtilisateur
            // 
            this.btnChangerUtilisateur.Text = "Changer d'utilisateur";
            this.btnChangerUtilisateur.Width = 320;
            this.btnChangerUtilisateur.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnChangerUtilisateur.Name = "btnChangerUtilisateur";
            this.btnChangerUtilisateur.Click += new System.EventHandler(this.btnChangerUtilisateur_Click);
            // 
            // lnkOubli
            // 
            this.lnkOubli.Text = "Mot de passe oublié ?";
            this.lnkOubli.AutoSize = true;
            this.lnkOubli.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkOubli.Name = "lnkOubli";
            this.lnkOubli.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOubli_LinkClicked);
            // 
            // FormVerrouillage
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(640, 520);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Écran verrouillé";
            this.AcceptButton = this.btnDeverrouiller;
            this.Name = "FormVerrouillage";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.panelCarte.ResumeLayout(false);
            this.panelCarte.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel panelCarte;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblUtilisateur;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtMotDePasse;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnDeverrouiller;
        private System.Windows.Forms.Button btnChangerUtilisateur;
        private System.Windows.Forms.LinkLabel lnkOubli;
    }
}
