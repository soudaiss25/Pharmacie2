using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Produits : UserControl
    {
        public Uc_Produits()
        {
            InitializeComponent();
            this.Load += (s, e) => ChargerProduits();
            txtRecherche.TextChanged += (s, e) => ChargerProduits();
            cmbTypeFiltre.SelectedIndexChanged += (s, e) => ChargerProduits();
            btnEffacer.Click += (s, e) => { txtRecherche.Clear(); cmbTypeFiltre.SelectedIndex = 0; };
        }

        private void ChargerProduits()
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.produits
                        .Include(p => p.Fournisseur)
                        .AsQueryable();

                    // Filtre texte
                    string search = txtRecherche.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(search))
                        query = query.Where(p =>
                            p.Nom.ToLower().Contains(search) ||
                            (p.Type != null && p.Type.ToLower().Contains(search)) ||
                            (p.Fournisseur != null && p.Fournisseur.Nom.ToLower().Contains(search)));

                    // Filtre type
                    string type = cmbTypeFiltre.SelectedItem?.ToString() ?? "Tous";
                    if (type != "Tous")
                        query = query.Where(p => p.Type == type);

                    var data = query.OrderBy(p => p.Nom).ToList().Select(p => new
                    {
                        p.Id,
                        p.Nom,
                        p.Type,
                        PrixAchat = p.PrixAchat.ToString("0.00"),
                        PrixVente = p.PrixVente.ToString("0.00"),
                        Marge = p.MargeBeneficiaire.ToString("0.0") + "%",
                        Stock = p.QuantiteEnStock,
                        Seuil = p.SeuilAlerte,
                        UniteVente = p.UniteVente,
                        Expiration = p.DateExpiration.ToString("dd/MM/yyyy"),
                        Fournisseur = p.Fournisseur?.Nom ?? "—",
                        Etat = p.EstEnRupture() ? "⚠️ Alerte" : "✅ OK"
                    }).ToList();

                    dgvProduits.DataSource = null;
                    dgvProduits.DataSource = data;

                    if (dgvProduits.Columns["Id"] != null)
                        dgvProduits.Columns["Id"].Visible = false;

                    // Colorer la colonne Etat
                    foreach (DataGridViewRow row in dgvProduits.Rows)
                    {
                        string etat = row.Cells["Etat"].Value?.ToString() ?? "";
                        if (etat.Contains("Alerte"))
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                        }
                    }

                    lblCompteur.Text = $"{data.Count} produit(s)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNouveauProduit_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddProduit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    ChargerProduits();
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un produit.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int id = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            using (var form = new FormAddProduit(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    ChargerProduits();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvProduits.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvProduits.SelectedRows[0].Cells["Id"].Value);
            string nom = dgvProduits.SelectedRows[0].Cells["Nom"].Value?.ToString();

            if (MessageBox.Show($"Supprimer « {nom} » ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var p = ctx.produits.Find(id);
                    if (p != null) { ctx.produits.Remove(p); ctx.SaveChanges(); }
                }
                ChargerProduits();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}