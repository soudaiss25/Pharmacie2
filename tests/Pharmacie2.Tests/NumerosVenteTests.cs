using System.Reflection;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>
/// Numéros de vente : l'ancien compteur statique avait produit 7 doublons quand deux fenêtres de vente étaient ouvertes en même temps.
/// Le numéro est maintenant calculé dans la transaction d'enregistrement.
/// </summary>
public class NumerosVenteTests
{
    private static int Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 1000m);
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        ctx.Users.Add(admin);
        ctx.SaveChanges();
        ctx.SessionsCaisse.Add(new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" });
        ctx.SaveChanges();
        SessionUtilisateur.Courant = admin;
        return pid;
    }

    private static FormVente Ouvrir(int produitId, int boites)
    {
        var f = new FormVente();
        var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(f)!;
        lignes.Add(new LigneVente { ProduitId = produitId, Quantite = boites, PrixUnitaire = 1000m, UniteVendue = "Boîte", Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Boîte" } });
        UiHelper.Appeler(f, "RafraichirGrille");
        UiHelper.Appeler(f, "RecalculerTotal");
        UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = ModesPaiement.Comptant;
        UiHelper.Champ<NumericUpDown>(f, "numEspeces").Value = boites * 1000;
        return f;
    }

    [Fact]
    public void Deux_fenetres_de_vente_ouvertes_en_meme_temps_enregistrent_deux_numeros_differents()
    {
        int pid = Preparer();
        var cts = UiHelper.FermerLesMessages(false);   // « Non » à l'impression de la facture
        try
        {
            UiHelper.EnSta(() =>
            {
                using var premiere = Ouvrir(pid, 1);
                using var seconde = Ouvrir(pid, 2);        // ouverte AVANT que la première n'enregistre : même état de la base au départ
                UiHelper.Appeler(premiere, "btnValider_Click", null, EventArgs.Empty);
                UiHelper.Appeler(seconde, "btnValider_Click", null, EventArgs.Empty);
                using var troisieme = Ouvrir(pid, 1);
                UiHelper.Appeler(troisieme, "btnValider_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using var ctx = new AppDbContext();
        var numeros = ctx.ventes.OrderBy(v => v.IdVente).Select(v => v.numeroVente).ToList();
        Assert.Equal(new[] { "V-000001", "V-000002", "V-000003" }, numeros);
        Assert.Equal(numeros.Count, numeros.Distinct().Count());
        Assert.Equal(96, ctx.produits.Single().QuantiteEnStock);          // 1 + 2 + 1 boîtes vendues
    }

    [Fact]
    public void Six_fenetres_ouvertes_avant_tout_enregistrement_ont_six_numeros_differents()
    {
        int pid = Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                var fenetres = Enumerable.Range(0, 6).Select(_ => Ouvrir(pid, 1)).ToList();
                foreach (var f in fenetres) UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
                foreach (var f in fenetres) f.Dispose();
            });
        }
        finally { cts.Cancel(); }

        using var ctx = new AppDbContext();
        var numeros = ctx.ventes.Select(v => v.numeroVente).ToList();
        Assert.Equal(6, numeros.Count);
        Assert.Equal(6, numeros.Distinct().Count());
    }
}
