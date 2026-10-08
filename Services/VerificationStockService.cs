using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public enum ChoixVerification
    {
        CorrectionAppliquee,
        StockCorrect,
        StockRecompte
    }

    /// <summary>
    /// Produits dont le stock a peut-être été faussé par l'ancienne réception des commandes.
    /// Rien n'est jamais corrigé automatiquement : chaque résolution est un choix explicite de l'utilisateur.
    /// </summary>
    public static class VerificationStockService
    {
        public static int NombreAVerifier()
        {
            using var ctx = new AppDbContext();
            return ctx.produits.Count(p => p.StockAVerifier);
        }

        /// <summary>
        /// Lève le marquage d'un produit. Pour CorrectionAppliquee, ajoute les unités estimées manquantes.
        /// Pour StockRecompte, le nouveau stock a déjà été enregistré par la fiche produit.
        /// Journalise : produit, ancien stock, nouveau stock, choix, utilisateur.
        /// </summary>
        public static void Resoudre(int produitId, ChoixVerification choix)
        {
            using var ctx = new AppDbContext();
            using var tx = ctx.Database.BeginTransaction();

            var p = ctx.produits.Find(produitId)
                ?? throw new InvalidOperationException("Produit introuvable.");

            int ancien = p.QuantiteEnStock;
            if (choix == ChoixVerification.CorrectionAppliquee)
                StockService.Ajouter(p, p.UnitesManquantesEstimees);

            p.StockAVerifier = false;
            p.UnitesManquantesEstimees = 0;
            ctx.SaveChanges();
            tx.Commit();

            Tracer(p, ancien, choix);
        }

        /// <summary>Journalise une levée de marquage faite lors d'un enregistrement de la fiche (stock modifié).</summary>
        public static void Tracer(Produit p, int ancienStock, ChoixVerification choix)
            => Journal.Info(
                $"Vérification de stock : produit « {p.Nom} » (id {p.Id}), ancien stock {ancienStock}, " +
                $"nouveau stock {p.QuantiteEnStock}, choix « {choix} », utilisateur {SessionUtilisateur.NomComplet}.");
    }
}
