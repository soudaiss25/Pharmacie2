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

            // Doit passer AVANT tout accès à la base.
            MigrationEmplacementBase.Executer();

            bool firstLaunch;
            using (var context = new AppDbContext())
            {
                // Crée la base si elle n'existe pas
                context.Database.Migrate();
                firstLaunch = !context.Users.Any();
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
