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
    /// Formulaire de commande d'un produit à son fournisseur.
    /// Conserve le constructeur (int produitId, string nomProduit) pour compatibilité.
    /// Fournisseur auto-détecté depuis le produit, champs optionnels enrichis.
    /// </summary>
    public partial class FormCommandeProduit : Form
    {
        private readonly int _produitId;
        private readonly string _nomProduit;

        public FormCommandeProduit(int produitId, string nomProduit)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _produitId = produitId;
            _nomProduit = nomProduit;

            lblProduit.Text = $"Produit : {nomProduit}";

            // Charger le produit pour pré-remplir les champs
            using (var ctx = new AppDbContext())
            {
                var produit = ctx.produits
                    .Include(p => p.Fournisseur)
                    .FirstOrDefault(p => p.Id == produitId);

                if (produit != null)
                {
                    // Quantité suggérée en BOÎTES = seuil - boîtes pleines en stock + 1 (au minimum 1)
                    int qteManquante = produit.SeuilAlerte - produit.NbBoitesEnStock + 1;
                    numQuantite.Value = Math.Max(1, qteManquante);

                    // Prix achat pré-rempli
                    if (produit.PrixAchat > 0)
                        numPrixUnitaire.Value = produit.PrixAchat;

                    // Fournisseur pré-rempli si déjà associé
                    if (produit.Fournisseur != null)
                    {
                        txtFournisseur.Text = produit.Fournisseur.Nom;
                        lblFournisseurInfo.Text =
                            string.IsNullOrWhiteSpace(produit.Fournisseur.Contact)
                                ? "Contact : —"
                                : $"Contact : {produit.Fournisseur.Contact}";
                        lblFournisseurInfo.Visible = true;
                    }
                    else
                    {
                        ChargerAutocompletionFournisseurs();
                    }
                }
                else
                {
                    numQuantite.Value = 1;
                    ChargerAutocompletionFournisseurs();
                }
            }

            // Date de livraison par défaut dans 7 jours
            dtpLivraisonPrevue.Value = DateTime.Today.AddDays(7);
        }

        // ── Autocomplétion fournisseurs ───────────────────────────────────

        private void ChargerAutocompletionFournisseurs()
        {
            using (var ctx = new AppDbContext())
            {
                var noms = ctx.fournisseur.Where(f => f.Actif).Select(f => f.Nom).ToArray();
                var auto = new AutoCompleteStringCollection();
                auto.AddRange(noms);
                txtFournisseur.AutoCompleteCustomSource = auto;
                txtFournisseur.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtFournisseur.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }

        // ── Info fournisseur quand on quitte le champ ─────────────────────

        private void txtFournisseur_Leave(object sender, EventArgs e)
        {
            string nom = txtFournisseur.Text.Trim();
            if (string.IsNullOrEmpty(nom)) return;

            using (var ctx = new AppDbContext())
            {
                var f = ctx.fournisseur
                    .FirstOrDefault(x => x.Nom.ToLower() == nom.ToLower());

                if (f != null && !f.Actif)
                {
                    lblFournisseurInfo.Text = "Fournisseur archivé : réactivez-le avant de commander";
                    lblFournisseurInfo.ForeColor = Theme.UrgentTexte;
                }
                else if (f != null)
                {
                    lblFournisseurInfo.Text =
                        string.IsNullOrWhiteSpace(f.Contact) ? "Contact : —" : $"Contact : {f.Contact}";
                    lblFournisseurInfo.ForeColor = Theme.Neutre;
                }
                else
                {
                    lblFournisseurInfo.Text = "Nouveau fournisseur : il sera créé automatiquement";
                    lblFournisseurInfo.ForeColor = Theme.AttentionTexte;
                }
                lblFournisseurInfo.Visible = true;
            }
        }

        // ── Date livraison optionnelle ────────────────────────────────────

        private void chkDateLivraison_CheckedChanged(object sender, EventArgs e)
            => dtpLivraisonPrevue.Enabled = chkDateLivraison.Checked;

        // ── Validation et enregistrement ──────────────────────────────────

        private void btnValider_Click(object sender, EventArgs e)
        {
            string fournisseurNom = txtFournisseur.Text.Trim();
            int qte = (int)numQuantite.Value;
            decimal prixU = numPrixUnitaire.Value;

            if (string.IsNullOrWhiteSpace(fournisseurNom))
            {
                MessageBox.Show("Le nom du fournisseur est obligatoire.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFournisseur.Focus();
                return;
            }

            if (qte <= 0)
            {
                MessageBox.Show("La quantité doit être supérieure à 0.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    // Chercher ou créer le fournisseur
                    var fournisseur = ctx.fournisseur
                        .FirstOrDefault(f => f.Nom.ToLower() == fournisseurNom.ToLower());

                    if (fournisseur != null && !fournisseur.Actif)
                    {
                        MessageBox.Show(
                            $"Le fournisseur « {fournisseur.Nom} » est archivé.\nRéactivez-le dans l'écran Fournisseurs avant de lui commander.",
                            "Fournisseur archivé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (fournisseur == null)
                    {
                        fournisseur = new Fournisseur { Nom = fournisseurNom, Contact = "" };
                        ctx.fournisseur.Add(fournisseur);
                        ctx.SaveChanges();
                    }

                    // Charger le produit
                    var produit = ctx.produits.Find(_produitId);
                    if (produit == null)
                    {
                        MessageBox.Show("Produit introuvable.", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Associer le fournisseur au produit si pas encore fait
                    if (produit.FournisseurId == null)
                    {
                        produit.FournisseurId = fournisseur.Id;
                    }

                    // Créer la commande
                    var commande = new Commande
                    {
                        FournisseurId = fournisseur.Id,
                        DateCommande = DateTime.Now,
                        Statut = "En attente",
                        NoteCommande = txtNote.Text.Trim(),
                        DateLivraisonPrevue = chkDateLivraison.Checked
                            ? dtpLivraisonPrevue.Value.Date
                            : (DateTime?)null
                    };

                    var ligne = new LigneCommande
                    {
                        ProduitId = produit.Id,
                        Quantite = qte,
                        PrixAchatUnitaire = prixU > 0 ? prixU : produit.PrixAchat
                    };

                    commande.Lignes.Add(ligne);
                    ctx.commandes.Add(commande);
                    ctx.SaveChanges();
                }

                BandeauNotification.Succes($"Commande enregistrée : {qte} boîte(s) de {_nomProduit} chez {fournisseurNom}.");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Création de commande", ex);
                MessageBox.Show("La commande n'a pas pu être enregistrée : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => Close();
    }
}