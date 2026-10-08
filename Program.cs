using Pharmacie2.Models;
using Pharmacie2.Services;
using Microsoft.EntityFrameworkCore;

namespace Pharmacie2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            GestionErreurs.Installer();   // journal + message simple en français pour toute erreur imprévue

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
                    context.Database.Migrate();
                    MotDePasseService.ConvertirMotsDePasseEnClair(context);   // idempotent
                    firstLaunch = !context.Users.Any();
                }
                catch (Exception ex)
                {
                    Journal.Erreur("Échec de la migration de la base", ex);
                    MessageBox.Show(
                        "La mise à jour de la base a échoué. Vos données n'ont pas été perdues : " +
                        "une sauvegarde a été faite dans " + CheminsApp.DossierSauvegardes + "\n\n" +
                        "Contactez le développeur.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                    return;
                }
            }

            if (firstLaunch)
            {
                Application.Run(new FormInitialize());
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
