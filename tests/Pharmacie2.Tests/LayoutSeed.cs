using Pharmacie2.Models;
using Pharmacie2.Services;

/// <summary>Données minimales pour pouvoir ouvrir tous les écrans dans le test de mise en page.</summary>
public static class LayoutSeed
{
    public sealed class Ids
    {
        public int Admin, Caissier, Produit, ProduitPerime, Fournisseur, Mutuelle, Vente, Commande, Session;
    }

    public static Ids Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ids = new Ids();
        using var ctx = new AppDbContext();

        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("1234") };
        var caissier = new User { Nom = "Ali", Prenom = "Hamidou", Role = Roles.Caissier, Login = "caisse", MotDePasse = MotDePasseService.Hacher("1234") };
        ctx.Users.AddRange(admin, caissier);

        var four = new Fournisseur { Nom = "Pharma Distribution", Contact = "+269 000 00 00" };
        var mut = new Mutuel { NomEmployeur = "Entreprise Moheli", EmailContact = "contact@exemple.km", telephoneEmployeur = "+269 111 11 11", TauxPriseEnCharge = 80 };
        ctx.fournisseur.Add(four);
        ctx.mutuels.Add(mut);
        ctx.SaveChanges();

        var p1 = new Produit
        {
            Nom = "Doliprane 500 mg", Type = "Médicament", PrixAchat = 800, PrixVente = 1500, MargeBeneficiaire = 87.5m,
            QuantiteEnStock = 9, NbUniteParBoite = 5, UniteVente = "Plaquette", SeuilAlerte = 2,
            DateExpiration = DateTime.Today.AddMonths(8), FournisseurId = four.Id, StockAVerifier = true, UnitesManquantesEstimees = 40,
            MotifVerification = "10 boîte(s) reçue(s) avant la correction : seulement 10 unité(s) ajoutée(s) au lieu de 50."
        };
        var p2 = new Produit
        {
            Nom = "Sirop périmé", Type = "Sirop", PrixAchat = 500, PrixVente = 900, MargeBeneficiaire = 80,
            QuantiteEnStock = 0, NbUniteParBoite = 1, UniteVente = "Boîte", SeuilAlerte = 1,
            DateExpiration = DateTime.Today.AddDays(-20), FournisseurId = four.Id
        };
        ctx.produits.AddRange(p1, p2);
        ctx.SaveChanges();

        var vente = new Vente
        {
            numeroVente = "V-000001", Statut = "Active", NomClient = "Client", PrenomClient = "Test", TelephoneClient = "",
            MotifAchat = "", MatriculeEmploye = "M1", MoyenPaiement = ModesPaiement.Mutuelle, Type = ModesPaiement.Mutuelle,
            MontantTotal = 3000, MontantMutuelle = 2400, TauxMutuelle = 80, MutuelId = mut.IdMutuel, UserId = caissier.Id,
            DateVente = DateTime.Now.AddDays(-40)
        };
        vente.Lignes.Add(new LigneVente { ProduitId = p1.Id, Quantite = 2, QuantiteUnites = 10, PrixUnitaire = 1500, UniteVendue = "Boîte" });
        vente.Paiements.Add(new Paiement { NumeroPaiement = "P-1", Montant = 600, UserId = caissier.Id });
        ctx.ventes.Add(vente);

        var cmd = new Commande { FournisseurId = four.Id, Statut = "En attente" };
        cmd.Lignes.Add(new LigneCommande { ProduitId = p1.Id, Quantite = 10, PrixAchatUnitaire = 800 });
        ctx.commandes.Add(cmd);

        var session = new SessionCaisse { UserId = caissier.Id, DateOuverture = DateTime.Today, FondOuverture = 5000, Statut = "Ouverte", CommentaireCloture = "" };
        ctx.SessionsCaisse.Add(session);
        ctx.SaveChanges();

        ids.Admin = admin.Id; ids.Caissier = caissier.Id; ids.Produit = p1.Id; ids.ProduitPerime = p2.Id;
        ids.Fournisseur = four.Id; ids.Mutuelle = mut.IdMutuel; ids.Vente = vente.IdVente; ids.Commande = cmd.Id; ids.Session = session.Id;

        SessionUtilisateur.Courant = admin;
        return ids;
    }

    /// <summary>Base de démonstration réaliste : 30 produits, ventes sur 7 jours, crédits, mutuelle impayée, périmé, à vérifier.</summary>
    public static Ids PreparerDemo()
    {
        var ids = Preparer();
        using var ctx = new AppDbContext();
        var four = ctx.fournisseur.First();
        var caissier = ctx.Users.First(u => u.Login == "caisse");
        var rng = new Random(42);

        string[] noms =
        {
            "Paracétamol 1 g", "Amoxicilline 500 mg", "Ibuprofène 400 mg", "Aspirine 500 mg", "Oméprazole 20 mg", "Métronidazole 250 mg",
            "Ciprofloxacine 500 mg", "Artéméther-luméfantrine", "Sérum physiologique", "Vitamine C 500 mg", "Sirop antitussif", "Amlodipine 5 mg",
            "Metformine 850 mg", "Salbutamol spray", "Loratadine 10 mg", "Diclofénac gel", "Cotrimoxazole 480 mg", "Zinc 20 mg",
            "Sels de réhydratation", "Povidone iodée", "Compresses stériles", "Bandes de gaze", "Gants d'examen", "Thermomètre digital",
            "Fer + acide folique", "Albendazole 400 mg", "Hydrocortisone crème", "Collyre antibiotique", "Gouttes nasales", "Pansements assortis"
        };
        var produits = new List<Produit>();
        for (int i = 0; i < noms.Length; i++)
        {
            int parBoite = i % 3 == 0 ? 10 : (i % 3 == 1 ? 1 : 20);
            var p = new Produit
            {
                Nom = noms[i], Type = i % 4 == 0 ? "Sirop" : (i % 4 == 1 ? "Comprimé" : "Autre"),
                PrixAchat = 300 + i * 40, PrixVente = 500 + i * 70, MargeBeneficiaire = 40 + i % 30,
                QuantiteEnStock = i % 9 == 0 ? 0 : (5 + i * 7) % 90, NbUniteParBoite = parBoite, UniteVente = parBoite > 1 ? "Plaquette" : "Boîte",
                SeuilAlerte = 3, DateExpiration = i == 5 ? DateTime.Today.AddDays(-12) : (i == 8 ? DateTime.Today.AddDays(15) : DateTime.Today.AddMonths(6 + i % 14)),
                FournisseurId = four.Id
            };
            produits.Add(p);
        }
        ctx.produits.AddRange(produits);
        ctx.SaveChanges();

        string[] clients = { "Ali Mohamed", "Fatima Said", "Ibrahim Abdou", "Mariam Soilihi", "Youssouf Ahmed", "Nadia Hassane", "Salim Ali" };
        int num = 100;
        for (int jour = 6; jour >= 0; jour--)
        {
            int nb = 2 + (jour * 3) % 4;
            for (int k = 0; k < nb; k++)
            {
                var p = produits[rng.Next(produits.Count)];
                int qte = 1 + rng.Next(3);
                decimal total = p.PrixVente * qte;
                bool credit = (jour + k) % 5 == 0;
                string mode = credit ? ModesPaiement.Credit : (k % 3 == 1 ? ModesPaiement.Mvola : ModesPaiement.Comptant);
                var c = clients[(jour + k) % clients.Length].Split(' ');
                var v = new Vente
                {
                    numeroVente = "V-" + (++num).ToString("D6"), Statut = "Active", PrenomClient = c[0], NomClient = c[1], TelephoneClient = "", MotifAchat = "",
                    MatriculeEmploye = "", MoyenPaiement = mode, Type = mode, MontantTotal = total, UserId = caissier.Id,
                    DateVente = DateTime.Today.AddDays(-jour).AddHours(8 + k * 2)
                };
                v.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = qte, QuantiteUnites = qte * p.NbUniteParBoite, PrixUnitaire = p.PrixVente, UniteVendue = "Boîte" });
                decimal verse = credit ? Math.Round(total / 3, 0) : total;
                v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + num, Montant = verse, UserId = caissier.Id, DatePaiement = v.DateVente });
                ctx.ventes.Add(v);
            }
        }
        ctx.SaveChanges();
        return ids;
    }
}
