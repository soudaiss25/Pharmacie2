using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;
using Xunit;

public class StatistiquesTests
{
    private static readonly DateTime Debut = new DateTime(2026, 10, 1);
    private static readonly DateTime Fin = new DateTime(2026, 10, 31, 23, 59, 59);

    private static Vente V(string num, string type, DateTime date, string client = "", decimal total = 0, string statut = "Active")
        => new Vente
        {
            numeroVente = num, DateVente = date, Statut = statut, NomClient = client, PrenomClient = "", TelephoneClient = "",
            MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = type, Type = type, MontantTotal = total
        };

    [Fact]
    public void Periode_precedente_de_meme_duree_juste_avant()
    {
        var (d, f) = StatistiquesService.PeriodePrecedente(Debut, Fin);
        Assert.Equal(Debut.AddTicks(-1), f);
        Assert.Equal(Fin - Debut, f - d);
    }

    [Fact]
    public void Statistiques_utilisent_les_prix_reels_et_l_argent_reellement_recu()
    {
        TestDb.Reinitialiser(); TestDb.MigrerTout();
        using (var ctx = new AppDbContext())
        {
            var p = new Produit { Nom = "Doliprane", Type = "Autre", PrixAchat = 800, PrixVente = 1500, QuantiteEnStock = 50, NbUniteParBoite = 5, UniteVente = "Plaquette", DateExpiration = Debut.AddYears(1) };
            var m = new Mutuel { NomEmployeur = "Entreprise X", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
            ctx.produits.Add(p); ctx.mutuels.Add(m); ctx.SaveChanges();

            // Comptant : 2 boîtes (3000) + 3 plaquettes à 300 (900)
            var v1 = V("V-1", ModesPaiement.Comptant, Debut.AddDays(2), total: 3900);
            v1.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 2, QuantiteUnites = 10, PrixUnitaire = 1500, UniteVendue = "Boîte" });
            v1.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 3, QuantiteUnites = 3, PrixUnitaire = 300, UniteVendue = "Plaquette" });
            v1.Paiements.Add(new Paiement { NumeroPaiement = "P1", Montant = 3900, DatePaiement = Debut.AddDays(2) });

            // Mvola
            var v2 = V("V-2", ModesPaiement.Mvola, Debut.AddDays(3), total: 1500);
            v2.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 1500, UniteVendue = "Boîte" });
            v2.Paiements.Add(new Paiement { NumeroPaiement = "P2", Montant = 1500, DatePaiement = Debut.AddDays(3) });

            // Crédit : Ali doit 2000 (1000 versés)
            var v3 = V("V-3", ModesPaiement.Credit, Debut.AddDays(4), "Ali", 3000);
            v3.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 2, QuantiteUnites = 10, PrixUnitaire = 1500, UniteVendue = "Boîte" });
            v3.Paiements.Add(new Paiement { NumeroPaiement = "P3", Montant = 1000, DatePaiement = Debut.AddDays(4) });

            // Mutuelle : part entreprise 750 non réglée, patient a payé sa part
            var v4 = V("V-4", ModesPaiement.Mutuelle, Debut.AddDays(5), total: 1500);
            v4.MutuelId = m.IdMutuel; v4.MontantMutuelle = 750; v4.TauxMutuelle = 50;
            v4.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 1500, UniteVendue = "Boîte" });
            v4.Paiements.Add(new Paiement { NumeroPaiement = "P4", Montant = 750, DatePaiement = Debut.AddDays(5) });

            // Annulée : ignorée partout
            var v5 = V("V-5", ModesPaiement.Comptant, Debut.AddDays(6), total: 9999, statut: "Annulée");
            v5.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 9999, UniteVendue = "Boîte" });
            v5.Paiements.Add(new Paiement { NumeroPaiement = "P5", Montant = 9999, DatePaiement = Debut.AddDays(6) });

            ctx.ventes.AddRange(v1, v2, v3, v4, v5);
            ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Loyer", Description = "", Montant = 1000, DateDepense = Debut.AddDays(10) });
            ctx.SaveChanges();
        }

        var s = StatistiquesService.Calculer(Debut, Fin);

        // CA = 3900 + 1500 + 3000 + 1500 ; coût = (10+3)/5 x 800 + 800 + 1600 + 800 = 2080 + 3200 = 5280
        Assert.Equal(9900m, s.CA);
        Assert.Equal(9900m - 5280m, s.MargeBrute);
        Assert.Equal(1000m, s.Depenses);
        Assert.Equal(9900m - 5280m - 1000m, s.BeneficeNet);
        Assert.Equal(4, s.NbVentes);

        Assert.Equal(3900m, s.Modes.Single(m => m.Mode == "Espèces").Recu);
        Assert.Equal(1500m, s.Modes.Single(m => m.Mode == "Mvola / Huri Money").Recu);
        Assert.Equal(1000m, s.Modes.Single(m => m.Mode.StartsWith("Crédit")).Recu);
        Assert.Equal(750m, s.Modes.Single(m => m.Mode.StartsWith("Mutuelle")).Recu);
        Assert.Equal(100m, Math.Round(s.Modes.Sum(m => m.Pourcentage)), 0);

        var credit = Assert.Single(s.Credits);
        Assert.Equal("Ali", credit.Client);
        Assert.Equal(2000m, credit.Restant);
        var mut = Assert.Single(s.Mutuelles);
        Assert.Equal("Entreprise X", mut.Mutuelle);
        Assert.Equal(750m, mut.TotalDu);
        Assert.Equal(2000m + 750m, s.ArgentARecuperer);
        Assert.Equal(2, s.NbVentesASolder);

        var top = Assert.Single(s.TopProduits);
        Assert.Equal("Doliprane", top.Produit);
        Assert.Equal(9900m, top.CA);
        Assert.Contains("boîte", top.Vendu);
    }

    [Fact]
    public void Ecran_statistiques_alertes_en_premier_dont_les_produits_deja_perimes()
    {
        LayoutSeed.Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.produits.Add(new Produit { Nom = "Sirop périmé en stock", Type = "Sirop", QuantiteEnStock = 4, NbUniteParBoite = 1, UniteVente = "Boîte", DateExpiration = DateTime.Today.AddDays(-10) });
            ctx.SaveChanges();
        }
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var uc = new Uc_Statistique();
                var liste = UiHelper.Champ<ListeActions>(uc, "listeAlertes");
                var premiere = (Label)liste.Controls[0];
                Assert.Contains("périmé", premiere.Text);                      // le plus urgent d'abord
                Assert.Contains("Sirop périmé en stock", premiere.Text);       // périmés DÉJÀ passés inclus
                Assert.Contains("KMF", UiHelper.Champ<CarteKpi>(uc, "carteCA").Valeur);
                Assert.NotEmpty((System.Collections.IList)UiHelper.Champ<DataGridView>(uc, "dgvVentilation").DataSource);
            });
        }
        finally { cts.Cancel(); }
    }
}
