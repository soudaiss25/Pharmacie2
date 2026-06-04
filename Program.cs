using Pharmacie2.Models;
using Microsoft.EntityFrameworkCore;

namespace Pharmacie2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (var context = new AppDbContext())
            {
                // Crée la base si elle n'existe pas
                context.Database.Migrate();

                bool firstLaunch = !context.Users.Any();

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
}