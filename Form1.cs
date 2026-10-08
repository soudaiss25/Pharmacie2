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

            var lienOubli = new LinkLabel
            {
                Text = "Mot de passe oublié ?",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Location = new System.Drawing.Point(40, 368),
                Size = new System.Drawing.Size(320, 24),
                Font = new System.Drawing.Font("Segoe UI", 9.5F)
            };
            lienOubli.LinkClicked += (s, e) =>
            {
                using var f = new FormReinitialisationMdp(txtUsername.Text.Trim());
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            };
            panelLogin.Controls.Add(lienOubli);
        }

        /// <summary>
        /// Déconnexion : on vide la session et on ré-affiche CETTE fenêtre de connexion (jamais une seconde).
        /// Fermeture par la croix : on quitte l'application.
        /// </summary>
        private void ApresFermetureDePage(bool deconnexion)
        {
            if (!deconnexion)
            {
                Application.Exit();
                return;
            }

            SessionUtilisateur.Deconnecter();
            txtUsername.Clear();
            txtPassword.Clear();
            Show();
            txtUsername.Focus();
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

            {
                var resultat = AuthentificationService.Authentifier(login, mdp, out var user);

                if (resultat == ResultatConnexion.CompteArchive)
                {
                    MessageBox.Show("Ce compte a été archivé. Contactez un administrateur.", "Connexion",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    return;
                }

                if (resultat != ResultatConnexion.Succes || user == null)
                {
                    MessageBox.Show("Login ou mot de passe incorrect.", "Connexion",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Administrateur sans clé de secours (première connexion après la mise à jour) : en créer une
                if (user.Role == Roles.Administrateur && !CleSecoursService.AUneCle(user.Id))
                    FormCleSecours.GenererAfficherEtEnregistrer(this, user.Id);

                // ? Stocker l'utilisateur connecté en session
                SessionUtilisateur.Courant = user;

                this.Hide();

                if (user.Role == Roles.Caissier)
                {
                    var caisse = new PageCaissier(user);
                    caisse.FormClosed += (s, args) => ApresFermetureDePage(caisse.DeconnexionDemandee);
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
                    accueil.FormClosed += (s, args) => ApresFermetureDePage(accueil.DeconnexionDemandee);
                    accueil.Show();
                    if (verifierStock)
                        accueil.OuvrirStockAVerifier();
                }
            }
        }
    }
}