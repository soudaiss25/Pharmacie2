using System.Windows.Forms;
using System.Drawing;
using Pharmacie2.Models;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Utilisateurs
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTop;
        private Label lblTitre, lblCompteur;

        private Panel pnlRecherche;
        private Label lblRecherche;
        private TextBox txtRecherche;
        private Button btnEffacerRecherche;

        private Panel pnlActions;
        private Button btnNouvel, btnModifier, btnSupprimer;

        private DataGridView dgvUtilisateurs;

        private Panel pnlFormulaire;
        private Label lblTitreForm;
        private Label lblNom, lblPrenom, lblLogin, lblMdp, lblRole;
        private TextBox txtNom, txtPrenom, txtLogin, txtMotDePasse;
        private ComboBox cbRole;
        private Button btnEnregistrer, btnAnnuler;

        private Panel pnlLegende;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblTitre = new Label();
            lblCompteur = new Label();
            pnlRecherche = new Panel();
            lblRecherche = new Label();
            txtRecherche = new TextBox();
            btnEffacerRecherche = new Button();
            pnlActions = new Panel();
            btnNouvel = new Button();
            btnModifier = new Button();
            btnSupprimer = new Button();
            dgvUtilisateurs = new DataGridView();
            pnlFormulaire = new Panel();
            lblTitreForm = new Label();
            lblNom = new Label(); txtNom = new TextBox();
            lblPrenom = new Label(); txtPrenom = new TextBox();
            lblLogin = new Label(); txtLogin = new TextBox();
            lblMdp = new Label(); txtMotDePasse = new TextBox();
            lblRole = new Label(); cbRole = new ComboBox();
            btnEnregistrer = new Button();
            btnAnnuler = new Button();
            pnlLegende = new Panel();

            ((System.ComponentModel.ISupportInitialize)dgvUtilisateurs).BeginInit();
            this.SuspendLayout();

            // ══════════════════════════════════════════════════════
            // PANEL TOP
            // ══════════════════════════════════════════════════════
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 55;
            pnlTop.BackColor = Color.FromArgb(34, 85, 34);

            lblTitre.Text = "👥  Gestion des Utilisateurs & Rôles";
            lblTitre.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Location = new Point(14, 12);
            lblTitre.Size = new Size(500, 32);

            lblCompteur.Text = "0 utilisateur(s)";
            lblCompteur.Font = new Font("Segoe UI", 9F);
            lblCompteur.ForeColor = Color.FromArgb(180, 230, 180);
            lblCompteur.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCompteur.Location = new Point(820, 18);
            lblCompteur.Size = new Size(160, 22);
            lblCompteur.TextAlign = ContentAlignment.MiddleRight;

            pnlTop.Controls.Add(lblTitre);
            pnlTop.Controls.Add(lblCompteur);

            // ══════════════════════════════════════════════════════
            // PANEL RECHERCHE
            // ══════════════════════════════════════════════════════
            pnlRecherche.Dock = DockStyle.Top;
            pnlRecherche.Height = 46;
            pnlRecherche.BackColor = Color.FromArgb(245, 250, 245);

            lblRecherche.Text = "🔍  Rechercher :";
            lblRecherche.Font = new Font("Segoe UI", 9F);
            lblRecherche.Location = new Point(14, 13);
            lblRecherche.Size = new Size(110, 22);
            lblRecherche.ForeColor = Color.FromArgb(60, 60, 60);

            txtRecherche.Location = new Point(128, 10);
            txtRecherche.Size = new Size(280, 26);
            txtRecherche.Font = new Font("Segoe UI", 9.5F);
            txtRecherche.BorderStyle = BorderStyle.FixedSingle;
            txtRecherche.PlaceholderText = "Nom, prénom, login ou rôle…";
            txtRecherche.TextChanged += new System.EventHandler(this.txtRecherche_TextChanged);

            btnEffacerRecherche.Text = "✕";
            btnEffacerRecherche.Location = new Point(416, 9);
            btnEffacerRecherche.Size = new Size(30, 28);
            btnEffacerRecherche.FlatStyle = FlatStyle.Flat;
            btnEffacerRecherche.FlatAppearance.BorderColor = Color.Silver;
            btnEffacerRecherche.Font = new Font("Segoe UI", 9F);
            btnEffacerRecherche.BackColor = Color.White;
            btnEffacerRecherche.Click += new System.EventHandler(this.btnEffacerRecherche_Click);

            pnlRecherche.Controls.Add(lblRecherche);
            pnlRecherche.Controls.Add(txtRecherche);
            pnlRecherche.Controls.Add(btnEffacerRecherche);

            // ══════════════════════════════════════════════════════
            // PANEL ACTIONS — 3 boutons (pas de ResetMdp)
            // ══════════════════════════════════════════════════════
            pnlActions.Dock = DockStyle.Top;
            pnlActions.Height = 50;
            pnlActions.BackColor = Color.White;

            void StyleBtn(Button b, string text, Color bg, int x)
            {
                b.Text = text;
                b.Location = new Point(x, 8);
                b.Size = new Size(155, 34);
                b.BackColor = bg;
                b.ForeColor = Color.White;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
            }

            StyleBtn(btnNouvel, "➕  Nouvel utilisateur", Color.FromArgb(46, 125, 50), 14);
            btnNouvel.Click += new System.EventHandler(this.btnNouvel_Click);

            StyleBtn(btnModifier, "✎  Modifier", Color.FromArgb(25, 118, 210), 178);
            btnModifier.Click += new System.EventHandler(this.btnModifier_Click);

            StyleBtn(btnSupprimer, "✖  Supprimer", Color.FromArgb(198, 40, 40), 342);
            btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);

            pnlActions.Controls.AddRange(new Control[] { btnNouvel, btnModifier, btnSupprimer });

            // ══════════════════════════════════════════════════════
            // DATAGRIDVIEW
            // ══════════════════════════════════════════════════════
            dgvUtilisateurs.Dock = DockStyle.Fill;
            dgvUtilisateurs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUtilisateurs.ReadOnly = true;
            dgvUtilisateurs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUtilisateurs.MultiSelect = false;
            dgvUtilisateurs.AllowUserToAddRows = false;
            dgvUtilisateurs.AllowUserToDeleteRows = false;
            dgvUtilisateurs.RowHeadersVisible = false;
            dgvUtilisateurs.BackgroundColor = Color.White;
            dgvUtilisateurs.GridColor = Color.FromArgb(220, 230, 220);
            dgvUtilisateurs.BorderStyle = BorderStyle.None;
            dgvUtilisateurs.Font = new Font("Segoe UI", 9.5F);
            dgvUtilisateurs.RowTemplate.Height = 36;
            dgvUtilisateurs.ColumnHeadersHeight = 38;
            dgvUtilisateurs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
            dgvUtilisateurs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUtilisateurs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvUtilisateurs.EnableHeadersVisualStyles = false;
            dgvUtilisateurs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            dgvUtilisateurs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(165, 214, 167);
            dgvUtilisateurs.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUtilisateurs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUtilisateurs.SelectionChanged += new System.EventHandler(this.dgvUtilisateurs_SelectionChanged);

            // ══════════════════════════════════════════════════════
            // PANEL FORMULAIRE LATÉRAL
            // ══════════════════════════════════════════════════════
            pnlFormulaire.Dock = DockStyle.Right;
            pnlFormulaire.Width = 300;
            pnlFormulaire.BackColor = Color.FromArgb(250, 253, 250);

            // Bordure gauche verte
            pnlFormulaire.Paint += (s, e) =>
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(46, 125, 50)),
                    0, 0, 4, pnlFormulaire.Height);
            };

            lblTitreForm.Text = "Détail utilisateur";
            lblTitreForm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitreForm.ForeColor = Color.FromArgb(34, 85, 34);
            lblTitreForm.Location = new Point(16, 14);
            lblTitreForm.Size = new Size(268, 26);

            int lx = 16, fw = 268, rh = 52;

            void AddChamp(Label lbl, string text, TextBox tb, int row)
            {
                lbl.Text = text;
                lbl.Location = new Point(lx, 48 + row * rh);
                lbl.Size = new Size(fw, 18);
                lbl.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(80, 80, 80);

                tb.Location = new Point(lx, 48 + row * rh + 20);
                tb.Size = new Size(fw, 28);
                tb.Font = new Font("Segoe UI", 9.5F);
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = Color.FromArgb(245, 250, 245);
                tb.Enabled = false;
                // Pas de PasswordChar — mot de passe affiché en clair

                pnlFormulaire.Controls.Add(lbl);
                pnlFormulaire.Controls.Add(tb);
            }

            AddChamp(lblNom, "Nom *", txtNom, 0);
            AddChamp(lblPrenom, "Prénom *", txtPrenom, 1);
            AddChamp(lblLogin, "Login *", txtLogin, 2);
            AddChamp(lblMdp, "Mot de passe *", txtMotDePasse, 3);

            // Rôle
            lblRole.Text = "Rôle *";
            lblRole.Location = new Point(lx, 48 + 4 * rh);
            lblRole.Size = new Size(fw, 18);
            lblRole.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblRole.ForeColor = Color.FromArgb(80, 80, 80);

            cbRole.Location = new Point(lx, 48 + 4 * rh + 20);
            cbRole.Size = new Size(fw, 28);
            cbRole.Font = new Font("Segoe UI", 9.5F);
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRole.FlatStyle = FlatStyle.Flat;
            cbRole.BackColor = Color.FromArgb(245, 250, 245);
            cbRole.Enabled = false;
            cbRole.Items.AddRange(new object[] { Roles.Administrateur, Roles.Pharmacien, Roles.Caissier });

            pnlFormulaire.Controls.Add(lblRole);
            pnlFormulaire.Controls.Add(cbRole);

            // Bouton Enregistrer
            btnEnregistrer.Text = "✔  Enregistrer";
            btnEnregistrer.Location = new Point(lx, 48 + 5 * rh + 12);
            btnEnregistrer.Size = new Size(fw, 36);
            btnEnregistrer.BackColor = Color.FromArgb(46, 125, 50);
            btnEnregistrer.ForeColor = Color.White;
            btnEnregistrer.FlatStyle = FlatStyle.Flat;
            btnEnregistrer.FlatAppearance.BorderSize = 0;
            btnEnregistrer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEnregistrer.Visible = false;
            btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);

            // Bouton Annuler
            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(lx, 48 + 5 * rh + 56);
            btnAnnuler.Size = new Size(fw, 30);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.FlatAppearance.BorderColor = Color.Silver;
            btnAnnuler.Font = new Font("Segoe UI", 9F);
            btnAnnuler.BackColor = Color.White;
            btnAnnuler.Visible = false;
            btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);

            pnlFormulaire.Controls.Add(lblTitreForm);
            pnlFormulaire.Controls.Add(btnEnregistrer);
            pnlFormulaire.Controls.Add(btnAnnuler);

            // ══════════════════════════════════════════════════════
            // PANEL LÉGENDE (bas)
            // ══════════════════════════════════════════════════════
            pnlLegende.Dock = DockStyle.Bottom;
            pnlLegende.Height = 36;
            pnlLegende.BackColor = Color.FromArgb(245, 250, 245);

            var lblLeg = new Label
            {
                Text = "Rôles :   🟢 Administrateur = accès complet   |   🔵 Pharmacien = accès complet   |   🟠 Caissier = ventes uniquement",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(14, 10),
                Size = new Size(850, 18)
            };
            pnlLegende.Controls.Add(lblLeg);

            // ══════════════════════════════════════════════════════
            // ASSEMBLAGE (ordre important pour Dock)
            // ══════════════════════════════════════════════════════
            ((System.ComponentModel.ISupportInitialize)dgvUtilisateurs).EndInit();

            this.Controls.Add(dgvUtilisateurs);   // Fill  (doit être avant les Dock:Right)
            this.Controls.Add(pnlFormulaire);      // Right
            this.Controls.Add(pnlLegende);         // Bottom
            this.Controls.Add(pnlActions);         // Top (3e)
            this.Controls.Add(pnlRecherche);       // Top (2e)
            this.Controls.Add(pnlTop);             // Top (1er)

            this.Size = new Size(1050, 640);
            this.BackColor = Color.White;
            this.ResumeLayout(false);
        }
    }
}