using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    /// <summary>Affiche tous les détails d'une vente (produits, paiements, infos).</summary>
    public partial class FormDetailVente : Form
    {
        private readonly int _venteId;

        public FormDetailVente(int venteId)
        {
            InitializeComponent();
            _venteId = venteId;
            ChargerDetail();
        }

        private void ChargerDetail()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                    .Include(v => v.Paiements).ThenInclude(p => p.User)
                    .Include(v => v.User)
                    .Include(v => v.Mutuel)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                // ── En-tête ───────────────────────────────────────────────
                lblNumero.Text = vente.numeroVente;
                lblDate.Text = vente.DateVente.ToString("dd/MM/yyyy HH:mm");
                lblClient.Text = $"{vente.PrenomClient} {vente.NomClient}".Trim();
                lblTel.Text = vente.TelephoneClient ?? "—";
                lblMotif.Text = !string.IsNullOrWhiteSpace(vente.MotifAchat)
                                  ? vente.MotifAchat : "—";
                lblVendeur.Text = vente.User != null
                                  ? $"{vente.User.Prenom} {vente.User.Nom}" : "—";
                lblMode.Text = vente.MoyenPaiement;
                lblStatut.Text = vente.Statut;
                lblStatut.ForeColor = vente.Statut == "Annulée"
                    ? System.Drawing.Color.OrangeRed
                    : System.Drawing.Color.ForestGreen;

                // ── Mutuelle — affiche statut de règlement ────────────────
                if (vente.Mutuel != null)
                {
                    string statutMutuelle = vente.MutuelleReglee
                        ? "✅ Réglée"
                        : "⏳ En attente de règlement";

                    lblMutuelle.Text =
                        $"🏢 {vente.Mutuel.NomEmployeur} ({vente.TauxMutuelle}%)  |  " +
                        $"Part entreprise : {vente.MontantMutuelle:N0} KMF  |  " +
                        $"Matricule : {vente.MatriculeEmploye ?? "—"}  |  " +
                        $"Statut mutuelle : {statutMutuelle}";

                    lblMutuelle.ForeColor = vente.MutuelleReglee
                        ? System.Drawing.Color.ForestGreen
                        : System.Drawing.Color.OrangeRed;

                    lblMutuelle.Visible = true;
                }
                else
                {
                    lblMutuelle.Visible = false;
                }

                // ── Lignes produits ───────────────────────────────────────
                dgvLignes.DataSource = vente.Lignes.Select(l => new
                {
                    Produit = l.Produit?.Nom ?? "—",
                    Unité = l.UniteVendue,
                    Quantité = l.Quantite,
                    PrixU = l.PrixUnitaire.ToString("N0") + " KMF",
                    SousTotal = l.SousTotal.ToString("N0") + " KMF"
                }).ToList();

                // ── Totaux ────────────────────────────────────────────────
                lblTotal.Text = $"Total : {vente.MontantTotal:N0} KMF";
                lblVerse.Text = $"Versé (patient) : {vente.MontantVerse:N0} KMF";
                lblRestant.Text = $"Reste : {vente.MontantRestant:N0} KMF";
                lblRestant.ForeColor = vente.MontantRestant > 0
                    ? System.Drawing.Color.OrangeRed
                    : System.Drawing.Color.ForestGreen;

                // ── Historique paiements patient ──────────────────────────
                lvPaiements.Items.Clear();
                foreach (var p in vente.Paiements.OrderBy(p => p.DatePaiement))
                {
                    var item = new ListViewItem(p.NumeroPaiement);
                    item.SubItems.Add(p.DatePaiement.ToString("dd/MM/yyyy HH:mm"));
                    item.SubItems.Add(p.Montant.ToString("N0") + " KMF");
                    item.SubItems.Add(p.User != null
                        ? $"{p.User.Prenom} {p.User.Nom}" : "—");
                    lvPaiements.Items.Add(item);
                }
            }
        }

        private void btnFermer_Click(object sender, EventArgs e) => this.Close();
    }
}