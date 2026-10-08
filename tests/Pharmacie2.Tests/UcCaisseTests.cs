using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;
using Xunit;

public class UcCaisseTests
{
    [Fact]
    public void Rapport_et_sessions_s_affichent_et_se_rafraichissent_apres_une_vente_sans_fuite()
    {
        var ids = LayoutSeed.Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                var uc = new Uc_Caisse();
                using var hote = new Form();
                uc.Dock = DockStyle.Fill;
                hote.Controls.Add(uc);
                hote.Show();
                Application.DoEvents();

                // L'onglet « Rapport » est sur la période du jour : la vente du jeu de données date de 40 jours
                Assert.Equal("0", UiHelper.Champ<CarteKpi>(uc, "cardVentes").Valeur);
                Assert.Contains("KMF", UiHelper.Champ<CarteKpi>(uc, "cardComptant").Valeur);

                // La session ouverte du caissier apparaît dans l'onglet « Sessions »
                var sessions = UiHelper.Champ<DataGridView>(uc, "dgvSessions");
                Assert.Single(sessions.Rows.Cast<DataGridViewRow>());

                // Nouvelle vente du jour -> rafraîchissement automatique via VenteEvenements
                using (var ctx = new AppDbContext())
                {
                    var v = new Vente
                    {
                        numeroVente = "V-000099", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "",
                        MatriculeEmploye = "", MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MontantTotal = 1500, DateVente = DateTime.Now
                    };
                    ctx.ventes.Add(v);
                    ctx.SaveChanges();
                }
                VenteEvenements.Notifier();
                Application.DoEvents();
                Assert.Equal("1", UiHelper.Champ<CarteKpi>(uc, "cardVentes").Valeur);

                // Après suppression de l'écran, l'événement ne le touche plus (pas de fuite d'abonnement)
                uc.Dispose();
                VenteEvenements.Notifier();
            });
        }
        finally { cts.Cancel(); }
    }
}
