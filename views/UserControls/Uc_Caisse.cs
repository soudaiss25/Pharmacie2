using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Caisse : UserControl
    {
        private int _sessionSelectionneeId = -1;
        private static readonly string[] MobileTypes = { ModesPaiement.Mvola, ModesPaiement.HuriMoney };

        public Uc_Caisse()
        {
            InitializeComponent();

            this.Load += (s, e) => { cbPeriode.SelectedIndex = 0; };

            // Recharger seulement si ce n'est PAS "Personnalisé"
            // Pour Personnalisé, on attend que l'utilisateur clique Actualiser
            cbPeriode.SelectedIndexChanged += (s, e) =>
            {
                if (cbPeriode.SelectedItem?.ToString() != "Personnalisé")
                    ChargerRapport();
            };

            // Pour Personnalisé, recharger quand les dates changent (après un délai)
            dtpDebut.ValueChanged += (s, e) =>
            {
                if (cbPeriode.SelectedItem?.ToString() == "Personnalisé")
                    ChargerRapport();
            };
            dtpFin.ValueChanged += (s, e) =>
            {
                if (cbPeriode.SelectedItem?.ToString() == "Personnalisé")
                    ChargerRapport();
            };

            cbFiltreAnnee.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreMois.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreJour.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreUser.SelectedIndexChanged += (s, e) => ChargerSessions();
            dgvSessions.SelectionChanged += DgvSessions_SelectionChanged;

            InitFiltresCombos();
            ChargerSessions();

            // ✅ Se rafraîchir automatiquement quand une vente est modifiée/payée
            Uc_Vente.VenteModifiee += (s, e) =>
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => ChargerRapport()));
            };
        }

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 1 — RAPPORT DE CAISSE
        // ══════════════════════════════════════════════════════════════════

        private void ChargerRapport()
        {
            var (debut, fin) = GetPeriode();
            lblPeriodeAffichee.Text = $"Période : {debut:dd/MM/yyyy}  →  {fin:dd/MM/yyyy}";

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var ventes = ctx.ventes
                        .Include(v => v.User)
                        .Where(v => v.DateVente >= debut && v.DateVente <= fin)
                        .ToList();

                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();
                    var idsActives = ventesActives.Select(v => v.IdVente).ToHashSet();

                    var paiements = ctx.paiement
                        .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                        .ToList();

                    // ── Calculs globaux ────────────────────────────────────
                    // totalEncaisse = TOUS les paiements reçus (y compris avances crédit)
                    decimal totalEncaisse = paiements
                        .Where(p => idsActives.Contains(p.VenteId))
                        .Sum(p => (decimal)p.Montant);

                    // Rendu supprimé du calcul — déjà inclus dans MontantTotal

                    // ── TOTAL ATTENDU EN CAISSE = espèces physiques reçues ──────────
                    // Comptant : espèces brutes reçues - rendu
                    // ── CE QUI EST PHYSIQUEMENT EN CAISSE ────────────────────
                    //
                    // 1. Ventes Comptant → MontantTotal encaissé
                    decimal caisseComptant = ventesActives
                        .Where(v => v.Type == "Comptant")
                        .Sum(v => (decimal)v.MontantTotal);

                    // 2. Ventes Mutuelle → part patient versée en espèces
                    //    = MontantTotal - MontantMutuelle
                    decimal caissePatientMutuelle = ventesActives
                        .Where(v => v.Type == "Mutuelle")
                        .Sum(v => (decimal)(v.MontantTotal - v.MontantMutuelle));

                    // 3. Ventes Mutuelle → part entreprise si déjà réglée
                    decimal caisseEntrepriseMutuelle = ventesActives
                        .Where(v => v.Type == "Mutuelle" && v.MutuelleReglee)
                        .Sum(v => (decimal)v.MontantMutuelle);

                    // 4. Ventes Crédit → avances versées en espèces
                    var idsCredit2 = ventesActives
                        .Where(v => v.Type == "Crédit")
                        .Select(v => v.IdVente).ToHashSet();
                    decimal avancesEnCaisse = paiements
                        .Where(p => idsCredit2.Contains(p.VenteId))
                        .Sum(p => (decimal)p.Montant);

                    // Total physique réel en caisse
                    decimal totalCaisse = caisseComptant
                                        + caissePatientMutuelle
                                        + caisseEntrepriseMutuelle
                                        + avancesEnCaisse;
                    decimal caTotal = ventesActives.Sum(v => (decimal)v.MontantTotal);

                    // Ventes électroniques (CB + Chèque + Mobile) — non incluses dans le total physique
                    // Affichées entre parenthèses pour rappel
                    decimal totalElectronique = ventesActives
                        .Where(v => v.Type == "Carte bancaire"
                                 || v.Type == "Chèque"
                                 || MobileTypes.Contains(v.Type))
                        .Sum(v => (decimal)v.MontantTotal);

                    // ── Ventilation par mode de paiement ──────────────────
                    // Montants facturés par type
                    decimal caEspeces = ventesActives.Where(v => v.Type == "Comptant").Sum(v => (decimal)v.MontantTotal);
                    decimal caCheque = ventesActives.Where(v => v.Type == "Chèque").Sum(v => (decimal)v.MontantTotal);
                    decimal caCB = ventesActives.Where(v => v.Type == "Carte bancaire").Sum(v => (decimal)v.MontantTotal);
                    decimal caMobileMoney = ventesActives.Where(v => MobileTypes.Contains(v.Type)).Sum(v => (decimal)v.MontantTotal);
                    decimal caMutuelle = ventesActives.Where(v => v.Type == "Mutuelle").Sum(v => (decimal)v.MontantTotal);
                    decimal caMutuelleEnt = ventesActives.Where(v => v.Type == "Mutuelle").Sum(v => (decimal)v.MontantMutuelle);
                    decimal especesRecues = ventesActives.Where(v => v.Type == "Comptant").Sum(v => (decimal)v.MontantEspeces);

                    // Pour le crédit : montant total facturé ET avances déjà reçues
                    decimal caCredit = ventesActives.Where(v => v.Type == "Crédit").Sum(v => (decimal)v.MontantTotal);
                    // Avances crédit = paiements reçus sur ventes de type Crédit
                    var idsCredit = ventesActives.Where(v => v.Type == "Crédit").Select(v => v.IdVente).ToHashSet();
                    decimal avancesCredit = paiements.Where(p => idsCredit.Contains(p.VenteId)).Sum(p => (decimal)p.Montant);
                    // Reste impayé crédit = total facturé - avances déjà reçues
                    decimal resteCredit = ventesActives.Where(v => v.Type == "Crédit").Sum(v => v.MontantRestant);

                    // ── UI Labels ──────────────────────────────────────────
                    lblNbVentes.Text = $"{ventesActives.Count}";
                    lblCaComptant.Text = $"{caEspeces:N0} KMF";
                    lblCaCredit.Text = $"{caCredit:N0} KMF\n(avances: {avancesCredit:N0})";
                    lblCaMutuelle.Text = $"{caMutuelle:N0} KMF";
                    lblCaCB.Text = $"{caCB:N0} KMF";
                    lblCaCheque.Text = $"{caCheque:N0} KMF";
                    // Panneau "Ce qui doit être en caisse"
                    lblEspecesRecues.Text = $"{caisseComptant:N0} KMF";

                    // Part patient mutuelle (espèces reçues du patient)
                    lblRendu.Text = $"{caissePatientMutuelle:N0} KMF";

                    // Avances crédit reçues en espèces
                    lblCreditRecu.Text = $"{avancesEnCaisse:N0} KMF";

                    // Part entreprise non encore réglée (info — pas en caisse)
                    decimal partEntrepriseEnAttente = caMutuelleEnt - caisseEntrepriseMutuelle;
                    lblMutuellePatient.Text = partEntrepriseEnAttente > 0
                        ? $"{partEntrepriseEnAttente:N0} KMF (non encaissé)"
                        : "Tout réglé ✅";
                    // Total + mention électronique entre parenthèses
                    lblTotalCaisse.Text = $"{totalCaisse:N0} KMF";
                    if (totalElectronique > 0)
                        lblTotalCaisse.Text += $"\n(+ {totalElectronique:N0} KMF en CB/Chèque/Mobile)";
                    lblTotalCaisse.ForeColor = totalCaisse >= 0
                        ? Color.FromArgb(27, 94, 32) : Color.OrangeRed;

                    // ── Grille ventilation ────────────────────────────────
                    var data = new[]
                    {
                        new { Mode = "💵 Espèces (Comptant)",          NbVentes = ventesActives.Count(v => v.Type == "Comptant"),           Montant = caEspeces,     PctCA = Pct(caEspeces, caTotal),     Statut = "✅ Physique en caisse" },
                        new { Mode = "📱 Mobile Money (Mvola / Huri)",  NbVentes = ventesActives.Count(v => MobileTypes.Contains(v.Type)),   Montant = caMobileMoney, PctCA = Pct(caMobileMoney, caTotal), Statut = "📲 Électronique" },
                        new { Mode = "💳 Carte bancaire",               NbVentes = ventesActives.Count(v => v.Type == "Carte bancaire"),     Montant = caCB,          PctCA = Pct(caCB, caTotal),          Statut = "📲 Électronique" },
                        new { Mode = "📄 Chèque",                       NbVentes = ventesActives.Count(v => v.Type == "Chèque"),             Montant = caCheque,      PctCA = Pct(caCheque, caTotal),      Statut = "🏦 À déposer en banque" },
                        new { Mode = "🏥 Mutuelle (total facturé)",     NbVentes = ventesActives.Count(v => v.Type == "Mutuelle"),           Montant = caMutuelle,    PctCA = Pct(caMutuelle, caTotal),    Statut = "⏳ Part ent. en attente" },
                        new { Mode = "📋 Crédit — facturé",            NbVentes = ventesActives.Count(v => v.Type == "Crédit"),             Montant = caCredit,      PctCA = Pct(caCredit, caTotal),      Statut = $"💵 Avances: {avancesCredit:N0} | Reste: {resteCredit:N0}" },
                    };

                    dgvVentilation.DataSource = null;
                    dgvVentilation.DataSource = data;

                    if (dgvVentilation.Columns["Mode"] != null) dgvVentilation.Columns["Mode"].HeaderText = "Mode de paiement";
                    if (dgvVentilation.Columns["NbVentes"] != null) dgvVentilation.Columns["NbVentes"].HeaderText = "Nb ventes";
                    if (dgvVentilation.Columns["Montant"] != null) dgvVentilation.Columns["Montant"].HeaderText = "Montant (KMF)";
                    if (dgvVentilation.Columns["PctCA"] != null) dgvVentilation.Columns["PctCA"].HeaderText = "% du CA";
                    if (dgvVentilation.Columns["Statut"] != null) dgvVentilation.Columns["Statut"].HeaderText = "Statut";

                    ColorerVentilation(dgvVentilation);

                    // ── Grille détail ─────────────────────────────────────
                    dgvDetail.DataSource = null;
                    dgvDetail.DataSource = ventes
                        .OrderByDescending(v => v.DateVente)
                        .Select(v => new
                        {
                            Date = v.DateVente.ToString("dd/MM/yyyy HH:mm"),
                            Numéro = v.numeroVente,
                            Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                            Motif = v.MotifAchat ?? "—",
                            Type = v.Type,
                            Statut = v.Statut,
                            Total = v.MontantTotal,
                            Espèces = v.MontantEspeces,
                            Rendu = v.MontantRendu,
                            Vendeur = v.User != null ? $"{v.User.Prenom} {v.User.Nom}" : "—"
                        }).ToList();

                    ColorerGrille(dgvDetail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur rapport : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static decimal Pct(decimal valeur, decimal total)
            => total > 0 ? Math.Round(valeur / total * 100, 1) : 0m;

        private static void ColorerVentilation(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string mode = row.Cells["Mode"].Value?.ToString() ?? "";
                row.DefaultCellStyle.BackColor = mode switch
                {
                    var m when m.StartsWith("💵") => Color.FromArgb(232, 245, 233),
                    var m when m.StartsWith("📱") => Color.FromArgb(227, 242, 253),
                    var m when m.StartsWith("💳") => Color.FromArgb(243, 229, 245),
                    var m when m.StartsWith("📄") => Color.FromArgb(255, 253, 231),
                    var m when m.StartsWith("🏥") => Color.FromArgb(225, 245, 254),
                    var m when m.StartsWith("📋") => Color.FromArgb(255, 235, 238),
                    _ => Color.White
                };
            }
        }

        private (DateTime debut, DateTime fin) GetPeriode()
        {
            DateTime now = DateTime.Now;
            switch (cbPeriode.SelectedItem?.ToString())
            {
                case "Aujourd'hui":
                    return (now.Date, now.Date.AddDays(1).AddSeconds(-1));
                case "Cette semaine":
                    int diff = (int)now.DayOfWeek - (int)DayOfWeek.Monday;
                    if (diff < 0) diff += 7;
                    var lundi = now.Date.AddDays(-diff);
                    return (lundi, lundi.AddDays(7).AddSeconds(-1));
                case "Ce mois":
                    return (new DateTime(now.Year, now.Month, 1),
                            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddSeconds(-1));
                case "Personnalisé":
                    DateTime pDebut = dtpDebut.Value.Date;
                    DateTime pFin = dtpFin.Value.Date.AddDays(1).AddSeconds(-1);
                    // Sécurité : si début > fin, inverser
                    if (pDebut > pFin) (pDebut, pFin) = (pFin, pDebut);
                    return (pDebut, pFin);
                default:
                    return (now.Date, now.Date.AddDays(1).AddSeconds(-1));
            }
        }

        private void btnActualiser_Click(object sender, EventArgs e) => ChargerRapport();

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 2 — SESSIONS CAISSE
        // ══════════════════════════════════════════════════════════════════

        private void InitFiltresCombos()
        {
            cbFiltreAnnee.Items.Add("Toutes");
            for (int a = DateTime.Now.Year; a >= DateTime.Now.Year - 4; a--)
                cbFiltreAnnee.Items.Add(a.ToString());
            cbFiltreAnnee.SelectedIndex = 0;

            cbFiltreMois.Items.Add("Tous");
            foreach (var m in new[] { "Janvier","Février","Mars","Avril","Mai","Juin",
                                       "Juillet","Août","Septembre","Octobre","Novembre","Décembre" })
                cbFiltreMois.Items.Add(m);
            cbFiltreMois.SelectedIndex = 0;

            cbFiltreJour.Items.Add("Tous");
            for (int j = 1; j <= 31; j++) cbFiltreJour.Items.Add(j.ToString());
            cbFiltreJour.SelectedIndex = 0;

            ChargerFiltreUsers();
        }

        private void ChargerFiltreUsers()
        {
            cbFiltreUser.Items.Clear();
            cbFiltreUser.Items.Add("Tous");
            using (var ctx = new AppDbContext())
            {
                var users = ctx.Users
                    .Where(u => u.Role == Roles.Caissier || u.Role == Roles.Pharmacien)
                    .OrderBy(u => u.Nom).ToList();
                foreach (var u in users)
                    cbFiltreUser.Items.Add(new UserItem(u.Id, $"{u.Prenom} {u.Nom}"));
            }
            cbFiltreUser.SelectedIndex = 0;
        }

        private void ChargerSessions()
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var query = ctx.SessionsCaisse.Include(s => s.User).AsQueryable();

                    if (cbFiltreAnnee.SelectedItem?.ToString() != "Toutes"
                        && int.TryParse(cbFiltreAnnee.SelectedItem?.ToString(), out int annee))
                        query = query.Where(s => s.DateOuverture.Year == annee);

                    int moisIdx = cbFiltreMois.SelectedIndex;
                    if (moisIdx > 0)
                        query = query.Where(s => s.DateOuverture.Month == moisIdx);

                    if (cbFiltreJour.SelectedItem?.ToString() != "Tous"
                        && int.TryParse(cbFiltreJour.SelectedItem?.ToString(), out int jour))
                        query = query.Where(s => s.DateOuverture.Day == jour);

                    if (cbFiltreUser.SelectedItem is UserItem ui)
                        query = query.Where(s => s.UserId == ui.Id);

                    var sessions = query.OrderByDescending(s => s.DateOuverture).ToList();
                    dgvSessions.Rows.Clear();

                    foreach (var s in sessions)
                    {
                        DateTime debut = s.DateOuverture;
                        DateTime fin = s.DateCloture ?? DateTime.Now;

                        var ventesSession = ctx.ventes
                            .Where(v => v.UserId == s.UserId
                                     && v.DateVente >= debut
                                     && v.DateVente <= fin
                                     && v.Statut == "Active")
                            .ToList();

                        var ids = ventesSession.Select(v => v.IdVente).ToHashSet();
                        var paiementsSession = ctx.paiement
                            .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                            .ToList();

                        decimal encaisse = paiementsSession.Where(p => ids.Contains(p.VenteId)).Sum(p => (decimal)p.Montant);
                        decimal rendu = ventesSession.Where(v => v.Type == "Comptant").Sum(v => (decimal)v.MontantRendu);
                        decimal theorique = s.FondOuverture + encaisse - rendu;
                        decimal? ecart = s.MontantCompteCloture.HasValue ? s.MontantCompteCloture.Value - theorique : (decimal?)null;
                        string ecartStr = ecart.HasValue ? (ecart >= 0 ? $"+{ecart:N0}" : $"{ecart:N0}") : "—";

                        int idx = dgvSessions.Rows.Add(
                            s.Id,
                            s.User != null ? $"{s.User.Prenom} {s.User.Nom}" : "—",
                            s.DateOuverture.ToString("dd/MM/yyyy HH:mm"),
                            s.DateCloture.HasValue ? s.DateCloture.Value.ToString("dd/MM/yyyy HH:mm") : "En cours",
                            s.Statut,
                            $"{s.FondOuverture:N0}",
                            $"{encaisse:N0}",
                            $"{theorique:N0}",
                            s.MontantCompteCloture.HasValue ? $"{s.MontantCompteCloture:N0}" : "—",
                            ecartStr,
                            ventesSession.Count.ToString()
                        );

                        if (s.Statut == "Ouverte")
                        {
                            dgvSessions.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                            dgvSessions.Rows[idx].DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32);
                        }
                        if (ecart.HasValue && ecart < 0)
                        {
                            dgvSessions.Rows[idx].Cells["colEcart"].Style.ForeColor = Color.OrangeRed;
                            dgvSessions.Rows[idx].Cells["colEcart"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                    }

                    lblNbSessions.Text =
                        $"{sessions.Count} session(s) — {sessions.Count(s => s.Statut == "Ouverte")} ouverte(s)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur sessions : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSessions_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSessions.SelectedRows.Count == 0) return;
            _sessionSelectionneeId = Convert.ToInt32(dgvSessions.SelectedRows[0].Cells["colSessionId"].Value);
            ChargerDetailSession(_sessionSelectionneeId);
        }

        private void ChargerDetailSession(int sessionId)
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var session = ctx.SessionsCaisse.Include(s => s.User).FirstOrDefault(s => s.Id == sessionId);
                    if (session == null) return;

                    DateTime debut = session.DateOuverture;
                    DateTime fin = session.DateCloture ?? DateTime.Now;

                    var ventes = ctx.ventes
                        .Include(v => v.Paiements)
                        .Where(v => v.UserId == session.UserId && v.DateVente >= debut && v.DateVente <= fin)
                        .OrderByDescending(v => v.DateVente)
                        .ToList();

                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();

                    // ── Ventilation de la session ──────────────────────────
                    decimal sesEspeces = ventesActives.Where(v => v.Type == "Comptant").Sum(v => (decimal)v.MontantTotal);
                    decimal sesMobile = ventesActives.Where(v => MobileTypes.Contains(v.Type)).Sum(v => (decimal)v.MontantTotal);
                    decimal sesCB = ventesActives.Where(v => v.Type == "Carte bancaire").Sum(v => (decimal)v.MontantTotal);
                    decimal sesCheque = ventesActives.Where(v => v.Type == "Chèque").Sum(v => (decimal)v.MontantTotal);
                    decimal sesMutuelle = ventesActives.Where(v => v.Type == "Mutuelle").Sum(v => (decimal)v.MontantTotal);
                    decimal sesCredit = ventesActives.Where(v => v.Type == "Crédit").Sum(v => (decimal)v.MontantTotal);

                    // Grille détail
                    dgvDetailSession.DataSource = null;
                    dgvDetailSession.DataSource = ventes.Select(v => new
                    {
                        Heure = v.DateVente.ToString("HH:mm:ss"),
                        Numéro = v.numeroVente,
                        Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                        Type = v.Type,
                        Statut = v.Statut,
                        Total = v.MontantTotal,
                        Versé = v.MontantVerse,
                        Reste = v.MontantRestant
                    }).ToList();

                    ColorerGrilleSession(dgvDetailSession);

                    // Calculs théorique / écart
                    var ids = ventesActives.Select(v => v.IdVente).ToHashSet();
                    var paiements = ctx.paiement.Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin).ToList();
                    decimal encaisse = paiements.Where(p => ids.Contains(p.VenteId)).Sum(p => (decimal)p.Montant);
                    decimal rendu = ventesActives.Where(v => v.Type == "Comptant").Sum(v => (decimal)v.MontantRendu);
                    decimal theorique = session.FondOuverture + encaisse - rendu;
                    decimal? ecart = session.MontantCompteCloture.HasValue ? session.MontantCompteCloture.Value - theorique : (decimal?)null;

                    string caissier = session.User != null ? $"{session.User.Prenom} {session.User.Nom}" : "—";

                    lblDetailSession.Text =
                        $"👤 {caissier}  |  📅 {debut:dd/MM/yyyy HH:mm} → " +
                        $"{(session.DateCloture.HasValue ? session.DateCloture.Value.ToString("HH:mm") : "en cours")}  |  " +
                        $"Fond : {session.FondOuverture:N0} KMF  |  Encaissé : {encaisse:N0} KMF  |  " +
                        $"Théorique : {theorique:N0} KMF" +
                        (ecart.HasValue ? $"  |  Écart : {(ecart >= 0 ? "+" : "")}{ecart:N0} KMF" : "");

                    lblDetailSession.ForeColor = ecart.HasValue && ecart < 0
                        ? Color.OrangeRed : Color.FromArgb(27, 94, 32);

                    // ── Ventilation en une ligne sous le récap ─────────────
                    lblVentilationSession.Text =
                        $"💵 Espèces : {sesEspeces:N0}  |  " +
                        $"📱 Mobile : {sesMobile:N0}  |  " +
                        $"💳 CB : {sesCB:N0}  |  " +
                        $"📄 Chèque : {sesCheque:N0}  |  " +
                        $"🏥 Mutuelle : {sesMutuelle:N0}  |  " +
                        $"📋 Crédit : {sesCredit:N0}   (KMF)";
                    lblVentilationSession.ForeColor = Color.FromArgb(25, 118, 210);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur détail session : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualiserSessions_Click(object sender, EventArgs e) => ChargerSessions();

        // ── Helpers coloriage ─────────────────────────────────────────────

        private static Color CouleurType(string type) => type switch
        {
            "Comptant" => Color.ForestGreen,
            "Crédit" => Color.OrangeRed,
            "Mutuelle" => Color.SteelBlue,
            "Chèque" => Color.FromArgb(180, 140, 0),
            "Carte bancaire" => Color.FromArgb(106, 27, 154),
            ModesPaiement.Mvola => Color.FromArgb(0, 120, 180),
            ModesPaiement.HuriMoney => Color.FromArgb(0, 120, 180),
            _ => Color.Gray
        };

        private static void ColorerGrille(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Type"]?.Value == null) continue;
                string type = row.Cells["Type"].Value.ToString();
                string statut = row.Cells["Statut"]?.Value?.ToString() ?? "";
                row.Cells["Type"].Style.BackColor = CouleurType(type);
                row.Cells["Type"].Style.ForeColor = Color.White;
                row.Cells["Type"].Style.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                if (statut == "Annulée")
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 160, 160);
            }
        }

        private static void ColorerGrilleSession(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Type"]?.Value == null) continue;
                string type = row.Cells["Type"].Value.ToString();
                string statut = row.Cells["Statut"]?.Value?.ToString() ?? "";
                row.Cells["Type"].Style.BackColor = CouleurType(type);
                row.Cells["Type"].Style.ForeColor = Color.White;
                row.Cells["Type"].Style.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                if (statut == "Annulée")
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 160, 160);
            }
        }
    }

    internal class UserItem
    {
        public int Id { get; }
        public string Nom { get; }
        public UserItem(int id, string nom) { Id = id; Nom = nom; }
        public override string ToString() => Nom;
    }
}