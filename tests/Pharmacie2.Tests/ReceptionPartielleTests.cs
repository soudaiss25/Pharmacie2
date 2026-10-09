using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.views;
using Xunit;

public class ReceptionPartielleTests
{
    private static (int produit, int cmd) Commande(int boites, string statut = "En attente", int recue = 0)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Amox", 0, 5, "Comprimé");
        using var ctx = new AppDbContext();
        ctx.fournisseur.Add(new Fournisseur { Nom = "Four", Contact = "" });
        ctx.SaveChanges();
        var c = new Commande { FournisseurId = ctx.fournisseur.First().Id, Statut = statut, Lignes = { new LigneCommande { ProduitId = pid, Quantite = boites, QuantiteRecue = recue } } };
        ctx.commandes.Add(c);
        ctx.SaveChanges();
        return (pid, c.Id);
    }

    [Fact]
    public void La_grille_propose_le_reste_puis_valide_la_quantite_saisie()
    {
        var (pid, cmd) = Commande(10);
        UiHelper.EnSta(() =>
        {
            using var f = new FormReceptionPartielle(cmd);
            var grille = UiHelper.Champ<DataGridView>(f, "dgvLignes");
            var ligne = grille.Rows[0];
            Assert.Equal(10, Convert.ToInt32(ligne.Cells["colReste"].Value));
            Assert.Equal(10, Convert.ToInt32(ligne.Cells["colRecu"].Value));   // préremplie avec le reste

            ligne.Cells["colRecu"].Value = 4;
            UiHelper.Appeler(f, "btnValider_Click", null!, EventArgs.Empty);
        });
        Assert.Equal(20, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));   // 4 boîtes de 5
        Assert.Equal(4, TestDb.Scalaire($"SELECT QuantiteRecue FROM LigneCommandes WHERE CommandeId={cmd}"));
        Assert.Equal("Reçu partiellement", TestDb.Texte($"SELECT Statut FROM commandes WHERE Id={cmd}"));
    }

    [Fact]
    public void Commande_partielle_ancienne_bloque_tout_recevoir_et_prerempli_zero()
    {
        var (pid, cmd) = Commande(10, "Reçu partiellement", 0);
        UiHelper.EnSta(() =>
        {
            using var f = new FormReceptionPartielle(cmd);
            Assert.False(UiHelper.Champ<Button>(f, "btnToutRecevoir").Enabled);
            Assert.NotEmpty(UiHelper.Champ<Label>(f, "lblAvertissement").Text);
            Assert.Equal(0, Convert.ToInt32(UiHelper.Champ<DataGridView>(f, "dgvLignes").Rows[0].Cells["colRecu"].Value));
        });
        Assert.Equal(0, TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}"));
    }
}
