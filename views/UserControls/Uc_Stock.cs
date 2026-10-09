using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Stock : UserControl
    {
        private readonly CheckBox _chkArchives;

        public Uc_Stock()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            cbSeuil.SelectedIndex = 0;
            cbDisponibilite.SelectedIndex = 0;

            // Archivage (remplace la suppression)
            _chkArchives = ArchivageUi.Installer(btnSupprimer, TypeElement.Produit, SelectionProduit, ChargerStock);

            ChargerStock();
        }

        /// <summary>Filtre la liste sur les produits dont le stock est à vérifier.</summary>
        public void FiltrerAVerifier() => Filtrer("À vérifier");

        /// <summary>Applique un filtre depuis l'extérieur (« À vérifier », « Périmés », « Péremption proche », « En rupture »).</summary>
        public void Filtrer(string filtre)
        {
            if (filtre == "En rupture") cbDisponibilite.SelectedItem = filtre;
            else cbSeuil.SelectedItem = filtre;
        }

        private void txtSearchProduit_TextChanged(object sender, EventArgs e) => ChargerStock();

        private void Filtre_Changed(object sender, EventArgs e) => ChargerStock();

        private void btnInventaire_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "Classeur Excel (*.xlsx)|*.xlsx",
                FileName = $"Inventaire_{DateTime.Now:yyyy-MM-dd}.xlsx",
                Title = "Enregistrer la feuille d'inventaire"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                InventaireExportService.Exporter(dlg.FileName);
                BandeauNotification.Succes("Feuille d'inventaire enregistrée : " + dlg.FileName);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Export de la feuille d'inventaire", ex);
                MessageBox.Show("Impossible de créer la feuille d'inventaire : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Chargement stock ──────────────────────────────────────────────

        private void ChargerStock()
        {
            // Pendant la construction, les listes ne sont pas encore initialisées
            if (_chkArchives == null || cbSeuil.SelectedIndex < 0 || cbDisponibilite.SelectedIndex < 0) return;

            string search = txtSearchProduit.Text.ToLower();
            bool archives = _chkArchives.Checked;
            DateTime aujourdhui = DateTime.Today;
            DateTime limite = aujourdhui.AddDays(TableauDeBordService.JoursAvantPeremption);

            using var ctx = new AppDbContext();
            var query = ctx.produits
                .AsNoTracking()
                .Include(p => p.Fournisseur)
                .Where(p => p.Actif || archives)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p =>
                    p.Nom.ToLower().Contains(search) ||
                    p.Type.ToLower().Contains(search));

            if (cbDisponibilite.Text == "En rupture")
                query = query.Where(p => p.QuantiteEnStock <= 0);
            else if (cbDisponibilite.Text == "Disponible")
                query = query.Where(p => p.QuantiteEnStock > 0);

            if (cbSeuil.Text == "Sous le seuil")
                query = query.Where(p => p.QuantiteEnStock <= p.SeuilAlerte * (p.NbUniteParBoite > 1 ? p.NbUniteParBoite : 1));
            else if (cbSeuil.Text == "Périmés")
                query = query.Where(p => p.Actif && p.QuantiteEnStock > 0 && p.DateExpiration < aujourdhui);
            else if (cbSeuil.Text == "Péremption proche")
                query = query.Where(p => p.Actif && p.QuantiteEnStock > 0 && p.DateExpiration >= aujourdhui && p.DateExpiration <= limite);
            else if (cbSeuil.Text == "À vérifier")
                query = query.Where(p => p.StockAVerifier && p.Actif);
            else if (cbSeuil.Text == "Normal")
                query = query.Where(p => p.QuantiteEnStock > p.SeuilAlerte * (p.NbUniteParBoite > 1 ? p.NbUniteParBoite : 1));

            var data = query
                .OrderByDescending(p => p.Actif)            // archivés en dernier
                .ThenByDescending(p => p.StockAVerifier)    // produits « À vérifier » en premier
                .ThenBy(p => p.Nom)
                .ToList()
                .Select(p => new
                {
                    p.Id,
                    Produit = p.Nom,
                    p.Type,
                    Fournisseur = p.Fournisseur != null ? p.Fournisseur.Nom : "—",
                    // Ex : 9 unités (5/boîte) → « 1 boîte(s) + 4 plaquette(s) »
                    Quantite = StockService.Formater(p),
                    Seuil = $"{Format.Compte(p.SeuilAlerte, "boîte")}",
                    Expiration = p.DateExpiration.ToString("dd/MM/yyyy"),
                    Verification = !p.Actif ? "Archivé" : p.StockAVerifier ? "À vérifier" : "",
                    Etat = !p.Actif ? "Archivé"
                         : p.QuantiteEnStock > 0 && p.DateExpiration.Date < aujourdhui ? "Périmé"
                         : p.QuantiteEnStock <= 0 ? "Rupture"
                         : p.QuantiteEnStock <= StockService.SeuilEnUnites(p) ? "Alerte"
                         : "OK"
                })
                .ToList();

            dgvStock.DataSource = data;
        }

        private void dgvStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvStock.Rows.Count) return;

            var ligne = dgvStock.Rows[e.RowIndex];
            string etat = ligne.Cells["Etat"].Value?.ToString() ?? "";
            string verif = ligne.Cells["Verification"].Value?.ToString() ?? "";

            // Fond de ligne léger (rouge pâle = urgent, orange pâle = attention, gris = archivé) ; texte noir ;
            // seule la cellule « État » porte la couleur forte, en gras (badge)
            string niveau = etat == "Archivé" ? "info"
                : etat == "Périmé" || etat == "Rupture" ? "urgent"
                : etat == "Alerte" || verif == "À vérifier" ? "attention" : "";
            if (niveau != "")
            {
                var (texte, fond) = Theme.Niveau(niveau);
                e.CellStyle.BackColor = fond;
                e.CellStyle.ForeColor = Theme.Texte;
                if (dgvStock.Columns[e.ColumnIndex].Name == "Etat")
                {
                    e.CellStyle.ForeColor = niveau == "info" ? Theme.Neutre : texte;
                    e.CellStyle.Font = Theme.Police(10, FontStyle.Bold);
                }
            }
        }

        // ── Ajouter ───────────────────────────────────────────────────────

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddProduit())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerStock();
            }
        }

        // ── Modifier ──────────────────────────────────────────────────────

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvStock.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit à modifier.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);

            using (var form = new FormAddProduit(id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    ChargerStock();
            }
        }

        // ── Archiver ──────────────────────────────────────────────────────

        private (int id, string nom, bool actif)? SelectionProduit()
        {
            if (dgvStock.SelectedRows.Count == 0) return null;
            int id = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);
            return (id, dgvStock.SelectedRows[0].Cells["Produit"].Value?.ToString() ?? "",
                    ArchivageService.EstActif(TypeElement.Produit, id));
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (ArchivageUi.Archiver(TypeElement.Produit, SelectionProduit()))
                ChargerStock();
        }

        // ── Commander au fournisseur ──────────────────────────────────────

        private void btnCommander_Click(object sender, EventArgs e)
        {
            if (dgvStock.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit à commander.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int produitId = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);
            string nomProduit = dgvStock.SelectedRows[0].Cells["Produit"].Value?.ToString() ?? "";

            using (var form = new FormCommandeProduit(produitId, nomProduit))
            {
                form.ShowDialog(this);
                ChargerStock();
            }
        }
    }
}
