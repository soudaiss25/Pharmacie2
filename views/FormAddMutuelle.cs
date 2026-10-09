using System;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

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
            Theme.Appliquer(this);
            _mutuel = new Mutuel();
            _isEdit = false;
            this.Text = "Nouvelle mutuelle";
            lblTitre.Text = "Nouvelle mutuelle";
        }

        // Modification
        public FormAddMutuelle(Mutuel mutuel)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _mutuel = mutuel;
            _isEdit = true;
            this.Text = "Modifier la mutuelle";
            lblTitre.Text = "Modifier la mutuelle";

            txtNomEmployeur.Text = mutuel.NomEmployeur;
            numTaux.Value = Math.Min(100, Math.Max(0, mutuel.TauxPriseEnCharge));
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

            decimal taux = numTaux.Value;   // 0 à 100, garanti par le contrôle

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

                BandeauNotification.Succes("Mutuelle enregistrée : " + txtNomEmployeur.Text.Trim());
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Journal.Erreur("Enregistrement d'une mutuelle", ex);
                MessageBox.Show("La mutuelle n'a pas pu être enregistrée : " + ex.Message, "Erreur",
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