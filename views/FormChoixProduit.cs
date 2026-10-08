using Pharmacie2.Models;
using Pharmacie2.Services;
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace Pharmacie2.views
{
    public partial class FormChoixProduit : Form
    {
        public Produit ProduitSelectionne { get; private set; }
        public int QuantiteSelectionnee { get; private set; }
        public string UniteSelectionnee { get; private set; } = "Boîte";

        public FormChoixProduit()
        {
            InitializeComponent();
            ChargerProduits();
        }

        private void ChargerProduits(string filtre = "")
        {
            using (var ctx = new AppDbContext())
            {
                var query = ctx.produits.Include(p => p.Fournisseur).Where(p => p.Actif).AsQueryable();
                if (!string.IsNullOrWhiteSpace(filtre))
                    query = query.Where(p => p.Nom.ToLower().Contains(filtre.ToLower()));

                dgvProduits.DataSource = query.ToList().Select(p => new
                {
                    p.Id,
                    p.Nom,
                    p.Type,
                    p.PrixVente,
                    Stock = StockService.Formater(p),
                    UniteVente = p.UniteVente,
                    ParBoite = p.NbUniteParBoite
                }).ToList();

                if (dgvProduits.Columns["Id"] != null)
                    dgvProduits.Columns["Id"].Visible = false;
            }
        }

        private void txtRecherche_TextChanged(object sender, EventArgs e)
            => ChargerProduits(txtRecherche.Text);

        private void dgvProduits_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0) return;
            var row = dgvProduits.SelectedRows[0];

            int nbParBoite = Convert.ToInt32(row.Cells["ParBoite"].Value);
            string unite = row.Cells["UniteVente"].Value?.ToString() ?? "Boîte";

            // Proposer boîte ET unité si applicable
            cbUnite.Items.Clear();
            cbUnite.Items.Add("Boîte");
            if (nbParBoite > 1)
                cbUnite.Items.Add(unite);
            cbUnite.SelectedIndex = 0;

            MettreAJourPrix();
            AfficherPosologie();
        }

        private void AfficherPosologie()
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                pnlPosologie.Visible = false;
                return;
            }

            int produitId = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            using (var ctx = new AppDbContext())
            {
                var p = ctx.produits.Find(produitId);
                if (p == null) { pnlPosologie.Visible = false; return; }

                bool hasInfo = !string.IsNullOrWhiteSpace(p.Indication)
                            || !string.IsNullOrWhiteSpace(p.Posologie)
                            || p.NbFoisParJour > 0;

                pnlPosologie.Visible = hasInfo;

                lblIndicationVal.Text = !string.IsNullOrWhiteSpace(p.Indication)
                    ? $"📌 Indication : {p.Indication}"
                    : "";

                lblPosologieVal.Text = !string.IsNullOrWhiteSpace(p.Posologie)
                    ? $"💊 Posologie : {p.Posologie}"
                    : "";

                lblNbFoisVal.Text = p.NbFoisParJour > 0
                    ? $"🕐 {p.NbFoisParJour} prise(s) par jour"
                    : "";
            }
        }

        private void cbUnite_SelectedIndexChanged(object sender, EventArgs e)
            => MettreAJourPrix();

        private void MettreAJourPrix()
        {
            if (dgvProduits.SelectedRows.Count == 0) return;
            var row = dgvProduits.SelectedRows[0];

            int produitId = Convert.ToInt32(row.Cells["Id"].Value);
            using (var ctx = new AppDbContext())
            {
                var p = ctx.produits.Find(produitId);
                if (p == null) return;

                decimal prix = cbUnite.SelectedItem?.ToString() == "Boîte"
                    ? p.PrixVente
                    : p.PrixUnitaireVente;

                lblPrixUnit.Text = $"Prix unitaire : {prix:0.00} KMF";
            }
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez choisir un produit.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int produitId = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);

            using (var ctx = new AppDbContext())
            {
                ProduitSelectionne = ctx.produits.Find(produitId);
            }

            QuantiteSelectionnee = (int)numQuantite.Value;
            UniteSelectionnee = cbUnite.SelectedItem?.ToString() ?? "Boîte";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}