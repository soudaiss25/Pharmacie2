using Pharmacie2.Models;
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
            numDelaiVerrouillage.Value = Math.Min(60, Math.Max(0, ParametresApp.Actuels.DelaiVerrouillageMinutes));
            ActualiserEtatExterne();
            InitialiserMiseAJour();
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

        // ── Verrouillage automatique ──────────────────────────────────────

        private void numDelaiVerrouillage_ValueChanged(object sender, EventArgs e)
            => ParametresApp.Modifier(p => p.DelaiVerrouillageMinutes = (int)numDelaiVerrouillage.Value);

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

        // ── Mises à jour ──────────────────────────────────────────────────

        private bool _canalEnChargement;

        private void InitialiserMiseAJour()
        {
            var service = MiseAJourService.Instance;
            lblVersion.Text = $"Version installée : {AppInfo.Version}" + (service.EstInstalle ? "" : " (copie non installée : lancée depuis Visual Studio)")
                              + $" — canal : {(service.Canal == MiseAJourService.CanalTest ? "Test" : "Stable")}";
            _canalEnChargement = true;
            cbCanal.SelectedIndex = service.Canal == MiseAJourService.CanalTest ? 1 : 0;
            _canalEnChargement = false;
            // le choix du canal est réservé à l'Administrateur
            bool admin = SessionUtilisateur.Courant?.Role == Roles.Administrateur;
            cbCanal.Enabled = admin;
            lblCanal.Enabled = admin;
        }

        private void cbCanal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_canalEnChargement || SessionUtilisateur.Courant?.Role != Roles.Administrateur) return;
            MiseAJourService.Instance.ChangerCanal(cbCanal.SelectedIndex == 1 ? MiseAJourService.CanalTest : MiseAJourService.CanalStable);
            InitialiserMiseAJour();
            lblDescMaj.Text = "Canal modifié. Cliquez sur « Rechercher une mise à jour maintenant » pour vérifier ce canal.";
        }

        private async void btnMaj_Click(object sender, EventArgs e)
        {
            btnMaj.Enabled = false;
            lblDescMaj.Text = "Recherche en cours…";
            try
            {
                var r = await MiseAJourService.Instance.VerifierEtTelechargerAsync();
                lblDescMaj.Text = $"Version actuelle : {r.VersionActuelle}\nVersion disponible : {r.VersionDisponible ?? "aucune"}\n{r.Message}";
            }
            finally { if (!IsDisposed) btnMaj.Enabled = true; }
        }

        // ── Import d'une ancienne base (assistant) ────────────────────────

        private void btnImporter_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Choisissez l'ancienne base à importer",
                Filter = "Base ou sauvegarde (*.sqlite;*.chiffre)|*.sqlite;*.chiffre|Tous les fichiers (*.*)|*.*",
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

            using (var assistant = new FormAssistantImport(ofd.FileName, phrase))
                if (assistant.ShowDialog(this) != DialogResult.OK) return;   // annulé ou refusé : rien n'a été modifié

            try
            {
                // Juste après l'import : diagnostic et corrections certaines, puis rapport
                RapportDiagnostic rapport;
                using (var ctx = new AppDbContext())
                    rapport = DiagnosticBaseService.Executer(ctx, appliquer: true);
                DiagnosticBaseService.Enregistrer(rapport);
                using var fenetre = new FormDiagnostic(rapport, apresImport: true);
                fenetre.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Diagnostic après import", ex);
                MessageBox.Show("L'import est fait, mais le diagnostic n'a pas pu être lancé : " + ex.Message, "Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show("L'import est terminé. L'application va redémarrer.", "Importer une ancienne base", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
            Environment.Exit(0);
        }

        // ── Diagnostic (lecture seule) ────────────────────────────────────

        private void btnDiagnostic_Click(object sender, EventArgs e)
        {
            try
            {
                RapportDiagnostic rapport;
                using (var ctx = new AppDbContext())
                    rapport = DiagnosticBaseService.Executer(ctx, appliquer: false);   // jamais de correction automatique ici
                DiagnosticBaseService.Enregistrer(rapport);
                using var fenetre = new FormDiagnostic(rapport, apresImport: false);
                fenetre.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Diagnostic de la base", ex);
                MessageBox.Show("Le diagnostic n'a pas pu être fait : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
