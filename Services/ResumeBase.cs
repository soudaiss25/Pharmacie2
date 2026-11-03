using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Pharmacie2.Services
{
    /// <summary>Ce que contient une base, lu en lecture seule (étape 4 de l'assistant d'import : base importée et base actuelle).</summary>
    public sealed record ResumeBase(string Fichier, bool Existe, bool Lisible, int Ventes, DateTime? DerniereVente, int Produits, int Utilisateurs, decimal ChiffreAffaires)
    {
        /// <summary>Aucune vente (ou fichier absent / illisible).</summary>
        public bool Vide => !Existe || !Lisible || Ventes == 0;

        public string DerniereVenteTexte => DerniereVente.HasValue ? DerniereVente.Value.ToString("dd/MM/yyyy HH:mm") : "aucune";

        /// <summary>Lecture seule : aucune écriture, aucune migration, pas de pool de connexions.</summary>
        public static ResumeBase Lire(string chemin)
        {
            string nom = Path.GetFileName(chemin);
            if (!File.Exists(chemin)) return new ResumeBase(nom, false, false, 0, null, 0, 0, 0m);
            try
            {
                using var c = new SqliteConnection($"Data Source={chemin};Mode=ReadOnly;Pooling=False");
                c.Open();
                int Compter(string table)
                {
                    using var cmd = c.CreateCommand();
                    cmd.CommandText = $"SELECT COUNT(*) FROM \"{table}\"";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                int ventes = Compter("ventes");
                DateTime? derniere = null;
                decimal ca = 0m;
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT DateVente, Statut, MontantTotal FROM ventes";
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        if (!r.IsDBNull(0) && DateTime.TryParse(Convert.ToString(r.GetValue(0)), CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
                            && (derniere == null || d > derniere)) derniere = d;
                        if (Convert.ToString(r.GetValue(1)) == "Active" && !r.IsDBNull(2)
                            && decimal.TryParse(Convert.ToString(r.GetValue(2), CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out var m))
                            ca += m;
                    }
                }
                return new ResumeBase(nom, true, true, ventes, derniere, Compter("produits"), Compter("Users"), ca);
            }
            catch
            {
                return new ResumeBase(nom, true, false, 0, null, 0, 0, 0m);
            }
        }
    }
}
