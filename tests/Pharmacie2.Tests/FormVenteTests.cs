using System.Reflection;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views;
using Xunit;

public class FormVenteTests
{
    private static int _evenements;

    private static (int produit, int mutuelle) Preparer(int stock = 100, int nbParBoite = 1)
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        var admin = new User { Nom = "Said", Prenom = "Fatima", Role = Roles.Administrateur, Login = "admin", MotDePasse = MotDePasseService.Hacher("x") };
        using (var ctx = new AppDbContext())
        {
            ctx.Users.Add(admin);
            ctx.SaveChanges();
            ctx.SessionsCaisse.Add(new SessionCaisse { UserId = admin.Id, DateOuverture = DateTime.Today, Statut = "Ouverte", CommentaireCloture = "" });   // encaisser des espèces demande une caisse ouverte
            ctx.SaveChanges();
        }
        SessionUtilisateur.Courant = admin;
        int pid = TestDb.NouveauProduit("Doliprane", stock, nbParBoite, "Plaquette", prixVente: 1000m);
        int mid;
        using (var ctx = new AppDbContext())
        {
            var m = new Mutuel { NomEmployeur = "Entreprise X", EmailContact = "", telephoneEmployeur = "", TauxPriseEnCharge = 80 };
            ctx.mutuels.Add(m);
            ctx.SaveChanges();
            mid = m.IdMutuel;
        }
        return (pid, mid);
    }

    /// <summary>Remplit le panier (boîtes à 1 000) et règle le mode de paiement, sans passer par la fenêtre de choix.</summary>
    private static FormVente Panier(int produitId, int boites, string mode, Action<FormVente>? reglages = null)
    {
        var f = new FormVente();
        var lignes = (List<LigneVente>)f.GetType().GetField("_lignes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
        lignes.Add(new LigneVente
        {
            ProduitId = produitId, Quantite = boites, PrixUnitaire = 1000m, UniteVendue = "Boîte",
            Produit = new Produit { Id = produitId, Nom = "Doliprane", NbUniteParBoite = 1, UniteVente = "Plaquette" }
        });
        UiHelper.Appeler(f, "RafraichirGrille");
        UiHelper.Appeler(f, "RecalculerTotal");
        UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = mode;
        if (mode is ModesPaiement.Credit or ModesPaiement.Mutuelle)
            UiHelper.Champ<TextBox>(f, "txtNom").Text = "Client";   // obligatoire pour ces deux modes
        reglages?.Invoke(f);
        return f;
    }

    [Fact]
    public void Section_client_repliee_pour_une_vente_rapide_et_ouverte_pour_un_credit()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        UiHelper.EnSta(() =>
        {
            using var f = new FormVente();
            var entete = UiHelper.Champ<LinkLabel>(f, "lblClient");
            Assert.EndsWith("+", entete.Text);                                  // comptant : section repliée, rien à saisir côté client
            Assert.Contains("facultatif", entete.Text);
            UiHelper.Champ<ComboBox>(f, "cbPaiement").SelectedItem = ModesPaiement.Credit;
            Assert.EndsWith("−", entete.Text);                             // crédit : ouverte automatiquement
            Assert.Contains("obligatoire", entete.Text);
            UiHelper.Appeler(f, "lblClient_Click", null, EventArgs.Empty);      // l'utilisateur peut aussi la replier
            Assert.EndsWith("+", entete.Text);
        });
    }

    [Fact]
    public void Credit_sans_nom_de_client_est_refuse()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Doliprane", 50, 1, "Comprimé");
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Credit, x => UiHelper.Champ<TextBox>(x, "txtNom").Text = "");
            Valider(f);
        });
        Assert.Equal(0, TestDb.Scalaire("SELECT COUNT(*) FROM ventes"));
    }

    private static void Valider(FormVente f) => UiHelper.Appeler(f, "btnValider_Click", null, EventArgs.Empty);

    private static void AvecMessages(Action a, bool oui = false)
    {
        var cts = UiHelper.FermerLesMessages(oui);
        try { UiHelper.EnSta(a); }
        finally { cts.Cancel(); }
    }

    [Fact]
    public void Vente_comptant_enregistre_ventes_lignes_paiement_stock_et_notifie_sans_champs_d_annulation()
    {
        var (pid, _) = Preparer();
        _evenements = 0;
        EventHandler h = (s, e) => _evenements++;
        VenteEvenements.VenteModifiee += h;
        try
        {
            AvecMessages(() =>
            {
                using var f = Panier(pid, 3, ModesPaiement.Comptant, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 5000);
                Valider(f);
            });
        }
        finally { VenteEvenements.VenteModifiee -= h; }

        Assert.True(_evenements >= 1);   // VenteModifiee déclenché après la vente
        using var ctx = new AppDbContext();
        var v = ctx.ventes.Single();
        Assert.Equal("V-000001", v.numeroVente);
        Assert.Equal("Active", v.Statut);
        Assert.Null(v.MotifAnnulation);
        Assert.Null(v.DateAnnulation);
        Assert.Equal(3000m, v.MontantTotal);
        Assert.Equal(5000m, v.MontantEspeces);
        Assert.Equal(2000m, v.MontantRendu);
        Assert.Equal(3000m, ctx.paiement.Single().Montant);
        Assert.Equal(3, ctx.LigneVentes.Single().QuantiteUnites);
        Assert.Equal(97, ctx.produits.Single().QuantiteEnStock);
    }

    [Theory]
    [InlineData("Chèque")]
    [InlineData("Carte bancaire")]
    [InlineData("Mvola")]
    [InlineData("Huri Money")]
    public void Modes_regles_en_totalite_enregistrent_toujours_le_montant_verse(string mode)
    {
        var (pid, _) = Preparer();
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, mode);
            Valider(f);
        });

        using var ctx = new AppDbContext();
        Assert.Equal(mode, ctx.ventes.Single().Type);
        Assert.Equal(2000m, ctx.paiement.Single().Montant);   // plus jamais 0
        Assert.Equal(98, ctx.produits.Single().QuantiteEnStock);
    }

    [Theory]
    [InlineData("Comptant", "Espèces")]
    [InlineData("Chèque", "Chèque")]
    [InlineData("Carte bancaire", "Carte bancaire")]
    [InlineData("Mvola", "Mvola")]
    [InlineData("Huri Money", "Huri Money")]
    public void Chaque_paiement_enregistre_son_mode(string typeVente, string modeAttendu)
    {
        var (pid, _) = Preparer();
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, typeVente, x => { if (typeVente == "Comptant") UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 2000; });
            Valider(f);
        });
        using var ctx = new AppDbContext();
        Assert.Equal(modeAttendu, ctx.paiement.Single().Mode);
    }

    [Fact]
    public void Avance_de_credit_et_part_patient_de_mutuelle_choisissent_leur_mode_Especes_par_defaut()
    {
        var (pid, mid) = Preparer();
        // Avance d'un crédit par Mvola
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Credit, x =>
            {
                UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 500;
                UiHelper.Champ<ComboBox>(x, "cbModeRecu").SelectedItem = ModesPaiement.Mvola;
            });
            Valider(f);
        });
        // Avance d'un crédit sans toucher au mode : Espèces
        AvecMessages(() =>
        {
            using var f = Panier(pid, 1, ModesPaiement.Credit, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 200);
            Assert.Equal(ModesPaiement.Especes, UiHelper.Champ<ComboBox>(f, "cbModeRecu").SelectedItem);
            Valider(f);
        });
        // Part patient d'une mutuelle (20 % de 2 000 = 400) payée par carte : pas de monnaie, montant exact
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, ModesPaiement.Mutuelle, x =>
            {
                UiHelper.Champ<ComboBox>(x, "cbMutuelle").SelectedIndex = 0;
                UiHelper.Champ<ComboBox>(x, "cbModeRecu").SelectedItem = ModesPaiement.CarteBancaire;
            });
            Valider(f);
        });

        using var ctx = new AppDbContext();
        var ps = ctx.paiement.OrderBy(p => p.Id).ToList();
        Assert.Equal(3, ps.Count);
        Assert.Equal((ModesPaiement.Mvola, 500m), (ps[0].Mode, ps[0].Montant));
        Assert.Equal((ModesPaiement.Especes, 200m), (ps[1].Mode, ps[1].Montant));
        Assert.Equal((ModesPaiement.CarteBancaire, 400m), (ps[2].Mode, ps[2].Montant));
        Assert.Equal(0m, ctx.ventes.OrderBy(v => v.IdVente).Last().MontantRendu);
    }

    [Fact]
    public void Comptant_avec_especes_insuffisantes_n_enregistre_rien()
    {
        var (pid, _) = Preparer();
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Comptant, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 2000);
            Valider(f);
        });
        using var ctx = new AppDbContext();
        Assert.Empty(ctx.ventes);
        Assert.Equal(100, ctx.produits.Single().QuantiteEnStock);
        Assert.True(UiHelper.TitreVu("Validation"));
    }

    [Fact]
    public void Credit_avec_avance_et_avance_trop_grande()
    {
        var (pid, _) = Preparer();
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Credit, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 5000);   // > total
            Valider(f);
        });
        using (var ctx = new AppDbContext()) Assert.Empty(ctx.ventes);

        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Credit, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 500);
            Valider(f);
        });
        using var ctx2 = new AppDbContext();
        var v = ctx2.ventes.Include(x => x.Paiements).Single();
        Assert.Equal(500m, v.MontantVerse);
        Assert.Equal(2500m, v.MontantRestant);
    }

    [Fact]
    public void Mutuelle_part_en_kmf_entiers_et_mutuelle_obligatoire()
    {
        var (pid, mid) = Preparer();
        // Aucune mutuelle choisie : refus
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Mutuelle, x => UiHelper.Champ<ComboBox>(x, "cbMutuelle").DataSource = null);
            Valider(f);
        });
        using (var ctx = new AppDbContext()) Assert.Empty(ctx.ventes);

        // Avec mutuelle à 80 % : 2 400 pris en charge, 600 pour le patient
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Mutuelle, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 1000);
            Valider(f);
        });
        using var ctx2 = new AppDbContext();
        var v = ctx2.ventes.Single();
        Assert.Equal(mid, v.MutuelId);
        Assert.Equal(2400m, v.MontantMutuelle);
        Assert.Equal(600m, ctx2.paiement.Single().Montant);
        Assert.Equal(400m, v.MontantRendu);
        Assert.False(v.MutuelleReglee);
    }

    [Fact]
    public void Stock_epuise_entre_temps_la_validation_est_refusee_et_rien_n_est_enregistre()
    {
        var (pid, _) = Preparer(stock: 100);
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Cheque);
            using (var ctx = new AppDbContext()) { ctx.produits.Find(pid).QuantiteEnStock = 2; ctx.SaveChanges(); }   // une autre vente est passée
            Valider(f);
        });
        using var ctx2 = new AppDbContext();
        Assert.Empty(ctx2.ventes);
        Assert.Equal(2, ctx2.produits.Single().QuantiteEnStock);
        Assert.True(UiHelper.TitreVu("Vente non enregistrée"));
    }

    [Fact]
    public void La_recherche_produit_a_le_focus_a_l_ouverture_et_le_panier_est_une_grille()
    {
        Preparer();
        UiHelper.EnSta(() =>
        {
            using var f = new FormVente();
            Assert.Same(UiHelper.Champ<TextBox>(f, "txtRecherche"), f.ActiveControl);
            Assert.IsType<DataGridView>(UiHelper.Champ<Control>(f, "dgvProduits"));
            Assert.Same(UiHelper.Champ<Button>(f, "btnValider"), f.AcceptButton);
            Assert.Same(UiHelper.Champ<Button>(f, "btnAnnuler"), f.CancelButton);
            Assert.True(f.KeyPreview);
        });
    }

    [Fact]
    public void Modification_et_annulation_notifient_aussi_VenteModifiee()
    {
        var (pid, _) = Preparer();
        int venteId;
        AvecMessages(() =>
        {
            using var f = Panier(pid, 3, ModesPaiement.Comptant, x => UiHelper.Champ<NumericUpDown>(x, "numEspeces").Value = 3000);
            Valider(f);
        });
        using (var ctx = new AppDbContext()) venteId = ctx.ventes.Single().IdVente;

        int compte = 0;
        EventHandler h = (s, e) => compte++;
        VenteEvenements.VenteModifiee += h;
        try
        {
            // Modification (OUI à la confirmation)
            AvecMessages(() =>
            {
                using var f = new FormModificationVente(venteId);
                UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
            }, oui: true);
            Assert.True(compte >= 1, "la modification doit notifier");

            int avant = compte;
            // Annulation (OUI à la confirmation)
            AvecMessages(() =>
            {
                using var f = new FormAnnulationVente(venteId);
                UiHelper.Champ<TextBox>(f, "txtMotif").Text = "Erreur de caisse";
                UiHelper.Appeler(f, "btnConfirmer_Click", null, EventArgs.Empty);
            }, oui: true);
            Assert.True(compte > avant, "l'annulation doit notifier");
        }
        finally { VenteEvenements.VenteModifiee -= h; }

        using var ctx2 = new AppDbContext();
        var v = ctx2.ventes.Single();
        Assert.Equal("Annulée", v.Statut);
        Assert.Equal("Erreur de caisse", v.MotifAnnulation);
        Assert.NotNull(v.DateAnnulation);
        Assert.Equal(100, ctx2.produits.Single().QuantiteEnStock);   // stock restitué
    }
    // ───────── mutuelle : nom du patient et matricule obligatoire (réglage par mutuelle) ─────────

    private static void ReglerMatricule(int mutuelleId, bool obligatoire)
    {
        using var ctx = new AppDbContext();
        ctx.mutuels.Single(m => m.IdMutuel == mutuelleId).MatriculeObligatoire = obligatoire;
        ctx.SaveChanges();
    }

    private static int VentesMutuelle() => (int)TestDb.Scalaire("SELECT COUNT(*) FROM ventes");

    [Fact]
    public void Mutuelle_sans_nom_de_patient_est_toujours_refusee()
    {
        var (pid, _) = Preparer();
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, ModesPaiement.Mutuelle, x =>
            {
                UiHelper.Champ<ComboBox>(x, "cbMutuelle").SelectedIndex = 0;
                UiHelper.Champ<TextBox>(x, "txtNom").Text = "";
                UiHelper.Champ<TextBox>(x, "txtMatricule").Text = "M-42";   // le matricule ne remplace pas le nom
            });
            Valider(f);
        });
        Assert.Equal(0, VentesMutuelle());
    }

    [Fact]
    public void Matricule_obligatoire_coche_refuse_la_vente_sans_matricule_et_l_accepte_avec()
    {
        var (pid, mid) = Preparer();
        ReglerMatricule(mid, true);
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, ModesPaiement.Mutuelle, x => UiHelper.Champ<ComboBox>(x, "cbMutuelle").SelectedIndex = 0);
            Valider(f);
        });
        Assert.Equal(0, VentesMutuelle());                                // refusée sans matricule

        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, ModesPaiement.Mutuelle, x =>
            {
                UiHelper.Champ<ComboBox>(x, "cbMutuelle").SelectedIndex = 0;
                UiHelper.Champ<TextBox>(x, "txtMatricule").Text = "M-42";
            });
            Valider(f);
        });
        Assert.Equal(1, VentesMutuelle());
        Assert.Equal("M-42", TestDb.Texte("SELECT MatriculeEmploye FROM ventes"));
    }

    [Fact]
    public void Matricule_non_obligatoire_par_defaut_la_vente_est_acceptee_sans_matricule()
    {
        var (pid, mid) = Preparer();
        using (var ctx = new AppDbContext()) Assert.False(ctx.mutuels.Single(m => m.IdMutuel == mid).MatriculeObligatoire);
        AvecMessages(() =>
        {
            using var f = Panier(pid, 2, ModesPaiement.Mutuelle, x => UiHelper.Champ<ComboBox>(x, "cbMutuelle").SelectedIndex = 0);
            Valider(f);
        });
        Assert.Equal(1, VentesMutuelle());
    }

    [Fact]
    public void La_fiche_mutuelle_enregistre_la_case_matricule_obligatoire()
    {
        var (_, mid) = Preparer();
        Mutuel m;
        using (var ctx = new AppDbContext()) m = ctx.mutuels.Single(x => x.IdMutuel == mid);
        AvecMessages(() =>
        {
            using var f = new FormAddMutuelle(m);
            var coche = UiHelper.Champ<CheckBox>(f, "chkMatricule");
            Assert.False(coche.Checked);                                    // décochée par défaut
            Assert.Equal("Matricule de l'employé obligatoire", coche.Text);
            coche.Checked = true;
            UiHelper.Appeler(f, "btnEnregistrer_Click", null, EventArgs.Empty);
        });
        using var ctx2 = new AppDbContext();
        Assert.True(ctx2.mutuels.Single(x => x.IdMutuel == mid).MatriculeObligatoire);
    }
}
