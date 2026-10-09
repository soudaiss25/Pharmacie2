using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;

namespace Pharmacie2
{
    public partial class FormInitialize : Form
    {
        public FormInitialize()
        {
            InitializeComponent();
            Theme.Appliquer(this);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Vérification des champs
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Tous les champs sont obligatoires.",
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int nouvelAdminId;
            using (var context = new AppDbContext())
            {
                // Vérifier si login existe déjà
                bool loginExists = context.Users
                                          .Any(u => u.Login == txtLogin.Text);

                if (loginExists)
                {
                    MessageBox.Show("Cet identifiant existe déjà.",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                var admin = new User
                {
                    Nom = txtNom.Text.Trim(),
                    Prenom = txtPrenom.Text.Trim(),
                    Login = txtLogin.Text.Trim(),
                    MotDePasse = MotDePasseService.Hacher(txtPassword.Text.Trim()),
                    Role = Roles.Administrateur
                };

                context.Users.Add(admin);
                context.SaveChanges();
                nouvelAdminId = admin.Id;
            }

            // Clé de secours du premier administrateur (affichée une seule fois)
            FormCleSecours.GenererAfficherEtEnregistrer(this, nouvelAdminId);

            // Ouvre Form1
            var connexion = new Form1();
            connexion.FormClosed += (s, args) => Application.Exit();   // sinon ce formulaire masqué garderait l'application ouverte
            this.Hide();
            connexion.Show();
        }
    }
}