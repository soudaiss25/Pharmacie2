using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Uc_Fournisser — POINT 6
    /// Partie haute  : liste des fournisseurs (search, add, edit, delete, voir commandes)
    /// Partie basse  : produits associés au fournisseur sélectionné
    ///                 → bouton "Commander ce produit" directement depuis la fiche
    ///                 → informations optionnelles complétées à la réception
    /// </summary>
    public partial class Uc_Fournisser : UserControl
    {
        private readonly AppDbContext _context;
        private int _fournisseurSelectionneId = -1;

        public Uc_Fournisser()
        {
            InitializeComponent();
            _context = new AppDbContext();
            dgvFournisseurs.AutoGenerateColumns = false;

            // Lier les colonnes aux proprietes du modele Fournisseur
            if (dgvFournisseurs.Columns["colNom"] != null) dgvFournisseurs.Columns["colNom"].DataPropertyName = "Nom";
            if (dgvFournisseurs.Columns["colContact"] != null) dgvFournisseurs.Columns["colContact"].DataPropertyName = "Contact";
            if (dgvFournisseurs.Columns["colTelephone"] != null) dgvFournisseurs.Columns["colTelephone"].DataPropertyName = "Telephone";
            if (dgvFournisseurs.Columns["colAdresse"] != null) dgvFournisseurs.Columns["colAdresse"].DataPropertyName = "Adresse";

            // SplitterDistance au Load
            this.Load += (s, e) =>
            {
                try
                {
                    int dist = (int)(splitMain.Height * 0.50);
                    if (dist > splitMain.Panel1MinSize &&
                        dist < splitMain.Height - splitMain.Panel2MinSize)
                        splitMain.SplitterDistance = dist;
                }
                catch { }
            };

            ChargerFournisseurs();

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            dgvFournisseurs.SelectionChanged += dgvFournisseurs_SelectionChanged;
            dgvFournisseurs.CellContentClick += dgvFournisseurs_CellContentClick;
            btnCommanderProduit.Click += btnCommanderProduit_Click;
            btnVoirCommandes.Click += btnVoirCommandes_Click;
        }

        // ── Chargement fournisseurs ───────────────────────────────────────

        private void ChargerFournisseurs(string search = null)
        {
            var query = _context.fournisseur.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(f =>
                    (f.Nom != null && f.Nom.ToLower().Contains(search)) ||
                    (f.Contact != null && f.Contact.ToLower().Contains(search)));
            }

            dgvFournisseurs.DataSource = null;
            dgvFournisseurs.DataSource = query.OrderBy(f => f.Nom).ToList();

            // Vider le panneau produits quand la liste change
            dgvProduits.Rows.Clear();
            lblProduitsTitre.Text = "← Sélectionnez un fournisseur pour voir ses produits";
            _fournisseurSelectionneId = -1;
            btnCommanderProduit.Enabled = false;
            btnVoirCommandes.Enabled = false;
        }

        private Fournisseur GetSelectedFournisseur()
        {
            if (dgvFournisseurs.CurrentRow == null) return null;
            return dgvFournisseurs.CurrentRow.DataBoundItem as Fournisseur;
        }

        // ── Sélection d'un fournisseur → charge ses produits ─────────────

        private void dgvFournisseurs_SelectionChanged(object sender, EventArgs e)
        {
            var f = GetSelectedFournisseur();
            if (f == null) return;

            _fournisseurSelectionneId = f.Id;
            ChargerProduitsFournisseur(f.Id, f.Nom);
            btnVoirCommandes.Enabled = true;
        }

        /// <summary>Charge dans dgvProduits tous les produits liés à ce fournisseur.</summary>
        private void ChargerProduitsFournisseur(int fournisseurId, string nomFournisseur)
        {
            using (var ctx = new AppDbContext())
            {
                var produits = ctx.produits
                    .Where(p => p.FournisseurId == fournisseurId)
                    .OrderBy(p => p.Nom)
                    .ToList();

                dgvProduits.Rows.Clear();

                foreach (var p in produits)
                {
                    string etat = p.QuantiteEnStock <= 0 ? "🔴 Rupture"
                                : p.QuantiteEnStock <= p.SeuilAlerte ? "🟠 Alerte"
                                : "🟢 OK";

                    int idx = dgvProduits.Rows.Add(
                        p.Id,
                        p.Nom,
                        p.Type,
                        p.QuantiteEnStock,
                        p.SeuilAlerte,
                        $"{p.PrixAchat:N0} KMF",
                        $"{p.PrixVente:N0} KMF",
                        etat
                    );

                    // Coloration selon état stock
                    if (p.QuantiteEnStock <= 0)
                        dgvProduits.Rows[idx].DefaultCellStyle.BackColor =
                            System.Drawing.Color.FromArgb(255, 205, 210);
                    else if (p.QuantiteEnStock <= p.SeuilAlerte)
                        dgvProduits.Rows[idx].DefaultCellStyle.BackColor =
                            System.Drawing.Color.FromArgb(255, 224, 178);
                }

                int nbProduits = produits.Count;
                int nbAlerte = produits.Count(p => p.QuantiteEnStock <= p.SeuilAlerte && p.QuantiteEnStock > 0);
                int nbRupture = produits.Count(p => p.QuantiteEnStock <= 0);

                lblProduitsTitre.Text =
                    $"📦 Produits de « {nomFournisseur} » — " +
                    $"{nbProduits} produit(s)  |  " +
                    $"🟠 {nbAlerte} en alerte  |  " +
                    $"🔴 {nbRupture} en rupture";

                btnCommanderProduit.Enabled = dgvProduits.Rows.Count > 0;
            }
        }

        // ── Commander un produit depuis la fiche fournisseur ─────────────

        private void btnCommanderProduit_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Sélectionnez un produit dans la liste pour le commander.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int produitId = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["colProduitId"].Value);
            string nomProduit = dgvProduits.SelectedRows[0].Cells["colProduitNom"].Value?.ToString() ?? "";

            using (var form = new FormCommandeProduit(produitId, nomProduit))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Rafraîchir la liste des produits après commande
                    var f = GetSelectedFournisseur();
                    if (f != null)
                        ChargerProduitsFournisseur(f.Id, f.Nom);
                }
            }
        }

        // ── Voir commandes du fournisseur ─────────────────────────────────

        private void btnVoirCommandes_Click(object sender, EventArgs e)
        {
            var f = GetSelectedFournisseur();
            if (f == null) return;

            using (var frm = new FormCommandesFournisseur(f.Id, f.Nom))
                frm.ShowDialog();
        }

        // ── Bouton Voir dans la colonne de la grille fournisseurs ─────────

        private void dgvFournisseurs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvFournisseurs.Columns[e.ColumnIndex].Name == "btnViewCommandes")
            {
                var fournisseur = dgvFournisseurs.Rows[e.RowIndex].DataBoundItem as Fournisseur;
                if (fournisseur == null) return;

                using (var frm = new FormCommandesFournisseur(fournisseur.Id, fournisseur.Nom))
                    frm.ShowDialog();
            }
        }

        // ── CRUD fournisseurs ─────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new FormaddFournisseur())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    ChargerFournisseurs(txtSearch.Text);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedFournisseur();
            if (selected == null)
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var fournisseurDb = _context.fournisseur.Find(selected.Id);
            if (fournisseurDb == null)
            {
                MessageBox.Show("Fournisseur introuvable.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChargerFournisseurs(txtSearch.Text);
                return;
            }

            using (var frm = new FormaddFournisseur(fournisseurDb))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    ChargerFournisseurs(txtSearch.Text);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedFournisseur();
            if (selected == null)
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Supprimer le fournisseur « {selected.Nom} » ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var fournisseurDb = _context.fournisseur.Find(selected.Id);
            if (fournisseurDb == null)
            {
                ChargerFournisseurs(txtSearch.Text);
                return;
            }

            _context.fournisseur.Remove(fournisseurDb);
            _context.SaveChanges();
            ChargerFournisseurs(txtSearch.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ChargerFournisseurs();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
            => ChargerFournisseurs(txtSearch.Text);
    }
}