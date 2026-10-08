using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Vente : UserControl
    {
        /// <summary>
        /// Déclenché après toute modification de vente (paiement, annulation, modification).
        /// Uc_Caisse s'y abonne pour se rafraîchir automatiquement.
        /// </summary>
        public static event EventHandler VenteModifiee
        {
            add => VenteEvenements.VenteModifiee += value;
            remove => VenteEvenements.VenteModifiee -= value;
        }

        private bool _creditsSeulement;

        /// <summary>Ne montre que les ventes qui restent à encaisser (crédits clients, parts de mutuelle).</summary>
        public void FiltrerCredits()
        {
            _creditsSeulement = true;
            ChargerVentes(txtSearchVente.Text);
        }

        public Uc_Vente()
        {
            InitializeComponent();

            bool isAdmin = SessionUtilisateur.Courant?.Role == Roles.Administrateur
                        || SessionUtilisateur.Courant?.Role == Roles.Pharmacien;

            btnModifierVente.Visible = isAdmin;
            btnAnnuler.Visible = isAdmin;

            ChargerVentes();
        }

        // ── Méthode publique pour permettre au parent de rafraîchir ───────
        public void Rafraichir() => ChargerVentes(txtSearchVente.Text);

        private void ChargerVentes(string recherche = "")
        {
            using (var ctx = new AppDbContext())
            {
                var query = ctx.ventes
                    .Include(v => v.Paiements)
                    .Include(v => v.User)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(recherche))
                {
                    recherche = recherche.ToLower();
                    query = query.Where(v =>
                        v.numeroVente.ToLower().Contains(recherche) ||
                        (v.NomClient != null && v.NomClient.ToLower().Contains(recherche)) ||
                        (v.PrenomClient != null && v.PrenomClient.ToLower().Contains(recherche)) ||
                        (v.TelephoneClient != null && v.TelephoneClient.Contains(recherche)));
                }

                var ventes = query.OrderByDescending(v => v.DateVente).ToList();
                if (_creditsSeulement)
                    ventes = ventes.Where(v => v.Statut == "Active" && v.MontantRestant > 0).ToList();

                dgvVentes.Rows.Clear();

                foreach (var v in ventes)
                {
                    int idx = dgvVentes.Rows.Add(
                        v.IdVente,
                        v.numeroVente,
                        $"{v.PrenomClient} {v.NomClient}".Trim(),
                        v.TelephoneClient ?? "—",
                        v.DateVente.ToString("dd/MM/yyyy HH:mm"),
                        v.MontantTotal.ToString("N0"),
                        v.MontantVerse.ToString("N0"),
                        v.MontantRestant.ToString("N0"),
                        v.MoyenPaiement,
                        v.User != null ? $"{v.User.Prenom} {v.User.Nom}" : "—",
                        v.Statut
                    );

                    if (v.Statut == "Annulée")
                    {
                        dgvVentes.Rows[idx].DefaultCellStyle.BackColor =
                            System.Drawing.Color.FromArgb(255, 235, 238);
                        dgvVentes.Rows[idx].DefaultCellStyle.ForeColor =
                            System.Drawing.Color.OrangeRed;
                    }
                    else if (v.MontantRestant > 0)
                    {
                        dgvVentes.Rows[idx].Cells["colMontantRestant"].Style.ForeColor =
                            System.Drawing.Color.OrangeRed;
                        dgvVentes.Rows[idx].Cells["colMontantRestant"].Style.Font =
                            new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                    }
                }

                // ── Barre du bas — cohérente avec Uc_Caisse ───────────────
                var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();

                // CA = total facturé (toutes ventes actives)
                decimal totalCA = ventesActives.Sum(v => v.MontantTotal);

                // Crédit en cours = restant dû sur ventes actives
                // Inclut : crédit client + part mutuelle non réglée
                decimal creditEnCours = ventesActives.Sum(v => v.MontantRestant);

                lblNombreVentes.Text = $"Total : {ventes.Count} vente(s)";
                lblTotalVentes.Text = $"CA : {totalCA:N0} KMF";
                lblVentesCredit.Text = $"Crédit en cours : {creditEnCours:N0} KMF";
                lblVentesCredit.ForeColor = creditEnCours > 0
                    ? System.Drawing.Color.OrangeRed
                    : System.Drawing.Color.FromArgb(27, 94, 32);
            }
        }

        // ── Boutons ───────────────────────────────────────────────────────

        private void btnNouvelleVente_Click(object sender, EventArgs e)
        {
            using (var form = new FormVente())
            {
                form.ShowDialog();
                ChargerVentes(txtSearchVente.Text);
                VenteEvenements.Notifier(this);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
            => ChargerVentes(txtSearchVente.Text);

        private void txtSearchVente_TextChanged(object sender, EventArgs e)
            => ChargerVentes(txtSearchVente.Text);

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (dgvVentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez une vente.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colIdVente"].Value);
            using (var form = new FormDetailVente(id))
                form.ShowDialog();
        }

        private void btnModifierVente_Click(object sender, EventArgs e)
        {
            if (SessionUtilisateur.Courant?.Role != Roles.Administrateur
             && SessionUtilisateur.Courant?.Role != Roles.Pharmacien)
            {
                MessageBox.Show("Accès réservé à l'administrateur ou au pharmacien.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvVentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez une vente à modifier.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string statut = dgvVentes.SelectedRows[0].Cells["colStatut"].Value?.ToString();
            if (statut == "Annulée")
            {
                MessageBox.Show("Impossible de modifier une vente annulée.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colIdVente"].Value);
            using (var form = new FormModificationVente(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ChargerVentes(txtSearchVente.Text);
                    VenteEvenements.Notifier(this); // ← notifie Uc_Caisse
                }
            }
        }

        private void btnEnregistrerPaiement_Click(object sender, EventArgs e)
        {
            if (dgvVentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez une vente.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colIdVente"].Value);

            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes.Include(v => v.Paiements)
                    .FirstOrDefault(v => v.IdVente == id);
                if (vente == null) return;

                if (vente.Statut == "Annulée")
                {
                    MessageBox.Show("Impossible d'encaisser une vente annulée.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (vente.MontantRestant <= 0)
                {
                    MessageBox.Show("Cette vente est déjà soldée.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            using (var form = new FormPaiements(id))
                form.ShowDialog();

            ChargerVentes(txtSearchVente.Text);
            VenteEvenements.Notifier(this); // ← notifie Uc_Caisse
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (SessionUtilisateur.Courant?.Role != Roles.Administrateur)
            {
                MessageBox.Show("Seul l'administrateur peut annuler une vente.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvVentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez une vente à annuler.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colIdVente"].Value);
            using (var form = new FormAnnulationVente(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ChargerVentes(txtSearchVente.Text);
                    VenteEvenements.Notifier(this); // ← notifie Uc_Caisse
                }
            }
        }
    }
}