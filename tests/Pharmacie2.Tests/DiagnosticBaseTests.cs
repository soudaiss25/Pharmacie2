using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>
/// Diagnostic de l'ancienne base (règles D1 à D23). La base « ancienne » de test contient au moins un cas de CHAQUE règle ;
/// chaque règle produit exactement le résultat attendu et un deuxième passage ne change plus rien (idempotence).
/// </summary>
public class DiagnosticBaseTests
{
    private const string Client = "Zéphyrine Rakotomalala";   // ne doit apparaître dans aucun rapport

    internal sealed class Ids
    {
        public int Neg, Nb0, Reception, Cmd, Prix, Aberrant, Unite, DateVide, Perime, Dup1, Dup2, Normal;
        public int V1, V2, V3a, V3b, V4a, V4b, V5, V6, V7, V8, V9, V10, V11, V12, V13;
        public int M1, M2, SessionHier, SessionAujourdhui;
    }

    internal static readonly DateTime Maintenant = new(2026, 10, 13, 10, 0, 0);

    private static Produit Prod(string nom, decimal achat = 100, decimal vente = 200, int nb = 1, decimal? unite = null, string type = "Autre", int stock = 10, DateTime? exp = null)
        => new()
        {
            Nom = nom, Type = type, PrixAchat = achat, PrixVente = vente, NbUniteParBoite = nb, PrixVenteUnite = unite, QuantiteEnStock = stock,
            UniteVente = nb > 1 ? "Plaquette" : "Boîte", SeuilAlerte = 1, DateExpiration = exp ?? new DateTime(2028, 1, 1), Indication = "", Posologie = ""
        };

    private static Vente NouvelleVente(string numero, string type, decimal total, int? userId, int produitId, DateTime? date = null, bool ligne = true)
    {
        var v = new Vente
        {
            numeroVente = numero, Statut = "Active", NomClient = Client, PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = type, Type = type, MotifAnnulation = "", MontantTotal = total, UserId = userId, DateVente = date ?? Maintenant.AddDays(-3)
        };
        if (ligne) v.Lignes.Add(new LigneVente { ProduitId = produitId, Quantite = 1, QuantiteUnites = 1, PrixUnitaire = total, UniteVendue = "Boîte" });
        return v;
    }

    private static Paiement Pai(string numero, decimal montant, int? userId, string mode = ModesPaiement.Especes, DateTime? date = null)
        => new() { NumeroPaiement = numero, Montant = montant, Mode = mode, UserId = userId, DatePaiement = date ?? Maintenant.AddDays(-3) };

    /// <summary>L'ancienne base : au moins un cas de chaque règle D1 à D23, plus des éléments normaux qui ne doivent rien déclencher.</summary>
    internal static Ids Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ids = new Ids();
        using var ctx = new AppDbContext();

        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        ctx.Users.Add(admin);
        var m1 = new Mutuel { NomEmployeur = "Entreprise Un", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
        var m2 = new Mutuel { NomEmployeur = "Entreprise Deux", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 50 };
        var four = new Fournisseur { Nom = "Four", Contact = "c" };
        ctx.mutuels.AddRange(m1, m2);
        ctx.fournisseur.Add(four);
        ctx.SaveChanges();
        int u = admin.Id;
        ids.M1 = m1.IdMutuel; ids.M2 = m2.IdMutuel;

        // ── produits ──
        var normal = Prod("Produit normal");
        var neg = Prod("Stock négatif", stock: -4);                                  // D1
        var nb0 = Prod("Conditionnement nul", nb: 0, stock: 5);                      // D4
        var reception = Prod("Ancienne réception"); reception.StockAVerifier = true; reception.MotifVerification = "10 boîtes reçues avant la correction";   // D2
        var cmd = Prod("Dans une commande annulée");                                 // D3
        var prix = Prod("Prix trop bas", achat: 200, vente: 100);                    // D5
        var aberrant = Prod("Sirop aberrant", achat: 500, vente: 600000, type: "Sirop");   // D6
        var unite = Prod("Plaquettes", achat: 600, vente: 1000, nb: 3, unite: 333);   // D7
        var dateVide = Prod("Péremption vide", exp: DateTime.MinValue);              // D8
        var perime = Prod("Périmé en stock", exp: new DateTime(2024, 5, 1), stock: 7);   // D8 (information)
        var dup1 = Prod("Paracétamol 500 mg");                                       // D9
        var dup2 = Prod("PARACETAMOL 500mg");
        ctx.produits.AddRange(normal, neg, nb0, reception, cmd, prix, aberrant, unite, dateVide, perime, dup1, dup2,
            Prod("Sirop A", 500, 900, type: "Sirop"), Prod("Sirop B", 520, 950, type: "Sirop"), Prod("Sirop C", 480, 880, type: "Sirop"));
        ctx.SaveChanges();
        (ids.Normal, ids.Neg, ids.Nb0, ids.Reception, ids.Cmd, ids.Prix, ids.Aberrant, ids.Unite, ids.DateVide, ids.Perime, ids.Dup1, ids.Dup2) =
            (normal.Id, neg.Id, nb0.Id, reception.Id, cmd.Id, prix.Id, aberrant.Id, unite.Id, dateVide.Id, perime.Id, dup1.Id, dup2.Id);

        var annulee = new Commande { FournisseurId = four.Id, Statut = "Annulée" };       // D3
        annulee.Lignes.Add(new LigneCommande { ProduitId = cmd.Id, Quantite = 5, PrixAchatUnitaire = 100 });
        ctx.commandes.Add(annulee);

        // ── ventes et paiements ──
        int p = normal.Id;
        var v0 = NouvelleVente("V-000100", ModesPaiement.Comptant, 1000, u, p); v0.Paiements.Add(Pai("P-100", 1000, u));      // normale : ne déclenche rien
        var v1 = NouvelleVente("V-000101", ModesPaiement.Comptant, 1000, u, p); v1.Paiements.Add(Pai("P-101", 1500, u));      // D11 : sur-payée de 500
        var v2 = NouvelleVente("V-000102", ModesPaiement.Credit, 1000, u, p); v2.Paiements.Add(Pai("P-102", 1200, u));        // D12
        var v3a = NouvelleVente("V-000103", ModesPaiement.Mutuelle, 1000, u, p); v3a.Paiements.Add(Pai("P-103", 1000, u));    // D13 : sans mutuelle, part 0
        var v3b = NouvelleVente("V-000104", ModesPaiement.Mutuelle, 1000, u, p); v3b.MontantMutuelle = 400; v3b.Paiements.Add(Pai("P-104", 600, u));   // D13 : mutuelle supprimée
        var v4a = NouvelleVente("V-DUP", ModesPaiement.Comptant, 300, u, p); v4a.Paiements.Add(Pai("P-105", 300, u));         // D14
        var v4b = NouvelleVente("V-DUP", ModesPaiement.Comptant, 300, u, p); v4b.Paiements.Add(Pai("P-106", 300, u));
        var v5 = NouvelleVente("V-000106", ModesPaiement.Comptant, 200, u, p, date: Maintenant.AddDays(10)); v5.Paiements.Add(Pai("P-107", 200, u, date: Maintenant.AddDays(10)));   // D15
        var v6 = NouvelleVente("V-000107", ModesPaiement.Mutuelle, 1000, u, p); v6.MutuelId = m1.IdMutuel; v6.MontantMutuelle = 500; v6.Paiements.Add(Pai("P-108", 500, u));   // D16
        var v7 = NouvelleVente("V-000108", ModesPaiement.Mutuelle, 1000, u, p); v7.MutuelId = m1.IdMutuel; v7.MontantMutuelle = 500; v7.MutuelleReglee = true; v7.Paiements.Add(Pai("P-109", 500, u));   // D17
        var v8 = NouvelleVente("V-000109", ModesPaiement.Mutuelle, 1000, u, p); v8.MutuelId = m1.IdMutuel; v8.MontantMutuelle = 500; v8.Paiements.Add(Pai("P-110", 500, u));   // D18
        var v9 = NouvelleVente("V-000110", ModesPaiement.Mutuelle, 1000, u, p); v9.MutuelId = m1.IdMutuel; v9.MontantMutuelle = 500; v9.MutuelleReglee = true; v9.Paiements.Add(Pai("P-111", 500, u));   // D19
        var v10 = NouvelleVente("V-000111", ModesPaiement.Comptant, 500, u, p, ligne: false); v10.Paiements.Add(Pai("P-112", 500, u));   // D20 : aucune ligne
        var v11 = NouvelleVente("V-000112", ModesPaiement.Comptant, 1000, u, p); v11.Lignes[0].PrixUnitaire = 700; v11.Paiements.Add(Pai("P-113", 1000, u));   // D20 : lignes ≠ total
        var v12 = NouvelleVente("V-000113", ModesPaiement.Comptant, 400, null, p); v12.Paiements.Add(Pai("P-114", 400, null));    // D21 : sans vendeur
        var v13 = NouvelleVente("V-000114", ModesPaiement.Comptant, 800, u, p);                                                     // D10 : paiement de reprise
        ctx.ventes.AddRange(v0, v1, v2, v3a, v3b, v4a, v4b, v5, v6, v7, v8, v9, v10, v11, v12, v13);
        ctx.SaveChanges();
        v13.Paiements.Add(Pai("P-REPRISE-" + v13.IdVente, 800, u));
        ctx.SaveChanges();
        (ids.V1, ids.V2, ids.V3a, ids.V3b, ids.V4a, ids.V4b, ids.V5, ids.V6, ids.V7, ids.V8, ids.V9, ids.V10, ids.V11, ids.V12, ids.V13) =
            (v1.IdVente, v2.IdVente, v3a.IdVente, v3b.IdVente, v4a.IdVente, v4b.IdVente, v5.IdVente, v6.IdVente, v7.IdVente, v8.IdVente, v9.IdVente, v10.IdVente, v11.IdVente, v12.IdVente, v13.IdVente);

        // règlements des mutuelles
        ctx.MutuelPaiements.AddRange(
            new MutuelPaiement { NumeroPaiement = "MP-1", VenteId = v6.IdVente, MutuelId = m1.IdMutuel, Montant = 500, Reference = "", Commentaire = "" },   // D16
            new MutuelPaiement { NumeroPaiement = "MP-2", VenteId = v7.IdVente, MutuelId = m2.IdMutuel, Montant = 500, Reference = "", Commentaire = "" },   // D17 : c'est m2 qui a payé
            new MutuelPaiement { NumeroPaiement = "MP-3", VenteId = v8.IdVente, MutuelId = m1.IdMutuel, Montant = 200, Reference = "", Commentaire = "" }); // D18

        // ── caisse et dépenses ──
        var sHier = new SessionCaisse { UserId = u, DateOuverture = Maintenant.Date.AddDays(-2).AddHours(8), Statut = "Ouverte", CommentaireCloture = "" };     // D22
        var sAuj = new SessionCaisse { UserId = u, DateOuverture = Maintenant.Date.AddHours(8), Statut = "Ouverte", CommentaireCloture = "" };                   // légitime : aujourd'hui
        ctx.SessionsCaisse.AddRange(sHier, sAuj);
        for (int i = 0; i < 5; i++) ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Loyer", Montant = 1000, DateDepense = Maintenant.AddDays(-i - 1), UserId = u });
        ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Facture", Montant = 100000, DateDepense = Maintenant.AddDays(-1), UserId = u });   // D23
        ctx.SaveChanges();
        ids.SessionHier = sHier.Id; ids.SessionAujourdhui = sAuj.Id;
        return ids;
    }

    private static string Instantane()
    {
        var sb = new StringBuilder();
        using var c = TestDb.Ouvrir();
        foreach (var t in new[] { "produits", "ventes", "paiement", "MutuelPaiements", "SessionsCaisse", "commandes", "DepensesAnnexes" })
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT * FROM \"{t}\" ORDER BY 1";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var l = new List<string>();
                for (int i = 0; i < r.FieldCount; i++) l.Add(r.GetName(i) + "=" + (r.IsDBNull(i) ? "<null>" : Convert.ToString(r.GetValue(i))));
                l.Sort(StringComparer.Ordinal);
                sb.AppendLine(t + ":" + string.Join(";", l));
            }
        }
        return sb.ToString();
    }

    internal static RapportDiagnostic Lancer(bool appliquer)
    {
        using var ctx = new AppDbContext();
        return DiagnosticBaseService.Executer(ctx, appliquer, Maintenant);
    }

    private static List<Constat> Code(RapportDiagnostic r, string code) => r.Constats.Where(c => c.Code == code).ToList();

    private static void Exactement(RapportDiagnostic r, string code, GroupeConstat groupe, params string[] objets)
    {
        var l = Code(r, code);
        Assert.True(l.All(c => c.Groupe == groupe), $"{code} : groupe attendu {groupe}, trouvé {string.Join(",", l.Select(c => c.Groupe))}");
        Assert.Equal(objets.OrderBy(x => x), l.Select(c => c.Objet).OrderBy(x => x));
    }

    // ───────────────── lecture seule : constate tout, ne modifie RIEN ─────────────────

    [Fact]
    public void Lecture_seule_ne_modifie_rien_et_signale_aussi_les_cas_automatiques_comme_a_verifier()
    {
        var ids = Preparer();
        string avant = Instantane();

        var r = Lancer(appliquer: false);

        Assert.Equal(avant, Instantane());                                    // aucune écriture
        Assert.False(r.CorrectionsAppliquees);
        Assert.Equal(0, r.Nombre(GroupeConstat.Corrige));
        foreach (var code in new[] { "D1", "D4", "D11", "D16", "D17" })
            Assert.All(Code(r, code), c => Assert.Equal(GroupeConstat.AVerifier, c.Groupe));   // anomalie après import = nouveau bug, pas de correction silencieuse
        Assert.Single(Code(r, "D1"));
        Assert.Single(Code(r, "D11"));
        Assert.Single(Code(r, "D16"));
        Assert.Single(Code(r, "D17"));
    }

    // ───────────────── après import : chaque règle donne exactement le résultat attendu ─────────────────

    [Fact]
    public void Apres_import_chaque_regle_D1_a_D23_produit_exactement_le_resultat_attendu()
    {
        var ids = Preparer();
        var r = Lancer(appliquer: true);
        Assert.True(r.CorrectionsAppliquees);

        // STOCK
        Exactement(r, "D1", GroupeConstat.Corrige, "Stock négatif");
        Exactement(r, "D2", GroupeConstat.Information, "Ancienne réception");           // et non les produits marqués par D1/D4
        Exactement(r, "D3", GroupeConstat.Information, "Dans une commande annulée");
        Exactement(r, "D4", GroupeConstat.Corrige, "Conditionnement nul");
        // PRIX
        Exactement(r, "D5", GroupeConstat.AVerifier, "Prix trop bas");
        Exactement(r, "D6", GroupeConstat.AVerifier, "Sirop aberrant");
        Exactement(r, "D7", GroupeConstat.Information, "Plaquettes");
        Assert.Equal(new[] { GroupeConstat.AVerifier, GroupeConstat.Information }.OrderBy(x => x), Code(r, "D8").Select(c => c.Groupe).OrderBy(x => x));
        Assert.Equal("Péremption vide", Code(r, "D8").Single(c => c.Groupe == GroupeConstat.AVerifier).Objet);
        Assert.Equal("Périmé en stock", Code(r, "D8").Single(c => c.Groupe == GroupeConstat.Information).Objet);
        Assert.Single(Code(r, "D9"));
        Assert.Contains("Paracétamol 500 mg", Code(r, "D9")[0].Objet);
        // VENTES ET PAIEMENTS
        Exactement(r, "D10", GroupeConstat.Information, "V-000114");
        Exactement(r, "D11", GroupeConstat.Corrige, "V-000101");
        Exactement(r, "D12", GroupeConstat.AVerifier, "V-000102");
        Exactement(r, "D13", GroupeConstat.AVerifier, "V-000103", "V-000104");
        Assert.Contains("totalité apparaît comme dette", Code(r, "D13").Single(c => c.Objet == "V-000103").Detail);
        Assert.Contains("mutuelle supprimée", Code(r, "D13").Single(c => c.Objet == "V-000104").Detail);
        Exactement(r, "D14", GroupeConstat.AVerifier, "V-DUP");
        Exactement(r, "D15", GroupeConstat.AVerifier, "V-000106");
        // MUTUELLES
        Exactement(r, "D16", GroupeConstat.Corrige, "V-000107");
        Exactement(r, "D17", GroupeConstat.Corrige, "V-000108");
        Exactement(r, "D18", GroupeConstat.AVerifier, "V-000109");
        Assert.Contains("reste", Code(r, "D18")[0].Detail);
        Exactement(r, "D19", GroupeConstat.AVerifier, "V-000110");
        // HISTORIQUE INCOMPLET
        Exactement(r, "D20", GroupeConstat.AVerifier, "V-000111", "V-000112");
        Assert.Single(Code(r, "D21"));
        Assert.Contains("1 vente(s) et 1 paiement(s)", Code(r, "D21")[0].Detail);
        // CAISSE ET DÉPENSES
        Exactement(r, "D22", GroupeConstat.AVerifier, $"Session n°{ids.SessionHier}");   // pas la session d'aujourd'hui
        Assert.Equal(new[] { ids.SessionHier }, r.SessionsOuvertes);
        Assert.Single(Code(r, "D23"));
        Assert.Contains("100", Code(r, "D23")[0].Detail);

        // ── les corrections automatiques (et elles seules) ont été appliquées ──
        using var ctx = new AppDbContext();
        var neg = ctx.produits.Single(x => x.Id == ids.Neg);
        Assert.Equal(0, neg.QuantiteEnStock);
        Assert.True(neg.StockAVerifier);
        Assert.Contains("stock négatif (−4) dans l'ancienne base", neg.MotifVerification);
        var nb0 = ctx.produits.Single(x => x.Id == ids.Nb0);
        Assert.Equal(1, nb0.NbUniteParBoite);
        Assert.True(nb0.StockAVerifier);
        Assert.Equal(5, nb0.QuantiteEnStock);                                 // le stock n'est pas touché

        var ajust = ctx.paiement.Single(x => x.NumeroPaiement == "P-AJUST-" + ids.V1);
        Assert.Equal(-500m, ajust.Montant);
        Assert.Equal(ModesPaiement.Especes, ajust.Mode);
        Assert.Equal(ctx.ventes.Single(x => x.IdVente == ids.V1).DateVente, ajust.DatePaiement);
        Assert.Equal(1000m, ctx.paiement.Where(x => x.VenteId == ids.V1).ToList().Sum(x => x.Montant));

        Assert.True(ctx.ventes.Single(x => x.IdVente == ids.V6).MutuelleReglee);                   // D16
        Assert.Equal(ids.M2, ctx.ventes.Single(x => x.IdVente == ids.V7).MutuelId);                 // D17
        Assert.False(ctx.ventes.Single(x => x.IdVente == ids.V8).MutuelleReglee);                   // D18 : rien modifié
        Assert.Equal(ids.M1, ctx.ventes.Single(x => x.IdVente == ids.V9).MutuelId);                 // D19 : rien modifié
        Assert.Equal(1200m, ctx.paiement.Where(x => x.VenteId == ids.V2).ToList().Sum(x => x.Montant));              // D12 : crédit sur-payé inchangé
        Assert.Equal(1, ctx.paiement.Count(x => x.NumeroPaiement.StartsWith("P-AJUST-")));                   // un seul ajustement (D11)
    }

    // ───────────────── idempotence : un deuxième import du même fichier donne le même résultat ─────────────────

    [Fact]
    public void Un_deuxieme_passage_ne_change_plus_rien_et_ne_corrige_plus_rien()
    {
        Preparer();
        Lancer(appliquer: true);
        string apresPremier = Instantane();

        var second = Lancer(appliquer: true);

        Assert.Equal(apresPremier, Instantane());
        Assert.Equal(0, second.Nombre(GroupeConstat.Corrige));
        Assert.Empty(Code(second, "D1"));
        Assert.Empty(Code(second, "D4"));
        Assert.Empty(Code(second, "D11"));
        Assert.Empty(Code(second, "D16"));
        Assert.Empty(Code(second, "D17"));
        Assert.Equal(1, TestDb.Scalaire("SELECT COUNT(*) FROM paiement WHERE NumeroPaiement LIKE 'P-AJUST-%'"));
        // les produits corrigés par D1 / D4 restent hors de la liste D2 (ils ont leur propre motif)
        Assert.Equal(new[] { "Ancienne réception" }, Code(second, "D2").Select(c => c.Objet).ToArray());
    }

    // ───────────────── D22 : clôture des sessions oubliées ─────────────────

    [Fact]
    public void Cloturer_les_sessions_oubliees_fige_le_montant_attendu_et_ne_touche_pas_la_session_du_jour()
    {
        var ids = Preparer();
        int n;
        using (var ctx = new AppDbContext())
            n = DiagnosticBaseService.CloturerSessions(ctx, new[] { ids.SessionHier }, Maintenant);
        Assert.Equal(1, n);

        using (var ctx = new AppDbContext())
        {
            var s = ctx.SessionsCaisse.Single(x => x.Id == ids.SessionHier);
            Assert.Equal("Clôturée", s.Statut);
            Assert.Equal("Clôturée lors de l'import", s.CommentaireCloture);
            Assert.NotNull(s.MontantAttenduCloture);
            Assert.Equal(CaisseService.EspecesAttendues(ctx, s), s.MontantAttenduCloture);
            Assert.Equal(new DateTime(2026, 10, 11, 23, 59, 59), s.DateCloture);        // fin de la journée d'ouverture
            Assert.Equal("Ouverte", ctx.SessionsCaisse.Single(x => x.Id == ids.SessionAujourdhui).Statut);
        }
        using (var ctx = new AppDbContext())
            Assert.Equal(0, DiagnosticBaseService.CloturerSessions(ctx, new[] { ids.SessionHier }, Maintenant));   // idempotent
        Assert.Empty(Code(Lancer(false), "D22"));
    }

    // ───────────────── export Excel : un onglet par groupe, aucun nom de client ─────────────────

    [Fact]
    public void Export_Excel_a_un_onglet_par_groupe_et_aucun_nom_de_client()
    {
        Preparer();
        var r = Lancer(appliquer: true);
        string chemin = Path.Combine(Path.GetTempPath(), "diag_" + Guid.NewGuid().ToString("N") + ".xlsx");
        try
        {
            DiagnosticBaseService.ExporterExcel(r, chemin);
            using var wb = new XLWorkbook(chemin);
            Assert.Equal(new[] { "Corrigés automatiquement", "À vérifier", "Pour information" }, wb.Worksheets.Select(w => w.Name).ToArray());
            var toutes = string.Join("\n", wb.Worksheets.SelectMany(w => w.CellsUsed().Select(c => c.GetString())));
            Assert.DoesNotContain("Zéphyrine", toutes);
            Assert.DoesNotContain("Rakotomalala", toutes);
            Assert.Contains("V-000101", toutes);              // numéros de vente
            Assert.Contains("Stock négatif", toutes);         // noms de produits
            Assert.Equal(r.Nombre(GroupeConstat.Corrige) + 1, wb.Worksheet("Corrigés automatiquement").RowsUsed().Count());   // en-tête + constats
        }
        finally { File.Delete(chemin); }
    }

    [Fact]
    public void Le_dernier_diagnostic_est_inclus_dans_le_ZIP_du_developpeur()
    {
        Preparer();
        DiagnosticBaseService.Enregistrer(Lancer(appliquer: false));
        try
        {
            string zip = RapportDeveloppeurService.Preparer(Path.Combine(Path.GetTempPath(), "p2_zip_" + Guid.NewGuid().ToString("N")), null, Maintenant);
            using var archive = ZipFile.OpenRead(zip);
            Assert.Contains(archive.Entries, e => e.FullName == "diagnostic_base.xlsx");
            Assert.NotNull(DiagnosticBaseService.Dernier());
        }
        finally { File.Delete(Path.Combine(CheminsApp.DossierDonnees, "dernier_diagnostic.json")); }
    }

    // ───────────────── écran ─────────────────

    [Fact]
    public void Ecran_trois_groupes_bouton_Cloturer_ces_sessions_et_export()
    {
        var ids = Preparer();
        var rapport = Lancer(appliquer: true);
        var cts = UiHelper.FermerLesMessages(true);
        string chemin = Path.Combine(Path.GetTempPath(), "diag_ecran_" + Guid.NewGuid().ToString("N") + ".xlsx");
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormDiagnostic(rapport, apresImport: true);
                Assert.Equal(rapport.Nombre(GroupeConstat.Corrige), f.Grille(GroupeConstat.Corrige).Rows.Count);
                Assert.Equal(rapport.Nombre(GroupeConstat.AVerifier), f.Grille(GroupeConstat.AVerifier).Rows.Count);
                Assert.Equal(rapport.Nombre(GroupeConstat.Information), f.Grille(GroupeConstat.Information).Rows.Count);
                Assert.True(f.BoutonSessionsVisible);

                f.Exporter(chemin);
                Assert.True(File.Exists(chemin));

                UiHelper.Appeler(f, "CloturerSessions");
                Assert.False(f.BoutonSessionsVisible);
                Assert.Contains(f.Grille(GroupeConstat.Corrige).Rows.Cast<DataGridViewRow>(), row => Convert.ToString(row.Cells["Code"].Value) == "D22");
            });
        }
        finally { cts.Cancel(); File.Delete(chemin); File.Delete(Path.Combine(CheminsApp.DossierDonnees, "dernier_diagnostic.json")); }
        using var ctx = new AppDbContext();
        Assert.Equal("Clôturée", ctx.SessionsCaisse.Single(s => s.Id == ids.SessionHier).Statut);
    }
}
