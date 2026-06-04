using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormModificationVente
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblNom, lblPrenom, lblTelephone, lblMotif, lblMatriculeLabel;
        private TextBox txtNom, txtPrenom, txtTelephone, txtMotif, txtMatricule;
        private ListView lvProduits;
        private Button btnAjouter, btnSupprimer;
        private Label lblTotal;
        private TextBox txtMontantTotal;
        private Label lblPaiement;
        private ComboBox cbPaiement;

        // Panel mutuelle compact (mutuelle + matricule sur la même ligne)
        private Panel pnlMutuelle;
        private Label lblMutuelle;
        private ComboBox cbMutuelle;
        private Panel pnlMatricule;
        private Label lblMatriculeLabel2;

        private Button btnEnregistrer, btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNom = new Label(); txtNom = new TextBox();
            lblPrenom = new Label(); txtPrenom = new TextBox();
            lblTelephone = new Label(); txtTelephone = new TextBox();
            lblMotif = new Label(); txtMotif = new TextBox();
            lblMatriculeLabel = new Label(); txtMatricule = new TextBox();
            lvProduits = new ListView();
            btnAjouter = new Button(); btnSupprimer = new Button();
            lblTotal = new Label(); txtMontantTotal = new TextBox();
            lblPaiement = new Label(); cbPaiement = new ComboBox();
            pnlMutuelle = new Panel();
            lblMutuelle = new Label(); cbMutuelle = new ComboBox();
            pnlMatricule = new Panel();
            lblMatriculeLabel2 = new Label();
            btnEnregistrer = new Button(); btnAnnuler = new Button();

            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            var lblTitre = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                Text = "✏️  MODIFICATION DE VENTE",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White
            };

            // ── Ligne 1 : Client ───────────────────────────────────────────
            lblNom.Text = "Nom :"; lblNom.Location = new Point(12, 64); lblNom.Size = new Size(40, 20);
            txtNom.Location = new Point(55, 61); txtNom.Size = new Size(130, 26);

            lblPrenom.Text = "Prénom :"; lblPrenom.Location = new Point(196, 64); lblPrenom.Size = new Size(55, 20);
            txtPrenom.Location = new Point(254, 61); txtPrenom.Size = new Size(120, 26);

            lblTelephone.Text = "Tél :"; lblTelephone.Location = new Point(384, 64); lblTelephone.Size = new Size(30, 20);
            txtTelephone.Location = new Point(416, 61); txtTelephone.Size = new Size(120, 26);

            // ── Ligne 2 : Motif ────────────────────────────────────────────
            lblMotif.Text = "Motif :"; lblMotif.Location = new Point(12, 96); lblMotif.Size = new Size(42, 20);
            lblMotif.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMotif.ForeColor = Color.FromArgb(25, 118, 210);
            txtMotif.Location = new Point(55, 93); txtMotif.Size = new Size(650, 26);
            txtMotif.PlaceholderText = "Motif ou ordonnance…";

            // ── ListView produits ──────────────────────────────────────────
            lvProduits.Location = new Point(12, 128);
            lvProduits.Size = new Size(790, 175);
            lvProduits.View = View.Details;
            lvProduits.FullRowSelect = true;
            lvProduits.GridLines = true;
            lvProduits.Columns.Add("Produit", 160);
            lvProduits.Columns.Add("Unité", 70);
            lvProduits.Columns.Add("Qté", 50);
            lvProduits.Columns.Add("Prix U.", 90);
            lvProduits.Columns.Add("Sous-total", 90);

            // ── Boutons + total ────────────────────────────────────────────
            btnAjouter.Text = "+ Ajouter produit";
            btnAjouter.Location = new Point(12, 312);
            btnAjouter.Size = new Size(155, 30);
            btnAjouter.BackColor = Color.MediumSeaGreen;
            btnAjouter.ForeColor = Color.White;
            btnAjouter.FlatStyle = FlatStyle.Flat;
            btnAjouter.FlatAppearance.BorderSize = 0;
            btnAjouter.Click += btnAjouter_Click;

            btnSupprimer.Text = "✖ Supprimer ligne";
            btnSupprimer.Location = new Point(176, 312);
            btnSupprimer.Size = new Size(140, 30);
            btnSupprimer.BackColor = Color.Tomato;
            btnSupprimer.ForeColor = Color.White;
            btnSupprimer.FlatStyle = FlatStyle.Flat;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            btnSupprimer.Click += btnSupprimer_Click;

            lblTotal.Text = "TOTAL :";
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(570, 314);
            lblTotal.Size = new Size(70, 26);

            txtMontantTotal.Location = new Point(644, 312);
            txtMontantTotal.Size = new Size(158, 30);
            txtMontantTotal.ReadOnly = true;
            txtMontantTotal.BackColor = Color.LightYellow;
            txtMontantTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // ── Mode paiement ──────────────────────────────────────────────
            lblPaiement.Text = "Mode :";
            lblPaiement.Location = new Point(12, 352);
            lblPaiement.Size = new Size(45, 22);

            cbPaiement.Location = new Point(60, 349);
            cbPaiement.Size = new Size(150, 28);
            cbPaiement.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPaiement.SelectedIndexChanged += cbPaiement_SelectedIndexChanged;

            // ── Panel Mutuelle — COMPACT, même ligne que mode paiement ─────
            // Contient : label "Mutuelle :" + combobox + label "Matricule :" + textbox
            pnlMutuelle.Location = new Point(220, 344);
            pnlMutuelle.Size = new Size(580, 36);
            pnlMutuelle.BackColor = Color.FromArgb(225, 245, 254);
            pnlMutuelle.BorderStyle = BorderStyle.FixedSingle;
            pnlMutuelle.Visible = false;

            lblMutuelle.Text = "Mutuelle :";
            lblMutuelle.Location = new Point(4, 9);
            lblMutuelle.Size = new Size(65, 20);
            lblMutuelle.Font = new Font("Segoe UI", 9F);

            cbMutuelle.Location = new Point(72, 6);
            cbMutuelle.Size = new Size(190, 26);
            cbMutuelle.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMutuelle.Font = new Font("Segoe UI", 9F);

            lblMatriculeLabel.Text = "Matricule :";
            lblMatriculeLabel.Location = new Point(272, 9);
            lblMatriculeLabel.Size = new Size(68, 20);
            lblMatriculeLabel.Font = new Font("Segoe UI", 9F);

            txtMatricule.Location = new Point(344, 5);
            txtMatricule.Size = new Size(160, 26);
            txtMatricule.PlaceholderText = "N° matricule…";
            txtMatricule.Font = new Font("Segoe UI", 9F);

            pnlMutuelle.Controls.AddRange(new Control[] {
                lblMutuelle, cbMutuelle, lblMatriculeLabel, txtMatricule });

            // ── Boutons enregistrer / annuler ──────────────────────────────
            btnEnregistrer.Text = "✔ Enregistrer";
            btnEnregistrer.Location = new Point(540, 392);
            btnEnregistrer.Size = new Size(140, 36);
            btnEnregistrer.BackColor = Color.FromArgb(25, 118, 210);
            btnEnregistrer.ForeColor = Color.White;
            btnEnregistrer.FlatStyle = FlatStyle.Flat;
            btnEnregistrer.FlatAppearance.BorderSize = 0;
            btnEnregistrer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEnregistrer.Click += btnEnregistrer_Click;

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(690, 392);
            btnAnnuler.Size = new Size(110, 36);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Font = new Font("Segoe UI", 9F);
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Assemblage ─────────────────────────────────────────────────
            ClientSize = new Size(820, 446);
            Text = "Modification de vente";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            Controls.AddRange(new Control[] {
                lblTitre,
                lblNom, txtNom, lblPrenom, txtPrenom, lblTelephone, txtTelephone,
                lblMotif, txtMotif,
                lvProduits,
                btnAjouter, btnSupprimer,
                lblTotal, txtMontantTotal,
                lblPaiement, cbPaiement,
                pnlMutuelle,
                btnEnregistrer, btnAnnuler
            });

            ResumeLayout(false);
        }
    }
}