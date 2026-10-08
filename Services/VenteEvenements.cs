namespace Pharmacie2.Services
{
    /// <summary>
    /// Notification statique : une vente vient d'être créée, modifiée ou annulée.
    /// Uc_Vente, Uc_Caisse et « Ma journée » s'y abonnent pour se rafraîchir.
    /// Elle est déclenchée par les formulaires eux-mêmes, après validation de la transaction.
    /// </summary>
    public static class VenteEvenements
    {
        public static event EventHandler? VenteModifiee;

        public static void Notifier(object? source = null)
        {
            try
            {
                VenteModifiee?.Invoke(source, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Un écran abonné qui échoue ne doit jamais faire échouer une vente déjà enregistrée
                Journal.Erreur("Rafraîchissement après modification de vente", ex);
            }
        }
    }
}
