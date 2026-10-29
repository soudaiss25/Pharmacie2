using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>Mutuelles : ventes sans patient identifié (D26), part patient sur-payée ou non réglée (D11/D12 étendus), badge dans l'écran.</summary>
public class MutuellePatientTests
{
    private static readonly DateTime Import = new(2026, 10, 24, 10, 0, 0);

    private static (int alpha, int beta) Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        ctx.Users.Add(admin);
        var a = new Mutuel { NomEmployeur = "Alpha SA", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
        var b = new Mutuel { NomEmployeur = "Beta SARL", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
        ctx.mutuels.AddRange(a, b);
        ctx.SaveChanges();

        // Alpha : 2 ventes sans nom ni matricule (1 000 + 600 en attente), 1 réglée sans identité (ne compte pas dans l'attente), 1 identifiée
        Vente(ctx, admin.Id, a.IdMutuel, "V-000001", "", "", "", total: 2000, mutuelle: 1000, verse: 1000);
        Vente(ctx, admin.Id, a.IdMutuel, "V-000002", "", "", "N/A", total: 1200, mutuelle: 600, verse: 600);
        Vente(ctx, admin.Id, a.IdMutuel, "V-000003", "", "", "", total: 4000, mutuelle: 2000, verse: 2000, reglee: true);
        Vente(ctx, admin.Id, a.IdMutuel, "V-000004", "Hamidou", "Ali", "M-1", total: 1000, mutuelle: 500, verse: 500);
        // Beta : une vente identifiée par son seul matricule (identifiée) ; une part patient sur-payée ; une part patient non réglée
        Vente(ctx, admin.Id, b.IdMutuel, "V-000005", "", "", "M-7", total: 1000, mutuelle: 500, verse: 500);
        Vente(ctx, admin.Id, b.IdMutuel, "V-000006", "Said", "", "M-8", total: 1000, mutuelle: 500, verse: 800);   // sur-payée de 300
        Vente(ctx, admin.Id, b.IdMutuel, "V-000007", "Said", "", "M-9", total: 1000, mutuelle: 500, verse: 200);   // il manque 300
        ctx.SaveChanges();
        return (a.IdMutuel, b.IdMutuel);
    }

    private static void Vente(AppDbContext ctx, int userId, int mutuelId, string numero, string nom, string prenom, string matricule, decimal total, decimal mutuelle, decimal verse, bool reglee = false)
    {
        var v = new Vente
        {
            numeroVente = numero, Statut = "Active", NomClient = nom, PrenomClient = prenom, TelephoneClient = "", MotifAchat = "", MatriculeEmploye = matricule,
            MoyenPaiement = ModesPaiement.Mutuelle, Type = ModesPaiement.Mutuelle, MotifAnnulation = "", MontantTotal = total, MutuelId = mutuelId,
            TauxMutuelle = 50, MontantMutuelle = mutuelle, MutuelleReglee = reglee, UserId = userId, DateVente = new DateTime(2026, 9, 1, 10, 0, 0)
        };
        if (verse != 0) v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + numero, Montant = verse, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = v.DateVente });
        ctx.ventes.Add(v);
    }

    private static RapportDiagnostic Lancer(bool appliquer)
    {
        using var ctx = new AppDbContext();
        return DiagnosticBaseService.Executer(ctx, appliquer, Import);
    }

    [Fact]
    public void D26_compte_par_mutuelle_les_ventes_sans_patient_identifie_avec_la_part_en_attente()
    {
        Preparer();
        var r = Lancer(appliquer: true);
        var d26 = r.Constats.Where(c => c.Code == "D26").ToList();
        Assert.All(d26, c => Assert.Equal(GroupeConstat.Information, c.Groupe));
        var alpha = Assert.Single(d26);                                              // Beta n'a aucune vente sans identité
        Assert.Equal("Alpha SA", alpha.Objet);
        Assert.Contains("3 vente(s)", alpha.Detail);
        Assert.Contains(Format.Montant(1600m), alpha.Detail);                         // 1 000 + 600 ; la vente déjà réglée n'est pas en attente
    }

    [Fact]
    public void D11_et_D12_listent_la_part_patient_sur_payee_ou_non_reglee_sans_rien_corriger()
    {
        Preparer();
        long paiementsAvant = TestDb.Scalaire("SELECT COUNT(*) FROM paiement");
        var r = Lancer(appliquer: true);

        var sur = r.Constats.Single(c => c.Code == "D11" && c.Objet == "V-000006");
        var non = r.Constats.Single(c => c.Code == "D12" && c.Objet == "V-000007");
        Assert.Equal(GroupeConstat.AVerifier, sur.Groupe);
        Assert.Equal(GroupeConstat.AVerifier, non.Groupe);
        Assert.Contains("sur-payée", sur.Detail);
        Assert.Contains("non réglée", non.Detail);
        Assert.DoesNotContain(r.Constats, c => (c.Code == "D11" || c.Code == "D12") && c.Objet is "V-000001" or "V-000002" or "V-000004" or "V-000005");
        Assert.Equal(paiementsAvant, TestDb.Scalaire("SELECT COUNT(*) FROM paiement"));   // aucune correction
        Assert.Empty(r.De(GroupeConstat.Corrige).Where(c => c.Code is "D11" or "D12"));
    }

    [Fact]
    public void L_ecran_des_mutuelles_affiche_le_badge_Patient_non_identifie()
    {
        var (alpha, _) = Preparer();
        UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var uc = new Uc_Mutuelle { Dock = DockStyle.Fill };
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();
            UiHelper.Appeler(uc, "ChargerVentesImpayees", alpha, "Alpha SA");

            var g = UiHelper.Champ<DataGridView>(uc, "dgvVentesImpayees");
            var clients = g.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["colClient"].Value)).ToList();
            Assert.Equal(2, clients.Count(c => c == "Patient non identifié"));         // V-000001 et V-000002 (V-000003 est réglée)
            Assert.Contains("Ali Hamidou", clients);
            Assert.Contains("Patient non identifié : 2 ventes", UiHelper.Champ<Label>(uc, "lblRecapImpaye").Text.Replace(' ', ' '));
            uc.Dispose();
        });
    }

    [Fact]
    public void Migration_AjoutMatriculeObligatoire_garde_les_mutuelles_decochees()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutEcartCloture"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "mutuels", new() { ["IdMutuel"] = 1L, ["NomEmployeur"] = "A", ["EmailContact"] = "", ["telephoneEmployeur"] = "", ["TauxPriseEnCharge"] = "50", ["Actif"] = 1L });
            TestDb.Inserer(c, "mutuels", new() { ["IdMutuel"] = 2L, ["NomEmployeur"] = "B", ["EmailContact"] = "", ["telephoneEmployeur"] = "", ["TauxPriseEnCharge"] = "30", ["Actif"] = 1L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutMatriculeObligatoire"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM mutuels WHERE MatriculeObligatoire = 0"));
    }
}
