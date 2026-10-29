using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public static class MutuelleService
    {
        public const string PatientNonIdentifieLibelle = "Patient non identifié";

        /// <summary>Vente sans nom ni prénom ni matricule (ventes d'avant la règle « nom du patient obligatoire »).</summary>
        public static bool PatientNonIdentifie(Vente v)
            => string.IsNullOrWhiteSpace(v.NomClient) && string.IsNullOrWhiteSpace(v.PrenomClient) && MatriculeVide(v.MatriculeEmploye);

        public static bool MatriculeVide(string? matricule)
        {
            var m = (matricule ?? "").Trim();
            return m == "" || m == "N/A" || m == "—" || m == "-";
        }
    }
}
