using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Fournisseurs. En haut : la liste (recherche, ajout, modification, archivage, historique des commandes).
    /// En bas : les produits du fournisseur sélectionné, avec « Commander ce produit ».
    /// </summary>
    public partial class Uc_Fournisser : UserControl
    {
        private readonly CheckBox _chkArchives;

        public Uc_Fournisser()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            // Archivage (remplace la suppression)
            _chkArchives = ArchivageUi.Installer(btnDelete, TypeElement.Fournisseur, SelectionFournisseur, () => ChargerFournisseurs(txtSearch.Text));

            ChargerFournisseurs();
        }

        // ── Chargement fournisseurs ───────────────────────────────────────

        private void ChargerFournisseurs(string? search = null)
        {
            if (_chkArchives == null) return;
            bool archives = _chkArchives.Checked;

            List<Fournisseur> liste;
            using (var ctx = new AppDbContext())
            {
                var query = ctx.fournisseur.AsNoTracking().Where(f => f.Actif || archives);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(f =>
                        (f.Nom != null && f.Nom.ToLower().Contains(search)) ||
                        (f.Contact != null && f.Contact.ToLower().Contains(search)));
                }

                liste = query.OrderBy(f => f.Nom).ToList();   // lu avant la fermeture du contexte
            }

            dgvFournisseurs.DataSource = null;
            dgvFournisseurs.DataSource = liste;

            // Vider le panneau produits quand la liste change
            dgvProduits.DataSource = null;
            lblProduitsTitre.Text = "Sélectionnez un fournisseur pour voir ses produits";
            btnCommanderProduit.Enabled = false;
            btnVoirCommandes.Enabled = false;
        }

        private Fournisseur? GetSelectedFournisseur()
        {
            if (dgvFournisseurs.CurrentRow == null) return null;
            return dgvFournisseurs.CurrentRow.DataBoundItem as Fournisseur;
        }

        private void dgvFournisseurs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvFournisseurs.Rows.Count
                && dgvFournisseurs.Rows[e.RowIndex].DataBoundItem is Fournisseur f && !f.Actif)
            {
                e.CellStyle.BackColor = Theme.InfoFond;
                e.CellStyle.ForeColor = Theme.Neutre;
                if (dgvFournisseurs.Columns[e.ColumnIndex].Name == "Nom")
                    e.Value = f.Nom + " (archivé)";
            }
        }

        // ── Sélection d'un fournisseur → charge ses produits ─────────────

        private void dgvFournisseurs_SelectionChanged(object sender, EventArgs e)
        {
            var f = GetSelectedFournisseur();
            if (f == null) return;

            ChargerProduitsFournisseur(f.Id, f.Nom);
            btnVoirCommandes.Enabled = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => ChargerFournisseurs(txtSearch.Text);

        /// <summary>Charge dans dgvProduits tous les produits actifs liés à ce fournisseur.</summary>
        private void ChargerProduitsFournisseur(int fournisseurId, string nomFournisseur)
        {
            using (var ctx = new AppDbContext())
            {
                var produits = ctx.produits
                    .AsNoTracking()
                    .Where(p => p.FournisseurId == fournisseurId && p.Actif)
                    .OrderBy(p => p.Nom)
                    .ToList();

                dgvProduits.DataSource = produits.Select(p => new
                {
                    p.Id,
                    p.Nom,
                    p.Type,
                    Stock = StockService.Formater(p),
                    Seuil = $"{p.SeuilAlerte} boîte(s)",
                    p.PrixAchat,
                    p.PrixVente,
                    Etat = p.QuantiteEnStock <= 0 ? "Rupture"
                         : p.QuantiteEnStock <= StockService.SeuilEnUnites(p) ? "Alerte"
                         : "OK"
                }).ToList();

                int nbAlerte = produits.Count(p => p.QuantiteEnStock <= StockService.SeuilEnUnites(p) && p.QuantiteEnStock > 0);
                int nbRupture = produits.Count(p => p.QuantiteEnStock <= 0);

                lblProduitsTitre.Text =
                    $"Produits de « {nomFournisseur} » — {produits.Count} produit(s), {nbAlerte} en alerte, {nbRupture} en rupture";

                btnCommanderProduit.Enabled = produits.Count > 0;
            }
        }

        private void dgvProduits_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProduits.Rows.Count) return;
            string etat = dgvProduits.Rows[e.RowIndex].Cells["Etat"].Value?.ToString() ?? "";
            if (etat == "Rupture")
            {
                e.CellStyle.BackColor = Theme.UrgentFond;
                e.CellStyle.ForeColor = Theme.UrgentTexte;
            }
            else if (etat == "Alerte")
            {
                e.CellStyle.BackColor = Theme.AttentionFond;
                e.CellStyle.ForeColor = Theme.AttentionTexte;
            }
        }

        // ── Commander un produit depuis la fiche fournisseur ─────────────

        private void btnCommanderProduit_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit dans la liste pour le commander.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int produitId = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            string nomProduit = dgvProduits.SelectedRows[0].Cells["Nom"].Value?.ToString() ?? "";

            using (var form = new FormCommandeProduit(produitId, nomProduit))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    var f = GetSelectedFournisseur();
                    if (f != null)
                        ChargerProduitsFournisseur(f.Id, f.Nom);
                }
            }
        }

        private void btnVoirCommandes_Click(object sender, EventArgs e)
        {
            var f = GetSelectedFournisseur();
            if (f == null) return;

            using (var frm = new FormCommandesFournisseur(f.Id, f.Nom))
                frm.ShowDialog(this);
        }

        // ── CRUD fournisseurs ─────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new FormaddFournisseur())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    ChargerFournisseurs(txtSearch.Text);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedFournisseur();
            if (selected == null)
            {
                MessageBox.Show("Sélectionnez un fournisseur.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Fournisseur? fournisseurDb;
            using (var ctxEdit = new AppDbContext())
                fournisseurDb = ctxEdit.fournisseur.AsNoTracking().FirstOrDefault(x => x.Id == selected.Id);
            if (fournisseurDb == null)
            {
                MessageBox.Show("Fournisseur introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChargerFournisseurs(txtSearch.Text);
                return;
            }

            using (var frm = new FormaddFournisseur(fournisseurDb))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    ChargerFournisseurs(txtSearch.Text);
            }
        }

        private (int id, string nom, bool actif)? SelectionFournisseur()
        {
            var f = GetSelectedFournisseur();
            if (f == null) return null;
            return (f.Id, f.Nom, ArchivageService.EstActif(TypeElement.Fournisseur, f.Id));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ArchivageUi.Archiver(TypeElement.Fournisseur, SelectionFournisseur()))
                ChargerFournisseurs(txtSearch.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";   // relance la recherche via TextChanged
        }
    }
}
