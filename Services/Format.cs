using System.Globalization;

namespace Pharmacie2.Services
{
    /// <summary>Formats d'affichage uniques pour toute l'application (écrans, PDF, Excel).</summary>
    public static class Format
    {
        private static readonly NumberFormatInfo _nombre = new NumberFormatInfo
        {
            NumberGroupSeparator = "\u00A0",   // espace insécable : « 2 400 » ne se coupe jamais
            NumberDecimalSeparator = ",",
            NumberGroupSizes = new[] { 3 },
            NegativeSign = "-"
        };

        public const string Unite = "KMF";

        /// <summary>« 12 500 KMF » : aucune décimale, espace pour les milliers.</summary>
        public static string Montant(decimal montant) => Nombre(montant) + "\u00A0" + Unite;

        public static string Montant(decimal? montant) => montant.HasValue ? Montant(montant.Value) : "—";

        private static readonly System.Text.RegularExpressions.Regex _nombreUnite = new(
            @"(?<=\d) (?=(?:KMF|mg|g|kg|ml|mL|l|L|cl|µg|mcg|UI|%|jours?|j|h|mois|ans?|comprimés?|gélules?|boîtes?|plaquettes?|flacons?|ampoules?|sachets?|\d{3}\b)(?![\p{L}]))",
            System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// Remplace par une espace insécable l'espace entre un nombre et son unité ou sa devise (« 1 g », « 2 400 KMF », « 30 jours »),
        /// pour qu'un retour à la ligne ne sépare jamais le nombre de son unité.
        /// </summary>
        public static string Insecable(string? texte)
            => string.IsNullOrEmpty(texte) ? "" : _nombreUnite.Replace(texte, "\u00A0");

        /// <summary>« 12 500 » : même format, sans l'unité (colonnes dont l'en-tête indique déjà KMF).</summary>
        public static string Nombre(decimal valeur)
            => Math.Round(valeur, 0, MidpointRounding.AwayFromZero).ToString("N0", _nombre);

        private static readonly System.Text.RegularExpressions.Regex _accord = new(@"(\d+)|(\p{L}+)\(s\)", System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// Texte déjà enregistré avec des « (s) » : accorde les mots avec le dernier nombre rencontré
        /// (« 10 boîte(s) reçue(s) » devient « 10 boîtes reçues », « 1 boîte(s) » devient « 1 boîte »).
        /// </summary>
        public static string Accorder(string texte)
        {
            if (string.IsNullOrEmpty(texte) || !texte.Contains("(s)")) return texte ?? "";
            bool pluriel = false;
            return _accord.Replace(texte, m =>
            {
                if (m.Groups[1].Success) { pluriel = int.Parse(m.Groups[1].Value) > 1; return m.Value; }
                return m.Groups[2].Value + (pluriel ? "s" : "");
            });
        }

        /// <summary>Vrai pluriel français : 0 et 1 au singulier, 2 et plus au pluriel (« 1 boîte », « 2 boîtes »).</summary>
        public static string Pluriel(int n, string singulier, string? pluriel = null)
            => n > 1 ? (pluriel ?? (singulier.EndsWith("s") || singulier.EndsWith("x") ? singulier : singulier + "s")) : singulier;

        /// <summary>« 3 produits », « 1 produit », « 0 produit ».</summary>
        public static string Compte(int n, string singulier, string? pluriel = null) => n + " " + Pluriel(n, singulier, pluriel);

        /// <summary>
        /// Évolution pour une carte : « +12 % », « -5 % », « = », ou le texte donné quand la base de comparaison est nulle
        /// (par exemple « pas de vente hier à la même heure »).
        /// </summary>
        public static string EvolutionOuTexte(decimal actuel, decimal precedent, string sansBase)
            => precedent == 0 ? (actuel == 0 ? "—" : sansBase) : Evolution(actuel, precedent);

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
