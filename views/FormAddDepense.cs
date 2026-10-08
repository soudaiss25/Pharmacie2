using System;
using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class FormAddDepense : Form
    {
        public FormAddDepense()
        {
            InitializeComponent();
            Theme.Appliquer(this);
            cbCategorie.SelectedIndex = -1;
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

            decimal montant = numMontant.Value;
            if (montant <= 0)
            {
                MessageBox.Show("Saisissez un montant supérieur à 0.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMontant.Focus(); return;
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

                BandeauNotification.Succes($"Dépense enregistrée : {cbCategorie.Text}, {Format.Montant(montant)}");

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