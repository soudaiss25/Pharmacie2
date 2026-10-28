using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Une seule session de caisse ouverte par utilisateur.</summary>
public class SessionsMultiplesTests
{
    private static int Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ali = new User { Nom = "Hamidou", Prenom = "Ali", Role = Roles.Caissier, Login = "ali", MotDePasse = MotDePasseService.Hacher("x") };
        using var ctx = new AppDbContext();
        ctx.Users.Add(ali);
        ctx.SaveChanges();
        return ali.Id;
    }

    private static int Ouvertes(int userId)
    {
        using var ctx = new AppDbContext();
        return ctx.SessionsCaisse.Count(s => s.UserId == userId && s.Statut == "Ouverte");
    }

    private static void Cliquer(int userId, string bouton, out bool boutonOuvrirActif)
    {
        bool actif = false;
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormSessionCaisse(userId);
                actif = UiHelper.Champ<Button>(f, "btnOuvrir").Enabled;
                UiHelper.Appeler(f, bouton, null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        boutonOuvrirActif = actif;
    }

    [Fact]
    public void Ouvrir_la_caisse_cree_une_seule_session()
    {
        var ali = Preparer();
        Cliquer(ali, "btnOuvrir_Click", out var actif);
        Assert.True(actif);
        Assert.Equal(1, Ouvertes(ali));
    }

    [Fact]
    public void Impossible_d_ouvrir_une_deuxieme_session_quand_une_est_deja_ouverte()
    {
        var ali = Preparer();
        using (var ctx = new AppDbContext()) Assert.NotNull(CaisseService.Ouvrir(ctx, ali));
        using (var ctx = new AppDbContext()) Assert.Null(CaisseService.Ouvrir(ctx, ali));
        Assert.Equal(1, Ouvertes(ali));

        Cliquer(ali, "btnOuvrir_Click", out var actif);          // l'écran refuse aussi
        Assert.False(actif);
        Assert.Equal(1, Ouvertes(ali));
    }

    [Fact]
    public void Une_session_restee_ouverte_la_veille_doit_etre_cloturee_d_abord()
    {
        var ali = Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.SessionsCaisse.Add(new SessionCaisse { UserId = ali, DateOuverture = DateTime.Today.AddDays(-1).AddHours(8), Statut = "Ouverte", CommentaireCloture = "" });
            ctx.SaveChanges();
        }
        using (var ctx = new AppDbContext()) Assert.Null(CaisseService.Ouvrir(ctx, ali));

        Cliquer(ali, "btnOuvrir_Click", out var actif);
        Assert.False(actif);                                      // « Ouvrir » grisé, la clôture est proposée
        Assert.Equal(1, Ouvertes(ali));

        string etat = "";
        UiHelper.EnSta(() =>
        {
            using var f = new FormSessionCaisse(ali);
            etat = UiHelper.Champ<Label>(f, "lblInfoSession").Text;
            Assert.True(UiHelper.Champ<Button>(f, "btnCloture").Enabled);
        });
        Assert.Contains("Clôturez-la d'abord", etat);

        var cts = UiHelper.FermerLesMessages(true);               // clôture puis ouverture du jour
        try { UiHelper.EnSta(() => { using var f = new FormSessionCaisse(ali); UiHelper.Appeler(f, "btnCloture_Click", null, EventArgs.Empty); }); }
        finally { cts.Cancel(); }
        Assert.Equal(0, Ouvertes(ali));
        using (var ctx = new AppDbContext()) Assert.NotNull(CaisseService.Ouvrir(ctx, ali));
    }

    [Fact]
    public void Avec_plusieurs_sessions_ouvertes_d_une_ancienne_base_la_plus_recente_est_utilisee()
    {
        var ali = Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.SessionsCaisse.Add(new SessionCaisse { UserId = ali, DateOuverture = DateTime.Today.AddHours(7), Statut = "Ouverte", CommentaireCloture = "" });
            ctx.SessionsCaisse.Add(new SessionCaisse { UserId = ali, DateOuverture = DateTime.Today.AddHours(9), Statut = "Ouverte", CommentaireCloture = "" });
            ctx.SaveChanges();
        }
        using var c = new AppDbContext();
        var s = CaisseService.SessionOuverteDe(c, ali)!;
        Assert.Equal(DateTime.Today.AddHours(9), s.DateOuverture);
    }
}
