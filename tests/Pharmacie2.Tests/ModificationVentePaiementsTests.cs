using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

/// <summary>R4 : la modification d'une vente ajuste les paiements (complément ou remboursement) dans la même transaction.</summary>
public class ModificationVentePaiementsTests
{
    private static int _evenements;

    /// <summary>Vente de 10 articles à 100 KMF (total 1 000), déjà versée selon les paramètres.</summary>
    private static (int vente, int session) Preparer(string type, decimal verse, string modeVerse = ModesPaiement.Especes, decimal mutuelleTaux = 0)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        int pid = TestDb.NouveauProduit("Doliprane", 1000, 1, "Boîte", 100m);
        using var ctx = new AppDbContext();
        ctx.Users.Add(admin);
        Mutuel? mut = null;
        if (mutuelleTaux > 0)
        {
            mut = new Mutuel { NomEmployeur = "Entreprise X", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = mutuelleTaux };
            ctx.mutuels.Add(mut);
        }
        ctx.SaveChanges();
        var s = new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" };
        ctx.SessionsCaisse.Add(s);

        var v = new Vente
        {
            numeroVente = "V-000001", Statut = "Active", NomClient = "N", PrenomClient = "P", TelephoneClient = "", MotifAchat = "", MatriculeEmploye = "N/A",
            MoyenPaiement = type, Type = type, MotifAnnulation = "", MontantTotal = 1000, UserId = admin.Id, DateVente = DateTime.Now.AddSeconds(-30)
        };
        if (mut != null)
        {
            v.MutuelId = mut.IdMutuel; v.TauxMutuelle = mutuelleTaux; v.MontantMutuelle = VenteModificationRegles.PartMutuelle(1000, mutuelleTaux); v.MatriculeEmploye = "M1";
        }
        v.Lignes.Add(new LigneVente { ProduitId = pid, Quantite = 10, QuantiteUnites = 10, PrixUnitaire = 100, UniteVendue = "Boîte" });
        if (verse > 0)
            v.Paiements.Add(new Paiement { NumeroPaiement = "P-ORIG", Montant = verse, Mode = modeVerse, UserId = admin.Id, DatePaiement = DateTime.Now.AddSeconds(-30) });
        ctx.ventes.Add(v);
        ctx.SaveChanges();
        SessionUtilisateur.Courant = admin;
        return (v.IdVente, s.Id);
    }

    /// <summary>Ouvre l'écran, change la quantité (prix 100 l'unité), éventuellement le mode, et enregistre en répondant « Oui » aux messages.</summary>
    private static void Modifier(int venteId, int quantite, string? nouveauType = null, string? modeComplement = null)
    {
        var cts = UiHelper.FermerLesMessages(true);
        EventHandler h = (s, e) => _evenements++;
        VenteEvenements.VenteModifiee += h;
        try
        {
            UiHelper.EnSta(() =>
            {
                var f = new FormModificationVente(venteId);
                var lignes = (IList)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
                lignes[0].GetType().GetProperty("Quantite").SetValue(lignes[0], quantite);
                if (nouveauType != null) UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = nouveauType;
                UiHelper.Appeler(f, "CalculerTotal");
                if (modeComplement != null) UiHelper.Champ<ComboBox>(f, "cbModeComplement").SelectedItem = modeComplement;
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            });
        }
        finally { cts.Cancel(); VenteEvenements.VenteModifiee -= h; }
    }

    private static (Vente vente, List<Paiement> paiements) Etat(int venteId)
    {
        using var ctx = new AppDbContext();
        var v = ctx.ventes.Include(x => x.Paiements).Single(x => x.IdVente == venteId);
        return (v, v.Paiements.OrderBy(p => p.Id).ToList());
    }

    private static decimal Attendu(int sessionId)
    {
        using var ctx = new AppDbContext();
        return CaisseService.EspecesAttendues(ctx, ctx.SessionsCaisse.Single(s => s.Id == sessionId));
    }

    [Fact]
    public void Comptant_augmente_le_client_paie_le_complement_en_especes_par_defaut()
    {
        var (vid, sid) = Preparer(ModesPaiement.Comptant, 1000);
        _evenements = 0;
        Modifier(vid, 30);                                    // total 3 000
        var (v, ps) = Etat(vid);
        Assert.Equal(3000m, v.MontantTotal);
        Assert.Equal(2, ps.Count);
        Assert.Equal((2000m, ModesPaiement.Especes), (ps[1].Montant, ps[1].Mode));
        Assert.StartsWith("P-", ps[1].NumeroPaiement);
        Assert.Equal(0m, v.MontantRestant);
        Assert.Equal(3000m, Attendu(sid));
        Assert.True(UiHelper.TitreVu("Complément à encaisser"));
        Assert.True(_evenements >= 1);                         // VenteModifiee déclenché
    }

    [Fact]
    public void Comptant_diminue_on_rend_X_au_client_par_un_paiement_negatif_R()
    {
        var (vid, sid) = Preparer(ModesPaiement.Comptant, 1000);
        _evenements = 0;
        Modifier(vid, 4);                                     // total 400 : rendre 600
        var (v, ps) = Etat(vid);
        Assert.Equal(2, ps.Count);
        Assert.Equal(-600m, ps[1].Montant);
        Assert.StartsWith("R-", ps[1].NumeroPaiement);
        Assert.Equal(ModesPaiement.Especes, ps[1].Mode);      // même mode que le paiement d'origine
        Assert.Equal(400m, v.MontantVerse);
        Assert.Equal(0m, v.MontantRestant);
        Assert.Equal(400m, Attendu(sid));                      // le remboursement en espèces sort du tiroir
        Assert.True(UiHelper.TitreVu("Rendre de l'argent au client"));
        Assert.True(_evenements >= 1);
    }

    [Fact]
    public void Carte_augmente_le_complement_est_en_especes_par_defaut_ou_dans_le_mode_choisi()
    {
        var (vid, sid) = Preparer(ModesPaiement.CarteBancaire, 1000, ModesPaiement.CarteBancaire);
        Modifier(vid, 15);                                    // +500
        var (_, ps) = Etat(vid);
        Assert.Equal((500m, ModesPaiement.Especes), (ps[1].Montant, ps[1].Mode));
        Assert.Equal(500m, Attendu(sid));

        Modifier(vid, 20, modeComplement: ModesPaiement.Mvola);   // +500 de plus, par Mvola
        var (v, ps2) = Etat(vid);
        Assert.Equal((500m, ModesPaiement.Mvola), (ps2[2].Montant, ps2[2].Mode));
        Assert.Equal(500m, Attendu(sid));                      // Mvola : hors tiroir
        Assert.Equal(0m, v.MontantRestant);
    }

    [Fact]
    public void Carte_diminue_le_remboursement_garde_le_mode_de_la_carte_et_ne_touche_pas_au_tiroir()
    {
        var (vid, sid) = Preparer(ModesPaiement.CarteBancaire, 1000, ModesPaiement.CarteBancaire);
        Modifier(vid, 6);                                     // rendre 400
        var (v, ps) = Etat(vid);
        Assert.Equal((-400m, ModesPaiement.CarteBancaire), (ps[1].Montant, ps[1].Mode));
        Assert.Equal(0m, Attendu(sid));
        Assert.Equal(600m, v.MontantVerse);
    }

    [Fact]
    public void Mutuelle_la_part_patient_est_completee_ou_rendue()
    {
        var (vid, sid) = Preparer(ModesPaiement.Mutuelle, 500, mutuelleTaux: 50);   // 1 000 : 500 mutuelle, 500 patient déjà versés
        Modifier(vid, 20);                                    // 2 000 : patient 1 000 → +500
        var (v, ps) = Etat(vid);
        Assert.Equal(1000m, v.MontantMutuelle);
        Assert.Equal(500m, ps[1].Montant);
        Assert.Equal(1000m, v.MontantVerse);
        Assert.Equal(1000m, v.MontantRestant);                // seule la part mutuelle reste due
        Assert.Equal(1000m, Attendu(sid));

        Modifier(vid, 6);                                     // 600 : patient 300, versé 1 000 → rendre 700
        var (v2, ps2) = Etat(vid);
        Assert.Equal(-700m, ps2[2].Montant);
        Assert.StartsWith("R-", ps2[2].NumeroPaiement);
        Assert.Equal(300m, v2.MontantVerse);
        Assert.Equal(300m, v2.MontantRestant);                // part mutuelle 300 en attente
        Assert.Equal(300m, Attendu(sid));
    }

    [Fact]
    public void Credit_aucun_paiement_automatique_sauf_remboursement_si_le_verse_depasse_le_nouveau_total()
    {
        var (vid, sid) = Preparer(ModesPaiement.Credit, 300);
        Modifier(vid, 20);                                    // 2 000 : aucun paiement ajouté
        var (v, ps) = Etat(vid);
        Assert.Single(ps);
        Assert.Equal(1700m, v.MontantRestant);

        Modifier(vid, 2);                                     // 200 < versé 300 : rendre 100
        var (v2, ps2) = Etat(vid);
        Assert.Equal(2, ps2.Count);
        Assert.Equal((-100m, ModesPaiement.Especes), (ps2[1].Montant, ps2[1].Mode));
        Assert.Equal(0m, v2.MontantRestant);
        Assert.Equal(200m, Attendu(sid));
    }

    [Fact]
    public void Passage_de_Credit_a_Comptant_le_complement_est_encaisse()
    {
        var (vid, sid) = Preparer(ModesPaiement.Credit, 300);
        Modifier(vid, 10, nouveauType: ModesPaiement.Comptant);   // 1 000 comptant : 700 à encaisser
        var (v, ps) = Etat(vid);
        Assert.Equal(ModesPaiement.Comptant, v.Type);
        Assert.Equal(2, ps.Count);
        Assert.Equal((700m, ModesPaiement.Especes), (ps[1].Montant, ps[1].Mode));
        Assert.Equal(0m, v.MontantRestant);
        Assert.Equal(1000m, Attendu(sid));
    }

    [Fact]
    public void Modification_sans_changement_de_montant_n_ajoute_aucun_paiement()
    {
        var (vid, _) = Preparer(ModesPaiement.Comptant, 1000);
        Modifier(vid, 10);
        Assert.Single(Etat(vid).paiements);
    }

    [Fact]
    public void Le_remboursement_est_affiche_comme_Remboursement_dans_l_historique_des_paiements()
    {
        var (vid, _) = Preparer(ModesPaiement.Comptant, 1000);
        Modifier(vid, 4);
        UiHelper.EnSta(() =>
        {
            using var f = new FormPaiements(vid);
            var grille = UiHelper.Champ<DataGridView>(f, "dgvPaiements");
            Assert.Equal(2, grille.Rows.Count);
            Assert.Equal("Remboursement (Espèces)", Convert.ToString(grille.Rows[1].Cells["Mode"].Value));
            Assert.Equal(ModesPaiement.Especes, Convert.ToString(grille.Rows[0].Cells["Mode"].Value));
        });
    }
}
