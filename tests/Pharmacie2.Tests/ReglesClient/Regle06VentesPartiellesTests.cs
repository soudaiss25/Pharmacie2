using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>
/// Règle 6 du client : Crédit et Mutuelle sont des VENTES PARTIELLES. Partout, seul ce qui a été réellement reçu est
/// « encaissé » ; le reste (reste dû, part mutuelle en attente) est affiché à part.
/// </summary>
public class Regle06VentesPartiellesTests
{
    private static (User caissier, int session) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var caissier = new User { Nom = "Ali", Prenom = "Hamidou", Role = Roles.Caissier, Login = "caisse", MotDePasse = MotDePasseService.Hacher("x") };
        using var ctx = new AppDbContext();
        ctx.Users.Add(caissier);
        var mut = new Mutuel { NomEmployeur = "Entreprise X", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 80 };
        ctx.mutuels.Add(mut);
        ctx.SaveChanges();
        var s = new SessionCaisse { UserId = caissier.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" };
        ctx.SessionsCaisse.Add(s);

        var maintenant = DateTime.Now.AddSeconds(-5);
        // Crédit de 1 000 avec 300 d'avance
        var credit = new Vente
        {
            numeroVente = "V-000001", Statut = "Active", NomClient = "Client", PrenomClient = "Test", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Credit, Type = ModesPaiement.Credit, MontantTotal = 1000, UserId = caissier.Id, DateVente = maintenant
        };
        credit.Paiements.Add(new Paiement { NumeroPaiement = "P-1", Montant = 300, Mode = ModesPaiement.Especes, UserId = caissier.Id, DatePaiement = maintenant });
        // Mutuelle de 2 000 à 80 % : 1 600 à la mutuelle, 400 reçus du patient
        var mutuelle = new Vente
        {
            numeroVente = "V-000002", Statut = "Active", NomClient = "Client", PrenomClient = "Mut", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "M1",
            MoyenPaiement = ModesPaiement.Mutuelle, Type = ModesPaiement.Mutuelle, MontantTotal = 2000, MontantMutuelle = 1600, TauxMutuelle = 80,
            MutuelId = mut.IdMutuel, UserId = caissier.Id, DateVente = maintenant
        };
        mutuelle.Paiements.Add(new Paiement { NumeroPaiement = "P-2", Montant = 400, Mode = ModesPaiement.Especes, UserId = caissier.Id, DatePaiement = maintenant });
        ctx.ventes.AddRange(credit, mutuelle);
        ctx.SaveChanges();
        SessionUtilisateur.Courant = caissier;
        return (caissier, s.Id);
    }

    private static string K(decimal m) => Format.Montant(m);

    [Fact]
    public void Regle06_le_service_ne_compte_que_ce_qui_a_ete_recu_et_affiche_le_reste_a_part()
    {
        var (_, sid) = Preparer();
        using var ctx = new AppDbContext();
        var v = CaisseService.Ventilation(ctx, ctx.SessionsCaisse.Single(s => s.Id == sid));
        Assert.Equal(300m, v.VersementsCredit);          // crédit de 1 000 avec 300 d'avance : 300 encaissés…
        Assert.Equal(700m, v.ResteDuCredit);             // … et 700 de reste dû
        Assert.Equal(400m, v.PartPatientMutuelle);
        Assert.Equal(1600m, v.PartMutuelleEnAttente);
        Assert.Equal(300m, v.ParTypeDeVente[ModesPaiement.Credit]);
        Assert.Equal(400m, v.ParTypeDeVente[ModesPaiement.Mutuelle]);
        Assert.Equal(700m, v.Encaisse);
    }

    [Fact]
    public void Regle06_le_detail_de_session_et_le_rapport_ne_montrent_jamais_le_total_d_un_credit_ou_d_une_mutuelle()
    {
        var (_, sid) = Preparer();
        UiHelper.EnSta(() =>
        {
            var uc = new Uc_Caisse();
            using var hote = new Form();
            uc.Dock = DockStyle.Fill;
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();

            UiHelper.Appeler(uc, "ChargerDetailSession", sid);
            string v = UiHelper.Champ<Label>(uc, "lblVentilationSession").Text;
            Assert.Contains("Crédit (versements reçus) : " + K(300) + ", reste dû : " + K(700), v);
            Assert.Contains("Mutuelle (part patient reçue) : " + K(400) + ", part des entreprises en attente : " + K(1600), v);
            Assert.DoesNotContain(K(1000), v);       // total du crédit
            Assert.DoesNotContain(K(2000), v);       // total de la mutuelle

            // Rapport de caisse : mêmes chiffres (même service)
            UiHelper.Appeler(uc, "ChargerRapport");
            var credit = UiHelper.Champ<CarteKpi>(uc, "cardCredit");
            Assert.Equal(K(300), credit.Valeur);
            Assert.Contains("reste dû : " + K(700), credit.Detail);
            var mutuelle = UiHelper.Champ<CarteKpi>(uc, "cardMutuelle");
            Assert.Equal(K(400), mutuelle.Valeur);
            Assert.Contains("en attente : " + K(1600), mutuelle.Detail);
            var grille = UiHelper.Champ<DataGridView>(uc, "dgvVentilation");
            Assert.Equal(K(700), K(Convert.ToDecimal(grille.Rows[4].Cells["Montant"].Value) + Convert.ToDecimal(grille.Rows[5].Cells["Montant"].Value)));
            uc.Dispose();
        });
    }

    [Fact]
    public void Regle06_la_page_du_caissier_affiche_les_memes_chiffres()
    {
        var (caissier, _) = Preparer();
        UiHelper.EnSta(() =>
        {
            using var page = new PageCaissier(caissier);
            Assert.Equal("Versements sur crédits : " + K(300), UiHelper.Champ<Label>(page, "lblCredit").Text);
            Assert.Equal("Part patient mutuelle : " + K(400), UiHelper.Champ<Label>(page, "lblMutuelle").Text);
            Assert.Equal(K(700), UiHelper.Champ<CarteKpi>(page, "carteEncaisse").Valeur);   // encaissé = ce qui a été reçu
            var reste = UiHelper.Champ<CarteKpi>(page, "carteReste");
            Assert.Equal(K(700 + 1600), reste.Valeur);                                       // reste dû + part mutuelle en attente, à part
        });
    }
}
