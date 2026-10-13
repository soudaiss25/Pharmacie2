using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>R2c : le montant attendu est figé à la clôture ; les anciennes sessions sont recalculées avec la mention « (recalculé) ».</summary>
public class ClotureTests
{
    private static (int admin, int session) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        using var ctx = new AppDbContext();
        ctx.Users.Add(admin);
        ctx.SaveChanges();
        var s = new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" };
        ctx.SessionsCaisse.Add(s);
        var v = new Vente
        {
            numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MontantTotal = 3000, UserId = admin.Id, DateVente = DateTime.Now.AddSeconds(-5)
        };
        v.Paiements.Add(new Paiement { NumeroPaiement = "P-1", Montant = 3000, Mode = ModesPaiement.Especes, UserId = admin.Id, DatePaiement = DateTime.Now.AddSeconds(-5) });
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        SessionUtilisateur.Courant = admin;
        return (admin.Id, s.Id);
    }

    [Fact]
    public void La_cloture_fige_le_montant_attendu_et_il_ne_change_plus()
    {
        var (admin, sid) = Preparer();
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormSessionCaisse(admin);
                UiHelper.Champ<NumericUpDown>(f, "numMontantReel").Value = 2900;
                UiHelper.Appeler(f, "btnCloture_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using (var ctx = new AppDbContext())
        {
            var s = ctx.SessionsCaisse.Single();
            Assert.Equal("Clôturée", s.Statut);
            Assert.Equal(3000m, s.MontantAttenduCloture);
            Assert.Equal(2900m, s.MontantCompteCloture);

            // Un paiement ajouté après coup, daté dans la session, ne change PAS le montant affiché d'une session clôturée
            var v = ctx.ventes.Single();
            ctx.paiement.Add(new Paiement { NumeroPaiement = "P-TARDIF", VenteId = v.IdVente, Montant = 777, Mode = ModesPaiement.Especes, UserId = admin, DatePaiement = DateTime.Now.AddSeconds(-30) });
            ctx.SaveChanges();

            var (montant, recalcule) = CaisseService.Attendu(ctx, s);
            Assert.Equal(3000m, montant);
            Assert.False(recalcule);
            Assert.Equal(3777m, CaisseService.EspecesAttendues(ctx, s));   // un recalcul donnerait autre chose : on ne l'utilise pas
        }
    }

    [Fact]
    public void Ancienne_session_cloturee_sans_valeur_est_recalculee_sans_ecriture_en_base_et_affichee_avec_la_mention()
    {
        var (admin, sid) = Preparer();
        using (var ctx = new AppDbContext())
        {
            var s = ctx.SessionsCaisse.Single();
            s.Statut = "Clôturée";
            s.DateCloture = DateTime.Now;
            s.MontantCompteCloture = 3000;
            s.MontantAttenduCloture = null;     // ancienne session
            ctx.SaveChanges();
        }

        using (var ctx = new AppDbContext())
        {
            var (montant, recalcule) = CaisseService.Attendu(ctx, ctx.SessionsCaisse.Single());
            Assert.Equal(3000m, montant);
            Assert.True(recalcule);
        }
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM SessionsCaisse WHERE MontantAttenduCloture IS NOT NULL"));   // rien n'a été écrit

        UiHelper.EnSta(() =>
        {
            var uc = new Uc_Caisse();
            using var hote = new Form();
            uc.Dock = DockStyle.Fill;
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();
            var grille = UiHelper.Champ<DataGridView>(uc, "dgvSessions");
            string texte = Convert.ToString(grille.Rows[0].Cells["Theorique"].Value);
            Assert.EndsWith("(recalculé)", texte);
            Assert.Contains("3", texte);
            UiHelper.Appeler(uc, "ChargerDetailSession", sid);
            Assert.Contains("(recalculé)", UiHelper.Champ<Label>(uc, "lblDetailSession").Text);
            uc.Dispose();
        });
    }

    [Fact]
    public void Migration_AjoutAttenduCloture_garde_les_sessions_et_laisse_la_colonne_vide()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutModePaiementPaiement"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "SessionsCaisse", new() { ["Id"] = 1L, ["UserId"] = 1L, ["Statut"] = "Clôturée", ["MontantCompteCloture"] = "5000" });
            TestDb.Inserer(c, "SessionsCaisse", new() { ["Id"] = 2L, ["UserId"] = 1L, ["Statut"] = "Ouverte" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutAttenduCloture"));   // seulement cette migration
        using var c2 = TestDb.Ouvrir();
        var apres = TestDb.Comptes(c2, TestDb.TablesMetier);
        Assert.All(avant, kv => Assert.Equal(kv.Value, apres[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM SessionsCaisse WHERE MontantAttenduCloture IS NULL"));
        Assert.Equal("5000", TestDb.Texte("SELECT MontantCompteCloture FROM SessionsCaisse WHERE Id=1"));
    }
}
