using System;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    /// <summary>
    /// Dialogue permettant de choisir la période d'export
    /// (mois courant par défaut, libre sinon).
    /// </summary>
    public partial class FormPeriodeExportMutuelle : Form
    {
        public DateTime DateDebut { get; private set; }
        public DateTime DateFin { get; private set; }

        public FormPeriodeExportMutuelle(string nomMutuelle)
        {
            InitializeComponent();
            lblNomMutuelle.Text = $"Mutuelle : {nomMutuelle}";

            // Par défaut : mois en cours
            var now = DateTime.Now;
            dtpDebut.Value = new DateTime(now.Year, now.Month, 1);
            dtpFin.Value = new DateTime(now.Year, now.Month,
                DateTime.DaysInMonth(now.Year, now.Month));

            // Raccourcis période
            cbPeriode.SelectedIndex = 0;
            cbPeriode.SelectedIndexChanged += CbPeriode_SelectedIndexChanged;
        }

        private void CbPeriode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            switch (cbPeriode.Text)
            {
                case "Mois en cours":
                    dtpDebut.Value = new DateTime(now.Year, now.Month, 1);
                    dtpFin.Value = new DateTime(now.Year, now.Month,
                        DateTime.DaysInMonth(now.Year, now.Month));
                    break;

                case "Mois précédent":
                    var prev = now.AddMonths(-1);
                    dtpDebut.Value = new DateTime(prev.Year, prev.Month, 1);
                    dtpFin.Value = new DateTime(prev.Year, prev.Month,
                        DateTime.DaysInMonth(prev.Year, prev.Month));
                    break;

                case "Trimestre en cours":
                    int q = (now.Month - 1) / 3;
                    dtpDebut.Value = new DateTime(now.Year, q * 3 + 1, 1);
                    dtpFin.Value = dtpDebut.Value.AddMonths(3).AddDays(-1);
                    break;

                case "Année en cours":
                    dtpDebut.Value = new DateTime(now.Year, 1, 1);
                    dtpFin.Value = new DateTime(now.Year, 12, 31);
                    break;
                    // "Personnalisé" : l'utilisateur saisit librement
            }
        }

        private void btnExporter_Click(object sender, EventArgs e)
        {
            if (dtpFin.Value < dtpDebut.Value)
            {
                MessageBox.Show(
                    "La date de fin doit être postérieure à la date de début.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateDebut = dtpDebut.Value.Date;
            DateFin = dtpFin.Value.Date.AddDays(1).AddSeconds(-1);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

// ── Designer inline (fichier unique pour simplifier l'intégration) ────────────
