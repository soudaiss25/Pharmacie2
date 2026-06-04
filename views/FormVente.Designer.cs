namespace Pharmacie2.views
{
    partial class FormVente
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNom, lblPrenom, lblTelephone, lblMotif;
        private System.Windows.Forms.TextBox txtNom, txtPrenom, txtTelephone, txtMotif;
        private System.Windows.Forms.Label lblVendeur;
        private System.Windows.Forms.ListView lvProduits;
        private System.Windows.Forms.Button btnAjouter, btnSupprimer;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtMontantTotal;
        private System.Windows.Forms.Label lblPaiement;
        private System.Windows.Forms.ComboBox cbPaiement;
        private System.Windows.Forms.Label lblVerse;
        private System.Windows.Forms.TextBox txtMontantVerse;

        private System.Windows.Forms.Panel pnlMutuelle;
        private System.Windows.Forms.Label lblMutuelle, lblTaux, lblRestePatient, lblPartMutuelle;
        private System.Windows.Forms.ComboBox cbMutuelle;
        private System.Windows.Forms.TextBox txtTauxMutuelle;

        private System.Windows.Forms.Panel pnlMatricule;
        private System.Windows.Forms.Label lblMatricule;
        private System.Windows.Forms.TextBox txtMatricule;

        private System.Windows.Forms.Panel pnlEspeces;
        private System.Windows.Forms.Label lblEspeces;
        private System.Windows.Forms.TextBox txtMontantEspeces;
        private System.Windows.Forms.Label lblMontantRendu;

        private System.Windows.Forms.Button btnValider, btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNom = new System.Windows.Forms.Label();
            txtNom = new System.Windows.Forms.TextBox();
            lblPrenom = new System.Windows.Forms.Label();
            txtPrenom = new System.Windows.Forms.TextBox();
            lblTelephone = new System.Windows.Forms.Label();
            txtTelephone = new System.Windows.Forms.TextBox();
            lblMotif = new System.Windows.Forms.Label();
            txtMotif = new System.Windows.Forms.TextBox();
            lblVendeur = new System.Windows.Forms.Label();
            lvProduits = new System.Windows.Forms.ListView();
            btnAjouter = new System.Windows.Forms.Button();
            btnSupprimer = new System.Windows.Forms.Button();
            lblTotal = new System.Windows.Forms.Label();
            txtMontantTotal = new System.Windows.Forms.TextBox();
            lblPaiement = new System.Windows.Forms.Label();
            cbPaiement = new System.Windows.Forms.ComboBox();
            lblVerse = new System.Windows.Forms.Label();
            txtMontantVerse = new System.Windows.Forms.TextBox();
            pnlMutuelle = new System.Windows.Forms.Panel();
            lblMutuelle = new System.Windows.Forms.Label();
            cbMutuelle = new System.Windows.Forms.ComboBox();
            lblTaux = new System.Windows.Forms.Label();
            txtTauxMutuelle = new System.Windows.Forms.TextBox();
            lblRestePatient = new System.Windows.Forms.Label();
            lblPartMutuelle = new System.Windows.Forms.Label();
            pnlMatricule = new System.Windows.Forms.Panel();
            lblMatricule = new System.Windows.Forms.Label();
            txtMatricule = new System.Windows.Forms.TextBox();
            pnlEspeces = new System.Windows.Forms.Panel();
            lblEspeces = new System.Windows.Forms.Label();
            txtMontantEspeces = new System.Windows.Forms.TextBox();
            lblMontantRendu = new System.Windows.Forms.Label();
            btnValider = new System.Windows.Forms.Button();
            btnAnnuler = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ── Ligne 1 : Client ──────────────────────────────────────────
            lblNom.Text = "Nom :";
            lblNom.Location = new System.Drawing.Point(12, 14);
            lblNom.Size = new System.Drawing.Size(50, 20);
            txtNom.Location = new System.Drawing.Point(65, 11);
            txtNom.Size = new System.Drawing.Size(130, 26);

            lblPrenom.Text = "Prénom :";
            lblPrenom.Location = new System.Drawing.Point(205, 14);
            lblPrenom.Size = new System.Drawing.Size(60, 20);
            txtPrenom.Location = new System.Drawing.Point(270, 11);
            txtPrenom.Size = new System.Drawing.Size(120, 26);

            lblTelephone.Text = "Tél :";
            lblTelephone.Location = new System.Drawing.Point(400, 14);
            lblTelephone.Size = new System.Drawing.Size(35, 20);
            txtTelephone.Location = new System.Drawing.Point(438, 11);
            txtTelephone.Size = new System.Drawing.Size(120, 26);

            lblVendeur.Text = $"🧑‍💼 {Pharmacie2.Models.SessionUtilisateur.NomComplet}";
            lblVendeur.Location = new System.Drawing.Point(570, 14);
            lblVendeur.Size = new System.Drawing.Size(230, 20);
            lblVendeur.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            lblVendeur.ForeColor = System.Drawing.Color.DimGray;

            // ── Ligne 2 : Motif ───────────────────────────────────────────
            lblMotif.Text = "Motif / Ordonnance :";
            lblMotif.Location = new System.Drawing.Point(12, 46);
            lblMotif.Size = new System.Drawing.Size(140, 20);
            lblMotif.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblMotif.ForeColor = System.Drawing.Color.FromArgb(25, 118, 210);
            txtMotif.Location = new System.Drawing.Point(155, 43);
            txtMotif.Size = new System.Drawing.Size(650, 26);
            txtMotif.PlaceholderText = "Ex: Hypertension, grippe, ordonnance Dr. Ahmed…";

            // ── ListView ──────────────────────────────────────────────────
            lvProduits.Location = new System.Drawing.Point(12, 78);
            lvProduits.Size = new System.Drawing.Size(790, 190);
            lvProduits.View = System.Windows.Forms.View.Details;
            lvProduits.FullRowSelect = true;
            lvProduits.GridLines = true;

            btnAjouter.Text = "+ Ajouter médicament";
            btnAjouter.Location = new System.Drawing.Point(12, 278);
            btnAjouter.Size = new System.Drawing.Size(180, 34);
            btnAjouter.BackColor = System.Drawing.Color.MediumSeaGreen;
            btnAjouter.ForeColor = System.Drawing.Color.White;
            btnAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAjouter.FlatAppearance.BorderSize = 0;
            btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);

            btnSupprimer.Text = "✖ Supprimer ligne";
            btnSupprimer.Location = new System.Drawing.Point(200, 278);
            btnSupprimer.Size = new System.Drawing.Size(140, 34);
            btnSupprimer.BackColor = System.Drawing.Color.Tomato;
            btnSupprimer.ForeColor = System.Drawing.Color.White;
            btnSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);

            lblTotal.Text = "TOTAL :";
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblTotal.Location = new System.Drawing.Point(580, 280);
            lblTotal.Size = new System.Drawing.Size(80, 28);

            txtMontantTotal.Location = new System.Drawing.Point(662, 278);
            txtMontantTotal.Size = new System.Drawing.Size(140, 30);
            txtMontantTotal.ReadOnly = true;
            txtMontantTotal.BackColor = System.Drawing.Color.LightYellow;
            txtMontantTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            // ── Mode paiement ─────────────────────────────────────────────
            lblPaiement.Text = "Mode paiement :";
            lblPaiement.Location = new System.Drawing.Point(12, 324);
            lblPaiement.Size = new System.Drawing.Size(120, 22);

            cbPaiement.Location = new System.Drawing.Point(135, 321);
            cbPaiement.Size = new System.Drawing.Size(160, 28);
            cbPaiement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbPaiement.SelectedIndexChanged += new System.EventHandler(this.cbPaiement_SelectedIndexChanged);

            lblVerse.Text = "Montant versé :";
            lblVerse.Location = new System.Drawing.Point(310, 324);
            lblVerse.Size = new System.Drawing.Size(110, 22);

            txtMontantVerse.Location = new System.Drawing.Point(425, 321);
            txtMontantVerse.Size = new System.Drawing.Size(120, 26);
            txtMontantVerse.ReadOnly = true;
            txtMontantVerse.BackColor = System.Drawing.Color.AliceBlue;

            // ── Panel Matricule (ligne 2 après mode paiement) ─────────────
            pnlMatricule.Location = new System.Drawing.Point(12, 356);
            pnlMatricule.Size = new System.Drawing.Size(362, 35);
            pnlMatricule.Visible = false;
            pnlMatricule.BackColor = System.Drawing.Color.LightCyan;

            lblMatricule.Text = "Matricule employé :";
            lblMatricule.Location = new System.Drawing.Point(4, 8);
            lblMatricule.Size = new System.Drawing.Size(130, 20);

            txtMatricule.Location = new System.Drawing.Point(138, 5);
            txtMatricule.Size = new System.Drawing.Size(150, 26);
            txtMatricule.PlaceholderText = "N° matricule de l'employé…";
            pnlMatricule.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblMatricule, txtMatricule });

            // ── Panel Mutuelle (sous matricule) ───────────────────────────
            pnlMutuelle.Location = new System.Drawing.Point(12, 396);
            pnlMutuelle.Size = new System.Drawing.Size(790, 62);
            pnlMutuelle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlMutuelle.BackColor = System.Drawing.Color.LightCyan;
            pnlMutuelle.Visible = false;

            lblMutuelle.Text = "Mutuelle :";
            lblMutuelle.Location = new System.Drawing.Point(8, 10);
            lblMutuelle.Size = new System.Drawing.Size(70, 22);

            cbMutuelle.Location = new System.Drawing.Point(82, 7);
            cbMutuelle.Size = new System.Drawing.Size(200, 28);
            cbMutuelle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbMutuelle.SelectedIndexChanged += new System.EventHandler(this.cbMutuelle_SelectedIndexChanged);

            lblTaux.Text = "Taux (%) :";
            lblTaux.Location = new System.Drawing.Point(292, 10);
            lblTaux.Size = new System.Drawing.Size(70, 22);

            txtTauxMutuelle.Location = new System.Drawing.Point(366, 7);
            txtTauxMutuelle.Size = new System.Drawing.Size(60, 26);
            txtTauxMutuelle.ReadOnly = true;
            txtTauxMutuelle.BackColor = System.Drawing.Color.LightCyan;

            lblPartMutuelle.Text = "Pris en charge mutuelle : 0.00 KMF";
            lblPartMutuelle.Location = new System.Drawing.Point(8, 36);
            lblPartMutuelle.Size = new System.Drawing.Size(380, 20);
            lblPartMutuelle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblPartMutuelle.ForeColor = System.Drawing.Color.SteelBlue;

            lblRestePatient.Text = "À payer par le patient : 0.00 KMF";
            lblRestePatient.Location = new System.Drawing.Point(400, 36);
            lblRestePatient.Size = new System.Drawing.Size(380, 20);
            lblRestePatient.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblRestePatient.ForeColor = System.Drawing.Color.DarkRed;

            pnlMutuelle.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblMutuelle, cbMutuelle, lblTaux, txtTauxMutuelle,
                lblPartMutuelle, lblRestePatient });

            // ── Panel Espèces (toujours sous tout) ────────────────────────
            pnlEspeces.Location = new System.Drawing.Point(12, 466);
            pnlEspeces.Size = new System.Drawing.Size(590, 42);
            pnlEspeces.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlEspeces.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            pnlEspeces.Visible = false;

            lblEspeces.Text = "Espèces reçues :";
            lblEspeces.Location = new System.Drawing.Point(8, 10);
            lblEspeces.Size = new System.Drawing.Size(120, 22);

            txtMontantEspeces.Location = new System.Drawing.Point(132, 7);
            txtMontantEspeces.Size = new System.Drawing.Size(110, 26);
            txtMontantEspeces.TextChanged += new System.EventHandler(this.txtMontantEspeces_TextChanged);

            lblMontantRendu.Text = "Rendu : 0.00 KMF";
            lblMontantRendu.Location = new System.Drawing.Point(255, 10);
            lblMontantRendu.Size = new System.Drawing.Size(325, 22);
            lblMontantRendu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblMontantRendu.ForeColor = System.Drawing.Color.DarkGreen;

            pnlEspeces.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblEspeces, txtMontantEspeces, lblMontantRendu });

            // ── Boutons Valider / Annuler ─────────────────────────────────
            btnValider.Text = "✔ Valider la vente";
            btnValider.Location = new System.Drawing.Point(614, 522);
            btnValider.Size = new System.Drawing.Size(192, 38);
            btnValider.BackColor = System.Drawing.Color.ForestGreen;
            btnValider.ForeColor = System.Drawing.Color.White;
            btnValider.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnValider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnValider.FlatAppearance.BorderSize = 0;
            btnValider.Click += new System.EventHandler(this.btnValider_Click);

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new System.Drawing.Point(504, 522);
            btnAnnuler.Size = new System.Drawing.Size(100, 38);
            btnAnnuler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);

            // ── Form ──────────────────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(816, 572);
            this.Text = "Nouvelle Vente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNom, txtNom, lblPrenom, txtPrenom,
                lblTelephone, txtTelephone, lblMotif, txtMotif, lblVendeur,
                lvProduits, btnAjouter, btnSupprimer,
                lblTotal, txtMontantTotal,
                lblPaiement, cbPaiement, lblVerse, txtMontantVerse,
                pnlMatricule, pnlMutuelle, pnlEspeces,
                btnValider, btnAnnuler
            });

            this.ResumeLayout(false);
        }
    }
}