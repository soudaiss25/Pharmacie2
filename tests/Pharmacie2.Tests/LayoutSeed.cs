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

        var session = new SessionCaisse { UserId = caissier.Id, DateOuverture = DateTime.Today, FondOuverture = 5000, Statut = "Ouverte" };
        ctx.SessionsCaisse.Add(session);
        ctx.SaveChanges();

        ids.Admin = admin.Id; ids.Caissier = caissier.Id; ids.Produit = p1.Id; ids.ProduitPerime = p2.Id;
        ids.Fournisseur = four.Id; ids.Mutuelle = mut.IdMutuel; ids.Vente = vente.IdVente; ids.Commande = cmd.Id; ids.Session = session.Id;

        SessionUtilisateur.Courant = admin;
        return ids;
    }
}
