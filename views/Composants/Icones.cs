using System.Reflection;

namespace Pharmacie2.views.Composants
{
    /// <summary>Icônes monochromes du menu (PNG blanc sur transparent), incluses en ressources de l'application.</summary>
    public static class Icones
    {
        private static readonly Dictionary<string, Image?> _cache = new();

        public static Image? Charger(string nom)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(nom, out var deja)) return deja;
                Image? image = null;
                try
                {
                    using var flux = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Pharmacie2.icones.{nom}.png");
                    if (flux != null)
                    {
                        using var brut = new Bitmap(flux);
                        image = new Bitmap(brut);   // copie : le flux peut être libéré
                    }
                }
                catch { /* une icône manquante ne doit jamais empêcher l'ouverture du menu */ }
                _cache[nom] = image;
                return image;
            }
        }
    }
}
