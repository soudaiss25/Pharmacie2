using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Stock : UserControl
    {
        private readonly AppDbContext _context = new AppDbContext();

        public Uc_Stock()
        {
            InitializeComponent();

            dgvStock.AutoGenerateColumns = true;
            dgvStock.MultiSelect = false;

            cbSeuil.SelectedIndex = 0;
            cbDisponibilite.SelectedIndex = 0;

            txtSearchProduit.TextChanged += (s, e) => ChargerStock();
            cbSeuil.SelectedIndexChanged += (s, e) => ChargerStock();
            cbDisponibilite.SelectedIndexChanged += (s, e) => ChargerStock();
            dgvStock.CellFormatting += dgvStock_CellFormatting;

            btnAjouter.Click += btnAjouter_Click;
            btnModifier.Click += btnModifier_Click;
            btnSupprimer.Click += btnSupprimer_Click;
            btnCommander.Click += btnCommander_Click;

            ChargerStock();
        }

        // ── Chargement stock ──────────────────────────────────────────────

        private void ChargerStock()
        {
            string search = txtSearchProduit.Text.ToLower();

            var query = _context.produits
                .Include(p => p.Fournisseur)
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
            else if (cbSeuil.Text == "Normal")
                query = query.Where(p => p.QuantiteEnStock > p.SeuilAlerte * (p.NbUniteParBoite > 1 ? p.NbUniteParBoite : 1));

            var data = query.ToList().Select(p => new
            {
                p.Id,
                Produit = p.Nom,
                p.Type,
                Fournisseur = p.Fournisseur != null ? p.Fournisseur.Nom : "—",
                // Ex : 9 unités (5/boîte) → « 1 boîte(s) + 4 plaquette(s) »
                Quantite = StockService.Formater(p),
                Seuil = $"{p.SeuilAlerte} boîte(s)",
                Etat = p.QuantiteEnStock <= 0 ? "Rupture"
                            : p.QuantiteEnStock <= StockService.SeuilEnUnites(p) ? "Alerte"
                            : "OK"
            }).ToList();

            dgvStock.DataSource = data;

            if (dgvStock.Columns["Id"] != null)
                dgvStock.Columns["Id"].Visible = false;
        }

        private void dgvStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvStock.Columns["Etat"] == null) return;

            var etat = dgvStock.Rows[e.RowIndex].Cells["Etat"].Value?.ToString();

            switch (etat)
            {
                case "Rupture":
                    dgvStock.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        System.Drawing.Color.FromArgb(255, 205, 210);
                    break;
                case "Alerte":
                    dgvStock.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        System.Drawing.Color.FromArgb(255, 224, 178);
                    break;
                default:
                    dgvStock.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        System.Drawing.Color.White;
                    break;
            }
        }

        // ── Ajouter ───────────────────────────────────────────────────────

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddProduit())
            {
                if (form.ShowDialog() == DialogResult.OK)
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

            // ✅ On passe l'ID (int), pas l'objet Produit
            int id = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);

            using (var form = new FormAddProduit(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    ChargerStock();
            }
        }

        // ── Supprimer ─────────────────────────────────────────────────────

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvStock.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit à supprimer.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);
            string nom = dgvStock.SelectedRows[0].Cells["Produit"].Value?.ToString();

            var confirm = MessageBox.Show(
                $"Supprimer le produit « {nom} » ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var produit = _context.produits.Find(id);
            if (produit == null) return;

            _context.produits.Remove(produit);
            _context.SaveChanges();
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

            // ✅ On passe l'ID (int) et le nom (string) — correspond au constructeur existant
            int produitId = Convert.ToInt32(dgvStock.SelectedRows[0].Cells["Id"].Value);
            string nomProduit = dgvStock.SelectedRows[0].Cells["Produit"].Value?.ToString() ?? "";

            using (var form = new FormCommandeProduit(produitId, nomProduit))
            {
                form.ShowDialog();
                ChargerStock();
            }
        }
    }
}