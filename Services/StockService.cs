using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Seul endroit où se font les conversions boîtes ↔ unités.
    /// Produit.QuantiteEnStock est TOUJOURS en unités de base.
    /// </summary>
    public static class StockService
    {
        public const string UniteBoite = "Boîte";

        private static int NbParBoite(Produit p) => Math.Max(1, p.NbUniteParBoite);

        /// <summary>Convertit une quantité saisie (en boîtes ou en unités) en unités de base.</summary>
        public static int EnUnites(Produit p, int quantite, string unite)
            => unite == UniteBoite ? quantite * NbParBoite(p) : quantite;

        public static bool EstDisponible(Produit p, int unites) => p.QuantiteEnStock >= unites;

        /// <summary>Retire du stock. Lève une exception si le stock est insuffisant (jamais d'écrêtage).</summary>
        public static void Retirer(Produit p, int unites)
        {
            if (!EstDisponible(p, unites))
                throw new StockInsuffisantException(
                    $"Stock insuffisant pour « {p.Nom} ».\nDisponible : {Formater(p)}");
            p.QuantiteEnStock -= unites;
        }

        public static void Ajouter(Produit p, int unites) => p.QuantiteEnStock += unites;

        /// <summary>Ex : « 2 boîte(s) + 4 plaquette(s) ».</summary>
        public static string Formater(Produit p) => Formater(p, p.QuantiteEnStock);

        public static string Formater(Produit p, int unites)
        {
            int nb = NbParBoite(p);
            if (nb <= 1)
                return $"{Format.Compte(unites, "boîte")}";

            int boites = unites / nb;
            int reste = unites % nb;
            string unite = string.IsNullOrWhiteSpace(p.UniteVente) ? "unité" : p.UniteVente.Trim().ToLowerInvariant();

            if (boites == 0 && reste == 0) return "0 boîte";
            if (reste == 0) return $"{Format.Compte(boites, "boîte")}";
            if (boites == 0) return Format.Compte(reste, unite);
            return $"{Format.Compte(boites, "boîte")} + {Format.Compte(reste, unite)}";
        }

        /// <summary>Seuil d'alerte (saisi en boîtes) converti en unités.</summary>
        public static int SeuilEnUnites(Produit p) => p.SeuilAlerte * NbParBoite(p);
    }
}
