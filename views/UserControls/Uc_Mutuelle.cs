using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Mutuelle : UserControl
    {
        private int _mutuelleSelectionneeId = -1;
        private string _mutuelleSelectionneeNom = "";

        public Uc_Mutuelle()
        {
            InitializeComponent();
            this.Load += (s, e) => ChargerMutuelles();

            dgvMutuelles.SelectionChanged += DgvMutuelles_SelectionChanged;
            btnReglertout.Click += btnReglerTout_Click;
            btnReglerSelection.Click += btnReglerSelection_Click;
            btnActualiser.Click += (s, e) =>
            {
                ChargerMutuelles();
                if (_mutuelleSelectionneeId > 0)
                    ChargerVentesImpayees(_mutuelleSelectionneeId, _mutuelleSelectionneeNom);
            };
        }

        // ══════════════════════════════════════════════════════════════════
        // PARTIE HAUTE — Liste des mutuelles
        // ══════════════════════════════════════════════════════════════════

        private CheckBox _chkArchives;
        private bool _filtreRetard;

        /// <summary>Ne garde que les mutuelles qui n'ont pas réglé leur part depuis plus de 30 jours.</summary>
        public void FiltrerEnRetard()
        {
            _filtreRetard = true;
            ChargerMutuelles();
        }

        private (int id, string nom, bool actif)? SelectionMutuelle()
        {
            if (dgvMutuelles.SelectedRows.Count == 0) return null;
            int id = Convert.ToInt32(dgvMutuelles.SelectedRows[0].Cells["IdMutuel"].Value);
            return (id, dgvMutuelles.SelectedRows[0].Cells["Mutuelle"].Value?.ToString() ?? "",
                    ArchivageService.EstActif(TypeElement.Mutuelle, id));
        }

        private void ChargerMutuelles()
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    bool archives = _chkArchives?.Checked ?? false;
                    var mutuels = ctx.mutuels.Where(m => m.Actif || archives).ToList();
                    if (_filtreRetard)
                    {
                        var enRetard = TableauDeBordService.MutuellesEnRetard(ctx, DateTime.Now).Select(x => x.mutuelleId).ToHashSet();
                        mutuels = mutuels.Where(m => enRetard.Contains(m.IdMutuel)).ToList();
                    }

                    // ✅ Charger toutes les ventes mutuelle impayées EN MÉMOIRE d'abord
                    // SQLite ne supporte pas Sum() sur decimal directement en requête EF
                    var ventesImpayees = ctx.ventes
                        .Where(v => v.Statut == "Active"
                                 && !v.MutuelleReglee
                                 && v.MontantMutuelle > 0
                                 && v.MutuelId != null)
                        .Select(v => new { v.MutuelId, v.MontantMutuelle })
                        .ToList(); // ← en mémoire avant le calcul

                    var data = mutuels.Select(m =>
                    {
                        var ventesM = ventesImpayees.Where(v => v.MutuelId == m.IdMutuel).ToList();
                        decimal totalImpaye = ventesM.Sum(v => (decimal)v.MontantMutuelle);
                        int nbImpayees = ventesM.Count;

                        return new
                        {
                            m.IdMutuel,
                            Mutuelle = m.Actif ? m.NomEmployeur : m.NomEmployeur + " (📦 archivée)",
                            Taux = m.TauxPriseEnCharge,
                            Telephone = m.telephoneEmployeur ?? "—",
                            Email = m.EmailContact ?? "—",
                            NbImpayees = nbImpayees,
                            TotalImpaye = totalImpaye
                        };
                    }).ToList();

                    dgvMutuelles.DataSource = null;
                    dgvMutuelles.DataSource = data;

                    if (dgvMutuelles.Columns["IdMutuel"] != null)
                        dgvMutuelles.Columns["IdMutuel"].Visible = false;
                    if (dgvMutuelles.Columns["Mutuelle"] != null)
                        dgvMutuelles.Columns["Mutuelle"].HeaderText = "Mutuelle / Employeur";
                    if (dgvMutuelles.Columns["Taux"] != null)
                        dgvMutuelles.Columns["Taux"].HeaderText = "Taux (%)";
                    if (dgvMutuelles.Columns["NbImpayees"] != null)
                        dgvMutuelles.Columns["NbImpayees"].HeaderText = "Ventes impayées";
                    if (dgvMutuelles.Columns["TotalImpaye"] != null)
                        dgvMutuelles.Columns["TotalImpaye"].HeaderText = "Total dû (KMF)";

                    foreach (DataGridViewRow row in dgvMutuelles.Rows)
                    {
                        var impaye = row.Cells["TotalImpaye"].Value;
                        if (impaye != null && Convert.ToDecimal(impaye) > 0)
                        {
                            row.Cells["TotalImpaye"].Style.ForeColor =
                                System.Drawing.Color.OrangeRed;
                            row.Cells["TotalImpaye"].Style.Font =
                                new System.Drawing.Font("Segoe UI", 9F,
                                    System.Drawing.FontStyle.Bold);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur chargement mutuelles : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMutuelles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMutuelles.SelectedRows.Count == 0) return;

            _mutuelleSelectionneeId = Convert.ToInt32(
                dgvMutuelles.SelectedRows[0].Cells["IdMutuel"].Value);
            _mutuelleSelectionneeNom =
                dgvMutuelles.SelectedRows[0].Cells["Mutuelle"].Value?.ToString() ?? "";

            ChargerVentesImpayees(_mutuelleSelectionneeId, _mutuelleSelectionneeNom);
        }

        // ══════════════════════════════════════════════════════════════════
        // PARTIE BASSE — Ventes impayées
        // ══════════════════════════════════════════════════════════════════

        private void ChargerVentesImpayees(int mutuelId, string nomMutuelle)
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var ventes = ctx.ventes
                        .Include(v => v.User)
                        .Where(v => v.MutuelId == mutuelId
                                 && v.Statut == "Active"
                                 && !v.MutuelleReglee
                                 && v.MontantMutuelle > 0)
                        .OrderByDescending(v => v.DateVente)
                        .ToList();

                    dgvVentesImpayees.Rows.Clear();

                    foreach (var v in ventes)
                    {
                        string client = $"{v.PrenomClient} {v.NomClient}".Trim();
                        if (string.IsNullOrWhiteSpace(client)) client = "—";
                        string vendeur = v.User != null
                            ? $"{v.User.Prenom} {v.User.Nom}" : "—";

                        dgvVentesImpayees.Rows.Add(
                            false,
                            v.IdVente,
                            v.numeroVente,
                            v.DateVente.ToString("dd/MM/yyyy HH:mm"),
                            client,
                            v.MatriculeEmploye ?? "—",
                            $"{v.MontantTotal:N0} KMF",
                            $"{v.MontantMutuelle:N0} KMF",
                            vendeur);
                    }

                    decimal totalDu = ventes.Sum(v => (decimal)v.MontantMutuelle);
                    int nbVentes = ventes.Count;

                    lblRecapImpaye.Text =
                        $"🏢 {nomMutuelle}  |  " +
                        $"{nbVentes} vente(s) impayée(s)  |  " +
                        $"Total dû : {totalDu:N0} KMF";

                    lblRecapImpaye.ForeColor = totalDu > 0
                        ? System.Drawing.Color.OrangeRed
                        : System.Drawing.Color.FromArgb(27, 94, 32);

                    btnReglertout.Enabled = nbVentes > 0;
                    btnReglerSelection.Enabled = nbVentes > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur chargement ventes : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // RÈGLEMENT
        // ══════════════════════════════════════════════════════════════════

        private void btnReglerTout_Click(object sender, EventArgs e)
        {
            if (_mutuelleSelectionneeId <= 0) return;
            if (dgvVentesImpayees.Rows.Count == 0) return;

            decimal totalDu = 0;
            foreach (DataGridViewRow row in dgvVentesImpayees.Rows)
            {
                string s = row.Cells["colMontantMutuelle"].Value?.ToString()
                    ?.Replace(" KMF", "").Replace("\u202f", "").Replace(" ", "") ?? "0";
                if (decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal m))
                    totalDu += m;
            }

            var confirm = MessageBox.Show(
                $"Régler TOUTES les ventes impayées de « {_mutuelleSelectionneeNom} » ?\n\n" +
                $"Nombre de ventes : {dgvVentesImpayees.Rows.Count}\n" +
                $"Montant total    : {totalDu:N0} KMF",
                "Confirmation règlement total",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            string reference = AskReference();
            AppliquerReglement(null, reference);
        }

        private void btnReglerSelection_Click(object sender, EventArgs e)
        {
            if (_mutuelleSelectionneeId <= 0) return;

            var ids = new List<int>();
            decimal total = 0;

            foreach (DataGridViewRow row in dgvVentesImpayees.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["colCoche"].Value)) continue;
                ids.Add(Convert.ToInt32(row.Cells["colVenteId"].Value));
                string s = row.Cells["colMontantMutuelle"].Value?.ToString()
                    ?.Replace(" KMF", "").Replace("\u202f", "").Replace(" ", "") ?? "0";
                if (decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal m))
                    total += m;
            }

            if (ids.Count == 0)
            {
                MessageBox.Show("Cochez au moins une vente à régler.",
                    "Sélection vide", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Régler {ids.Count} vente(s) sélectionnée(s) ?\nMontant : {total:N0} KMF",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            AppliquerReglement(ids, AskReference());
        }

        private void AppliquerReglement(List<int> ventesIds, string reference)
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.ventes.Where(v =>
                        v.MutuelId == _mutuelleSelectionneeId
                        && v.Statut == "Active"
                        && !v.MutuelleReglee
                        && v.MontantMutuelle > 0);

                    if (ventesIds != null)
                        query = query.Where(v => ventesIds.Contains(v.IdVente));

                    var ventes = query.ToList();
                    decimal totalRegle = 0;

                    foreach (var vente in ventes)
                    {
                        vente.MutuelleReglee = true;

                        ctx.MutuelPaiements.Add(new MutuelPaiement
                        {
                            NumeroPaiement = "MP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            DatePaiement = DateTime.Now,
                            Montant = vente.MontantMutuelle,
                            Reference = reference,
                            Commentaire = $"Règlement {_mutuelleSelectionneeNom}",
                            VenteId = vente.IdVente,
                            MutuelId = _mutuelleSelectionneeId,
                            UserId = SessionUtilisateur.IdCourant
                        });

                        totalRegle += vente.MontantMutuelle;
                    }

                    ctx.SaveChanges();

                    MessageBox.Show(
                        $"✅ Règlement enregistré !\n\n" +
                        $"Mutuelle       : {_mutuelleSelectionneeNom}\n" +
                        $"Ventes réglées : {ventes.Count}\n" +
                        $"Montant total  : {totalRegle:N0} KMF" +
                        (string.IsNullOrEmpty(reference) ? "" : $"\nRéférence      : {reference}"),
                        "Règlement effectué",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ChargerMutuelles();
                ChargerVentesImpayees(_mutuelleSelectionneeId, _mutuelleSelectionneeNom);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string AskReference()
        {
            using (var frm = new Form())
            {
                frm.Text = "Référence du paiement";
                frm.ClientSize = new System.Drawing.Size(400, 130);
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.MaximizeBox = false;
                frm.BackColor = System.Drawing.Color.White;

                var lbl = new Label
                {
                    Text = "Référence (virement, chèque…) — optionnel :",
                    Location = new System.Drawing.Point(12, 14),
                    Size = new System.Drawing.Size(376, 20),
                    Font = new System.Drawing.Font("Segoe UI", 9F)
                };
                var txt = new TextBox
                {
                    Location = new System.Drawing.Point(12, 38),
                    Size = new System.Drawing.Size(376, 28),
                    PlaceholderText = "Ex : VIR-2026-001, CHQ-123456…",
                    Font = new System.Drawing.Font("Segoe UI", 9.5F)
                };
                var btnOk = new Button
                {
                    Text = "Confirmer",
                    Location = new System.Drawing.Point(272, 80),
                    Size = new System.Drawing.Size(116, 32),
                    BackColor = System.Drawing.Color.FromArgb(46, 125, 50),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };
                btnOk.FlatAppearance.BorderSize = 0;

                frm.Controls.AddRange(new Control[] { lbl, txt, btnOk });
                frm.AcceptButton = btnOk;
                frm.ShowDialog();
                return txt.Text.Trim();
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CRUD MUTUELLES
        // ══════════════════════════════════════════════════════════════════

        private void btnNouvelleMutuelle_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddMutuelle())
                if (form.ShowDialog() == DialogResult.OK)
                    ChargerMutuelles();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgvMutuelles.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvMutuelles.SelectedRows[0].Cells["IdMutuel"].Value);
            using (var ctx = new AppDbContext())
            {
                var m = ctx.mutuels.Find(id);
                if (m == null) return;
                using (var form = new FormAddMutuelle(m))
                    if (form.ShowDialog() == DialogResult.OK)
                        ChargerMutuelles();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (ArchivageUi.Archiver(TypeElement.Mutuelle, SelectionMutuelle()))
                ChargerMutuelles();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvMutuelles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Sélectionnez une mutuelle avant d'exporter.",
                    "Export Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int id = Convert.ToInt32(dgvMutuelles.SelectedRows[0].Cells["IdMutuel"].Value);
            string nom = dgvMutuelles.SelectedRows[0].Cells["Mutuelle"].Value?.ToString() ?? "Mutuelle";

            using (var frmP = new FormPeriodeExportMutuelle(nom))
            {
                if (frmP.ShowDialog() != DialogResult.OK) return;
                var donnees = MutuelleExportService.GetDonnees(id, frmP.DateDebut, frmP.DateFin);
                if (donnees.Count == 0)
                {
                    MessageBox.Show($"Aucune vente trouvée pour « {nom} » sur cette période.",
                        "Export Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Fichier Excel (*.xlsx)|*.xlsx";
                    sfd.FileName = $"Mutuelle_{nom.Replace(" ", "_")}_{frmP.DateDebut:yyyy_MM}.xlsx";
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    try
                    {
                        MutuelleExportService.Exporter(sfd.FileName, id, nom, frmP.DateDebut, frmP.DateFin);
                        if (MessageBox.Show($"Export réussi : {donnees.Count} achat(s).\n\nOuvrir ?",
                            "Export Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            System.Diagnostics.Process.Start(
                                new System.Diagnostics.ProcessStartInfo
                                { FileName = sfd.FileName, UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur : " + ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}