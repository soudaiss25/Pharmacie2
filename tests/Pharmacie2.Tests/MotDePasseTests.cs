using System.Text.RegularExpressions;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class MotDePasseTests
{
    private const string MigrationAvantCleSecours = "20261008192700_AjoutArchivage";

    [Fact]
    public void Conversion_sur_base_remplie_chaque_utilisateur_se_connecte_avec_son_ancien_mot_de_passe()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(MigrationAvantCleSecours);   // base « de production » avant la phase 8

        var comptes = new (string login, string mdp)[]
        {
            ("admin", "1234"),
            ("fatima", "Mot de passe é à ç ü"),   // accents et espaces internes
            ("ali", "a b  c d"),                  // espaces internes multiples
            ("hamidou", "p@ss;w'ord\"%"),         // caractères spéciaux
            ("zaïnaba", "été2026"),               // login accentué
        };
        string hashExistant = MotDePasseService.Hacher("déjà haché");

        using (var c = TestDb.Ouvrir())
        {
            long id = 1;
            foreach (var (login, mdp) in comptes)
                TestDb.Inserer(c, "Users", new() { ["Id"] = id++, ["Nom"] = "N", ["Prenom"] = "P", ["Role"] = "Caissier", ["Login"] = login, ["MotDePasse"] = mdp });
            // espaces autour : la connexion fait Trim() sur la saisie
            TestDb.Inserer(c, "Users", new() { ["Id"] = id++, ["Nom"] = "N", ["Prenom"] = "P", ["Role"] = "Caissier", ["Login"] = "espaces", ["MotDePasse"] = "  entoure  " });
            TestDb.Inserer(c, "Users", new() { ["Id"] = id++, ["Nom"] = "N", ["Prenom"] = "P", ["Role"] = "Caissier", ["Login"] = "dejahache", ["MotDePasse"] = hashExistant });
        }
        long nbAvant = TestDb.Scalaire("SELECT COUNT(*) FROM Users");

        TestDb.MigrerTout();
        int convertis;
        using (var ctx = new AppDbContext())
            convertis = MotDePasseService.ConvertirMotsDePasseEnClair(ctx);

        Assert.Equal(comptes.Length + 1, convertis);   // le compte déjà haché n'est pas touché
        Assert.Equal(nbAvant, TestDb.Scalaire("SELECT COUNT(*) FROM Users"));
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM Users WHERE MotDePasse NOT LIKE '$2%'"));

        foreach (var (login, mdp) in comptes)
        {
            Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier(login, mdp, out var u));
            Assert.Equal(login, u.Login);
            Assert.Equal(ResultatConnexion.Incorrect, AuthentificationService.Authentifier(login, mdp + "x", out _));
        }
        Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("espaces", "entoure", out _));
        Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("dejahache", "déjà haché", out _));
        Assert.Equal(ResultatConnexion.Incorrect, AuthentificationService.Authentifier("inconnu", "1234", out _));
    }

    [Fact]
    public void Conversion_idempotente_un_hash_n_est_jamais_re_hache()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        using (var ctx = new AppDbContext())
        {
            ctx.Users.Add(new User { Nom = "N", Prenom = "P", Role = "Caissier", Login = "a", MotDePasse = "clair" });
            ctx.SaveChanges();
            Assert.Equal(1, MotDePasseService.ConvertirMotsDePasseEnClair(ctx));
        }
        string apres1 = Convert.ToString(new AppDbContext().Users.First().MotDePasse);

        using (var ctx = new AppDbContext())
            Assert.Equal(0, MotDePasseService.ConvertirMotsDePasseEnClair(ctx));   // relance : rien à faire
        Assert.Equal(apres1, new AppDbContext().Users.First().MotDePasse);          // hash strictement identique
        Assert.StartsWith("$2", apres1);
        Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("a", "clair", out _));
    }

    [Fact]
    public void Compte_archive_et_hash_invalide()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        using (var ctx = new AppDbContext())
        {
            ctx.Users.Add(new User { Nom = "N", Prenom = "P", Role = "Caissier", Login = "arch", MotDePasse = MotDePasseService.Hacher("secret"), Actif = false });
            ctx.Users.Add(new User { Nom = "N", Prenom = "P", Role = "Caissier", Login = "brut", MotDePasse = "pas-un-hash" });
            ctx.SaveChanges();
        }
        Assert.Equal(ResultatConnexion.CompteArchive, AuthentificationService.Authentifier("arch", "secret", out var u));
        Assert.Null(u);
        Assert.Equal(ResultatConnexion.Incorrect, AuthentificationService.Authentifier("arch", "faux", out _));   // pas d'information sur l'archivage
        // un mot de passe encore en clair n'ouvre jamais l'accès (la conversion le traite au démarrage)
        Assert.Equal(ResultatConnexion.Incorrect, AuthentificationService.Authentifier("brut", "pas-un-hash", out _));
        Assert.False(MotDePasseService.Verifier("x", "n'importe quoi"));
    }
}

public class CleSecoursTests
{
    private static int NouvelAdmin(string login = "admin", string mdp = "ancien")
    {
        using var ctx = new AppDbContext();
        var u = new User { Nom = "N", Prenom = "P", Role = "Administrateur", Login = login, MotDePasse = MotDePasseService.Hacher(mdp) };
        ctx.Users.Add(u);
        ctx.SaveChanges();
        return u.Id;
    }

    private static void Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        CleSecoursService.OublierLesLoginsInconnus();
        CleSecoursService.Maintenant = () => DateTime.Now;
    }

    [Fact]
    public void Format_4_groupes_de_4_sans_caracteres_ambigus()
    {
        var motif = new Regex("^[ABCDEFGHJKMNPQRSTUVWXYZ2-9]{4}(-[ABCDEFGHJKMNPQRSTUVWXYZ2-9]{4}){3}$");
        for (int i = 0; i < 2000; i++)
        {
            string cle = CleSecoursService.Generer();
            Assert.Matches(motif, cle);
            Assert.DoesNotContain("0", cle); Assert.DoesNotContain("O", cle);
            Assert.DoesNotContain("1", cle); Assert.DoesNotContain("I", cle); Assert.DoesNotContain("L", cle);
        }
        Assert.NotEqual(CleSecoursService.Generer(), CleSecoursService.Generer());
    }

    [Fact]
    public void Reinitialisation_par_la_cle_puis_ancienne_cle_invalide()
    {
        Preparer();
        int id = NouvelAdmin();
        string cle1 = CleSecoursService.Generer();
        CleSecoursService.Definir(id, cle1);
        Assert.True(CleSecoursService.AUneCle(id));
        Assert.DoesNotContain(cle1, new AppDbContext().Users.First().CleSecoursHash);   // seul le hash est stocké

        // la clé se saisit avec ou sans tirets, en minuscules
        int idReset = CleSecoursService.Reinitialiser("admin", cle1.Replace("-", " ").ToLowerInvariant(), "nouveau mot de passe");
        Assert.Equal(id, idReset);

        Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("admin", "nouveau mot de passe", out _));
        Assert.Equal(ResultatConnexion.Incorrect, AuthentificationService.Authentifier("admin", "ancien", out _));

        // la clé est consommée : même sans nouvelle clé enregistrée, la première ne fonctionne plus
        Assert.False(CleSecoursService.AUneCle(id));
        Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("admin", cle1, "autre"));

        // nouvelle clé générée et enregistrée : l'ancienne reste invalide, la nouvelle fonctionne
        string cle2 = CleSecoursService.Generer();
        CleSecoursService.Definir(id, cle2);
        Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("admin", cle1, "autre"));
        CleSecoursService.Reinitialiser("admin", cle2, "encore un autre");
        Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("admin", "encore un autre", out _));
    }

    [Fact]
    public void Blocage_15_minutes_apres_5_essais_errones_puis_deblocage()
    {
        Preparer();
        int id = NouvelAdmin();
        string cle = CleSecoursService.Generer();
        CleSecoursService.Definir(id, cle);
        var t0 = new DateTime(2026, 10, 8, 12, 0, 0);
        CleSecoursService.Maintenant = () => t0;
        try
        {
            for (int i = 1; i <= 4; i++)
                Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("admin", "AAAA-AAAA-AAAA-AAAA", "x"));
            Assert.Throws<CleSecoursBloqueeException>(() => CleSecoursService.Reinitialiser("admin", "AAAA-AAAA-AAAA-AAAA", "x"));   // 5e essai

            // même la bonne clé est refusée pendant le blocage
            Assert.Throws<CleSecoursBloqueeException>(() => CleSecoursService.Reinitialiser("admin", cle, "x"));
            CleSecoursService.Maintenant = () => t0.AddMinutes(14);
            Assert.Throws<CleSecoursBloqueeException>(() => CleSecoursService.Reinitialiser("admin", cle, "x"));

            // après 15 minutes, la bonne clé fonctionne à nouveau
            CleSecoursService.Maintenant = () => t0.AddMinutes(16);
            CleSecoursService.Reinitialiser("admin", cle, "debloque");
            Assert.Equal(ResultatConnexion.Succes, AuthentificationService.Authentifier("admin", "debloque", out _));
        }
        finally { CleSecoursService.Maintenant = () => DateTime.Now; }
    }

    [Fact]
    public void Un_succes_remet_le_compteur_d_essais_a_zero()
    {
        Preparer();
        int id = NouvelAdmin();
        string cle = CleSecoursService.Generer();
        CleSecoursService.Definir(id, cle);
        for (int i = 0; i < 4; i++)
            Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("admin", "ZZZZ-ZZZZ-ZZZZ-ZZZZ", "x"));
        CleSecoursService.Reinitialiser("admin", cle, "ok");
        Assert.Equal(0, TestDb.Scalaire("SELECT EssaisCleEchoues FROM Users WHERE Login='admin'"));
    }

    [Fact]
    public void Login_inconnu_ou_non_administrateur_meme_traitement_et_meme_blocage()
    {
        Preparer();
        using (var ctx = new AppDbContext())
        {
            var u = new User { Nom = "N", Prenom = "P", Role = "Caissier", Login = "caisse", MotDePasse = MotDePasseService.Hacher("x") };
            ctx.Users.Add(u);
            ctx.SaveChanges();
            CleSecoursService.Definir(u.Id, "AAAA-BBBB-CCCC-DDDD");
        }
        // un caissier ne peut pas utiliser cette fonction, même avec une « bonne » clé
        Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("caisse", "AAAA-BBBB-CCCC-DDDD", "y"));
        for (int i = 0; i < 3; i++)
            Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("fantome", "AAAA-AAAA-AAAA-AAAA", "y"));
        Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("fantome", "AAAA-AAAA-AAAA-AAAA", "y"));
        Assert.Throws<CleSecoursBloqueeException>(() => CleSecoursService.Reinitialiser("fantome", "AAAA-AAAA-AAAA-AAAA", "y"));
    }

    [Fact]
    public void La_cle_n_est_jamais_ecrite_dans_le_journal()
    {
        Preparer();
        int id = NouvelAdmin();
        string cle = CleSecoursService.Generer();
        CleSecoursService.Definir(id, cle);
        for (int i = 0; i < 2; i++)
            Assert.Throws<CleSecoursInvalideException>(() => CleSecoursService.Reinitialiser("admin", cle.Replace(cle[0], cle[0] == 'A' ? 'B' : 'A'), "x"));
        CleSecoursService.Reinitialiser("admin", cle, "nouveau");

        string normalisee = CleSecoursService.Normaliser(cle);
        foreach (var f in Directory.GetFiles(CheminsApp.DossierLogs))
        {
            string texte = File.ReadAllText(f);
            Assert.DoesNotContain(cle, texte);
            Assert.DoesNotContain(normalisee, texte);
        }
        Assert.DoesNotContain(normalisee, new AppDbContext().Users.First().MotDePasse);
    }
}
