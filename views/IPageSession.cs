namespace Pharmacie2.views
{
    /// <summary>Page principale ouverte après la connexion (PageAccueil, PageCaissier).</summary>
    public interface IPageSession
    {
        /// <summary>true si la page a été fermée par « Déconnexion » (et non par la croix).</summary>
        bool DeconnexionDemandee { get; }
    }
}
