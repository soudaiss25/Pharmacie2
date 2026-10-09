using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace Pharmacie2.Services
{
    public class PhraseSecreteIncorrecteException : Exception
    {
        public PhraseSecreteIncorrecteException() : base("La phrase secrète est incorrecte, ou le fichier est abîmé.") { }
    }

    public class FichierSauvegardeInvalideException : Exception
    {
        public FichierSauvegardeInvalideException(string message) : base(message) { }
    }

    /// <summary>
    /// Copie CHIFFRÉE (AES-256-GCM, clé PBKDF2 d'une phrase secrète) de la sauvegarde du jour vers un dossier externe
    /// (Google Drive pour ordinateur, clé USB), et restauration. La phrase secrète n'est jamais écrite ni journalisée :
    /// la clé dérivée est gardée protégée par Windows (DPAPI, compte de l'utilisateur) pour les copies automatiques.
    /// </summary>
    public static class SauvegardeExterneService
    {
        public const int NbCopiesGardees = 30;
        public const int JoursAvantAlerte = 7;
        public const string Extension = ".sqlite.chiffre";
        private const int Iterations = 200_000;
        private static readonly byte[] Magique = Encoding.ASCII.GetBytes("LGPHBK01");

        private static string CheminCle => Path.Combine(CheminsApp.DossierDonnees, "cle-sauvegarde.bin");

        public static bool EstConfiguree
            => ParametresApp.Actuels.DossierSauvegardeExterne is { Length: > 0 } && ParametresApp.Actuels.SelSauvegarde is { Length: > 0 } && File.Exists(CheminCle);

        // ── Chiffrement ───────────────────────────────────────────────────

        public static byte[] DeriverCle(string phrase, byte[] sel, int iterations = Iterations)
            => Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(phrase), sel, iterations, HashAlgorithmName.SHA256, 32);

        /// <summary>Format : magique(8) | itérations(4) | sel(16) | nonce(12) | étiquette(16) | données chiffrées.</summary>
        public static void ChiffrerFichier(string source, string destination, byte[] cle, byte[] sel, int iterations = Iterations)
        {
            byte[] clair = File.ReadAllBytes(source);
            byte[] nonce = RandomNumberGenerator.GetBytes(12);
            byte[] chiffre = new byte[clair.Length];
            byte[] etiquette = new byte[16];
            using (var aes = new AesGcm(cle, 16))
                aes.Encrypt(nonce, clair, chiffre, etiquette);

            using var flux = File.Create(destination);
            flux.Write(Magique);
            flux.Write(BitConverter.GetBytes(iterations));
            flux.Write(sel);
            flux.Write(nonce);
            flux.Write(etiquette);
            flux.Write(chiffre);
        }

        public static void DechiffrerFichier(string source, string destination, string phrase)
        {
            byte[] tout = File.ReadAllBytes(source);
            int entete = Magique.Length + 4 + 16 + 12 + 16;
            if (tout.Length < entete || !tout.AsSpan(0, Magique.Length).SequenceEqual(Magique))
                throw new FichierSauvegardeInvalideException("Ce fichier n'est pas une sauvegarde chiffrée LIGUAPHARME.");

            int iterations = BitConverter.ToInt32(tout, Magique.Length);
            byte[] sel = tout.AsSpan(Magique.Length + 4, 16).ToArray();
            byte[] nonce = tout.AsSpan(Magique.Length + 20, 12).ToArray();
            byte[] etiquette = tout.AsSpan(Magique.Length + 32, 16).ToArray();
            byte[] chiffre = tout.AsSpan(entete).ToArray();
            byte[] clair = new byte[chiffre.Length];
            try
            {
                using var aes = new AesGcm(DeriverCle(phrase, sel, iterations), 16);
                aes.Decrypt(nonce, chiffre, etiquette, clair);
            }
            catch (CryptographicException)
            {
                throw new PhraseSecreteIncorrecteException();
            }
            File.WriteAllBytes(destination, clair);
        }

        // ── Configuration ─────────────────────────────────────────────────

        /// <summary>Définit le dossier externe et la phrase secrète (la clé dérivée est protégée par Windows).</summary>
        public static void Configurer(string dossier, string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase) || phrase.Length < 8)
                throw new ArgumentException("La phrase secrète doit contenir au moins 8 caractères.");
            byte[] sel = RandomNumberGenerator.GetBytes(16);
            byte[] cle = DeriverCle(phrase, sel);
            File.WriteAllBytes(CheminCle, ProtectedData.Protect(cle, null, DataProtectionScope.CurrentUser));
            ParametresApp.Modifier(p => { p.DossierSauvegardeExterne = dossier; p.SelSauvegarde = Convert.ToBase64String(sel); });
        }

        public static void ChangerDossier(string dossier) => ParametresApp.Modifier(p => p.DossierSauvegardeExterne = dossier);

        /// <summary>Dossier « Mon Drive » / « My Drive » de Google Drive pour ordinateur, s'il existe sur un lecteur.</summary>
        public static string? DetecterGoogleDrive()
        {
            try
            {
                foreach (var lecteur in DriveInfo.GetDrives())
                {
                    if (!lecteur.IsReady) continue;
                    foreach (var nom in new[] { "Mon Drive", "My Drive" })
                    {
                        string chemin = Path.Combine(lecteur.RootDirectory.FullName, nom);
                        if (Directory.Exists(chemin)) return chemin;
                    }
                }
                string profil = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                foreach (var nom in new[] { "Mon Drive", "My Drive", "Google Drive" })
                {
                    string chemin = Path.Combine(profil, nom);
                    if (Directory.Exists(chemin)) return chemin;
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Détection de Google Drive", ex);
            }
            return null;
        }

        // ── Copie quotidienne ─────────────────────────────────────────────

        /// <summary>
        /// Copie chiffrée de la sauvegarde locale du jour vers le dossier externe (fichier temporaire puis renommage),
        /// puis rotation à 30 copies. Ne lève jamais d'exception : renvoie faux si le dossier est absent ou la copie impossible.
        /// </summary>
        public static bool CopierDuJour(DateTime? maintenant = null, bool forcer = false)
        {
            try
            {
                if (!EstConfiguree) return false;
                var jour = (maintenant ?? DateTime.Now).Date;
                string dossier = ParametresApp.Actuels.DossierSauvegardeExterne!;
                if (!Directory.Exists(dossier))
                {
                    Journal.Info("Dossier de sauvegarde externe absent (clé débranchée ?) : nouvel essai au prochain démarrage.");
                    return false;
                }

                string locale = Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_{jour:yyyy-MM-dd}.sqlite");
                if (!File.Exists(locale)) return false;

                string destination = Path.Combine(dossier, $"PharmacieDB_{jour:yyyy-MM-dd}{Extension}");
                if (File.Exists(destination) && !forcer)
                    return true;   // déjà copiée aujourd'hui

                byte[] cle = ProtectedData.Unprotect(File.ReadAllBytes(CheminCle), null, DataProtectionScope.CurrentUser);
                byte[] sel = Convert.FromBase64String(ParametresApp.Actuels.SelSauvegarde!);
                string temporaire = destination + ".tmp";
                try
                {
                    ChiffrerFichier(locale, temporaire, cle, sel);
                    File.Move(temporaire, destination, overwrite: true);
                }
                catch
                {
                    try { if (File.Exists(temporaire)) File.Delete(temporaire); } catch { }
                    throw;
                }

                ParametresApp.Modifier(p => p.DerniereCopieExterne = DateTime.Now);
                Journal.Info("Copie chiffrée envoyée vers le dossier externe : " + Path.GetFileName(destination));
                Purger(dossier);
                return true;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Copie vers le dossier de sauvegarde externe impossible", ex);
                return false;
            }
        }

        /// <summary>Garde les 30 copies chiffrées les plus récentes.</summary>
        public static void Purger(string dossier)
        {
            var anciennes = new DirectoryInfo(dossier).GetFiles("PharmacieDB_????-??-??" + Extension)
                .OrderByDescending(f => f.Name).Skip(NbCopiesGardees);
            foreach (var f in anciennes)
            {
                try { f.Delete(); }
                catch (Exception ex) { Journal.Erreur("Purge de " + f.Name, ex); }
            }
        }

        /// <summary>Jours écoulés depuis la dernière copie externe réussie ; null si aucune copie n'a jamais réussi.</summary>
        public static int? JoursDepuisDerniereCopie(DateTime maintenant)
            => ParametresApp.Actuels.DerniereCopieExterne is DateTime d ? (int)Math.Floor((maintenant - d).TotalDays) : null;

        // ── Restauration ──────────────────────────────────────────────────

        /// <summary>
        /// Remplace la base par une sauvegarde (locale ou externe, chiffrée ou non) : contrôle d'intégrité, sauvegarde de la
        /// base actuelle, remplacement. L'application doit ensuite redémarrer.
        /// </summary>
        public static void Restaurer(string fichier, string? phrase)
        {
            string temporaire = Path.Combine(CheminsApp.DossierSauvegardes, "restauration_" + Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                if (fichier.EndsWith(".chiffre", StringComparison.OrdinalIgnoreCase))
                    DechiffrerFichier(fichier, temporaire, phrase ?? "");
                else
                    File.Copy(fichier, temporaire, true);

                VerifierIntegrite(temporaire);

                if (File.Exists(CheminsApp.CheminBase))
                    SauvegardeAutomatique.Sauvegarder(Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_avant-restauration_{DateTime.Now:yyyy-MM-dd_HHmmss}.sqlite"));

                SqliteConnection.ClearAllPools();
                foreach (var suffixe in new[] { "-wal", "-shm" })
                    if (File.Exists(CheminsApp.CheminBase + suffixe)) File.Delete(CheminsApp.CheminBase + suffixe);
                File.Copy(temporaire, CheminsApp.CheminBase, true);
                Journal.Info("Base restaurée depuis " + Path.GetFileName(fichier));
            }
            finally
            {
                try { if (File.Exists(temporaire)) File.Delete(temporaire); } catch { }
            }
        }

        /// <summary>PRAGMA integrity_check doit répondre « ok » et le fichier doit contenir les tables de la pharmacie.</summary>
        public static void VerifierIntegrite(string fichier)
        {
            try
            {
                using var c = new SqliteConnection($"Data Source={fichier};Mode=ReadOnly;Pooling=False");
                c.Open();
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA integrity_check";
                    if (!string.Equals(Convert.ToString(cmd.ExecuteScalar()), "ok", StringComparison.OrdinalIgnoreCase))
                        throw new FichierSauvegardeInvalideException("La sauvegarde est abîmée (contrôle d'intégrité en échec) : elle n'a pas été utilisée.");
                }
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name IN ('produits','ventes','Users')";
                    if (Convert.ToInt32(cmd.ExecuteScalar()) < 3)
                        throw new FichierSauvegardeInvalideException("Ce fichier n'est pas une sauvegarde de la pharmacie.");
                }
            }
            catch (SqliteException)
            {
                throw new FichierSauvegardeInvalideException("Ce fichier n'est pas une sauvegarde valide.");
            }
        }
    }
}
