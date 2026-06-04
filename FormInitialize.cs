using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;

namespace Pharmacie2
{
    public partial class FormInitialize : Form
    {
        public FormInitialize()
        {
            InitializeComponent();
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

            using (var context = new AppDbContext())
            {
                // Vérifier si login existe déjà
                bool loginExists = context.Users
                                          .Any(u => u.Login == txtLogin.Text);

                if (loginExists)
                {
                    MessageBox.Show("Ce login existe déjà.",
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
                    MotDePasse = txtPassword.Text.Trim(), // ⚠️ On hash demain
                    Role = "Administrateur"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }

            MessageBox.Show("Administrateur créé avec succès.",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            // Ouvre Form1
            this.Hide();
            new Form1().Show();
        }
    }
}