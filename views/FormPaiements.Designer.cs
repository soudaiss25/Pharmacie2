namespace Pharmacie2.views
{
    partial class FormPaiements
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNumero, lblTotal, lblVerse, lblRestant;
        private System.Windows.Forms.Label lblVendeurVente, lblMotif;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblSaisie;
        private System.Windows.Forms.TextBox txtMontant;
        private System.Windows.Forms.Button btnAjouterPaiement, btnFermer;
        private System.Windows.Forms.ListView lvPaiements;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNumero = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            lblVerse = new System.Windows.Forms.Label();
            lblRestant = new System.Windows.Forms.Label();
            lblVendeurVente = new System.Windows.Forms.Label();
            lblMotif = new System.Windows.Forms.Label();
            pnlInfo = new System.Windows.Forms.Panel();
            lblSaisie = new System.Windows.Forms.Label();
            txtMontant = new System.Windows.Forms.TextBox();
            btnAjouterPaiement = new System.Windows.Forms.Button();
            btnFermer = new System.Windows.Forms.Button();
            lvPaiements = new System.Windows.Forms.ListView();

            pnlInfo.SuspendLayout();
            this.SuspendLayout();

            // Panel résumé vente
            pnlInfo.Location = new System.Drawing.Point(12, 12);
            pnlInfo.Size = new System.Drawing.Size(576, 115);
            pnlInfo.BackColor = System.Drawing.Color.AliceBlue;
            pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblNumero.Text = "Vente : -";
            lblNumero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblNumero.Location = new System.Drawing.Point(8, 8); lblNumero.Size = new System.Drawing.Size(280, 22);

            lblTotal.Text = "Montant total : -";
            lblTotal.Location = new System.Drawing.Point(8, 32); lblTotal.Size = new System.Drawing.Size(250, 20);
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F);

            lblVerse.Text = "Déjà versé : -";
            lblVerse.Location = new System.Drawing.Point(270, 32); lblVerse.Size = new System.Drawing.Size(250, 20);
            lblVerse.Font = new System.Drawing.Font("Segoe UI", 9F);

            lblRestant.Text = "Reste à payer : -";
            lblRestant.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblRestant.Location = new System.Drawing.Point(8, 56); lblRestant.Size = new System.Drawing.Size(280, 22);
            lblRestant.ForeColor = System.Drawing.Color.OrangeRed;

            // Vendeur + Motif
            lblVendeurVente.Text = "Vendeur : —";
            lblVendeurVente.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            lblVendeurVente.ForeColor = System.Drawing.Color.DimGray;
            lblVendeurVente.Location = new System.Drawing.Point(8, 82); lblVendeurVente.Size = new System.Drawing.Size(250, 20);

            lblMotif.Text = "Motif : —";
            lblMotif.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            lblMotif.ForeColor = System.Drawing.Color.FromArgb(25, 118, 210);
            lblMotif.Location = new System.Drawing.Point(270, 82); lblMotif.Size = new System.Drawing.Size(300, 20);

            pnlInfo.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNumero, lblTotal, lblVerse, lblRestant, lblVendeurVente, lblMotif });

            // Saisie paiement
            lblSaisie.Text = "Montant à encaisser (KMF) :";
            lblSaisie.Location = new System.Drawing.Point(12, 140); lblSaisie.Size = new System.Drawing.Size(200, 22);
            lblSaisie.Font = new System.Drawing.Font("Segoe UI", 9F);

            txtMontant.Location = new System.Drawing.Point(218, 137); txtMontant.Size = new System.Drawing.Size(130, 28);
            txtMontant.Font = new System.Drawing.Font("Segoe UI", 10F);

            btnAjouterPaiement.Text = "✔ Enregistrer paiement";
            btnAjouterPaiement.Location = new System.Drawing.Point(358, 134);
            btnAjouterPaiement.Size = new System.Drawing.Size(190, 34);
            btnAjouterPaiement.BackColor = System.Drawing.Color.ForestGreen;
            btnAjouterPaiement.ForeColor = System.Drawing.Color.White;
            btnAjouterPaiement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAjouterPaiement.FlatAppearance.BorderSize = 0;
            btnAjouterPaiement.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAjouterPaiement.Click += new System.EventHandler(this.btnAjouterPaiement_Click);

            // ListView paiements — avec colonne Caissier
            lvPaiements.Location = new System.Drawing.Point(12, 178);
            lvPaiements.Size = new System.Drawing.Size(576, 220);
            lvPaiements.View = System.Windows.Forms.View.Details;
            lvPaiements.FullRowSelect = true;
            lvPaiements.GridLines = true;
            lvPaiements.Columns.Add("N° Paiement", 130);
            lvPaiements.Columns.Add("Date", 140);
            lvPaiements.Columns.Add("Montant", 110);
            lvPaiements.Columns.Add("Encaissé par", 180);   // ← nouveau

            btnFermer.Text = "Fermer";
            btnFermer.Location = new System.Drawing.Point(466, 408);
            btnFermer.Size = new System.Drawing.Size(122, 34);
            btnFermer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnFermer.Click += new System.EventHandler(this.btnFermer_Click);

            pnlInfo.ResumeLayout(false);

            this.ClientSize = new System.Drawing.Size(605, 455);
            this.Text = "Paiements — Crédit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                pnlInfo, lblSaisie, txtMontant, btnAjouterPaiement,
                lvPaiements, btnFermer
            });

            this.ResumeLayout(false);
        }
    }
}