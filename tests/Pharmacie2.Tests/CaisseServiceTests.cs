using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>R2b : le montant attendu en caisse est exactement les espèces réellement reçues.</summary>
public class CaisseServiceTests
{
    private static (int produit, int admin, int autre, int session) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        var autre = new User { Nom = "Ali", Prenom = "Hamidou", Role = Roles.Caissier, Login = "caisse", MotDePasse = MotDePasseService.Hacher("x") };
        int sid;
        using (var ctx = new AppDbContext())
        {
            ctx.Users.AddRange(admin, autre);
            ctx.SaveChanges();
            var s = new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Now.AddHours(-1), Statut = "Ouverte", CommentaireCloture = "" };
            ctx.SessionsCaisse.Add(s);
            ctx.SaveChanges();
            sid = s.Id;
        }
        SessionUtilisateur.Courant = admin;
        int pid = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 1000m);
        return (pid, admin.Id, autre.Id, sid);
    }

    private static decimal Attendu(int sessionId)
    {
        using var ctx = new AppDbContext();
        return CaisseService.EspecesAttendues(ctx, ctx.SessionsCaisse.Single(s => s.Id == sessionId));
    }

    private static void AvecMessages(Action a)
    {
        var cts = UiHelper.FermerLesMessages(false);
        try { UiHelper.EnSta(a); }
        finally { cts.Cancel(); }
    }

    private static void VendreViaFormulaire(int produitId, int boites, string mode, decimal? especes = null)
    {
        AvecMessages(() =>
        {
            using var f = new FormVente();
            var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
            lignes.Add(new LigneVente
            {
                ProduitId = produitId, Quantite = boites, PrixUnitaire = 1000m, UniteVendue = "Boîte",
                Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Plaquette" }
            });
            UiHelper.Appeler(f, "RafraichirGrille");
            UiHelper.Appeler(f, "RecalculerTotal");
            UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = mode;
            if (especes.HasValue) UiHelper.Champ<NumericUpDown>(f, "numEspeces").Value = especes.Value;
            UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
        });
    }

    private static int CreditDeLaVeille(int userId, int produitId)
    {
        using var ctx = new AppDbContext();
        var v = new Vente
        {
            numeroVente = "V-000900", Statut = "Active", NomClient = "Client", PrenomClient = "Test", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Credit, Type = ModesPaiement.Credit, MontantTotal = 5000, UserId = userId, DateVente = DateTime.Now.AddDays(-1)
        };
        v.Paiements.Add(new Paiement { NumeroPaiement = "P-HIER", Montant = 1000, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = DateTime.Now.AddDays(-1) });
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        return v.IdVente;
    }

    private static void RembourserCredit(int venteId, decimal montant, string mode)
    {
        AvecMessages(() =>
        {
            using var f = new FormPaiements(venteId);
            UiHelper.Champ<NumericUpDown>(f, "numMontant").Value = montant;
            UiHelper.Champ<ComboBox>(f, "cbMode").SelectedItem = mode;
            UiHelper.Appeler(f, "btnAjouterPaiement_Click", null, EventArgs.Empty);
        });
    }

    [Fact]
    public void Comptant_3000_avec_5000_donnes_attend_3000_en_caisse()
    {
        var (pid, _, _, sid) = Preparer();
        VendreViaFormulaire(pid, 3, ModesPaiement.Comptant, 5000);
        Assert.Equal(3000m, Attendu(sid));      // le rendu de 2 000 n'est jamais soustrait une seconde fois
    }

    [Theory]
    [InlineData("Carte bancaire")]
    [InlineData("Chèque")]
    [InlineData("Mvola")]
    [InlineData("Huri Money")]
    public void Une_vente_hors_especes_ne_change_pas_l_attendu(string mode)
    {
        var (pid, _, _, sid) = Preparer();
        VendreViaFormulaire(pid, 2, ModesPaiement.Comptant, 2000);
        Assert.Equal(2000m, Attendu(sid));
        VendreViaFormulaire(pid, 4, mode);
        Assert.Equal(2000m, Attendu(sid));
        using var ctx = new AppDbContext();
        var v = CaisseService.Ventilation(ctx, ctx.SessionsCaisse.Single());
        Assert.Equal(4000m, v.HorsTiroir[mode]);
        Assert.Equal(6000m, v.Encaisse);
    }

    [Fact]
    public void Remboursement_d_un_credit_de_la_veille_en_especes_est_inclus_mais_pas_par_Mvola()
    {
        var (pid, admin, _, sid) = Preparer();
        int credit = CreditDeLaVeille(admin, pid);
        Assert.Equal(0m, Attendu(sid));                       // l'avance d'hier appartient à la session d'hier

        RembourserCredit(credit, 1500, ModesPaiement.Especes);
        Assert.Equal(1500m, Attendu(sid));                    // inclus, quelle que soit la date de la vente

        RembourserCredit(credit, 800, ModesPaiement.Mvola);
        Assert.Equal(1500m, Attendu(sid));                    // même remboursement par Mvola : exclu
        using var ctx = new AppDbContext();
        Assert.Equal(800m, CaisseService.Ventilation(ctx, ctx.SessionsCaisse.Single()).HorsTiroir[ModesPaiement.Mvola]);
    }

    [Fact]
    public void Une_vente_annulee_est_exclue_et_les_paiements_des_autres_caissiers_aussi()
    {
        var (pid, admin, autre, sid) = Preparer();
        VendreViaFormulaire(pid, 3, ModesPaiement.Comptant, 3000);
        VendreViaFormulaire(pid, 1, ModesPaiement.Comptant, 1000);
        Assert.Equal(4000m, Attendu(sid));

        using (var ctx = new AppDbContext())
        {
            ctx.ventes.OrderBy(v => v.IdVente).First().Statut = "Annulée";
            // paiement en espèces d'un autre caissier pendant la même période : jamais dans CE tiroir
            var v2 = ctx.ventes.OrderBy(v => v.IdVente).Last();
            ctx.paiement.Add(new Paiement { NumeroPaiement = "P-AUTRE", VenteId = v2.IdVente, Montant = 700, Mode = ModesPaiement.Especes, UserId = autre, DatePaiement = DateTime.Now });
            ctx.SaveChanges();
        }
        Assert.Equal(1000m, Attendu(sid));
    }

    [Fact]
    public void Remboursement_negatif_en_especes_est_soustrait_de_l_attendu()
    {
        var (pid, admin, _, sid) = Preparer();
        VendreViaFormulaire(pid, 2, ModesPaiement.Comptant, 2000);
        using (var ctx = new AppDbContext())
        {
            var v = ctx.ventes.Single();
            ctx.paiement.Add(new Paiement { NumeroPaiement = "R-TEST", VenteId = v.IdVente, Montant = -500, Mode = ModesPaiement.Especes, UserId = admin, DatePaiement = DateTime.Now });
            ctx.SaveChanges();
        }
        Assert.Equal(1500m, Attendu(sid));
        using var c2 = new AppDbContext();
        var vent = CaisseService.Ventilation(c2, c2.SessionsCaisse.Single());
        Assert.Equal(2000m, vent.EspecesRecues);
        Assert.Equal(-500m, vent.RemboursementsEspeces);
    }
}
