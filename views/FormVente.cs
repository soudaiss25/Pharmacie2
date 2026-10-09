using System.Drawing;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class FormVente : Form
    {
        private readonly List<LigneVente> _lignes = new List<LigneVente>();
        private List<Mutuel> _mutuels = new List<Mutuel>();
        private decimal _total;
        private bool _enMiseAJour;

        // ID de la dernière vente sauvegardée (pour impression immédiate)
        private int _derniereVenteId = -1;

        /// <summary>Prochain numéro de vente, calculé depuis la base (appelé dans la transaction).</summary>
        private static string GetProchainNumeroVente(AppDbContext ctx)
        {
            var derniere = ctx.ventes
                .Where(v => v.numeroVente.StartsWith("V-"))
                .Select(v => v.numeroVente)
                .OrderByDescending(n => n)
                .FirstOrDefault();

            int num = 1;
            if (derniere != null && int.TryParse(derniere.Substring(2), out int n))
                num = n + 1;
            return "V-" + num.ToString("D6");
        }

        public FormVente()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            lblVendeur.Text = "Vendeur : " + SessionUtilisateur.NomComplet;
            ActiveControl = txtRecherche;   // focus sur la recherche produit dès l'ouverture

            ChargerMutuelles();
            InitComboMoyenPaiement();
            RecalculerTotal();
        }

        private void FormVente_Shown(object sender, EventArgs e) => txtRecherche.Focus();

        private void ChargerMutuelles()
        {
            using (var ctx = new AppDbContext())
                _mutuels = ctx.mutuels.Where(m => m.Actif).ToList();
        }

        private void InitComboMoyenPaiement()
        {
            cbPaiement.Items.Clear();
            cbPaiement.Items.AddRange(ModesPaiement.Tous);
            cbPaiement.SelectedIndex = 0;
        }

        // ── Raccourcis clavier ────────────────────────────────────────────

        private void FormVente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
                AjouterProduit();
            }
            else if (e.KeyCode == Keys.F9)
            {
                e.Handled = true;
                btnValider_Click(sender, EventArgs.Empty);
            }
        }

        private void txtRecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;   // Entrée ouvre le choix du produit au lieu de valider la vente
                AjouterProduit();
            }
        }

        private void dgvProduits_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                RetirerLigne();
            }
        }

        // ── Produits ──────────────────────────────────────────────────────

        private void btnAjouter_Click(object sender, EventArgs e) => AjouterProduit();

        private void AjouterProduit()
        {
            using (var form = new FormChoixProduit { FiltreInitial = txtRecherche.Text.Trim() })
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;

                var produit = form.ProduitSelectionne;
                int qte = form.QuantiteSelectionnee;
                string unite = form.UniteSelectionnee;

                // ── Contrôle du stock en UNITÉS sur la quantité CUMULÉE du panier ──
                int unitesPanier = _lignes
                    .Where(l => l.ProduitId == produit.Id)
                    .Sum(l => StockService.EnUnites(produit, l.Quantite, l.UniteVendue));
                int unitesDemandees = StockService.EnUnites(produit, qte, unite);

                if (!StockService.EstDisponible(produit, unitesPanier + unitesDemandees))
                {
                    string dejaPanier = unitesPanier > 0
                        ? $"\nDéjà dans le panier : {StockService.Formater(produit, unitesPanier)}"
                        : "";
                    MessageBox.Show(
                        $"Stock insuffisant pour « {produit.Nom} ».\n" +
                        $"Disponible : {StockService.Formater(produit)}" + dejaPanier,
                        "Stock insuffisant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prix unitaire selon l'unité choisie
                decimal prixU = unite == StockService.UniteBoite
                    ? produit.PrixVente
                    : produit.PrixUnitaireVente;

                // Si le produit est déjà dans la liste avec la même unité → cumul
                var existant = _lignes.FirstOrDefault(l =>
                    l.ProduitId == produit.Id && l.UniteVendue == unite);

                if (existant != null)
                    existant.Quantite += qte;
                else
                    _lignes.Add(new LigneVente
                    {
                        ProduitId = produit.Id,
                        Produit = produit,
                        Quantite = qte,
                        PrixUnitaire = prixU,
                        UniteVendue = unite
                    });

                txtRecherche.Clear();
                RafraichirGrille();
                RecalculerTotal();
                txtRecherche.Focus();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e) => RetirerLigne();

        private void RetirerLigne()
        {
            if (dgvProduits.SelectedRows.Count == 0) return;
            _lignes.RemoveAt(dgvProduits.SelectedRows[0].Index);
            RafraichirGrille();
            RecalculerTotal();
        }

        private void RafraichirGrille()
        {
            dgvProduits.DataSource = _lignes.Select(l => new
            {
                Produit = l.Produit?.Nom ?? "",
                Unite = l.UniteVendue,
                l.Quantite,
                l.PrixUnitaire,
                SousTotal = l.SousTotal
            }).ToList();
        }

        private void RecalculerTotal()
        {
            _total = _lignes.Sum(l => l.SousTotal);
            lblMontantTotal.Text = Format.Montant(_total);
            MettreAJourAffichagePaiement();
        }

        // ── Paiement ──────────────────────────────────────────────────────

        private string ModeChoisi => cbPaiement.SelectedItem?.ToString() ?? ModesPaiement.Comptant;

        private void cbPaiement_SelectedIndexChanged(object sender, EventArgs e)
            => MettreAJourAffichagePaiement();

        private void DefinirEspeces(decimal valeur)
        {
            _enMiseAJour = true;
            numEspeces.Value = Math.Min(numEspeces.Maximum, Math.Max(numEspeces.Minimum, valeur));
            _enMiseAJour = false;
        }

        private void MettreAJourAffichagePaiement()
        {
            if (cbPaiement.SelectedIndex < 0) return;

            pnlMutuelle.Visible = false;
            pnlEspeces.Visible = false;
            pnlMatricule.Visible = false;

            switch (ModeChoisi)
            {
                case ModesPaiement.Comptant:
                    pnlEspeces.Visible = true;
                    lblEspeces.Text = "Espèces reçues";
                    DefinirEspeces(Math.Ceiling(_total));
                    ActualiserMessageEspeces();
                    break;

                case ModesPaiement.Credit:
                    // Avance partielle possible en crédit
                    pnlEspeces.Visible = true;
                    lblEspeces.Text = "Avance (facultatif)";
                    DefinirEspeces(0);
                    ActualiserMessageEspeces();
                    break;

                case ModesPaiement.Mutuelle:
                    pnlMutuelle.Visible = true;
                    pnlEspeces.Visible = true;
                    pnlMatricule.Visible = true;
                    lblEspeces.Text = "Espèces reçues du patient";
                    cbMutuelle.DataSource = null;
                    cbMutuelle.DataSource = _mutuels;
                    cbMutuelle.DisplayMember = "NomEmployeur";
                    cbMutuelle.ValueMember = "IdMutuel";
                    AppliquerTauxMutuelle();
                    break;

                    // Chèque, carte, Mvola, Huri Money : réglés en totalité, aucune saisie
            }
        }

        private void cbMutuelle_SelectedIndexChanged(object sender, EventArgs e)
            => AppliquerTauxMutuelle();

        private void AppliquerTauxMutuelle()
        {
            if (cbMutuelle.SelectedItem is Mutuel m)
            {
                decimal partMutuelle = VenteModificationRegles.PartMutuelle(_total, m.TauxPriseEnCharge);
                decimal partPatient = _total - partMutuelle;

                txtTauxMutuelle.Text = m.TauxPriseEnCharge.ToString("0.##");
                lblPartMutuelle.Text = $"Pris en charge par la mutuelle : {Format.Montant(partMutuelle)} (crédit de l'entreprise)";
                lblRestePatient.Text = $"À payer par le patient : {Format.Montant(partPatient)}";
                DefinirEspeces(Math.Ceiling(partPatient));
                ActualiserMessageEspeces();
            }
        }

        private void numEspeces_ValueChanged(object sender, EventArgs e)
        {
            if (_enMiseAJour) return;
            ActualiserMessageEspeces();
        }

        /// <summary>Texte sous le champ « espèces » : rendu à rendre, manque ou reste à crédit selon le mode.</summary>
        private void ActualiserMessageEspeces()
        {
            decimal especes = numEspeces.Value;

            switch (ModeChoisi)
            {
                case ModesPaiement.Comptant:
                    AfficherRendu(especes, _total, "Rendu au client");
                    break;

                case ModesPaiement.Mutuelle:
                    if (cbMutuelle.SelectedItem is Mutuel m)
                        AfficherRendu(especes, _total - VenteModificationRegles.PartMutuelle(_total, m.TauxPriseEnCharge), "Rendu au patient");
                    break;

                case ModesPaiement.Credit:
                    decimal avance = Math.Min(especes, _total);
                    decimal reste = _total - avance;
                    lblMontantRendu.Text = reste > 0
                        ? $"Reste à payer plus tard (crédit) : {Format.Montant(reste)}"
                        : "Payé en totalité";
                    lblMontantRendu.ForeColor = reste > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;
                    break;
            }
        }

        private void AfficherRendu(decimal recu, decimal aPayer, string libelle)
        {
            if (recu >= aPayer)
            {
                lblMontantRendu.Text = $"{libelle} : {Format.Montant(recu - aPayer)}";
                lblMontantRendu.ForeColor = Theme.SuccesTexte;
            }
            else
            {
                lblMontantRendu.Text = $"Il manque : {Format.Montant(aPayer - recu)}";
                lblMontantRendu.ForeColor = Theme.UrgentTexte;
            }
        }

        // ── Validation ────────────────────────────────────────────────────

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (_lignes.Count == 0)
            {
                MessageBox.Show("Ajoutez au moins un médicament.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRecherche.Focus();
                return;
            }

            string mode = ModeChoisi;
            decimal total = _total;
            decimal especes = 0, rendu = 0, verse = 0, partMutuelle = 0;
            Mutuel? mutuelleChoisie = null;

            if (mode == ModesPaiement.Comptant)
            {
                especes = numEspeces.Value;
                if (especes < total)
                {
                    MessageBox.Show($"Le montant reçu en espèces est insuffisant : il manque {Format.Montant(total - especes)}.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numEspeces.Focus();
                    return;
                }
                rendu = especes - total;
                verse = total;
            }
            else if (mode == ModesPaiement.Mutuelle)
            {
                mutuelleChoisie = cbMutuelle.SelectedItem as Mutuel;
                if (mutuelleChoisie == null)
                {
                    MessageBox.Show("Choisissez la mutuelle qui prend en charge cette vente.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbMutuelle.Focus();
                    return;
                }

                partMutuelle = VenteModificationRegles.PartMutuelle(total, mutuelleChoisie.TauxPriseEnCharge);
                decimal partPatient = total - partMutuelle;
                especes = numEspeces.Value;

                if (especes < partPatient)
                {
                    MessageBox.Show(
                        $"Espèces insuffisantes pour la part du patient.\nAttendu : {Format.Montant(partPatient)}",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numEspeces.Focus();
                    return;
                }
                rendu = especes - partPatient;
                verse = partPatient;
            }
            else if (mode == ModesPaiement.Credit)
            {
                // Avance optionnelle — le client peut verser une partie maintenant
                especes = numEspeces.Value;
                if (especes > total)
                {
                    MessageBox.Show(
                        $"L'avance ({Format.Montant(especes)}) ne peut pas dépasser le total ({Format.Montant(total)}).",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numEspeces.Focus();
                    return;
                }

                verse = especes;     // ce qui est versé maintenant
                rendu = 0;           // pas de rendu en crédit
            }
            else
            {
                // Chèque, carte, Mvola, Huri Money → versé en totalité (aucun champ à saisir : jamais 0 par erreur)
                verse = total;
                especes = verse;
            }

            try
            {
                using (var ctx = new AppDbContext())
                using (var tx = ctx.Database.BeginTransaction())
                {
                    // ── Rechargement des produits et revérification du stock ──
                    var ids = _lignes.Select(l => l.ProduitId).Distinct().ToList();
                    var produits = ctx.produits.Where(p => ids.Contains(p.Id)).ToDictionary(p => p.Id);

                    var lignesVente = new List<LigneVente>();
                    foreach (var ligne in _lignes)
                    {
                        if (!produits.TryGetValue(ligne.ProduitId, out var produit))
                            throw new InvalidOperationException("Un produit du panier n'existe plus.");

                        int unites = StockService.EnUnites(produit, ligne.Quantite, ligne.UniteVendue);
                        StockService.Retirer(produit, unites);   // lève une exception si insuffisant

                        lignesVente.Add(new LigneVente
                        {
                            ProduitId = ligne.ProduitId,
                            Quantite = ligne.Quantite,
                            QuantiteUnites = unites,
                            PrixUnitaire = ligne.PrixUnitaire,
                            UniteVendue = ligne.UniteVendue
                        });
                    }

                    var vente = new Vente
                    {
                        numeroVente = GetProchainNumeroVente(ctx),
                        NomClient = txtNom.Text.Trim(),
                        PrenomClient = txtPrenom.Text.Trim(),
                        TelephoneClient = txtTelephone.Text.Trim(),
                        MotifAchat = txtMotif.Text.Trim(),
                        MatriculeEmploye = mode == ModesPaiement.Mutuelle ? txtMatricule.Text.Trim() : "N/A",
                        MoyenPaiement = mode,
                        Type = mode,
                        MontantTotal = total,
                        MontantEspeces = especes,
                        MontantRendu = rendu,
                        MontantMutuelle = partMutuelle,
                        // La part mutuelle est un CRÉDIT ENTREPRISE — pas encore réglée
                        MutuelleReglee = false,
                        Statut = "Active",
                        DateVente = DateTime.Now,
                        UserId = SessionUtilisateur.IdCourant
                    };

                    if (mutuelleChoisie != null)
                    {
                        vente.MutuelId = mutuelleChoisie.IdMutuel;
                        vente.TauxMutuelle = mutuelleChoisie.TauxPriseEnCharge;
                    }

                    ctx.ventes.Add(vente);
                    ctx.SaveChanges();

                    foreach (var lv in lignesVente)
                    {
                        lv.VenteId = vente.IdVente;
                        ctx.LigneVentes.Add(lv);
                    }

                    // ── Paiement initial ──────────────────────────────────
                    if (verse > 0)
                    {
                        ctx.paiement.Add(new Paiement
                        {
                            NumeroPaiement = "P-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            VenteId = vente.IdVente,
                            Montant = verse,
                            DatePaiement = DateTime.Now,
                            UserId = SessionUtilisateur.IdCourant
                        });
                    }

                    ctx.SaveChanges();
                    tx.Commit();   // sans Commit, tout est annulé à la fermeture
                    _derniereVenteId = vente.IdVente;
                }

                VenteEvenements.Notifier(this);   // Uc_Vente, Uc_Caisse et « Ma journée » se rafraîchissent
                BandeauNotification.Succes("Vente enregistrée");

                // ── Récapitulatif + proposition d'impression ──────────────
                string msg = "Vente enregistrée.";
                if (rendu > 0) msg += $"\n\nRendu au client : {Format.Montant(rendu)}";
                if (mode == ModesPaiement.Credit && verse > 0) msg += $"\n\nAvance reçue : {Format.Montant(verse)}\nReste à payer : {Format.Montant(total - verse)}";
                if (mode == ModesPaiement.Credit && verse == 0) msg += $"\n\nVente entièrement à crédit : {Format.Montant(total)} à récupérer";
                if (mode == ModesPaiement.Mutuelle) msg += $"\n\nPart de l'entreprise (crédit) : {Format.Montant(partMutuelle)}";
                msg += "\n\nVoulez-vous imprimer la facture ?";

                var res = MessageBox.Show(msg, "Vente enregistrée",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes && _derniereVenteId > 0)
                    ImprimerFacture(_derniereVenteId);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (StockInsuffisantException ex)
            {
                // Stock insuffisant à la validation : rien n'a été enregistré
                MessageBox.Show(ex.Message + "\n\nAucune vente n'a été enregistrée.", "Vente non enregistrée",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Enregistrement de la vente", ex);
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\n\nDétail : " + ex.InnerException.Message;
                if (ex.InnerException?.InnerException != null)
                    msg += "\n\n" + ex.InnerException.InnerException.Message;
                MessageBox.Show(msg + "\n\nAucune vente n'a été enregistrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Impression Facture ────────────────────────────────────────────

        /// <summary>
        /// Charge la vente depuis la DB et affiche une prévisualisation d'impression.
        /// </summary>
        private void ImprimerFacture(int venteId)
        {
            try
            {
                Vente vente;
                List<LigneVente> lignes;

                using (var ctx = new AppDbContext())
                {
                    vente = ctx.ventes
                        .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                        .Include(v => v.Paiements)   // ← INDISPENSABLE pour MontantRestant correct
                        .Include(v => v.Mutuel)
                        .Include(v => v.User)
                        .FirstOrDefault(v => v.IdVente == venteId);

                    if (vente == null) return;
                    lignes = vente.Lignes.ToList();
                }

                var pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize =
                    new PaperSize("A4", 827, 1169);   // A4 en centièmes de pouce

                pd.PrintPage += (s, ev) =>
                    DessinerFacture(ev.Graphics, vente, lignes);

                using (var preview = new PrintPreviewDialog())
                {
                    preview.Document = pd;
                    preview.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    preview.Text = $"Facture — {vente.numeroVente}";

                    // Zoom out pour voir toute la page A4 d'un coup
                    preview.PrintPreviewControl.Zoom = 0.75;

                    preview.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'impression : " + ex.Message,
                    "Erreur impression", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dessine la facture dans le contexte graphique GDI+ fourni par PrintDocument.
        /// </summary>
        private void DessinerFacture(Graphics g, Vente vente, List<LigneVente> lignes)
        {
            // A4 : 827 × 1169 centièmes de pouce — on utilise toute la largeur
            float marge = 40f;
            float x = marge;
            float y = marge;
            float largeur = 827f - (marge * 2);  // 747 pts utiles

            // ── Ressources ────────────────────────────────────────────────
            using var penVert = new Pen(Color.FromArgb(20, 60, 20), 1.5f);
            using var penGris = new Pen(Color.FromArgb(180, 180, 180), 0.7f);
            using var brushVert = new SolidBrush(Color.FromArgb(20, 60, 20));
            using var brushVert2 = new SolidBrush(Color.FromArgb(46, 100, 46));
            using var brushGris = new SolidBrush(Color.FromArgb(90, 90, 90));
            using var brushLG = new SolidBrush(Color.FromArgb(235, 245, 235));
            using var brushAlt = new SolidBrush(Color.FromArgb(248, 252, 248));
            using var brushRed = new SolidBrush(Color.FromArgb(183, 28, 28));
            using var brushRedL = new SolidBrush(Color.FromArgb(255, 235, 235));
            var brushNoir = Brushes.Black;
            var brushBlanc = Brushes.White;

            using var fontNom = new Font("Segoe UI", 26F, FontStyle.Bold);
            using var fontSlogan = new Font("Segoe UI", 9F, FontStyle.Italic);
            using var fontST = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fontGras = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var fontNormal = new Font("Segoe UI", 9F);
            using var fontPetit = new Font("Segoe UI", 7.5F);
            using var fontTotal = new Font("Segoe UI", 13F, FontStyle.Bold);

            // ══════════════════════════════════════════════════════════════
            // 1. EN-TÊTE — bande verte pleine largeur
            // ══════════════════════════════════════════════════════════════
            g.FillRectangle(brushVert, x, y, largeur, 80f);
            g.DrawString(AppInfo.NomPharmacie, fontNom, brushBlanc, x + 16, y + 8);
            g.DrawString("Votre santé, notre priorité — Pharmacie agréée",
                fontSlogan, brushBlanc, x + 16, y + 56);
            g.DrawString($"Facture  {vente.numeroVente ?? "—"}",
                fontGras, brushBlanc, x + largeur - 220, y + 14);
            g.DrawString(vente.DateVente.ToString("dd/MM/yyyy  HH:mm"),
                fontNormal, brushBlanc, x + largeur - 220, y + 34);
            y += 96;

            // ══════════════════════════════════════════════════════════════
            // 2. BLOC INFO — deux colonnes
            // ══════════════════════════════════════════════════════════════
            float colG = x;
            float colD = x + largeur / 2 + 10;

            g.FillRectangle(brushLG, x, y, largeur, 76);

            void Info(string lbl, string val, float lx, float ly)
            {
                g.DrawString(lbl, fontGras, brushGris, lx, ly);
                g.DrawString(val, fontNormal, brushNoir, lx + 105f, ly);
            }

            string nomVendeur = vente.User != null
                ? $"{vente.User.Prenom} {vente.User.Nom}" : "—";
            string nomClient = $"{vente.PrenomClient} {vente.NomClient}".Trim();
            if (string.IsNullOrWhiteSpace(nomClient)) nomClient = "Client anonyme";
            string tel = string.IsNullOrWhiteSpace(vente.TelephoneClient)
                ? "—" : vente.TelephoneClient;
            string motif = !string.IsNullOrWhiteSpace(vente.MotifAchat)
                && vente.MotifAchat != "null" ? vente.MotifAchat : "—";

            Info("Vendeur :", nomVendeur, colG, y + 8);
            Info("Paiement :", vente.MoyenPaiement, colG, y + 28);
            Info("Motif :", motif, colG, y + 48);
            Info("Client :", nomClient, colD, y + 8);
            Info("Téléphone :", tel, colD, y + 28);
            if (!string.IsNullOrWhiteSpace(vente.MatriculeEmploye)
                && vente.MatriculeEmploye != "N/A")
                Info("Matricule :", vente.MatriculeEmploye, colD, y + 48);

            y += 84;
            g.DrawLine(penVert, x, y, x + largeur, y);
            y += 12;

            // ══════════════════════════════════════════════════════════════
            // 3. TABLEAU PRODUITS
            // ══════════════════════════════════════════════════════════════
            // Colonnes : Produit | Unité | Qté | Prix U. | Sous-total
            float[] cx = { x + 5, x + 360, x + 450, x + 530, x + 630 };
            float rh = 24f;

            // En-tête
            g.FillRectangle(brushVert2, x, y, largeur, rh);
            string[] ent = { "Produit", "Unité", "Qté", "Prix unitaire", "Sous-total" };
            for (int i = 0; i < ent.Length; i++)
                g.DrawString(ent[i], fontGras, brushBlanc, cx[i], y + 4);
            y += rh + 2;

            // Lignes produits
            bool alt = false;
            foreach (var l in lignes)
            {
                // Hauteur variable : +18 si posologie présente
                bool hasPoso = l.Produit != null &&
                    (!string.IsNullOrWhiteSpace(l.Produit.Indication)
                  || !string.IsNullOrWhiteSpace(l.Produit.Posologie)
                  || l.Produit.NbFoisParJour > 0);

                float ligneH = hasPoso ? rh + 18f : rh;

                if (alt) g.FillRectangle(brushAlt, x, y, largeur, ligneH);

                string nom = l.Produit?.Nom ?? $"#{l.ProduitId}";
                if (nom.Length > 52) nom = nom.Substring(0, 50) + "…";

                g.DrawString(nom, fontNormal, brushNoir, cx[0], y + 3);
                g.DrawString(l.UniteVendue, fontNormal, brushNoir, cx[1], y + 3);
                g.DrawString(l.Quantite.ToString(), fontNormal, brushNoir, cx[2], y + 3);
                g.DrawString(Format.Montant(l.PrixUnitaire), fontNormal, brushNoir, cx[3], y + 3);
                g.DrawString(Format.Montant(l.SousTotal), fontGras, brushNoir, cx[4], y + 3);

                // Posologie sous le nom du médicament
                if (hasPoso)
                {
                    var parts = new System.Collections.Generic.List<string>();
                    if (!string.IsNullOrWhiteSpace(l.Produit.Indication))
                        parts.Add(l.Produit.Indication);
                    if (!string.IsNullOrWhiteSpace(l.Produit.Posologie))
                        parts.Add(l.Produit.Posologie);
                    if (l.Produit.NbFoisParJour > 0)
                        parts.Add($"{l.Produit.NbFoisParJour}x/jour");

                    string poso = "   " + string.Join("  |  ", parts);
                    using var fontPoso = new Font("Segoe UI", 7.5F, FontStyle.Italic);
                    using var brushPoso = new SolidBrush(Color.FromArgb(60, 100, 60));
                    g.DrawString(poso, fontPoso, brushPoso, cx[0], y + rh - 2);
                }

                y += ligneH;
                alt = !alt;
            }

            y += 8;
            g.DrawLine(penVert, x, y, x + largeur, y);
            y += 14;

            // ══════════════════════════════════════════════════════════════
            // 4. TOTAUX
            // ══════════════════════════════════════════════════════════════
            float xLbl = x + largeur - 340;
            float xVal = x + largeur - 110;

            void LigneT(string lbl, string val, Font fl, Font fv,
                        System.Drawing.Brush cl, System.Drawing.Brush cv)
            {
                g.DrawString(lbl, fl, cl, xLbl, y);
                g.DrawString(val, fv, cv, xVal, y);
                y += 22f;
            }

            // Total principal
            g.FillRectangle(brushLG, x + largeur - 360, y - 4, 360, 32);
            g.DrawString("TOTAL", fontTotal, brushNoir, xLbl, y);
            g.DrawString(Format.Montant(vente.MontantTotal), fontTotal, brushVert2, xVal, y);
            y += 36;

            // Mutuelle
            if (vente.MontantMutuelle > 0)
            {
                string nomMut = vente.Mutuel?.NomEmployeur ?? "Mutuelle";
                decimal partPat = vente.MontantTotal - vente.MontantMutuelle;
                LigneT($"Part {nomMut} :", Format.Montant(vente.MontantMutuelle),
                    fontNormal, fontGras, brushGris, brushGris);
                LigneT("Part patient :", Format.Montant(partPat),
                    fontNormal, fontGras, brushNoir, brushNoir);
            }

            // Avance crédit
            if (vente.Type == "Crédit" && vente.MontantVerse > 0)
                LigneT("Avance reçue :", Format.Montant(vente.MontantVerse),
                    fontNormal, fontGras, brushGris, brushGris);

            // Reste à payer
            if (vente.MontantRestant > 0)
            {
                g.FillRectangle(brushRedL, x + largeur - 360, y - 2, 360, 28);
                LigneT("RESTE À PAYER :", Format.Montant(vente.MontantRestant),
                    fontGras, fontGras, brushRed, brushRed);
            }
            else if (vente.Type == "Crédit")
            {
                g.DrawString("Soldé intégralement", fontGras, brushVert2, xLbl, y);
                y += 22;
            }

            // ══════════════════════════════════════════════════════════════
            // 5. PIED DE PAGE — bande verte en bas de page
            // ══════════════════════════════════════════════════════════════
            float yPied = 1169f - marge - 36f;
            g.FillRectangle(brushVert, x, yPied, largeur, 36f);
            g.DrawString(
                "Merci pour votre confiance.   Conservez cette facture pour tout remboursement.",
                fontPetit, brushBlanc,
                x + largeur / 2 - 230, yPied + 6);
            g.DrawString(
                $"Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}  —  " + AppInfo.NomPharmacie,
                fontPetit, brushBlanc,
                x + largeur / 2 - 175, yPied + 20);
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => this.Close();
    }
}