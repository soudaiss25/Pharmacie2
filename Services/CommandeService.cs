using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Réception des commandes fournisseurs : une seule implémentation pour tous les écrans.
    /// Les quantités commandées/reçues sont en BOÎTES ; le stock est en unités de base.
    /// </summary>
    public static class CommandeService
    {
        public const string MessageQuantiteInconnue =
            "Quantité déjà reçue inconnue, vérifiez avant de réceptionner.";

        /// <summary>
        /// Commande « Reçu partiellement » d'avant la mémorisation des quantités reçues
        /// (aucune quantité reçue enregistrée) : la réception totale automatique y est interdite.
        /// </summary>
        public static bool EstPartielleHeritee(int commandeId)
        {
            using var ctx = new AppDbContext();
            var c = ctx.commandes.AsNoTracking().Include(x => x.Lignes).FirstOrDefault(x => x.Id == commandeId);
            return c != null && c.Statut == "Reçu partiellement" && c.Lignes.Sum(l => l.QuantiteRecue) == 0;
        }

        /// <summary>
        /// Réceptionne une commande dans une transaction.
        /// </summary>
        /// <param name="commandeId">Commande concernée.</param>
        /// <param name="quantitesParLigne">
        /// Boîtes reçues cette fois, par Id de ligne de commande.
        /// null = tout le reste à recevoir de chaque ligne.
        /// </param>
        /// <returns>Nombre total de boîtes réceptionnées par cet appel.</returns>
        public static int Receptionner(int commandeId, IDictionary<int, int>? quantitesParLigne = null)
        {
            using var ctx = new AppDbContext();
            using var tx = ctx.Database.BeginTransaction();

            var commande = ctx.commandes
                .Include(c => c.Lignes)
                .FirstOrDefault(c => c.Id == commandeId)
                ?? throw new InvalidOperationException("Commande introuvable.");

            if (commande.Statut != "En attente" && commande.Statut != "Reçu partiellement")
                throw new InvalidOperationException($"Cette commande est déjà « {commande.Statut} » : réception impossible.");

            if (quantitesParLigne == null && commande.Statut == "Reçu partiellement"
                && commande.Lignes.Sum(l => l.QuantiteRecue) == 0)
                throw new InvalidOperationException(
                    MessageQuantiteInconnue + "\n\nCette commande doit être réceptionnée manuellement, ligne par ligne " +
                    "(écran Commandes du fournisseur → « Réception partielle »).");

            int totalBoites = 0;
            foreach (var ligne in commande.Lignes)
            {
                int reste = Math.Max(0, ligne.Quantite - ligne.QuantiteRecue);

                int demande = reste;
                if (quantitesParLigne != null && !quantitesParLigne.TryGetValue(ligne.Id, out demande))
                    continue;

                int aRecevoir = Math.Min(demande, reste);   // plafonné au reste à recevoir
                if (aRecevoir <= 0) continue;

                var produit = ctx.produits.Find(ligne.ProduitId)
                    ?? throw new InvalidOperationException("Un produit de la commande n'existe plus.");

                StockService.Ajouter(produit, StockService.EnUnites(produit, aRecevoir, StockService.UniteBoite));
                ligne.QuantiteRecue += aRecevoir;
                totalBoites += aRecevoir;
            }

            if (totalBoites == 0)
                return 0;   // rien à enregistrer

            commande.Statut = commande.Lignes.All(l => l.QuantiteRecue >= l.Quantite) ? "Reçu" : "Reçu partiellement";
            commande.DateReception = DateTime.Now;

            ctx.SaveChanges();
            tx.Commit();
            return totalBoites;
        }
    }
}
