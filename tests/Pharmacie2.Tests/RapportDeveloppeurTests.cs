using System.IO.Compression;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class RapportDeveloppeurTests
{
    private static string DossierTemp()
    {
        string racine = Environment.GetEnvironmentVariable("RUNNER_TEMP") is { Length: > 0 } r ? r : Path.GetTempPath();
        string d = Path.Combine(racine, "p2rapport_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(d);
        return d;
    }

    [Fact]
    public void Le_zip_contient_infos_et_journaux_mais_aucune_donnee_personnelle_ou_metier()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        ParametresApp.ReinitialiserPourTests();

        // données sensibles dans la base de test
        using (var ctx = new AppDbContext())
        {
            ctx.Users.Add(new User { Nom = "Dupont", Prenom = "Jean", Login = "jean.dupont", Role = Roles.Administrateur, MotDePasse = MotDePasseService.Hacher("secret-tres-long-123") });
            ctx.produits.Add(new Produit { Nom = "MedicamentSecret", Type = "Autre", QuantiteEnStock = 3, NbUniteParBoite = 1, UniteVente = "Boîte", DateExpiration = DateTime.Today.AddYears(1), PrixVente = 98765 });
            ctx.SaveChanges();
            ctx.ventes.Add(new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "Rakotomalala", PrenomClient = "Zéphyrine", TelephoneClient = "0612345678", MotifAchat = "",
                MatriculeEmploye = "", MoyenPaiement = "Comptant", Type = "Comptant", MontantTotal = 98765, DateVente = DateTime.Now
            });
            ctx.SaveChanges();
        }
        SauvegardeAutomatique.Sauvegarder(Path.Combine(CheminsApp.DossierSauvegardes, $"PharmacieDB_{DateTime.Now:yyyy-MM-dd}.sqlite"));

        // le journal reçoit des lignes qui contiennent des informations personnelles
        Journal.Info("Archivage : Produit id 3 par Jean Dupont.");
        Journal.Info("Mot de passe du compte « jean.dupont » réinitialisé avec la clé de secours.");
        Journal.Info("Client joint au 0612345678 ou à zephyrine@exemple.com");
        Journal.Erreur("Fichier C:\\Users\\JeanDupont\\AppData\\Local\\Pharmacie2Data\\x.sqlite introuvable", new IOException("accès refusé"));
        Journal.Info("Ligne ordinaire sans rien de personnel.");

        string zip = RapportDeveloppeurService.Preparer(DossierTemp(), null, DateTime.Now);

        Assert.Matches(@"Rapport_LIGUAPHARME_\d{4}-\d{2}-\d{2}_\d{4}\.zip$", Path.GetFileName(zip));
        using var archive = ZipFile.OpenRead(zip);
        var noms = archive.Entries.Select(e => e.FullName).ToList();
        Assert.Contains("infos.txt", noms);
        Assert.Contains(noms, n => n.StartsWith("journaux/journal_") && n.EndsWith(".log"));
        Assert.DoesNotContain(noms, n => n.EndsWith(".sqlite") || n.EndsWith(".chiffre") || n.EndsWith(".bin") || n.EndsWith(".json"));   // ni base, ni clé, ni réglages

        string tout = string.Concat(archive.Entries.Select(e => { using var r = new StreamReader(e.Open()); return r.ReadToEnd(); }));
        foreach (var interdit in new[] { "Dupont", "jean.dupont", "JeanDupont", "Rakotomalala", "Zéphyrine", "0612345678", "zephyrine@", "98765", "MedicamentSecret", "$2a$", "$2b$", "secret-tres-long" })
            Assert.DoesNotContain(interdit, tout, StringComparison.OrdinalIgnoreCase);

        // mais les informations utiles au développeur sont bien là
        string infos = new StreamReader(archive.GetEntry("infos.txt")!.Open()).ReadToEnd();
        Assert.Contains("Version de l'application", infos);
        Assert.Contains("Windows", infos);
        Assert.Contains("Migrations appliquées", infos);
        Assert.Contains("Espace disque libre", infos);
        Assert.Contains("Taille de la base", infos);
        Assert.Contains("Dernière sauvegarde locale", infos);
        Assert.Contains("produits : 1", infos);
        Assert.Contains("ventes : 1", infos);
        Assert.Contains("Ligne ordinaire sans rien de personnel.", tout);
        Assert.Contains("[INFO]", tout);                                       // le niveau et l'heure restent lisibles
    }

    [Fact]
    public void Seuls_les_journaux_des_30_derniers_jours_sont_inclus()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        File.WriteAllText(Path.Combine(CheminsApp.DossierLogs, "journal_2020-01.log"),
            "2020-01-15 10:00:00 [INFO] très ancienne ligne\r\n2020-01-16 10:00:00 [INFO] autre ancienne ligne\r\n");
        Journal.Info("ligne récente");

        string zip = RapportDeveloppeurService.Preparer(DossierTemp(), null, DateTime.Now);
        using var archive = ZipFile.OpenRead(zip);
        Assert.DoesNotContain(archive.Entries, e => e.FullName.Contains("2020-01"));
        Assert.Contains(archive.Entries, e => e.FullName.StartsWith("journaux/journal_" + DateTime.Now.ToString("yyyy-MM")));
    }

    [Theory]
    [InlineData("Compte « admin » bloqué", "admin")]
    [InlineData("Archivage : Client id 4 par Fatima Said.", "Fatima")]
    [InlineData("Appelez le +269 333 44 55", "333")]
    public void Filtrage_ligne_par_ligne(string ligne, string interdit)
        => Assert.DoesNotContain(interdit, RapportDeveloppeurService.Filtrer(ligne));

    [Fact]
    public void Les_dates_et_versions_ne_sont_pas_masquees()
    {
        string ligne = "2026-10-09 11:22:33 [ERREUR] Version 8.0.5 du composant";
        Assert.Equal(ligne, RapportDeveloppeurService.Filtrer(ligne));
    }
}
