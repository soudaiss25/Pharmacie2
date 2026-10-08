using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Vue globale de TOUTES les commandes fournisseurs.
    /// Filtres : statut, fournisseur, période.
    /// Actions : voir détail, marquer reçue, annuler.
    /// </summary>
    public partial class Uc_Commande : UserControl
    {
        public Uc_Commande()
        {
            InitializeComponent();
            this.Load += (s, e) =>
            {
                ChargerFournisseursFiltres();
                ChargerCommandes();
            };

            cbFiltreStatut.SelectedIndexChanged += (s, e) => ChargerCommandes();
            cbFiltreFournisseur.SelectedIndexChanged += (s, e) => ChargerCommandes();
            dtpDebut.ValueChanged += (s, e) => ChargerCommandes();
            dtpFin.ValueChanged += (s, e) => ChargerCommandes();
            btnActualiser.Click += (s, e) => ChargerCommandes();
            dgvCommandes.SelectionChanged += DgvCommandes_SelectionChanged;
            btnMarquerRecu.Click += btnMarquerRecu_Click;
            btnAnnuler.Click += btnAnnuler_Click;
            btnNouvelleCommande.Click += btnNouvelleCommande_Click;
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
                            $"{montant:N0} KMF",
                            cmd.DateReception.HasValue
                                ? cmd.DateReception.Value.ToString("dd/MM/yyyy") : "—",
                            string.IsNullOrWhiteSpace(cmd.NoteCommande) ? "—" : cmd.NoteCommande
                        );

                        // Coloration selon statut
                        var row = dgvCommandes.Rows[idx];
                        switch (cmd.Statut)
                        {
                            case "Reçu":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32);
                                break;
                            case "Reçu partiellement":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                                break;
                            case "Annulée":
                                row.DefaultCellStyle.ForeColor = Color.Gray;
                                break;
                            case "En attente":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(227, 242, 253);
                                break;
                        }
                    }

                    // Récap
                    int nbTotal = commandes.Count;
                    int nbAttente = commandes.Count(c => c.Statut == "En attente");
                    int nbRecues = commandes.Count(c => c.Statut == "Reçu");
                    decimal total = commandes.Sum(c => c.Lignes.Sum(l => l.TotalLigne));

                    lblRecap.Text =
                        $"{nbTotal} commande(s)  |  " +
                        $"⏳ {nbAttente} en attente  |  " +
                        $"✅ {nbRecues} reçue(s)  |  " +
                        $"Total : {total:N0} KMF";

                    MettreAJourBoutons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message,
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
                            $"{l.PrixAchatUnitaire:N0} KMF",
                            $"{l.TotalLigne:N0} KMF",
                            l.Produit != null ? StockService.Formater(l.Produit) : "—"
                        );
                    }
                }
            }
            catch { }
        }

        private void MettreAJourBoutons()
        {
            if (dgvCommandes.SelectedRows.Count == 0)
            {
                btnMarquerRecu.Enabled = false;
                btnAnnuler.Enabled = false;
                return;
            }

            string statut = dgvCommandes.SelectedRows[0]
                .Cells["colStatut"].Value?.ToString() ?? "";

            btnMarquerRecu.Enabled = statut == "En attente" || statut == "Reçu partiellement";
            btnAnnuler.Enabled = statut == "En attente" || statut == "Reçu partiellement";
        }

        // ══════════════════════════════════════════════════════════════════
        // ACTIONS
        // ══════════════════════════════════════════════════════════════════

        private void btnMarquerRecu_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            var confirm = MessageBox.Show(
                "Marquer cette commande comme REÇUE ?\n\n" +
                "⚠️ Le stock de chaque produit sera mis à jour automatiquement.",
                "Confirmation réception",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var cmd = ctx.commandes
                        .Include(c => c.Lignes).ThenInclude(l => l.Produit)
                        .FirstOrDefault(c => c.Id == cmdId);

                    if (cmd == null) return;

                    foreach (var l in cmd.Lignes)
                    {
                        var p = ctx.produits.Find(l.ProduitId);
                        if (p != null)
                            p.QuantiteEnStock += l.Quantite;
                    }

                    cmd.Statut = "Reçu";
                    cmd.DateReception = DateTime.Now;
                    ctx.SaveChanges();
                }

                MessageBox.Show("✅ Commande reçue. Stock mis à jour.",
                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ChargerCommandes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            if (dgvCommandes.SelectedRows.Count == 0) return;

            int cmdId = Convert.ToInt32(dgvCommandes.SelectedRows[0].Cells["colCmdId"].Value);

            if (MessageBox.Show("Annuler cette commande ?", "Confirmation",
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
    }

    internal class FournisseurItem
    {
        public int Id { get; }
        public string Nom { get; }
        public FournisseurItem(int id, string nom) { Id = id; Nom = nom; }
        public override string ToString() => Nom;
    }
}