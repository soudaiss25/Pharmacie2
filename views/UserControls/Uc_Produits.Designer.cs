using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Produits : UserControl
    {
        private Panel pnlTop, pnlFiltres, pnlActions;
        private Label lblTitre, lblCompteur;
        private Label lblRecherche, lblTypeFiltre;
        private TextBox txtRecherche;
        private ComboBox cmbTypeFiltre;
        private Button btnEffacer;
        private Button btnNouveauProduit, btnModifier, btnSupprimer;
        private DataGridView dgvProduits;

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblTitre = new Label();
            lblCompteur = new Label();
            pnlFiltres = new Panel();
            lblRecherche = new Label();
            txtRecherche = new TextBox();
            lblTypeFiltre = new Label();
            cmbTypeFiltre = new ComboBox();
            btnEffacer = new Button();
            pnlActions = new Panel();
            btnNouveauProduit = new Button();
            btnModifier = new Button();
            btnSupprimer = new Button();
            dgvProduits = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvProduits).BeginInit();
            this.SuspendLayout();

            // Top
            pnlTop.Dock = DockStyle.Top; pnlTop.Height = 52;
            pnlTop.BackColor = Color.FromArgb(34, 85, 34);
            lblTitre.Text = "💊  Gestion des Produits";
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold); lblTitre.ForeColor = Color.White;
            lblTitre.Location = new Point(14, 12); lblTitre.Size = new Size(400, 28);
            lblCompteur.Text = "0 produit(s)"; lblCompteur.Font = new Font("Segoe UI", 9F);
            lblCompteur.ForeColor = Color.LightGreen;
            lblCompteur.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCompteur.Location = new Point(820, 16); lblCompteur.Size = new Size(150, 22);
            lblCompteur.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            pnlTop.Controls.Add(lblTitre); pnlTop.Controls.Add(lblCompteur);

            // Filtres
            pnlFiltres.Dock = DockStyle.Top; pnlFiltres.Height = 46;
            pnlFiltres.BackColor = Color.FromArgb(245, 250, 245);
            lblRecherche.Text = "🔍 Rechercher :"; lblRecherche.Location = new Point(14, 13); lblRecherche.Size = new Size(110, 22);
            txtRecherche.Location = new Point(128, 10); txtRecherche.Size = new Size(240, 26);
            txtRecherche.PlaceholderText = "Nom, type ou fournisseur…"; txtRecherche.BorderStyle = BorderStyle.FixedSingle;
            lblTypeFiltre.Text = "Type :"; lblTypeFiltre.Location = new Point(384, 13); lblTypeFiltre.Size = new Size(45, 22);
            cmbTypeFiltre.Location = new Point(432, 10); cmbTypeFiltre.Size = new Size(180, 26);
            cmbTypeFiltre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTypeFiltre.Items.AddRange(new object[] { "Tous", "Médicament", "Matériel", "Produit d'hygiène", "Autre" });
            cmbTypeFiltre.SelectedIndex = 0;
            btnEffacer.Text = "✕ Effacer"; btnEffacer.Location = new Point(624, 9); btnEffacer.Size = new Size(90, 28);
            btnEffacer.FlatStyle = FlatStyle.Flat; btnEffacer.BackColor = Color.White;
            pnlFiltres.Controls.AddRange(new Control[] { lblRecherche, txtRecherche, lblTypeFiltre, cmbTypeFiltre, btnEffacer });

            // Actions
            pnlActions.Dock = DockStyle.Top; pnlActions.Height = 46;
            pnlActions.BackColor = Color.White;

            void Btn(Button b, string t, Color c, int x)
            {
                b.Text = t; b.Location = new Point(x, 8); b.Size = new Size(160, 30);
                b.BackColor = c; b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0; b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            Btn(btnNouveauProduit, "➕ Nouveau produit", Color.FromArgb(46, 125, 50), 14);
            Btn(btnModifier, "✎ Modifier", Color.FromArgb(25, 118, 210), 182);
            Btn(btnSupprimer, "✖ Supprimer", Color.FromArgb(198, 40, 40), 350);
            btnNouveauProduit.Click += btnNouveauProduit_Click;
            btnModifier.Click += btnModifier_Click;
            btnSupprimer.Click += btnSupprimer_Click;
            pnlActions.Controls.AddRange(new Control[] { btnNouveauProduit, btnModifier, btnSupprimer });

            // Grid
            dgvProduits.Dock = DockStyle.Fill;
            dgvProduits.ReadOnly = true; dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduits.MultiSelect = false; dgvProduits.AllowUserToAddRows = false;
            dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduits.BackgroundColor = Color.White; dgvProduits.BorderStyle = BorderStyle.None;
            dgvProduits.RowTemplate.Height = 34; dgvProduits.ColumnHeadersHeight = 36;
            dgvProduits.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
            dgvProduits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduits.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProduits.EnableHeadersVisualStyles = false;
            dgvProduits.GridColor = Color.FromArgb(220, 235, 220);
            dgvProduits.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProduits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
            dgvProduits.Font = new Font("Segoe UI", 9F);

            ((System.ComponentModel.ISupportInitialize)dgvProduits).EndInit();

            this.Controls.Add(dgvProduits);
            this.Controls.Add(pnlActions);
            this.Controls.Add(pnlFiltres);
            this.Controls.Add(pnlTop);
            this.Size = new Size(1000, 640);
            this.BackColor = Color.White;
            this.ResumeLayout(false);
        }
    }
}