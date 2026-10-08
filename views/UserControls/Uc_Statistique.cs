using Pharmacie2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pharmacie2.Services;
using System.Collections.Generic;
using System.Drawing;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Statistique : UserControl
    {
        // ── Cache pour export ─────────────────────────────────────────────
        private decimal _ca;
        private decimal _marge;
        private decimal _totalDepenses;
        private int _nbVentes;
        private int _nbImpayees;
        private decimal _restant;

        // Ventilation par mode de paiement
        private decimal _caEspeces;
        private decimal _caCheque;
        private decimal _caMobileMoney;   // Mvolo + Huri Money
        private decimal _caCB;
        private decimal _caCredit;
        private decimal _caMutuelle;

        private List<dynamic> _topProduits = new();
        private List<dynamic> _creditsClients = new();

        public Uc_Statistique()
        {
            InitializeComponent();
            cbPeriode.SelectedIndex = 2; // "Ce mois" par défaut
            cbPeriode.SelectedIndexChanged += (s, e) => ToggleCustomDates();
            btnActualiser.Click += (s, e) => ChargerStats();
            ToggleCustomDates();
            ChargerStats();
        }

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
        // CHARGEMENT PRINCIPAL
        // ══════════════════════════════════════════════════════════════════

        private void ChargerStats()
        {
            try
            {
                var (start, end) = GetPeriode();
                lblPeriodeAffichee.Text =
                    $"Période : {start:dd/MM/yyyy}  →  {end:dd/MM/yyyy}";

                using (var ctx = new AppDbContext())
                {
                    // Charger toutes les ventes de la période EN MÉMOIRE
                    var ventes = ctx.ventes
                        .AsNoTracking()
                        .Include(v => v.Paiements)
                        .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                        .Where(v => v.DateVente >= start && v.DateVente <= end)
                        .ToList();

                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();

                    // ── KPI généraux ───────────────────────────────────────
                    _nbVentes = ventesActives.Count;
                    _nbImpayees = ventesActives.Count(v => v.MontantRestant > 0);
                    _restant = ventesActives.Sum(v => v.MontantRestant);

                    _ca = ventesActives
                        .SelectMany(v => v.Lignes)
                        .Sum(l => (decimal)l.Quantite * (l.Produit?.PrixVente ?? 0m));

                    decimal achat = ventesActives
                        .SelectMany(v => v.Lignes)
                        .Sum(l => (decimal)l.Quantite * (l.Produit?.PrixAchat ?? 0m));

                    _marge = _ca - achat;

                    // ── Dépenses annexes de la période ────────────────────
                    var depenses = ctx.DepensesAnnexes
                        .Where(d => d.DateDepense >= start && d.DateDepense <= end)
                        .ToList();
                    decimal totalDepenses = depenses.Sum(d => d.Montant);

                    // Bénéfice net = marge brute - dépenses annexes
                    decimal beneficeNet = _marge - totalDepenses;
                    _totalDepenses = totalDepenses;

                    // ── Ventilation par MODE DE PAIEMENT ──────────────────
                    // Groupes mobiles
                    var mobileTypes = new[] { "Mvolo", "Huri Money" };

                    _caEspeces = ventesActives
                        .Where(v => v.Type == "Comptant")
                        .Sum(v => v.MontantTotal);

                    _caCheque = ventesActives
                        .Where(v => v.Type == "Chèque")
                        .Sum(v => v.MontantTotal);

                    _caCB = ventesActives
                        .Where(v => v.Type == "Carte bancaire")
                        .Sum(v => v.MontantTotal);

                    _caMobileMoney = ventesActives
                        .Where(v => mobileTypes.Contains(v.Type))
                        .Sum(v => v.MontantTotal);

                    _caCredit = ventesActives
                        .Where(v => v.Type == "Crédit")
                        .Sum(v => v.MontantTotal);

                    _caMutuelle = ventesActives
                        .Where(v => v.Type == "Mutuelle")
                        .Sum(v => v.MontantMutuelle);   // Part entreprise uniquement

                    // ── Mise à jour KPI cards ──────────────────────────────
                    lblCAValue.Text = $"{_ca:N0} KMF";
                    lblMargeValue.Text = $"{_marge:N0} KMF\n(net: {beneficeNet:N0})";
                    lblVentesValue.Text = _nbVentes.ToString();
                    lblImpayesValue.Text = _nbImpayees.ToString();
                    lblRestantValue.Text = $"{_restant:N0} KMF";

                    // Couleur marge
                    lblMargeValue.ForeColor = _marge >= 0
                        ? Color.FromArgb(27, 94, 32)
                        : Color.OrangeRed;

                    // ── Ventilation paiements — grille ─────────────────────
                    ChargerVentilationPaiements(ventesActives);

                    // ── Top produits ───────────────────────────────────────
                    _topProduits = ventesActives
                        .SelectMany(v => v.Lignes)
                        .Where(l => l.Produit != null)
                        .GroupBy(l => new { l.ProduitId, l.Produit.Nom })
                        .Select(g => new
                        {
                            Produit = g.Key.Nom,
                            QuantiteVendue = g.Sum(x => x.Quantite),
                            CA = g.Sum(x => (decimal)x.Quantite
                                               * (x.Produit?.PrixVente ?? 0m))
                        })
                        .OrderByDescending(x => x.CA)
                        .Take(10)
                        .Cast<dynamic>()
                        .ToList();

                    dgvTopProduits.DataSource = _topProduits;
                    StyleGrid(dgvTopProduits);

                    // ── Crédits clients ────────────────────────────────────
                    _creditsClients = ventesActives
                        .Where(v => v.MontantRestant > 0 && v.Type != "Mutuelle")
                        .GroupBy(v => new
                        {
                            v.NomClient,
                            v.PrenomClient,
                            v.TelephoneClient
                        })
                        .Select(g => new
                        {
                            Client = $"{g.Key.PrenomClient} {g.Key.NomClient}".Trim(),
                            Telephone = g.Key.TelephoneClient ?? "—",
                            NbVentes = g.Count(),
                            Restant = g.Sum(x => x.MontantRestant)
                        })
                        .OrderByDescending(x => x.Restant)
                        .Cast<dynamic>()
                        .ToList();

                    dgvCreditsClients.DataSource = _creditsClients;
                    StyleGrid(dgvCreditsClients);

                    // ── Mutuelles impayées ─────────────────────────────────
                    var mutuellesImpayees = ctx.mutuels
                        .AsNoTracking()
                        .ToList()
                        .Select(m =>
                        {
                            var ventesM = ventesActives
                                .Where(v => v.MutuelId == m.IdMutuel
                                         && !v.MutuelleReglee
                                         && v.MontantMutuelle > 0)
                                .ToList();
                            return new
                            {
                                Mutuelle = m.NomEmployeur,
                                NbVentes = ventesM.Count,
                                TotalDu = ventesM.Sum(v => (decimal)v.MontantMutuelle)
                            };
                        })
                        .Where(x => x.NbVentes > 0)
                        .OrderByDescending(x => x.TotalDu)
                        .ToList();

                    dgvMutuelleImpayes.DataSource = mutuellesImpayees;
                    StyleGrid(dgvMutuelleImpayes);

                    // ── Recommandations ────────────────────────────────────
                    ChargerRecommandations(ctx, ventesActives);

                    lblLastUpdate.Text =
                        $"Actualisation : {DateTime.Now:dd/MM/yyyy HH:mm}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur stats : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Ventilation par mode de paiement ──────────────────────────────

        private void ChargerVentilationPaiements(List<Vente> ventesActives)
        {
            var data = new List<object>
            {
                new {
                    ModePaiement = "💵 Espèces (Comptant)",
                    NbVentes     = ventesActives.Count(v => v.Type == "Comptant"),
                    Montant      = _caEspeces,
                    Pourcentage  = _ca > 0 ? Math.Round(_caEspeces / _ca * 100, 1) : 0m
                },
                new {
                    ModePaiement = "📱 Mobile Money (Mvolo / Huri)",
                    NbVentes     = ventesActives.Count(v => v.Type == "Mvolo" || v.Type == "Huri Money"),
                    Montant      = _caMobileMoney,
                    Pourcentage  = _ca > 0 ? Math.Round(_caMobileMoney / _ca * 100, 1) : 0m
                },
                new {
                    ModePaiement = "💳 Carte bancaire",
                    NbVentes     = ventesActives.Count(v => v.Type == "Carte bancaire"),
                    Montant      = _caCB,
                    Pourcentage  = _ca > 0 ? Math.Round(_caCB / _ca * 100, 1) : 0m
                },
                new {
                    ModePaiement = "📄 Chèque",
                    NbVentes     = ventesActives.Count(v => v.Type == "Chèque"),
                    Montant      = _caCheque,
                    Pourcentage  = _ca > 0 ? Math.Round(_caCheque / _ca * 100, 1) : 0m
                },
                new {
                    ModePaiement = "🏥 Mutuelle (part entreprise)",
                    NbVentes     = ventesActives.Count(v => v.Type == "Mutuelle"),
                    Montant      = _caMutuelle,
                    Pourcentage  = _ca > 0 ? Math.Round(_caMutuelle / _ca * 100, 1) : 0m
                },
                new {
                    ModePaiement = "📋 Crédit (impayé client)",
                    NbVentes     = ventesActives.Count(v => v.Type == "Crédit"),
                    Montant      = _caCredit,
                    Pourcentage  = _ca > 0 ? Math.Round(_caCredit / _ca * 100, 1) : 0m
                },
            };

            dgvVentilation.DataSource = null;
            dgvVentilation.DataSource = data;

            if (dgvVentilation.Columns["ModePaiement"] != null)
                dgvVentilation.Columns["ModePaiement"].HeaderText = "Mode de paiement";
            if (dgvVentilation.Columns["NbVentes"] != null)
                dgvVentilation.Columns["NbVentes"].HeaderText = "Nb ventes";
            if (dgvVentilation.Columns["Montant"] != null)
                dgvVentilation.Columns["Montant"].HeaderText = "Montant (KMF)";
            if (dgvVentilation.Columns["Pourcentage"] != null)
                dgvVentilation.Columns["Pourcentage"].HeaderText = "% du CA";

            // Colorer selon le montant
            foreach (DataGridViewRow row in dgvVentilation.Rows)
            {
                string mode = row.Cells["ModePaiement"].Value?.ToString() ?? "";
                if (mode.StartsWith("💵"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233); // vert espèces
                else if (mode.StartsWith("📱"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(227, 242, 253); // bleu mobile
                else if (mode.StartsWith("💳"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(243, 229, 245); // violet CB
                else if (mode.StartsWith("📄"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 253, 231); // jaune chèque
                else if (mode.StartsWith("🏥"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(225, 245, 254); // bleu clair mutuelle
                else if (mode.StartsWith("📋"))
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238); // rouge clair crédit
            }

            StyleGrid(dgvVentilation);
        }

        // ── Recommandations ───────────────────────────────────────────────

        private void ChargerRecommandations(AppDbContext ctx, List<Vente> ventesActives)
        {
            var sb = new StringBuilder();

            // Produits en alerte
            var alertes = ctx.produits.AsNoTracking()
                .Where(p => p.Actif && p.QuantiteEnStock > 0 && p.QuantiteEnStock <= p.SeuilAlerte * (p.NbUniteParBoite > 1 ? p.NbUniteParBoite : 1))
                .OrderBy(p => p.QuantiteEnStock)
                .Take(8).ToList();

            if (alertes.Any())
            {
                sb.AppendLine("⚠️  PRODUITS EN ALERTE STOCK :");
                foreach (var p in alertes)
                {
                    sb.AppendLine($"  • {p.Nom} — {StockService.Formater(p)} restant(s)");
                }
                sb.AppendLine();
            }

            // Ruptures
            var ruptures = ctx.produits.AsNoTracking()
                .Where(p => p.Actif && p.QuantiteEnStock <= 0)
                .Take(8).ToList();

            if (ruptures.Any())
            {
                sb.AppendLine("🔴  RUPTURES DE STOCK :");
                foreach (var p in ruptures)
                    sb.AppendLine($"  • {p.Nom}");
                sb.AppendLine();
            }

            // Expirations proches (30 jours)
            DateTime limite = DateTime.Now.AddDays(30);
            var expirations = ctx.produits.AsNoTracking()
                .Where(p => p.Actif && p.DateExpiration <= limite && p.DateExpiration >= DateTime.Now)
                .OrderBy(p => p.DateExpiration)
                .Take(8).ToList();

            if (expirations.Any())
            {
                sb.AppendLine("⏳  EXPIRATIONS DANS 30 JOURS :");
                foreach (var p in expirations)
                    sb.AppendLine($"  • {p.Nom} — expire le {p.DateExpiration:dd/MM/yyyy}");
                sb.AppendLine();
            }

            // Gros impayés clients
            var grosImpayeurs = ventesActives
                .Where(v => v.MontantRestant > 0 && v.Type != "Mutuelle")
                .GroupBy(v => $"{v.PrenomClient} {v.NomClient}".Trim())
                .Select(g => new { Client = g.Key, Total = g.Sum(x => x.MontantRestant) })
                .OrderByDescending(x => x.Total)
                .Take(3).ToList();

            if (grosImpayeurs.Any())
            {
                sb.AppendLine("💸  TOP DÉBITEURS :");
                foreach (var c in grosImpayeurs)
                    sb.AppendLine($"  • {c.Client} — {c.Total:N0} KMF");
            }

            if (sb.Length == 0)
                sb.AppendLine("✅  Tout est en ordre pour cette période !");

            rtbRecommandations.Text = sb.ToString();
        }

        // ── Style grille partagé ──────────────────────────────────────────

        private void StyleGrid(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 100, 46);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 245);
            dgv.Font = new Font("Segoe UI", 9F);
            dgv.RowTemplate.Height = 30;
            dgv.ColumnHeadersHeight = 34;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(220, 230, 220);
        }

        // ── Exports ───────────────────────────────────────────────────────

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"Rapport_{DateTime.Now:yyyy_MM}.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
                StatExportService.ExportExcel(sfd.FileName, _ca, _marge,
                    _nbVentes, _restant, _topProduits, _creditsClients);
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"Rapport_{DateTime.Now:yyyy_MM}.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
                StatExportService.ExportPDF(sfd.FileName, cbPeriode.Text,
                    _ca, _marge, _nbVentes, _nbImpayees, _restant,
                    _topProduits, _creditsClients, rtbRecommandations.Text);
        }
    }
}