using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

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
            _fournisseurId = fournisseurId;
            _fournisseurNom = fournisseurNom;
            lblTitre.Text = $"📋  Commandes — {fournisseurNom}";

            cbFiltreStatut.SelectedIndex = 0;
            cbFiltreStatut.SelectedIndexChanged += (s, e) => ChargerCommandes();

            ChargerCommandes();
        }

        // ── Chargement commandes ──────────────────────────────────────────

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
                        cmd.MontantTotal.ToString("N0") + " KMF",
                        livraison,
                        reception,
                        string.IsNullOrWhiteSpace(cmd.NoteCommande) ? "—" : cmd.NoteCommande
                    );

                    // Coloration selon statut
                    var row = dgvCommandes.Rows[idx];
                    switch (cmd.Statut)
                    {
                        case "Reçu":
                            row.DefaultCellStyle.BackColor =
                                System.Drawing.Color.FromArgb(220, 245, 220);
                            row.DefaultCellStyle.ForeColor =
                                System.Drawing.Color.FromArgb(27, 94, 32);
                            break;
                        case "Reçu partiellement":
                            row.DefaultCellStyle.BackColor =
                                System.Drawing.Color.FromArgb(255, 243, 205);
                            break;
                        case "Annulée":
                            row.DefaultCellStyle.ForeColor =
                                System.Drawing.Color.FromArgb(160, 160, 160);
                            break;
                    }
                }

                int nbTotal = commandes.Count;
                int nbAttente = commandes.Count(c => c.Statut == "En attente");
                int nbRecu = commandes.Count(c => c.Statut == "Reçu");
                decimal total = commandes.Sum(c => c.MontantTotal);

                lblNbCommandes.Text =
                    $"{nbTotal} commande(s)  |  " +
                    $"⏳ {nbAttente} en attente  |  " +
                    $"✅ {nbRecu} reçue(s)  |  " +
                    $"Total : {total:N0} KMF";

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
                        l.PrixAchatUnitaire.ToString("N0") + " KMF",
                        l.TotalLigne.ToString("N0") + " KMF",
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

            btnMarquerRecu.Enabled = statut == "En attente" || statut == "Reçu partiellement";
            btnMarquerPartiel.Enabled = statut == "En attente" || statut == "Reçu partiellement";
            btnAnnuler.Enabled = statut == "En attente" || statut == "Reçu partiellement";
        }

        // ── Marquer comme REÇUE → met à jour le stock ────────────────────

        private void btnMarquerRecu_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);
            string statut = dgvCommandes.SelectedRows[0].Cells["colStatut"].Value?.ToString() ?? "";

            var confirm = MessageBox.Show(
                "Marquer cette commande comme REÇUE ?\n\n" +
                "⚠️ Le stock de chaque produit sera automatiquement mis à jour.",
                "Confirmation réception",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                CommandeService.Receptionner(cmdId);

                MessageBox.Show(
                    "✅ Commande marquée comme reçue.\nLe stock a été mis à jour.",
                    "Réception enregistrée",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ChargerCommandes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Marquer comme REÇUE PARTIELLEMENT ────────────────────────────

        private void btnMarquerPartiel_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            // Demander la quantité réellement reçue (en boîtes) pour chaque ligne
            try
            {
                List<LigneCommande> lignes;
                using (var ctx = new AppDbContext())
                {
                    lignes = ctx.LigneCommandes
                        .Include(l => l.Produit)
                        .Where(l => l.CommandeId == cmdId)
                        .ToList();
                }

                bool legacy = CommandeService.EstPartielleHeritee(cmdId);
                var quantites = new Dictionary<int, int>();
                foreach (var ligne in lignes)
                {
                    int reste = ligne.Quantite - ligne.QuantiteRecue;
                    if (reste <= 0) continue;

                    string nomProduit = ligne.Produit?.Nom ?? $"Produit #{ligne.ProduitId}";

                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Quantité reçue (en boîtes) pour « {nomProduit} » :\n(commandée : {ligne.Quantite}, déjà reçue : {ligne.QuantiteRecue})",
                        "Réception partielle",
                        legacy ? "" : reste.ToString());

                    if (!int.TryParse(input, out int qteRecue) || qteRecue <= 0)
                        continue;

                    quantites[ligne.Id] = qteRecue;
                }

                if (quantites.Count > 0 && CommandeService.Receptionner(cmdId, quantites) > 0)
                {
                    MessageBox.Show(
                        "✅ Réception enregistrée.\nLe stock a été mis à jour.",
                        "Réception partielle",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Réception partielle de la commande " + cmdId, ex);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ChargerCommandes();
        }

        // ── Annuler une commande ──────────────────────────────────────────

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            var confirm = MessageBox.Show(
                "Annuler cette commande ?\nCette action ne peut pas être annulée.",
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