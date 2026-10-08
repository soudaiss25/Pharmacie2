using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
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

                if (user != null && !user.Actif)
                {
                    MessageBox.Show("Ce compte a été archivé. Contactez un administrateur.", "Connexion",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    return;
                }

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
                    // Un seul message par session, pour l'Administrateur et le Pharmacien
                    bool verifierStock = false;
                    int nbAVerifier = VerificationStockService.NombreAVerifier();
                    if (nbAVerifier > 0)
                    {
                        verifierStock = MessageBox.Show(
                            $"{nbAVerifier} produit(s) ont peut-être un stock incorrect à cause d'une ancienne erreur du logiciel.\n\n" +
                            "Voulez-vous les vérifier maintenant ?",
                            "Stock à vérifier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }

                    var accueil = new PageAccueil(user);
                    accueil.FormClosed += (s, args) => Application.Exit();
                    accueil.Show();
                    if (verifierStock)
                        accueil.OuvrirStockAVerifier();
                }
            }
        }
    }
}