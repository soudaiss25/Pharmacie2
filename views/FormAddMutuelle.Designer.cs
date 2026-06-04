namespace Pharmacie2.views
{
    partial class FormAddMutuelle
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNom, lblTaux, lblEmail, lblTelephone, lblExplTaux;
        private System.Windows.Forms.TextBox txtNomEmployeur, txtTaux, txtEmail, txtTelephone;
        private System.Windows.Forms.Button btnEnregistrer, btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNom = new System.Windows.Forms.Label();
            txtNomEmployeur = new System.Windows.Forms.TextBox();
            lblTaux = new System.Windows.Forms.Label();
            txtTaux = new System.Windows.Forms.TextBox();
            lblExplTaux = new System.Windows.Forms.Label();
            lblEmail = new System.Windows.Forms.Label();
            txtEmail = new System.Windows.Forms.TextBox();
            lblTelephone = new System.Windows.Forms.Label();
            txtTelephone = new System.Windows.Forms.TextBox();
            btnEnregistrer = new System.Windows.Forms.Button();
            btnAnnuler = new System.Windows.Forms.Button();

            this.SuspendLayout();

            int lx = 12, tx = 200, w = 260, rh = 42;

            lblNom.Text = "Nom employeur / mutuelle *"; lblNom.Location = new System.Drawing.Point(lx, 18); lblNom.Size = new System.Drawing.Size(185, 22);
            txtNomEmployeur.Location = new System.Drawing.Point(tx, 15); txtNomEmployeur.Size = new System.Drawing.Size(w, 26);

            lblTaux.Text = "Taux prise en charge (%) *"; lblTaux.Location = new System.Drawing.Point(lx, 18 + rh); lblTaux.Size = new System.Drawing.Size(185, 22);
            txtTaux.Location = new System.Drawing.Point(tx, 15 + rh); txtTaux.Size = new System.Drawing.Size(80, 26); txtTaux.Text = "0";

            lblExplTaux.Text = "Ex. : 80 = mutuelle paie 80%, patient paie 20%";
            lblExplTaux.Location = new System.Drawing.Point(lx, 18 + 2 * rh - 15);
            lblExplTaux.Size = new System.Drawing.Size(460, 20);
            lblExplTaux.ForeColor = System.Drawing.Color.Gray;
            lblExplTaux.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);

            lblEmail.Text = "Email contact"; lblEmail.Location = new System.Drawing.Point(lx, 18 + 2 * rh + 5); lblEmail.Size = new System.Drawing.Size(185, 22);
            txtEmail.Location = new System.Drawing.Point(tx, 15 + 2 * rh + 5); txtEmail.Size = new System.Drawing.Size(w, 26);

            lblTelephone.Text = "Téléphone"; lblTelephone.Location = new System.Drawing.Point(lx, 18 + 3 * rh + 5); lblTelephone.Size = new System.Drawing.Size(185, 22);
            txtTelephone.Location = new System.Drawing.Point(tx, 15 + 3 * rh + 5); txtTelephone.Size = new System.Drawing.Size(180, 26);

            btnEnregistrer.Text = "✔ Enregistrer";
            btnEnregistrer.Location = new System.Drawing.Point(270, 18 + 5 * rh - 20);
            btnEnregistrer.Size = new System.Drawing.Size(130, 34);
            btnEnregistrer.BackColor = System.Drawing.Color.ForestGreen;
            btnEnregistrer.ForeColor = System.Drawing.Color.White;
            btnEnregistrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEnregistrer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new System.Drawing.Point(160, 18 + 5 * rh - 20);
            btnAnnuler.Size = new System.Drawing.Size(100, 34);
            btnAnnuler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);

            this.ClientSize = new System.Drawing.Size(480, 18 + 6 * rh);
            this.Text = "Mutuelle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNom, txtNomEmployeur,
                lblTaux, txtTaux, lblExplTaux,
                lblEmail, txtEmail,
                lblTelephone, txtTelephone,
                btnEnregistrer, btnAnnuler
            });

            this.ResumeLayout(false);
        }
    }
}