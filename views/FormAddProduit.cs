using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;

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
            ChargerFournisseurs();
            WireEvents();
            this.Load += LimiterHauteur;
            this.Text = "➕  Ajouter un produit";
        }

        public FormAddProduit(int produitId)
        {
            _produitId = produitId;
            InitializeComponent();
            ChargerFournisseurs();
            WireEvents();
            this.Load += LimiterHauteur;
            this.Text = "✏️  Modifier le produit";
            PreRemplir(produitId);
            AfficherBandeauVerification();
        }

        /// <summary>
        /// Limite la hauteur à 90% de l'écran pour que le formulaire
        /// ne dépasse jamais la barre des tâches.
        /// </summary>
        private void LimiterHauteur(object sender, EventArgs e)
        {
            int hauteurMax = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.90);
            if (this.Height > hauteurMax)
                this.Height = hauteurMax;

            // Recentrer après ajustement
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
        }

        // ── Câblage des événements ────────────────────────────────────────

        private void WireEvents()
        {
            txtPrixAchat.TextChanged += RecalculerPrixVente;
            txtMarge.TextChanged += RecalculerPrixVente;
            txtPrixVente.TextChanged += VerifierMarge;
            chkVenteDetail.CheckedChanged += chkVenteDetail_CheckedChanged;
            cmbUniteVente.SelectedIndexChanged += MettreAJourApercu;
            numNbUniteParBoite.ValueChanged += MettreAJourApercu;
            txtPrixVente.TextChanged += MettreAJourApercu;
        }

        /// <summary>Colore le champ prix vente en rouge si marge <= 0.</summary>
        private void VerifierMarge(object sender, EventArgs e)
        {
            bool ok = decimal.TryParse(txtPrixAchat.Text,
                          System.Globalization.NumberStyles.Any,
                          System.Globalization.CultureInfo.InvariantCulture, out decimal achat)
                   && decimal.TryParse(txtPrixVente.Text,
                          System.Globalization.NumberStyles.Any,
                          System.Globalization.CultureInfo.InvariantCulture, out decimal vente)
                   && vente > achat && achat > 0;

            txtPrixVente.BackColor = ok
                ? System.Drawing.Color.LightGreen
                : System.Drawing.Color.FromArgb(255, 200, 200);  // rouge clair si marge nulle

            if (!ok && achat > 0)
                lblMarge.Text = "⚠️ Marge (%) →";
            else
                lblMarge.Text = "Marge bénéfice (%) →";
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
                txtPrixAchat.Text = p.PrixAchat.ToString("0.00", CultureInfo.InvariantCulture);
                txtMarge.Text = p.MargeBeneficiaire.ToString("0.00", CultureInfo.InvariantCulture);
                txtPrixVente.Text = p.PrixVente.ToString("0.00", CultureInfo.InvariantCulture);

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

            const int h = 150;
            var orange = System.Drawing.Color.FromArgb(255, 224, 178);
            var pnl = new Panel
            {
                BackColor = orange,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new System.Drawing.Point(14, 8),
                Size = new System.Drawing.Size(ClientSize.Width - 28, h)
            };

            var lbl = new Label
            {
                Text = "⚠ Le stock de ce produit est peut-être incorrect (ancienne erreur du logiciel).\n" + _motifVerification,
                Location = new System.Drawing.Point(10, 8),
                Size = new System.Drawing.Size(pnl.Width - 24, 56),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(120, 53, 0)
            };
            pnl.Controls.Add(lbl);

            int y = 70;
            if (_unitesManquantes > 0)
            {
                int nouveau = _produitOrigine.QuantiteEnStock + _unitesManquantes;
                var btnCorr = new Button
                {
                    Text = $"Appliquer la correction (+ {_unitesManquantes} unités → nouveau stock : {StockService.Formater(_produitOrigine, nouveau)})",
                    Location = new System.Drawing.Point(10, y),
                    Size = new System.Drawing.Size(pnl.Width - 24, 30)
                };
                btnCorr.Click += (s, e) => AppliquerCorrection(nouveau);
                pnl.Controls.Add(btnCorr);
                y += 36;
            }

            var btnOk = new Button
            {
                Text = "Le stock est correct",
                Location = new System.Drawing.Point(10, y),
                Size = new System.Drawing.Size(200, 30)
            };
            btnOk.Click += (s, e) => ResoudreEtFermer(ChoixVerification.StockCorrect,
                "Confirmer que le stock actuel (" + StockService.Formater(_produitOrigine) + ") est correct ?",
                "Le marquage « à vérifier » est levé. Le stock n'est pas modifié.");
            pnl.Controls.Add(btnOk);

            var btnCompte = new Button
            {
                Text = "J'ai compté le stock",
                Location = new System.Drawing.Point(220, y),
                Size = new System.Drawing.Size(200, 30)
            };
            btnCompte.Click += (s, e) =>
            {
                lbl.Text += "\nSaisissez le stock compté (boîtes pleines + unités en vrac), puis enregistrez : le marquage sera levé.";
                numQuantite.Focus();
                numQuantite.Select(0, numQuantite.Text.Length);
            };
            pnl.Controls.Add(btnCompte);

            // Décaler le formulaire existant sous le bandeau
            foreach (Control c in Controls) c.Top += h + 12;
            ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + h + 12);
            Controls.Add(pnl);
        }

        private void AppliquerCorrection(int nouveauStock)
        {
            var conf = MessageBox.Show(
                $"Ajouter {_unitesManquantes} unité(s) au stock ?\n\n" +
                $"Stock actuel : {StockService.Formater(_produitOrigine)}\n" +
                $"Nouveau stock : {StockService.Formater(_produitOrigine, nouveauStock)}\n\n" +
                "Ce chiffre est une estimation. Si des ventes ont eu lieu depuis, comptez plutôt le stock en rayon.",
                "Appliquer la correction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                VerificationStockService.Resoudre(_produitId.Value, ChoixVerification.CorrectionAppliquee);
                MessageBox.Show("✅ Stock corrigé.", "Correction appliquée", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(resultat, "Enregistré", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                _fournisseurs = ctx.fournisseur.OrderBy(f => f.Nom).ToList();
                cbFournisseur.DataSource = _fournisseurs;
                cbFournisseur.DisplayMember = "Nom";
                cbFournisseur.ValueMember = "Id";
                cbFournisseur.SelectedIndex = -1;
            }
        }

        // ── Calcul automatique prix de vente ──────────────────────────────

        private void RecalculerPrixVente(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrixAchat.Text, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out decimal achat)) return;
            if (!decimal.TryParse(txtMarge.Text, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out decimal marge)) return;

            decimal pv = Math.Round(achat * (1m + marge / 100m), 2);
            txtPrixVente.Text = pv.ToString("0.00", CultureInfo.InvariantCulture);
        }

        // ── Activation/désactivation du bloc vente en détail ─────────────

        private void chkVenteDetail_CheckedChanged(object sender, EventArgs e)
        {
            bool actif = chkVenteDetail.Checked;

            pnlVenteDetail.Enabled = actif;
            numUnitesVrac.Enabled = actif;
            if (!actif) numUnitesVrac.Value = 0;
            pnlVenteDetail.BackColor = actif
                ? System.Drawing.Color.FromArgb(232, 245, 233)
                : System.Drawing.Color.FromArgb(245, 245, 245);

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
                lblApercu.Text = "⚠️ Le nombre d'unités doit être supérieur à 1.";
                lblApercu.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            // Calcul prix par unité
            decimal pv = 0;
            decimal.TryParse(txtPrixVente.Text, NumberStyles.Any,
                CultureInfo.InvariantCulture, out pv);

            decimal prixParUnite = nbParB > 0 ? Math.Round(pv / nbParB, 2) : 0;

            lblApercu.Text =
                $"✅  1 boîte  =  {nbParB} {unite}(s)\n" +
                $"     Prix / {unite} : {prixParUnite:N0} KMF\n" +
                $"     Exemple : vendre 4 {unite}(s) = consomme " +
                $"{(int)Math.Ceiling(4.0 / nbParB)} boîte(s) du stock";

            lblApercu.ForeColor = System.Drawing.Color.FromArgb(27, 94, 32);
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

            if (!decimal.TryParse(txtPrixAchat.Text, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out decimal prixAchat))
            {
                MessageBox.Show("Prix d'achat invalide.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixAchat.Focus(); return;
            }

            if (!decimal.TryParse(txtPrixVente.Text, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out decimal prixVente))
            {
                MessageBox.Show("Prix de vente invalide.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixVente.Focus(); return;
            }

            // ✅ Vérification marge obligatoire
            if (prixVente <= prixAchat)
            {
                MessageBox.Show(
                    $"Le prix de vente ({prixVente:N0} KMF) doit être supérieur au prix d'achat ({prixAchat:N0} KMF).\n\nAjustez la marge bénéficiaire.",
                    "Marge invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMarge.Focus(); return;
            }

            if (prixAchat <= 0)
            {
                MessageBox.Show("Le prix d'achat doit être supérieur à 0.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrixAchat.Focus(); return;
            }

            // Vente en détail — vérification
            if (chkVenteDetail.Checked && numNbUniteParBoite.Value <= 1)
            {
                MessageBox.Show(
                    "Si le produit est vendu à l'unité, le nombre d'unités par boîte doit être ≥ 2.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numNbUniteParBoite.Focus(); return;
            }

            decimal.TryParse(txtMarge.Text, NumberStyles.Any,
                CultureInfo.InvariantCulture, out decimal marge);

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
                        $"Vous modifiez le nombre d'unités par boîte ({_nbParBoiteOrigine} → {nbUniteParBoite}).\n\n" +
                        "Le stock actuel ne peut plus être converti automatiquement.\n" +
                        "Comptez le stock réel en rayon et saisissez-le (boîtes pleines + unités en vrac), puis enregistrez à nouveau.",
                        "Stock à ressaisir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numQuantite.Focus();
                    return;
                }

                var conf = MessageBox.Show(
                    $"Stock réel saisi : {numQuantite.Value} boîte(s) + {numUnitesVrac.Value} unité(s) " +
                    $"= {stockEnUnites} unité(s) de base.\n\nConfirmer que c'est bien le stock réel ?",
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

                MessageBox.Show("✅ Produit enregistré avec succès !", "Succès",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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