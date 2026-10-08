using System.Globalization;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// « Ma journée » : ce qu'il faut savoir en quelques secondes — argent reçu, ce qui manque ou va périmer,
    /// qui doit de l'argent. Écran d'ouverture de l'application.
    /// </summary>
    public partial class Uc_MaJournee : UserControl
    {
        private static readonly CultureInfo Fr = new CultureInfo("fr-FR");

        /// <summary>Demande d'ouverture d'un écran (déjà filtré) depuis la liste « À faire maintenant ».</summary>
        public event Action<TypeAFaire>? OuvrirDemande;

        public Uc_MaJournee()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            VenteEvenements.VenteModifiee += SurVenteModifiee;
            Disposed += (s, e) => VenteEvenements.VenteModifiee -= SurVenteModifiee;

            Actualiser();
        }

        private void SurVenteModifiee(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired) BeginInvoke(new Action(Actualiser)); else Actualiser();
        }

        public void Actualiser()
        {
            var maintenant = DateTime.Now;
            string prenom = SessionUtilisateur.Courant?.Prenom ?? "";
            string jour = maintenant.ToString("dddd d MMMM yyyy", Fr);
            jour = char.ToUpper(jour[0], Fr) + jour.Substring(1);
            lblBonjour.Text = (string.IsNullOrWhiteSpace(prenom) ? "Bonjour" : "Bonjour " + prenom) + " — " + jour;

            try
            {
                var k = TableauDeBordService.Kpis(maintenant);

                carteEncaisse.Valeur = Format.Montant(k.EncaisseAujourdhui);
                carteEncaisse.Detail = Format.Evolution(k.EncaisseAujourdhui, k.EncaisseHier) + " par rapport à hier, à la même heure";
                carteEncaisse.Tendance = Format.Tendance(k.EncaisseAujourdhui, k.EncaisseHier);

                carteVentes.Valeur = k.VentesAujourdhui.ToString(Fr);
                carteVentes.Detail = Format.Evolution(k.VentesAujourdhui, k.VentesHier) + " par rapport à hier, à la même heure";
                carteVentes.Tendance = Format.Tendance(k.VentesAujourdhui, k.VentesHier);

                carteARecuperer.Valeur = Format.Montant(k.ArgentARecuperer);
                carteARecuperer.Detail = $"{k.ClientsDebiteurs} client(s) et {k.MutuellesDebitrices} mutuelle(s)";
                carteARecuperer.Niveau = k.ArgentARecuperer > 0 ? "attention" : "succes";
                carteARecuperer.HausseEstBonne = false;

                carteBenefice.Valeur = Format.Montant(k.BeneficeMois);
                carteBenefice.Detail = Format.Evolution(k.BeneficeMois, k.BeneficeMoisPrecedent) + " par rapport au mois dernier (même période)";
                carteBenefice.Tendance = Format.Tendance(k.BeneficeMois, k.BeneficeMoisPrecedent);

                var aFaire = TableauDeBordService.AFaireMaintenant(maintenant)
                    .Select(a => new ElementAction(a.Texte, a.Niveau, () => OuvrirDemande?.Invoke(a.Type)));
                listeActions.Definir(aFaire);

                graphique.Definir(TableauDeBordService.Encaissements7Jours(maintenant));
            }
            catch (Exception ex)
            {
                Journal.Erreur("Calcul de l'écran Ma journée", ex);
                listeActions.Definir(new[] { new ElementAction("Les chiffres n'ont pas pu être calculés. Voir le journal.", "urgent", null) });
            }
        }
    }
}
