using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public enum TypeElement
    {
        Produit,
        Fournisseur,
        Mutuelle,
        Utilisateur
    }

    /// <summary>
    /// On n'efface jamais un enregistrement qui a un historique : on l'archive (Actif = false).
    /// La suppression réelle n'est possible que s'il n'a aucun historique.
    /// </summary>
    public static class ArchivageService
    {
        public static bool AHistorique(TypeElement type, int id)
        {
            using var ctx = new AppDbContext();
            return type switch
            {
                TypeElement.Produit => ctx.LigneVentes.Any(l => l.ProduitId == id)
                                       || ctx.LigneCommandes.Any(l => l.ProduitId == id),
                TypeElement.Fournisseur => ctx.commandes.Any(c => c.FournisseurId == id)
                                           || ctx.produits.Any(p => p.FournisseurId == id),
                TypeElement.Mutuelle => ctx.ventes.Any(v => v.MutuelId == id)
                                        || ctx.MutuelPaiements.Any(m => m.MutuelId == id),
                TypeElement.Utilisateur => ctx.ventes.Any(v => v.UserId == id)
                                           || ctx.paiement.Any(p => p.UserId == id)
                                           || ctx.MutuelPaiements.Any(m => m.UserId == id)
                                           || ctx.SessionsCaisse.Any(s => s.UserId == id)
                                           || ctx.DepensesAnnexes.Any(d => d.UserId == id),
                _ => true
            };
        }

        public static bool EstActif(TypeElement type, int id)
        {
            using var ctx = new AppDbContext();
            return type switch
            {
                TypeElement.Produit => ctx.produits.Where(x => x.Id == id).Select(x => x.Actif).FirstOrDefault(),
                TypeElement.Fournisseur => ctx.fournisseur.Where(x => x.Id == id).Select(x => x.Actif).FirstOrDefault(),
                TypeElement.Mutuelle => ctx.mutuels.Where(x => x.IdMutuel == id).Select(x => x.Actif).FirstOrDefault(),
                TypeElement.Utilisateur => ctx.Users.Where(x => x.Id == id).Select(x => x.Actif).FirstOrDefault(),
                _ => true
            };
        }

        public static void DefinirActif(TypeElement type, int id, bool actif)
        {
            using var ctx = new AppDbContext();
            switch (type)
            {
                case TypeElement.Produit:
                    (ctx.produits.Find(id) ?? throw Introuvable()).Actif = actif;
                    break;
                case TypeElement.Fournisseur:
                    (ctx.fournisseur.Find(id) ?? throw Introuvable()).Actif = actif;
                    break;
                case TypeElement.Mutuelle:
                    (ctx.mutuels.Find(id) ?? throw Introuvable()).Actif = actif;
                    break;
                case TypeElement.Utilisateur:
                    if (!actif)
                        VerifierArchivageUtilisateur(ctx, id);
                    (ctx.Users.Find(id) ?? throw Introuvable()).Actif = actif;
                    break;
            }
            ctx.SaveChanges();
            Journal.Info($"{(actif ? "Réactivation" : "Archivage")} : {type} id {id} par {SessionUtilisateur.NomComplet}.");
        }

        /// <summary>Suppression réelle : refusée si l'élément a un historique.</summary>
        public static void SupprimerDefinitivement(TypeElement type, int id)
        {
            if (AHistorique(type, id))
                throw new InvalidOperationException("Cet élément a un historique : il ne peut pas être supprimé, seulement archivé.");

            using var ctx = new AppDbContext();
            switch (type)
            {
                case TypeElement.Produit: ctx.produits.Remove(ctx.produits.Find(id) ?? throw Introuvable()); break;
                case TypeElement.Fournisseur: ctx.fournisseur.Remove(ctx.fournisseur.Find(id) ?? throw Introuvable()); break;
                case TypeElement.Mutuelle: ctx.mutuels.Remove(ctx.mutuels.Find(id) ?? throw Introuvable()); break;
                case TypeElement.Utilisateur:
                    VerifierArchivageUtilisateur(ctx, id);
                    ctx.Users.Remove(ctx.Users.Find(id) ?? throw Introuvable());
                    break;
            }
            ctx.SaveChanges();
            Journal.Info($"Suppression définitive : {type} id {id} par {SessionUtilisateur.NomComplet}.");
        }

        /// <summary>On ne peut ni archiver/supprimer son propre compte, ni le dernier administrateur actif.</summary>
        private static void VerifierArchivageUtilisateur(AppDbContext ctx, int id)
        {
            if (id == SessionUtilisateur.IdCourant)
                throw new InvalidOperationException("Vous ne pouvez pas archiver ou supprimer votre propre compte.");

            var u = ctx.Users.Find(id);
            if (u != null && u.Role == Roles.Administrateur && u.Actif
                && !ctx.Users.Any(x => x.Id != id && x.Actif && x.Role == Roles.Administrateur))
                throw new InvalidOperationException("Il doit rester au moins un administrateur actif.");
        }

        private static InvalidOperationException Introuvable() => new("Élément introuvable.");
    }
}
