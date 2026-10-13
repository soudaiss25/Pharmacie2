using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>Fiche produit « à vérifier » (D5, D6, D8) : posée à l'import seulement, visible, levée par le bouton ou par la correction.</summary>
public class FicheAVerifierTests
{
    private static Produit Lire(int id)
    {
        using var ctx = new AppDbContext();
        return ctx.produits.AsNoTracking().Single(p => p.Id == id);
    }

    [Fact]
    public void Le_marquage_est_pose_a_l_import_sur_D5_D6_et_D8_seulement()
    {
        var ids = DiagnosticBaseTests.Preparer();
        DiagnosticBaseTests.Lancer(appliquer: true);

        var prix = Lire(ids.Prix);                       // D5
        Assert.True(prix.FicheAVerifier);
        Assert.Contains("Prix : prix de vente", prix.MotifsFiche);
        var aberrant = Lire(ids.Aberrant);               // D6
        Assert.True(aberrant.FicheAVerifier);
        Assert.Contains("Prix : prix peut-être multiplié par 100", aberrant.MotifsFiche);
        var dateVide = Lire(ids.DateVide);               // D8 (péremption vide)
        Assert.True(dateVide.FicheAVerifier);
        Assert.StartsWith("Péremption : ", dateVide.MotifsFiche);

        // les autres produits ne sont pas marqués (D8 « périmé en stock » et D9 sont de simples informations)
        foreach (var id in new[] { ids.Normal, ids.Perime, ids.Dup1, ids.Dup2, ids.Unite, ids.Cmd })
            Assert.False(Lire(id).FicheAVerifier, "produit " + id);

        // idempotent : un deuxième passage ne double aucun motif
        string motifs = prix.MotifsFiche!;
        DiagnosticBaseTests.Lancer(appliquer: true);
        Assert.Equal(motifs, Lire(ids.Prix).MotifsFiche);
    }

    [Fact]
    public void En_lecture_seule_aucun_marquage_n_est_ecrit()
    {
        var ids = DiagnosticBaseTests.Preparer();
        DiagnosticBaseTests.Lancer(appliquer: false);
        using var ctx = new AppDbContext();
        Assert.Equal(0, ctx.produits.Count(p => p.FicheAVerifier || p.MotifsFiche != null));
    }

    [Fact]
    public void Le_bouton_J_ai_verifie_cette_fiche_leve_le_marquage_et_le_journalise()
    {
        var ids = DiagnosticBaseTests.Preparer();
        DiagnosticBaseTests.Lancer(appliquer: true);
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.Prix);
                var bandeau = UiHelper.Champ<TableLayoutPanel>(f, "pnlFiche");
                Assert.True(bandeau.Visible);
                Assert.Contains("Prix : prix de vente", UiHelper.Champ<Label>(f, "lblFiche").Text);
                UiHelper.Appeler(f, "btnFicheVerifiee_Click", null, EventArgs.Empty);
                Assert.False(bandeau.Visible);
            });
        }
        finally { cts.Cancel(); }

        var p = Lire(ids.Prix);
        Assert.False(p.FicheAVerifier);
        Assert.Null(p.MotifsFiche);
        string journal = File.ReadAllText(Directory.GetFiles(CheminsApp.DossierLogs, "journal_*.log").OrderBy(f => f).Last());
        Assert.Contains("Fiche produit « Prix trop bas » vérifiée par l'utilisateur", journal);
    }

    [Fact]
    public void Corriger_le_prix_leve_le_motif_prix_mais_pas_le_motif_peremption()
    {
        var ids = DiagnosticBaseTests.Preparer();
        using (var ctx = new AppDbContext())
        {
            var p = ctx.produits.Single(x => x.Id == ids.Prix);
            FicheAVerifierService.Marquer(p, "Prix : prix de vente trop bas");
            FicheAVerifierService.Marquer(p, "Péremption : date vide ou invraisemblable");
            ctx.SaveChanges();
        }

        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.Prix);
                UiHelper.Champ<NumericUpDown>(f, "numPrixVente").Value = 500;       // nouveau prix : 500 > achat 200
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }

        var apres = Lire(ids.Prix);
        Assert.Equal(500m, apres.PrixVente);
        Assert.True(apres.FicheAVerifier);                                  // il reste le motif de péremption
        Assert.Equal("Péremption : date vide ou invraisemblable", apres.MotifsFiche);

        cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddProduit(ids.Prix);
                UiHelper.Champ<DateTimePicker>(f, "dtpDateExpiration").Value = new DateTime(2029, 6, 30);   // nouvelle date
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
        var fin = Lire(ids.Prix);
        Assert.False(fin.FicheAVerifier);
        Assert.Null(fin.MotifsFiche);
    }

    [Fact]
    public void A_la_vente_d_un_produit_au_prix_a_verifier_un_avertissement_est_affiche_la_vente_reste_possible()
    {
        var ids = DiagnosticBaseTests.Preparer();
        DiagnosticBaseTests.Lancer(appliquer: true);
        var aberrant = Lire(ids.Aberrant);
        string? texte = FicheAVerifierService.AvertissementPrix(aberrant, aberrant.PrixVente);
        Assert.NotNull(texte);
        Assert.Contains("Prix à vérifier : " + Format.Montant(600000), texte);
        Assert.Contains("La vente reste possible", texte);
        Assert.Null(FicheAVerifierService.AvertissementPrix(Lire(ids.Normal), 200));                         // produit sans marquage
        Assert.Null(FicheAVerifierService.AvertissementPrix(Lire(ids.DateVide), 200));                       // marqué seulement pour sa péremption : pas d'avertissement de prix
    }

    [Fact]
    public void Ma_journee_Stock_et_Catalogue_montrent_le_meme_badge_et_le_meme_filtre()
    {
        var ids = DiagnosticBaseTests.Preparer();
        DiagnosticBaseTests.Lancer(appliquer: true);
        int fiches;
        using (var ctx = new AppDbContext()) fiches = FicheAVerifierService.Nombre(ctx);
        Assert.Equal(3, fiches);

        // Ma journée : ligne « X fiches produits à vérifier » dans « À faire maintenant »
        var aFaire = TableauDeBordService.AFaireMaintenant(DiagnosticBaseTests.Maintenant).Single(a => a.Type == TypeAFaire.FichesAVerifier);
        Assert.Equal(3, aFaire.Nombre);
        Assert.Contains("3 fiches produits à vérifier", aFaire.Texte.Replace(' ', ' '));

        int attendus;
        using (var ctx = new AppDbContext()) attendus = ctx.produits.Count(p => (p.StockAVerifier || p.FicheAVerifier) && p.Actif);

        UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var stock = new Uc_Stock { Dock = DockStyle.Fill };
            hote.Controls.Add(stock);
            hote.Show();
            Application.DoEvents();
            stock.Filtrer("À vérifier");
            var g = UiHelper.Champ<DataGridView>(stock, "dgvStock");
            Assert.Equal(attendus, g.Rows.Count);
            Assert.All(g.Rows.Cast<DataGridViewRow>(), r => Assert.Equal("À vérifier", Convert.ToString(r.Cells["Verification"].Value)));
            stock.Dispose();

            var catalogue = new Uc_Produits { Dock = DockStyle.Fill };
            hote.Controls.Clear();
            hote.Controls.Add(catalogue);
            Application.DoEvents();
            var gp = UiHelper.Champ<DataGridView>(catalogue, "dgvProduits");
            int total = gp.Rows.Count;
            UiHelper.Champ<CheckBox>(catalogue, "chkAVerifier").Checked = true;
            Assert.Equal(attendus, gp.Rows.Count);
            Assert.True(total > attendus);
            Assert.All(gp.Rows.Cast<DataGridViewRow>(), r => Assert.Equal("À vérifier", Convert.ToString(r.Cells["Verification"].Value)));
            catalogue.Dispose();
        });
    }

    [Fact]
    public void Migration_AjoutFicheAVerifier_garde_les_produits_et_met_la_colonne_a_faux()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("ArrondiRepriseVentes"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "produits", new() { ["Id"] = 1L, ["Nom"] = "A", ["PrixVente"] = "200", ["NbUniteParBoite"] = 1L });
            TestDb.Inserer(c, "produits", new() { ["Id"] = 2L, ["Nom"] = "B", ["PrixVente"] = "300", ["NbUniteParBoite"] = 1L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerTout();
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM produits WHERE FicheAVerifier = 0 AND MotifsFiche IS NULL"));
    }
}
