using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Prépare un ZIP à envoyer au développeur : journaux des 30 derniers jours (filtrés) et informations techniques.
    /// AUCUNE donnée personnelle ni métier : ni la base, ni noms de clients, ni montants, ni mots de passe, ni clé de secours,
    /// ni phrase secrète. Les journaux sont filtrés ligne par ligne : tout ce qui pourrait identifier quelqu'un est masqué.
    /// </summary>
    public static class RapportDeveloppeurService
    {
        public const int JoursDeJournaux = 30;

        // Éléments masqués dans les journaux
        private static readonly (Regex motif, string remplacement)[] Filtres =
        {
            (new Regex(@"«[^»]*»", RegexOptions.Compiled), "« [masqué] »"),                                          // logins, noms entre guillemets
            (new Regex(@"\bpar [^\r\n]*?\.(?=\s*$)", RegexOptions.Compiled | RegexOptions.Multiline), "par [utilisateur]."),   // « … par Prénom Nom. »
            (new Regex(@"[\w.+-]+@[\w-]+\.[\w.-]+", RegexOptions.Compiled), "[e-mail masqué]"),
            (new Regex(@"(?i)(mot de passe|password|passphrase|phrase secr\S+|cl[ée] de secours)\s*[:=]\s*\S+", RegexOptions.Compiled), "$1 : [masqué]"),
            (new Regex(@"(?i)(?<=\\Users\\)[^\\/\r\n]+", RegexOptions.Compiled), "[utilisateur-windows]"),
        };

        // numéros de téléphone ou d'identification : au moins 7 chiffres d'affilée (espaces ou tirets simples tolérés), hors dates et versions
        private static readonly Regex Numeros = new(@"(?<![\w.:])\+?\d(?:[ -]?\d){6,}(?![\w.:])", RegexOptions.Compiled);

        /// <summary>Retire des lignes de journal tout ce qui pourrait identifier une personne.</summary>
        public static string Filtrer(string texte)
        {
            foreach (var (motif, remplacement) in Filtres)
                texte = motif.Replace(texte, remplacement);
            return Numeros.Replace(texte, m => Regex.IsMatch(m.Value, @"^\d{4}-\d{2}-\d{2}") ? m.Value : "[numéro masqué]");
        }

        /// <summary>Crée le ZIP dans <paramref name="dossierSortie"/> (le Bureau en usage normal) et renvoie son chemin.</summary>
        public static string Preparer(string dossierSortie, Control? ecran = null, DateTime? maintenant = null)
        {
            var now = maintenant ?? DateTime.Now;
            Directory.CreateDirectory(dossierSortie);
            string zip = Path.Combine(dossierSortie, $"Rapport_LIGUAPHARME_{now:yyyy-MM-dd_HHmm}.zip");
            if (File.Exists(zip)) File.Delete(zip);

            using (var archive = ZipFile.Open(zip, ZipArchiveMode.Create))
            {
                EcrireEntree(archive, "infos.txt", Informations(now, ecran));

                // journaux : lignes des 30 derniers jours, filtrées
                var limite = now.Date.AddDays(-JoursDeJournaux);
                foreach (var f in new DirectoryInfo(CheminsApp.DossierLogs).GetFiles("journal_*.log").OrderBy(f => f.Name))
                {
                    var sb = new StringBuilder();
                    bool recent = false;
                    foreach (var ligne in File.ReadLines(f.FullName, Encoding.UTF8))
                    {
                        // une ligne datée commence par « aaaa-MM-jj HH:mm:ss » ; les suivantes (piles d'appels) suivent la précédente
                        if (ligne.Length >= 10 && DateTime.TryParseExact(ligne.AsSpan(0, 10), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var d))
                            recent = d >= limite;
                        if (recent) sb.AppendLine(Filtrer(ligne));
                    }
                    if (sb.Length > 0)
                        EcrireEntree(archive, "journaux/" + f.Name, sb.ToString());
                }
            }
            Journal.Info("Rapport pour le développeur préparé : " + Path.GetFileName(zip));
            return zip;
        }

        private static void EcrireEntree(ZipArchive archive, string nom, string contenu)
        {
            var entree = archive.CreateEntry(nom, CompressionLevel.Optimal);
            using var flux = new StreamWriter(entree.Open(), new UTF8Encoding(true));
            flux.Write(contenu);
        }

        /// <summary>Informations techniques uniquement : versions, écran, disque, migrations, dates de sauvegarde, nombre de lignes par table.</summary>
        public static string Informations(DateTime maintenant, Control? ecran = null)
        {
            var sb = new StringBuilder();
            void L(string cle, string valeur) => sb.AppendLine($"{cle} : {valeur}");

            sb.AppendLine($"Rapport LIGUAPHARME du {maintenant:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('-', 50));
            L("Version de l'application", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "inconnue");
            L("Windows", RuntimeInformation.OSDescription + " (" + RuntimeInformation.OSArchitecture + ")");
            L(".NET", RuntimeInformation.FrameworkDescription);

            try
            {
                var ecranPrincipal = Screen.PrimaryScreen;
                if (ecranPrincipal != null)
                {
                    float zoom = ecran != null ? Theme.Echelle(ecran) : 1f;
                    L("Écran", $"{ecranPrincipal.Bounds.Width} x {ecranPrincipal.Bounds.Height} pixels, zoom {zoom * 100:0} %, " +
                               $"soit {(int)(ecranPrincipal.Bounds.Width / zoom)} x {(int)(ecranPrincipal.Bounds.Height / zoom)} unités logiques");
                }
            }
            catch (Exception ex) { L("Écran", "illisible (" + ex.GetType().Name + ")"); }

            try
            {
                var lecteur = new DriveInfo(Path.GetPathRoot(CheminsApp.DossierDonnees)!);
                L("Espace disque libre", $"{lecteur.AvailableFreeSpace / 1024 / 1024:N0} Mo sur {lecteur.TotalSize / 1024 / 1024:N0} Mo");
            }
            catch (Exception ex) { L("Espace disque libre", "illisible (" + ex.GetType().Name + ")"); }

            L("Taille de la base", File.Exists(CheminsApp.CheminBase) ? $"{new FileInfo(CheminsApp.CheminBase).Length / 1024:N0} Ko" : "base absente");

            try
            {
                using var ctx = new AppDbContext();
                var appliquees = ctx.Database.GetAppliedMigrations().ToList();
                L("Migrations appliquées", appliquees.Count.ToString());
                foreach (var m in appliquees) sb.AppendLine("   " + m);
                L("Migrations en attente", ctx.Database.GetPendingMigrations().Count().ToString());
            }
            catch (Exception ex) { L("Migrations", "illisibles (" + ex.GetType().Name + ")"); }

            try
            {
                var derniere = new DirectoryInfo(CheminsApp.DossierSauvegardes).GetFiles("PharmacieDB_????-??-??.sqlite").OrderByDescending(f => f.Name).FirstOrDefault();
                L("Dernière sauvegarde locale", derniere != null ? derniere.LastWriteTime.ToString("dd/MM/yyyy HH:mm") : "aucune");
                L("Sauvegarde externe configurée", SauvegardeExterneService.EstConfiguree ? "oui" : "non");
                L("Dernière copie externe réussie", ParametresApp.Actuels.DerniereCopieExterne is DateTime d ? d.ToString("dd/MM/yyyy HH:mm") : "aucune");
                L("Verrouillage automatique", ParametresApp.Actuels.DelaiVerrouillageMinutes == 0 ? "désactivé" : ParametresApp.Actuels.DelaiVerrouillageMinutes + " min");
            }
            catch (Exception ex) { L("Sauvegardes", "illisibles (" + ex.GetType().Name + ")"); }

            sb.AppendLine();
            sb.AppendLine("Nombre de lignes par table (aucun contenu) :");
            try
            {
                if (File.Exists(CheminsApp.CheminBase))
                {
                    using var c = new SqliteConnection($"Data Source={CheminsApp.CheminBase};Mode=ReadOnly;Pooling=False");
                    c.Open();
                    var tables = new List<string>();
                    using (var cmd = c.CreateCommand())
                    {
                        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name";
                        using var r = cmd.ExecuteReader();
                        while (r.Read()) tables.Add(r.GetString(0));
                    }
                    foreach (var t in tables)
                    {
                        using var cmd = c.CreateCommand();
                        cmd.CommandText = $"SELECT COUNT(*) FROM \"{t.Replace("\"", "")}\"";
                        sb.AppendLine($"   {t} : {Convert.ToInt64(cmd.ExecuteScalar())}");
                    }
                }
            }
            catch (Exception ex) { sb.AppendLine("   illisible (" + ex.GetType().Name + ")"); }

            return sb.ToString();
        }

        /// <summary>Prépare le rapport sur le Bureau puis ouvre l'Explorateur sur le fichier ; renvoie le chemin du ZIP.</summary>
        public static string PreparerSurLeBureauEtMontrer(Control? ecran = null)
        {
            string bureau = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string zip = Preparer(bureau, ecran);
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", $"/select,\"{zip}\"") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Journal.Erreur("Ouverture de l'Explorateur sur le rapport", ex);
            }
            return zip;
        }

        public const string MessageApresRapport = "Le rapport est sur le Bureau. Envoyez-le au développeur par WhatsApp ou par e-mail.";
    }
}
