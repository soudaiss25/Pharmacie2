using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Pharmacie2.views.UserControls;
using Xunit;

/// <summary>
/// Remboursements de commandes : une dépense « Remboursement commande » est une note reliée à sa commande ; elle réduit le reste à payer,
/// n'entre jamais dans le bénéfice et ne touche jamais à une caisse.
/// </summary>
public class RemboursementCommandeTests
{
    private sealed class Ids { public int Admin, Fournisseur, Produit, CommandeAPayer, CommandePayee, Session; }

    private static Ids Preparer()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var ids = new Ids();
        ids.Produit = TestDb.NouveauProduit("Doliprane", 100, 1, "Boîte", prixVente: 1000m);
        using var ctx = new AppDbContext();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        var four = new Fournisseur { Nom = "Grossiste", Contact = "" };
        ctx.Users.Add(admin);
        ctx.fournisseur.Add(four);
        ctx.SaveChanges();
        // 10 boîtes à 1 000 = 10 000, à rembourser plus tard
        var aPayer = new Commande { FournisseurId = four.Id, DateCommande = DateTime.Now.AddDays(-3), Statut = "Reçu", PayeeALaCommande = false };
        aPayer.Lignes.Add(new LigneCommande { ProduitId = ids.Produit, Quantite = 10, PrixAchatUnitaire = 1000m });
        // 5 boîtes à 400 = 2 000, payée à la commande
        var payee = new Commande { FournisseurId = four.Id, DateCommande = DateTime.Now.AddDays(-2), Statut = "Reçu", PayeeALaCommande = true };
        payee.Lignes.Add(new LigneCommande { ProduitId = ids.Produit, Quantite = 5, PrixAchatUnitaire = 400m });
        ctx.commandes.AddRange(aPayer, payee);
        ctx.SaveChanges();
        ids.Admin = admin.Id; ids.Fournisseur = four.Id; ids.CommandeAPayer = aPayer.Id; ids.CommandePayee = payee.Id;
        SessionUtilisateur.Courant = admin;
        UiHelper.ViderTitres();
        return ids;
    }

    private static DetteCommande Etat(int commandeId)
    {
        using var ctx = new AppDbContext();
        return RemboursementCommandeService.Commandes(ctx).Single(c => c.CommandeId == commandeId);
    }

    private static int Remboursements() => (int)TestDb.Scalaire("SELECT COUNT(*) FROM DepensesAnnexes WHERE Categorie = 'Remboursement commande'");

    /// <summary>Saisit un remboursement par l'écran (catégorie et commande présélectionnées), comme le bouton « Rembourser ».</summary>
    private static void Rembourser(int? commandeId, decimal montant)
    {
        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var f = new FormAddDepense(commandeId);
                if (commandeId == null) UiHelper.Champ<ComboBox>(f, "cbCategorie").SelectedItem = RemboursementCommandeService.Categorie;
                UiHelper.Champ<NumericUpDown>(f, "numMontant").Value = montant;
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); }
    }

    // ───────── reste à payer ─────────

    [Fact]
    public void Remboursement_en_trois_fois_jusqu_au_solde()
    {
        var ids = Preparer();
        var depart = Etat(ids.CommandeAPayer);
        Assert.Equal((10000m, 10000m, "Non payée"), (depart.Total, depart.Reste, depart.Etat));

        Rembourser(ids.CommandeAPayer, 3000);
        var un = Etat(ids.CommandeAPayer);
        Assert.Equal((7000m, "Partiellement payée"), (un.Reste, un.Etat));

        Rembourser(ids.CommandeAPayer, 4000);
        var deux = Etat(ids.CommandeAPayer);
        Assert.Equal((3000m, "Partiellement payée"), (deux.Reste, deux.Etat));

        Rembourser(ids.CommandeAPayer, 3000);
        var trois = Etat(ids.CommandeAPayer);
        Assert.Equal((0m, "Payée"), (trois.Reste, trois.Etat));
        Assert.Equal(3, Remboursements());
        Assert.Equal(3, TestDb.Scalaire($"SELECT COUNT(*) FROM DepensesAnnexes WHERE CommandeId = {ids.CommandeAPayer}"));

        using var ctx = new AppDbContext();
        Assert.DoesNotContain(RemboursementCommandeService.NonSoldees(ctx), c => c.CommandeId == ids.CommandeAPayer);
        Assert.Equal(0m, RemboursementCommandeService.DettesFournisseurs(ctx));
    }

    [Fact]
    public void Un_montant_au_dela_du_reste_a_payer_est_refuse_et_la_commande_est_obligatoire()
    {
        var ids = Preparer();
        Rembourser(ids.CommandeAPayer, 3000);
        Rembourser(ids.CommandeAPayer, 7001);                 // reste 7 000 : refusé
        Assert.True(UiHelper.TitreVu("Validation"));
        Assert.Equal(1, Remboursements());
        Assert.Equal(7000m, Etat(ids.CommandeAPayer).Reste);

        Rembourser(null, 1000);                                // aucune commande choisie : refusé
        Assert.Equal(1, Remboursements());

        Rembourser(ids.CommandePayee, 500);                    // payée à la commande : n'est même pas proposée, refusé
        Assert.Equal(1, Remboursements());

        using var ctx = new AppDbContext();
        Assert.Equal("Le montant dépasse le reste à payer de cette commande (" + Format.Montant(7000m) + ").",
            RemboursementCommandeService.ControlerRemboursement(ctx, ids.CommandeAPayer, 8000));
        Assert.Null(RemboursementCommandeService.ControlerRemboursement(ctx, ids.CommandeAPayer, 7000));
    }

    [Fact]
    public void Une_commande_payee_a_la_commande_n_a_aucun_reste()
    {
        var ids = Preparer();
        var payee = Etat(ids.CommandePayee);
        Assert.Equal((2000m, 0m, "Payée"), (payee.Total, payee.Reste, payee.Etat));
        using var ctx = new AppDbContext();
        var nonSoldees = RemboursementCommandeService.NonSoldees(ctx);
        Assert.Equal(new[] { ids.CommandeAPayer }, nonSoldees.Select(c => c.CommandeId).ToArray());
    }

    [Fact]
    public void Supprimer_une_depense_de_remboursement_recalcule_le_reste_a_payer()
    {
        var ids = Preparer();
        Rembourser(ids.CommandeAPayer, 4000);
        Assert.Equal(6000m, Etat(ids.CommandeAPayer).Reste);
        using (var ctx = new AppDbContext())
        {
            ctx.DepensesAnnexes.RemoveRange(ctx.DepensesAnnexes.Where(d => d.CommandeId == ids.CommandeAPayer));
            ctx.SaveChanges();
        }
        var apres = Etat(ids.CommandeAPayer);
        Assert.Equal((10000m, "Non payée"), (apres.Reste, apres.Etat));
    }

    // ───────── bénéfice ─────────

    [Fact]
    public void Le_benefice_ne_change_pas_apres_un_remboursement_de_commande()
    {
        var ids = Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Loyer", Description = "", Montant = 5000, DateDepense = DateTime.Today, UserId = ids.Admin });
            ctx.SaveChanges();
        }
        DateTime debut = DateTime.Today.AddDays(-10), fin = DateTime.Now.AddDays(1);
        decimal avant, apres, statsAvant, statsApres;
        using (var ctx = new AppDbContext()) { avant = TableauDeBordService.Benefice(ctx, debut, fin); }
        statsAvant = StatistiquesService.Calculer(debut, fin).Depenses;

        Rembourser(ids.CommandeAPayer, 6000);

        using (var ctx = new AppDbContext()) { apres = TableauDeBordService.Benefice(ctx, debut, fin); }
        statsApres = StatistiquesService.Calculer(debut, fin).Depenses;
        Assert.Equal(avant, apres);
        Assert.Equal(5000m, statsAvant);            // seul le loyer compte
        Assert.Equal(statsAvant, statsApres);
        Assert.Equal(1, Remboursements());
    }

    [Fact]
    public void L_onglet_Depenses_grise_les_remboursements_et_les_sort_du_total()
    {
        var ids = Preparer();
        using (var ctx = new AppDbContext())
        {
            ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Loyer", Description = "Octobre", Montant = 5000, DateDepense = DateTime.Today, UserId = ids.Admin });
            ctx.SaveChanges();
        }
        Rembourser(ids.CommandeAPayer, 6000);
        var cts = UiHelper.FermerLesMessages(true);
        try { UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var uc = new Uc_Depenses { Dock = DockStyle.Fill };
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();

            Assert.Contains(Format.Montant(5000m), UiHelper.Champ<Label>(uc, "lblTotalHistorique").Text);      // le total ne contient pas le remboursement
            Assert.DoesNotContain(Format.Montant(11000m), UiHelper.Champ<Label>(uc, "lblTotalHistorique").Text);
            string note = UiHelper.Champ<Label>(uc, "lblRemboursements").Text;
            Assert.Contains("Remboursements de commandes (pour information)", note);
            Assert.Contains(Format.Montant(6000m), note);

            var g = UiHelper.Champ<DataGridView>(uc, "dgvHistorique");
            var ligne = g.Rows.Cast<DataGridViewRow>().Single(r => Convert.ToString(r.Cells["Categorie"].Value) == RemboursementCommandeService.Categorie);
            Assert.Contains("Note : n'entre pas dans le bénéfice", Convert.ToString(ligne.Cells["Description"].Value));
            uc.Dispose();
        }); }
        finally { cts.Cancel(); }
        Assert.False(UiHelper.TitreVu("Erreur"));
    }

    // ───────── aucun lien avec la caisse ─────────

    [Fact]
    public void Une_depense_enregistree_pendant_qu_une_caisse_est_ouverte_ne_change_pas_ses_especes_attendues()
    {
        var ids = Preparer();
        int session;
        using (var ctx = new AppDbContext())
        {
            var s = new SessionCaisse { UserId = ids.Admin, DateOuverture = DateTime.Now.AddHours(-1), Statut = "Ouverte", CommentaireCloture = "" };
            ctx.SessionsCaisse.Add(s);
            var v = new Vente
            {
                numeroVente = "V-000001", Statut = "Active", NomClient = "", PrenomClient = "", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
                MoyenPaiement = ModesPaiement.Comptant, Type = ModesPaiement.Comptant, MotifAnnulation = "", MontantTotal = 5000, UserId = ids.Admin, DateVente = DateTime.Now.AddMinutes(-30)
            };
            ctx.ventes.Add(v);
            ctx.SaveChanges();
            ctx.paiement.Add(new Paiement { NumeroPaiement = "P-1", VenteId = v.IdVente, Montant = 5000, Mode = ModesPaiement.Especes, UserId = ids.Admin, SessionCaisseId = s.Id, DatePaiement = DateTime.Now.AddMinutes(-29) });
            ctx.SaveChanges();
            session = s.Id;
        }
        decimal Attendu() { using var c = new AppDbContext(); return CaisseService.EspecesAttendues(c, c.SessionsCaisse.Single(x => x.Id == session)); }
        Assert.Equal(5000m, Attendu());

        // dépense ordinaire puis remboursement : aucune question sur le tiroir, aucun lien avec la session
        using (var ctx = new AppDbContext())
        {
            ctx.DepensesAnnexes.Add(new DepenseAnnexe { Categorie = "Fournitures", Description = "", Montant = 1500, DateDepense = DateTime.Today, UserId = ids.Admin });
            ctx.SaveChanges();
        }
        Rembourser(ids.CommandeAPayer, 2500);
        Assert.False(UiHelper.TitreVu("Caisse"));
        Assert.Equal(5000m, Attendu());
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM pragma_table_info('DepensesAnnexes') WHERE name LIKE '%Session%'"));   // aucune colonne de lien avec une session
    }

    // ───────── annulation ─────────

    [Fact]
    public void Une_commande_remboursee_en_partie_ne_peut_plus_etre_annulee()
    {
        var ids = Preparer();
        Rembourser(ids.CommandeAPayer, 1000);
        UiHelper.ViderTitres();

        var cts = UiHelper.FermerLesMessages(true);
        try
        {
            UiHelper.EnSta(() =>
            {
                using var hote = new Form();
                var uc = new Uc_Commande { Dock = DockStyle.Fill };
                hote.Controls.Add(uc);
                hote.Show();
                Application.DoEvents();
                var g = UiHelper.Champ<DataGridView>(uc, "dgvCommandes");
                foreach (DataGridViewRow r in g.Rows) r.Selected = Convert.ToInt32(r.Cells["colCmdId"].Value) == ids.CommandeAPayer;
                g.CurrentCell = g.Rows.Cast<DataGridViewRow>().First(r => Convert.ToInt32(r.Cells["colCmdId"].Value) == ids.CommandeAPayer).Cells["colStatut"];
                UiHelper.Appeler(uc, "btnAnnuler_Click", null, EventArgs.Empty);
                uc.Dispose();
            });
        }
        finally { cts.Cancel(); }

        Assert.True(UiHelper.TitreVu("Annulation impossible"));
        Assert.Equal("Reçu", TestDb.Texte($"SELECT Statut FROM commandes WHERE Id = {ids.CommandeAPayer}"));
        using var ctx = new AppDbContext();
        Assert.NotNull(RemboursementCommandeService.RefusAnnulation(ctx, ids.CommandeAPayer));
        Assert.Null(RemboursementCommandeService.RefusAnnulation(ctx, ids.CommandePayee));          // sans remboursement, l'annulation reste possible
    }

    // ───────── affichage ─────────

    [Fact]
    public void Ma_journee_affiche_les_dettes_fournisseurs_seulement_si_elles_existent()
    {
        var ids = Preparer();
        var ligne = TableauDeBordService.AFaireMaintenant(DateTime.Now).Single(a => a.Type == TypeAFaire.DettesFournisseurs);
        Assert.Equal("Dettes fournisseurs : " + Format.Montant(10000m), ligne.Texte);
        Assert.Equal("info", ligne.Niveau);

        Rembourser(ids.CommandeAPayer, 10000);
        Assert.DoesNotContain(TableauDeBordService.AFaireMaintenant(DateTime.Now), a => a.Type == TypeAFaire.DettesFournisseurs);
    }

    [Fact]
    public void Les_commandes_montrent_le_reste_a_payer_et_le_bouton_Rembourser_depuis_une_commande_non_soldee()
    {
        var ids = Preparer();
        Rembourser(ids.CommandeAPayer, 2500);
        UiHelper.EnSta(() =>
        {
            using var hote = new Form();
            var uc = new Uc_Commande { Dock = DockStyle.Fill };
            hote.Controls.Add(uc);
            hote.Show();
            Application.DoEvents();
            var g = UiHelper.Champ<DataGridView>(uc, "dgvCommandes");
            var l = g.Rows.Cast<DataGridViewRow>().Single(r => Convert.ToInt32(r.Cells["colCmdId"].Value) == ids.CommandeAPayer);
            Assert.Equal("Partiellement payée", Convert.ToString(l.Cells["colEtatPaiement"].Value));
            Assert.Equal(7500m, Convert.ToDecimal(l.Cells["colReste"].Value));
            var p = g.Rows.Cast<DataGridViewRow>().Single(r => Convert.ToInt32(r.Cells["colCmdId"].Value) == ids.CommandePayee);
            Assert.Equal("Payée", Convert.ToString(p.Cells["colEtatPaiement"].Value));

            var rembourser = UiHelper.Champ<Button>(uc, "btnRembourser");
            g.ClearSelection(); l.Selected = true; g.CurrentCell = l.Cells["colStatut"];
            Assert.True(rembourser.Enabled);
            g.ClearSelection(); p.Selected = true; g.CurrentCell = p.Cells["colStatut"];
            Assert.False(rembourser.Enabled);                                                  // rien à rembourser sur une commande payée

            // filtre « À rembourser » (lien depuis Ma journée)
            uc.FiltrerARembourser();
            Application.DoEvents();
            Assert.Equal(new[] { ids.CommandeAPayer }, g.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToInt32(r.Cells["colCmdId"].Value)).ToArray());
            uc.Dispose();
        });
    }

    [Fact]
    public void Le_formulaire_Rembourser_presélectionne_la_commande_et_affiche_le_reste()
    {
        var ids = Preparer();
        Rembourser(ids.CommandeAPayer, 4000);
        UiHelper.EnSta(() =>
        {
            using var f = new FormAddDepense(ids.CommandeAPayer);
            Assert.Equal(RemboursementCommandeService.Categorie, UiHelper.Champ<ComboBox>(f, "cbCategorie").Text);
            var choix = UiHelper.Champ<ComboBox>(f, "cbCommande");
            Assert.Single(choix.Items);                                                        // seule la commande non soldée est proposée
            Assert.NotNull(choix.SelectedItem);
            Assert.Contains("Grossiste", Convert.ToString(choix.SelectedItem));
            Assert.Contains(Format.Montant(6000m), Convert.ToString(choix.SelectedItem));
            Assert.Contains("Reste à payer sur cette commande : " + Format.Montant(6000m), UiHelper.Champ<Label>(f, "lblResteCommande").Text);
        });
    }

    [Fact]
    public void La_case_Payee_a_la_commande_est_decochee_par_defaut_et_enregistree()
    {
        var ids = Preparer();
        UiHelper.EnSta(() =>
        {
            using var f = new FormCommandeProduit(ids.Produit, "Doliprane");
            var coche = UiHelper.Champ<CheckBox>(f, "chkPayee");
            Assert.False(coche.Checked);
            UiHelper.Champ<TextBox>(f, "txtFournisseur").Text = "Grossiste";
            UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);
        });
        using var ctx = new AppDbContext();
        var nouvelle = ctx.commandes.OrderByDescending(c => c.Id).First();
        Assert.False(nouvelle.PayeeALaCommande);
        Assert.NotEqual(ids.CommandePayee, nouvelle.Id);
    }

    // ───────── reprise ─────────

    [Fact]
    public void Migration_AjoutRemboursementCommande_marque_les_commandes_existantes_payees_sans_toucher_aux_depenses()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutMatriculeObligatoire"));
        Dictionary<string, long> avant;
        using (var c = TestDb.Ouvrir())
        {
            TestDb.Inserer(c, "Users", new() { ["Id"] = 1L, ["Nom"] = "A", ["Login"] = "admin", ["MotDePasse"] = "x", ["Role"] = "Administrateur" });
            TestDb.Inserer(c, "fournisseur", new() { ["Id"] = 1L, ["Nom"] = "F", ["Contact"] = "", ["Actif"] = 1L });
            TestDb.Inserer(c, "commandes", new() { ["Id"] = 1L, ["FournisseurId"] = 1L, ["Statut"] = "Reçu", ["NoteCommande"] = "", ["DateCommande"] = "2026-09-01 10:00:00" });
            TestDb.Inserer(c, "commandes", new() { ["Id"] = 2L, ["FournisseurId"] = 1L, ["Statut"] = "En attente", ["NoteCommande"] = "", ["DateCommande"] = "2026-10-01 10:00:00" });
            TestDb.Inserer(c, "DepensesAnnexes", new() { ["Id"] = 1L, ["Categorie"] = "Loyer", ["Description"] = "", ["Montant"] = "5000", ["DateDepense"] = "2026-10-01 00:00:00", ["UserId"] = 1L });
            avant = TestDb.Comptes(c, TestDb.TablesMetier);
        }
        TestDb.MigrerJusqua(TestDb.MigrationId("AjoutRemboursementCommande"));
        using var c2 = TestDb.Ouvrir();
        Assert.All(avant, kv => Assert.Equal(kv.Value, TestDb.Comptes(c2, kv.Key)[kv.Key]));
        Assert.Equal(2, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM commandes WHERE PayeeALaCommande = 1"));         // aucune fausse dette
        Assert.Equal(1, TestDb.Scalaire(c2, "SELECT COUNT(*) FROM DepensesAnnexes WHERE CommandeId IS NULL"));
        using var ctx = new AppDbContext();
        Assert.Equal(0m, RemboursementCommandeService.DettesFournisseurs(ctx));
    }
}
