using System.Collections;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class VenteMutuelleTests
{
    private static (int m0, int m1, int produit, int vente) Preparer(bool m1Archivee = false, bool reglee = false)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", 100m);
        using var ctx = new AppDbContext();
        var m0 = new Mutuel { NomEmployeur = "Aaa première", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 30 };
        var m1 = new Mutuel { NomEmployeur = "Zzz entreprise", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50, Actif = !m1Archivee };
        ctx.mutuels.AddRange(m0, m1);
        ctx.SaveChanges();

        var v = new Vente
        {
            numeroVente = "V-000001", Statut = "Active", NomClient = "N", PrenomClient = "P", TelephoneClient = "", MotifAchat = "",
            MatriculeEmploye = "M1", MoyenPaiement = "Mutuelle", Type = "Mutuelle", MotifAnnulation = "",
            MontantTotal = 1000, MutuelId = m1.IdMutuel, TauxMutuelle = 50, MontantMutuelle = 500, MutuelleReglee = reglee
        };
        v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 10, QuantiteUnites = 10, PrixUnitaire = 100, UniteVendue = "Boîte" });
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        return (m0.IdMutuel, m1.IdMutuel, pid, v.IdVente);
    }

    // ───────── règles (sans écran) ─────────

    [Fact]
    public void Modifier_la_quantite_garde_la_meme_mutuelle_et_recalcule_la_part()
    {
        var v = new Vente { MutuelId = 2, TauxMutuelle = 50, MontantMutuelle = 500, MutuelleReglee = false };
        var m1 = new Mutuel { IdMutuel = 2, TauxPriseEnCharge = 40 };   // le taux de la mutuelle a changé depuis la vente

        VenteModificationRegles.AppliquerMutuelle(v, true, m1, 2000m);

        Assert.Equal(2, v.MutuelId);
        Assert.Equal(50m, v.TauxMutuelle);          // taux d'origine conservé
        Assert.Equal(1000m, v.MontantMutuelle);
    }

    [Fact]
    public void MutuelleReglee_ne_change_que_si_la_mutuelle_ou_la_part_change()
    {
        var m1 = new Mutuel { IdMutuel = 2, TauxPriseEnCharge = 50 };
        var m0 = new Mutuel { IdMutuel = 1, TauxPriseEnCharge = 30 };

        var identique = new Vente { MutuelId = 2, TauxMutuelle = 50, MontantMutuelle = 500, MutuelleReglee = true };
        VenteModificationRegles.AppliquerMutuelle(identique, true, m1, 1000m);
        Assert.True(identique.MutuelleReglee);

        var partChange = new Vente { MutuelId = 2, TauxMutuelle = 50, MontantMutuelle = 500, MutuelleReglee = true };
        VenteModificationRegles.AppliquerMutuelle(partChange, true, m1, 2000m);
        Assert.False(partChange.MutuelleReglee);

        var autreMutuelle = new Vente { MutuelId = 2, TauxMutuelle = 50, MontantMutuelle = 500, MutuelleReglee = true };
        VenteModificationRegles.AppliquerMutuelle(autreMutuelle, true, m0, 1000m);
        Assert.False(autreMutuelle.MutuelleReglee);
        Assert.Equal(1, autreMutuelle.MutuelId);
    }

    [Fact]
    public void Vente_reglee_par_la_mutuelle_non_modifiable()
    {
        var v = new Vente { MutuelId = 2, MutuelleReglee = true, Statut = "Active" };
        var ex = Assert.Throws<InvalidOperationException>(() => VenteModificationRegles.VerifierModifiable(v));
        Assert.Equal("La mutuelle a déjà réglé cette vente : elle ne peut plus être modifiée.", ex.Message);

        VenteModificationRegles.VerifierModifiable(new Vente { MutuelId = 2, MutuelleReglee = false, Statut = "Active" });   // ne lève pas
    }

    // ───────── écran FormModificationVente ─────────

    [Fact]
    public void Ecran_presélectionne_la_mutuelle_d_origine_meme_archivee_avec_la_mention()
    {
        var (m0, m1, _, venteId) = Preparer(m1Archivee: true);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                var f = new Pharmacie2.views.FormModificationVente(venteId);
                var combo = UiHelper.Champ<ComboBox>(f, "cbMutuelle");
                var liste = (IList)combo.DataSource;

                Assert.Equal(2, liste.Count);   // m0 active + m1 archivée de la vente
                var selection = (Mutuel)combo.SelectedItem;
                Assert.Equal(m1, selection.IdMutuel);          // pas la première de la liste
                Assert.Contains("(archivée)", selection.NomEmployeur);
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Ecran_une_mutuelle_archivee_d_une_autre_vente_n_est_pas_proposee()
    {
        var (m0, m1, _, venteId) = Preparer(m1Archivee: false);
        ArchivageService.DefinirActif(TypeElement.Mutuelle, m0, false);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                var f = new Pharmacie2.views.FormModificationVente(venteId);
                var liste = (IList)UiHelper.Champ<ComboBox>(f, "cbMutuelle").DataSource;
                Assert.Single(liste);   // seule m1 (active, celle de la vente)
            });
        }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Ecran_modifier_une_quantite_garde_la_meme_mutuelle()
    {
        var (m0, m1, pid, venteId) = Preparer();
        var cts = UiHelper.FermerLesMessages(true);   // répond « Oui » à la confirmation
        try
        {
            UiHelper.EnSta(() =>
            {
                var f = new Pharmacie2.views.FormModificationVente(venteId);

                // Quantité 10 → 20 sur la ligne existante
                var lignes = (IList)f.GetType().GetField("_lignes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(f);
                var ligne = lignes[0];
                ligne.GetType().GetProperty("Quantite").SetValue(ligne, 20);
                UiHelper.Appeler(f, "CalculerTotal");
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        using var ctx = new AppDbContext();
        var v = ctx.ventes.First(x => x.IdVente == venteId);
        Assert.Equal(m1, v.MutuelId);                 // et non m0, la première de la liste
        Assert.Equal(2000m, v.MontantTotal);
        Assert.Equal(1000m, v.MontantMutuelle);       // 50 % de 2000
        Assert.Equal(90, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));   // 100 + 10 - 20
    }

    [Fact]
    public void Ecran_vente_reglee_affiche_le_message_et_ne_se_modifie_pas()
    {
        var (_, _, _, venteId) = Preparer(reglee: true);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() => { var f = new Pharmacie2.views.FormModificationVente(venteId); });
            Assert.True(UiHelper.TitreVu("Modification impossible"));
        }
        finally { cts.Cancel(); }

        Assert.Equal(1000, TestDb.Scalaire("SELECT CAST(MontantTotal AS INTEGER) FROM ventes"));
    }
}
