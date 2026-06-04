using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormSessionCaisse
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitre;
        private Label lblEtat;
        private Label lblInfoSession;
        private Label lblMontantReel;
        private NumericUpDown numMontantReel;
        private Label lblCommentaireLabel;
        private TextBox txtCommentaire;
        private Button btnOuvrir;
        private Button btnCloture;
        private Button btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            lblEtat = new Label();
            lblInfoSession = new Label();
            lblMontantReel = new Label();
            numMontantReel = new NumericUpDown();
            lblCommentaireLabel = new Label();
            txtCommentaire = new TextBox();
            btnOuvrir = new Button();
            btnCloture = new Button();
            btnAnnuler = new Button();

            ((System.ComponentModel.ISupportInitialize)numMontantReel).BeginInit();
            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitre.BackColor = Color.FromArgb(34, 85, 34);
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Name = "lblTitre";
            lblTitre.Size = new Size(460, 55);
            lblTitre.TabIndex = 0;
            lblTitre.Text = "💰  Gestion de la Caisse";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;

            // ── État session ───────────────────────────────────────────────
            lblEtat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEtat.Location = new Point(20, 68);
            lblEtat.Name = "lblEtat";
            lblEtat.Size = new Size(420, 24);
            lblEtat.TabIndex = 1;
            lblEtat.Text = "Vérification…";

            // ── Info session (théorique) ────────────────────────────────────
            lblInfoSession.Font = new Font("Segoe UI", 9F);
            lblInfoSession.ForeColor = Color.FromArgb(34, 85, 34);
            lblInfoSession.Location = new Point(20, 98);
            lblInfoSession.Name = "lblInfoSession";
            lblInfoSession.Size = new Size(420, 55);
            lblInfoSession.TabIndex = 2;

            // ── Montant compté (clôture uniquement) ────────────────────────
            lblMontantReel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMontantReel.Location = new Point(20, 164);
            lblMontantReel.Name = "lblMontantReel";
            lblMontantReel.Size = new Size(200, 28);
            lblMontantReel.TabIndex = 3;
            lblMontantReel.Text = "Montant compté en caisse (KMF) :";

            numMontantReel.Font = new Font("Segoe UI", 10F);
            numMontantReel.Location = new Point(232, 162);
            numMontantReel.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMontantReel.Name = "numMontantReel";
            numMontantReel.Size = new Size(168, 34);
            numMontantReel.TabIndex = 4;
            numMontantReel.ThousandsSeparator = true;

            // ── Commentaire (clôture) ──────────────────────────────────────
            lblCommentaireLabel.Font = new Font("Segoe UI", 9F);
            lblCommentaireLabel.Location = new Point(20, 208);
            lblCommentaireLabel.Name = "lblCommentaireLabel";
            lblCommentaireLabel.Size = new Size(200, 22);
            lblCommentaireLabel.TabIndex = 5;
            lblCommentaireLabel.Text = "Commentaire de clôture :";

            txtCommentaire.Font = new Font("Segoe UI", 9F);
            txtCommentaire.Location = new Point(20, 232);
            txtCommentaire.Name = "txtCommentaire";
            txtCommentaire.PlaceholderText = "Ex: 5×1000 + 3×500 + monnaie…";
            txtCommentaire.Size = new Size(420, 31);
            txtCommentaire.TabIndex = 6;

            // ── Bouton Ouvrir ──────────────────────────────────────────────
            btnOuvrir.BackColor = Color.FromArgb(46, 125, 50);
            btnOuvrir.FlatAppearance.BorderSize = 0;
            btnOuvrir.FlatStyle = FlatStyle.Flat;
            btnOuvrir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOuvrir.ForeColor = Color.White;
            btnOuvrir.Location = new Point(20, 280);
            btnOuvrir.Name = "btnOuvrir";
            btnOuvrir.Size = new Size(175, 38);
            btnOuvrir.TabIndex = 7;
            btnOuvrir.Text = "✅ Ouvrir la caisse";
            btnOuvrir.UseVisualStyleBackColor = false;
            btnOuvrir.Click += btnOuvrir_Click;

            // ── Bouton Clôturer ────────────────────────────────────────────
            btnCloture.BackColor = Color.FromArgb(180, 60, 0);
            btnCloture.FlatAppearance.BorderSize = 0;
            btnCloture.FlatStyle = FlatStyle.Flat;
            btnCloture.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCloture.ForeColor = Color.White;
            btnCloture.Location = new Point(203, 280);
            btnCloture.Name = "btnCloture";
            btnCloture.Size = new Size(140, 38);
            btnCloture.TabIndex = 8;
            btnCloture.Text = "🔒 Clôturer";
            btnCloture.UseVisualStyleBackColor = false;
            btnCloture.Click += btnCloture_Click;

            // ── Bouton Fermer ──────────────────────────────────────────────
            btnAnnuler.FlatAppearance.BorderColor = Color.Silver;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Font = new Font("Segoe UI", 9F);
            btnAnnuler.Location = new Point(351, 280);
            btnAnnuler.Name = "btnAnnuler";
            btnAnnuler.Size = new Size(89, 38);
            btnAnnuler.TabIndex = 9;
            btnAnnuler.Text = "Fermer";
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Form ───────────────────────────────────────────────────────
            BackColor = Color.White;
            ClientSize = new Size(460, 336);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormSessionCaisse";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Session Caisse";

            Controls.Add(lblTitre);
            Controls.Add(lblEtat);
            Controls.Add(lblInfoSession);
            Controls.Add(lblMontantReel);
            Controls.Add(numMontantReel);
            Controls.Add(lblCommentaireLabel);
            Controls.Add(txtCommentaire);
            Controls.Add(btnOuvrir);
            Controls.Add(btnCloture);
            Controls.Add(btnAnnuler);

            ((System.ComponentModel.ISupportInitialize)numMontantReel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}