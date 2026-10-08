using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class ArchivageServiceTests
{
    private static Vente NouvelleVente(int? userId = null, int? mutuelId = null) => new Vente
    {
        numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "",
        MotifAchat = "", MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MotifAnnulation = "",
        UserId = userId, MutuelId = mutuelId
    };

    private static int NouvelUtilisateur(string login, string role = "Pharmacien")
    {
        using var ctx = new AppDbContext();
        var u = new User { Nom = "N", Prenom = "P", Role = role, Login = login, MotDePasse = "x" };
        ctx.Users.Add(u);
        ctx.SaveChanges();
        return u.Id;
    }

    [Fact]
    public void Produit_avec_historique_ne_se_supprime_pas_mais_s_archive_et_reste_dans_l_historique()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 7, 5);
        using (var ctx = new AppDbContext())
        {
            var v = NouvelleVente();
            v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 1, QuantiteUnites = 5 });
            ctx.ventes.Add(v);
            ctx.SaveChanges();
        }

        Assert.True(ArchivageService.AHistorique(TypeElement.Produit, pid));
        Assert.Throws<InvalidOperationException>(() => ArchivageService.SupprimerDefinitivement(TypeElement.Produit, pid));

        ArchivageService.DefinirActif(TypeElement.Produit, pid, false);
        Assert.False(ArchivageService.EstActif(TypeElement.Produit, pid));
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM LigneVentes"));   // l'historique est intact

        ArchivageService.DefinirActif(TypeElement.Produit, pid, true);
        Assert.True(ArchivageService.EstActif(TypeElement.Produit, pid));
    }

    [Fact]
    public void Produit_sans_historique_peut_etre_supprime()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Jamais vendu", 0, 1);

        Assert.False(ArchivageService.AHistorique(TypeElement.Produit, pid));
        ArchivageService.SupprimerDefinitivement(TypeElement.Produit, pid);
        Assert.Equal(0, TestDb.Scalaire($"SELECT COUNT(*) FROM produits WHERE Id={pid}"));
    }

    [Fact]
    public void Produit_archive_marque_a_verifier_ne_compte_plus_pour_le_message_de_connexion()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int p1 = TestDb.NouveauProduit("A", 10, 5);
        int p2 = TestDb.NouveauProduit("B", 10, 5);
        using (var ctx = new AppDbContext())
        {
            foreach (var p in ctx.produits) { p.StockAVerifier = true; p.UnitesManquantesEstimees = 40; }
            ctx.SaveChanges();
        }
        Assert.Equal(2, VerificationStockService.NombreAVerifier());

        ArchivageService.DefinirActif(TypeElement.Produit, p1, false);
        Assert.Equal(1, VerificationStockService.NombreAVerifier());
        ArchivageService.DefinirActif(TypeElement.Produit, p2, false);
        Assert.Equal(0, VerificationStockService.NombreAVerifier());
    }

    [Fact]
    public void Fournisseur_et_mutuelle_avec_historique_ne_se_suppriment_pas()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int fid, mid;
        using (var ctx = new AppDbContext())
        {
            var f = new Fournisseur { Nom = "Four", Contact = "" };
            var m = new Mutuel { NomEmployeur = "Mut", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 80 };
            ctx.fournisseur.Add(f); ctx.mutuels.Add(m);
            ctx.SaveChanges();
            fid = f.Id; mid = m.IdMutuel;
            ctx.commandes.Add(new Commande { FournisseurId = fid });
            var vente = NouvelleVente(mutuelId: mid);
            ctx.ventes.Add(vente);
            ctx.SaveChanges();
            ctx.MutuelPaiements.Add(new MutuelPaiement { MutuelId = mid, VenteId = vente.IdVente, NumeroPaiement = "MP-1", Montant = 100, Reference = "", Commentaire = "" });
            ctx.SaveChanges();
        }

        Assert.True(ArchivageService.AHistorique(TypeElement.Fournisseur, fid));
        Assert.True(ArchivageService.AHistorique(TypeElement.Mutuelle, mid));
        Assert.Throws<InvalidOperationException>(() => ArchivageService.SupprimerDefinitivement(TypeElement.Fournisseur, fid));
        Assert.Throws<InvalidOperationException>(() => ArchivageService.SupprimerDefinitivement(TypeElement.Mutuelle, mid));

        // Même en forçant la suppression côté EF, la base refuse (clés étrangères en RESTRICT)
        Assert.ThrowsAny<Exception>(() =>
        {
            using var ctx = new AppDbContext();
            ctx.mutuels.Remove(ctx.mutuels.Find(mid));
            ctx.SaveChanges();
        });
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM MutuelPaiements"));

        ArchivageService.DefinirActif(TypeElement.Fournisseur, fid, false);
        ArchivageService.DefinirActif(TypeElement.Mutuelle, mid, false);
        Assert.False(ArchivageService.EstActif(TypeElement.Fournisseur, fid));
        Assert.False(ArchivageService.EstActif(TypeElement.Mutuelle, mid));
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM commandes"));
    }

    [Fact]
    public void Utilisateur_archive_garde_ses_ventes_et_ses_paiements_avec_son_identifiant()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int admin = NouvelUtilisateur("admin", "Administrateur");
        int caissier = NouvelUtilisateur("caisse", "Caissier");
        SessionUtilisateur.Courant = new User { Id = admin, Nom = "A", Prenom = "A" };
        using (var ctx = new AppDbContext())
        {
            ctx.ventes.Add(NouvelleVente(userId: caissier));
            ctx.SaveChanges();
        }

        Assert.True(ArchivageService.AHistorique(TypeElement.Utilisateur, caissier));
        Assert.Throws<InvalidOperationException>(() => ArchivageService.SupprimerDefinitivement(TypeElement.Utilisateur, caissier));

        ArchivageService.DefinirActif(TypeElement.Utilisateur, caissier, false);
        Assert.False(ArchivageService.EstActif(TypeElement.Utilisateur, caissier));
        Assert.Equal(caissier, TestDb.Scalaire("SELECT UserId FROM ventes"));   // UserId n'est pas remis à NULL
    }

    [Fact]
    public void On_ne_peut_archiver_ni_soi_meme_ni_le_dernier_administrateur()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int admin1 = NouvelUtilisateur("admin1", "Administrateur");
        int admin2 = NouvelUtilisateur("admin2", "Administrateur");

        SessionUtilisateur.Courant = new User { Id = admin1, Nom = "A", Prenom = "A" };
        Assert.Throws<InvalidOperationException>(() => ArchivageService.DefinirActif(TypeElement.Utilisateur, admin1, false));   // soi-même

        ArchivageService.DefinirActif(TypeElement.Utilisateur, admin2, false);   // autre admin : possible, il en reste un
        SessionUtilisateur.Courant = new User { Id = admin2, Nom = "B", Prenom = "B" };
        Assert.Throws<InvalidOperationException>(() => ArchivageService.DefinirActif(TypeElement.Utilisateur, admin1, false));   // dernier admin actif
        Assert.True(ArchivageService.EstActif(TypeElement.Utilisateur, admin1));
    }
}
