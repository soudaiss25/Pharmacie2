using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    /// <summary>
    /// Réception d'une commande ligne par ligne : commandé, déjà reçu, reste, et « reçu maintenant » (modifiable).
    /// Les quantités sont en boîtes ; le stock est mis à jour par CommandeService.Receptionner.
    /// </summary>
    public partial class FormReceptionPartielle : Form
    {
        private readonly int _commandeId;

        public FormReceptionPartielle(int commandeId)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            Theme.StyliserGrille(dgvLignes);
            _commandeId = commandeId;

            bool heritee = CommandeService.EstPartielleHeritee(commandeId);
            if (heritee)
            {
                lblAvertissement.Text = CommandeService.MessageQuantiteInconnue
                    + " Saisissez les quantités réellement reçues ligne par ligne.";
                lblAvertissement.Visible = true;
                btnToutRecevoir.Enabled = false;
            }

            using (var ctx = new AppDbContext())
            {
                var commande = ctx.commandes.AsNoTracking().Include(c => c.Fournisseur).FirstOrDefault(c => c.Id == commandeId);
                lblTitre.Text = commande?.Fournisseur != null
                    ? "Réception de la commande de " + commande.Fournisseur.Nom
                    : "Réception de commande";

                var lignes = ctx.LigneCommandes.AsNoTracking().Include(l => l.Produit)
                    .Where(l => l.CommandeId == commandeId).OrderBy(l => l.Id).ToList();
                foreach (var l in lignes)
                {
                    int reste = Math.Max(0, l.Quantite - l.QuantiteRecue);
                    int idx = dgvLignes.Rows.Add(l.Id, l.Produit?.Nom ?? "—", l.Quantite, l.QuantiteRecue, reste, heritee ? 0 : reste);
                    if (reste == 0)
                    {
                        dgvLignes.Rows[idx].Cells["colRecu"].ReadOnly = true;
                        dgvLignes.Rows[idx].Cells["colRecu"].Value = 0;
                        dgvLignes.Rows[idx].DefaultCellStyle.ForeColor = Theme.Neutre;
                    }
                }
            }
        }

        private void btnToutRecevoir_Click(object sender, EventArgs e)
        {
            dgvLignes.EndEdit();
            foreach (DataGridViewRow r in dgvLignes.Rows)
                r.Cells["colRecu"].Value = r.Cells["colReste"].Value;
        }

        private void dgvLignes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvLignes.Columns[e.ColumnIndex].Name != "colRecu") return;
            var ligne = dgvLignes.Rows[e.RowIndex];
            string saisie = Convert.ToString(e.FormattedValue)?.Trim() ?? "";
            if (saisie.Length == 0) saisie = "0";

            if (!int.TryParse(saisie, out int qte) || qte < 0)
            {
                ligne.ErrorText = "Saisissez un nombre entier de boîtes (0 ou plus).";
                e.Cancel = true;
                return;
            }
            int reste = Convert.ToInt32(ligne.Cells["colReste"].Value);
            if (qte > reste)
            {
                ligne.ErrorText = $"Il ne reste que {Format.Compte(reste, "boîte")} à recevoir pour ce produit.";
                e.Cancel = true;
                return;
            }
            ligne.ErrorText = "";
        }

        private void dgvLignes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = false;
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (!dgvLignes.EndEdit() || !ValidateChildren())
                return;

            var quantites = new Dictionary<int, int>();
            foreach (DataGridViewRow r in dgvLignes.Rows)
            {
                int qte = 0;
                var v = r.Cells["colRecu"].Value;
                if (v != null) int.TryParse(Convert.ToString(v), out qte);
                if (qte > 0) quantites[Convert.ToInt32(r.Cells["colLigneId"].Value)] = qte;
            }

            if (quantites.Count == 0)
            {
                MessageBox.Show("Saisissez au moins une quantité reçue.", "Réception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int boites = CommandeService.Receptionner(_commandeId, quantites);
                if (boites <= 0)
                {
                    MessageBox.Show("Aucune boîte à enregistrer.", "Réception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                BandeauNotification.Succes($"Réception enregistrée : {Format.Compte(boites, "boîte")}. Le stock est à jour.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Réception de la commande " + _commandeId, ex);
                MessageBox.Show("La réception n'a pas pu être enregistrée : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => Close();
    }
}
