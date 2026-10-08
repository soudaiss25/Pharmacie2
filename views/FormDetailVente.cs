using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>Affiche tous les détails d'une vente (produits, paiements, infos).</summary>
    public partial class FormDetailVente : Form
    {
        private readonly int _venteId;

        public FormDetailVente(int venteId)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _venteId = venteId;
            ChargerDetail();
        }

        private void ChargerDetail()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .AsNoTracking()
                    .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                    .Include(v => v.Paiements).ThenInclude(p => p.User)
                    .Include(v => v.User)
                    .Include(v => v.Mutuel)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                // ── En-tête ───────────────────────────────────────────────
                lblNumero.Text = "Vente " + vente.numeroVente;
                lblDate.Text = vente.DateVente.ToString("dd/MM/yyyy HH:mm");
                lblClient.Text = $"{vente.PrenomClient} {vente.NomClient}".Trim() is { Length: > 0 } c ? c : "—";
                lblTel.Text = string.IsNullOrWhiteSpace(vente.TelephoneClient) ? "—" : vente.TelephoneClient;
                lblMotif.Text = "Motif : " + (!string.IsNullOrWhiteSpace(vente.MotifAchat) ? vente.MotifAchat : "—");
                lblVendeur.Text = vente.User != null ? $"{vente.User.Prenom} {vente.User.Nom}" : "—";
                lblMode.Text = vente.MoyenPaiement;
                lblStatut.Text = vente.Statut;
                lblStatut.ForeColor = vente.Statut == "Annulée" ? Theme.UrgentTexte : Theme.SuccesTexte;

                // ── Mutuelle — affiche statut de règlement ────────────────
                if (vente.Mutuel != null)
                {
                    string statutMutuelle = vente.MutuelleReglee ? "réglée" : "en attente de règlement";

                    lblMutuelle.Text =
                        $"Mutuelle {vente.Mutuel.NomEmployeur} ({vente.TauxMutuelle:0.##} %) — " +
                        $"part de l'entreprise : {Format.Montant(vente.MontantMutuelle)} — " +
                        $"matricule : {(string.IsNullOrWhiteSpace(vente.MatriculeEmploye) ? "—" : vente.MatriculeEmploye)} — " +
                        $"{statutMutuelle}";

                    lblMutuelle.ForeColor = vente.MutuelleReglee ? Theme.SuccesTexte : Theme.AttentionTexte;
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
                    Unite = l.UniteVendue,
                    Quantite = l.Quantite,
                    PrixU = l.PrixUnitaire,
                    SousTotal = l.SousTotal
                }).ToList();

                // ── Totaux ────────────────────────────────────────────────
                lblTotal.Text = "Total : " + Format.Montant(vente.MontantTotal);
                lblVerse.Text = "Versé par le patient : " + Format.Montant(vente.MontantVerse);
                lblRestant.Text = "Reste à récupérer : " + Format.Montant(vente.MontantRestant);
                lblRestant.ForeColor = vente.MontantRestant > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;

                // ── Historique paiements patient ──────────────────────────
                dgvPaiements.DataSource = vente.Paiements.OrderBy(p => p.DatePaiement).Select(p => new
                {
                    Numero = p.NumeroPaiement,
                    Date = p.DatePaiement.ToString("dd/MM/yyyy HH:mm"),
                    p.Montant,
                    Caissier = p.User != null ? $"{p.User.Prenom} {p.User.Nom}" : "—"
                }).ToList();
            }
        }

        private void btnFermer_Click(object sender, EventArgs e) => this.Close();
    }
}
