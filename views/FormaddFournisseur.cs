using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class FormaddFournisseur : Form
    {
        private readonly Fournisseur _fournisseur;
        private readonly bool _isEdit;

        // Ajout
        public FormaddFournisseur()
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _fournisseur = new Fournisseur();
            _isEdit = false;

            lblTitle.Text = "Nouveau fournisseur";
            btnEnregistrer.Text = "Enregistrer";
        }

        // Modification (on passe un fournisseur existant)
        public FormaddFournisseur(Fournisseur fournisseur)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _fournisseur = fournisseur;
            _isEdit = true;

            lblTitle.Text = "Modifier le fournisseur";
            btnEnregistrer.Text = "Modifier";

            // Pré-remplir
            txtNom.Text = _fournisseur.Nom;
            txtContact.Text = _fournisseur.Contact;
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNom.Text))
                {
                    MessageBox.Show("Le nom est obligatoire.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNom.Focus();
                    return;
                }

                var nom = txtNom.Text.Trim();
                var contact = txtContact.Text.Trim();

                using (var context = new AppDbContext())
                {
                    // Doublons (uniquement en ajout)
                    if (!_isEdit)
                    {
                        var exists = context.fournisseur.Any(f => f.Nom.ToLower() == nom.ToLower());
                        if (exists)
                        {
                            MessageBox.Show("Ce fournisseur existe déjà.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var newF = new Fournisseur
                        {
                            Nom = nom,
                            Contact = contact
                        };

                        context.fournisseur.Add(newF);
                        context.SaveChanges();
                    }
                    else
                    {
                        // Update : on recharge depuis DB et on modifie
                        var fDb = context.fournisseur.Find(_fournisseur.Id);
                        if (fDb == null)
                        {
                            MessageBox.Show("Fournisseur introuvable.", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        fDb.Nom = nom;
                        fDb.Contact = contact;

                        context.SaveChanges();
                    }
                }

BandeauNotification.Succes("Fournisseur enregistré : " + nom);

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
    }
}
