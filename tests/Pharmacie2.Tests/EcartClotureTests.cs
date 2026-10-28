using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Écarts de caisse : motif obligatoire au-delà de 100 KMF, statistiques et alerte de répétition.</summary>
public class EcartClotureTests
{
    private static readonly DateTime Maintenant = DateTime.Now;

    private static (int ali, int fatima) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ali = new User { Nom = "Hamidou", Prenom = "Ali", Role = Roles.Caissier, Login = "ali", MotDePasse = MotDePasseService.Hacher("x") };
        var fatima = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Caissier, Login = "fatima", MotDePasse = MotDePasseService.Hacher("x") };
        using var ctx = new AppDbContext();
        ctx.Users.AddRange(ali, fatima);
        ctx.SaveChanges();
        return (ali.Id, fatima.Id);
    }

    private static int SessionOuverte(int userId)
    {
        using var ctx = new AppDbContext();
        var s = new SessionCaisse { UserId = userId, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" };
        ctx.SessionsCaisse.Add(s);
        // 5 000 KMF d'espèces reçus pendant la session : c'est le montant attendu
        var v = new Vente
        {
            numeroVente = "V-" + userId, Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = 5000, UserId = userId, DateVente = DateTime.Now.AddMinutes(-3)
        };
        v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + userId, Montant = 5000, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = DateTime.Now.AddMinutes(-2) });
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        return s.Id;
    }

    private static int Cloturer(int userId, decimal compte, Func<decimal, (string, string)?>? demande, out decimal? ecartDemande)
    {
        decimal? vu = null;
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormSessionCaisse(userId);
                f.DemandeMotifEcart = ecart => { vu = ecart; var r = demande?.Invoke(ecart); return r == null ? null : (r.Value.Item1, r.Value.Item2); };
                UiHelper.Champ<NumericUpDown>(f, "numMontantReel").Value = compte;
                UiHelper.Appeler(f, "btnCloture_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        ecartDemande = vu;
        using var ctx = new AppDbContext();
        return ctx.SessionsCaisse.Where(s => s.UserId == userId).Select(s => s.Id).First();
    }

    private static SessionCaisse Lire(int id) { using var ctx = new AppDbContext(); return ctx.SessionsCaisse.AsNoTracking().Single(s => s.Id == id); }

    // ───────── clôture ─────────

    [Fact]
    public void Jusqu_a_100_KMF_d_ecart_aucune_question_et_pas_de_mot_de_passe()
    {
        var (ali, _) = Preparer();
        int sid = SessionOuverte(ali);
        Cloturer(ali, 5100m, _ => throw new InvalidOperationException("aucun motif demandé"), out var demande);   // exactement 100 KMF de trop

        Assert.Null(demande);
        var s = Lire(sid);
        Assert.Equal("Clôturée", s.Statut);
        Assert.Equal(5000m, s.MontantAttenduCloture);
        Assert.Null(s.MotifEcart);
    }

    [Fact]
    public void Au_dela_de_100_KMF_le_motif_est_obligatoire_et_enregistre_avec_son_commentaire()
    {
        var (ali, _) = Preparer();
        int sid = SessionOuverte(ali);
        Cloturer(ali, 3500m, e => ("Vente oubliée", "  cahier retrouvé le soir  "), out var demande);                // 1 500 KMF de moins

        Assert.Equal(-1500m, demande);
        var s = Lire(sid);
        Assert.Equal("Clôturée", s.Statut);
        Assert.Equal("Vente oubliée", s.MotifEcart);
        Assert.Equal("cahier retrouvé le soir", s.CommentaireEcart);
        Assert.Equal(3500m, s.MontantCompteCloture);
    }

    [Fact]
    public void Sans_motif_la_cloture_n_est_pas_enregistree()
    {
        var (ali, _) = Preparer();
        int sid = SessionOuverte(ali);
        Cloturer(ali, 8000m, e => null, out var demande);                                                              // excédent de 3 000 ; l'utilisateur annule

        Assert.Equal(3000m, demande);
        var s = Lire(sid);
        Assert.Equal("Ouverte", s.Statut);                                                                             // la caisse reste ouverte
        Assert.Null(s.MontantCompteCloture);
        Assert.Null(s.MotifEcart);
    }

    [Fact]
    public void La_fenetre_du_motif_n_accepte_qu_un_des_quatre_motifs_et_le_commentaire_est_facultatif()
    {
        Assert.Equal(new[] { "Vente oubliée", "Erreur de rendu de monnaie", "Paiement non saisi", "Inconnu" }, CaisseService.MotifsEcart);
        UiHelper.EnSta(() =>
        {
            using var f = new FormEcartCloture(-1500m);
            var motif = UiHelper.Champ<ComboBox>(f, "_motif");
            var ok = UiHelper.Champ<Button>(f, "_ok");
            Assert.Equal(4, motif.Items.Count);
            Assert.False(ok.Enabled);                                  // pas de motif : on ne peut pas valider
            Assert.Equal("", f.Motif);
            motif.SelectedItem = "Inconnu";
            Assert.True(ok.Enabled);
            Assert.Equal("Inconnu", f.Motif);
            Assert.Equal("", f.Commentaire);                           // commentaire facultatif
        });
    }

    // ───────── statistiques et alerte ─────────

    private static void SessionCloturee(int userId, decimal attendu, decimal compte, DateTime cloture)
    {
        using var ctx = new AppDbContext();
        ctx.SessionsCaisse.Add(new SessionCaisse
        {
            UserId = userId, DateOuverture = cloture.AddHours(-8), DateCloture = cloture, Statut = "Clôturée", CommentaireCloture = "",
            MontantAttenduCloture = attendu, MontantCompteCloture = compte
        });
        ctx.SaveChanges();
    }

    [Fact]
    public void Les_statistiques_donnent_excedents_manquants_net_et_le_detail_par_caissier()
    {
        var (ali, fatima) = Preparer();
        var jour = DateTime.Today.AddDays(-1).AddHours(18);
        SessionCloturee(ali, 5000, 5300, jour);               // +300
        SessionCloturee(ali, 5000, 4500, jour.AddHours(-30)); // −500
        SessionCloturee(fatima, 2000, 1800, jour);            // −200
        SessionCloturee(fatima, 2000, 2000, jour.AddHours(-1));   // pile : sans écart
        SessionCloturee(ali, 1000, 900, DateTime.Today.AddDays(-60));   // hors période

        var s = StatistiquesService.Calculer(DateTime.Today.AddDays(-7), DateTime.Now);
        var e = s.Ecarts!;
        Assert.Equal(300m, e.Excedents);
        Assert.Equal(700m, e.Manquants);
        Assert.Equal(-400m, e.Net);
        Assert.Equal(3, e.SessionsAvecEcart);
        var parCaissier = e.ParCaissier.ToDictionary(c => c.Caissier);
        Assert.Equal((300m, 500m, -200m, 2), (parCaissier["Ali Hamidou"].Excedents, parCaissier["Ali Hamidou"].Manquants, parCaissier["Ali Hamidou"].Net, parCaissier["Ali Hamidou"].Sessions));
        Assert.Equal((0m, 200m, -200m, 1), (parCaissier["Fatima Said"].Excedents, parCaissier["Fatima Said"].Manquants, parCaissier["Fatima Said"].Net, parCaissier["Fatima Said"].Sessions));
    }

    [Fact]
    public void Ma_journee_alerte_quand_un_meme_caissier_a_trois_ecarts_ou_plus_sur_sept_jours()
    {
        var (ali, fatima) = Preparer();
        var j = DateTime.Now.AddDays(-1);
        // Ali : 3 écarts notables en 7 jours (+500, −200, +101) → alerte
        SessionCloturee(ali, 5000, 5500, j);
        SessionCloturee(ali, 5000, 4800, j.AddDays(-1));
        SessionCloturee(ali, 5000, 5101, j.AddDays(-2));
        // Fatima : 2 écarts notables + un écart de 100 exactement (non notable) + un ancien (> 7 jours) → pas d'alerte
        SessionCloturee(fatima, 5000, 5300, j);
        SessionCloturee(fatima, 5000, 4700, j.AddDays(-1));
        SessionCloturee(fatima, 5000, 5100, j.AddDays(-2));
        SessionCloturee(fatima, 5000, 6000, j.AddDays(-20));

        var alerte = TableauDeBordService.AFaireMaintenant(DateTime.Now).Single(a => a.Type == TypeAFaire.EcartsCaisseRepetes);
        Assert.Equal("attention", alerte.Niveau);                       // orange
        Assert.Equal(1, alerte.Nombre);
        Assert.Contains("Ali Hamidou", alerte.Texte);
        Assert.DoesNotContain("Fatima", alerte.Texte);
        Assert.Contains("3 écarts de caisse", alerte.Texte.Replace(' ', ' '));

        SessionCloturee(fatima, 5000, 4000, j.AddDays(-3));              // un troisième écart notable pour Fatima
        var deux = TableauDeBordService.AFaireMaintenant(DateTime.Now).Single(a => a.Type == TypeAFaire.EcartsCaisseRepetes);
        Assert.Equal(2, deux.Nombre);
    }

    [Fact]
    public void Migration_AjoutEcartCloture_ne_perd_aucune_session_et_laisse_les_colonnes_vides()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutSessionPaiement"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "SessionsCaisse", new() { ["Id"] = 1L, ["UserId"] = 1L, ["Statut"] = "Clôturée", ["MontantCompteCloture"] = "5000" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutEcartCloture"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM SessionsCaisse WHERE MotifEcart IS NULL AND CommentaireEcart IS NULL"));
    }
}
