using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>Règles 8 et 9 du client : il fixe lui-même le prix de la boîte ET le prix à l'unité ; les deux sont supérieurs au prix d'achat.</summary>
public class Regle08PrixUniteTests
{
    [Fact]
    public void Regle08_migration_reprend_le_prix_a_l_unite_sur_base_remplie_et_le_journalise()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("RepriseVentesPayeesEnTotalite"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "Doliprane", ["PrixVente"] = "1000", ["NbUniteParBoite"] = 3L, ["UniteVente"] = "Plaquette" });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "Sirop", ["PrixVente"] = "900", ["NbUniteParBoite"] = 1L, ["UniteVente"] = "Boîte" });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 3L, ["Nom"] = "Amoxicilline", ["PrixVente"] = "1500", ["NbUniteParBoite"] = 4L, ["UniteVente"] = "Gélule" });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        using (var ctx = new AppDbContext()) Assert.True(PrixUniteService.EstEnAttente(ctx));

        TestDb.MigrerTout();

        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(333, TestDb.Scalaire(c2, "SELECT CAST(PrixVenteUnite AS INTEGER) FROM produits WHERE Id=1"));   // 1 000 / 3
        Assert.Equal(375, TestDb.Scalaire(c2, "SELECT CAST(PrixVenteUnite AS INTEGER) FROM produits WHERE Id=3"));
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM produits WHERE Id=2 AND PrixVenteUnite IS NULL"));   // vendu à la boîte seulement
        c2.Close();

        using var ctx2 = new AppDbContext();
        var liste = PrixUniteService.ProduitsRepris(ctx2);
        Assert.Equal(2, liste.Count);
        Assert.Contains(liste, l => l.StartsWith("Doliprane") && l.Contains("333"));
        PrixUniteService.Journaliser(ctx2);
        string journal = File.ReadAllText(Directory.GetFiles(CheminsApp.DossierLogs, "journal_*.log").OrderBy(f => f).Last());
        Assert.Contains("Prix à l'unité repris pour 2 produit(s)", journal);
        Assert.Contains("Amoxicilline", journal);
    }

    [Fact]
    public void Regle08_le_prix_a_l_unite_n_est_jamais_calcule_a_partir_de_celui_de_la_boite()
    {
        var p = new Produit { Nom = "X", NbUniteParBoite = 3, PrixVente = 1000m, PrixVenteUnite = 400m };
        Assert.Equal(400m, p.PrixUnitaireVente);        // exactement le prix saisi, pas 333
        p.PrixVente = 2000m;
        Assert.Equal(400m, p.PrixUnitaireVente);        // changer le prix de la boîte ne change pas l'unité
        Assert.Equal(0m, new Produit { NbUniteParBoite = 3, PrixVente = 1000m }.PrixUnitaireVente);   // aucune déduction : 0 = « non renseigné »
        Assert.Equal(900m, new Produit { NbUniteParBoite = 1, PrixVente = 900m }.PrixUnitaireVente);  // boîte seule : le prix de la boîte
    }

    private static void SaisirNouveau(Action<FormAddProduit> reglages)
    {
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit();
                UiHelper.Champ<TextBox>(f, "txtNom").Text = "Test plaquettes";
                UiHelper.Champ<NumericUpDown>(f, "numPrixAchat").Value = 600;
                UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 1000;     // après l'achat : le prix de la boîte est fixé en dernier
                UiHelper.Champ<CheckBox>(f, "chkVenteDetail").Checked = true;
                UiHelper.Champ<NumericUpDown>(f, "numNbUniteParBoite").Value = 3;
                UiHelper.Champ<NumericUpDown>(f, "numQuantite").Value = 5;
                reglages(f);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
    }

    private static void Preparer() { TestDb.Reinitialiser(); TestDb.MigrerTout(); }

    [Fact]
    public void Regle08_un_nouveau_produit_au_detail_sans_prix_a_l_unite_est_refuse()
    {
        Preparer();
        SaisirNouveau(f => Assert.Equal(0m, UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value));   // aucune valeur proposée
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
        Assert.True(UiHelper.TitreVu("Prix à l'unité obligatoire"));
    }

    [Fact]
    public void Regle09_un_prix_a_l_unite_inferieur_ou_egal_au_cout_d_achat_unitaire_est_refuse()
    {
        Preparer();
        SaisirNouveau(f => UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 200);   // coût d'achat unitaire : 600 / 3 = 200
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
        Assert.True(UiHelper.TitreVu("Marge invalide à l'unité"));

        SaisirNouveau(f => UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 201);
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
        Assert.Equal(201, TestDb.Scalaire("SELECT CAST(PrixVenteUnite AS INTEGER) FROM produits"));   // exactement le prix saisi
        Assert.Equal(1000, TestDb.Scalaire("SELECT CAST(PrixVente AS INTEGER) FROM produits"));
    }

    [Fact]
    public void Regle09_le_prix_de_la_boite_doit_aussi_depasser_le_prix_d_achat()
    {
        Preparer();
        SaisirNouveau(f =>
        {
            UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 500;   // inférieur à l'achat (600)
            UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 300;
        });
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM produits"));
    }

    [Fact]
    public void Regle08_une_vente_a_la_plaquette_utilise_exactement_le_prix_saisi()
    {
        Preparer();
        SaisirNouveau(f => UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 400);   // et non 333
        using var ctx = new AppDbContext();
        var p = ctx.produits.Single();
        Assert.Equal(400m, p.PrixUnitaireVente);
        Assert.Equal(1000m, p.PrixVente);
        Assert.Equal(15, p.QuantiteEnStock);     // stock toujours en unités de base : 5 boîtes de 3
    }

    [Fact]
    public void Regle08_changer_le_nombre_d_unites_ne_change_pas_le_prix_et_demande_de_le_verifier()
    {
        Preparer();
        SaisirNouveau(f => UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value = 400);
        int pid = (int)TestDb.Scalaire("SELECT Id FROM produits");

        // Passer à 4 unités : il faut ressaisir le stock réel (règle existante), puis confirmer le prix à l'unité
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(pid);
                UiHelper.Champ<NumericUpDown>(f, "numNbUniteParBoite").Value = 4;
                Assert.Equal(400m, UiHelper.Champ<NumericUpDown>(f, "numPrixVenteUnite").Value);       // pas modifié automatiquement
                Assert.Contains("Vérifiez le prix à l'unité", UiHelper.Champ<Label>(f, "lblApercu").Text);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);   // avertissement « stock à ressaisir »
                UiHelper.Champ<NumericUpDown>(f, "numQuantite").Value = 2;
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);   // confirmation du stock puis du prix
            });
        }
        finally { cts.Cancel(); }

        Assert.True(UiHelper.TitreVu("Vérifiez le prix à l'unité"));
        Assert.Equal(4, TestDb.Scalaire($"SELECT NbUniteParBoite FROM produits WHERE Id={pid}"));
        Assert.Equal(400, TestDb.Scalaire($"SELECT CAST(PrixVenteUnite AS INTEGER) FROM produits WHERE Id={pid}"));
    }
}
