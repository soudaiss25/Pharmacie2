using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Vente : UserControl
    {
        /// <summary>
        /// Déclenché après toute modification de vente (paiement, annulation, modification).
        /// Relais vers l'événement central VenteEvenements (Uc_Caisse s'y abonne).
        /// </summary>
        public static event EventHandler VenteModifiee
        {
            add => VenteEvenements.VenteModifiee += value;
            remove => VenteEvenements.VenteModifiee -= value;
        }

        /// <summary>Ne montre que les ventes qui restent à encaisser (crédits clients, parts de mutuelle).</summary>
        public void FiltrerCredits() => chkACaisser.Checked = true;

        public Uc_Vente()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            bool gestionnaire = Roles.EstGestionnaire(SessionUtilisateur.Courant?.Role);
            btnModifierVente.Visible = gestionnaire;
            btnAnnuler.Visible = gestionnaire;

            VenteEvenements.VenteModifiee += SurVenteModifiee;
            Disposed += (s, e) => VenteEvenements.VenteModifiee -= SurVenteModifiee;

            ChargerVentes();
        }

        private void SurVenteModifiee(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired) BeginInvoke(new Action(Rafraichir)); else Rafraichir();
        }

        // ── Méthode publique pour permettre au parent de rafraîchir ───────
        public void Rafraichir() => ChargerVentes();

        private void ChargerVentes()
        {
            string recherche = txtSearchVente.Text.Trim().ToLower();

            using (var ctx = new AppDbContext())
            {
                var query = ctx.ventes
                    .AsNoTracking()
                    .Include(v => v.Paiements)
                    .Include(v => v.User)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(recherche))
                {
                    query = query.Where(v =>
                        v.numeroVente.ToLower().Contains(recherche) ||
                        (v.NomClient != null && v.NomClient.ToLower().Contains(recherche)) ||
                        (v.PrenomClient != null && v.PrenomClient.ToLower().Contains(recherche)) ||
                        (v.TelephoneClient != null && v.TelephoneClient.Contains(recherche)));
                }

                var ventes = query.OrderByDescending(v => v.DateVente).ToList();
                if (chkACaisser.Checked)
                    ventes = ventes.Where(v => v.Statut == "Active" && v.MontantRestant > 0).ToList();

                dgvVentes.DataSource = ventes.Select(v => new
                {
                    v.IdVente,
                    Numero = v.numeroVente,
                    Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                    Tel = string.IsNullOrWhiteSpace(v.TelephoneClient) ? "—" : v.TelephoneClient,
                    Date = v.DateVente,
                    Total = v.MontantTotal,
                    Verse = v.MontantVerse,
                    Restant = v.MontantRestant,
                    Mode = v.MoyenPaiement,
                    Vendeur = v.User != null ? $"{v.User.Prenom} {v.User.Nom}" : "—",
                    v.Statut
                }).ToList();

                // ── Barre du bas — cohérente avec Uc_Caisse ───────────────
                var actives = ventes.Where(v => v.Statut == "Active").ToList();
                decimal totalCA = actives.Sum(v => v.MontantTotal);
                // Argent à récupérer : crédits clients + part de mutuelle non réglée
                decimal aRecuperer = actives.Sum(v => v.MontantRestant);

                lblNombreVentes.Text = $"{Format.Compte(ventes.Count, "vente")}";
                lblTotalVentes.Text = $"Ventes : {Format.Montant(totalCA)}";
                lblVentesCredit.Text = $"Argent à récupérer : {Format.Montant(aRecuperer)}";
                lblVentesCredit.ForeColor = aRecuperer > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;
            }
        }

        private void dgvVentes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvVentes.Rows.Count) return;
            var ligne = dgvVentes.Rows[e.RowIndex];
            string statut = ligne.Cells["Statut"].Value?.ToString() ?? "";

            if (dgvVentes.Columns[e.ColumnIndex].Name == "Date" && e.Value is DateTime date)
            {
                e.Value = Theme.DateAdaptee(date, dgvVentes.Columns[e.ColumnIndex], dgvVentes.DefaultCellStyle.Font ?? dgvVentes.Font);
                e.FormattingApplied = true;
            }

            if (statut == "Annulée")
            {
                e.CellStyle.BackColor = Theme.InfoFond;
                e.CellStyle.ForeColor = Theme.Neutre;
            }
            else if (dgvVentes.Columns[e.ColumnIndex].Name == "Restant" && ligne.Cells["Restant"].Value is decimal reste && reste > 0)
            {
                e.CellStyle.ForeColor = Theme.AttentionTexte;
                e.CellStyle.Font = Theme.Police(10, FontStyle.Bold);
            }
        }

        private int? VenteSelectionnee()
            => dgvVentes.SelectedRows.Count == 0 ? null : Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["IdVente"].Value);

        // ── Boutons ───────────────────────────────────────────────────────

        private void btnNouvelleVente_Click(object sender, EventArgs e)
        {
            using (var form = new FormVente())
            {
                form.ShowDialog(this);
                ChargerVentes();
            }
        }

        private void txtSearchVente_TextChanged(object sender, EventArgs e) => ChargerVentes();

        private void chkACaisser_CheckedChanged(object sender, EventArgs e) => ChargerVentes();

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (VenteSelectionnee() is not int id)
            {
                MessageBox.Show("Sélectionnez une vente.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var form = new FormDetailVente(id))
                form.ShowDialog(this);
        }

        private void btnModifierVente_Click(object sender, EventArgs e)
        {
            if (!Roles.EstGestionnaire(SessionUtilisateur.Courant?.Role))
            {
                MessageBox.Show("Accès réservé à l'administrateur ou au pharmacien.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (VenteSelectionnee() is not int id)
            {
                MessageBox.Show("Sélectionnez une vente à modifier.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string statut = dgvVentes.SelectedRows[0].Cells["Statut"].Value?.ToString() ?? "";
            if (statut == "Annulée")
            {
                MessageBox.Show("Impossible de modifier une vente annulée.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new FormModificationVente(id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerVentes();
            }
        }

        private void btnEnregistrerPaiement_Click(object sender, EventArgs e)
        {
            if (VenteSelectionnee() is not int id)
            {
                MessageBox.Show("Sélectionnez une vente.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes.Include(v => v.Paiements).FirstOrDefault(v => v.IdVente == id);
                if (vente == null) return;

                if (vente.Statut == "Annulée")
                {
                    MessageBox.Show("Impossible d'encaisser une vente annulée.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (vente.RestantPatient <= 0)
                {
                    MessageBox.Show(vente.MontantRestant > 0
                            ? "Le patient a tout payé. La part de la mutuelle se règle depuis l'écran Mutuelles."
                            : "Cette vente est déjà soldée.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            using (var form = new FormPaiements(id))
                form.ShowDialog(this);

            ChargerVentes();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (SessionUtilisateur.Courant?.Role != Roles.Administrateur)
            {
                MessageBox.Show("Seul l'administrateur peut annuler une vente.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (VenteSelectionnee() is not int id)
            {
                MessageBox.Show("Sélectionnez une vente à annuler.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new FormAnnulationVente(id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerVentes();
            }
        }
    }
}
