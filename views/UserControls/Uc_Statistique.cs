using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// Statistiques : alertes d'abord, puis chiffres clés comparés à la période précédente, puis tableaux.
    /// </summary>
    public partial class Uc_Statistique : UserControl, IModeCompact
    {
        private StatsPeriode? _stats;
        private string _alertesTexte = "";

        /// <summary>Demande d'ouverture d'un écran (déjà filtré) depuis la liste « À surveiller ».</summary>
        public event Action<TypeAFaire>? OuvrirDemande;

        public Uc_Statistique()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            listeAlertes.AutoScroll = false;   // les alertes s'affichent toutes ; c'est l'écran entier qui défile
            ModeCompact.CartesAdaptatives(tlpKpi, 190);

            cbPeriode.SelectedIndex = 2;   // « Ce mois » par défaut (déclenche le premier calcul)
            ToggleCustomDates();
        }

        /// <summary>Compact : les quatre tableaux passent l'un sous l'autre (de hauteur fixe) ; l'écran défile verticalement.</summary>
        public void DefinirCompact(bool compact)
        {
            var m = (int a, int b, int c, int d) => new Padding(a, b, c, d);
            if (compact)
                ModeCompact.Recomposer(tlpTables, new[] { "P100" }, new[] { "F220", "F220", "F220", "F220" },
                    (tlpModes, 0, 0, m(0, 0, 0, 8)), (tlpTop, 0, 1, m(0, 0, 0, 8)), (tlpCredits, 0, 2, m(0, 0, 0, 8)), (tlpMutuelles, 0, 3, m(0, 0, 0, 0)));
            else
                ModeCompact.Recomposer(tlpTables, new[] { "P50", "P50" }, new[] { "F220", "F220" },
                    (tlpModes, 0, 0, m(0, 0, 8, 8)), (tlpTop, 1, 0, m(8, 0, 0, 8)), (tlpCredits, 0, 1, m(0, 0, 8, 0)), (tlpMutuelles, 1, 1, m(8, 0, 0, 0)));
        }

        private void cbPeriode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleCustomDates();
            if (cbPeriode.Text != "Personnalisé")
                ChargerStats();
        }

        private void btnActualiser_Click(object sender, EventArgs e) => ChargerStats();

        private void ToggleCustomDates()
        {
            bool custom = cbPeriode.Text == "Personnalisé";
            dtpDebut.Visible = custom;
            dtpFin.Visible = custom;
        }

        private (DateTime start, DateTime end) GetPeriode()
        {
            DateTime now = DateTime.Now;
            DateTime start, end;

            switch (cbPeriode.Text)
            {
                case "Aujourd'hui":
                    start = now.Date;
                    end = now.Date.AddDays(1).AddTicks(-1);
                    break;
                case "Cette semaine":
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    start = now.Date.AddDays(-diff);
                    end = start.AddDays(7).AddTicks(-1);
                    break;
                case "Ce mois":
                    start = new DateTime(now.Year, now.Month, 1);
                    end = start.AddMonths(1).AddTicks(-1);
                    break;
                case "Cette année":
                    start = new DateTime(now.Year, 1, 1);
                    end = start.AddYears(1).AddTicks(-1);
                    break;
                case "Personnalisé":
                    start = dtpDebut.Value.Date;
                    end = dtpFin.Value.Date.AddDays(1).AddTicks(-1);
                    break;
                default:
                    start = new DateTime(now.Year, now.Month, 1);
                    end = start.AddMonths(1).AddTicks(-1);
                    break;
            }

            if (end < start) (start, end) = (end, start);
            return (start, end);
        }

        // ══════════════════════════════════════════════════════════════════
        // CHARGEMENT
        // ══════════════════════════════════════════════════════════════════

        private void ChargerStats()
        {
            try
            {
                var (start, end) = GetPeriode();
                var (prevStart, prevEnd) = StatistiquesService.PeriodePrecedente(start, end);
                lblPeriodeAffichee.Text = $"Du {start:dd/MM/yyyy} au {end:dd/MM/yyyy}";

                var s = StatistiquesService.Calculer(start, end);
                var p = StatistiquesService.Calculer(prevStart, prevEnd);
                _stats = s;

                const string vs = " vs période précédente";
                const string sansBase = "rien sur la période précédente";

                carteCA.Valeur = Format.Montant(s.CA);
                carteCA.Detail = Format.EvolutionOuTexte(s.CA, p.CA, sansBase) + (p.CA == 0 ? "" : vs);
                carteCA.Tendance = p.CA == 0 ? 0 : Format.Tendance(s.CA, p.CA);

                carteBenefice.Valeur = Format.Montant(s.BeneficeNet);
                carteBenefice.Detail = Format.EvolutionOuTexte(s.BeneficeNet, p.BeneficeNet, sansBase) + (p.BeneficeNet == 0 ? "" : vs);
                carteBenefice.Tendance = p.BeneficeNet == 0 ? 0 : Format.Tendance(s.BeneficeNet, p.BeneficeNet);
                carteBenefice.Niveau = s.BeneficeNet >= 0 ? "succes" : "urgent";

                carteVentes.Valeur = s.NbVentes.ToString();
                carteVentes.Detail = Format.EvolutionOuTexte(s.NbVentes, p.NbVentes, sansBase) + (p.NbVentes == 0 ? "" : vs);
                carteVentes.Tendance = p.NbVentes == 0 ? 0 : Format.Tendance(s.NbVentes, p.NbVentes);

                carteASolder.Valeur = s.NbVentesASolder.ToString();
                carteASolder.Detail = "ventes pas encore soldées";
                carteASolder.Niveau = s.NbVentesASolder > 0 ? "attention" : "succes";

                carteARecuperer.Valeur = Format.Montant(s.ArgentARecuperer);
                carteARecuperer.Detail = Format.EvolutionOuTexte(s.ArgentARecuperer, p.ArgentARecuperer, sansBase) + (p.ArgentARecuperer == 0 ? "" : vs);
                carteARecuperer.Tendance = p.ArgentARecuperer == 0 ? 0 : Format.Tendance(s.ArgentARecuperer, p.ArgentARecuperer);
                carteARecuperer.HausseEstBonne = false;
                carteARecuperer.Niveau = s.ArgentARecuperer > 0 ? "attention" : "succes";

                dgvVentilation.DataSource = s.Modes;
                dgvTopProduits.DataSource = s.TopProduits;
                dgvCreditsClients.DataSource = s.Credits;
                dgvMutuelleImpayes.DataSource = s.Mutuelles;

                ChargerAlertes(s);
                lblLastUpdate.Text = $"Actualisé le {DateTime.Now:dd/MM/yyyy à HH:mm}";
            }
            catch (Exception ex)
            {
                Journal.Erreur("Calcul des statistiques", ex);
                MessageBox.Show("Les statistiques n'ont pas pu être calculées : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Alertes (périmés, ruptures, péremptions, stocks à vérifier, mutuelles en retard) puis plus gros débiteurs.</summary>
        private void ChargerAlertes(StatsPeriode s)
        {
            var elements = TableauDeBordService.AFaireMaintenant(DateTime.Now)
                .Select(a => new ElementAction(a.Texte, a.Niveau, () => OuvrirDemande?.Invoke(a.Type)))
                .ToList();

            var debiteurs = s.Credits.Take(3).ToList();
            if (debiteurs.Count > 0)
                elements.Add(new ElementAction(
                    "Plus gros débiteurs : " + string.Join(", ", debiteurs.Select(c => $"{c.Client} ({Format.Montant(c.Restant)})")),
                    "info", null));

            listeAlertes.Definir(elements);
            _alertesTexte = string.Join("\n", elements.Select(e => e.Texte));
        }

        // ── Exports ───────────────────────────────────────────────────────

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_stats == null) return;
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"Rapport_{DateTime.Now:yyyy_MM}.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                StatExportService.ExportExcel(sfd.FileName, lblPeriodeAffichee.Text, _stats);
                BandeauNotification.Succes("Rapport Excel enregistré : " + sfd.FileName);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Export Excel des statistiques", ex);
                MessageBox.Show("Export impossible : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (_stats == null) return;
            using var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"Rapport_{DateTime.Now:yyyy_MM}.pdf"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                StatExportService.ExportPDF(sfd.FileName, lblPeriodeAffichee.Text, _stats, _alertesTexte);
                BandeauNotification.Succes("Rapport PDF enregistré : " + sfd.FileName);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Export PDF des statistiques", ex);
                MessageBox.Show("Export impossible : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
