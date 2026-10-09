using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Vue globale de TOUTES les commandes fournisseurs.
    /// Filtres : statut, fournisseur, période.
    /// Actions : voir détail, marquer reçue, annuler.
    /// </summary>
    public partial class Uc_Commande : UserControl, IModeCompact
    {
        public Uc_Commande()
        {
            InitializeComponent();
            Theme.Appliquer(this);
            this.Load += (s, e) =>
            {
                ChargerFournisseursFiltres();
                ChargerCommandes();
            };

            cbFiltreStatut.SelectedIndexChanged += (s, e) => ChargerCommandes();
            cbFiltreFournisseur.SelectedIndexChanged += (s, e) => ChargerCommandes();
            dtpDebut.ValueChanged += (s, e) => ChargerCommandes();
            dtpFin.ValueChanged += (s, e) => ChargerCommandes();
        }

        // ══════════════════════════════════════════════════════════════════
        // CHARGEMENT
        // ══════════════════════════════════════════════════════════════════

        private void ChargerFournisseursFiltres()
        {
            using (var ctx = new AppDbContext())
            {
                cbFiltreFournisseur.Items.Clear();
                cbFiltreFournisseur.Items.Add("Tous les fournisseurs");
                foreach (var f in ctx.fournisseur.OrderBy(f => f.Nom).ToList())
                    cbFiltreFournisseur.Items.Add(new FournisseurItem(f.Id, f.Nom));
                cbFiltreFournisseur.SelectedIndex = 0;
            }
        }

        private void ChargerCommandes()
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.commandes
                        .Include(c => c.Fournisseur)
                        .Include(c => c.Lignes).ThenInclude(l => l.Produit)
                        .AsQueryable();

                    // Filtre statut
                    string statut = cbFiltreStatut.SelectedItem?.ToString() ?? "Tous";
                    if (statut != "Tous")
                        query = query.Where(c => c.Statut == statut);

                    // Filtre fournisseur
                    if (cbFiltreFournisseur.SelectedItem is FournisseurItem fi)
                        query = query.Where(c => c.FournisseurId == fi.Id);

                    // Filtre période
                    if (chkPeriode.Checked)
                    {
                        DateTime debut = dtpDebut.Value.Date;
                        DateTime fin = dtpFin.Value.Date.AddDays(1).AddSeconds(-1);
                        query = query.Where(c => c.DateCommande >= debut && c.DateCommande <= fin);
                    }

                    var commandes = query.OrderByDescending(c => c.DateCommande).ToList();

                    dgvCommandes.Rows.Clear();

                    foreach (var cmd in commandes)
                    {
                        string nomFourn = cmd.Fournisseur?.Nom ?? "—";
                        int nbProduits = cmd.Lignes.Count;
                        decimal montant = cmd.Lignes.Sum(l => l.TotalLigne);

                        int idx = dgvCommandes.Rows.Add(
                            cmd.Id,
                            cmd.DateCommande.ToString("dd/MM/yyyy HH:mm"),
                            nomFourn,
                            cmd.Statut,
                            nbProduits,
                            montant,
                            cmd.DateReception.HasValue
                                ? cmd.DateReception.Value.ToString("dd/MM/yyyy") : "—",
                            string.IsNullOrWhiteSpace(cmd.NoteCommande) ? "—" : cmd.NoteCommande
                        );

                        // Coloration selon statut
                        var row = dgvCommandes.Rows[idx];
                        switch (cmd.Statut)
                        {
                            case "Reçu":
                                row.DefaultCellStyle.BackColor = Theme.SuccesFond;
                                row.DefaultCellStyle.ForeColor = Theme.SuccesTexte;
                                break;
                            case "Reçu partiellement":
                                row.DefaultCellStyle.BackColor = Theme.AttentionFond;
                                row.DefaultCellStyle.ForeColor = Theme.AttentionTexte;
                                break;
                            case "Annulée":
                                row.DefaultCellStyle.ForeColor = Theme.Neutre;
                                break;
                            case "En attente":
                                row.DefaultCellStyle.BackColor = Theme.InfoFond;
                                break;
                        }
                    }

                    // Récap
                    int nbTotal = commandes.Count;
                    int nbAttente = commandes.Count(c => c.Statut == "En attente");
                    int nbRecues = commandes.Count(c => c.Statut == "Reçu");
                    decimal total = commandes.Sum(c => c.Lignes.Sum(l => l.TotalLigne));

                    lblRecap.Text =
                        $"{Format.Compte(nbTotal, "commande")}  |  " +
                        $"{nbAttente} en attente  |  " +
                        $"{Format.Compte(nbRecues, "reçue")}  |  " +
                        $"Total : {Format.Montant(total)}";

                    MettreAJourBoutons();
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Chargement des commandes", ex);
                MessageBox.Show("Les commandes n'ont pas pu être affichées : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Sélection → charge le détail des lignes ───────────────────────

        private void DgvCommandes_SelectionChanged(object sender, EventArgs e)
        {
            dgvLignes.Rows.Clear();
            MettreAJourBoutons();

            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            if (dgvCommandes.SelectedRows[0].Cells["colStatut"].Value?.ToString() == "Reçu partiellement"
                && CommandeService.EstPartielleHeritee(cmdId))
                MessageBox.Show(CommandeService.MessageQuantiteInconnue, "Attention",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var lignes = ctx.LigneCommandes
                        .Include(l => l.Produit)
                        .Where(l => l.CommandeId == cmdId)
                        .ToList();

                    foreach (var l in lignes)
                    {
                        dgvLignes.Rows.Add(
                            l.Produit?.Nom ?? "—",
                            l.Quantite,
                            l.PrixAchatUnitaire,
                            l.TotalLigne,
                            l.Produit != null ? StockService.Formater(l.Produit) : "—"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Chargement des lignes de la commande", ex);
                MessageBox.Show("Impossible d'afficher le détail de la commande : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MettreAJourBoutons()
        {
            if (dgvCommandes.SelectedRows.Count == 0)
            {
                btnMarquerRecu.Enabled = false;
                btnMarquerPartiel.Enabled = false;
                btnAnnuler.Enabled = false;
                return;
            }

            string statut = dgvCommandes.SelectedRows[0]
                .Cells["colStatut"].Value?.ToString() ?? "";

            bool ouverte = statut == "En attente" || statut == "Reçu partiellement";
            bool heritee = statut == "Reçu partiellement"
                && CommandeService.EstPartielleHeritee(Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value));
            btnMarquerRecu.Enabled = ouverte && !heritee;   // une partielle ancienne se reçoit ligne par ligne
            btnMarquerPartiel.Enabled = ouverte;
            btnAnnuler.Enabled = ouverte;
        }

        // ══════════════════════════════════════════════════════════════════
        // ACTIONS
        // ══════════════════════════════════════════════════════════════════

        private void btnMarquerRecu_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            var confirm = MessageBox.Show(
                "Tout le reste de cette commande est-il arrivé ?\n\n" +
                "Le stock de chaque produit sera mis à jour.",
                "Confirmation réception",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                CommandeService.Receptionner(cmdId);

                BandeauNotification.Succes("Commande reçue. Le stock est à jour.");

                ChargerCommandes();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Réception de la commande " + cmdId, ex);
                MessageBox.Show("Erreur : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualiser_Click(object sender, EventArgs e) => ChargerCommandes();

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

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            if (MessageBox.Show("Annuler cette commande ? Cette action ne peut pas être défaite.", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using (var ctx = new AppDbContext())
            {
                var cmd = ctx.commandes.Find(cmdId);
                if (cmd != null) { cmd.Statut = "Annulée"; ctx.SaveChanges(); }
            }

            ChargerCommandes();
        }

        private void btnNouvelleCommande_Click(object sender, EventArgs e)
        {
            // Étape 1 : choisir le produit à commander
            using (var choix = new FormChoixProduit())
            {
                // On veut juste choisir le produit, pas valider une vente
                // FormChoixProduit renvoie le produit sélectionné
                if (choix.ShowDialog() != DialogResult.OK) return;

                var produit = choix.ProduitSelectionne;
                if (produit == null) return;

                // Étape 2 : ouvrir le formulaire de commande avec le produit choisi
                using (var form = new FormCommandeProduit(produit.Id, produit.Nom))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                        ChargerCommandes();
                }
            }
        }

        private void chkPeriode_CheckedChanged(object sender, EventArgs e)
        {
            dtpDebut.Enabled = chkPeriode.Checked;
            dtpFin.Enabled = chkPeriode.Checked;
            ChargerCommandes();
        }

        /// <summary>Mode compact : les colonnes secondaires sont masquées, l'essentiel reste visible.</summary>
        public void DefinirCompact(bool compact)
        {
            ModeCompact.MasquerColonnes(dgvCommandes, compact, "colNbProd", "colReception", "colNote");
            Theme.AjusterColonnes(dgvCommandes);
        }
    }

    internal class FournisseurItem
    {
        public int Id { get; }
        public string Nom { get; }
        public FournisseurItem(int id, string nom) { Id = id; Nom = nom; }
        public override string ToString() => Nom;
        }
}