using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Dépenses et résultat : résultat du mois, résultat de l'année (12 mois) et dépenses saisies.
    /// Résultat = argent reçu − achat des produits vendus − dépenses.
    /// </summary>
    public partial class Uc_Depenses : UserControl
    {
        public Uc_Depenses()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            InitFiltres();

            cbMois.SelectedIndexChanged += (s, e) => ChargerResultatMensuel();
            cbAnnee.SelectedIndexChanged += (s, e) => { ChargerResultatMensuel(); ChargerResultatAnnuel(); };
            cbAnneeAnnuel.SelectedIndexChanged += (s, e) => ChargerResultatAnnuel();
            cbFiltreHistoCat.SelectedIndexChanged += (s, e) => ChargerHistorique();
            btnNouvelleDepense.Click += (s, e) => OuvrirFormulaire();
            btnSupprimerDep.Click += (s, e) => SupprimerDepense();
            btnActualiser.Click += (s, e) => { ChargerResultatMensuel(); ChargerResultatAnnuel(); ChargerHistorique(); };

            ChargerResultatMensuel();
            ChargerResultatAnnuel();
            ChargerHistorique();
        }

        private void InitFiltres()
        {
            for (int a = DateTime.Now.Year; a >= DateTime.Now.Year - 4; a--)
            {
                cbAnnee.Items.Add(a);
                cbAnneeAnnuel.Items.Add(a);
            }
            cbAnnee.SelectedItem = DateTime.Now.Year;
            cbAnneeAnnuel.SelectedItem = DateTime.Now.Year;

            string[] mois = { "Janvier","Février","Mars","Avril","Mai","Juin",
                               "Juillet","Août","Septembre","Octobre","Novembre","Décembre" };
            for (int i = 0; i < 12; i++) cbMois.Items.Add(new MoisItem(i + 1, mois[i]));
            cbMois.SelectedIndex = DateTime.Now.Month - 1;

            cbFiltreHistoCat.Items.Add("Toutes");
            foreach (var c in new[] { "Salaire", "Facture", "Loyer", "Fournitures", "Autre" })
                cbFiltreHistoCat.Items.Add(c);
            cbFiltreHistoCat.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════════════════════════
        // RÉSULTAT DU MOIS
        // ══════════════════════════════════════════════════════════════════

        private void ChargerResultatMensuel()
        {
            if (cbMois.SelectedItem == null || cbAnnee.SelectedItem == null) return;

            int mois = ((MoisItem)cbMois.SelectedItem).Numero;
            int annee = (int)cbAnnee.SelectedItem;

            DateTime debut = new DateTime(annee, mois, 1);
            DateTime fin = debut.AddMonths(1).AddSeconds(-1);

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var ventes = ctx.ventes.AsNoTracking()
                        .Where(v => v.DateVente >= debut && v.DateVente <= fin && v.Statut == "Active")
                        .ToList();

                    var paiements = ctx.paiement.AsNoTracking()
                        .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                        .ToList();

                    // Extraire les IDs en mémoire AVANT la requête SQLite
                    var idsVentesMois = ventes.Select(v => v.IdVente).ToList();
                    var lignes = ctx.LigneVentes.AsNoTracking()
                        .Include(l => l.Produit)
                        .Where(l => idsVentesMois.Contains(l.VenteId))
                        .ToList();

                    var depenses = ctx.DepensesAnnexes.AsNoTracking()
                        .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                        .ToList();

                    // ── ARGENT REÇU ───────────────────────────────────────
                    var idsVentes = ventes.Select(v => v.IdVente).ToHashSet();

                    decimal encaisseComptant = ventes.Where(v => v.Type == ModesPaiement.Comptant).Sum(v => v.MontantTotal);

                    decimal avancesCredit = paiements
                        .Where(p => idsVentes.Contains(p.VenteId))
                        .Where(p => ventes.FirstOrDefault(x => x.IdVente == p.VenteId)?.Type == ModesPaiement.Credit)
                        .Sum(p => p.Montant);

                    decimal partPatientMutuelle = ventes.Where(v => v.Type == ModesPaiement.Mutuelle).Sum(v => v.MontantTotal - v.MontantMutuelle);
                    decimal mutuelleReglee = ventes.Where(v => v.Type == ModesPaiement.Mutuelle && v.MutuelleReglee).Sum(v => v.MontantMutuelle);

                    decimal encaisseCB = ventes.Where(v => v.Type == ModesPaiement.CarteBancaire).Sum(v => v.MontantTotal);
                    decimal encaisseCheque = ventes.Where(v => v.Type == ModesPaiement.Cheque).Sum(v => v.MontantTotal);
                    decimal encaisseMobile = ventes.Where(v => ModesPaiement.EstMobile(v.Type)).Sum(v => v.MontantTotal);

                    decimal totalRevenus = encaisseComptant + avancesCredit + partPatientMutuelle + mutuelleReglee
                                           + encaisseCB + encaisseCheque + encaisseMobile;

                    // ── DÉPENSES ──────────────────────────────────────────
                    // Achat des produits vendus ce mois (coût réel à l'unité vendue)
                    decimal cogs = lignes.Sum(l => TableauDeBordService.CoutAchat(l));

                    decimal salaires = depenses.Where(d => d.Categorie == "Salaire").Sum(d => d.Montant);
                    decimal loyer = depenses.Where(d => d.Categorie == "Loyer").Sum(d => d.Montant);
                    decimal factures = depenses.Where(d => d.Categorie == "Facture").Sum(d => d.Montant);
                    decimal fournitures = depenses.Where(d => d.Categorie == "Fournitures").Sum(d => d.Montant);
                    decimal autres = depenses.Where(d => d.Categorie == "Autre").Sum(d => d.Montant);

                    decimal totalDepenses = salaires + loyer + factures + fournitures + autres;
                    decimal totalCharges = cogs + totalDepenses;

                    // ── RÉSULTAT ──────────────────────────────────────────
                    decimal beneficeNet = totalRevenus - totalCharges;
                    decimal margeNette = totalRevenus > 0 ? Math.Round(beneficeNet / totalRevenus * 100, 1) : 0;

                    SetLigne(lblEspeces, encaisseComptant);
                    SetLigne(lblCB, encaisseCB + encaisseCheque + encaisseMobile);
                    SetLigne(lblAvancesCredit, avancesCredit);
                    SetLigne(lblMutuelleP, partPatientMutuelle);
                    SetLigne(lblMutuelleE, mutuelleReglee);
                    SetLigneTotal(lblTotalRevenus, totalRevenus, true);

                    SetLigne(lblCOGS, cogs, depense: true);
                    SetLigne(lblSalaires, salaires, depense: true);
                    SetLigne(lblLoyer, loyer, depense: true);
                    SetLigne(lblFactures, factures, depense: true);
                    SetLigne(lblFournitures, fournitures, depense: true);
                    SetLigne(lblAutres, autres, depense: true);
                    SetLigneTotal(lblTotalCharges, totalCharges, false);

                    lblBeneficeNet.Text = Format.Montant(beneficeNet);
                    lblBeneficeNet.ForeColor = beneficeNet >= 0 ? Theme.SuccesTexte : Theme.UrgentTexte;

                    lblMarge.Text = $"Marge : {margeNette:0.#} %";
                    lblMarge.ForeColor = margeNette >= 20 ? Theme.SuccesTexte : margeNette >= 0 ? Theme.AttentionTexte : Theme.UrgentTexte;

                    lblPeriodeMensuel.Text =
                        $"{((MoisItem)cbMois.SelectedItem).Nom} {annee} — {Format.Compte(ventes.Count, "vente")}, {Format.Compte(depenses.Count, "dépense")} saisie{(depenses.Count > 1 ? "s" : "")}";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Résultat mensuel", ex);
                MessageBox.Show("Le résultat du mois n'a pas pu être calculé : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void SetLigne(Label lbl, decimal val, bool depense = false)
        {
            lbl.Text = Format.Montant(val);
            lbl.ForeColor = val > 0 ? (depense ? Theme.UrgentTexte : Theme.SuccesTexte) : Theme.Neutre;
        }

        private static void SetLigneTotal(Label lbl, decimal val, bool revenu)
        {
            lbl.Text = Format.Montant(val);
            lbl.ForeColor = revenu ? Theme.SuccesTexte : Theme.UrgentTexte;
        }

        // ══════════════════════════════════════════════════════════════════
        // RÉSULTAT DE L'ANNÉE — 12 MOIS
        // ══════════════════════════════════════════════════════════════════

        private void ChargerResultatAnnuel()
        {
            if (cbAnneeAnnuel.SelectedItem == null) return;
            int annee = (int)cbAnneeAnnuel.SelectedItem;

            string[] moisNoms = { "Janvier","Février","Mars","Avril","Mai","Juin",
                                   "Juillet","Août","Septembre","Octobre","Novembre","Décembre" };

            var lignes = new List<object>();
            decimal totalCA = 0, totalCOGS = 0, totalDep = 0, totalBen = 0;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    for (int m = 1; m <= 12; m++)
                    {
                        DateTime debut = new DateTime(annee, m, 1);
                        DateTime fin = debut.AddMonths(1).AddSeconds(-1);

                        var ventes = ctx.ventes.AsNoTracking()
                            .Where(v => v.DateVente >= debut && v.DateVente <= fin && v.Statut == "Active")
                            .ToList();

                        decimal ca = ventes.Sum(v => v.MontantTotal);

                        var idsMois = ventes.Select(v => v.IdVente).ToList();
                        var lignesVente = ctx.LigneVentes.AsNoTracking()
                            .Include(l => l.Produit)
                            .Where(l => idsMois.Contains(l.VenteId))
                            .ToList();

                        decimal cogs = lignesVente.Sum(l => TableauDeBordService.CoutAchat(l));

                        decimal depenses = ctx.DepensesAnnexes.AsNoTracking()
                            .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                            .Select(d => d.Montant)
                            .ToList()   // ToList() obligatoire avant Sum() sur decimal avec SQLite
                            .Sum();

                        decimal benefice = ca - cogs - depenses;
                        decimal marge = ca > 0 ? Math.Round(benefice / ca * 100, 1) : 0;

                        totalCA += ca; totalCOGS += cogs; totalDep += depenses; totalBen += benefice;

                        lignes.Add(new { Mois = moisNoms[m - 1], CA = ca, Cogs = cogs, Depenses = depenses, Benefice = benefice, Marge = $"{marge:0.#} %" });
                    }

                    decimal margeAnnuelle = totalCA > 0 ? Math.Round(totalBen / totalCA * 100, 1) : 0;
                    lignes.Add(new { Mois = "TOTAL", CA = totalCA, Cogs = totalCOGS, Depenses = totalDep, Benefice = totalBen, Marge = $"{margeAnnuelle:0.#} %" });
                }

                dgvAnnuel.DataSource = lignes;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Résultat annuel", ex);
                MessageBox.Show("Le résultat de l'année n'a pas pu être calculé : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAnnuel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAnnuel.Rows.Count) return;
            var ligne = dgvAnnuel.Rows[e.RowIndex];
            bool total = ligne.Cells["Mois"].Value?.ToString() == "TOTAL";

            if (total)
            {
                e.CellStyle.BackColor = Theme.InfoFond;
                e.CellStyle.Font = Theme.Police(10, FontStyle.Bold);
            }
            if (dgvAnnuel.Columns[e.ColumnIndex].Name == "Benefice" && ligne.Cells["Benefice"].Value is decimal b)
            {
                e.CellStyle.ForeColor = b >= 0 ? Theme.SuccesTexte : Theme.UrgentTexte;
                e.CellStyle.Font = Theme.Police(10, FontStyle.Bold);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // DÉPENSES SAISIES
        // ══════════════════════════════════════════════════════════════════

        private void ChargerHistorique()
        {
            if (cbAnnee.SelectedItem == null || cbFiltreHistoCat.SelectedItem == null) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.DepensesAnnexes.AsNoTracking().Include(d => d.User).AsQueryable();

                    string cat = cbFiltreHistoCat.SelectedItem?.ToString() ?? "Toutes";
                    if (cat != "Toutes") query = query.Where(d => d.Categorie == cat);

                    var depenses = query.OrderByDescending(d => d.DateDepense).ToList();

                    dgvHistorique.DataSource = depenses.Select(d => new
                    {
                        d.Id,
                        Date = d.DateDepense.ToString("dd/MM/yyyy"),
                        d.Categorie,
                        d.Description,
                        d.Montant,
                        Utilisateur = d.User != null ? $"{d.User.Prenom} {d.User.Nom}" : "—"
                    }).ToList();

                    lblTotalHistorique.Text = $"Total affiché : {Format.Montant(depenses.Sum(d => d.Montant))} — {Format.Compte(depenses.Count, "dépense")}";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Historique des dépenses", ex);
                MessageBox.Show("Les dépenses n'ont pas pu être affichées : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Actions ───────────────────────────────────────────────────────

        private void OuvrirFormulaire()
        {
            using (var form = new FormAddDepense())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ChargerResultatMensuel();
                    ChargerResultatAnnuel();
                    ChargerHistorique();
                }
            }
        }

        private void SupprimerDepense()
        {
            if (tabMain.SelectedTab != tabHistorique) return;
            if (dgvHistorique.SelectedRows.Count == 0) return;

            if (SessionUtilisateur.Courant?.Role != Roles.Administrateur)
            {
                MessageBox.Show("Seul l'administrateur peut supprimer une dépense.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Supprimer cette dépense ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            int id = Convert.ToInt32(dgvHistorique.SelectedRows[0].Cells["Id"].Value);
            using (var ctx = new AppDbContext())
            {
                var dep = ctx.DepensesAnnexes.Find(id);
                if (dep != null) { ctx.DepensesAnnexes.Remove(dep); ctx.SaveChanges(); }
            }

            ChargerResultatMensuel();
            ChargerResultatAnnuel();
            ChargerHistorique();
        }
    }

    internal class MoisItem
    {
        public int Numero { get; }
        public string Nom { get; }
        public MoisItem(int n, string nom) { Numero = n; Nom = nom; }
        public override string ToString() => Nom;
    }
}
