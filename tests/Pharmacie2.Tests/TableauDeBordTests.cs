using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class TableauDeBordTests
{
    // jeudi 15 octobre 2026, 14 h 00
    private static readonly DateTime Maintenant = new DateTime(2026, 10, 15, 14, 0, 0);

    private static Vente NouvelleVente(string numero, DateTime date, string statut = "Active", decimal total = 0, string client = "")
        => new Vente
        {
            numeroVente = numero, DateVente = date, Statut = statut, NomClient = client, PrenomClient = "", TelephoneClient = "",
            MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MontantTotal = total
        };

    private static void Preparer() { TestDb.Reinitialiser(); TestDb.MigrerTout(); }

    [Fact]
    public void Encaisse_aujourdhui_compare_a_hier_a_la_meme_heure_sans_les_ventes_annulees()
    {
        Preparer();
        using (var ctx = new AppDbContext())
        {
            var v1 = NouvelleVente("V-1", Maintenant.Date.AddHours(9), total: 1000);
            v1.Paiements.Add(new Paiement { NumeroPaiement = "P1", Montant = 1000, DatePaiement = Maintenant.Date.AddHours(10) });
            v1.Paiements.Add(new Paiement { NumeroPaiement = "P2", Montant = 300, DatePaiement = Maintenant.Date.AddHours(15) });    // dans le futur : ignoré
            var v2 = NouvelleVente("V-2", Maintenant.Date.AddDays(-1).AddHours(9), total: 800);
            v2.Paiements.Add(new Paiement { NumeroPaiement = "P3", Montant = 800, DatePaiement = Maintenant.Date.AddDays(-1).AddHours(9) });
            v2.Paiements.Add(new Paiement { NumeroPaiement = "P4", Montant = 400, DatePaiement = Maintenant.Date.AddDays(-1).AddHours(16) }); // après 14 h hier : ignoré
            var annulee = NouvelleVente("V-3", Maintenant.Date.AddHours(8), statut: "Annulée", total: 5000);
            annulee.Paiements.Add(new Paiement { NumeroPaiement = "P5", Montant = 5000, DatePaiement = Maintenant.Date.AddHours(8) });
            ctx.ventes.AddRange(v1, v2, annulee);
            ctx.SaveChanges();

            var m = new Mutuel { NomEmployeur = "M", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
            ctx.mutuels.Add(m);
            ctx.SaveChanges();
            ctx.MutuelPaiements.Add(new MutuelPaiement { MutuelId = m.IdMutuel, VenteId = v1.IdVente, NumeroPaiement = "MP1", Montant = 500, DatePaiement = Maintenant.Date.AddHours(11), Reference = "", Commentaire = "" });
            ctx.SaveChanges();
        }

        var k = TableauDeBordService.Kpis(Maintenant);

        Assert.Equal(1500m, k.EncaisseAujourdhui);   // 1000 + 500 de la mutuelle
        Assert.Equal(800m, k.EncaisseHier);
        Assert.Equal(1, k.VentesAujourdhui);          // l'annulée n'est pas comptée
        Assert.Equal(1, k.VentesHier);
    }

    [Fact]
    public void Argent_a_recuperer_credits_et_parts_mutuelle_non_reglees()
    {
        Preparer();
        using (var ctx = new AppDbContext())
        {
            var m = new Mutuel { NomEmployeur = "M", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 80 };
            ctx.mutuels.Add(m);
            ctx.SaveChanges();

            var credit = NouvelleVente("V-1", Maintenant.AddDays(-3), total: 3000, client: "Ali");
            credit.Paiements.Add(new Paiement { NumeroPaiement = "P1", Montant = 1000, DatePaiement = Maintenant.AddDays(-3) });     // reste 2000
            var credit2 = NouvelleVente("V-2", Maintenant.AddDays(-2), total: 500, client: "Ali");                                    // même client : reste 500
            var mut = NouvelleVente("V-3", Maintenant.AddDays(-5), total: 1000);
            mut.MutuelId = m.IdMutuel; mut.MontantMutuelle = 800; mut.TauxMutuelle = 80;
            mut.Paiements.Add(new Paiement { NumeroPaiement = "P2", Montant = 200, DatePaiement = Maintenant.AddDays(-5) });         // patient réglé, mutuelle doit 800
            var annulee = NouvelleVente("V-4", Maintenant.AddDays(-1), statut: "Annulée", total: 9999);
            var soldee = NouvelleVente("V-5", Maintenant.AddDays(-1), total: 100);
            soldee.Paiements.Add(new Paiement { NumeroPaiement = "P3", Montant = 100, DatePaiement = Maintenant.AddDays(-1) });
            ctx.ventes.AddRange(credit, credit2, mut, annulee, soldee);
            ctx.SaveChanges();
        }

        var k = TableauDeBordService.Kpis(Maintenant);

        Assert.Equal(2000m + 500m + 800m, k.ArgentARecuperer);
        Assert.Equal(1, k.ClientsDebiteurs);     // « Ali » deux fois = un client
        Assert.Equal(1, k.MutuellesDebitrices);
    }

    [Fact]
    public void Benefice_utilise_les_prix_reels_de_vente_et_le_cout_a_l_unite_moins_les_depenses()
    {
        Preparer();
        using (var ctx = new AppDbContext())
        {
            var p = new Produit
            {
                Nom = "Doliprane", Type = "Autre", PrixAchat = 800, PrixVente = 1500, QuantiteEnStock = 100, NbUniteParBoite = 5,
                UniteVente = "Plaquette", DateExpiration = Maintenant.AddYears(1)
            };
            ctx.produits.Add(p);
            ctx.SaveChanges();

            var v = NouvelleVente("V-1", Maintenant.Date.AddDays(-3));
            v.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 2, QuantiteUnites = 10, PrixUnitaire = 1500, UniteVendue = "Boîte" });   // CA 3000, coût 1600
            v.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 3, QuantiteUnites = 3, PrixUnitaire = 300, UniteVendue = "Plaquette" });   // CA 900, coût 3 x 160 = 480
            ctx.ventes.Add(v);
            ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Loyer", Description = "", Montant = 200, DateDepense = Maintenant.Date.AddDays(-2) });

            // mois précédent, même nombre de jours (1..15 septembre)
            var vp = NouvelleVente("V-0", new DateTime(2026, 9, 10, 10, 0, 0));
            vp.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 1500, UniteVendue = "Boîte" });    // 1500 - 800 = 700
            var apres = NouvelleVente("V-00", new DateTime(2026, 9, 20, 10, 0, 0));
            apres.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 5, PrixUnitaire = 1500, UniteVendue = "Boîte" });  // après le 15 : ignorée
            ctx.ventes.AddRange(vp, apres);
            ctx.SaveChanges();
        }

        var k = TableauDeBordService.Kpis(Maintenant);

        Assert.Equal((3000m - 1600m) + (900m - 480m) - 200m, k.BeneficeMois);   // 1400 + 420 - 200 = 1620
        Assert.Equal(700m, k.BeneficeMoisPrecedent);
    }

    [Fact]
    public void A_faire_ordonne_par_urgence_et_exclut_les_produits_archives()
    {
        Preparer();
        using (var ctx = new AppDbContext())
        {
            Produit P(string nom, int stock, DateTime exp, bool actif = true, bool averifier = false)
                => new Produit { Nom = nom, Type = "Autre", QuantiteEnStock = stock, NbUniteParBoite = 1, UniteVente = "Boîte", DateExpiration = exp, Actif = actif, StockAVerifier = averifier };

            ctx.produits.AddRange(
                P("Périmé en stock", 5, Maintenant.Date.AddDays(-3)),
                P("Périmé archivé", 5, Maintenant.Date.AddDays(-3), actif: false),
                P("Périmé mais plus de stock", 0, Maintenant.Date.AddDays(-30)),
                P("Rupture", 0, Maintenant.Date.AddYears(1)),
                P("Rupture archivée", 0, Maintenant.Date.AddYears(1), actif: false),
                P("Bientôt périmé", 4, Maintenant.Date.AddDays(10)),
                P("À vérifier", 8, Maintenant.Date.AddYears(1), averifier: true),
                P("À vérifier archivé", 8, Maintenant.Date.AddYears(1), actif: false, averifier: true),
                P("Tout va bien", 8, Maintenant.Date.AddYears(1)));

            var m = new Mutuel { NomEmployeur = "M", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 80 };
            ctx.mutuels.Add(m);
            ctx.SaveChanges();
            var ancienne = NouvelleVente("V-1", Maintenant.AddDays(-45), total: 1000);
            ancienne.MutuelId = m.IdMutuel; ancienne.MontantMutuelle = 800;
            var recente = NouvelleVente("V-2", Maintenant.AddDays(-5), total: 1000);
            recente.MutuelId = m.IdMutuel; recente.MontantMutuelle = 800;
            ctx.ventes.AddRange(ancienne, recente);
            ctx.SaveChanges();
        }

        var a = TableauDeBordService.AFaireMaintenant(Maintenant);

        Assert.Equal(new[] { TypeAFaire.Perimes, TypeAFaire.Ruptures, TypeAFaire.PeremptionProche, TypeAFaire.StocksAVerifier, TypeAFaire.MutuellesEnRetard },
                     a.Select(x => x.Type).ToArray());
        Assert.Equal("urgent", a[0].Niveau);
        Assert.Equal("urgent", a[1].Niveau);
        Assert.Equal("attention", a[2].Niveau);
        Assert.Equal(1, a[0].Nombre);     // seulement « Périmé en stock »
        Assert.Contains("Périmé en stock", a[0].Texte);
        Assert.DoesNotContain("archivé", a[0].Texte);
        Assert.Equal(2, a[1].Nombre);   // « Rupture » + « Périmé mais plus de stock » (archivés exclus)
        Assert.Equal(1, a[3].Nombre);
        Assert.Equal(1, a[4].Nombre);     // la vente de 45 jours compte, pas celle de 5 jours
        Assert.Contains("800 KMF", a[4].Texte);
    }

    [Fact]
    public void A_faire_vide_quand_tout_va_bien_et_graphique_sur_7_jours()
    {
        Preparer();
        TestDb.NouveauProduit("Bon produit", 20, 1, "Boîte");
        Assert.Empty(TableauDeBordService.AFaireMaintenant(Maintenant));

        var g = TableauDeBordService.Encaissements7Jours(Maintenant);
        Assert.Equal(7, g.Count);
        Assert.StartsWith("jeu", g[^1].etiquette);   // aujourd'hui (jeudi 15) en dernier
        Assert.StartsWith("ven", g[0].etiquette);
    }
}
