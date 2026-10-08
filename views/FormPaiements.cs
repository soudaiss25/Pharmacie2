using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>Encaissement d'un paiement sur une vente (crédit ou reste à payer) et historique des paiements.</summary>
    public partial class FormPaiements : Form
    {
        private readonly int _venteId;

        public FormPaiements(int venteId)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _venteId = venteId;
            ChargerResume();
            ChargerPaiements();
        }

        private void ChargerResume()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .AsNoTracking()
                    .Include(v => v.Paiements)
                    .Include(v => v.User)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                lblNumero.Text = $"Vente {vente.numeroVente}";
                lblTotal.Text = $"Total : {Format.Montant(vente.MontantTotal)}";
                lblVerse.Text = $"Déjà versé : {Format.Montant(vente.MontantVerse)}";

                decimal restantPatient = vente.RestantPatient;
                lblRestant.Text = $"Reste à payer par le patient : {Format.Montant(restantPatient)}";
                lblRestant.ForeColor = restantPatient > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;

                string vendeur = vente.User != null ? $"{vente.User.Prenom} {vente.User.Nom}" : "inconnu";
                string motif = !string.IsNullOrWhiteSpace(vente.MotifAchat) ? vente.MotifAchat : "—";
                lblVendeurVente.Text = $"Vendeur : {vendeur}";
                lblMotif.Text = $"Motif : {motif}";

                // Maximum avant Value : on ne peut pas encaisser plus que ce que le patient doit
                numMontant.Maximum = Math.Max(0m, Math.Ceiling(restantPatient));
                numMontant.Value = numMontant.Maximum;
                numMontant.Enabled = btnAjouterPaiement.Enabled = restantPatient > 0 && vente.Statut != "Annulée";

                if (vente.Statut == "Annulée")
                    lblRetour.Text = "Cette vente est annulée : aucun paiement possible.";
                else if (restantPatient <= 0 && vente.MontantMutuelle > 0 && !vente.MutuelleReglee)
                    lblRetour.Text = $"Le patient a tout payé. La part de la mutuelle ({Format.Montant(vente.MontantMutuelle)}) se règle depuis l'écran Mutuelles.";
                else if (restantPatient <= 0)
                    lblRetour.Text = "Cette vente est entièrement payée.";
            }
        }

        private void ChargerPaiements()
        {
            using (var ctx = new AppDbContext())
            {
                var paiements = ctx.paiement
                    .AsNoTracking()
                    .Include(p => p.User)
                    .Where(p => p.VenteId == _venteId)
                    .OrderBy(p => p.DatePaiement)
                    .ToList();

                dgvPaiements.DataSource = paiements.Select(p => new
                {
                    Numero = p.NumeroPaiement,
                    Date = p.DatePaiement.ToString("dd/MM/yyyy HH:mm"),
                    p.Montant,
                    Caissier = p.User != null ? $"{p.User.Prenom} {p.User.Nom}" : "—"
                }).ToList();
            }
        }

        private void btnAjouterPaiement_Click(object sender, EventArgs e)
        {
            decimal montant = numMontant.Value;
            if (montant <= 0)
            {
                MessageBox.Show("Saisissez un montant supérieur à 0.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMontant.Focus();
                return;
            }

            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Paiements)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                if (vente.Statut == "Annulée")
                {
                    MessageBox.Show("Impossible d'encaisser une vente annulée.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                decimal restant = vente.RestantPatient;

                if (restant <= 0)
                {
                    MessageBox.Show("Cette vente est déjà entièrement payée par le patient.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (montant > restant)
                {
                    montant = restant;   // jamais plus que ce que le patient doit
                    MessageBox.Show($"Montant ramené au reste à payer : {Format.Montant(restant)}",
                        "Ajustement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ctx.paiement.Add(new Paiement
                {
                    NumeroPaiement = "P-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                    VenteId = _venteId,
                    Montant = montant,
                    DatePaiement = DateTime.Now,
                    UserId = SessionUtilisateur.IdCourant  // ← qui encaisse
                });
                ctx.SaveChanges();

                lblRetour.Text = $"Paiement de {Format.Montant(montant)} enregistré.";
            }

            VenteEvenements.Notifier(this);
            ChargerResume();
            ChargerPaiements();
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
