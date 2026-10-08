namespace Pharmacie2.Models
{
    /// <summary>Rôles des utilisateurs. Les valeurs sont celles déjà enregistrées en base : ne pas les modifier.</summary>
    public static class Roles
    {
        public const string Administrateur = "Administrateur";
        public const string Pharmacien = "Pharmacien";
        public const string Caissier = "Caissier";

        public static readonly string[] Tous = { Administrateur, Pharmacien, Caissier };

        /// <summary>Administrateur ou Pharmacien.</summary>
        public static bool EstGestionnaire(string? role) => role == Administrateur || role == Pharmacien;
    }
}
