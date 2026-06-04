using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.views;

namespace Pharmacie2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtUsername.Text.Trim();
            string mdp = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(mdp))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Connexion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var ctx = new AppDbContext())
            {
                var user = ctx.Users.FirstOrDefault(u =>
                    u.Login == login && u.MotDePasse == mdp);

                if (user == null)
                {
                    MessageBox.Show("Login ou mot de passe incorrect.", "Connexion",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // ? Stocker l'utilisateur connecté en session
                SessionUtilisateur.Courant = user;

                this.Hide();

                if (user.Role == "Caissier")
                {
                    var caisse = new PageCaissier(user);
                    caisse.FormClosed += (s, args) => Application.Exit();
                    caisse.Show();
                }
                else
                {
                    var accueil = new PageAccueil(user);
                    accueil.FormClosed += (s, args) => Application.Exit();
                    accueil.Show();
                }
            }
        }
    }
}