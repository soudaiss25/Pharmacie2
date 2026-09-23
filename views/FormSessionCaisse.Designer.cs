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
        private Label lblFond;
        private NumericUpDown numFond;
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
            lblFond = new Label();
            numFond = new NumericUpDown();
            lblMontantReel = new Label();
            numMontantReel = new NumericUpDown();
            lblCommentaireLabel = new Label();
            txtCommentaire = new TextBox();
            btnOuvrir = new Button();
            btnCloture = new Button();
            btnAnnuler = new Button();
            ((System.ComponentModel.ISupportInitialize)numFond).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMontantReel).BeginInit();
            SuspendLayout();
            // 
            // lblTitre
            // 
            lblTitre.BackColor = Color.FromArgb(34, 85, 34);
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Location = new Point(0, 0);
            lblTitre.Name = "lblTitre";
            lblTitre.Size = new Size(560, 55);
            lblTitre.TabIndex = 0;
            lblTitre.Text = "💰  Gestion de la Caisse";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEtat
            // 
            lblEtat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEtat.Location = new Point(20, 70);
            lblEtat.Name = "lblEtat";
            lblEtat.Size = new Size(520, 24);
            lblEtat.TabIndex = 1;
            lblEtat.Text = "Vérification…";
            // 
            // lblInfoSession
            // 
            lblInfoSession.Font = new Font("Segoe UI", 9F);
            lblInfoSession.ForeColor = Color.FromArgb(34, 85, 34);
            lblInfoSession.Location = new Point(20, 100);
            lblInfoSession.Name = "lblInfoSession";
            lblInfoSession.Size = new Size(520, 220);
            lblInfoSession.TabIndex = 2;
            // 
            // lblFond
            // 
            lblFond.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFond.Location = new Point(20, 330);
            lblFond.Name = "lblFond";
            lblFond.Size = new Size(200, 22);
            lblFond.TabIndex = 3;
            lblFond.Text = "Fond d'ouverture (KMF) :";
            // 
            // numFond
            // 
            numFond.Font = new Font("Segoe UI", 10F);
            numFond.Location = new Point(330, 327);
            numFond.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numFond.Name = "numFond";
            numFond.Size = new Size(180, 34);
            numFond.TabIndex = 4;
            numFond.ThousandsSeparator = true;
            // 
            // lblMontantReel
            // 
            lblMontantReel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMontantReel.Location = new Point(4, 372);
            lblMontantReel.Name = "lblMontantReel";
            lblMontantReel.Size = new Size(191, 55);
            lblMontantReel.TabIndex = 5;
            lblMontantReel.Text = "Montant compté en caisse :";
            // 
            // numMontantReel
            // 
            numMontantReel.Font = new Font("Segoe UI", 10F);
            numMontantReel.Location = new Point(330, 370);
            numMontantReel.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMontantReel.Name = "numMontantReel";
            numMontantReel.Size = new Size(180, 34);
            numMontantReel.TabIndex = 6;
            numMontantReel.ThousandsSeparator = true;
            // 
            // lblCommentaireLabel
            // 
            lblCommentaireLabel.Font = new Font("Segoe UI", 9F);
            lblCommentaireLabel.Location = new Point(20, 418);
            lblCommentaireLabel.Name = "lblCommentaireLabel";
            lblCommentaireLabel.Size = new Size(200, 22);
            lblCommentaireLabel.TabIndex = 7;
            lblCommentaireLabel.Text = "Commentaire :";
            // 
            // txtCommentaire
            // 
            txtCommentaire.Font = new Font("Segoe UI", 9F);
            txtCommentaire.Location = new Point(20, 442);
            txtCommentaire.Name = "txtCommentaire";
            txtCommentaire.PlaceholderText = "Ex: 5×1000 + 3×500 + monnaie…";
            txtCommentaire.Size = new Size(520, 31);
            txtCommentaire.TabIndex = 8;
            // 
            // btnOuvrir
            // 
            btnOuvrir.BackColor = Color.FromArgb(46, 125, 50);
            btnOuvrir.FlatAppearance.BorderSize = 0;
            btnOuvrir.FlatStyle = FlatStyle.Flat;
            btnOuvrir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOuvrir.ForeColor = Color.White;
            btnOuvrir.Location = new Point(20, 490);
            btnOuvrir.Name = "btnOuvrir";
            btnOuvrir.Size = new Size(175, 38);
            btnOuvrir.TabIndex = 9;
            btnOuvrir.Text = "✅ Ouvrir la caisse";
            btnOuvrir.UseVisualStyleBackColor = false;
            btnOuvrir.Click += btnOuvrir_Click;
            // 
            // btnCloture
            // 
            btnCloture.BackColor = Color.FromArgb(180, 60, 0);
            btnCloture.FlatAppearance.BorderSize = 0;
            btnCloture.FlatStyle = FlatStyle.Flat;
            btnCloture.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCloture.ForeColor = Color.White;
            btnCloture.Location = new Point(213, 490);
            btnCloture.Name = "btnCloture";
            btnCloture.Size = new Size(140, 38);
            btnCloture.TabIndex = 10;
            btnCloture.Text = "🔒 Clôturer";
            btnCloture.UseVisualStyleBackColor = false;
            btnCloture.Click += btnCloture_Click;
            // 
            // btnAnnuler
            // 
            btnAnnuler.FlatAppearance.BorderColor = Color.Silver;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Font = new Font("Segoe UI", 9F);
            btnAnnuler.Location = new Point(460, 490);
            btnAnnuler.Name = "btnAnnuler";
            btnAnnuler.Size = new Size(89, 38);
            btnAnnuler.TabIndex = 11;
            btnAnnuler.Text = "Fermer";
            btnAnnuler.Click += btnAnnuler_Click;
            // 
            // FormSessionCaisse
            // 
            BackColor = Color.White;
            ClientSize = new Size(560, 548);
            Controls.Add(lblTitre);
            Controls.Add(lblEtat);
            Controls.Add(lblInfoSession);
            Controls.Add(lblFond);
            Controls.Add(numFond);
            Controls.Add(lblMontantReel);
            Controls.Add(numMontantReel);
            Controls.Add(lblCommentaireLabel);
            Controls.Add(txtCommentaire);
            Controls.Add(btnOuvrir);
            Controls.Add(btnCloture);
            Controls.Add(btnAnnuler);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormSessionCaisse";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Session Caisse";
            ((System.ComponentModel.ISupportInitialize)numFond).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMontantReel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}