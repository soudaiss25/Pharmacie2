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

        // ── Constructeurs ─────────────────────────────────────────────────

        public FormAddProduit()
        {
            _produitId = null;
            InitializeComponent();
            Theme.Appliquer(this);
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
            cmbUniteVente.SelectedIndex = 0;
            ChargerFournisseurs();
            WireEvents();
            this.Text = AppInfo.Titre("Modifier le produit");
            PreRemplir(produitId);
            AfficherBandeauVerification();
        }

        // ── Câblage des événements ────────────────────────────────────────

        private void WireEvents()
        {
            numPrixAchat.ValueChanged += RecalculerPrixVente;
            numMarge.ValueChanged += RecalculerPrixVente;
            numPrixVente.ValueChanged += VerifierMarge;
            chkVenteDetail.CheckedChanged += chkVenteDetail_CheckedChanged;
            cmbUniteVente.SelectedIndexChanged += MettreAJourApercu;
            numNbUniteParBoite.ValueChanged += MettreAJourApercu;
            numPrixVente.ValueChanged += MettreAJourApercu;
        }

        /// <summary>Colore le champ prix de vente en rouge si la marge est nulle ou négative.</summary>
        private void VerifierMarge(object sender, EventArgs e)
        {
            decimal achat = numPrixAchat.Value;
            bool ok = numPrixVente.Value > achat && achat > 0;

            numPrixVente.BackColor = ok ? Theme.SuccesFond : Theme.UrgentFond;
            lblMarge.Text = !ok && achat > 0 ? "Marge bénéfice (%) : trop faible" : "Marge bénéfice (%)";
        }

        // ── Pré-remplissage en mode modification ──────────────────────────

        private void PreRemplir(int id)
        {
            using (var ctx = new AppDbContext())
            {
                var p = ctx.produits.Find(id);
                if (p == null) return;

                txtNom.Text = p.Nom;
                cmbType.Text = p.Type;
                // Ordre important : le prix de vente est saisi EN DERNIER (achat et marge le recalculent)
                numPrixAchat.Value = Math.Min(numPrixAchat.Maximum, p.PrixAchat);
                numMarge.Value = Math.Min(numMarge.Maximum, Math.Max(numMarge.Minimum, p.MargeBeneficiaire));
                numPrixVente.Value = Math.Min(numPrixVente.Maximum, p.PrixVente);

                // Stock en unités de base → boîtes pleines + unités en vrac (la boîte entamée reste visible)
                int nbParBoite = Math.Max(1, p.NbUniteParBoite);
                _stockOrigineUnites = p.QuantiteEnStock;
                _verifier = p.StockAVerifier;
                _unitesManquantes = p.UnitesManquantesEstimees;
                _motifVerification = p.MotifVerification ?? "";
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

                if (p.DateExpiration > DateTime.MinValue)
                    dtpDateExpiration.Value = p.DateExpiration;

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

        // ── Calcul automatique prix de vente ──────────────────────────────

        private void RecalculerPrixVente(object sender, EventArgs e)
        {
            decimal pv = Math.Round(numPrixAchat.Value * (1m + numMarge.Value / 100m), 0, MidpointRounding.AwayFromZero);
            numPrixVente.Value = Math.Min(numPrixVente.Maximum, pv);
        }

        // ── Activation/désactivation du bloc vente en détail ─────────────

        private void chkVenteDetail_CheckedChanged(object sender, EventArgs e)
        {
            bool actif = chkVenteDetail.Checked;

            pnlVenteDetail.Enabled = actif;
            numUnitesVrac.Enabled = actif;
            if (!actif) numUnitesVrac.Value = 0;
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

            // Prix par unité
            decimal pv = numPrixVente.Value;
            decimal prixParUnite = Math.Round(pv / nbParB, 2);

            lblApercu.Text =
                $"1 boîte = {nbParB} {Format.Pluriel(nbParB, unite)}\n" +
                $"Prix par {unite} : {Format.Montant(prixParUnite)}\n" +
                $"Exemple : vendre 4 {Format.Pluriel(4, unite)} consomme " +
                $"{Format.Compte((int)Math.Ceiling(4.0 / nbParB), "boîte")} du stock";

            lblApercu.ForeColor = Theme.SuccesTexte;
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
                    $"Le prix de vente ({Format.Montant(prixVente)}) doit être supérieur au prix d'achat ({Format.Montant(prixAchat)}).\n\nAjustez la marge bénéficiaire.",
                    "Marge invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMarge.Focus(); return;
            }

            if (prixAchat <= 0)
            {
                MessageBox.Show("Le prix d'achat doit être supérieur à 0.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrixAchat.Focus(); return;
            }

            // Vente en détail — vérification
            if (chkVenteDetail.Checked && numNbUniteParBoite.Value <= 1)
            {
                MessageBox.Show(
                    "Si le produit est vendu à l'unité, le nombre d'unités par boîte doit être ≥ 2.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numNbUniteParBoite.Focus(); return;
            }

            decimal marge = numMarge.Value;

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
            if (_produitId != null && _stockOrigineUnites > 0 && nbUniteParBoite != _nbParBoiteOrigine)
            {
                if (!_nbAverti)
                {
                    _nbAverti = true;
                    MessageBox.Show(
                        $"Vous modifiez le nombre d'unités par boîte (de {_nbParBoiteOrigine} à {nbUniteParBoite}).\n\n" +
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
                            QuantiteEnStock = stockEnUnites,
                            SeuilAlerte = (int)numSeuil.Value,
                            DateExpiration = dtpDateExpiration.Value,
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
                        p.DateExpiration = dtpDateExpiration.Value;
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