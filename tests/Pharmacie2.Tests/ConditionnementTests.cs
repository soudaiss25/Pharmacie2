using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Conditionnement et ventes à l'unité : D25, D27, D28, D29 et leur prévention (FormVente, FormAddProduit).</summary>
public class ConditionnementTests
{
    internal sealed class Ids { public int BoiteDetail, Normal, VenteUniteNb1, PlaquetteMal, Centimes, EntiersSeulement; }

    private static Produit P(string nom, string unite, int nb, decimal vente = 1000, decimal? unitePrix = 100, int stock = 50)
        => new()
        {
            Nom = nom, Type = "Autre", PrixAchat = 500, PrixVente = vente, PrixVenteUnite = nb > 1 ? unitePrix : null, NbUniteParBoite = nb, UniteVente = unite,
            QuantiteEnStock = stock, SeuilAlerte = 1, DateExpiration = new DateTime(2029, 1, 1), Indication = "", Posologie = ""
        };

    private static void Vendre(AppDbContext ctx, int userId, Produit p, string numero, string unite, int quantite, decimal prixUnitaire)
    {
        var v = new Vente
        {
            numeroVente = numero, Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = quantite * prixUnitaire, UserId = userId, DateVente = DateTime.Now.AddDays(-5)
        };
        v.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = quantite, QuantiteUnites = quantite, PrixUnitaire = prixUnitaire, UniteVendue = unite });
        v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + numero, Montant = v.MontantTotal, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = v.DateVente });
        ctx.ventes.Add(v);
    }

    internal static Ids Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ids = new Ids();
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        ctx.Users.Add(admin);
        ctx.SaveChanges();

        var boiteDetail = P("Détail nommé Boîte", "Boîte", 10);                              // D25
        var normal = P("Plaquette normale", "Plaquette", 10);
        var nb1 = P("Vendu à l'unité puis à la boîte", "Boîte", 1);                            // D27
        var mal = P("Plaquette de 30", "Plaquette", 30);                                      // D28
        var comprime30 = P("Comprimé de 30", "Comprimé", 30);                                 // pas « Plaquette » : pas de D28
        var centimes = P("Prix calculé à centimes", "Plaquette", 3, vente: 1000, unitePrix: 333);   // D29
        var entiers = P("Prix entiers", "Plaquette", 5, vente: 1000, unitePrix: 200);
        ctx.produits.AddRange(boiteDetail, normal, nb1, mal, comprime30, centimes, entiers);
        ctx.SaveChanges();

        Vendre(ctx, admin.Id, nb1, "V-000001", "Plaquette", 3, 100m);
        Vendre(ctx, admin.Id, centimes, "V-000002", "Plaquette", 2, 333.33m);
        Vendre(ctx, admin.Id, entiers, "V-000003", "Plaquette", 2, 200m);
        Vendre(ctx, admin.Id, normal, "V-000004", StockService.UniteBoite, 1, 1000m);          // vendu à la boîte : ne compte pas
        ctx.SaveChanges();
        (ids.BoiteDetail, ids.Normal, ids.VenteUniteNb1, ids.PlaquetteMal, ids.Centimes, ids.EntiersSeulement) =
            (boiteDetail.Id, normal.Id, nb1.Id, mal.Id, centimes.Id, entiers.Id);
        SessionUtilisateur.Courant = admin;
        return ids;
    }

    private static Produit Lire(int id) { using var ctx = new AppDbContext(); return ctx.produits.AsNoTracking().Single(p => p.Id == id); }

    private static RapportDiagnostic Lancer(bool appliquer)
    {
        using var ctx = new AppDbContext();
        return DiagnosticBaseService.Executer(ctx, appliquer, DateTime.Now);
    }

    private static List<Constat> Code(RapportDiagnostic r, string code) => r.Constats.Where(c => c.Code == code).ToList();

    // ───────── diagnostic ─────────

    [Fact]
    public void D25_D27_D28_D29_donnent_exactement_le_resultat_attendu_a_l_import()
    {
        var ids = Preparer();
        var r = Lancer(appliquer: true);

        // D25 : unité de détail « Boîte » → fiche à vérifier
        Assert.Equal(new[] { "Détail nommé Boîte" }, Code(r, "D25").Select(c => c.Objet).ToArray());
        var p25 = Lire(ids.BoiteDetail);
        Assert.True(p25.FicheAVerifier);
        Assert.Contains("Conditionnement : l'unité de détail s'appelle « Boîte »", p25.MotifsFiche);

        // D27 : ventes à l'unité passées mais produit maintenant à la boîte → stock « À vérifier »
        Assert.Equal(new[] { "Vendu à l'unité puis à la boîte" }, Code(r, "D27").Select(c => c.Objet).ToArray());
        var p27 = Lire(ids.VenteUniteNb1);
        Assert.True(p27.StockAVerifier);
        Assert.Contains("conditionnement modifié après des ventes à l'unité", p27.MotifVerification);
        Assert.Equal(50, p27.QuantiteEnStock);                                                 // le stock n'est jamais modifié
        Assert.DoesNotContain(Code(r, "D2"), c => c.Objet == "Vendu à l'unité puis à la boîte");

        // D28 : « Plaquette » avec 20 unités ou plus (information, rien de modifié)
        Assert.Equal(new[] { "Plaquette de 30" }, Code(r, "D28").Select(c => c.Objet).ToArray());
        Assert.All(Code(r, "D28"), c => Assert.Equal(GroupeConstat.Information, c.Groupe));
        Assert.False(Lire(ids.PlaquetteMal).FicheAVerifier);

        // D29 : ventes à l'unité avec un prix à centimes (rapport d'import)
        Assert.Equal(new[] { "Prix calculé à centimes" }, Code(r, "D29").Select(c => c.Objet).ToArray());
        Assert.All(Code(r, "D29"), c => Assert.Equal(GroupeConstat.Information, c.Groupe));
        Assert.Contains("333,33", Code(r, "D29")[0].Detail.Replace('.', ','));

        // idempotent
        string avant = TestDb.Texte("SELECT group_concat(MotifsFiche || '|' || IFNULL(MotifVerification,''), '#') FROM produits ORDER BY Id");
        Lancer(appliquer: true);
        Assert.Equal(avant, TestDb.Texte("SELECT group_concat(MotifsFiche || '|' || IFNULL(MotifVerification,''), '#') FROM produits ORDER BY Id"));
    }

    [Fact]
    public void En_lecture_seule_rien_n_est_marque()
    {
        Preparer();
        var r = Lancer(appliquer: false);
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE FicheAVerifier = 1 OR StockAVerifier = 1"));
        Assert.Single(Code(r, "D25"));
        Assert.Single(Code(r, "D27"));
    }

    // ───────── prévention : vente à l'unité d'une boîte entière ou plus ─────────

    [Fact]
    public void La_proposition_en_boites_donne_les_deux_totaux_et_ne_s_applique_qu_a_partir_d_une_boite()
    {
        var p = new Produit { Nom = "Doliprane", NbUniteParBoite = 10, UniteVente = "Plaquette", PrixVente = 900m, PrixVenteUnite = 100m };

        var prop = ConditionnementService.Proposer(p, 25, "Plaquette")!;
        Assert.Equal((2, 5), (prop.Boites, prop.Reste));
        Assert.Equal(2500m, prop.TotalUnites);                  // 25 × 100
        Assert.Equal(2300m, prop.TotalBoites);                  // 2 × 900 + 5 × 100
        string message = prop.Message("Doliprane").Replace(' ', ' ');
        Assert.Contains("2 500 KMF", message);
        Assert.Contains("2 300 KMF", message);
        Assert.Contains("2 boîtes + 5 plaquettes", message);

        Assert.Equal(1, ConditionnementService.Proposer(p, 10, "Plaquette")!.Boites);        // exactement une boîte
        Assert.Null(ConditionnementService.Proposer(p, 9, "Plaquette"));                      // moins d'une boîte
        Assert.Null(ConditionnementService.Proposer(p, 25, "Boîte"));                         // déjà vendu à la boîte
        Assert.Null(ConditionnementService.Proposer(new Produit { NbUniteParBoite = 1, PrixVente = 100m }, 25, "Boîte"));
    }

    private static List<LigneVente> Panier(FormVente f)
        => (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Le_vendeur_choisit_boites_ou_unites_rien_n_est_impose(bool accepter)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid;
        using (var ctx = new AppDbContext())
        {
            var p = P("Doliprane", "Plaquette", 10, vente: 900, unitePrix: 100);
            ctx.produits.Add(p);
            ctx.SaveChanges();
            pid = p.Id;
        }
        var cts = UiHelper.FermerLesMessages(accepter);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormVente();
                Produit produit;
                using (var ctx = new AppDbContext()) produit = ctx.produits.Single(x => x.Id == pid);
                UiHelper.Appeler(f, "AjouterLigne", produit, 25, "Plaquette");
                var lignes = Panier(f);
                if (accepter)
                {
                    Assert.Equal(2, lignes.Count);
                    Assert.Equal(("Boîte", 2, 900m), (lignes[0].UniteVendue, lignes[0].Quantite, lignes[0].PrixUnitaire));
                    Assert.Equal(("Plaquette", 5, 100m), (lignes[1].UniteVendue, lignes[1].Quantite, lignes[1].PrixUnitaire));
                    Assert.Equal(2300m, lignes.Sum(l => l.SousTotal));
                }
                else
                {
                    var l = Assert.Single(lignes);
                    Assert.Equal(("Plaquette", 25, 100m), (l.UniteVendue, l.Quantite, l.PrixUnitaire));
                    Assert.Equal(2500m, l.SousTotal);
                }
                // moins d'une boîte : aucune question, la ligne est ajoutée telle quelle
                UiHelper.Appeler(f, "AjouterLigne", produit, 3, "Plaquette");
                Assert.Equal(accepter ? 8 : 28, lignes.Where(l => l.UniteVendue == "Plaquette").Sum(l => l.Quantite));
            });
        }
        finally { cts.Cancel(); }
        Assert.True(UiHelper.TitreVu("Vendre en boîtes ?"));
    }

    // ───────── prévention : fiche produit ─────────

    [Fact]
    public void La_fiche_produit_refuse_Boite_comme_unite_de_detail_puis_l_accepte_corrigee_et_leve_le_motif()
    {
        var ids = Preparer();
        Lancer(appliquer: true);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.BoiteDetail);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);              // unité « Boîte » conservée : refusé
            });
            Assert.True(UiHelper.TitreVu("Unité de détail"));
            Assert.Equal("Boîte", Lire(ids.BoiteDetail).UniteVente);
            Assert.True(Lire(ids.BoiteDetail).FicheAVerifier);

            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.BoiteDetail);
                UiHelper.Champ<ComboBox>(f, "cmbUniteVente").SelectedItem = "Comprimé";
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        var p = Lire(ids.BoiteDetail);
        Assert.Equal("Comprimé", p.UniteVente);
        Assert.False(p.FicheAVerifier);                                                          // motif « Conditionnement » levé
    }

    [Fact]
    public void Changer_le_nombre_d_unites_d_un_produit_vendu_a_l_unite_demande_une_ressaisie_du_stock_meme_a_zero()
    {
        var ids = Preparer();
        using (var ctx = new AppDbContext())
        {
            var p = ctx.produits.Single(x => x.Id == ids.EntiersSeulement);
            p.QuantiteEnStock = 0;                                                               // stock à 0 : avant, aucun avertissement
            ctx.SaveChanges();
        }
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.EntiersSeulement);
                UiHelper.Champ<NumericUpDown>(f, "numNbUniteParBoite").Value = 8;                // 5 → 8
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);              // 1er essai : avertissement, rien n'est écrit
                Assert.Equal(5, Lire(ids.EntiersSeulement).NbUniteParBoite);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);              // 2e essai : stock réel confirmé, puis enregistré
            });
        }
        finally { cts.Cancel(); }
        Assert.True(UiHelper.TitreVu("Stock à ressaisir"));
        Assert.True(UiHelper.TitreVu("Confirmer le stock"));
        Assert.Equal(8, Lire(ids.EntiersSeulement).NbUniteParBoite);
    }
}
