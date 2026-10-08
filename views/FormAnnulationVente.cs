using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Annule une vente : remet le stock, marque la vente "Annulée".
    /// Ne supprime rien de la base — tout est conservé pour l'historique.
    /// CORRECTION : stock géré en unités de base (pas en boîtes via ceil)
    /// </summary>
    public partial class FormAnnulationVente : Form
    {
        private readonly int _venteId;

        public FormAnnulationVente(int venteId)
        {
            InitializeComponent();
            _venteId = venteId;
            ChargerInfo();
        }

        private void ChargerInfo()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                    .Include(v => v.Paiements)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                lblInfo.Text =
                    $"Vente : {vente.numeroVente}\n" +
                    $"Client : {vente.PrenomClient} {vente.NomClient}\n" +
                    $"Total : {vente.MontantTotal:N0} KMF\n" +
                    $"Déjà versé : {vente.MontantVerse:N0} KMF";

                lvLignes.Items.Clear();
                foreach (var l in vente.Lignes)
                {
                    var item = new ListViewItem(l.Produit?.Nom ?? "—");
                    item.SubItems.Add(l.UniteVendue);
                    item.SubItems.Add(l.Quantite.ToString());
                    lvLignes.Items.Add(item);
                }
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => this.Close();

        private void btnConfirmer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotif.Text))
            {
                MessageBox.Show("Le motif d'annulation est obligatoire.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotif.Focus();
                return;
            }

            var confirm = MessageBox.Show(
                "Confirmer l'annulation de cette vente ?\nLe stock sera remis à jour.",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var ctx = new AppDbContext())
                using (var tx = ctx.Database.BeginTransaction())
                {
                    var vente = ctx.ventes
                        .Include(v => v.Lignes)
                        .Include(v => v.Paiements)
                        .FirstOrDefault(v => v.IdVente == _venteId);

                    if (vente == null) return;

                    if (vente.Statut == "Annulée")
                    {
                        MessageBox.Show("Cette vente est déjà annulée.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 1. Marquer la vente comme annulée
                    vente.Statut = "Annulée";
                    vente.DateAnnulation = DateTime.Now;
                    vente.MotifAnnulation = txtMotif.Text.Trim();

                    // 2. Remettre le stock : on restitue exactement les unités retirées à la vente
                    foreach (var ligne in vente.Lignes)
                    {
                        var produit = ctx.produits.Find(ligne.ProduitId);
                        if (produit == null) continue;

                        StockService.Ajouter(produit, ligne.QuantiteUnites);
                    }

                    ctx.SaveChanges();
                    tx.Commit();
                }

                MessageBox.Show(
                    "✅ Vente annulée avec succès.\nLe stock a été remis à jour.",
                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Annulation de la vente " + _venteId, ex);
                MessageBox.Show("Erreur : " + ex.Message + "\n\nLa vente n'a pas été annulée.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}