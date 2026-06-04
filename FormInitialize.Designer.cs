namespace Pharmacie2
{
    partial class FormInitialize
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCreate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitre = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ---------------- FORM ----------------
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 380);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initialisation - Création Admin";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // ---------------- TITRE ----------------
            this.lblTitre.Text = "Création du compte Administrateur";
            this.lblTitre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitre.AutoSize = true;
            this.lblTitre.Location = new System.Drawing.Point(40, 20);

            // ---------------- NOM ----------------
            this.txtNom.PlaceholderText = "Nom";
            this.txtNom.Location = new System.Drawing.Point(50, 70);
            this.txtNom.Size = new System.Drawing.Size(300, 27);

            // ---------------- PRENOM ----------------
            this.txtPrenom.PlaceholderText = "Prénom";
            this.txtPrenom.Location = new System.Drawing.Point(50, 110);
            this.txtPrenom.Size = new System.Drawing.Size(300, 27);

            // ---------------- LOGIN ----------------
            this.txtLogin.PlaceholderText = "Login";
            this.txtLogin.Location = new System.Drawing.Point(50, 150);
            this.txtLogin.Size = new System.Drawing.Size(300, 27);

            // ---------------- PASSWORD ----------------
            this.txtPassword.PlaceholderText = "Mot de passe";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Location = new System.Drawing.Point(50, 190);
            this.txtPassword.Size = new System.Drawing.Size(300, 27);

            // ---------------- BOUTON ----------------
            this.btnCreate.Text = "Créer Administrateur";
            this.btnCreate.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Location = new System.Drawing.Point(50, 240);
            this.btnCreate.Size = new System.Drawing.Size(300, 40);
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);

            // ---------------- ADD CONTROLS ----------------
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.txtNom);
            this.Controls.Add(this.txtPrenom);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnCreate);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}