using System;
using System.Windows.Forms;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class FormAddMutuelle : Form
    {
        private readonly Mutuel _mutuel;
        private readonly bool _isEdit;

        // Ajout
        public FormAddMutuelle()
        {
            InitializeComponent();
            _mutuel = new Mutuel();
            _isEdit = false;
            this.Text = "Nouvelle Mutuelle";
        }

        // Modification
        public FormAddMutuelle(Mutuel mutuel)
        {
            InitializeComponent();
            _mutuel = mutuel;
            _isEdit = true;
            this.Text = "Modifier Mutuelle";

            txtNomEmployeur.Text = mutuel.NomEmployeur;
            txtTaux.Text = mutuel.TauxPriseEnCharge.ToString("0.00");
            txtEmail.Text = mutuel.EmailContact;
            txtTelephone.Text = mutuel.telephoneEmployeur;
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomEmployeur.Text))
            {
                MessageBox.Show("Le nom de l'employeur est obligatoire.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomEmployeur.Focus();
                return;
            }

            if (!decimal.TryParse(txtTaux.Text, out decimal taux) || taux < 0 || taux > 100)
            {
                MessageBox.Show("Le taux doit être entre 0 et 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaux.Focus();
                return;
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    if (_isEdit)
                    {
                        var m = ctx.mutuels.Find(_mutuel.IdMutuel);
                        if (m == null)
                        {
                            MessageBox.Show("Mutuelle introuvable.", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        m.NomEmployeur = txtNomEmployeur.Text.Trim();
                        m.TauxPriseEnCharge = taux;
                        m.EmailContact = txtEmail.Text.Trim();
                        m.telephoneEmployeur = txtTelephone.Text.Trim();
                    }
                    else
                    {
                        ctx.mutuels.Add(new Mutuel
                        {
                            NomEmployeur = txtNomEmployeur.Text.Trim(),
                            TauxPriseEnCharge = taux,
                            EmailContact = txtEmail.Text.Trim(),
                            telephoneEmployeur = txtTelephone.Text.Trim()
                        });
                    }
                    ctx.SaveChanges();
                }

                MessageBox.Show("Enregistrement réussi !", "Succès",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                this.Close();
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
            this.Close();
        }
    }
}