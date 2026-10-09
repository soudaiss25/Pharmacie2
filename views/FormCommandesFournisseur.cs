using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    /// <summary>
    /// Affiche toutes les commandes passées à un fournisseur.
    /// AMÉLIORATIONS :
    ///   - Colonne Statut visible (En attente / Reçu partiellement / Reçu / Annulée)
    ///   - Bouton "Marquer comme reçue" → met à jour le stock automatiquement
    ///   - Bouton "Annuler commande"
    ///   - Filtre par statut
    ///   - Détail de la commande avec note et date livraison prévue
    /// </summary>
    public partial class FormCommandesFournisseur : Form
    {
        private readonly int _fournisseurId;
        private readonly string _fournisseurNom;

        public FormCommandesFournisseur(int fournisseurId, string fournisseurNom)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            dgvCommandes.CellFormatting += dgvCommandes_CellFormatting;
            _fournisseurId = fournisseurId;
            _fournisseurNom = fournisseurNom;
            lblTitre.Text = $"Commandes — {fournisseurNom}";

            cbFiltreStatut.SelectedIndexChanged += (s, e) => ChargerCommandes();

            ChargerCommandes();
        }

        // ── Chargement commandes ──────────────────────────────────────────

        private void dgvCommandes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCommandes.Rows.Count) return;
            string statut = dgvCommandes.Rows[e.RowIndex].Cells["colStatut"].Value?.ToString() ?? "";
            string niveau = statut switch { "Reçu" => "succes", "Reçu partiellement" => "attention", "Annulée" => "info", _ => "" };
            Theme.ColorerEtat(dgvCommandes, e, niveau, "colStatut");
        }

        private void ChargerCommandes()
        {
            using (var ctx = new AppDbContext())
            {
                var query = ctx.commandes
                    .Include(c => c.Lignes).ThenInclude(l => l.Produit)
                    .Where(c => c.FournisseurId == _fournisseurId)
                    .AsQueryable();

                // Filtre statut
                string filtre = cbFiltreStatut.SelectedItem?.ToString() ?? "Tous";
                if (filtre != "Tous")
                    query = query.Where(c => c.Statut == filtre);

                var commandes = query.OrderByDescending(c => c.DateCommande).ToList();

                dgvCommandes.Rows.Clear();
                foreach (var cmd in commandes)
                {
                    string livraison = cmd.DateLivraisonPrevue.HasValue
                        ? cmd.DateLivraisonPrevue.Value.ToString("dd/MM/yyyy")
                        : "—";

                    string reception = cmd.DateReception.HasValue
                        ? cmd.DateReception.Value.ToString("dd/MM/yyyy HH:mm")
                        : "—";

                    int idx = dgvCommandes.Rows.Add(
                        cmd.Id,
                        cmd.DateCommande.ToString("dd/MM/yyyy HH:mm"),
                        cmd.Statut,
                        cmd.Lignes.Count,
                        cmd.MontantTotal,
                        livraison,
                        reception,
                        string.IsNullOrWhiteSpace(cmd.NoteCommande) ? "—" : cmd.NoteCommande
                    );

                    // la coloration selon le statut se fait à l'affichage (cellule « Statut » en gras, fond de ligne léger)

                }

                int nbTotal = commandes.Count;
                int nbAttente = commandes.Count(c => c.Statut == "En attente");
                int nbRecu = commandes.Count(c => c.Statut == "Reçu");
                decimal total = commandes.Sum(c => c.MontantTotal);

                lblNbCommandes.Text =
                    $"{Format.Compte(nbTotal, "commande")}  |  " +
                    $"{nbAttente} en attente  |  " +
                    $"{Format.Compte(nbRecu, "reçue")}  |  " +
                    $"Total : {Format.Montant(total)}";

                MettreAJourBoutons();
            }
        }

        // ── Sélection d'une commande → charge le détail ───────────────────

        private void dgvCommandes_SelectionChanged(object sender, EventArgs e)
        {
            dgvLignes.Rows.Clear();
            MettreAJourBoutons();

            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            if (dgvCommandes.SelectedRows[0].Cells["colStatut"].Value?.ToString() == "Reçu partiellement"
                && CommandeService.EstPartielleHeritee(cmdId))
                MessageBox.Show(CommandeService.MessageQuantiteInconnue, "Attention",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            using (var ctx = new AppDbContext())
            {
                var lignes = ctx.LigneCommandes
                    .Include(l => l.Produit)
                    .Where(l => l.CommandeId == cmdId)
                    .ToList();

                foreach (var l in lignes)
                {
                    dgvLignes.Rows.Add(
                        l.Id,
                        l.Produit?.Nom ?? "—",
                        l.Quantite,
                        l.PrixAchatUnitaire,
                        l.TotalLigne,
                        l.Produit != null ? StockService.Formater(l.Produit) : "—"
                    );
                }
            }
        }

        // ── Activer/désactiver les boutons selon le statut sélectionné ────

        private void MettreAJourBoutons()
        {
            if (dgvCommandes.SelectedRows.Count == 0)
            {
                btnMarquerRecu.Enabled = false;
                btnMarquerPartiel.Enabled = false;
                btnAnnuler.Enabled = false;
                return;
            }

            string statut = dgvCommandes.SelectedRows[0].Cells["colStatut"].Value?.ToString() ?? "";

            bool ouverte = statut == "En attente" || statut == "Reçu partiellement";
            bool heritee = statut == "Reçu partiellement"
                && CommandeService.EstPartielleHeritee(Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value));
            btnMarquerRecu.Enabled = ouverte && !heritee;   // une partielle ancienne se reçoit ligne par ligne
            btnMarquerPartiel.Enabled = ouverte;
            btnAnnuler.Enabled = ouverte;
        }

        // ── Marquer comme REÇUE → met à jour le stock ────────────────────

        private void btnMarquerRecu_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);
            string statut = dgvCommandes.SelectedRows[0].Cells["colStatut"].Value?.ToString() ?? "";

            var confirm = MessageBox.Show(
                "Tout le reste de cette commande est-il arrivé ?\n\n" +
                "Le stock de chaque produit sera mis à jour.",
                "Confirmation réception",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                CommandeService.Receptionner(cmdId);

                BandeauNotification.Succes("Commande reçue. Le stock est à jour.");

                ChargerCommandes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Réception partielle : grille ligne par ligne ──────────────────

        private void btnMarquerPartiel_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;
            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);
            using (var form = new FormReceptionPartielle(cmdId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerCommandes();
            }
        }

        // ── Annuler une commande ──────────────────────────────────────────

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            var confirm = MessageBox.Show(
                "Annuler cette commande ?\nCette action ne peut pas être défaite.",
                "Confirmation annulation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            using (var ctx = new AppDbContext())
            {
                var commande = ctx.commandes.Find(cmdId);
                if (commande == null) return;

                commande.Statut = "Annulée";
                ctx.SaveChanges();
            }

            ChargerCommandes();
        }

        private void btnFermer_Click(object sender, EventArgs e) => Close();
    }
}