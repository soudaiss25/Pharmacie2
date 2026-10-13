using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Marquage « fiche à vérifier » d'un produit (prix, péremption…). Un rapport se lit une fois ; le badge reste visible tant que
    /// le problème n'est pas réglé.
    /// </summary>
    public static class FicheAVerifierService
    {
        public const string PrefixePrix = "Prix";
        public const string PrefixePeremption = "Péremption";
        private const string Separateur = " ; ";

        public static List<string> Motifs(Produit p)
            => string.IsNullOrWhiteSpace(p.MotifsFiche)
                ? new List<string>()
                : p.MotifsFiche.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        /// <summary>Ajoute un motif (jamais en double) et marque la fiche. Le motif ne doit pas contenir de point-virgule.</summary>
        public static bool Marquer(Produit p, string motif)
        {
            motif = motif.Replace(';', ',').Trim();
            var motifs = Motifs(p);
            if (p.FicheAVerifier && motifs.Contains(motif)) return false;
            if (!motifs.Contains(motif)) motifs.Add(motif);
            p.MotifsFiche = string.Join(Separateur, motifs);
            p.FicheAVerifier = true;
            return true;
        }

        /// <summary>Lève les motifs qui commencent par <paramref name="prefixe"/> (tous si null) ; la fiche n'est plus marquée s'il n'en reste aucun.</summary>
        public static bool Lever(Produit p, string? prefixe = null)
        {
            if (!p.FicheAVerifier && string.IsNullOrEmpty(p.MotifsFiche)) return false;
            var restants = prefixe == null ? new List<string>() : Motifs(p).Where(m => !m.StartsWith(prefixe, StringComparison.OrdinalIgnoreCase)).ToList();
            bool change = restants.Count != Motifs(p).Count || (restants.Count == 0 && p.FicheAVerifier);
            p.MotifsFiche = restants.Count == 0 ? null : string.Join(Separateur, restants);
            p.FicheAVerifier = restants.Count > 0;
            return change;
        }

        public static bool MotifPrix(Produit p) => p.FicheAVerifier && Motifs(p).Any(m => m.StartsWith(PrefixePrix, StringComparison.OrdinalIgnoreCase));

        /// <summary>Texte d'avertissement à afficher avant d'ajouter au panier un produit dont le prix est à vérifier (null sinon). La vente reste possible.</summary>
        public static string? AvertissementPrix(Produit p, decimal prixApplique)
        {
            if (!MotifPrix(p)) return null;
            var motifs = Motifs(p).Where(m => m.StartsWith(PrefixePrix, StringComparison.OrdinalIgnoreCase)).Select(m => "• " + m);
            return $"Prix à vérifier : {Format.Montant(prixApplique)} pour « {p.Nom} »." + Environment.NewLine + Environment.NewLine +
                   string.Join(Environment.NewLine, motifs) + Environment.NewLine + Environment.NewLine +
                   "La vente reste possible. Faites vérifier la fiche du produit.";
        }

        public static int Nombre(AppDbContext ctx) => ctx.produits.Count(p => p.FicheAVerifier && p.Actif);

        /// <summary>Bouton « J'ai vérifié cette fiche » : lève tous les motifs, journalisé.</summary>
        public static void MarquerVerifiee(int produitId)
        {
            using var ctx = new AppDbContext();
            var p = ctx.produits.Find(produitId);
            if (p == null) return;
            string avant = p.MotifsFiche ?? "";
            if (Lever(p))
            {
                ctx.SaveChanges();
                Journal.Info($"Fiche produit « {p.Nom} » vérifiée par l'utilisateur (motifs levés : {avant})");
            }
        }
    }
}
