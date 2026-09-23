using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class FormAddProduit : Form
    {
        private List<Fournisseur> _fournisseurs;
        private readonly int? _produitId; // null = ajout, int = modification

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

                // Afficher le stock en BOÎTES (l'admin pense en boîtes)
                // Le stock est stocké en unités → convertir pour affichage
                int stockEnBoites = p.NbUniteParBoite > 1
                                    ? p.QuantiteEnStock / p.NbUniteParBoite
                                    : p.QuantiteEnStock;
                numQuantite.Value = stockEnBoites;
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

            // ── Conversion stock : l'admin saisit en BOÎTES
            // On stocke en UNITÉS DE BASE pour la gestion précise du stock
            // Ex : 2 boîtes × 5 plaquettes = 10 unités stockées
            int stockSaisi = (int)numQuantite.Value;
            int stockEnUnites = nbUniteParBoite > 1
                                ? stockSaisi * nbUniteParBoite
                                : stockSaisi;

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
                        p.QuantiteEnStock = stockEnUnites;
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