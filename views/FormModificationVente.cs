using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;

namespace Pharmacie2.views
{
    /// <summary>
    /// Permet de modifier une vente ACTIVE.
    /// CORRECTIONS :
    ///   - Stock géré en unités de base (plus de ceil)
    ///   - MutuelleReglee réinitialisé si on change le mode de paiement
    /// </summary>
    public partial class FormModificationVente : Form
    {
        private readonly int _venteId;
        private List<LigneVenteModif> _lignes = new List<LigneVenteModif>();
        private List<Mutuel> _mutuels;
        private int? _mutuelIdOrigine;   // mutuelle d'origine de la vente, présélectionnée dans la liste
        private decimal _total;
        private decimal _verseActuel;                          // total net des paiements déjà enregistrés (remboursements déduits)
        private string _modeRemboursement = ModesPaiement.Especes;   // mode du dernier paiement reçu : on rend dans le même mode
        private decimal _tauxMutuelleOrigine;
        // Unités de base déjà retirées du stock par la vente d'origine, par produit
        private Dictionary<int, int> _unitesOriginales = new Dictionary<int, int>();

        private class LigneVenteModif
        {
            public int? Id { get; set; }
            public int ProduitId { get; set; }
            public string ProduitNom { get; set; }
            public int Quantite { get; set; }
            public decimal PrixUnitaire { get; set; }
            public decimal? CoutAchatUnitaire { get; set; }   // coût figé d'une ligne existante : conservé tel quel
            public string UniteVendue { get; set; }
            public bool Supprimee { get; set; }
            public decimal SousTotal => Quantite * PrixUnitaire;
        }

        public FormModificationVente(int venteId)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _venteId = venteId;
            // ✅ ORDRE CRITIQUE :
            // 1. Charger mutuelles AVANT le combo (evite _mutuels null dans SelectedIndexChanged)
            // 2. Init combo AVANT ChargerVente (evite cbPaiement.Text sans items)
            // 3. ChargerVente en dernier (assigne cbPaiement.Text quand items existent)
            ChargerMutuelles();
            InitComboMoyenPaiement();
            ChargerVente();
        }

        // ─── Initialisation ──────────────────────────────────────────────

        private void ChargerVente()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                    .Include(v => v.Mutuel)
                    .Include(v => v.Paiements)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null)
                {
                    MessageBox.Show("Vente introuvable.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close(); return;
                }

                if (vente.Statut == "Annulée")
                {
                    MessageBox.Show("Impossible de modifier une vente annulée.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close(); return;
                }

                // Vente déjà réglée par la mutuelle : plus modifiable
                if (vente.MutuelId != null && vente.MutuelleReglee)
                {
                    MessageBox.Show(VenteModificationRegles.MessageVenteReglee, "Modification impossible",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Load += (s, e) => Close();
                    return;
                }

                _mutuelIdOrigine = vente.MutuelId;
                _tauxMutuelleOrigine = vente.TauxMutuelle;
                _verseActuel = vente.Paiements.Sum(p => p.Montant);
                _modeRemboursement = vente.Paiements.Where(p => p.Montant > 0).OrderByDescending(p => p.DatePaiement).ThenByDescending(p => p.Id)
                                         .Select(p => p.Mode).FirstOrDefault() ?? ModesPaiement.Especes;

                txtNom.Text = vente.NomClient;
                txtPrenom.Text = vente.PrenomClient;
                txtTelephone.Text = vente.TelephoneClient;
                txtMotif.Text = vente.MotifAchat;
                txtMatricule.Text = vente.MatriculeEmploye;

                _unitesOriginales = vente.Lignes
                    .GroupBy(l => l.ProduitId)
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.QuantiteUnites));

                _lignes = vente.Lignes.Select(l => new LigneVenteModif
                {
                    Id = l.Id,
                    ProduitId = l.ProduitId,
                    ProduitNom = l.Produit?.Nom ?? "—",
                    Quantite = l.Quantite,
                    PrixUnitaire = l.PrixUnitaire,
                    CoutAchatUnitaire = l.CoutAchatUnitaire,
                    UniteVendue = l.UniteVendue
                }).ToList();

                // Assigner via SelectedIndex (fonctionne meme si Text ne trouve pas l'item)
                int modeIdx = cbPaiement.FindStringExact(vente.MoyenPaiement ?? "Comptant");
                cbPaiement.SelectedIndex = modeIdx >= 0 ? modeIdx : 0;
                RafraichirListView();
                CalculerTotal();
            }
        }

        private void InitComboMoyenPaiement()
        {
            cbPaiement.Items.Clear();
            cbPaiement.Items.AddRange(ModesPaiement.Tous);
            cbModeComplement.Items.AddRange(ModesPaiement.ModesDeReception);
            cbModeComplement.SelectedItem = ModesPaiement.Especes;   // par défaut
        }

        private void ChargerMutuelles()
        {
            using (var ctx = new AppDbContext())
            {
                int? origine = ctx.ventes.Where(v => v.IdVente == _venteId).Select(v => v.MutuelId).FirstOrDefault();

                // Mutuelles actives + celle de la vente même si elle est archivée
                _mutuels = ctx.mutuels.Where(m => m.Actif || m.IdMutuel == origine).ToList();
                foreach (var m in _mutuels.Where(m => !m.Actif))
                    m.NomEmployeur += " (archivée)";   // affichage seulement : ce contexte n'est jamais enregistré
            }
        }

        // ─── Produits ────────────────────────────────────────────────────

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            using (var form = new FormChoixProduit())
            {
                if (form.ShowDialog() != DialogResult.OK) return;

                var produit = form.ProduitSelectionne;

                // Contrôle en unités sur la quantité cumulée : stock actuel + ce que la vente d'origine a déjà retiré
                int unitesPanier = _lignes
                    .Where(l => l.ProduitId == produit.Id && !l.Supprimee)
                    .Sum(l => StockService.EnUnites(produit, l.Quantite, l.UniteVendue));
                int unitesDemandees = StockService.EnUnites(produit, form.QuantiteSelectionnee, form.UniteSelectionnee);
                _unitesOriginales.TryGetValue(produit.Id, out int dejaRetirees);

                if (unitesPanier + unitesDemandees > produit.QuantiteEnStock + dejaRetirees)
                {
                    MessageBox.Show(
                        $"Stock insuffisant pour « {produit.Nom} ».\n" +
                        $"Disponible : {StockService.Formater(produit, produit.QuantiteEnStock + dejaRetirees)}",
                        "Stock insuffisant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existant = _lignes.FirstOrDefault(l =>
                    l.ProduitId == produit.Id
                    && l.UniteVendue == form.UniteSelectionnee
                    && !l.Supprimee);

                decimal prixU = form.UniteSelectionnee == "Boîte"
                    ? produit.PrixVente
                    : produit.PrixUnitaireVente;

                if (FicheAVerifierService.AvertissementPrix(produit, prixU) is { } avertissement)
                    MessageBox.Show(avertissement, "Prix à vérifier", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (existant != null)
                    existant.Quantite += form.QuantiteSelectionnee;
                else
                    _lignes.Add(new LigneVenteModif
                    {
                        Id = null,
                        ProduitId = produit.Id,
                        ProduitNom = produit.Nom,
                        Quantite = form.QuantiteSelectionnee,
                        PrixUnitaire = prixU,
                        UniteVendue = form.UniteSelectionnee
                    });

                RafraichirListView();
                CalculerTotal();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0) return;
            int idx = dgvProduits.SelectedRows[0].Index;
            var actives = _lignes.Where(l => !l.Supprimee).ToList();
            actives[idx].Supprimee = true;
            RafraichirListView();
            CalculerTotal();
        }

        private void RafraichirListView()
        {
            dgvProduits.DataSource = _lignes.Where(l => !l.Supprimee).Select(l => new
            {
                Produit = l.ProduitNom,
                Unite = l.UniteVendue,
                l.Quantite,
                l.PrixUnitaire,
                l.SousTotal
            }).ToList();
        }

        private void CalculerTotal()
        {
            _total = _lignes.Where(l => !l.Supprimee).Sum(l => l.SousTotal);
            lblMontantTotal.Text = Format.Montant(_total);
            MettreAJourAjustement();
        }

        /// <summary>Part prise en charge par la mutuelle pour le total actuel (même taux que l'enregistrement).</summary>
        private decimal PartMutuelleActuelle()
        {
            if (cbPaiement.Text != ModesPaiement.Mutuelle || cbMutuelle.SelectedItem is not Mutuel m) return 0m;
            decimal taux = m.IdMutuel == _mutuelIdOrigine && _tauxMutuelleOrigine > 0 ? _tauxMutuelleOrigine : m.TauxPriseEnCharge;
            return VenteModificationRegles.PartMutuelle(_total, taux);
        }

        /// <summary>Annonce tout de suite ce qui sera à encaisser en plus ou à rendre au client.</summary>
        private void MettreAJourAjustement()
        {
            if (string.IsNullOrEmpty(cbPaiement.Text) || _total <= 0) { lblAjustement.Text = ""; tlpModeComplement.Visible = false; return; }
            decimal ajust = VenteModificationRegles.AjustementPaiements(cbPaiement.Text, _total, PartMutuelleActuelle(), _verseActuel);
            tlpModeComplement.Visible = ajust > 0;
            if (ajust > 0)
            {
                lblAjustement.Text = $"Le client paie {Format.Montant(ajust)} en plus";
                lblAjustement.ForeColor = Theme.AttentionTexte;
            }
            else if (ajust < 0)
            {
                lblAjustement.Text = $"Rendre {Format.Montant(-ajust)} au client ({_modeRemboursement})";
                lblAjustement.ForeColor = Theme.UrgentTexte;
            }
            else
            {
                lblAjustement.Text = "";
            }
        }

        // ─── Sauvegarde ──────────────────────────────────────────────────

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            var actives = _lignes.Where(l => !l.Supprimee).ToList();

            if (!actives.Any())
            {
                MessageBox.Show("La vente doit contenir au moins un produit.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = _total;

            var confirm = MessageBox.Show(
                "Enregistrer les modifications de cette vente ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // Complément à encaisser ou argent à rendre : annoncé et confirmé avant d'enregistrer quoi que ce soit
            decimal ajustAnnonce = VenteModificationRegles.AjustementPaiements(cbPaiement.Text, total, PartMutuelleActuelle(), VerseEnBase());
            string modeComplement = cbModeComplement.SelectedItem?.ToString() ?? ModesPaiement.Especes;
            if (ajustAnnonce > 0)
            {
                if (MessageBox.Show($"Le client paie {Format.Montant(ajustAnnonce)} en plus ({modeComplement}).\n\nConfirmer l'encaissement de ce complément ?",
                        "Complément à encaisser", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            else if (ajustAnnonce < 0)
            {
                MessageBox.Show($"Rendre {Format.Montant(-ajustAnnonce)} au client.\n\nCe remboursement sera enregistré en {_modeRemboursement}.",
                    "Rendre de l'argent au client", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            try
            {
                using (var ctx = new AppDbContext())
                using (var tx = ctx.Database.BeginTransaction())
                {
                    var vente = ctx.ventes
                        .Include(v => v.Lignes)
                        .FirstOrDefault(v => v.IdVente == _venteId);

                    if (vente == null) return;

                    VenteModificationRegles.VerifierModifiable(vente);

                    // ── 1. Remettre le stock de l'ancienne version (unités figées à la vente) ──
                    foreach (var ancienne in vente.Lignes)
                    {
                        var p = ctx.produits.Find(ancienne.ProduitId);
                        if (p == null) continue;

                        StockService.Ajouter(p, ancienne.QuantiteUnites);
                    }

                    // ── 2. Supprimer les anciennes lignes ─────────────────
                    ctx.LigneVentes.RemoveRange(vente.Lignes);

                    // ── 3. Infos client ───────────────────────────────────
                    vente.NomClient = txtNom.Text.Trim();
                    vente.PrenomClient = txtPrenom.Text.Trim();
                    vente.TelephoneClient = txtTelephone.Text.Trim();
                    vente.MotifAchat = txtMotif.Text.Trim();
                    vente.MoyenPaiement = cbPaiement.Text;
                    vente.Type = cbPaiement.Text;
                    vente.MontantTotal = total;
                    vente.MatriculeEmploye = cbPaiement.Text == "Mutuelle"
                                            ? txtMatricule.Text.Trim() : "N/A";

                    // ── 4. Mutuelle : même mutuelle conservée, règlement remis à zéro seulement si elle ou la part change ──
                    VenteModificationRegles.AppliquerMutuelle(
                        vente, cbPaiement.Text == "Mutuelle", cbMutuelle.SelectedItem as Mutuel, total);

                    // ── 5. Nouvelles lignes + décrémentation stock ────────
                    foreach (var l in actives)
                    {
                        var prod = ctx.produits.Find(l.ProduitId);
                        if (prod == null)
                            throw new InvalidOperationException("Un produit de la vente n'existe plus.");

                        int unites = StockService.EnUnites(prod, l.Quantite, l.UniteVendue);
                        StockService.Retirer(prod, unites);   // lève une exception si insuffisant

                        ctx.LigneVentes.Add(new LigneVente
                        {
                            VenteId = _venteId,
                            ProduitId = l.ProduitId,
                            Quantite = l.Quantite,
                            QuantiteUnites = unites,
                            PrixUnitaire = l.PrixUnitaire,
                            // ligne existante : son coût d'origine est conservé ; nouvelle ligne : coût du jour
                            CoutAchatUnitaire = l.Id != null ? l.CoutAchatUnitaire : MargeVenteService.CoutUnitaireDuJour(prod),
                            UniteVendue = l.UniteVendue
                        });
                    }

                    // ── 6. Paiements : complément à encaisser ou remboursement, dans la même transaction ──
                    decimal verse = ctx.paiement.Where(p => p.VenteId == _venteId).ToList().Sum(p => p.Montant);
                    decimal ajust = VenteModificationRegles.AjustementPaiements(vente.Type, total, vente.MontantMutuelle, verse);
                    if (ajust != ajustAnnonce)
                        throw new InvalidOperationException("Les paiements de cette vente ont changé entre-temps. Rouvrez la vente puis recommencez.");
                    if (ajust != 0)
                    {
                        ctx.paiement.Add(new Paiement
                        {
                            NumeroPaiement = (ajust > 0 ? "P-" : "R-") + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            VenteId = _venteId,
                            Montant = ajust,
                            Mode = ajust > 0 ? modeComplement : _modeRemboursement,
                            DatePaiement = DateTime.Now,
                            UserId = SessionUtilisateur.IdCourant
                        });
                    }

                    ctx.SaveChanges();
                    tx.Commit();
                }

                VenteEvenements.Notifier(this);

                BandeauNotification.Succes("Vente modifiée");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (StockInsuffisantException ex)
            {
                // Stock insuffisant : rien n'a été modifié
                MessageBox.Show(ex.Message + "\n\nLa vente n'a pas été modifiée.", "Stock insuffisant",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Modification de la vente " + _venteId, ex);
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\n\nDétail : " + ex.InnerException.Message;
                MessageBox.Show(msg + "\n\nLa vente n'a pas été modifiée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Total net des paiements actuellement en base (relu au moment d'enregistrer).</summary>
        private decimal VerseEnBase()
        {
            using var ctx = new AppDbContext();
            return ctx.paiement.AsNoTracking().Where(p => p.VenteId == _venteId).ToList().Sum(p => p.Montant);
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cbPaiement_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMutuelle.Visible = cbPaiement.Text == "Mutuelle";
            pnlMatricule.Visible = cbPaiement.Text == "Mutuelle";
            MettreAJourAjustement();
            if (cbPaiement.Text == "Mutuelle")
            {
                cbMutuelle.DataSource = null;
                cbMutuelle.DataSource = _mutuels;
                cbMutuelle.DisplayMember = "NomEmployeur";
                cbMutuelle.ValueMember = "IdMutuel";

                // Présélectionner la mutuelle d'origine de la vente (et non la première de la liste)
                if (_mutuelIdOrigine != null && _mutuels.Any(m => m.IdMutuel == _mutuelIdOrigine))
                    cbMutuelle.SelectedValue = _mutuelIdOrigine.Value;
            }
        }
    }
}