using System;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Utilisateurs : UserControl
    {
        private enum Mode { Aucun, Ajout, Modification }
        private Mode _mode = Mode.Aucun;
        private int _idEnCours = -1;
        private CheckBox _chkArchives;

        public Uc_Utilisateurs()
        {
            InitializeComponent();
            Theme.Appliquer(this);
            _chkArchives = ArchivageUi.Installer(btnSupprimer, TypeElement.Utilisateur, SelectionUtilisateur, () => ChargerUtilisateurs(txtRecherche.Text));
            this.Load += (s, e) => ChargerUtilisateurs();
        }

        // ─── Chargement grille ────────────────────────────────────────────

        private void ChargerUtilisateurs(string recherche = "")
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    bool archives = _chkArchives?.Checked ?? false;
                    var query = ctx.Users.Where(u => u.Actif || archives);

                    if (!string.IsNullOrWhiteSpace(recherche))
                    {
                        recherche = recherche.ToLower();
                        query = query.Where(u =>
                            u.Nom.ToLower().Contains(recherche) ||
                            u.Prenom.ToLower().Contains(recherche) ||
                            u.Login.ToLower().Contains(recherche) ||
                            u.Role.ToLower().Contains(recherche));
                    }

                    // Le mot de passe n'est jamais affiché : l'administrateur peut seulement en définir un nouveau
                    var data = query
                        .OrderBy(u => u.Nom)
                        .Select(u => new
                        {
                            u.Id,
                            u.Nom,
                            u.Prenom,
                            u.Login,
                            u.Role,
                            Statut = u.Actif ? "" : "Archivé"
                        })
                        .ToList();

                    dgvUtilisateurs.DataSource = null;
                    dgvUtilisateurs.DataSource = data;

                    ColorerColonneRole();
                    lblCompteur.Text = $"{Format.Compte(data.Count, "utilisateur")}";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Chargement des utilisateurs", ex);
                MessageBox.Show("Les utilisateurs n'ont pas pu être affichés : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorerColonneRole()
        {
            foreach (DataGridViewRow row in dgvUtilisateurs.Rows)
            {
                if (row.Cells["Role"].Value == null) continue;
                string role = row.Cells["Role"].Value.ToString();
                var (texte, fond) = role switch
                {
                    Roles.Administrateur => (Theme.SuccesTexte, Theme.SuccesFond),
                    Roles.Pharmacien => (Theme.InfoTexte, Theme.InfoFond),
                    Roles.Caissier => (Theme.AttentionTexte, Theme.AttentionFond),
                    _ => (Theme.Neutre, Theme.InfoFond)
                };
                row.Cells["Role"].Style.ForeColor = texte;
                row.Cells["Role"].Style.BackColor = fond;
            }
        }

        // ─── Recherche ────────────────────────────────────────────────────

        private void txtRecherche_TextChanged(object sender, EventArgs e)
            => ChargerUtilisateurs(txtRecherche.Text);

        private void btnEffacerRecherche_Click(object sender, EventArgs e)
        {
            txtRecherche.Clear();
            ChargerUtilisateurs();
        }

        // ─── Sélection dans la grille ─────────────────────────────────────

        private void dgvUtilisateurs_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode != Mode.Aucun) return;
            if (dgvUtilisateurs.SelectedRows.Count == 0) return;
            AfficherDansFormulaire(dgvUtilisateurs.SelectedRows[0]);
        }

        private void AfficherDansFormulaire(DataGridViewRow row)
        {
            if (row == null) return;
            int id = Convert.ToInt32(row.Cells["Id"].Value);

            using (var ctx = new AppDbContext())
            {
                var u = ctx.Users.Find(id);
                if (u == null) return;

                txtNom.Text = u.Nom;
                txtPrenom.Text = u.Prenom;
                txtLogin.Text = u.Login;
                txtMotDePasse.Clear();   // le mot de passe n'est jamais affiché
                cbRole.SelectedItem = u.Role;
                _idEnCours = u.Id;
            }
        }

        // ─── CRUD ─────────────────────────────────────────────────────────

        private void btnNouvel_Click(object sender, EventArgs e)
        {
            _mode = Mode.Ajout;
            _idEnCours = -1;
            ViderFormulaire();
            ActiverFormulaire(true);
            MettreAJourBoutons();
            txtNom.Focus();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvUtilisateurs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez un utilisateur à modifier.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _mode = Mode.Modification;
            AfficherDansFormulaire(dgvUtilisateurs.SelectedRows[0]);
            ActiverFormulaire(true);
            MettreAJourBoutons();
            txtNom.Focus();
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (!ValiderFormulaire()) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    if (_mode == Mode.Ajout)
                    {
                        if (ctx.Users.Any(u => u.Login == txtLogin.Text.Trim()))
                        {
                            MessageBox.Show("Cet identifiant est déjà utilisé.", "Doublon",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtLogin.Focus();
                            return;
                        }

                        ctx.Users.Add(new User
                        {
                            Nom = txtNom.Text.Trim(),
                            Prenom = txtPrenom.Text.Trim(),
                            Login = txtLogin.Text.Trim(),
                            MotDePasse = MotDePasseService.Hacher(txtMotDePasse.Text.Trim()),
                            Role = cbRole.SelectedItem.ToString()
                        });

                        ctx.SaveChanges();
                        BandeauNotification.Succes("Utilisateur créé : " + txtLogin.Text.Trim());
                    }
                    else if (_mode == Mode.Modification)
                    {
                        var u = ctx.Users.Find(_idEnCours);
                        if (u == null)
                        {
                            MessageBox.Show("Utilisateur introuvable.", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (ctx.Users.Any(x => x.Login == txtLogin.Text.Trim() && x.Id != _idEnCours))
                        {
                            MessageBox.Show("Cet identifiant est déjà utilisé.", "Doublon",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtLogin.Focus();
                            return;
                        }

                        u.Nom = txtNom.Text.Trim();
                        u.Prenom = txtPrenom.Text.Trim();
                        u.Login = txtLogin.Text.Trim();
                        // Champ vide = mot de passe inchangé ; sinon l'administrateur définit un nouveau mot de passe
                        if (!string.IsNullOrWhiteSpace(txtMotDePasse.Text))
                            u.MotDePasse = MotDePasseService.Hacher(txtMotDePasse.Text.Trim());
                        u.Role = cbRole.SelectedItem.ToString();

                        ctx.SaveChanges();
                        BandeauNotification.Succes("Utilisateur modifié : " + txtLogin.Text.Trim());
                    }
                }

                AnnulerEdition();
                ChargerUtilisateurs(txtRecherche.Text);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Enregistrement d'un utilisateur", ex);
                MessageBox.Show("L'utilisateur n'a pas pu être enregistré : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
            => AnnulerEdition();

        private (int id, string nom, bool actif)? SelectionUtilisateur()
        {
            if (dgvUtilisateurs.SelectedRows.Count == 0) return null;
            var row = dgvUtilisateurs.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["Id"].Value);
            return (id, $"{row.Cells["Prenom"].Value} {row.Cells["Nom"].Value}",
                    ArchivageService.EstActif(TypeElement.Utilisateur, id));
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (ArchivageUi.Archiver(TypeElement.Utilisateur, SelectionUtilisateur()))
            {
                AnnulerEdition();
                ChargerUtilisateurs(txtRecherche.Text);
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────

        private bool ValiderFormulaire()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            { MessageBox.Show("Nom obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNom.Focus(); return false; }

            if (string.IsNullOrWhiteSpace(txtPrenom.Text))
            { MessageBox.Show("Prénom obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPrenom.Focus(); return false; }

            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            { MessageBox.Show("L'identifiant de connexion est obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLogin.Focus(); return false; }

            if (_mode == Mode.Ajout && string.IsNullOrWhiteSpace(txtMotDePasse.Text))
            { MessageBox.Show("Mot de passe obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMotDePasse.Focus(); return false; }

            if (cbRole.SelectedIndex < 0)
            { MessageBox.Show("Sélectionnez un rôle.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cbRole.Focus(); return false; }

            return true;
        }

        private void ViderFormulaire()
        {
            txtNom.Clear(); txtPrenom.Clear();
            txtLogin.Clear(); txtMotDePasse.Clear();
            cbRole.SelectedIndex = -1;
            _idEnCours = -1;
        }

        private void ActiverFormulaire(bool actif)
        {
            txtMotDePasse.UseSystemPasswordChar = true;
            txtMotDePasse.PlaceholderText = _mode == Mode.Modification ? "(laisser vide = inchangé)" : "";
            lblMdp.Text = _mode == Mode.Modification ? "Nouveau mot de passe" : "Mot de passe *";
            txtNom.Enabled = actif;
            txtPrenom.Enabled = actif;
            txtLogin.Enabled = actif;
            txtMotDePasse.Enabled = actif;
            cbRole.Enabled = actif;
            btnEnregistrer.Visible = actif;
            btnAnnuler.Visible = actif;
        }

        private void AnnulerEdition()
        {
            _mode = Mode.Aucun;
            _idEnCours = -1;
            ViderFormulaire();
            ActiverFormulaire(false);
            MettreAJourBoutons();
        }

        private void MettreAJourBoutons()
        {
            bool enEdition = (_mode != Mode.Aucun);
            btnNouvel.Enabled = !enEdition;
            btnModifier.Enabled = !enEdition;
            btnSupprimer.Enabled = !enEdition;
        }
    }
}