using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>Une commande fournisseur et ce qu'il reste à payer dessus.</summary>
    public sealed record DetteCommande(int CommandeId, int FournisseurId, string Fournisseur, DateTime Date, decimal Total, decimal Rembourse, decimal Reste, string Etat);

    /// <summary>
    /// Remboursement des commandes fournisseurs. Le coût des médicaments est compté au moment de la vente (marge). Une dépense
    /// « Remboursement commande » est une note reliée à sa commande : elle réduit le reste à payer et n'entre JAMAIS dans le bénéfice.
    /// Les dépenses sont payées avec la trésorerie de la pharmacie : elles ne touchent jamais à une caisse.
    /// </summary>
    public static class RemboursementCommandeService
    {
        public const string Categorie = "Remboursement commande";

        public const string EtatNonPayee = "Non payée";
        public const string EtatPartielle = "Partiellement payée";
        public const string EtatPayee = "Payée";

        /// <summary>Dépenses qui entrent dans les calculs de résultat (tout sauf les remboursements de commandes).</summary>
        public static IQueryable<DepenseAnnexe> HorsRemboursements(this IQueryable<DepenseAnnexe> depenses)
            => depenses.Where(d => d.Categorie != Categorie);

        public static bool EstRemboursement(DepenseAnnexe d) => d.Categorie == Categorie;

        /// <summary>Total déjà remboursé pour une commande.</summary>
        public static decimal Rembourse(AppDbContext ctx, int commandeId)
            => ctx.DepensesAnnexes.AsNoTracking().Where(d => d.Categorie == Categorie && d.CommandeId == commandeId)
                .Select(d => d.Montant).ToList().Sum();

        /// <summary>
        /// Reste à payer = 0 si la commande est payée à la commande (ou annulée), sinon total − remboursements reliés.
        /// </summary>
        public static (decimal Reste, string Etat) Calculer(Commande c, decimal rembourse)
        {
            if (c.PayeeALaCommande || c.Statut == "Annulée") return (0m, c.Statut == "Annulée" ? "—" : EtatPayee);
            decimal reste = Math.Max(0m, c.MontantTotal - rembourse);
            string etat = reste <= 0m ? EtatPayee : rembourse > 0m ? EtatPartielle : EtatNonPayee;
            return (reste, etat);
        }

        /// <summary>Toutes les commandes avec leur reste à payer (les plus récentes d'abord).</summary>
        public static List<DetteCommande> Commandes(AppDbContext ctx, int? fournisseurId = null)
        {
            var remboursements = ctx.DepensesAnnexes.AsNoTracking().Where(d => d.Categorie == Categorie && d.CommandeId != null)
                .Select(d => new { d.CommandeId, d.Montant }).ToList()
                .GroupBy(d => d.CommandeId!.Value).ToDictionary(g => g.Key, g => g.Sum(x => x.Montant));
            var requete = ctx.commandes.AsNoTracking().Include(c => c.Fournisseur).Include(c => c.Lignes).AsQueryable();
            if (fournisseurId != null) requete = requete.Where(c => c.FournisseurId == fournisseurId);
            return requete.OrderByDescending(c => c.DateCommande).ToList().Select(c =>
            {
                decimal rembourse = remboursements.TryGetValue(c.Id, out var r) ? r : 0m;
                var (reste, etat) = Calculer(c, rembourse);
                return new DetteCommande(c.Id, c.FournisseurId, c.Fournisseur?.Nom ?? "—", c.DateCommande, c.MontantTotal, rembourse, reste, etat);
            }).ToList();
        }

        /// <summary>Commandes non soldées (reste à payer &gt; 0).</summary>
        public static List<DetteCommande> NonSoldees(AppDbContext ctx, int? fournisseurId = null)
            => Commandes(ctx, fournisseurId).Where(c => c.Reste > 0m).ToList();

        /// <summary>Dettes fournisseurs : somme des restes à payer.</summary>
        public static decimal DettesFournisseurs(AppDbContext ctx) => NonSoldees(ctx).Sum(c => c.Reste);

        /// <summary>Contrôle la saisie d'un remboursement : commande non soldée et montant au plus égal au reste à payer. Null = valide.</summary>
        public static string? ControlerRemboursement(AppDbContext ctx, int? commandeId, decimal montant)
        {
            if (commandeId == null) return "Choisissez la commande remboursée.";
            var commande = Commandes(ctx).FirstOrDefault(c => c.CommandeId == commandeId);
            if (commande == null) return "Commande introuvable.";
            if (commande.Reste <= 0m) return "Cette commande est déjà soldée : il n'y a plus rien à rembourser.";
            if (montant > commande.Reste)
                return $"Le montant dépasse le reste à payer de cette commande ({Format.Montant(commande.Reste)}).";
            return null;
        }

        /// <summary>Message de refus si la commande a déjà reçu des remboursements (elle ne peut plus être annulée), sinon null.</summary>
        public static string? RefusAnnulation(AppDbContext ctx, int commandeId)
            => Rembourse(ctx, commandeId) > 0m
                ? "Cette commande a déjà été remboursée en partie : elle ne peut plus être annulée. Supprimez d'abord les remboursements saisis dans Dépenses."
                : null;
    }
}
