using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>D24 : péremptions jamais saisies (l'ancienne fiche mettait la date du jour). Elles ne sont ni « périmées » ni comptées dans les alertes.</summary>
public class PeremptionConnueTests
{
    private static readonly DateTime Import = new(2026, 10, 24, 10, 0, 0);
    private static readonly DateTime PremiereVente = new(2026, 8, 21, 9, 0, 0);

    internal sealed class Ids { public int Creation, VenduApres, Future, VraiPerime, VraiPerimeJamaisVendu; }

    private static Produit P(string nom, DateTime exp, int stock = 10)
        => new()
        {
            Nom = nom, Type = "Autre", PrixAchat = 100, PrixVente = 200, NbUniteParBoite = 1, UniteVente = "Boîte", QuantiteEnStock = stock,
            SeuilAlerte = 1, DateExpiration = exp, Indication = "", Posologie = ""
        };

    private static void Vendre(AppDbContext ctx, int userId, Produit p, DateTime date, string numero)
    {
        var v = new Vente
        {
            numeroVente = numero, Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = 200, UserId = userId, DateVente = date
        };
        v.Lignes.Add(new LigneVente { ProduitId = p.Id, Quantite = 1, QuantiteUnites = 1, PrixUnitaire = 200, UniteVendue = "Boîte" });
        v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + numero, Montant = 200, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = date });
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

        var creation = P("Fausse date : création du produit", new DateTime(2026, 8, 20, 11, 0, 0));          // = date de création : jamais saisie
        var venduApres = P("Fausse date : vendu après", new DateTime(2026, 7, 2, 15, 30, 12));               // vendu après cette date
        var future = P("Vraie date future", new DateTime(2028, 5, 31));
        var vraiPerime = P("Vrai périmé", new DateTime(2025, 2, 28), stock: 4);                              // vendu seulement AVANT sa date
        var vraiPerimeJamais = P("Vrai périmé jamais vendu", new DateTime(2025, 1, 31), stock: 6);
        ctx.produits.AddRange(creation, venduApres, future, vraiPerime, vraiPerimeJamais);
        ctx.SaveChanges();

        Vendre(ctx, admin.Id, creation, PremiereVente, "V-000001");
        Vendre(ctx, admin.Id, venduApres, PremiereVente.AddDays(10), "V-000002");
        Vendre(ctx, admin.Id, vraiPerime, new DateTime(2025, 2, 10), "V-000003");   // avant sa date : cette vente précède même la première vente « de la base »… on la date juste avant
        ctx.SaveChanges();
        // la première vente de la base est bien celle du 21 août 2026 : on déplace la vente ancienne hors de la base d'analyse
        ctx.ventes.Single(v => v.numeroVente == "V-000003").Statut = "Annulée";
        ctx.SaveChanges();

        ids.Creation = creation.Id; ids.VenduApres = venduApres.Id; ids.Future = future.Id; ids.VraiPerime = vraiPerime.Id; ids.VraiPerimeJamaisVendu = vraiPerimeJamais.Id;
        SessionUtilisateur.Courant = admin;
        return ids;
    }

    private static Produit Lire(int id) { using var ctx = new AppDbContext(); return ctx.produits.AsNoTracking().Single(p => p.Id == id); }

    private static RapportDiagnostic Lancer(bool appliquer)
    {
        using var ctx = new AppDbContext();
        return DiagnosticBaseService.Executer(ctx, appliquer, Import);
    }

    [Fact]
    public void A_l_import_les_dates_jamais_saisies_sont_marquees_et_les_vraies_dates_restent_intactes()
    {
        var ids = Preparer();
        var r = Lancer(appliquer: true);

        foreach (var id in new[] { ids.Creation, ids.VenduApres })
        {
            var p = Lire(id);
            Assert.False(p.PeremptionConnue);
            Assert.True(p.FicheAVerifier);
            Assert.Equal("Péremption : jamais saisie", p.MotifsFiche);
        }
        Assert.True(Lire(ids.Future).PeremptionConnue);                          // vraie date future : intact
        Assert.False(Lire(ids.Future).FicheAVerifier);
        Assert.True(Lire(ids.VraiPerime).PeremptionConnue);                      // vrai périmé (jamais vendu après sa date) : reste « Périmé »
        Assert.True(Lire(ids.VraiPerimeJamaisVendu).PeremptionConnue);
        Assert.Equal(2, r.Constats.Count(c => c.Code == "D24" && c.Groupe == GroupeConstat.Corrige));
        Assert.DoesNotContain(r.Constats, c => c.Code == "D8" && (c.Objet.StartsWith("Fausse date")));   // plus aucun « périmé » faux dans D8
        Assert.Contains(r.Constats, c => c.Code == "D8" && c.Objet == "Vrai périmé");

        // idempotent
        Lancer(appliquer: true);
        Assert.Equal(2, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE PeremptionConnue = 0"));
    }

    [Fact]
    public void En_lecture_seule_rien_n_est_modifie_et_le_cas_est_signale_a_verifier()
    {
        var ids = Preparer();
        var r = Lancer(appliquer: false);
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE PeremptionConnue = 0 OR FicheAVerifier = 1"));
        Assert.Equal(2, r.Constats.Count(c => c.Code == "D24" && c.Groupe == GroupeConstat.AVerifier));
        Assert.Empty(r.De(GroupeConstat.Corrige));
    }

    [Fact]
    public void Non_renseignee_en_gris_jamais_perime_et_exclue_des_alertes()
    {
        var ids = Preparer();
        Lancer(appliquer: true);

        // Ma journée : seuls les vrais périmés sont comptés ; une ligne orange pour les péremptions manquantes
        var aFaire = TableauDeBordService.AFaireMaintenant(Import);
        var perimes = aFaire.Single(a => a.Type == TypeAFaire.Perimes);
        Assert.Equal(2, perimes.Nombre);                                          // « Vrai périmé » et « Vrai périmé jamais vendu »
        Assert.DoesNotContain("Fausse date", perimes.Texte);
        var manquantes = aFaire.Single(a => a.Type == TypeAFaire.PeremptionManquante);
        Assert.Equal(2, manquantes.Nombre);
        Assert.Equal("attention", manquantes.Niveau);
        Assert.Contains("2 produits sans date de péremption", manquantes.Texte.Replace(' ', ' '));
        Assert.DoesNotContain(aFaire, a => a.Type == TypeAFaire.PeremptionProche && a.Texte.Contains("Fausse date"));

        UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var stock = new Uc_Stock { Dock = DockStyle.Fill };
            hote.Controls.Add(stock);
            hote.Show();
            Application.DoEvents();
            var g = UiHelper.Champ<DataGridView>(stock, "dgvStock");

            stock.Filtrer("Sans date de péremption");
            Assert.Equal(2, g.Rows.Count);
            foreach (DataGridViewRow ligne in g.Rows)
            {
                Assert.Equal("Non renseignée", Convert.ToString(ligne.Cells["Expiration"].Value));
                Assert.NotEqual("Périmé", Convert.ToString(ligne.Cells["Etat"].Value));            // jamais « Périmé »
            }

            stock.Filtrer("Périmés");
            Assert.Equal(new[] { "Vrai périmé", "Vrai périmé jamais vendu" }, g.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["Produit"].Value)).OrderBy(x => x).ToArray());
            stock.Dispose();

            var catalogue = new Uc_Produits { Dock = DockStyle.Fill };
            hote.Controls.Clear();
            hote.Controls.Add(catalogue);
            Application.DoEvents();
            var gp = UiHelper.Champ<DataGridView>(catalogue, "dgvProduits");
            var fausse = gp.Rows.Cast<DataGridViewRow>().First(r => Convert.ToString(r.Cells["Nom"].Value) == "Fausse date : création du produit");
            Assert.Equal("Non renseignée", Convert.ToString(fausse.Cells["Expiration"].Value));
            Assert.Equal("OK", Convert.ToString(fausse.Cells["Etat"].Value));                      // ni « Périmé » ni « Alerte » à cause de la date
            catalogue.Dispose();
        });
    }

    [Fact]
    public void Saisir_une_date_repasse_la_peremption_a_connue_et_leve_le_motif()
    {
        var ids = Preparer();
        Lancer(appliquer: true);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.Creation);
                Assert.False(UiHelper.Champ<DateTimePicker>(f, "dtpDateExpiration").Checked);       // la fausse date n'est pas reproposée
                Assert.True(UiHelper.Champ<CheckBox>(f, "chkPasPeremption").Checked);
                var dtp = UiHelper.Champ<DateTimePicker>(f, "dtpDateExpiration");
                dtp.Value = new DateTime(2029, 3, 31);
                dtp.Checked = true;
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        var p = Lire(ids.Creation);
        Assert.True(p.PeremptionConnue);
        Assert.Equal(new DateTime(2029, 3, 31), p.DateExpiration);
        Assert.False(p.FicheAVerifier);
        Assert.Null(p.MotifsFiche);
    }

    private static string Nouveau(Action<FormAddProduit> reglages)
    {
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit();
                var dtp = UiHelper.Champ<DateTimePicker>(f, "dtpDateExpiration");
                Assert.False(dtp.Checked);                                                              // jamais la date du jour par défaut
                UiHelper.Champ<TextBox>(f, "txtNom").Text = "Produit neuf";
                UiHelper.Champ<NumericUpDown>(f, "numPrixAchat").Value = 100;
                UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 200;
                UiHelper.Champ<NumericUpDown>(f, "numQuantite").Value = 5;
                reglages(f);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        return "";
    }

    [Fact]
    public void Un_nouveau_produit_sans_date_est_refuse_sauf_avec_Pas_de_date_de_peremption()
    {
        Preparer();
        Nouveau(f => { });
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE Nom = 'Produit neuf'"));
        Assert.True(UiHelper.TitreVu("Date de péremption"));

        Nouveau(f => UiHelper.Champ<CheckBox>(f, "chkPasPeremption").Checked = true);
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE Nom = 'Produit neuf' AND PeremptionConnue = 0 AND FicheAVerifier = 0"));   // sans marquage

        using (var ctx = new AppDbContext()) { ctx.produits.Remove(ctx.produits.Single(p => p.Nom == "Produit neuf")); ctx.SaveChanges(); }
        Nouveau(f =>
        {
            var dtp = UiHelper.Champ<DateTimePicker>(f, "dtpDateExpiration");
            dtp.Value = new DateTime(2029, 1, 31);
            dtp.Checked = true;
        });
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM produits WHERE Nom = 'Produit neuf' AND PeremptionConnue = 1"));
        Assert.Equal("2029-01-31", TestDb.Texte("SELECT substr(DateExpiration, 1, 10) FROM produits WHERE Nom = 'Produit neuf'"));
    }

    [Fact]
    public void Migration_AjoutPeremptionConnue_garde_les_produits_et_met_la_colonne_a_vrai()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutFicheAVerifier"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "A", ["PrixVente"] = "200", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "B", ["PrixVente"] = "300", ["NbUniteParBoite"] = 1L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutPeremptionConnue"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM produits WHERE PeremptionConnue = 1"));
    }
}
