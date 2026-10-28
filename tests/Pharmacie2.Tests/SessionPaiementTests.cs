using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Caisse : chaque paiement est rattaché au bon tiroir (session), jamais au nom de celui qui a simplement tenu le clavier.</summary>
public class SessionPaiementTests
{
    private sealed class Ctx { public int Admin, Caissier, SessionCaissier, Produit; }

    private static Ctx Preparer(bool sessionCaissierOuverte = true, bool sessionAdmin = false)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var c = new Ctx();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        var caissier = new User { Nom = "Hamidou", Prenom = "Ali", Role = Roles.Caissier, Login = "caisse", MotDePasse = MotDePasseService.Hacher("x") };
        using (var ctx = new AppDbContext())
        {
            ctx.Users.AddRange(admin, caissier);
            ctx.SaveChanges();
            var s = new SessionCaisse { UserId = caissier.Id, DateOuverture = DateTime.Today, Statut = sessionCaissierOuverte ? "Ouverte" : "Clôturée", CommentaireCloture = "" };
            ctx.SessionsCaisse.Add(s);
            if (sessionAdmin) ctx.SessionsCaisse.Add(new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today.AddHours(1), Statut = "Ouverte", CommentaireCloture = "" });
            ctx.SaveChanges();
            c.SessionCaissier = s.Id;
        }
        c.Admin = admin.Id; c.Caissier = caissier.Id;
        c.Produit = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 1000m);
        SessionUtilisateur.Courant = admin;          // c'est l'ADMINISTRATEUR qui encaisse
        UiHelper.ViderTitres();
        return c;
    }

    /// <summary>« Oui » à la question du tiroir, « Non » à la proposition d'imprimer la facture.</summary>
    private static CancellationTokenSource Reponses(bool ouiAuTiroir) => UiHelper.FermerLesMessages(titre => titre == "Caisse" && ouiAuTiroir);

    private static void Vendre(int produitId, string mode, int boites, decimal? especes, bool ouiAuTiroir = true)
    {
        var cts = Reponses(ouiAuTiroir);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormVente();
                var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
                lignes.Add(new LigneVente
                {
                    ProduitId = produitId, Quantite = boites, PrixUnitaire = 1000m, UniteVendue = "Boîte",
                    Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Boîte" }
                });
                UiHelper.Appeler(f, "RafraichirGrille");
                UiHelper.Appeler(f, "RecalculerTotal");
                UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = mode;
                if (especes.HasValue) UiHelper.Champ<NumericUpDown>(f, "numEspeces").Value = especes.Value;
                UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
    }

    private static decimal Attendu(int sessionId)
    {
        using var ctx = new AppDbContext();
        return CaisseService.EspecesAttendues(ctx, ctx.SessionsCaisse.Single(s => s.Id == sessionId));
    }

    // ───────── l'argent va dans le tiroir du caissier, pas sous le nom de l'administrateur ─────────

    [Fact]
    public void Encaissement_par_l_administrateur_sans_session_est_compte_dans_la_session_du_caissier_apres_confirmation()
    {
        var c = Preparer();
        Vendre(c.Produit, ModesPaiement.Comptant, 3, 3000);

        Assert.True(UiHelper.TitreVu("Caisse"));                                  // « Cet argent va dans la caisse de Ali Hamidou ? »
        using var ctx = new AppDbContext();
        var p = ctx.paiement.Single();
        Assert.Equal(c.SessionCaissier, p.SessionCaisseId);                       // rattaché au tiroir du caissier…
        Assert.Equal(c.Admin, p.UserId);                                          // … même si c'est l'administrateur qui l'a encaissé
        Assert.Equal(3000m, Attendu(c.SessionCaissier));                          // le tiroir du caissier contient bien cet argent : plus de faux excédent
        Assert.Equal(97, ctx.produits.Single().QuantiteEnStock);
    }

    [Fact]
    public void Si_l_administrateur_repond_Non_rien_n_est_enregistre()
    {
        var c = Preparer();
        Vendre(c.Produit, ModesPaiement.Comptant, 3, 3000, ouiAuTiroir: false);

        Assert.True(UiHelper.TitreVu("Caisse"));
        Assert.False(UiHelper.TitreVu("Encaissement impossible"));                // refus voulu : pas de message en plus
        using var ctx = new AppDbContext();
        Assert.Empty(ctx.ventes);
        Assert.Empty(ctx.paiement);
        Assert.Equal(100, ctx.produits.Single().QuantiteEnStock);
        Assert.Equal(0m, Attendu(c.SessionCaissier));
    }

    [Fact]
    public void Sans_aucune_session_ouverte_un_paiement_en_especes_est_refuse_avec_un_message_clair_mais_pas_une_carte()
    {
        var c = Preparer(sessionCaissierOuverte: false);
        Vendre(c.Produit, ModesPaiement.Comptant, 3, 3000);

        Assert.True(UiHelper.TitreVu("Encaissement impossible"));
        Assert.False(UiHelper.TitreVu("Caisse"));                                 // personne à qui rattacher l'argent : aucune question
        using (var ctx = new AppDbContext())
        {
            Assert.Empty(ctx.ventes);
            Assert.Empty(ctx.paiement);
            Assert.Equal(100, ctx.produits.Single().QuantiteEnStock);
        }

        // Carte, chèque, Mvola, Huri Money ne passent pas par le tiroir : jamais refusés
        Vendre(c.Produit, ModesPaiement.CarteBancaire, 2, null);
        using var ctx2 = new AppDbContext();
        var p = ctx2.paiement.Single();
        Assert.Equal((2000m, ModesPaiement.CarteBancaire), (p.Montant, p.Mode));
        Assert.Null(p.SessionCaisseId);
    }

    [Fact]
    public void L_utilisateur_qui_a_sa_propre_session_encaisse_dans_son_tiroir_sans_aucune_question()
    {
        var c = Preparer(sessionAdmin: true);
        Vendre(c.Produit, ModesPaiement.Comptant, 2, 2000);

        Assert.False(UiHelper.TitreVu("Caisse"));
        using var ctx = new AppDbContext();
        var p = ctx.paiement.Single();
        var sessionAdmin = ctx.SessionsCaisse.Single(s => s.UserId == c.Admin);
        Assert.Equal(sessionAdmin.Id, p.SessionCaisseId);
        Assert.Equal(2000m, CaisseService.EspecesAttendues(ctx, sessionAdmin));
        Assert.Equal(0m, Attendu(c.SessionCaissier));
    }

    [Fact]
    public void Un_remboursement_de_credit_en_especes_par_l_administrateur_va_aussi_dans_le_tiroir_du_caissier()
    {
        var c = Preparer();
        int venteId;
        using (var ctx = new AppDbContext())
        {
            var v = new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "N", PrenomClient = "P", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
                MoyenPaiement = ModesPaiement.Credit, Type = ModesPaiement.Credit, MotifAnnulation = "", MontantTotal = 5000, UserId = c.Caissier, DateVente = DateTime.Now.AddDays(-1)
            };
            ctx.ventes.Add(v);
            ctx.SaveChanges();
            venteId = v.IdVente;
        }
        var cts = Reponses(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormPaiements(venteId);
                UiHelper.Champ<NumericUpDown>(f, "numMontant").Value = 1500;
                UiHelper.Appeler(f, "btnAjouterPaiement_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        using var c2 = new AppDbContext();
        Assert.Equal(c.SessionCaissier, c2.paiement.Single().SessionCaisseId);
        Assert.Equal(1500m, Attendu(c.SessionCaissier));
    }

    // ───────── anciens paiements : la règle d'origine est conservée ─────────

    [Fact]
    public void Les_anciens_paiements_sans_session_gardent_la_regle_d_origine_et_un_paiement_rattache_ailleurs_n_est_pas_compte_ici()
    {
        var c = Preparer();
        using (var ctx = new AppDbContext())
        {
            var v = new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
                MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = 9000, UserId = c.Caissier, DateVente = DateTime.Now.AddMinutes(-10)
            };
            var autreSession = new SessionCaisse { UserId = c.Admin, DateOuverture = DateTime.Now.AddMinutes(-5), Statut = "Ouverte", CommentaireCloture = "" };
            ctx.SessionsCaisse.Add(autreSession);
            ctx.SaveChanges();
            v.Paiements.Add(new Paiement { NumeroPaiement = "P-ANCIEN", Montant = 1000, Mode = ModesPaiement.Especes, UserId = c.Caissier, DatePaiement = DateTime.Now.AddMinutes(-9) });      // ancien : sans session, caissier de la session
            v.Paiements.Add(new Paiement { NumeroPaiement = "P-ANCIEN-AUTRE", Montant = 700, Mode = ModesPaiement.Especes, UserId = c.Admin, DatePaiement = DateTime.Now.AddMinutes(-8) });     // ancien : sans session, autre utilisateur : pas dans ce tiroir
            v.Paiements.Add(new Paiement { NumeroPaiement = "P-RATTACHE", Montant = 500, Mode = ModesPaiement.Especes, UserId = c.Caissier, SessionCaisseId = autreSession.Id, DatePaiement = DateTime.Now.AddMinutes(-7) });   // rattaché à un AUTRE tiroir
            v.Paiements.Add(new Paiement { NumeroPaiement = "P-ICI", Montant = 300, Mode = ModesPaiement.Especes, UserId = c.Admin, SessionCaisseId = c.SessionCaissier, DatePaiement = DateTime.Now.AddMinutes(-6) });         // rattaché à ce tiroir par l'admin
            ctx.ventes.Add(v);
            ctx.SaveChanges();
        }
        Assert.Equal(1300m, Attendu(c.SessionCaissier));                              // 1 000 (ancien) + 300 (rattaché)
        using var c2 = new AppDbContext();
        Assert.Equal(500m, CaisseService.EspecesAttendues(c2, c2.SessionsCaisse.Single(s => s.UserId == c.Admin)));
    }

    [Fact]
    public void Le_service_d_affectation_respecte_les_regles_sans_interface()
    {
        var c = Preparer();
        {
        using var ctx = new AppDbContext();
        var avecQuestion = CaisseService.AffecterPaiement(ctx, c.Admin, ModesPaiement.Especes, nom => { Assert.Equal("Ali Hamidou", nom); return true; });
        Assert.Equal((true, (int?)c.SessionCaissier, (string?)null), (avecQuestion.Autorise, avecQuestion.SessionCaisseId, avecQuestion.Message));
        var mvola = CaisseService.AffecterPaiement(ctx, c.Admin, ModesPaiement.Mvola, _ => throw new InvalidOperationException("aucune question pour Mvola"));
        Assert.True(mvola.Autorise);
        Assert.Null(mvola.SessionCaisseId);
        var propre = CaisseService.AffecterPaiement(ctx, c.Caissier, ModesPaiement.Especes, _ => throw new InvalidOperationException("sa propre session : aucune question"));
        Assert.Equal(c.SessionCaissier, propre.SessionCaisseId);
        }

        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        using var vide = new AppDbContext();
        var refus = CaisseService.AffecterPaiement(vide, 1, ModesPaiement.Especes, _ => true);
        Assert.False(refus.Autorise);
        Assert.Equal(CaisseService.MessageAucuneCaisse, refus.Message);
    }

    [Fact]
    public void Migration_AjoutSessionPaiement_ne_perd_rien_et_laisse_la_colonne_vide()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutCoutAchatLigneVente"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "ventes", new() { ["IdVente"] = 1L, ["numeroVente"] = "V-000001", ["Statut"] = "Active", ["MontantTotal"] = "1000", ["UserId"] = 1L });
            TestDb.Inserer(c, "paiement", new() { ["Id"] = 1L, ["NumeroPaiement"] = "P-1", ["VenteId"] = 1L, ["Montant"] = "1000", ["UserId"] = 1L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutSessionPaiement"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM paiement WHERE SessionCaisseId IS NULL"));
    }
}
