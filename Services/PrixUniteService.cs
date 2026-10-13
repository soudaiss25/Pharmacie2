using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Suivi de la migration AjoutPrixVenteUnite : au premier démarrage après la migration, on journalise les produits
    /// vendus au détail et le prix à l'unité repris, pour que le client puisse les vérifier et les corriger.
    /// </summary>
    public static class PrixUniteService
    {
        public const string Migration = "AjoutPrixVenteUnite";

        public static bool EstEnAttente(AppDbContext ctx)
            => ctx.Database.GetPendingMigrations().Any(m => m.EndsWith("_" + Migration));

        /// <summary>« Nom : boîte X, unité Y » pour chaque produit vendu au détail.</summary>
        public static List<string> ProduitsRepris(AppDbContext ctx)
            => ctx.produits.AsNoTracking()
                .Where(p => p.NbUniteParBoite > 1 && p.PrixVenteUnite != null)
                .OrderBy(p => p.Nom)
                .ToList()
                .Select(p => $"{p.Nom} : boîte {Format.Montant(p.PrixVente)}, {p.UniteVente.ToLowerInvariant()} {Format.Montant(p.PrixVenteUnite!.Value)} ({p.NbUniteParBoite} par boîte)")
                .ToList();

        public static void Journaliser(AppDbContext ctx)
        {
            var l = ProduitsRepris(ctx);
            Journal.Info(l.Count == 0
                ? "Prix à l'unité : aucun produit vendu au détail."
                : $"Prix à l'unité repris pour {l.Count} produit(s) vendu(s) au détail (à vérifier par le client) :{Environment.NewLine}  " + string.Join(Environment.NewLine + "  ", l));
        }
    }
}
