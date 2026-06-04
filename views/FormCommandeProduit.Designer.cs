using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormCommandeProduit
    {
        private System.ComponentModel.IContainer components = null;

        // ── Contrôles ─────────────────────────────────────────────────────
        private Label lblTitre;
        private Label lblProduit;
        private Label lblFournisseurLabel;
        private TextBox txtFournisseur;
        private Label lblFournisseurInfo;
        private Label lblQuantiteLabel;
        private NumericUpDown numQuantite;
        private Label lblPrixUnitaireLabel;
        private NumericUpDown numPrixUnitaire;
        private CheckBox chkDateLivraison;
        private DateTimePicker dtpLivraisonPrevue;
        private Label lblNoteLabel;
        private TextBox txtNote;
        private Button btnValider;
        private Button btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            lblProduit = new Label();
            lblFournisseurLabel = new Label();
            txtFournisseur = new TextBox();
            lblFournisseurInfo = new Label();
            lblQuantiteLabel = new Label();
            numQuantite = new NumericUpDown();
            lblPrixUnitaireLabel = new Label();
            numPrixUnitaire = new NumericUpDown();
            chkDateLivraison = new CheckBox();
            dtpLivraisonPrevue = new DateTimePicker();
            lblNoteLabel = new Label();
            txtNote = new TextBox();
            btnValider = new Button();
            btnAnnuler = new Button();

            ((System.ComponentModel.ISupportInitialize)numQuantite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrixUnitaire).BeginInit();
            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Height = 58;
            lblTitre.Text = "🛒  PASSER UNE COMMANDE";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.BackColor = Color.FromArgb(230, 120, 0);
            lblTitre.ForeColor = Color.White;

            // ── Nom du produit ─────────────────────────────────────────────
            lblProduit.Location = new Point(20, 72);
            lblProduit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblProduit.ForeColor = Color.FromArgb(34, 85, 34);
            lblProduit.Size = new Size(440, 28);
            lblProduit.Text = "Produit : —";

            // ── Fournisseur ────────────────────────────────────────────────
            lblFournisseurLabel.Text = "Fournisseur *";
            lblFournisseurLabel.Location = new Point(20, 114);
            lblFournisseurLabel.Size = new Size(110, 22);
            lblFournisseurLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            txtFournisseur.Location = new Point(138, 111);
            txtFournisseur.Size = new Size(300, 28);
            txtFournisseur.Font = new Font("Segoe UI", 10F);
            txtFournisseur.PlaceholderText = "Nom du fournisseur…";
            txtFournisseur.Leave += txtFournisseur_Leave;

            lblFournisseurInfo.Location = new Point(138, 142);
            lblFournisseurInfo.Size = new Size(320, 20);
            lblFournisseurInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblFournisseurInfo.ForeColor = Color.DimGray;
            lblFournisseurInfo.Visible = false;
            lblFournisseurInfo.Text = "";

            // ── Quantité ───────────────────────────────────────────────────
            lblQuantiteLabel.Text = "Quantité (boîtes) *";
            lblQuantiteLabel.Location = new Point(20, 170);
            lblQuantiteLabel.Size = new Size(140, 22);
            lblQuantiteLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            numQuantite.Location = new Point(168, 167);
            numQuantite.Minimum = 1;
            numQuantite.Maximum = 100000;
            numQuantite.Value = 1;
            numQuantite.Size = new Size(120, 28);
            numQuantite.Font = new Font("Segoe UI", 10F);
            numQuantite.ThousandsSeparator = true;

            // ── Prix unitaire (optionnel) ──────────────────────────────────
            lblPrixUnitaireLabel.Text = "Prix achat unitaire (optionnel) :";
            lblPrixUnitaireLabel.Location = new Point(20, 208);
            lblPrixUnitaireLabel.Size = new Size(220, 22);
            lblPrixUnitaireLabel.Font = new Font("Segoe UI", 9F);
            lblPrixUnitaireLabel.ForeColor = Color.DimGray;

            numPrixUnitaire.Location = new Point(250, 205);
            numPrixUnitaire.Minimum = 0;
            numPrixUnitaire.Maximum = 10000000;
            numPrixUnitaire.DecimalPlaces = 2;
            numPrixUnitaire.Size = new Size(150, 28);
            numPrixUnitaire.Font = new Font("Segoe UI", 10F);
            numPrixUnitaire.ThousandsSeparator = true;

            // ── Date livraison prévue (optionnelle) ────────────────────────
            chkDateLivraison.Text = "Date de livraison prévue :";
            chkDateLivraison.Location = new Point(20, 246);
            chkDateLivraison.Size = new Size(200, 22);
            chkDateLivraison.Font = new Font("Segoe UI", 9F);
            chkDateLivraison.CheckedChanged += chkDateLivraison_CheckedChanged;

            dtpLivraisonPrevue.Location = new Point(228, 243);
            dtpLivraisonPrevue.Size = new Size(160, 28);
            dtpLivraisonPrevue.Format = DateTimePickerFormat.Short;
            dtpLivraisonPrevue.Enabled = false;
            dtpLivraisonPrevue.Font = new Font("Segoe UI", 9.5F);

            // ── Note (optionnelle) ─────────────────────────────────────────
            lblNoteLabel.Text = "Note / commentaire :";
            lblNoteLabel.Location = new Point(20, 280);
            lblNoteLabel.Size = new Size(150, 22);
            lblNoteLabel.Font = new Font("Segoe UI", 9F);
            lblNoteLabel.ForeColor = Color.DimGray;

            txtNote.Location = new Point(20, 304);
            txtNote.Size = new Size(440, 52);
            txtNote.Multiline = true;
            txtNote.Font = new Font("Segoe UI", 9F);
            txtNote.PlaceholderText = "Ex : urgence, référence fournisseur, conditions particulières…";

            // ── Bouton Valider ─────────────────────────────────────────────
            btnValider.Text = "✔ Valider la commande";
            btnValider.Location = new Point(240, 372);
            btnValider.Size = new Size(180, 38);
            btnValider.BackColor = Color.FromArgb(230, 120, 0);
            btnValider.ForeColor = Color.White;
            btnValider.FlatStyle = FlatStyle.Flat;
            btnValider.FlatAppearance.BorderSize = 0;
            btnValider.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnValider.UseVisualStyleBackColor = false;
            btnValider.Click += btnValider_Click;

            // ── Bouton Annuler ─────────────────────────────────────────────
            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(100, 372);
            btnAnnuler.Size = new Size(130, 38);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Font = new Font("Segoe UI", 9.5F);
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Form ───────────────────────────────────────────────────────
            BackColor = Color.White;
            ClientSize = new Size(480, 426);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Commande fournisseur";

            Controls.Add(lblTitre);
            Controls.Add(lblProduit);
            Controls.Add(lblFournisseurLabel);
            Controls.Add(txtFournisseur);
            Controls.Add(lblFournisseurInfo);
            Controls.Add(lblQuantiteLabel);
            Controls.Add(numQuantite);
            Controls.Add(lblPrixUnitaireLabel);
            Controls.Add(numPrixUnitaire);
            Controls.Add(chkDateLivraison);
            Controls.Add(dtpLivraisonPrevue);
            Controls.Add(lblNoteLabel);
            Controls.Add(txtNote);
            Controls.Add(btnValider);
            Controls.Add(btnAnnuler);

            ((System.ComponentModel.ISupportInitialize)numQuantite).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrixUnitaire).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}