using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>Rattache l'argent reçu au bon tiroir, avec les questions et les messages en français.</summary>
    public static class CaisseUi
    {
        public static AffectationPaiement Affecter(IWin32Window proprietaire, string mode)
        {
            using var ctx = new AppDbContext();
            int? userId = SessionUtilisateur.IdCourant == 0 ? null : SessionUtilisateur.IdCourant;
            var r = CaisseService.AffecterPaiement(ctx, userId, mode,
                nom => MessageBox.Show(proprietaire, $"Cet argent va dans la caisse de {nom} ?", "Caisse", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            if (!r.Autorise && r.Message != null)
                MessageBox.Show(proprietaire, r.Message, "Encaissement impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return r;
        }
    }
}
