using Pharmacie2.Models;
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace Pharmacie2.views
{
    /// <summary>
    /// Formulaire de choix d'un produit lors d'une vente.
    /// POINT 7 — Gestion plaquette/boîte :
    ///   - cbUnite propose "Boîte" ET l'unité inférieure si NbUniteParBoite > 1
    ///   - numQuantite.Maximum est recalculé selon le stock ET l'unité choisie
    ///     Ex : stock = 2 boîtes, NbUniteParBoite = 8 plaquettes
    ///          → si "Boîte"     : max = 2
    ///          → si "Plaquette" : max = 16
    ///   - Affichage clair : stock dispo + prix unitaire + info boîtes consommées
    ///   - Produits en rupture grisés et non sélectionnables
    /// </summary>
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

        // ── Chargement de la liste des produits ───────────────────────────

        private void ChargerProduits(string filtre = "")
        {
            using (var ctx = new AppDbContext())
            {
                var query = ctx.produits.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtre))
                    query = query.Where(p => p.Nom.ToLower().Contains(filtre.ToLower()));

                // QuantiteEnStock est en UNITÉS DE BASE
                var data = query
                    .OrderBy(p => p.Nom)
                    .Select(p => new
                    {
                        p.Id,
                        Nom = p.Nom,
                        Type = p.Type,
                        PrixVente = p.PrixVente,
                        // Afficher le stock en boîtes pour la lisibilité
                        Stock = p.NbUniteParBoite > 1
                                     ? p.QuantiteEnStock / p.NbUniteParBoite
                                     : p.QuantiteEnStock,
                        StockUnites = p.QuantiteEnStock,       // stock réel en unités
                        UniteVente = p.UniteVente,
                        ParBoite = p.NbUniteParBoite,
                        Etat = p.QuantiteEnStock <= 0 ? "Rupture"
                                   : p.QuantiteEnStock <= p.SeuilAlerte ? "Alerte"
                                   : "OK"
                    }).ToList();

                dgvProduits.DataSource = data;

                if (dgvProduits.Columns["Id"] != null)
                    dgvProduits.Columns["Id"].Visible = false;
                if (dgvProduits.Columns["ParBoite"] != null)
                    dgvProduits.Columns["ParBoite"].Visible = false;
                if (dgvProduits.Columns["UniteVente"] != null)
                    dgvProduits.Columns["UniteVente"].Visible = false;
                if (dgvProduits.Columns["StockUnites"] != null)
                    dgvProduits.Columns["StockUnites"].Visible = false;

                // Colorer les lignes selon l'état stock
                foreach (DataGridViewRow row in dgvProduits.Rows)
                {
                    string etat = row.Cells["Etat"].Value?.ToString();
                    if (etat == "Rupture")
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                    }
                    else if (etat == "Alerte")
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 243, 205);
                    }
                }
            }
        }

        private void txtRecherche_TextChanged(object sender, EventArgs e)
            => ChargerProduits(txtRecherche.Text);

        // ── Sélection d'un produit → met à jour unités et max quantité ────

        private void dgvProduits_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0) return;

            var row = dgvProduits.SelectedRows[0];
            int nbParBoite = Convert.ToInt32(row.Cells["ParBoite"].Value);
            string unite = row.Cells["UniteVente"].Value?.ToString() ?? "Boîte";
            string etat = row.Cells["Etat"].Value?.ToString() ?? "OK";

            // Bloquer la sélection si rupture
            if (etat == "Rupture")
            {
                lblInfo.Text = "⛔ Produit en rupture de stock.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                cbUnite.Items.Clear();
                cbUnite.Items.Add("Boîte");
                cbUnite.SelectedIndex = 0;
                numQuantite.Maximum = 1;
                numQuantite.Value = 1;
                btnValider.Enabled = false;
                return;
            }

            btnValider.Enabled = true;

            // Remplir le combo unités
            cbUnite.Items.Clear();
            cbUnite.Items.Add("Boîte");
            if (nbParBoite > 1)
                cbUnite.Items.Add(unite);
            cbUnite.SelectedIndex = 0;

            MettreAJourPrixEtMax();
        }

        private void cbUnite_SelectedIndexChanged(object sender, EventArgs e)
            => MettreAJourPrixEtMax();

        // ── Recalcul prix + maximum quantité selon unité choisie ─────────

        private void MettreAJourPrixEtMax()
        {
            if (dgvProduits.SelectedRows.Count == 0) return;

            var row = dgvProduits.SelectedRows[0];
            int produitId = Convert.ToInt32(row.Cells["Id"].Value);
            // StockUnites = stock réel en unités de base (plaquettes/comprimés/boîtes)
            int stockUnites = Convert.ToInt32(row.Cells["StockUnites"].Value);
            int nbParBoite = Convert.ToInt32(row.Cells["ParBoite"].Value);

            string uniteChoisie = cbUnite.SelectedItem?.ToString() ?? "Boîte";

            using (var ctx = new AppDbContext())
            {
                var p = ctx.produits.Find(produitId);
                if (p == null) return;

                decimal prix;
                int maxQte;

                if (uniteChoisie == "Boîte" || nbParBoite <= 1)
                {
                    // Vente à la boîte entière
                    prix = p.PrixVente;
                    // Stock en boîtes = stockUnites / nbParBoite (si nbParBoite > 1)
                    maxQte = nbParBoite > 1
                             ? stockUnites / nbParBoite
                             : stockUnites;

                    int nbBoites = maxQte;
                    lblPrixUnit.Text = $"💰 Prix : {prix:N0} KMF / boîte";
                    lblInfo.Text = $"📦 Stock disponible : {nbBoites} boîte(s)";
                    lblInfo.ForeColor = stockUnites <= p.SeuilAlerte
                        ? System.Drawing.Color.OrangeRed
                        : System.Drawing.Color.FromArgb(27, 94, 32);
                }
                else
                {
                    // Vente à l'unité (plaquette, comprimé…)
                    prix = p.PrixUnitaireVente;
                    // Max = stock réel en unités de base (ex : 10 plaquettes)
                    maxQte = stockUnites;

                    int boitesDispo = stockUnites / nbParBoite;
                    int unitesRestantes = stockUnites % nbParBoite;

                    lblPrixUnit.Text = $"💰 Prix : {prix:N0} KMF / {uniteChoisie}";
                    lblInfo.Text =
                        $"📦 Stock : {boitesDispo} boîte(s)" +
                        (unitesRestantes > 0 ? $" + {unitesRestantes} {uniteChoisie}(s)" : "") +
                        $" = {stockUnites} {uniteChoisie}(s) disponibles\n" +
                        $"ℹ️ {nbParBoite} {uniteChoisie}(s) par boîte";
                    lblInfo.ForeColor = System.Drawing.Color.FromArgb(25, 118, 210);
                }

                // Mettre à jour le Maximum AVANT de changer la Value
                if (maxQte < 1) maxQte = 1;
                numQuantite.Maximum = maxQte;

                // Remettre la valeur dans les bornes
                if (numQuantite.Value > maxQte) numQuantite.Value = maxQte;
                if (numQuantite.Value < 1) numQuantite.Value = 1;
            }
        }

        // ── Mise à jour info boîtes consommées quand la quantité change ───

        private void numQuantite_ValueChanged(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0) return;

            var row = dgvProduits.SelectedRows[0];
            int nbParBoite = Convert.ToInt32(row.Cells["ParBoite"].Value);
            int stockUnites = Convert.ToInt32(row.Cells["StockUnites"].Value);
            string unite = cbUnite.SelectedItem?.ToString() ?? "Boîte";

            if (unite != "Boîte" && nbParBoite > 1)
            {
                int qte = (int)numQuantite.Value;
                // Boîtes entières consommées = quantité vendue / nbParBoite (division entière)
                // Ex : vendre 3 plaquettes sur 5 par boîte → 0 boîte entière consommée
                // Ex : vendre 6 plaquettes sur 5 par boîte → 1 boîte entière consommée, 1 restante
                int boitesEntames = qte / nbParBoite;
                int unitesRestantes = qte % nbParBoite;

                string detail = boitesEntames > 0
                    ? $"{boitesEntames} boîte(s) entière(s)" +
                      (unitesRestantes > 0 ? $" + {unitesRestantes} {unite}(s)" : "")
                    : $"{unitesRestantes} {unite}(s) sur une boîte entamée";

                int stockBoites = stockUnites / nbParBoite;
                int stockRestes = stockUnites % nbParBoite;

                lblInfo.Text =
                    $"📦 Stock : {stockBoites} boîte(s)" +
                    (stockRestes > 0 ? $" + {stockRestes} {unite}(s)" : "") +
                    $" = {stockUnites} {unite}(s) disponibles\n" +
                    $"🔢 {qte} {unite}(s) vendues → {detail} déduites du stock";
            }
        }

        // ── Validation ────────────────────────────────────────────────────

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez choisir un produit.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvProduits.SelectedRows[0];
            string etat = row.Cells["Etat"].Value?.ToString() ?? "OK";

            if (etat == "Rupture")
            {
                MessageBox.Show("Ce produit est en rupture de stock.", "Stock insuffisant",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int produitId = Convert.ToInt32(row.Cells["Id"].Value);

            using (var ctx = new AppDbContext())
                ProduitSelectionne = ctx.produits.Find(produitId);

            QuantiteSelectionnee = (int)numQuantite.Value;
            UniteSelectionnee = cbUnite.SelectedItem?.ToString() ?? "Boîte";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}