using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class FormAddDepense
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private Label lblTitreHeader;
        private Label lblCategorie, lblDescription, lblMontant, lblDate, lblNote;
        private ComboBox cbCategorie;
        private TextBox txtDescription, txtMontant;
        private DateTimePicker dtpDate;
        private Button btnEnregistrer, btnAnnuler;

        private void InitializeComponent()
        {
            lblTitreHeader = new Label();
            lblCategorie = new Label(); cbCategorie = new ComboBox();
            lblDescription = new Label(); txtDescription = new TextBox();
            lblMontant = new Label(); txtMontant = new TextBox();
            lblDate = new Label(); dtpDate = new DateTimePicker();
            lblNote = new Label();
            btnEnregistrer = new Button(); btnAnnuler = new Button();

            SuspendLayout();

            // ── En-tête ────────────────────────────────────────────────────
            lblTitreHeader.Dock = DockStyle.Top;
            lblTitreHeader.Height = 56;
            lblTitreHeader.Text = "💸  Nouvelle Dépense Annexe";
            lblTitreHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitreHeader.ForeColor = Color.White;
            lblTitreHeader.BackColor = Color.FromArgb(183, 28, 28);
            lblTitreHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── Helper ─────────────────────────────────────────────────────
            int lx = 24, tx = 170, rw = 360, rh = 52;

            void Ligne(Label l, string t, Control c, int row, int cw = 360)
            {
                l.Text = t;
                l.Location = new Point(lx, row * rh + 74);
                l.Size = new Size(140, 24);
                l.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                l.ForeColor = Color.FromArgb(60, 60, 60);
                c.Location = new Point(tx, row * rh + 70);
                c.Size = new Size(cw, 30);
                c.Font = new Font("Segoe UI", 10F);
            }

            // ── Ligne 1 : Catégorie ────────────────────────────────────────
            Ligne(lblCategorie, "Catégorie * :", cbCategorie, 0, 200);
            cbCategorie.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategorie.Items.AddRange(new object[] {
                "Salaire", "Facture", "Loyer", "Fournitures", "Autre" });
            cbCategorie.SelectedIndexChanged += cbCategorie_SelectedIndexChanged;

            // ── Ligne 2 : Description ──────────────────────────────────────
            Ligne(lblDescription, "Description :", txtDescription, 1);
            txtDescription.PlaceholderText = "Ex: Salaire Thomas — Mai 2026";

            // ── Ligne 3 : Montant ──────────────────────────────────────────
            Ligne(lblMontant, "Montant (KMF) * :", txtMontant, 2, 160);
            txtMontant.PlaceholderText = "0";
            txtMontant.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtMontant.ForeColor = Color.FromArgb(183, 28, 28);

            // ── Ligne 4 : Date ─────────────────────────────────────────────
            Ligne(lblDate, "Date :", dtpDate, 3, 160);
            dtpDate.Format = DateTimePickerFormat.Short;

            // ── Note ───────────────────────────────────────────────────────
            lblNote.Text = "* Champs obligatoires";
            lblNote.Location = new Point(lx, 4 * rh + 80);
            lblNote.Size = new Size(300, 20);
            lblNote.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblNote.ForeColor = Color.Gray;

            // ── Boutons ────────────────────────────────────────────────────
            int yBtn = 4 * rh + 108;

            btnEnregistrer.Text = "✔ Enregistrer";
            btnEnregistrer.Location = new Point(200, yBtn);
            btnEnregistrer.Size = new Size(160, 40);
            btnEnregistrer.BackColor = Color.FromArgb(183, 28, 28);
            btnEnregistrer.ForeColor = Color.White;
            btnEnregistrer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEnregistrer.FlatStyle = FlatStyle.Flat;
            btnEnregistrer.FlatAppearance.BorderSize = 0;
            btnEnregistrer.Click += btnEnregistrer_Click;

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new Point(370, yBtn);
            btnAnnuler.Size = new Size(100, 40);
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.Font = new Font("Segoe UI", 9.5F);
            btnAnnuler.Click += btnAnnuler_Click;

            // ── Form ───────────────────────────────────────────────────────
            ClientSize = new Size(560, yBtn + 58);
            Text = "Ajouter une dépense";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            Controls.AddRange(new Control[] {
                lblTitreHeader,
                lblCategorie,   cbCategorie,
                lblDescription, txtDescription,
                lblMontant,     txtMontant,
                lblDate,        dtpDate,
                lblNote,
                btnEnregistrer, btnAnnuler
            });

            ResumeLayout(false);
        }
    }
}