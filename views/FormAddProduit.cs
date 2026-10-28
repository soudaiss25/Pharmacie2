using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class FormAddProduit : Form
    {
        private List<Fournisseur> _fournisseurs;
        private readonly int? _produitId; // null = ajout, int = modification

        // État d'origine du stock (mode modification) : sert à ne pas réécrire un stock non touché
        private int _stockOrigineUnites;
        private int _boitesOrigine;
        private int _vracOrigine;
        private int _nbParBoiteOrigine = 1;
        private bool _nbAverti;

        // Produit marqué « stock à vérifier » (ancienne erreur de réception des commandes)
        private bool _verifier;
        private int _unitesManquantes;
        private string _motifVerification = "";
        private Produit _produitOrigine;

        // Valeurs d'origine (mode modification) : un nouveau prix ou une nouvelle date lève le motif correspondant de la fiche à vérifier
        private decimal _prixAchatOrigine, _prixVenteOrigine;
        private decimal? _prixUniteOrigine;
        private DateTime _dateOrigine;
        private bool _ficheMarquee;
        private bool _aVentesUnite;               // des ventes passées ont été faites à l'unité
        private bool _peremptionOrigine = true;   // la date d'origine était-elle connue ?

        // ── Constructeurs ─────────────────────────────────────────────────

        /// <summary>
        /// Petit écran (largeur logique &lt; 1200) : les deux colonnes passent l'une sous l'autre, la fenêtre se réduit
        /// et la zone centrale défile verticalement ; les boutons restent fixes en bas.
        /// </summary>
        private void AppliquerMode()
        {
            if (!ModeCompact.EstEcranPetit(this)) return;
            var ecran = Theme.EcranLogique(this);
            // tailles en unités logiques : la mise à l'échelle (DPI) s'applique ensuite à l'ouverture de la fenêtre
            ClientSize = new Size(Math.Min(ClientSize.Width, Math.Min(640, (int)(ecran.Width * 0.85))),
                                  Math.Min(ClientSize.Height, Math.Min(520, (int)(ecran.Height * 0.8))));
            ModeCompact.Recomposer(tlpContenu, new[] { "P100" }, new[] { "A", "A" },
                (tlpGauche, 0, 0, tlpGauche.Margin), (tlpDroite, 0, 1, tlpDroite.Margin));
        }

        public FormAddProduit()
        {
            _produitId = null;
            InitializeComponent();
            Theme.Appliquer(this);
            AppliquerMode();
            cmbUniteVente.SelectedIndex = 0;
            ChargerFournisseurs();
            WireEvents();
            this.Text = AppInfo.Titre("Ajouter un produit");
        }

        public FormAddProduit(int produitId)
        {
            _produitId = produitId;
            InitializeComponent();
            Theme.Appliquer(this);
            AppliquerMode();
            cmbUniteVente.SelectedIndex = 0;
            ChargerFournisseurs();
            WireEvents();
            this.Text = AppInfo.Titre("Modifier le produit");
            PreRemplir(produitId);
            AfficherBandeauVerification();
            AfficherBandeauFiche();
        }

        // ── Câblage des événements ────────────────────────────────────────

        private void WireEvents()
        {
            // Le client tape ses deux prix ; le prix de vente n'est JAMAIS recalculé. La marge s'affiche automatiquement.
            numPrixAchat.ValueChanged += VerifierMarge;
            numPrixVente.ValueChanged += VerifierMarge;
            numPrixVenteUnite.ValueChanged += VerifierMarge;
            numNbUniteParBoite.ValueChanged += VerifierMarge;
            chkVenteDetail.CheckedChanged += VerifierMarge;
            chkVenteDetail.CheckedChanged += chkVenteDetail_CheckedChanged;
            cmbUniteVente.SelectedIndexChanged += MettreAJourApercu;
            numNbUniteParBoite.ValueChanged += MettreAJourApercu;
            numPrixVente.ValueChanged += MettreAJourApercu;
            numPrixVenteUnite.ValueChanged += MettreAJourApercu;
            // La date du jour n'est jamais proposée : soit une date est saisie (case cochée), soit « Pas de date de péremption »
            dtpDateExpiration.ValueChanged += (s, e) => { if (dtpDateExpiration.Checked) chkPasPeremption.Checked = false; };
            chkPasPeremption.CheckedChanged += (s, e) => { if (chkPasPeremption.Checked) dtpDateExpiration.Checked = false; };
        }

        /// <summary>
        /// Affiche la marge en lecture seule, en direct (« Marge : 500 KMF (50 % du prix d'achat) », et à l'unité pour un produit au détail),
        /// en vert si elle est positive et en rouge sinon. Ne modifie JAMAIS un prix.
        /// </summary>
        private void VerifierMarge(object sender, EventArgs e)
        {
            decimal achat = numPrixAchat.Value, vente = numPrixVente.Value;
            bool ok = vente > achat && achat > 0;
            numPrixVente.BackColor = ok ? Theme.SuccesFond : Theme.UrgentFond;

            if (achat <= 0 && vente <= 0)
            {
                lblMarge.Text = "Marge : saisissez le prix d'achat et le prix de vente.";
                lblMarge.ForeColor = Theme.Neutre;
                return;
            }

            decimal marge = MargeService.Kmf(achat, vente);
            bool positive = marge > 0;
            lblMarge.Text = "Marge : " + MargeService.Texte(achat, vente);

            if (chkVenteDetail.Checked && numNbUniteParBoite.Value > 1)
            {
                decimal margeUnite = Math.Round(MargeService.KmfUnite(achat, (int)numNbUniteParBoite.Value, numPrixVenteUnite.Value), 2);
                decimal coutUnite = achat / numNbUniteParBoite.Value;
                string pctUnite = coutUnite > 0 ? $" ({margeUnite / coutUnite * 100m:0.#} % du prix d'achat)" : "";
                string unite = cmbUniteVente.SelectedItem?.ToString()?.ToLowerInvariant() ?? "unité";
                lblMarge.Text += "\nPar " + unite + " : " + Format.Montant(margeUnite) + pctUnite;
                positive &= margeUnite > 0;
            }
            lblMarge.ForeColor = positive ? Theme.SuccesTexte : Theme.UrgentTexte;
        }

        // ── Pré-remplissage en mode modification ──────────────────────────

        private void PreRemplir(int id)
        {
            using (var ctx = new AppDbContext())
            {
                var p = ctx.produits.Find(id);
                if (p == null) return;

                txtNom.Text = p.Nom;
                _prixAchatOrigine = p.PrixAchat; _prixVenteOrigine = p.PrixVente; _prixUniteOrigine = p.PrixVenteUnite; _dateOrigine = p.DateExpiration;
                _ficheMarquee = p.FicheAVerifier;
                _aVentesUnite = ctx.LigneVentes.Any(l => l.ProduitId == p.Id && l.UniteVendue != StockService.UniteBoite);
                _motifsFiche = FicheAVerifierService.Motifs(p);
                cmbType.Text = p.Type;
                // Les deux prix sont affichés tels qu'enregistrés : aucun n'est recalculé à partir de l'autre
                numPrixAchat.Value = Math.Min(numPrixAchat.Maximum, p.PrixAchat);
                numPrixVente.Value = Math.Min(numPrixVente.Maximum, p.PrixVente);
                numPrixVenteUnite.Value = Math.Min(numPrixVenteUnite.Maximum, p.PrixVenteUnite ?? 0m);   // jamais déduit du prix de la boîte

                // Stock en unités de base → boîtes pleines + unités en vrac (la boîte entamée reste visible)
                int nbParBoite = Math.Max(1, p.NbUniteParBoite);
                _stockOrigineUnites = p.QuantiteEnStock;
                _verifier = p.StockAVerifier;
                _unitesManquantes = p.UnitesManquantesEstimees;
                _motifVerification = Format.Accorder(p.MotifVerification ?? "");   // anciens textes en base : « 10 boîte(s) reçue(s) » devient « 10 boîtes reçues »
                _produitOrigine = new Produit
                {
                    Id = p.Id, Nom = p.Nom, NbUniteParBoite = p.NbUniteParBoite,
                    UniteVente = p.UniteVente, QuantiteEnStock = p.QuantiteEnStock
                };
                _nbParBoiteOrigine = nbParBoite;
                _boitesOrigine = p.QuantiteEnStock / nbParBoite;
                _vracOrigine = p.QuantiteEnStock % nbParBoite;
                numQuantite.Value = Math.Min(numQuantite.Maximum, _boitesOrigine);
                numUnitesVrac.Value = Math.Min(numUnitesVrac.Maximum, _vracOrigine);
                numSeuil.Value = p.SeuilAlerte;

                _peremptionOrigine = p.PeremptionConnue;
                if (p.PeremptionConnue && p.DateExpiration.Year >= 1900)
                {
                    dtpDateExpiration.Value = p.DateExpiration;
                    dtpDateExpiration.Checked = true;
                }
                else
                {
                    dtpDateExpiration.Checked = false;
                    chkPasPeremption.Checked = !p.PeremptionConnue;   // date inconnue : il faut la saisir, ou confirmer qu'il n'y en a pas
                }

                if (p.FournisseurId.HasValue)
                    cbFournisseur.SelectedValue = p.FournisseurId.Value;

                // Détails médicament
                txtIndication.Text = p.Indication ?? "";
                txtPosologie.Text = p.Posologie ?? "";
                numPosologieJour.Value = Math.Max(1, Math.Min(p.NbFoisParJour, 10));

                // ── Vente en détail ───────────────────────────────────────
                bool venteDetail = p.NbUniteParBoite > 1;

                // ✅ Abaisser le Minimum à 1 AVANT d'assigner la valeur
                // pour éviter ArgumentOutOfRangeException si NbUniteParBoite = 1
                numNbUniteParBoite.Minimum = 1;
                numNbUniteParBoite.Value = Math.Max(1, p.NbUniteParBoite);

                // Remonter le Minimum à 2 seulement si vente en détail activée
                if (venteDetail)
                    numNbUniteParBoite.Minimum = 2;

                chkVenteDetail.Checked = venteDetail;

                // Sélectionner l'unité dans le combo
                int idx = cmbUniteVente.FindStringExact(p.UniteVente);
                if (idx >= 0)
                    cmbUniteVente.SelectedIndex = idx;
                else
                {
                    // Unité personnalisée — l'ajouter
                    cmbUniteVente.Items.Add(p.UniteVente);
                    cmbUniteVente.SelectedItem = p.UniteVente;
                }
            }
        }

        // ── Bandeau « fiche à vérifier » (prix, péremption) ──────────────

        private List<string> _motifsFiche = new();

        private void AfficherBandeauFiche()
        {
            if (!_ficheMarquee || _motifsFiche.Count == 0) { pnlFiche.Visible = false; return; }
            lblFiche.Text = "Fiche à vérifier (ancienne base) :" + Environment.NewLine + string.Join(Environment.NewLine, _motifsFiche.Select(m => "• " + m));
            pnlFiche.Visible = true;
        }

        private void btnFicheVerifiee_Click(object sender, EventArgs e)
        {
            if (_produitId == null) return;
            FicheAVerifierService.MarquerVerifiee(_produitId.Value);
            _ficheMarquee = false;
            _motifsFiche = new List<string>();
            pnlFiche.Visible = false;
            BandeauNotification.Succes("Fiche vérifiée");
        }

        // ── Bandeau « stock à vérifier » ──────────────────────────────────

        private void AfficherBandeauVerification()
        {
            if (!_verifier || _produitOrigine == null) return;

            lblVerification.Text = "Le stock de ce produit est peut-être incorrect (ancienne erreur du logiciel).\n" + _motifVerification;
            pnlVerification.BackColor = Theme.AttentionFond;
            lblVerification.ForeColor = Theme.AttentionTexte;

            if (_unitesManquantes > 0)
            {
                int nouveau = _produitOrigine.QuantiteEnStock + _unitesManquantes;
                btnCorrection.Text = $"Appliquer la correction (+ {_unitesManquantes} unités, nouveau stock : {StockService.Formater(_produitOrigine, nouveau)})";
                btnCorrection.Click += (s, e) => AppliquerCorrection(nouveau);
                btnCorrection.Visible = true;
            }
            else
            {
                btnCorrection.Visible = false;   // « Reçu partiellement » : quantité inconnue, pas d'estimation
            }

            btnStockCorrect.Click += (s, e) => ResoudreEtFermer(ChoixVerification.StockCorrect,
                "Confirmer que le stock actuel (" + StockService.Formater(_produitOrigine) + ") est correct ?",
                "Le marquage « à vérifier » est levé. Le stock n'est pas modifié.");

            btnCompte.Click += (s, e) =>
            {
                lblVerification.Text += "\nSaisissez le stock compté (boîtes pleines + unités en vrac), puis enregistrez : le marquage sera levé.";
                numQuantite.Focus();
                numQuantite.Select(0, numQuantite.Text.Length);
            };

            pnlVerification.Visible = true;
        }

        private void AppliquerCorrection(int nouveauStock)
        {
            var conf = MessageBox.Show(
                $"Ajouter {Format.Compte(_unitesManquantes, "unité")} au stock ?\n\n" +
                $"Stock actuel : {StockService.Formater(_produitOrigine)}\n" +
                $"Nouveau stock : {StockService.Formater(_produitOrigine, nouveauStock)}\n\n" +
                "Ce chiffre est une estimation. Si des ventes ont eu lieu depuis, comptez plutôt le stock en rayon.",
                "Appliquer la correction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                VerificationStockService.Resoudre(_produitId.Value, ChoixVerification.CorrectionAppliquee);
                BandeauNotification.Succes("Stock corrigé");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Correction de stock", ex);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResoudreEtFermer(ChoixVerification choix, string question, string resultat)
        {
            if (MessageBox.Show(question, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                VerificationStockService.Resoudre(_produitId.Value, choix);
                BandeauNotification.Succes(resultat);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Vérification de stock", ex);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChargerFournisseurs()
        {
            using (var ctx = new AppDbContext())
            {
                int? fournisseurActuel = _produitId == null ? null
                    : ctx.produits.Where(x => x.Id == _produitId.Value).Select(x => x.FournisseurId).FirstOrDefault();
                _fournisseurs = ctx.fournisseur
                    .Where(f => f.Actif || f.Id == fournisseurActuel)
                    .OrderBy(f => f.Nom).ToList();
                cbFournisseur.DataSource = _fournisseurs;
                cbFournisseur.DisplayMember = "Nom";
                cbFournisseur.ValueMember = "Id";
                cbFournisseur.SelectedIndex = -1;
            }
        }

        // ── Activation/désactivation du bloc vente en détail ─────────────

        private void chkVenteDetail_CheckedChanged(object sender, EventArgs e)
        {
            bool actif = chkVenteDetail.Checked;

            pnlVenteDetail.Enabled = actif;
            numUnitesVrac.Enabled = actif;
            if (!actif) { numUnitesVrac.Value = 0; numPrixVenteUnite.Value = 0; }
            pnlVenteDetail.BackColor = actif ? Theme.SuccesFond : Theme.Fond;

            if (!actif)
            {
                // Désactivé → remettre à 1 (boîte entière seulement)
                numNbUniteParBoite.Minimum = 1;
                numNbUniteParBoite.Value = 1;
                cmbUniteVente.SelectedIndex = 0;
                lblApercu.Text = "";
            }
            else
            {
                // Activé → minimum 2 unités par boîte
                numNbUniteParBoite.Minimum = 2;
                if (numNbUniteParBoite.Value < 2)
                    numNbUniteParBoite.Value = 2;
                MettreAJourApercu(null, null);
            }
        }

        // ── Aperçu en temps réel de l'équivalence ─────────────────────────

        private void MettreAJourApercu(object sender, EventArgs e)
        {
            if (!chkVenteDetail.Checked) { lblApercu.Text = ""; return; }

            string unite = cmbUniteVente.SelectedItem?.ToString() ?? "unité";
            int nbParB = (int)numNbUniteParBoite.Value;

            if (nbParB <= 1)
            {
                lblApercu.Text = "Le nombre d'unités doit être supérieur à 1.";
                lblApercu.ForeColor = Theme.AttentionTexte;
                return;
            }

            // Les deux prix sont ceux saisis par le client : rien n'est recalculé
            bool aVerifier = _produitId != null && _nbParBoiteOrigine > 1 && nbParB != _nbParBoiteOrigine;
            lblApercu.Text =
                $"1 boîte = {nbParB} {Format.Pluriel(nbParB, unite)}" + "\n" +
                $"Prix de la boîte : {Format.Montant(numPrixVente.Value)}   |   Prix par {unite} : {Format.Montant(numPrixVenteUnite.Value)}" + "\n" +
                $"Exemple : vendre 4 {Format.Pluriel(4, unite)} consomme " +
                $"{Format.Compte((int)Math.Ceiling(4.0 / nbParB), "boîte")} du stock" +
                (aVerifier ? "\n" + "Vérifiez le prix à l'unité" : "");

            lblApercu.ForeColor = aVerifier ? Theme.AttentionTexte : Theme.SuccesTexte;
        }

        // ── Validation et enregistrement ──────────────────────────────────

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            // ── Validations ───────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom du produit est obligatoire.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNom.Focus(); return;
            }

            decimal prixAchat = numPrixAchat.Value;
            decimal prixVente = numPrixVente.Value;

            // Vérification marge obligatoire
            if (prixVente <= prixAchat)
            {
                MessageBox.Show(
                    $"Le prix de vente ({Format.Montant(prixVente)}) doit être supérieur au prix d'achat ({Format.Montant(prixAchat)}).\n\nAugmentez le prix de vente ou corrigez le prix d'achat.",
                    "Marge invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrixVente.Focus(); return;
            }

            if (prixAchat <= 0)
            {
                MessageBox.Show("Le prix d'achat doit être supérieur à 0.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrixAchat.Focus(); return;
            }

            // Vente en détail : « Boîte » ne peut pas être l'unité de détail (la boîte est déjà l'unité de base)
            if (chkVenteDetail.Checked && string.Equals(cmbUniteVente.SelectedItem?.ToString(), StockService.UniteBoite, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "« Boîte » ne peut pas être l'unité de détail : la boîte est déjà l'unité de base.\n\nChoisissez ce que l'on vend à l'unité : Plaquette, Comprimé, Gélule, Ampoule, Flacon, Sachet, Tube…",
                    "Unité de détail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUniteVente.Focus(); return;
            }

            // Vente en détail — vérification
            if (chkVenteDetail.Checked && numNbUniteParBoite.Value <= 1)
            {
                MessageBox.Show(
                    "Si le produit est vendu à l'unité, le nombre d'unités par boîte doit être ≥ 2.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numNbUniteParBoite.Focus(); return;
            }

            // Règle du client : prix de vente > prix d'achat, à la boîte ET à l'unité ; le prix à l'unité est saisi, jamais calculé
            decimal? prixVenteUnite = null;
            if (chkVenteDetail.Checked)
            {
                prixVenteUnite = numPrixVenteUnite.Value;
                decimal coutUnite = prixAchat / numNbUniteParBoite.Value;
                if (prixVenteUnite <= 0)
                {
                    MessageBox.Show("Saisissez le prix de vente à l'unité : c'est vous qui le fixez, il n'est jamais calculé à partir du prix de la boîte.",
                        "Prix à l'unité obligatoire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numPrixVenteUnite.Focus(); return;
                }
                if (prixVenteUnite <= coutUnite)
                {
                    MessageBox.Show(
                        $"Le prix de vente à l'unité ({Format.Montant(prixVenteUnite.Value)}) doit être supérieur au prix d'achat d'une unité ({Format.Montant(Math.Round(coutUnite, 2))}, soit {Format.Montant(prixAchat)} pour {numNbUniteParBoite.Value:0} unités).\n\nAugmentez le prix à l'unité.",
                        "Marge invalide à l'unité", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numPrixVenteUnite.Focus(); return;
                }
            }

            // Date de péremption : obligatoire (jamais la date du jour par défaut), sauf « Pas de date de péremption »
            if (!dtpDateExpiration.Checked && !chkPasPeremption.Checked)
            {
                MessageBox.Show("Saisissez la date de péremption (cochez la case devant la date), ou cochez « Pas de date de péremption ».",
                    "Date de péremption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDateExpiration.Focus(); return;
            }
            bool peremptionConnue = dtpDateExpiration.Checked;
            DateTime dateExpiration = peremptionConnue
                ? dtpDateExpiration.Value.Date
                : (_produitId != null && _dateOrigine.Year >= 2000 ? _dateOrigine : new DateTime(2099, 12, 31));   // valeur sans effet : PeremptionConnue = false

            // La marge enregistrée est déduite des deux prix : (prix de vente − prix d'achat) ÷ prix d'achat × 100
            decimal marge = MargeService.Enregistree(prixAchat, prixVente);

            int? fournisseurId = cbFournisseur.SelectedValue is int fId && fId > 0
                                   ? fId : (int?)null;

            // Unité et nb/boîte selon le choix
            string uniteVente = chkVenteDetail.Checked
                                  ? (cmbUniteVente.SelectedItem?.ToString() ?? "Boîte")
                                  : "Boîte";
            int nbUniteParBoite = chkVenteDetail.Checked
                                  ? (int)numNbUniteParBoite.Value
                                  : 1;

            // ── Stock saisi : boîtes pleines × nb par boîte + unités en vrac ──
            int stockEnUnites = nbUniteParBoite > 1
                                ? (int)numQuantite.Value * nbUniteParBoite + (int)numUnitesVrac.Value
                                : (int)numQuantite.Value;

            // Stock réécrit seulement si l'utilisateur l'a touché (ou si NbUniteParBoite change)
            bool stockTouche = _produitId == null
                || (int)numQuantite.Value != _boitesOrigine
                || (int)numUnitesVrac.Value != _vracOrigine
                || nbUniteParBoite != _nbParBoiteOrigine;

            // Changement de NbUniteParBoite avec du stock : ressaisie obligatoire du stock réel
            if (_produitId != null && (_stockOrigineUnites > 0 || _aVentesUnite) && nbUniteParBoite != _nbParBoiteOrigine)
            {
                if (!_nbAverti)
                {
                    _nbAverti = true;
                    MessageBox.Show(
                        $"Vous modifiez le nombre d'unités par boîte (de {_nbParBoiteOrigine} à {nbUniteParBoite}).\n\n" +
                        (_aVentesUnite ? "Ce produit a déjà été vendu à l'unité : les anciennes ventes gardent leur conditionnement d'origine, mais le stock doit être recompté avec le nouveau conditionnement.\n\n" : "") +
                        "Le stock actuel ne peut plus être converti automatiquement.\n" +
                        "Comptez le stock réel en rayon et saisissez-le (boîtes pleines + unités en vrac), puis enregistrez à nouveau.",
                        "Stock à ressaisir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numQuantite.Focus();
                    return;
                }

                var conf = MessageBox.Show(
                    $"Stock réel saisi : {Format.Compte((int)numQuantite.Value, "boîte")} + {Format.Compte((int)numUnitesVrac.Value, "unité")} " +
                    $"= {Format.Compte(stockEnUnites, "unité")} de base.\n\nConfirmer que c'est bien le stock réel ?",
                    "Confirmer le stock", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (conf != DialogResult.Yes) return;
            }

                        if (nbUniteParBoite > 1 && numUnitesVrac.Value >= nbUniteParBoite)
            {
                MessageBox.Show(
                    $"Les unités en vrac doivent être inférieures au nombre d'unités par boîte ({nbUniteParBoite}).\n" +
                    "Ajoutez plutôt une boîte pleine.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numUnitesVrac.Focus(); return;
            }

            // Le nombre d'unités par boîte change : le prix à l'unité n'est PAS modifié automatiquement, le client doit le confirmer
            if (_produitId != null && chkVenteDetail.Checked && _nbParBoiteOrigine > 1 && nbUniteParBoite != _nbParBoiteOrigine)
            {
                var confPrix = MessageBox.Show(
                    $"Vérifiez le prix à l'unité : {Format.Montant(prixVenteUnite!.Value)} pour 1 {uniteVente.ToLowerInvariant()} (boîte de {nbUniteParBoite}, prix de la boîte {Format.Montant(prixVente)}).\n\nConfirmer ce prix ?",
                    "Vérifiez le prix à l'unité", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confPrix != DialogResult.Yes) { numPrixVenteUnite.Focus(); return; }
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    if (_produitId == null)
                    {
                        ctx.produits.Add(new Produit
                        {
                            Nom = txtNom.Text.Trim(),
                            Type = cmbType.SelectedItem?.ToString() ?? "Autre",
                            PrixAchat = prixAchat,
                            MargeBeneficiaire = marge,
                            PrixVente = prixVente,
                            PrixVenteUnite = prixVenteUnite,
                            QuantiteEnStock = stockEnUnites,
                            SeuilAlerte = (int)numSeuil.Value,
                            DateExpiration = dateExpiration,
                            PeremptionConnue = peremptionConnue,
                            UniteVente = uniteVente,
                            NbUniteParBoite = nbUniteParBoite,
                            FournisseurId = fournisseurId,
                            // Détails médicament pour le caissier
                            Indication = txtIndication.Text.Trim(),
                            Posologie = txtPosologie.Text.Trim(),
                            NbFoisParJour = (int)numPosologieJour.Value
                        });
                    }
                    else
                    {
                        var p = ctx.produits.Find(_produitId.Value);
                        if (p == null) return;

                        p.Nom = txtNom.Text.Trim();
                        p.Type = cmbType.SelectedItem?.ToString() ?? "Autre";
                        p.PrixAchat = prixAchat;
                        p.MargeBeneficiaire = marge;
                        p.PrixVente = prixVente;
                        p.PrixVenteUnite = prixVenteUnite;

                        // Un nouveau prix ou une nouvelle date lève le motif correspondant de la fiche à vérifier
                        // (saisir une date pour une péremption jusque-là inconnue repasse PeremptionConnue à true)
                        if (p.FicheAVerifier)
                        {
                            string avant = p.MotifsFiche ?? "";
                            bool prixChange = prixAchat != _prixAchatOrigine || prixVente != _prixVenteOrigine || prixVenteUnite != _prixUniteOrigine;
                            bool dateChange = peremptionConnue && (!_peremptionOrigine || dateExpiration.Date != _dateOrigine.Date);
                            if (uniteVente != StockService.UniteBoite || nbUniteParBoite <= 1)
                                FicheAVerifierService.Lever(p, FicheAVerifierService.PrefixeConditionnement);   // unité de détail corrigée
                            if (prixChange) FicheAVerifierService.Lever(p, FicheAVerifierService.PrefixePrix);
                            if (dateChange) FicheAVerifierService.Lever(p, FicheAVerifierService.PrefixePeremption);
                            if ((prixChange || dateChange) && avant != (p.MotifsFiche ?? ""))
                                Journal.Info($"Fiche produit « {p.Nom} » : motifs levés par correction ({(prixChange ? "prix" : "")}{(prixChange && dateChange ? ", " : "")}{(dateChange ? "date" : "")}). Avant : {avant}");
                        }
                        if (stockTouche)
                        {
                            int ancienStock = p.QuantiteEnStock;
                            p.QuantiteEnStock = stockEnUnites;

                            // Stock saisi/enregistré à la main : le marquage « à vérifier » est levé
                            if (p.StockAVerifier)
                            {
                                p.StockAVerifier = false;
                                p.UnitesManquantesEstimees = 0;
                                VerificationStockService.Tracer(p, ancienStock, ChoixVerification.StockRecompte);
                            }
                        }
                        p.SeuilAlerte = (int)numSeuil.Value;
                        p.DateExpiration = dateExpiration;
                        p.PeremptionConnue = peremptionConnue;
                        p.UniteVente = uniteVente;
                        p.NbUniteParBoite = nbUniteParBoite;
                        p.FournisseurId = fournisseurId;
                        p.Indication = txtIndication.Text.Trim();
                        p.Posologie = txtPosologie.Text.Trim();
                        p.NbFoisParJour = (int)numPosologieJour.Value;
                    }

                    ctx.SaveChanges();
                }

                BandeauNotification.Succes("Produit enregistré : " + txtNom.Text.Trim());

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}