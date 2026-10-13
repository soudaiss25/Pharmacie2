using System.IO.Compression;
using System.Text;
using Pharmacie2.Services;
using Xunit;

/// <summary>Le ZIP du développeur ne contient ni nom de client, ni téléphone, ni motif d'achat (journaux, informations et diagnostic compris).</summary>
public class RapportSansDonneesPersonnellesTests
{
    private static string TexteDe(ZipArchive archive, StringBuilder sb)
    {
        foreach (var e in archive.Entries)
        {
            using var flux = e.Open();
            if (e.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                // le classeur est lui-même un ZIP de fichiers XML : on lit chacun
                using var ms = new MemoryStream();
                flux.CopyTo(ms);
                ms.Position = 0;
                using var xlsx = new ZipArchive(ms, ZipArchiveMode.Read);
                TexteDe(xlsx, sb);
            }
            else
            {
                using var r = new StreamReader(flux, Encoding.UTF8);
                sb.AppendLine(r.ReadToEnd());
            }
        }
        return sb.ToString();
    }

    [Fact]
    public void Le_ZIP_ne_contient_aucun_nom_de_client_telephone_ou_motif_d_achat()
    {
        DiagnosticBaseTests.Preparer();
        using (var c = TestDb.Ouvrir())
        using (var cmd = c.CreateCommand())
        {
            cmd.CommandText = "UPDATE ventes SET PrenomClient = 'Hadidja', TelephoneClient = '+269 333 44 55', MotifAchat = 'Traitement du diabète de la mère'";
            cmd.ExecuteNonQuery();
        }
        // le journal reçoit aussi une ligne qui pourrait trahir un client (nom entre guillemets, téléphone) : elle est filtrée ; l'application ne journalise jamais de motif d'achat
        Journal.Info("Vente pour « Zéphyrine Rakotomalala », joignable au +269 333 44 55");
        DiagnosticBaseService.Enregistrer(DiagnosticBaseTests.Lancer(appliquer: true));

        try
        {
            string zip = RapportDeveloppeurService.Preparer(Path.Combine(Path.GetTempPath(), "p2_zip_" + Guid.NewGuid().ToString("N")), null, DiagnosticBaseTests.Maintenant);
            using var archive = ZipFile.OpenRead(zip);
            Assert.Contains(archive.Entries, e => e.FullName == "diagnostic_base.xlsx");   // le diagnostic y est bien

            string tout = TexteDe(archive, new StringBuilder());
            foreach (var interdit in new[] { "Zéphyrine", "Zephyrine", "Rakotomalala", "Hadidja", "333 44 55", "3334455", "diabète", "diabete", "Traitement du" })
                Assert.DoesNotContain(interdit, tout, StringComparison.OrdinalIgnoreCase);
            // mais les numéros de vente et les noms de produits y sont (utiles au développeur)
            Assert.Contains("V-000101", tout);
        }
        finally { File.Delete(Path.Combine(CheminsApp.DossierDonnees, "dernier_diagnostic.json")); }
    }

    [Fact]
    public void La_mention_de_confidentialite_est_la_meme_partout()
    {
        const string attendu = "Il ne contient aucune donnée personnelle : ni nom de client, ni téléphone, ni motif d'achat.";
        Assert.Equal(attendu, RapportDeveloppeurService.MentionConfidentialite);
        Assert.EndsWith(attendu, RapportDeveloppeurService.MessageApresRapport);
        // plus aucune trace de l'ancienne formule dans le code source de l'application
        string racine = AppContext.BaseDirectory;
        for (int i = 0; i < 8 && !File.Exists(Path.Combine(racine, "Pharmacie2.csproj")); i++) racine = Path.GetFullPath(Path.Combine(racine, ".."));
        if (File.Exists(Path.Combine(racine, "Pharmacie2.csproj")))
        {
            foreach (var f in Directory.GetFiles(racine, "*.cs", SearchOption.AllDirectories).Where(f => !f.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar)
                                                                                                         && !f.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar)
                                                                                                         && !f.Contains(Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar)))
                Assert.DoesNotContain("aucune donnée de la pharmacie", File.ReadAllText(f));
        }
    }
}
