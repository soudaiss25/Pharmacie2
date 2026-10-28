using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Ce qui est réellement entré (ou sorti) de la caisse sur une période. Un seul calcul pour tous les écrans
    /// (Caisse, page du Caissier, clôture, rapport) : jamais de calcul dupliqué.
    /// </summary>
    /// <param name="Especes">Espèces réellement reçues = ce qui doit être dans le tiroir (remboursements en espèces déduits).</param>
    /// <param name="EspecesRecues">Espèces reçues (montants positifs seulement).</param>
    /// <param name="RemboursementsEspeces">Espèces rendues aux clients (montants négatifs, ex. −500), 0 s'il n'y en a pas.</param>
    /// <param name="HorsTiroir">Chèque, Carte bancaire, Mvola, Huri Money : encaissés mais pas dans le tiroir, par mode.</param>
    /// <param name="Encaisse">Tout ce qui a été réellement reçu, tous modes confondus (remboursements déduits).</param>
    /// <param name="ParTypeDeVente">Reçu (net) par type de vente (Comptant, Chèque, Carte bancaire, Mvola, Huri Money, Crédit, Mutuelle) : jamais le total facturé.</param>
    /// <param name="VentesPayees">Reçu sur des ventes réglées en totalité (Comptant, Chèque, Carte, Mvola, Huri Money).</param>
    /// <param name="VersementsCredit">Reçu sur des ventes à crédit (avances et remboursements de crédits, quelle que soit la date de la vente).</param>
    /// <param name="PartPatientMutuelle">Reçu des patients sur des ventes mutuelle.</param>
    /// <param name="ResteDuCredit">Reste dû sur les ventes à crédit faites pendant la période (affiché à part, jamais « encaissé »).</param>
    /// <param name="PartMutuelleEnAttente">Part des mutuelles pas encore réglée sur les ventes mutuelle de la période (crédit de l'entreprise).</param>
    public sealed record VentilationCaisse(
        decimal Especes,
        decimal EspecesRecues,
        decimal RemboursementsEspeces,
        IReadOnlyDictionary<string, decimal> HorsTiroir,
        decimal Encaisse,
        IReadOnlyDictionary<string, decimal> ParTypeDeVente,
        decimal VentesPayees,
        decimal VersementsCredit,
        decimal PartPatientMutuelle,
        decimal ResteDuCredit,
        decimal PartMutuelleEnAttente)
    {
        public decimal TotalHorsTiroir => HorsTiroir.Values.Sum();
    }

    /// <summary>Où va l'argent d'un paiement : session de caisse retenue, ou refus avec un message clair (Message null = refus voulu par l'utilisateur).</summary>
    public sealed record AffectationPaiement(bool Autorise, int? SessionCaisseId, string? Message);

    public static class CaisseService
    {
        /// <summary>Écart (en KMF) au-delà duquel un motif est obligatoire à la clôture. À 100 KMF ou moins : aucune question.</summary>
        public const decimal SeuilEcart = 100m;

        public static readonly string[] MotifsEcart = { "Vente oubliée", "Erreur de rendu de monnaie", "Paiement non saisi", "Inconnu" };

        /// <summary>Écart d'une session clôturée = montant compté − montant attendu (figé, ou recalculé pour une ancienne session) ; null si pas de comptage.</summary>
        public static decimal? Ecart(AppDbContext ctx, SessionCaisse session)
        {
            if (session.Statut != "Clôturée" || !session.MontantCompteCloture.HasValue) return null;
            return session.MontantCompteCloture.Value - Attendu(ctx, session).Montant;
        }

        public const string MessageAucuneCaisse = "Aucune caisse n'est ouverte : impossible d'encaisser de l'argent en espèces. Ouvrez la caisse d'abord (Caisse → Ouvrir la caisse), puis recommencez.";

        /// <summary>
        /// Rattache un paiement au bon tiroir. Règle : la session ouverte de l'utilisateur ; sinon, pour des espèces, la session ouverte d'un autre utilisateur
        /// après confirmation (« Cet argent va dans la caisse de … ? ») ; s'il n'y a aucune session ouverte, un paiement en espèces est refusé.
        /// Chèque, carte, Mvola et Huri Money ne passent pas par le tiroir : jamais refusés.
        /// </summary>
        public static AffectationPaiement AffecterPaiement(AppDbContext ctx, int? userId, string mode, Func<string, bool> confirmerCaisseDe)
        {
            var ouvertes = ctx.SessionsCaisse.AsNoTracking().Include(s => s.User).Where(s => s.Statut == "Ouverte")
                .OrderByDescending(s => s.DateOuverture).ToList();
            var propre = userId == null ? null : ouvertes.FirstOrDefault(s => s.UserId == userId);
            if (propre != null) return new AffectationPaiement(true, propre.Id, null);
            if (mode != ModesPaiement.Especes) return new AffectationPaiement(true, null, null);

            var autre = ouvertes.FirstOrDefault();   // la plus récente
            if (autre == null) return new AffectationPaiement(false, null, MessageAucuneCaisse);
            string nom = autre.User != null ? $"{autre.User.Prenom} {autre.User.Nom}" : "un autre utilisateur";
            return confirmerCaisseDe(nom) ? new AffectationPaiement(true, autre.Id, null) : new AffectationPaiement(false, null, null);
        }

        /// <summary>
        /// Espèces attendues dans le tiroir pour une session : somme des paiements en espèces du caissier de la session,
        /// datés pendant la session, sur des ventes non annulées. Les remboursements (montants négatifs) sont inclus.
        /// Le rendu de monnaie n'est JAMAIS soustrait : le paiement est déjà net du rendu.
        /// Inclut les remboursements d'anciens crédits reçus pendant la session, quelle que soit la date de la vente.
        /// </summary>
        public static decimal EspecesAttendues(AppDbContext ctx, SessionCaisse session)
            => Ventilation(ctx, session).Especes;

        /// <summary>Ventilation d'une session (de l'ouverture à la clôture, ou à maintenant si elle est en cours).</summary>
        public static VentilationCaisse Ventilation(AppDbContext ctx, SessionCaisse session)
            => Calculer(ctx, session.DateOuverture, session.DateCloture ?? DateTime.Now, session.UserId, session.Id);

        /// <summary>
        /// Montant à afficher pour une session : le montant figé à la clôture si la session est clôturée (jamais de recalcul) ;
        /// pour une ancienne session clôturée sans valeur figée, la valeur recalculée (Recalcule = true, rien n'est écrit en base) ;
        /// pour une session en cours, le calcul en direct.
        /// </summary>
        public static (decimal Montant, bool Recalcule) Attendu(AppDbContext ctx, SessionCaisse session)
        {
            if (session.Statut == "Clôturée" && session.MontantAttenduCloture.HasValue)
                return (session.MontantAttenduCloture.Value, false);
            return (EspecesAttendues(ctx, session), session.Statut == "Clôturée");
        }

        /// <param name="userId">Caissier concerné ; null = tous les utilisateurs (rapport de caisse).</param>
        /// <param name="sessionId">
        /// Pour le détail d'une session : les paiements rattachés à CETTE session (peu importe qui les a encaissés), plus les anciens paiements sans session
        /// (règle d'origine : caissier de la session et dates de la session). Null : toutes les sessions (rapport de caisse par période).
        /// </param>
        public static VentilationCaisse Calculer(AppDbContext ctx, DateTime debut, DateTime fin, int? userId, int? sessionId = null)
        {
            var requete = ctx.paiement.AsNoTracking().Include(p => p.Vente).Where(p => p.Vente.Statut != "Annulée");
            requete = sessionId == null
                ? requete.Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin && (userId == null || p.UserId == userId))
                : requete.Where(p => p.SessionCaisseId == sessionId
                                  || (p.SessionCaisseId == null && p.DatePaiement >= debut && p.DatePaiement <= fin && (userId == null || p.UserId == userId)));
            var paiements = requete.ToList();

            decimal Somme(IEnumerable<Paiement> l) => l.Sum(p => p.Montant);
            var horsTiroir = ModesPaiement.ModesDeReception
                .Where(m => m != ModesPaiement.Especes)
                .ToDictionary(m => m, m => Somme(paiements.Where(p => p.Mode == m)));

            var ventes = ctx.ventes.AsNoTracking()
                .Include(v => v.Paiements)
                .Where(v => v.DateVente >= debut && v.DateVente <= fin && v.Statut == "Active"
                         && (userId == null || v.UserId == userId))
                .ToList();

            return new VentilationCaisse(
                Especes: Somme(paiements.Where(p => p.Mode == ModesPaiement.Especes)),
                EspecesRecues: Somme(paiements.Where(p => p.Mode == ModesPaiement.Especes && p.Montant > 0)),
                RemboursementsEspeces: Somme(paiements.Where(p => p.Mode == ModesPaiement.Especes && p.Montant < 0)),
                HorsTiroir: horsTiroir,
                Encaisse: Somme(paiements),
                ParTypeDeVente: ModesPaiement.Tous.ToDictionary(m => m, m => Somme(paiements.Where(p => p.Vente.Type == m))),
                VentesPayees: Somme(paiements.Where(p => p.Vente.Type != ModesPaiement.Credit && p.Vente.Type != ModesPaiement.Mutuelle)),
                VersementsCredit: Somme(paiements.Where(p => p.Vente.Type == ModesPaiement.Credit)),
                PartPatientMutuelle: Somme(paiements.Where(p => p.Vente.Type == ModesPaiement.Mutuelle)),
                ResteDuCredit: ventes.Where(v => v.Type == ModesPaiement.Credit).Sum(v => v.MontantRestant),
                PartMutuelleEnAttente: ventes.Where(v => v.Type == ModesPaiement.Mutuelle && !v.MutuelleReglee).Sum(v => v.MontantMutuelle));
        }
    }
}
