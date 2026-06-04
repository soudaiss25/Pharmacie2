
namespace Pharmacie2.views
{
    partial class FormPeriodeExportMutuelle
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblNomMutuelle;
        private System.Windows.Forms.Label lblChoixPeriode;
        private System.Windows.Forms.ComboBox cbPeriode;
        private System.Windows.Forms.Label lblDebut;
        private System.Windows.Forms.DateTimePicker dtpDebut;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Button btnExporter;
        private System.Windows.Forms.Button btnAnnuler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new System.Windows.Forms.Label();
            lblNomMutuelle = new System.Windows.Forms.Label();
            lblChoixPeriode = new System.Windows.Forms.Label();
            cbPeriode = new System.Windows.Forms.ComboBox();
            lblDebut = new System.Windows.Forms.Label();
            dtpDebut = new System.Windows.Forms.DateTimePicker();
            lblFin = new System.Windows.Forms.Label();
            dtpFin = new System.Windows.Forms.DateTimePicker();
            btnExporter = new System.Windows.Forms.Button();
            btnAnnuler = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // Titre
            lblTitre.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitre.Height = 50;
            lblTitre.Text = "📊  Export Excel — Période";
            lblTitre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitre.Font = new System.Drawing.Font(
                "Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitre.BackColor = System.Drawing.Color.FromArgb(30, 120, 50);
            lblTitre.ForeColor = System.Drawing.Color.White;

            // Mutuelle
            lblNomMutuelle.Location = new System.Drawing.Point(18, 62);
            lblNomMutuelle.Size = new System.Drawing.Size(424, 22);
            lblNomMutuelle.Font = new System.Drawing.Font(
                "Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblNomMutuelle.ForeColor = System.Drawing.Color.FromArgb(27, 94, 32);

            // Raccourci période
            lblChoixPeriode.Text = "Raccourci :";
            lblChoixPeriode.Location = new System.Drawing.Point(18, 100);
            lblChoixPeriode.Size = new System.Drawing.Size(80, 22);
            lblChoixPeriode.Font = new System.Drawing.Font("Segoe UI", 9F);

            cbPeriode.Location = new System.Drawing.Point(100, 97);
            cbPeriode.Size = new System.Drawing.Size(200, 28);
            cbPeriode.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbPeriode.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cbPeriode.Items.AddRange(new object[] {
                "Mois en cours", "Mois précédent",
                "Trimestre en cours", "Année en cours", "Personnalisé" });

            // Date début
            lblDebut.Text = "Du :";
            lblDebut.Location = new System.Drawing.Point(18, 144);
            lblDebut.Size = new System.Drawing.Size(50, 22);
            lblDebut.Font = new System.Drawing.Font("Segoe UI", 9F);

            dtpDebut.Location = new System.Drawing.Point(72, 141);
            dtpDebut.Size = new System.Drawing.Size(160, 28);
            dtpDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // Date fin
            lblFin.Text = "Au :";
            lblFin.Location = new System.Drawing.Point(248, 144);
            lblFin.Size = new System.Drawing.Size(40, 22);
            lblFin.Font = new System.Drawing.Font("Segoe UI", 9F);

            dtpFin.Location = new System.Drawing.Point(292, 141);
            dtpFin.Size = new System.Drawing.Size(160, 28);
            dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // Boutons
            btnExporter.Text = "📊 Exporter";
            btnExporter.Location = new System.Drawing.Point(200, 188);
            btnExporter.Size = new System.Drawing.Size(130, 36);
            btnExporter.BackColor = System.Drawing.Color.FromArgb(30, 120, 50);
            btnExporter.ForeColor = System.Drawing.Color.White;
            btnExporter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExporter.FlatAppearance.BorderSize = 0;
            btnExporter.Font = new System.Drawing.Font(
                "Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnExporter.Click += new System.EventHandler(this.btnExporter_Click);

            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new System.Drawing.Point(340, 188);
            btnAnnuler.Size = new System.Drawing.Size(100, 36);
            btnAnnuler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);

            this.ClientSize = new System.Drawing.Size(460, 242);
            this.Text = "Période d'export";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitre, lblNomMutuelle,
                lblChoixPeriode, cbPeriode,
                lblDebut, dtpDebut, lblFin, dtpFin,
                btnExporter, btnAnnuler
            });

            this.ResumeLayout(false);
        }
    }
}
