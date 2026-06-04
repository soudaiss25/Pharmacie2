using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class FormPaiements : Form
    {
        private readonly int _venteId;

        public FormPaiements(int venteId)
        {
            InitializeComponent();
            _venteId = venteId;
            ChargerResume();
            ChargerPaiements();
        }

        private void ChargerResume()
        {
            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Paiements)
                    .Include(v => v.User)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                lblNumero.Text = $"Vente : {vente.numeroVente}";
                lblTotal.Text = $"Montant total : {vente.MontantTotal:0.00} KMF";
                lblVerse.Text = $"Déjà versé : {vente.MontantVerse:0.00} KMF";
                lblRestant.Text = $"Reste à payer : {vente.MontantRestant:0.00} KMF";
                lblRestant.ForeColor = vente.MontantRestant > 0
                    ? System.Drawing.Color.OrangeRed
                    : System.Drawing.Color.ForestGreen;

                // Afficher vendeur + motif si renseigné
                string vendeur = vente.User != null
                    ? $"{vente.User.Prenom} {vente.User.Nom}"
                    : "Inconnu";
                lblVendeurVente.Text = $"Vendeur : {vendeur}";

                lblMotif.Text = !string.IsNullOrWhiteSpace(vente.MotifAchat)
                    ? $"Motif : {vente.MotifAchat}"
                    : "Motif : —";

                txtMontant.Text = vente.MontantRestant.ToString("0.00");
            }
        }

        private void ChargerPaiements()
        {
            using (var ctx = new AppDbContext())
            {
                var paiements = ctx.paiement
                    .Include(p => p.User)
                    .Where(p => p.VenteId == _venteId)
                    .OrderBy(p => p.DatePaiement)
                    .ToList();

                lvPaiements.Items.Clear();
                foreach (var p in paiements)
                {
                    string caissier = p.User != null
                        ? $"{p.User.Prenom} {p.User.Nom}"
                        : "—";

                    var item = new ListViewItem(p.NumeroPaiement);
                    item.SubItems.Add(p.DatePaiement.ToString("dd/MM/yyyy HH:mm"));
                    item.SubItems.Add(p.Montant.ToString("0.00") + " KMF");
                    item.SubItems.Add(caissier);
                    lvPaiements.Items.Add(item);
                }
            }
        }

        private void btnAjouterPaiement_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMontant.Text, out decimal montant) || montant <= 0)
            {
                MessageBox.Show("Montant invalide.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var ctx = new AppDbContext())
            {
                var vente = ctx.ventes
                    .Include(v => v.Paiements)
                    .FirstOrDefault(v => v.IdVente == _venteId);

                if (vente == null) return;

                decimal restant = vente.MontantRestant;

                if (restant <= 0)
                {
                    MessageBox.Show("Cette vente est déjà entièrement payée.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (montant > restant)
                {
                    montant = restant;
                    MessageBox.Show($"Montant ajusté au restant : {restant:0.00} KMF",
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
            }

            ChargerResume();
            ChargerPaiements();
            MessageBox.Show("Paiement enregistré !", "Succès",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}