using System.Security.Cryptography;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public class CleSecoursInvalideException : InvalidOperationException
    {
        public CleSecoursInvalideException(string message) : base(message) { }
    }

    public class CleSecoursBloqueeException : InvalidOperationException
    {
        public CleSecoursBloqueeException(string message) : base(message) { }
    }

    /// <summary>
    /// Clé de secours des administrateurs : 4 groupes de 4 caractères sans caractères ambigus.
    /// Seul le hash BCrypt est conservé. La clé n'est jamais écrite dans le journal.
    /// </summary>
    public static class CleSecoursService
    {
        public const int EssaisMax = 5;
        public static readonly TimeSpan DureeBlocage = TimeSpan.FromMinutes(15);

        // Sans 0/O ni 1/I/L
        private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

        /// <summary>Horloge remplaçable (tests uniquement).</summary>
        public static Func<DateTime> Maintenant { get; set; } = () => DateTime.Now;

        // Essais pour les logins inconnus (non enregistrés en base) : même comportement, sans fuite d'information
        private static readonly Dictionary<string, (int essais, DateTime? bloqueJusqua)> _inconnus = new();
        private static readonly object _verrou = new();

        /// <summary>Ex : « K7MQ-4XRT-9WPA-H3ZD ».</summary>
        public static string Generer()
        {
            var groupes = new string[4];
            for (int g = 0; g < 4; g++)
            {
                var c = new char[4];
                for (int i = 0; i < 4; i++)
                    c[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
                groupes[g] = new string(c);
            }
            return string.Join("-", groupes);
        }

        /// <summary>Majuscules, sans tirets ni espaces.</summary>
        public static string Normaliser(string cle)
            => new string((cle ?? "").Where(char.IsLetterOrDigit).Select(char.ToUpperInvariant).ToArray());

        public static bool AUneCle(int userId)
        {
            using var ctx = new AppDbContext();
            return ctx.Users.Where(u => u.Id == userId).Select(u => u.CleSecoursHash).FirstOrDefault() != null;
        }

        /// <summary>Enregistre le hash de la clé (l'ancienne devient invalide) et remet les compteurs à zéro.</summary>
        public static void Definir(int userId, string cle)
        {
            using var ctx = new AppDbContext();
            var u = ctx.Users.Find(userId) ?? throw new InvalidOperationException("Utilisateur introuvable.");
            u.CleSecoursHash = MotDePasseService.Hacher(Normaliser(cle));
            u.EssaisCleEchoues = 0;
            u.BlocageCleJusqua = null;
            ctx.SaveChanges();
            Journal.Info($"Clé de secours (re)définie pour le compte « {u.Login} » (la clé n'est pas enregistrée).");
        }

        /// <summary>
        /// Réinitialise le mot de passe d'un administrateur avec sa clé de secours. L'ancienne clé devient
        /// invalide : l'appelant doit ensuite générer, afficher puis enregistrer (Definir) une nouvelle clé.
        /// </summary>
        /// <returns>Id de l'utilisateur.</returns>
        public static int Reinitialiser(string login, string cle, string nouveauMotDePasse)
        {
            login = (login ?? "").Trim();
            nouveauMotDePasse = (nouveauMotDePasse ?? "").Trim();
            if (nouveauMotDePasse.Length == 0)
                throw new InvalidOperationException("Le nouveau mot de passe ne peut pas être vide.");

            var maintenant = Maintenant();
            using var ctx = new AppDbContext();
            var u = ctx.Users.FirstOrDefault(x => x.Login == login && x.Role == Roles.Administrateur && x.Actif);

            if (u == null)
            {
                EchecInconnu(login, maintenant);   // lève toujours
                throw new CleSecoursInvalideException("Login ou clé de secours incorrect.");
            }

            if (u.BlocageCleJusqua != null)
            {
                if (u.BlocageCleJusqua > maintenant)
                    throw Bloquee(u.BlocageCleJusqua.Value, maintenant);

                u.BlocageCleJusqua = null;   // blocage expiré
                u.EssaisCleEchoues = 0;
            }

            if (!MotDePasseService.Verifier(Normaliser(cle), u.CleSecoursHash))
            {
                u.EssaisCleEchoues++;
                if (u.EssaisCleEchoues >= EssaisMax)
                {
                    u.BlocageCleJusqua = maintenant + DureeBlocage;
                    u.EssaisCleEchoues = 0;
                    ctx.SaveChanges();
                    Journal.Info($"Clé de secours : {EssaisMax} essais erronés pour le compte « {u.Login} », fonction bloquée 15 minutes.");
                    throw Bloquee(u.BlocageCleJusqua.Value, maintenant);
                }
                ctx.SaveChanges();
                Journal.Info($"Clé de secours erronée pour le compte « {u.Login} » ({u.EssaisCleEchoues}/{EssaisMax}).");
                throw new CleSecoursInvalideException(
                    $"Login ou clé de secours incorrect. Essais restants : {EssaisMax - u.EssaisCleEchoues}.");
            }

            u.MotDePasse = MotDePasseService.Hacher(nouveauMotDePasse);
            u.CleSecoursHash = null;   // l'ancienne clé est consommée
            u.EssaisCleEchoues = 0;
            u.BlocageCleJusqua = null;
            ctx.SaveChanges();
            Journal.Info($"Mot de passe du compte « {u.Login} » réinitialisé avec la clé de secours.");
            return u.Id;
        }

        private static void EchecInconnu(string login, DateTime maintenant)
        {
            string k = login.ToLowerInvariant();
            lock (_verrou)
            {
                _inconnus.TryGetValue(k, out var etat);
                if (etat.bloqueJusqua != null)
                {
                    if (etat.bloqueJusqua > maintenant)
                        throw Bloquee(etat.bloqueJusqua.Value, maintenant);
                    etat = (0, null);
                }

                etat.essais++;
                if (etat.essais >= EssaisMax)
                {
                    _inconnus[k] = (0, maintenant + DureeBlocage);
                    Journal.Info($"Clé de secours : {EssaisMax} essais pour un login inconnu « {login} », fonction bloquée 15 minutes.");
                    throw Bloquee(maintenant + DureeBlocage, maintenant);
                }
                _inconnus[k] = etat;
                throw new CleSecoursInvalideException(
                    $"Login ou clé de secours incorrect. Essais restants : {EssaisMax - etat.essais}.");
            }
        }

        /// <summary>Tests uniquement.</summary>
        public static void OublierLesLoginsInconnus() { lock (_verrou) _inconnus.Clear(); }

        private static CleSecoursBloqueeException Bloquee(DateTime jusqua, DateTime maintenant)
        {
            int minutes = Math.Max(1, (int)Math.Ceiling((jusqua - maintenant).TotalMinutes));
            return new CleSecoursBloqueeException($"Trop d'essais. Cette fonction est bloquée, réessayez dans {minutes} minute(s).");
        }
    }
}
