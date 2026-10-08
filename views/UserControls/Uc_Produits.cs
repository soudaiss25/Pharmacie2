using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    /// <summary>Catalogue des produits : prix, marge, stock, fournisseur. L'état du stock détaillé est dans « Stock ».</summary>
    public partial class Uc_Produits : UserControl
    {
        private readonly CheckBox _chkArchives;

        public Uc_Produits()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            cmbTypeFiltre.SelectedIndex = 0;
            txtRecherche.TextChanged += (s, e) => ChargerProduits();
            cmbTypeFiltre.SelectedIndexChanged += (s, e) => ChargerProduits();
            btnEffacer.Click += (s, e) => { txtRecherche.Clear(); cmbTypeFiltre.SelectedIndex = 0; };

            // Archivage (remplace la suppression)
            _chkArchives = ArchivageUi.Installer(btnSupprimer, TypeElement.Produit, SelectionProduit, ChargerProduits);

            ChargerProduits();
        }

        private void ChargerProduits()
        {
            if (_chkArchives == null || cmbTypeFiltre.SelectedIndex < 0) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    bool archives = _chkArchives.Checked;
                    var query = ctx.produits
                        .AsNoTracking()
                        .Include(p => p.Fournisseur)
                        .Where(p => p.Actif || archives)
                        .AsQueryable();

                    string search = txtRecherche.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(search))
                        query = query.Where(p =>
                            p.Nom.ToLower().Contains(search) ||
                            (p.Type != null && p.Type.ToLower().Contains(search)) ||
                            (p.Fournisseur != null && p.Fournisseur.Nom.ToLower().Contains(search)));

                    string type = cmbTypeFiltre.SelectedItem?.ToString() ?? "Tous";
                    if (type != "Tous")
                        query = query.Where(p => p.Type == type);

                    DateTime aujourdhui = DateTime.Today;
                    var data = query.OrderBy(p => p.Nom).ToList().Select(p => new
                    {
                        p.Id,
                        p.Nom,
                        p.Type,
                        p.PrixAchat,
                        p.PrixVente,
                        Marge = p.MargeBeneficiaire.ToString("0.#") + " %",
                        Stock = StockService.Formater(p),
                        Seuil = $"{p.SeuilAlerte} boîte(s)",
                        UniteVente = p.UniteVente,
                        Expiration = p.DateExpiration.ToString("dd/MM/yyyy"),
                        Fournisseur = p.Fournisseur?.Nom ?? "—",
                        Etat = !p.Actif ? "Archivé"
                             : p.QuantiteEnStock > 0 && p.DateExpiration.Date < aujourdhui ? "Périmé"
                             : p.EstEnRupture() ? "Alerte"
                             : "OK"
                    }).ToList();

                    dgvProduits.DataSource = data;
                    lblCompteur.Text = $"{data.Count} produit(s)";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Catalogue produits", ex);
                MessageBox.Show("Les produits n'ont pas pu être affichés : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProduits_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProduits.Rows.Count) return;
            string etat = dgvProduits.Rows[e.RowIndex].Cells["Etat"].Value?.ToString() ?? "";

            if (etat == "Archivé")
            {
                e.CellStyle.BackColor = Theme.InfoFond;
                e.CellStyle.ForeColor = Theme.Neutre;
            }
            else if (etat == "Périmé")
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

        private void dgvProduits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnModifier_Click(sender, EventArgs.Empty);
        }

        private void btnNouveauProduit_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddProduit())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerProduits();
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int id = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            using (var form = new FormAddProduit(id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerProduits();
            }
        }

        private (int id, string nom, bool actif)? SelectionProduit()
        {
            if (dgvProduits.SelectedRows.Count == 0) return null;
            int id = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            return (id, dgvProduits.SelectedRows[0].Cells["Nom"].Value?.ToString() ?? "",
                    ArchivageService.EstActif(TypeElement.Produit, id));
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (ArchivageUi.Archiver(TypeElement.Produit, SelectionProduit()))
                ChargerProduits();
        }
    }
}
