using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormAddProduit
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Déclarations ──────────────────────────────────────────────
            groupBoxProduit = new GroupBox();
            groupBoxDetail = new GroupBox();
            pnlVenteDetail = new Panel();

            lblNom = new Label(); txtNom = new TextBox();
            lblType = new Label(); cmbType = new ComboBox();
            lblPrixAchat = new Label(); txtPrixAchat = new TextBox();
            lblMarge = new Label(); txtMarge = new TextBox();
            lblPrixVente = new Label(); txtPrixVente = new TextBox();
            lblQuantite = new Label(); numQuantite = new NumericUpDown();
            lblUnitesVrac = new Label(); numUnitesVrac = new NumericUpDown();
            lblSeuil = new Label(); numSeuil = new NumericUpDown();
            lblDateExpiration = new Label(); dtpDateExpiration = new DateTimePicker();
            lblFournisseur = new Label(); cbFournisseur = new ComboBox();

            // Vente en détail
            chkVenteDetail = new CheckBox();
            lblUniteVente = new Label(); cmbUniteVente = new ComboBox();
            lblNbUnite = new Label(); numNbUniteParBoite = new NumericUpDown();
            lblApercu = new Label();

            btnEnregistrer = new Button();
            btnAnnuler = new Button();
            groupBoxMedicament = new GroupBox();
            lblIndication = new Label(); txtIndication = new TextBox();
            lblPosologie = new Label(); txtPosologie = new TextBox();
            lblPosologieJour = new Label(); numPosologieJour = new NumericUpDown();

            ((System.ComponentModel.ISupportInitialize)numQuantite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnitesVrac).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSeuil).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numNbUniteParBoite).BeginInit();
            groupBoxProduit.SuspendLayout();
            groupBoxDetail.SuspendLayout();
            pnlVenteDetail.SuspendLayout();
            SuspendLayout();

            // ══════════════════════════════════════════════════════════════
            // GROUPBOX PRINCIPAL — Informations produit
            // ══════════════════════════════════════════════════════════════
            int lx = 20, tx = 210, rh = 46;

            void Row(Label l, string t, Control c, int row, int cw = 460)
            {
                l.Text = t;
                l.Location = new Point(lx, row * rh + 28);
                l.Size = new Size(185, 22);
                l.Font = new Font("Segoe UI", 9F);
                c.Location = new Point(tx, row * rh + 25);
                c.Size = new Size(cw, 28);
            }

            Row(lblNom, "Nom du produit *", txtNom, 1);
            txtNom.PlaceholderText = "Ex : Doliprane 500mg, Amoxicilline…";
            txtNom.Font = new Font("Segoe UI", 9.5F);

            Row(lblType, "Type *", cmbType, 2, 200);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new object[] {
                "Médicament", "Matériel", "Produit d'hygiène", "Autre" });

            Row(lblPrixAchat, "Prix d'achat (KMF) *", txtPrixAchat, 3, 180);
            txtPrixAchat.PlaceholderText = "0.00";

            // Label marge avec style
            Row(lblMarge, "Marge bénéfice (%) →", txtMarge, 4, 100);
            lblMarge.ForeColor = Color.FromArgb(25, 118, 210);
            lblMarge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtMarge.Text = "0";

            // Flèche vers prix de vente
            var lblFleche = new Label
            {
                Text = "→ calculé automatiquement",
                Location = new Point(tx + 110, 4 * rh + 30),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            groupBoxProduit.Controls.Add(lblFleche);

            Row(lblPrixVente, "Prix de vente (KMF) ←", txtPrixVente, 5, 180);
            txtPrixVente.BackColor = Color.LightYellow;
            txtPrixVente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrixVente.ForeColor = Color.ForestGreen;
            lblPrixVente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            Row(lblQuantite, "Boîtes pleines en stock *", numQuantite, 6, 120);
            numQuantite.Maximum = 100000;
            numQuantite.Minimum = 0;

            // Unités en vrac (boîte entamée) : actif seulement si vente au détail
            lblUnitesVrac.Text = "+ unités en vrac :";
            lblUnitesVrac.Location = new Point(tx + 135, 6 * rh + 28);
            lblUnitesVrac.Size = new Size(115, 22);
            lblUnitesVrac.Font = new Font("Segoe UI", 9F);
            numUnitesVrac.Location = new Point(tx + 255, 6 * rh + 25);
            numUnitesVrac.Size = new Size(80, 28);
            numUnitesVrac.Maximum = 100000;
            numUnitesVrac.Minimum = 0;
            numUnitesVrac.Enabled = false;

            Row(lblSeuil, "Seuil d'alerte *", numSeuil, 7, 120);
            numSeuil.Maximum = 100000;
            numSeuil.Value = 10;
            var lblSeuilHint = new Label
            {
                Text = "(alerte quand stock ≤ seuil)",
                Location = new Point(tx + 130, 7 * rh + 30),
                Size = new Size(260, 18),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            groupBoxProduit.Controls.Add(lblSeuilHint);

            Row(lblDateExpiration, "Date d'expiration *", dtpDateExpiration, 8, 140);
            dtpDateExpiration.Format = DateTimePickerFormat.Short;

            Row(lblFournisseur, "Fournisseur :", cbFournisseur, 9, 280);
            cbFournisseur.DropDownStyle = ComboBoxStyle.DropDownList;

            groupBoxProduit.Controls.AddRange(new Control[] {
                lblNom, txtNom, lblType, cmbType,
                lblPrixAchat, txtPrixAchat, lblMarge, txtMarge,
                lblPrixVente, txtPrixVente,
                lblQuantite, numQuantite, lblUnitesVrac, numUnitesVrac, lblSeuil, numSeuil,
                lblDateExpiration, dtpDateExpiration,
                lblFournisseur, cbFournisseur
            });

            groupBoxProduit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxProduit.Location = new Point(14, 12);
            groupBoxProduit.Size = new Size(720, 10 * rh + 10);
            groupBoxProduit.Text = "📋  Informations sur le produit";

            // ══════════════════════════════════════════════════════════════
            // GROUPBOX VENTE EN DÉTAIL (plaquette, comprimé, flacon…)
            // ══════════════════════════════════════════════════════════════

            // Checkbox principale
            chkVenteDetail.Text = "Ce produit peut être vendu à l'unité (plaquette, comprimé, flacon…)";
            chkVenteDetail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            chkVenteDetail.ForeColor = Color.FromArgb(25, 118, 210);
            chkVenteDetail.Location = new Point(14, 22);
            chkVenteDetail.Size = new Size(690, 26);
            chkVenteDetail.Checked = false;

            // Panel du détail (activé uniquement si checkbox cochée)
            // ── Ligne 1 : Unité de vente ──────────────────────────────────
            lblUniteVente.Text = "Unité de vente :";
            lblUniteVente.Location = new Point(20, 14);
            lblUniteVente.Size = new Size(130, 22);
            lblUniteVente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cmbUniteVente.Location = new Point(158, 11);
            cmbUniteVente.Size = new Size(160, 28);
            cmbUniteVente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUniteVente.Font = new Font("Segoe UI", 9.5F);
            cmbUniteVente.Items.AddRange(new object[] {
                "Plaquette", "Comprimé", "Gélule", "Ampoule",
                "Flacon", "Sachet", "Tube", "Autre" });
            cmbUniteVente.SelectedIndex = 0;

            // ── Ligne 2 : Nombre d'unités par boîte ──────────────────────
            lblNbUnite.Text = "Nb d'unités par boîte :";
            lblNbUnite.Location = new Point(20, 50);
            lblNbUnite.Size = new Size(165, 22);
            lblNbUnite.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            numNbUniteParBoite.Location = new Point(190, 47);
            numNbUniteParBoite.Size = new Size(90, 28);
            numNbUniteParBoite.Minimum = 1;
            numNbUniteParBoite.Maximum = 1000;
            numNbUniteParBoite.Value = 2;
            numNbUniteParBoite.Font = new Font("Segoe UI", 10F);

            var lblNbHint = new Label
            {
                Text = "Ex : boîte de Doliprane = 16 comprimés → saisir 16",
                Location = new Point(290, 51),
                Size = new Size(390, 18),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            // ── Aperçu équivalence ────────────────────────────────────────
            lblApercu.Location = new Point(20, 84);
            lblApercu.Size = new Size(660, 56);
            lblApercu.Font = new Font("Segoe UI", 9F);
            lblApercu.ForeColor = Color.FromArgb(27, 94, 32);
            lblApercu.Text = "";
            lblApercu.Name = "lblApercu";

            pnlVenteDetail.BackColor = Color.FromArgb(245, 245, 245);
            pnlVenteDetail.BorderStyle = BorderStyle.FixedSingle;
            pnlVenteDetail.Controls.AddRange(new Control[] {
                lblUniteVente, cmbUniteVente,
                lblNbUnite, numNbUniteParBoite, lblNbHint,
                lblApercu
            });
            pnlVenteDetail.Enabled = false;
            pnlVenteDetail.Location = new Point(14, 52);
            pnlVenteDetail.Size = new Size(690, 148);
            pnlVenteDetail.Name = "pnlVenteDetail";

            groupBoxDetail.Controls.Add(chkVenteDetail);
            groupBoxDetail.Controls.Add(pnlVenteDetail);
            groupBoxDetail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxDetail.Location = new Point(14, 12 + groupBoxProduit.Size.Height + 10);
            groupBoxDetail.Size = new Size(720, 210);
            groupBoxDetail.Text = "📦  Vente en détail (optionnel)";
            groupBoxDetail.ForeColor = Color.FromArgb(0, 100, 80);

            // ══════════════════════════════════════════════════════════════
            // GROUPBOX DÉTAILS MÉDICAMENT — pour le caissier
            // ══════════════════════════════════════════════════════════════
            ((System.ComponentModel.ISupportInitialize)numPosologieJour).BeginInit();

            int yMed = groupBoxDetail.Location.Y + groupBoxDetail.Height + 10;

            lblIndication.Text = "Indication (pour quoi ?) :";
            lblIndication.Location = new Point(20, 30);
            lblIndication.Size = new Size(185, 22);
            lblIndication.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIndication.ForeColor = Color.FromArgb(25, 118, 210);

            txtIndication.Location = new Point(210, 27);
            txtIndication.Size = new Size(490, 28);
            txtIndication.Font = new Font("Segoe UI", 9.5F);
            txtIndication.PlaceholderText = "Ex: Pour la toux, fièvre, hypertension, douleur...";

            lblPosologie.Text = "Posologie (comment prendre) :";
            lblPosologie.Location = new Point(20, 68);
            lblPosologie.Size = new Size(185, 22);
            lblPosologie.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            txtPosologie.Location = new Point(210, 65);
            txtPosologie.Size = new Size(490, 28);
            txtPosologie.Font = new Font("Segoe UI", 9.5F);
            txtPosologie.PlaceholderText = "Ex: 1 comprimé après le repas, 2 gélules le matin...";

            lblPosologieJour.Text = "Nb de prises par jour :";
            lblPosologieJour.Location = new Point(20, 108);
            lblPosologieJour.Size = new Size(185, 22);
            lblPosologieJour.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            numPosologieJour.Location = new Point(210, 105);
            numPosologieJour.Size = new Size(80, 28);
            numPosologieJour.Minimum = 0;
            numPosologieJour.Maximum = 10;
            numPosologieJour.Value = 0;
            numPosologieJour.Font = new Font("Segoe UI", 10F);

            var lblPriseHint = new Label
            {
                Text = "(0 = non renseigné)",
                Location = new Point(300, 109),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            groupBoxMedicament.Controls.AddRange(new Control[] {
                lblIndication, txtIndication,
                lblPosologie,  txtPosologie,
                lblPosologieJour, numPosologieJour, lblPriseHint
            });
            groupBoxMedicament.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBoxMedicament.Location = new Point(14, yMed);
            groupBoxMedicament.Size = new Size(720, 140);
            groupBoxMedicament.Text = "💊  Informations pour le caissier (optionnel)";
            groupBoxMedicament.ForeColor = Color.FromArgb(156, 39, 176);

            ((System.ComponentModel.ISupportInitialize)numPosologieJour).EndInit();

            // ── Boutons ────────────────────────────────────────────────────
            int yBtn = groupBoxMedicament.Location.Y + groupBoxMedicament.Height + 16;

            btnEnregistrer.Text = "✔ Enregistrer";
            btnEnregistrer.Location = new Point(430, yBtn);
            btnEnregistrer.Size = new Size(160, 40);
            btnEnregistrer.BackColor = Color.FromArgb(46, 125, 50);
            btnEnregistrer.ForeColor = Color.White;
            btnEnregistrer.FlatStyle = FlatStyle.Flat;
            btnEnregistrer.FlatAppearance.BorderSize = 0;
            btnEnregistrer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEnregistrer.Anchor = AnchorStyles.None;
            btnEnregistrer.Click += btnEnregistrer_Click;

            btnAnnuler.Text = "✖ Annuler";
            btnAnnuler.Location = new Point(600, yBtn);
            btnAnnuler.Size = new Size(120, 40);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.BackColor = Color.FromArgb(120, 120, 120);
            btnAnnuler.ForeColor = Color.White;
            btnAnnuler.FlatAppearance.BorderSize = 0;
            btnAnnuler.Font = new Font("Segoe UI", 10F);
            btnAnnuler.Anchor = AnchorStyles.None;
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Form : AutoScroll directement sur la Form ─────────────────
            // Plus simple et plus fiable que pnlScroll + pnlBas
            int hauteurContenu = yBtn + 56;

            ClientSize = new Size(780, hauteurContenu);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimumSize = new Size(600, 440);
            AutoScroll = true;   // ← scroll directement sur la Form
            BackColor = Color.White;
            Text = "Produit";

            Controls.Add(groupBoxProduit);
            Controls.Add(groupBoxDetail);
            Controls.Add(groupBoxMedicament);
            Controls.Add(btnEnregistrer);
            Controls.Add(btnAnnuler);

            ((System.ComponentModel.ISupportInitialize)numQuantite).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnitesVrac).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSeuil).EndInit();
            ((System.ComponentModel.ISupportInitialize)numNbUniteParBoite).EndInit();
            pnlVenteDetail.ResumeLayout(false);
            groupBoxDetail.ResumeLayout(false);
            groupBoxProduit.ResumeLayout(false);
            groupBoxProduit.PerformLayout();
            ResumeLayout(false);
        }

        // ── Champs ────────────────────────────────────────────────────────
        private GroupBox groupBoxProduit;
        private GroupBox groupBoxDetail;
        private Panel pnlVenteDetail;

        private Label lblNom; private TextBox txtNom;
        private Label lblType; private ComboBox cmbType;
        private Label lblPrixAchat; private TextBox txtPrixAchat;
        private Label lblMarge; private TextBox txtMarge;
        private Label lblPrixVente; private TextBox txtPrixVente;
        private Label lblQuantite; private NumericUpDown numQuantite;
        private Label lblUnitesVrac; private NumericUpDown numUnitesVrac;
        private Label lblSeuil; private NumericUpDown numSeuil;
        private Label lblDateExpiration; private DateTimePicker dtpDateExpiration;
        private Label lblFournisseur; private ComboBox cbFournisseur;

        // Vente en détail
        private CheckBox chkVenteDetail;
        private Label lblUniteVente; private ComboBox cmbUniteVente;
        private Label lblNbUnite; private NumericUpDown numNbUniteParBoite;
        private Label lblApercu;

        private Button btnEnregistrer;
        private Button btnAnnuler;
        // Détails médicament
        private GroupBox groupBoxMedicament;
        private Label lblIndication, lblPosologie, lblPosologieJour;
        private TextBox txtIndication, txtPosologie;
        private NumericUpDown numPosologieJour;
    }
}