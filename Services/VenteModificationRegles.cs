using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>Règles de modification d'une vente existante (mutuelle comprise).</summary>
    public static class VenteModificationRegles
    {
        public const string MessageVenteReglee = "La mutuelle a déjà réglé cette vente : elle ne peut plus être modifiée.";

        /// <summary>Lève une exception en français si la vente ne peut plus être modifiée.</summary>
        public static void VerifierModifiable(Vente vente)
        {
            if (vente.Statut == "Annulée")
                throw new InvalidOperationException("Impossible de modifier une vente annulée.");

            if (vente.MutuelId != null && vente.MutuelleReglee)
                throw new InvalidOperationException(MessageVenteReglee);
        }

        /// <summary>
        /// Paiement à ajouter quand une vente est modifiée : positif = le client paie en plus (complément), négatif = on rend
        /// de l'argent (remboursement), 0 = rien. Modes payés en totalité et part patient d'une mutuelle : le versé doit
        /// égaler ce qui est dû. Crédit : aucun paiement automatique, sauf si le versé dépasse le nouveau total (remboursement).
        /// Le rendu de monnaie d'origine n'intervient jamais : les paiements enregistrés sont déjà nets.
        /// </summary>
        public static decimal AjustementPaiements(string nouveauType, decimal montantTotal, decimal partMutuelle, decimal verse)
        {
            decimal du = montantTotal - (nouveauType == ModesPaiement.Mutuelle ? partMutuelle : 0m);
            if (du < 0) du = 0;
            if (nouveauType == ModesPaiement.Credit)
                return verse > du ? du - verse : 0m;
            return du - verse;
        }

        /// <summary>Part prise en charge par la mutuelle, en KMF entiers (pas de centimes).</summary>
        public static decimal PartMutuelle(decimal total, decimal tauxPourcent)
            => Math.Round(total * tauxPourcent / 100m, 0, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Applique la mutuelle choisie à la vente. MutuelleReglee ne repasse à false que si la mutuelle
        /// ou le montant de la part mutuelle change réellement. Le taux d'origine est conservé tant que
        /// la mutuelle ne change pas.
        /// </summary>
        public static void AppliquerMutuelle(Vente vente, bool modeMutuelle, Mutuel? mutuelle, decimal total)
        {
            if (modeMutuelle && mutuelle != null)
            {
                bool memeMutuelle = vente.MutuelId == mutuelle.IdMutuel;
                decimal taux = memeMutuelle && vente.TauxMutuelle > 0 ? vente.TauxMutuelle : mutuelle.TauxPriseEnCharge;
                decimal montant = PartMutuelle(total, taux);

                bool change = !memeMutuelle || vente.MontantMutuelle != montant;

                vente.MutuelId = mutuelle.IdMutuel;
                vente.TauxMutuelle = taux;
                vente.MontantMutuelle = montant;
                if (change)
                    vente.MutuelleReglee = false;
            }
            else
            {
                bool avaitMutuelle = vente.MutuelId != null || vente.MontantMutuelle != 0;
                vente.MutuelId = null;
                vente.TauxMutuelle = 0;
                vente.MontantMutuelle = 0;
                if (avaitMutuelle)
                    vente.MutuelleReglee = false;
            }
        }
    }
}
