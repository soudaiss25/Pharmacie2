using ClosedXML.Excel;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

/// <summary>Export Excel d'une mutuelle : ventes non réglées par défaut, deux versions du fichier, détail des médicaments.</summary>
public class MutuelleExportTests
{
    private static readonly DateTime Debut = new(2026, 9, 1);
    private static readonly DateTime Fin = new(2026, 9, 30, 23, 59, 59);
    private const string Telephone = "0612345678";

    private sealed class Donnees { public int Mutuelle; public int Produit1, Produit2; }

    private static Donnees Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var d = new Donnees();
        d.Produit1 = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 500m);
        d.Produit2 = TestDb.NouveauProduit("Amoxicilline", 100, 1, "Boîte", prixVente: 1500m);
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        var m = new Mutuel { NomEmployeur = "Entreprise X", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
        ctx.Users.Add(admin);
        ctx.mutuels.Add(m);
        ctx.SaveChanges();
        d.Mutuelle = m.IdMutuel;

        // V1 : identifiée, non réglée, 2 lignes (500×2 + 1500 = 2 500)
        Vente(ctx, admin.Id, m.IdMutuel, "V-000001", "Hamidou", "Ali", "M-1", Telephone, "Ordonnance Dr X", false, 2500, 1250, 300,
            (d.Produit1, 2, 500m), (d.Produit2, 1, 1500m));
        // V2 : identifiée, déjà réglée par la mutuelle le 15/10/2026, 1 ligne
        Vente(ctx, admin.Id, m.IdMutuel, "V-000002", "Said", "Mariam", "M-2", Telephone, "", true, 1000, 500, 500, (d.Produit1, 2, 500m));
        // V3 : sans nom ni matricule, non réglée, 1 ligne
        Vente(ctx, admin.Id, m.IdMutuel, "V-000003", "", "", "", "", "", false, 1500, 750, 750, (d.Produit2, 1, 1500m));
        // V4 : annulée (jamais exportée)
        Vente(ctx, admin.Id, m.IdMutuel, "V-000004", "Annulé", "Client", "M-4", Telephone, "", false, 500, 250, 250, (d.Produit1, 1, 500m)).Statut = "Annulée";
        ctx.SaveChanges();
        ctx.MutuelPaiements.Add(new MutuelPaiement
        {
            NumeroPaiement = "MP-1", DatePaiement = new DateTime(2026, 10, 15), Montant = 500, Reference = "", Commentaire = "", VenteId = ctx.ventes.Single(v => v.numeroVente == "V-000002").IdVente, MutuelId = m.IdMutuel
        });
        ctx.SaveChanges();
        return d;
    }

    private static Vente Vente(AppDbContext ctx, int userId, int mutuelId, string numero, string nom, string prenom, string matricule, string tel, string motif,
        bool reglee, decimal total, decimal mutuelle, decimal verse, params (int Produit, int Quantite, decimal Prix)[] lignes)
    {
        var v = new Vente
        {
            numeroVente = numero, Statut = "Active", NomClient = nom, PrenomClient = prenom, TelephoneClient = tel, MotifAchat = motif, MatriculeEmploye = matricule,
            MoyenPaiement = ModesPaiement.Mutuelle, Type = ModesPaiement.Mutuelle, MotifAnnulation = "", MontantTotal = total, MutuelId = mutuelId,
            TauxMutuelle = 50, MontantMutuelle = mutuelle, MutuelleReglee = reglee, UserId = userId, DateVente = new DateTime(2026, 9, 10, 10, 0, 0)
        };
        foreach (var l in lignes)
            v.Lignes.Add(new LigneVente { ProduitId = l.Produit, Quantite = l.Quantite, QuantiteUnites = l.Quantite, PrixUnitaire = l.Prix, UniteVendue = "Boîte" });
        if (verse != 0) v.Paiements.Add(new Paiement { NumeroPaiement = "P-" + numero, Montant = verse, Mode = ModesPaiement.Especes, UserId = userId, DatePaiement = v.DateVente });
        ctx.ventes.Add(v);
        return v;
    }

    private static XLWorkbook Exporter(int mutuelle, bool nonReglees, MutuelleExportService.VersionFichier version, out string chemin)
    {
        chemin = Path.Combine(Path.GetTempPath(), $"export_mutuelle_{Guid.NewGuid():N}.xlsx");
        MutuelleExportService.Exporter(chemin, mutuelle, "Entreprise X", Debut, Fin, nonReglees, version);
        return new XLWorkbook(chemin);
    }

    private static List<string> Textes(XLWorkbook wb)
        => wb.Worksheets.SelectMany(w => w.CellsUsed()).Select(c => c.GetFormattedString()).ToList();

    private static List<string> ColonneNumeros(IXLWorksheet ws)
        => ws.CellsUsed().Where(c => c.Address.ColumnNumber == 1 && c.GetString().StartsWith("V-")).Select(c => c.GetString()).ToList();

    [Fact]
    public void Une_vente_deja_reglee_n_apparait_pas_dans_l_export_non_reglees_mais_dans_toutes_avec_sa_date()
    {
        var d = Preparer();
        string chemin;
        using (var wb = Exporter(d.Mutuelle, true, MutuelleExportService.VersionFichier.Entreprise, out chemin))
        {
            Assert.Equal(new[] { "V-000001", "V-000003" }, ColonneNumeros(wb.Worksheet("Détail employés")));
            Assert.Contains("Ventes non réglées", Textes(wb));
            Assert.DoesNotContain("V-000002", Textes(wb));
        }
        using (var wb = Exporter(d.Mutuelle, false, MutuelleExportService.VersionFichier.Entreprise, out var chemin2))
        {
            Assert.Equal(new[] { "V-000001", "V-000002", "V-000003" }, ColonneNumeros(wb.Worksheet("Détail employés")));
            var textes = Textes(wb);
            Assert.Contains("Toutes les ventes", textes);
            Assert.Contains("Oui (15/10/2026)", textes);
            Assert.Contains("Non", textes);
            File.Delete(chemin2);
        }
        File.Delete(chemin);
    }

    [Fact]
    public void La_version_pour_l_entreprise_ne_contient_ni_telephone_ni_versement_ni_reste_du()
    {
        var d = Preparer();
        using var wb = Exporter(d.Mutuelle, false, MutuelleExportService.VersionFichier.Entreprise, out var chemin);
        var textes = Textes(wb);
        Assert.DoesNotContain(textes, t => t.Contains(Telephone));
        foreach (var interdit in new[] { "Téléphone", "Montant versé", "Reste à payer", "Part patient", "Motif", "Ordonnance Dr X", "Statut" })
            Assert.DoesNotContain(textes, t => t.Contains(interdit));
        foreach (var attendu in new[] { "Matricule", "Médicaments achetés", "Montant total", "Taux mutuelle (%)", "Part Entreprise X" })
            Assert.Contains(attendu, textes);
        wb.Dispose();
        File.Delete(chemin);
    }

    [Fact]
    public void La_version_interne_garde_toutes_les_colonnes_et_remplit_le_motif()
    {
        var d = Preparer();
        using var wb = Exporter(d.Mutuelle, false, MutuelleExportService.VersionFichier.Interne, out var chemin);
        var textes = Textes(wb);
        foreach (var attendu in new[] { "Téléphone", "Motif / ordonnance", "Part patient", "Montant versé", "Reste à payer", "Statut", "Réglée par la mutuelle", "Ordonnance Dr X", Telephone })
            Assert.Contains(attendu, textes);
        wb.Dispose();
        File.Delete(chemin);
    }

    [Fact]
    public void L_onglet_des_medicaments_a_autant_de_lignes_que_les_lignes_de_vente_avec_un_total_par_vente()
    {
        var d = Preparer();
        using var wb = Exporter(d.Mutuelle, true, MutuelleExportService.VersionFichier.Entreprise, out var chemin);
        var ws = wb.Worksheet("Détail des médicaments");
        var medicaments = ws.CellsUsed().Where(c => c.Address.ColumnNumber == 1 && c.GetString().StartsWith("V-")).ToList();
        Assert.Equal(3, medicaments.Count);                                        // V1 : 2 lignes, V3 : 1 ligne (V2 réglée, V4 annulée)
        var totaux = ws.CellsUsed().Where(c => c.Address.ColumnNumber == 4 && c.GetString().StartsWith("Total de la vente")).Select(c => c.GetString()).ToList();
        Assert.Equal(new[] { "Total de la vente V-000001", "Total de la vente V-000003" }, totaux);
        var ligne1 = medicaments.First().WorksheetRow();
        Assert.Equal("Doliprane", ligne1.Cell(4).GetString());
        Assert.Equal("Boîte", ligne1.Cell(5).GetString());
        Assert.Equal(2, ligne1.Cell(6).GetValue<int>());
        Assert.Equal(500m, ligne1.Cell(7).GetValue<decimal>());
        Assert.Equal(1000m, ligne1.Cell(8).GetValue<decimal>());
        Assert.Equal("Ali Hamidou", ligne1.Cell(3).GetString());
        wb.Dispose();
        File.Delete(chemin);
    }

    [Fact]
    public void L_entete_donne_la_pharmacie_l_entreprise_la_periode_et_la_date_d_edition()
    {
        var d = Preparer();
        using var wb = Exporter(d.Mutuelle, true, MutuelleExportService.VersionFichier.Entreprise, out var chemin);
        foreach (var ws in wb.Worksheets)
        {
            Assert.Equal(AppInfo.NomPharmacie, ws.Cell("A1").GetString());
            Assert.Equal("Entreprise : Entreprise X", ws.Cell("A2").GetString());
            Assert.Contains("du 01/09/2026 au 30/09/2026", ws.Cell("A3").GetString());
            Assert.Contains("Édité le : " + DateTime.Now.ToString("dd/MM/yyyy"), ws.Cell("A3").GetString());
            Assert.Equal("Ventes non réglées", ws.Cell("A4").GetString());
        }
        wb.Dispose();
        File.Delete(chemin);
    }

    [Fact]
    public void Le_recap_regroupe_les_ventes_sans_identite_sous_Patient_non_identifie_en_dernier()
    {
        var d = Preparer();
        using var wb = Exporter(d.Mutuelle, true, MutuelleExportService.VersionFichier.Entreprise, out var chemin);
        var ws = wb.Worksheet("Récap par employé");
        var lignes = ws.RowsUsed().Where(r => r.Cell(1).GetString() != "" && r.RowNumber() > 6 && r.Cell(2).GetString() != "TOTAL").ToList();
        Assert.Equal(2, lignes.Count);
        Assert.Equal("Hamidou", lignes[0].Cell(2).GetString());
        Assert.Equal("Patient non identifié", lignes[1].Cell(2).GetString());     // en dernier
        Assert.Equal(1, lignes[1].Cell(4).GetValue<int>());                         // avec leur nombre
        wb.Dispose();
        File.Delete(chemin);
    }
}
