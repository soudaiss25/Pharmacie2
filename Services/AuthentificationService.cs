using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public enum ResultatConnexion
    {
        Succes,
        Incorrect,
        CompteArchive
    }

    public static class AuthentificationService
    {
        /// <summary>Recherche par login uniquement, puis vérification du hash. Les entrées sont « trimées ».</summary>
        public static ResultatConnexion Authentifier(string login, string motDePasse, out User? utilisateur)
        {
            utilisateur = null;
            login = (login ?? "").Trim();
            motDePasse = (motDePasse ?? "").Trim();

            using var ctx = new AppDbContext();
            var u = ctx.Users.FirstOrDefault(x => x.Login == login);

            if (u == null || !MotDePasseService.Verifier(motDePasse, u.MotDePasse))
                return ResultatConnexion.Incorrect;

            if (!u.Actif)
                return ResultatConnexion.CompteArchive;

            utilisateur = u;
            return ResultatConnexion.Succes;
        }
    }
}
