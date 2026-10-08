using System.Globalization;

namespace Pharmacie2.Services
{
    /// <summary>Formats d'affichage uniques pour toute l'application (écrans, PDF, Excel).</summary>
    public static class Format
    {
        private static readonly NumberFormatInfo _nombre = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ",",
            NumberGroupSizes = new[] { 3 },
            NegativeSign = "-"
        };

        public const string Unite = "KMF";

        /// <summary>« 12 500 KMF » : aucune décimale, espace pour les milliers.</summary>
        public static string Montant(decimal montant) => Nombre(montant) + " " + Unite;

        public static string Montant(decimal? montant) => montant.HasValue ? Montant(montant.Value) : "—";

        /// <summary>« 12 500 » : même format, sans l'unité (colonnes dont l'en-tête indique déjà KMF).</summary>
        public static string Nombre(decimal valeur)
            => Math.Round(valeur, 0, MidpointRounding.AwayFromZero).ToString("N0", _nombre);

        /// <summary>« +12 % » / « -5 % » / « 0 % » (arrondi à l'entier).</summary>
        public static string Pourcentage(decimal valeur)
        {
            decimal r = Math.Round(valeur, 0, MidpointRounding.AwayFromZero);
            return (r > 0 ? "+" : "") + r.ToString("0", CultureInfo.InvariantCulture) + " %";
        }

        /// <summary>
        /// Évolution par rapport à une valeur précédente : « +12 % », « -5 % », « = » si identique,
        /// « nouveau » si on part de zéro, « — » si les deux sont nuls.
        /// </summary>
        public static string Evolution(decimal actuel, decimal precedent)
        {
            if (precedent == 0)
                return actuel == 0 ? "—" : "nouveau";
            return Pourcentage((actuel - precedent) / Math.Abs(precedent) * 100m);
        }

        /// <summary>Signe de l'évolution : 1 hausse, -1 baisse, 0 stable ou non comparable.</summary>
        public static int Tendance(decimal actuel, decimal precedent)
        {
            if (precedent == 0) return actuel > 0 ? 1 : 0;
            decimal pct = Math.Round((actuel - precedent) / Math.Abs(precedent) * 100m, 0, MidpointRounding.AwayFromZero);
            return pct > 0 ? 1 : pct < 0 ? -1 : 0;
        }
    }
}
