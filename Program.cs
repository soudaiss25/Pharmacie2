using Pharmacie2.Models;
using Pharmacie2.Services;
using Microsoft.EntityFrameworkCore;
using Velopack;

namespace Pharmacie2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Velopack doit être la TOUTE première chose : lors d'une installation, d'une mise à jour ou d'une désinstallation,
            // il prend la main puis quitte avant que le reste du programme ne s'exécute.
            VelopackApp.Build().Run();

            ApplicationConfiguration.Initialize();
            GestionErreurs.Installer();   // journal + message simple en français pour toute erreur imprévue

            // Mises à jour automatiques : rien ne se passe hors d'une vraie installation (Visual Studio, tests)
            MiseAJourService.Instance.DetecterMiseAJourInstallee();

            // Doit passer AVANT tout accès à la base.
            MigrationEmplacementBase.Executer();
            SauvegardeAutomatique.Executer();

            bool firstLaunch;
            using (var context = new AppDbContext())
            {
                // Crée la base si elle n'existe pas, applique les migrations en attente.
                // L'application ne doit jamais tourner sur une base partiellement migrée.
                try
                {
                    bool repriseAttendue = RepriseVentesService.EstEnAttente(context);
                    bool prixUniteAttendu = PrixUniteService.EstEnAttente(context);
                    context.Database.Migrate();
                    if (repriseAttendue) RepriseVentesService.Journaliser(context);   // une seule fois : au démarrage qui applique la migration
                    if (prixUniteAttendu) PrixUniteService.Journaliser(context);
                    MotDePasseService.ConvertirMotsDePasseEnClair(context);   // idempotent
                    firstLaunch = !context.Users.Any();

                    // Juste après l'import d'une ancienne base : diagnostic (D1 à D23) + corrections certaines (D1, D4, D11, D16, D17) + rapport.
                    // Une sauvegarde de la base copiée a été faite avant les migrations (SauvegardeAutomatique).
                    if (MigrationEmplacementBase.ImportEffectue && !firstLaunch)
                    {
                        try
                        {
                            var rapport = DiagnosticBaseService.Executer(context, appliquer: true);
                            DiagnosticBaseService.Enregistrer(rapport);
                            using var fenetre = new Pharmacie2.views.FormDiagnostic(rapport, apresImport: true);
                            fenetre.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            Journal.Erreur("Diagnostic de l'ancienne base", ex);
                            Messages.Afficher("Le diagnostic de l'ancienne base n'a pas pu être fait : " + ex.Message + "\n\nVos données n'ont pas été modifiées par le diagnostic.",
                                "Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Journal.Erreur("Échec de la migration de la base", ex);
                    Messages.Afficher(
                        "La mise à jour de la base a échoué. Vos données n'ont pas été perdues : " +
                        "une sauvegarde a été faite dans " + CheminsApp.DossierSauvegardes + "\n\n" +
                        "Contactez le développeur.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                    return;
                }
            }

            MiseAJourService.Instance.Demarrer();   // vérification 30 s après l'ouverture puis toutes les 4 h, en arrière-plan

            if (firstLaunch)
            {
                Application.Run(new FormInitialize());
            }
            else
            {
                Application.Run(new Form1());
            }

            // Fermeture normale : une mise à jour déjà téléchargée s'installe maintenant, sans redémarrage forcé
            MiseAJourService.Instance.AppliquerALaFermeture();
        }
    }
}
