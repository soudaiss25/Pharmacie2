using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Module Dépenses & Résultat — P&amp;L mensuel et annuel.
    ///
    /// Onglet 1 : Compte de résultat mensuel
    ///   Revenus encaissés - COGS - Dépenses fixes/variables = Bénéfice net
    ///
    /// Onglet 2 : Vue annuelle — tableau 12 mois
    ///
    /// Onglet 3 : Saisie et historique des dépenses
    /// </summary>
    public partial class Uc_Depenses : UserControl
    {
        public Uc_Depenses()
        {
            InitializeComponent();
            this.Load += (s, e) =>
            {
                // SplitterDistance au Load — taille réelle connue à ce moment
                try
                {
                    int dist = (int)(splitMensuel.Width * 0.65);
                    if (dist > splitMensuel.Panel1MinSize &&
                        dist < splitMensuel.Width - splitMensuel.Panel2MinSize)
                        splitMensuel.SplitterDistance = dist;
                }
                catch { }

                InitFiltres();
                ChargerResultatMensuel();
                ChargerResultatAnnuel();
                ChargerHistorique();
            };

            cbMois.SelectedIndexChanged += (s, e) => ChargerResultatMensuel();
            cbAnnee.SelectedIndexChanged += (s, e) => { ChargerResultatMensuel(); ChargerResultatAnnuel(); };
            cbAnneeAnnuel.SelectedIndexChanged += (s, e) => ChargerResultatAnnuel();
            cbFiltreHistoCat.SelectedIndexChanged += (s, e) => ChargerHistorique();
            btnNouvelleDepense.Click += (s, e) => OuvrirFormulaire();
            btnSupprimerDep.Click += (s, e) => SupprimerDepense();
            btnActualiser.Click += (s, e) => { ChargerResultatMensuel(); ChargerResultatAnnuel(); ChargerHistorique(); };
        }

        private void InitFiltres()
        {
            // Années
            cbAnnee.Items.Clear();
            cbAnneeAnnuel.Items.Clear();
            for (int a = DateTime.Now.Year; a >= DateTime.Now.Year - 4; a--)
            {
                cbAnnee.Items.Add(a);
                cbAnneeAnnuel.Items.Add(a);
            }
            cbAnnee.SelectedItem = DateTime.Now.Year;
            cbAnneeAnnuel.SelectedItem = DateTime.Now.Year;

            // Mois
            cbMois.Items.Clear();
            string[] mois = { "Janvier","Février","Mars","Avril","Mai","Juin",
                               "Juillet","Août","Septembre","Octobre","Novembre","Décembre" };
            for (int i = 0; i < 12; i++) cbMois.Items.Add(new MoisItem(i + 1, mois[i]));
            cbMois.SelectedIndex = DateTime.Now.Month - 1;

            // Filtre historique
            cbFiltreHistoCat.Items.Add("Toutes");
            foreach (var c in new[] { "Salaire", "Facture", "Loyer", "Fournitures", "Autre" })
                cbFiltreHistoCat.Items.Add(c);
            cbFiltreHistoCat.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 1 — COMPTE DE RÉSULTAT MENSUEL
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
                    var ventes = ctx.ventes
                        .Where(v => v.DateVente >= debut && v.DateVente <= fin
                               && v.Statut == "Active")
                        .ToList();

                    var paiements = ctx.paiement
                        .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                        .ToList();

                    var lignes = ctx.LigneVentes
                        .Include(l => l.Produit)
                        .Where(l => ventes.Select(v => v.IdVente).Contains(l.VenteId))
                        .ToList();

                    var depenses = ctx.DepensesAnnexes
                        .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                        .ToList();

                    // ── REVENUS ───────────────────────────────────────────
                    var idsVentes = ventes.Select(v => v.IdVente).ToHashSet();

                    // Encaissements réels (paiements reçus)
                    decimal encaisseComptant = ventes
                        .Where(v => v.Type == "Comptant")
                        .Sum(v => (decimal)v.MontantTotal);

                    decimal avancesCredit = paiements
                        .Where(p => idsVentes.Contains(p.VenteId))
                        .Where(p => {
                            var v = ventes.FirstOrDefault(x => x.IdVente == p.VenteId);
                            return v?.Type == "Crédit";
                        })
                        .Sum(p => (decimal)p.Montant);

                    decimal partPatientMutuelle = ventes
                        .Where(v => v.Type == "Mutuelle")
                        .Sum(v => (decimal)(v.MontantTotal - v.MontantMutuelle));

                    decimal mutuelleReglee = ventes
                        .Where(v => v.Type == "Mutuelle" && v.MutuelleReglee)
                        .Sum(v => (decimal)v.MontantMutuelle);

                    decimal encaisseCB = ventes.Where(v => v.Type == "Carte bancaire").Sum(v => (decimal)v.MontantTotal);
                    decimal encaisseCheque = ventes.Where(v => v.Type == "Chèque").Sum(v => (decimal)v.MontantTotal);
                    decimal encaisseMobile = ventes.Where(v => v.Type == "Mvolo" || v.Type == "Huri Money").Sum(v => (decimal)v.MontantTotal);

                    decimal totalRevenus = encaisseComptant + avancesCredit
                                           + partPatientMutuelle + mutuelleReglee
                                           + encaisseCB + encaisseCheque + encaisseMobile;

                    // ── CHARGES ───────────────────────────────────────────
                    // COGS = coût d'achat des produits vendus ce mois
                    decimal cogs = lignes.Sum(l =>
                        (decimal)l.Quantite * (l.Produit?.PrixAchat ?? 0m));

                    // Dépenses par catégorie
                    decimal salaires = depenses.Where(d => d.Categorie == "Salaire").Sum(d => d.Montant);
                    decimal loyer = depenses.Where(d => d.Categorie == "Loyer").Sum(d => d.Montant);
                    decimal factures = depenses.Where(d => d.Categorie == "Facture").Sum(d => d.Montant);
                    decimal fournitures = depenses.Where(d => d.Categorie == "Fournitures").Sum(d => d.Montant);
                    decimal autres = depenses.Where(d => d.Categorie == "Autre").Sum(d => d.Montant);

                    decimal totalDepenses = salaires + loyer + factures + fournitures + autres;
                    decimal totalCharges = cogs + totalDepenses;

                    // ── RÉSULTAT ──────────────────────────────────────────
                    decimal beneficeNet = totalRevenus - totalCharges;
                    decimal margeNette = totalRevenus > 0
                        ? Math.Round(beneficeNet / totalRevenus * 100, 1) : 0;

                    // ── Mise à jour UI ────────────────────────────────────
                    // Revenus
                    SetLigne(lblEspeces, encaisseComptant);
                    SetLigne(lblCB, encaisseCB + encaisseCheque + encaisseMobile);
                    SetLigne(lblAvancesCredit, avancesCredit);
                    SetLigne(lblMutuelleP, partPatientMutuelle);
                    SetLigne(lblMutuelleE, mutuelleReglee);
                    SetLigneTotal(lblTotalRevenus, totalRevenus, true);

                    // Charges
                    SetLigne(lblCOGS, cogs, rouge: true);
                    SetLigne(lblSalaires, salaires, rouge: true);
                    SetLigne(lblLoyer, loyer, rouge: true);
                    SetLigne(lblFactures, factures, rouge: true);
                    SetLigne(lblFournitures, fournitures, rouge: true);
                    SetLigne(lblAutres, autres, rouge: true);
                    SetLigneTotal(lblTotalCharges, totalCharges, false);

                    // Résultat
                    lblBeneficeNet.Text = $"{beneficeNet:N0} KMF";
                    lblBeneficeNet.ForeColor = beneficeNet >= 0
                        ? Color.FromArgb(27, 94, 32) : Color.FromArgb(183, 28, 28);
                    lblBeneficeNet.Font = new Font("Segoe UI", 20F, FontStyle.Bold);

                    lblMarge.Text = $"Marge nette : {margeNette}%";
                    lblMarge.ForeColor = margeNette >= 20
                        ? Color.FromArgb(27, 94, 32)
                        : margeNette >= 0 ? Color.OrangeRed
                        : Color.FromArgb(183, 28, 28);

                    lblPeriodeMensuel.Text =
                        $"{((MoisItem)cbMois.SelectedItem).Nom} {annee}  " +
                        $"— {ventes.Count} vente(s)  |  {depenses.Count} dépense(s) saisie(s)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur résultat mensuel : " + ex.Message);
            }
        }

        private void SetLigne(Label lbl, decimal val, bool rouge = false)
        {
            lbl.Text = $"{val:N0} KMF";
            lbl.ForeColor = rouge
                ? (val > 0 ? Color.FromArgb(183, 28, 28) : Color.Gray)
                : (val > 0 ? Color.FromArgb(27, 94, 32) : Color.Gray);
        }

        private void SetLigneTotal(Label lbl, decimal val, bool revenu)
        {
            lbl.Text = $"{val:N0} KMF";
            lbl.ForeColor = revenu
                ? Color.FromArgb(27, 94, 32)
                : Color.FromArgb(183, 28, 28);
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 2 — TABLEAU ANNUEL 12 MOIS
        // ══════════════════════════════════════════════════════════════════

        private void ChargerResultatAnnuel()
        {
            if (cbAnneeAnnuel.SelectedItem == null) return;
            int annee = (int)cbAnneeAnnuel.SelectedItem;

            string[] moisNoms = { "Jan","Fév","Mar","Avr","Mai","Juin",
                                   "Juil","Aoû","Sep","Oct","Nov","Déc" };

            dgvAnnuel.Rows.Clear();

            decimal totalCA = 0, totalCOGS = 0, totalDep = 0, totalBen = 0;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    for (int m = 1; m <= 12; m++)
                    {
                        DateTime debut = new DateTime(annee, m, 1);
                        DateTime fin = debut.AddMonths(1).AddSeconds(-1);

                        var ventes = ctx.ventes
                            .Where(v => v.DateVente >= debut && v.DateVente <= fin
                                   && v.Statut == "Active")
                            .ToList();

                        decimal ca = ventes.Sum(v => (decimal)v.MontantTotal);

                        var lignes = ctx.LigneVentes
                            .Include(l => l.Produit)
                            .Where(l => ventes.Select(v => v.IdVente).Contains(l.VenteId))
                            .ToList();

                        decimal cogs = lignes.Sum(l =>
                            (decimal)l.Quantite * (l.Produit?.PrixAchat ?? 0m));

                        decimal depenses = ctx.DepensesAnnexes
                            .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                            .ToList()   // ← ToList() obligatoire avant Sum() sur decimal avec SQLite
                            .Sum(d => d.Montant);

                        decimal benefice = ca - cogs - depenses;
                        decimal marge = ca > 0 ? Math.Round(benefice / ca * 100, 1) : 0;

                        totalCA += ca;
                        totalCOGS += cogs;
                        totalDep += depenses;
                        totalBen += benefice;

                        int idx = dgvAnnuel.Rows.Add(
                            moisNoms[m - 1],
                            $"{ca:N0}",
                            $"{cogs:N0}",
                            $"{depenses:N0}",
                            $"{benefice:N0}",
                            $"{marge}%"
                        );

                        // Coloration bénéfice
                        var cell = dgvAnnuel.Rows[idx].Cells["colBenefice"];
                        cell.Style.ForeColor = benefice >= 0
                            ? Color.FromArgb(27, 94, 32)
                            : Color.FromArgb(183, 28, 28);
                        cell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                        // Griser les mois futurs
                        if (debut > DateTime.Now)
                            dgvAnnuel.Rows[idx].DefaultCellStyle.ForeColor = Color.LightGray;
                    }

                    // Ligne TOTAL
                    decimal margeAnnuelle = totalCA > 0
                        ? Math.Round(totalBen / totalCA * 100, 1) : 0;

                    int idxTotal = dgvAnnuel.Rows.Add(
                        "TOTAL",
                        $"{totalCA:N0}",
                        $"{totalCOGS:N0}",
                        $"{totalDep:N0}",
                        $"{totalBen:N0}",
                        $"{margeAnnuelle}%"
                    );

                    var rowTotal = dgvAnnuel.Rows[idxTotal];
                    rowTotal.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                    rowTotal.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    rowTotal.Cells["colBenefice"].Style.ForeColor = totalBen >= 0
                        ? Color.FromArgb(27, 94, 32) : Color.FromArgb(183, 28, 28);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur résultat annuel : " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 3 — HISTORIQUE DÉPENSES
        // ══════════════════════════════════════════════════════════════════

        private void ChargerHistorique()
        {
            if (cbAnnee.SelectedItem == null) return;

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.DepensesAnnexes
                        .Include(d => d.User)
                        .AsQueryable();

                    string cat = cbFiltreHistoCat.SelectedItem?.ToString() ?? "Toutes";
                    if (cat != "Toutes") query = query.Where(d => d.Categorie == cat);

                    var depenses = query.OrderByDescending(d => d.DateDepense).ToList();

                    dgvHistorique.Rows.Clear();
                    foreach (var d in depenses)
                    {
                        dgvHistorique.Rows.Add(
                            d.Id,
                            d.DateDepense.ToString("dd/MM/yyyy"),
                            d.Categorie,
                            d.Description,
                            $"{d.Montant:N0} KMF",
                            d.User != null ? $"{d.User.Prenom} {d.User.Nom}" : "—"
                        );
                    }

                    lblTotalHistorique.Text =
                        $"Total affiché : {depenses.Sum(d => d.Montant):N0} KMF  " +
                        $"— {depenses.Count} entrée(s)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur historique : " + ex.Message);
            }
        }

        // ── Actions ───────────────────────────────────────────────────────

        private void OuvrirFormulaire()
        {
            using (var form = new Pharmacie2.views.FormAddDepense())
            {
                if (form.ShowDialog() == DialogResult.OK)
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

            if (SessionUtilisateur.Courant?.Role != "Administrateur")
            {
                MessageBox.Show("Seul l'administrateur peut supprimer une dépense.",
                    "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Supprimer cette dépense ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            int id = Convert.ToInt32(dgvHistorique.SelectedRows[0].Cells[0].Value);
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