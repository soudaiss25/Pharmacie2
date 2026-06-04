using System;
using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class FormAddDepense : Form
    {
        public FormAddDepense()
        {
            InitializeComponent();
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            // Validations
            if (cbCategorie.SelectedIndex < 0)
            {
                MessageBox.Show("Choisissez une catégorie.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategorie.Focus(); return;
            }

            if (!decimal.TryParse(txtMontant.Text.Replace(" ", "").Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal montant) || montant <= 0)
            {
                MessageBox.Show("Saisissez un montant valide (supérieur à 0).", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontant.Focus(); return;
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    ctx.DepensesAnnexes.Add(new DepenseAnnexe
                    {
                        Categorie = cbCategorie.Text,
                        Description = txtDescription.Text.Trim(),
                        Montant = montant,
                        DateDepense = dtpDate.Value.Date,
                        UserId = SessionUtilisateur.IdCourant
                    });
                    ctx.SaveChanges();
                }

                MessageBox.Show(
                    $"✅ Dépense enregistrée !\n\n" +
                    $"Catégorie : {cbCategorie.Text}\n" +
                    $"Montant   : {montant:N0} KMF\n" +
                    $"Date      : {dtpDate.Value:dd/MM/yyyy}",
                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cbCategorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Placeholder selon la catégorie
            txtDescription.PlaceholderText = cbCategorie.Text switch
            {
                "Salaire" => "Ex: Salaire Thomas — Mai 2026",
                "Facture" => "Ex: Facture électricité — Mai 2026",
                "Loyer" => "Ex: Loyer local pharmacie — Mai 2026",
                "Fournitures" => "Ex: Sacs, étiquettes, stylos...",
                _ => "Description de la dépense..."
            };
        }
    }
}