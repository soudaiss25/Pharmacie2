namespace Pharmacie2.Models
{
    /// <summary>
    /// Stocke l'utilisateur connecté et la session de caisse active
    /// pour toute la durée de la session applicative.
    /// Initialisé au moment du login dans Form1.cs.
    /// </summary>
    public static class SessionUtilisateur
    {
        public static User Courant { get; set; }

        public static int IdCourant => Courant?.Id ?? 0;
        public static string NomComplet => Courant != null
            ? $"{Courant.Prenom} {Courant.Nom}"
            : "Inconnu";

        /// <summary>Session de caisse actuellement ouverte (null si aucune).</summary>
        public static SessionCaisse SessionCaisseEnCours { get; set; }

        /// <summary>Vide l'utilisateur connecté et la session de caisse en cours (déconnexion).</summary>
        public static void Deconnecter()
        {
            Courant = null;
            SessionCaisseEnCours = null;
        }

        /// <summary>Recharge depuis la DB la dernière session ouverte, si elle existe.</summary>
        public static void ChargerSessionCaisseActive()
        {
            using (var ctx = new AppDbContext())
            {
                SessionCaisseEnCours = ctx.SessionsCaisse
                    .OrderByDescending(s => s.DateOuverture)
                    .FirstOrDefault(s => s.Statut == "Ouverte");
            }
        }
    }
}