using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    /// <summary>Page « Administration » : comptes, sauvegardes (locales et hors du PC), restauration, réglages.</summary>
    public partial class Uc_Administration : UserControl
    {
        public event Action? UtilisateursDemande;
        public event Action? SauvegardeDemande;
        public event Action? DossierDemande;

        public Uc_Administration()
        {
            InitializeComponent();
            Theme.Appliquer(this);
            ActualiserEtatExterne();
        }

        private void btnUtilisateurs_Click(object sender, EventArgs e) => UtilisateursDemande?.Invoke();
        private void btnSauvegarde_Click(object sender, EventArgs e) => SauvegardeDemande?.Invoke();
        private void btnDossier_Click(object sender, EventArgs e) => DossierDemande?.Invoke();

        // ── Sauvegarde hors du PC ─────────────────────────────────────────

        private void ActualiserEtatExterne()
        {
            if (!SauvegardeExterneService.EstConfiguree)
            {
                lblEtatExterne.Text = "Aucune sauvegarde hors du PC n'est configurée : une panne, un vol ou un virus ferait tout perdre. Choisissez un dossier ci-dessous.";
                lblEtatExterne.ForeColor = Theme.AttentionTexte;
                return;
            }

            var p = ParametresApp.Actuels;
            string derniere = p.DerniereCopieExterne is DateTime d ? d.ToString("dd/MM/yyyy à HH:mm") : "aucune pour le moment";
            lblEtatExterne.Text = $"Dossier : {p.DossierSauvegardeExterne}\nDernière copie réussie : {derniere}";
            lblEtatExterne.ForeColor = SauvegardeExterneService.JoursDepuisDerniereCopie(DateTime.Now) is int j && j <= SauvegardeExterneService.JoursAvantAlerte
                ? Theme.SuccesTexte : Theme.AttentionTexte;
        }

        private void btnDossierExterne_Click(object sender, EventArgs e)
        {
            string? dossier = null;
            string? drive = SauvegardeExterneService.DetecterGoogleDrive();
            if (drive != null &&
                MessageBox.Show($"Google Drive a été trouvé sur cet ordinateur :\n{drive}\n\nUtiliser ce dossier pour les sauvegardes ?",
                    "Sauvegarde hors du PC", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                dossier = drive;

            if (dossier == null)
            {
                using var fbd = new FolderBrowserDialog
                {
                    Description = "Choisissez le dossier de sauvegarde externe (dossier Google Drive, ou clé USB)",
                    ShowNewFolderButton = true
                };
                if (fbd.ShowDialog(this) != DialogResult.OK) return;
                dossier = fbd.SelectedPath;
            }

            try
            {
                if (!SauvegardeExterneService.EstConfiguree)
                {
                    using var f = new FormPhraseSecrete(confirmer: true);
                    if (f.ShowDialog(this) != DialogResult.OK) return;
                    SauvegardeExterneService.Configurer(dossier, f.Phrase);
                }
                else
                {
                    SauvegardeExterneService.ChangerDossier(dossier);
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Configuration de la sauvegarde externe", ex);
                MessageBox.Show("Le réglage n'a pas pu être enregistré : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ActualiserEtatExterne();
            CopierMaintenant();
        }

        private void btnCopierMaintenant_Click(object sender, EventArgs e)
        {
            if (!SauvegardeExterneService.EstConfiguree)
            {
                MessageBox.Show("Choisissez d'abord le dossier de sauvegarde externe.", "Sauvegarde hors du PC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CopierMaintenant();
        }

        private void CopierMaintenant()
        {
            bool ok;
            try
            {
                string locale = Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_{DateTime.Now:yyyy-MM-dd}.sqlite");
                SauvegardeAutomatique.Sauvegarder(locale);   // copie fraîche, avec les ventes de la journée
                ok = SauvegardeExterneService.CopierDuJour(forcer: true);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Copie externe immédiate", ex);
                ok = false;
            }

            ActualiserEtatExterne();
            if (ok)
                BandeauNotification.Succes("Copie chiffrée envoyée vers " + ParametresApp.Actuels.DossierSauvegardeExterne + ".");
            else
                MessageBox.Show("La copie n'a pas pu être faite : le dossier est introuvable (clé débranchée ?) ou protégé.\n" +
                                "Rien n'est perdu : une nouvelle tentative aura lieu au prochain démarrage.",
                    "Sauvegarde hors du PC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ── Rapport pour le développeur ───────────────────────────────────

        private void btnRapport_Click(object sender, EventArgs e)
        {
            try
            {
                RapportDeveloppeurService.PreparerSurLeBureauEtMontrer(this);
                MessageBox.Show(RapportDeveloppeurService.MessageApresRapport, "Rapport pour le développeur", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Préparation du rapport pour le développeur", ex);
                MessageBox.Show("Le rapport n'a pas pu être préparé : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Restauration ──────────────────────────────────────────────────

        private void btnRestaurer_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Choisissez la sauvegarde à restaurer",
                Filter = "Sauvegardes (*.sqlite;*.chiffre)|*.sqlite;*.chiffre|Tous les fichiers (*.*)|*.*",
                InitialDirectory = ParametresApp.Actuels.DossierSauvegardeExterne is { Length: > 0 } ext && Directory.Exists(ext) ? ext : CheminsApp.DossierSauvegardes
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            string? phrase = null;
            if (ofd.FileName.EndsWith(".chiffre", StringComparison.OrdinalIgnoreCase))
            {
                using var f = new FormPhraseSecrete(confirmer: false);
                if (f.ShowDialog(this) != DialogResult.OK) return;
                phrase = f.Phrase;
            }

            if (MessageBox.Show(
                    "Les données actuelles de la pharmacie vont être REMPLACÉES par celles de cette sauvegarde :\n" + Path.GetFileName(ofd.FileName) +
                    "\n\nLa base actuelle est d'abord mise de côté dans le dossier des sauvegardes (« avant-restauration »). Continuer ?",
                    "Restaurer une sauvegarde", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                SauvegardeExterneService.Restaurer(ofd.FileName, phrase);
            }
            catch (PhraseSecreteIncorrecteException)
            {
                MessageBox.Show("La phrase secrète est incorrecte (ou le fichier est abîmé). Rien n'a été modifié.", "Restaurer une sauvegarde",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (FichierSauvegardeInvalideException ex)
            {
                MessageBox.Show(ex.Message + "\nRien n'a été modifié.", "Restaurer une sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Restauration d'une sauvegarde", ex);
                MessageBox.Show("La restauration a échoué : " + ex.Message + "\nVos données actuelles n'ont pas été touchées.",
                    "Restaurer une sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("La sauvegarde a été restaurée. L'application va redémarrer.", "Restaurer une sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
