using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Suivi de la migration RepriseVentesPayeesEnTotalite : au premier démarrage après la migration, on journalise
    /// combien de ventes ont été réparées et leurs numéros (jamais le nom d'un client).
    /// </summary>
    public static class RepriseVentesService
    {
        public const string Migration = "RepriseVentesPayeesEnTotalite";
        public const string Prefixe = "P-REPRISE-";

        /// <summary>Vrai si la migration de reprise n'est pas encore appliquée à cette base (à lire AVANT Migrate()).</summary>
        public static bool EstEnAttente(AppDbContext ctx)
            => ctx.Database.GetPendingMigrations().Any(m => m.EndsWith("_" + Migration));

        /// <summary>Numéros de vente (V-000123) des ventes réparées par la reprise.</summary>
        public static List<string> NumerosRepares(AppDbContext ctx)
            => ctx.paiement.AsNoTracking()
                .Where(p => p.NumeroPaiement.StartsWith(Prefixe))
                .Select(p => p.Vente.numeroVente)
                .OrderBy(n => n)
                .ToList();

        public static void Journaliser(AppDbContext ctx)
        {
            var numeros = NumerosRepares(ctx);
            Journal.Info(numeros.Count == 0
                ? "Reprise des ventes payées en totalité : aucune vente à réparer."
                : $"Reprise des ventes payées en totalité : {numeros.Count} vente(s) réparée(s) (paiement manquant ajouté) : {string.Join(", ", numeros)}");
        }
    }
}
