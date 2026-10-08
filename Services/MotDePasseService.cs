using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>Hachage BCrypt des mots de passe (et de la clé de secours).</summary>
    public static class MotDePasseService
    {
        private const int Cout = 11;

        public static string Hacher(string motDePasse)
            => BCrypt.Net.BCrypt.HashPassword(motDePasse, Cout);

        /// <summary>false si le mot de passe est faux ou si le hash stocké n'est pas un hash valide.</summary>
        public static bool Verifier(string motDePasse, string? hash)
        {
            if (string.IsNullOrEmpty(hash) || !EstHache(hash))
                return false;
            try
            {
                return BCrypt.Net.BCrypt.Verify(motDePasse, hash);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Vérification d'un mot de passe impossible (hash invalide)", ex);
                return false;
            }
        }

        public static bool EstHache(string valeur) => valeur.StartsWith("$2");

        /// <summary>
        /// Hache tout mot de passe encore en clair (ne commençant pas par « $2 »). Idempotent :
        /// un hash n'est jamais re-haché. Le mot de passe en clair est haché après Trim(), comme à la connexion.
        /// </summary>
        /// <returns>Nombre de comptes convertis.</returns>
        public static int ConvertirMotsDePasseEnClair(AppDbContext ctx)
        {
            var aConvertir = ctx.Users.ToList().Where(u => !EstHache(u.MotDePasse ?? "")).ToList();
            foreach (var u in aConvertir)
                u.MotDePasse = Hacher((u.MotDePasse ?? "").Trim());

            if (aConvertir.Count > 0)
            {
                ctx.SaveChanges();
                Journal.Info($"{aConvertir.Count} mot(s) de passe converti(s) en hash BCrypt.");
            }
            return aConvertir.Count;
        }
    }
}
