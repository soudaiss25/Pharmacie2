using Pharmacie2.views.Composants;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Caisse : UserControl, IModeCompact
    {
        private int _sessionSelectionneeId = -1;

        public Uc_Caisse()
        {
            InitializeComponent();
            ModeCompact.CartesAdaptatives(pnlCartes);
            dgvDetail.Resize += (s, e) => AppliquerColonnesDetail();
            Theme.Appliquer(this);

            InitFiltresCombos();
            cbPeriode.SelectedIndex = 0;
            ToggleDatesPersonnalisees();

            // Recharger seulement si ce n'est PAS « Personnalisé » (il faut alors cliquer sur Actualiser)
            cbPeriode.SelectedIndexChanged += (s, e) =>
            {
                ToggleDatesPersonnalisees();
                if (cbPeriode.SelectedItem?.ToString() != "Personnalisé")
                    ChargerRapport();
            };
            dtpDebut.ValueChanged += (s, e) => { if (cbPeriode.SelectedItem?.ToString() == "Personnalisé") ChargerRapport(); };
            dtpFin.ValueChanged += (s, e) => { if (cbPeriode.SelectedItem?.ToString() == "Personnalisé") ChargerRapport(); };

            cbFiltreAnnee.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreMois.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreJour.SelectedIndexChanged += (s, e) => ChargerSessions();
            cbFiltreUser.SelectedIndexChanged += (s, e) => ChargerSessions();

            // Se rafraîchit automatiquement quand une vente est créée, modifiée, payée ou annulée
            VenteEvenements.VenteModifiee += SurVenteModifiee;
            Disposed += (s, e) => VenteEvenements.VenteModifiee -= SurVenteModifiee;

            ChargerRapport();
            ChargerSessions();
        }

        private void SurVenteModifiee(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired) BeginInvoke(new Action(ChargerRapport)); else ChargerRapport();
        }

        private void ToggleDatesPersonnalisees()
        {
            bool perso = cbPeriode.SelectedItem?.ToString() == "Personnalisé";
            dtpDebut.Visible = perso;
            dtpFin.Visible = perso;
        }

        // ══════════════════════════════════════════════════════════════════
        // ONGLET 1 — RAPPORT DE CAISSE
        // ══════════════════════════════════════════════════════════════════

        private void ChargerRapport()
        {
            var (debut, fin) = GetPeriode();
            lblPeriodeAffichee.Text = $"Du {debut:dd/MM/yyyy} au {fin:dd/MM/yyyy}";

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var ventes = ctx.ventes
                        .AsNoTracking()
                        .Include(v => v.User)
                        .Include(v => v.Paiements)
                        .Where(v => v.DateVente >= debut && v.DateVente <= fin)
                        .ToList();

                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();
                    var idsActives = ventesActives.Select(v => v.IdVente).ToHashSet();

                    var paiements = ctx.paiement
                        .AsNoTracking()
                        .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                        .ToList();

                    // ── CE QUI EST PHYSIQUEMENT EN CAISSE ────────────────────
                    // 1. Ventes comptant → montant total encaissé
                    decimal caisseComptant = ventesActives.Where(v => v.Type == ModesPaiement.Comptant).Sum(v => v.MontantTotal);

                    // 2. Ventes mutuelle → part du patient versée en espèces
                    decimal caissePatientMutuelle = ventesActives.Where(v => v.Type == ModesPaiement.Mutuelle).Sum(v => v.MontantTotal - v.MontantMutuelle);

                    // 3. Ventes mutuelle → part de l'entreprise si déjà réglée
                    decimal caisseEntrepriseMutuelle = ventesActives.Where(v => v.Type == ModesPaiement.Mutuelle && v.MutuelleReglee).Sum(v => v.MontantMutuelle);

                    // 4. Ventes à crédit → versements reçus
                    var idsCredit = ventesActives.Where(v => v.Type == ModesPaiement.Credit).Select(v => v.IdVente).ToHashSet();
                    decimal avancesCredit = paiements.Where(p => idsCredit.Contains(p.VenteId)).Sum(p => p.Montant);

                    decimal totalCaisse = caisseComptant + caissePatientMutuelle + caisseEntrepriseMutuelle + avancesCredit;
                    decimal caTotal = ventesActives.Sum(v => v.MontantTotal);

                    // Ventes électroniques (carte, chèque, mobile) : pas dans le tiroir
                    decimal totalElectronique = ventesActives
                        .Where(v => v.Type == ModesPaiement.CarteBancaire || v.Type == ModesPaiement.Cheque || ModesPaiement.EstMobile(v.Type))
                        .Sum(v => v.MontantTotal);

                    // ── Ventilation par moyen de paiement (montants facturés) ──
                    decimal caEspeces = caisseComptant;
                    decimal caCheque = ventesActives.Where(v => v.Type == ModesPaiement.Cheque).Sum(v => v.MontantTotal);
                    decimal caCB = ventesActives.Where(v => v.Type == ModesPaiement.CarteBancaire).Sum(v => v.MontantTotal);
                    decimal caMobileMoney = ventesActives.Where(v => ModesPaiement.EstMobile(v.Type)).Sum(v => v.MontantTotal);
                    decimal caMutuelle = ventesActives.Where(v => v.Type == ModesPaiement.Mutuelle).Sum(v => v.MontantTotal);
                    decimal caMutuelleEnt = ventesActives.Where(v => v.Type == ModesPaiement.Mutuelle).Sum(v => v.MontantMutuelle);
                    decimal caCredit = ventesActives.Where(v => v.Type == ModesPaiement.Credit).Sum(v => v.MontantTotal);
                    decimal resteCredit = ventesActives.Where(v => v.Type == ModesPaiement.Credit).Sum(v => v.MontantRestant);

                    // ── Cartes ─────────────────────────────────────────────
                    cardVentes.Valeur = ventesActives.Count.ToString();
                    cardVentes.Detail = "pour " + Format.Montant(caTotal);
                    cardComptant.Valeur = Format.Montant(caEspeces);
                    cardComptant.Detail = $"{Format.Compte(ventesActives.Count(v => v.Type == ModesPaiement.Comptant), "vente")}";
                    cardCredit.Valeur = Format.Montant(caCredit);
                    cardCredit.Detail = $"versé : {Format.Montant(avancesCredit)}";
                    cardCredit.Niveau = resteCredit > 0 ? "attention" : "succes";
                    cardMutuelle.Valeur = Format.Montant(caMutuelle);
                    cardMutuelle.Detail = $"part des entreprises : {Format.Montant(caMutuelleEnt)}";
                    cardCB.Valeur = Format.Montant(caCB);
                    cardCB.Detail = $"{Format.Compte(ventesActives.Count(v => v.Type == ModesPaiement.CarteBancaire), "vente")}";
                    cardCheque.Valeur = Format.Montant(caCheque);
                    cardCheque.Detail = "à déposer en banque";

                    // ── Ce qui doit être dans le tiroir ────────────────────
                    lblEspecesRecues.Text = Format.Montant(caisseComptant);
                    lblRendu.Text = Format.Montant(caissePatientMutuelle);
                    lblCreditRecu.Text = Format.Montant(avancesCredit);
                    decimal partEntrepriseEnAttente = caMutuelleEnt - caisseEntrepriseMutuelle;
                    lblMutuellePatient.Text = partEntrepriseEnAttente > 0
                        ? $"{Format.Montant(partEntrepriseEnAttente)} (pas encore reçu)"
                        : "Tout est réglé";
                    lblMutuellePatient.ForeColor = partEntrepriseEnAttente > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;
                    lblTotalCaisse.Text = Format.Montant(totalCaisse);
                    lblElectronique.Text = totalElectronique > 0
                        ? $"En plus : {Format.Montant(totalElectronique)} payés par carte, chèque ou mobile (hors tiroir)"
                        : "";

                    // ── Grille ventilation ────────────────────────────────
                    dgvVentilation.DataSource = new[]
                    {
                        new { Mode = "Espèces (comptant)",             NbVentes = ventesActives.Count(v => v.Type == ModesPaiement.Comptant),        Montant = caEspeces,     PctCA = Pct(caEspeces, caTotal),     Statut = "Dans le tiroir" },
                        new { Mode = "Mvola / Huri Money",             NbVentes = ventesActives.Count(v => ModesPaiement.EstMobile(v.Type)),         Montant = caMobileMoney, PctCA = Pct(caMobileMoney, caTotal), Statut = "Électronique" },
                        new { Mode = "Carte bancaire",                 NbVentes = ventesActives.Count(v => v.Type == ModesPaiement.CarteBancaire),   Montant = caCB,          PctCA = Pct(caCB, caTotal),          Statut = "Électronique" },
                        new { Mode = "Chèque",                         NbVentes = ventesActives.Count(v => v.Type == ModesPaiement.Cheque),          Montant = caCheque,      PctCA = Pct(caCheque, caTotal),      Statut = "À déposer en banque" },
                        new { Mode = "Mutuelle (total facturé)",       NbVentes = ventesActives.Count(v => v.Type == ModesPaiement.Mutuelle),        Montant = caMutuelle,    PctCA = Pct(caMutuelle, caTotal),    Statut = "Part de l'entreprise en attente" },
                        new { Mode = "Crédit (total facturé)",         NbVentes = ventesActives.Count(v => v.Type == ModesPaiement.Credit),          Montant = caCredit,      PctCA = Pct(caCredit, caTotal),      Statut = $"Versé {Format.Montant(avancesCredit)} — reste {Format.Montant(resteCredit)}" },
                    }.ToList();

                    // ── Grille détail ─────────────────────────────────────
                    dgvDetail.DataSource = ventes
                        .OrderByDescending(v => v.DateVente)
                        .Select(v => new
                        {
                            Date = v.DateVente.ToString("dd/MM/yyyy HH:mm"),
                            Numero = v.numeroVente,
                            Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                            Motif = string.IsNullOrWhiteSpace(v.MotifAchat) ? "—" : v.MotifAchat,
                            v.Type,
                            v.Statut,
                            Total = v.MontantTotal,
                            Especes = v.MontantEspeces,
                            Rendu = v.MontantRendu,
                            Vendeur = v.User != null ? $"{v.User.Prenom} {v.User.Nom}" : "—"
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Rapport de caisse", ex);
                MessageBox.Show($"Le rapport de caisse n'a pas pu être calculé : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static decimal Pct(decimal valeur, decimal total)
            => total > 0 ? Math.Round(valeur / total * 100, 1) : 0m;

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
                    if (pDebut > pFin) (pDebut, pFin) = (pFin, pDebut);   // sécurité : début > fin
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
                    var query = ctx.SessionsCaisse.AsNoTracking().Include(s => s.User).AsQueryable();

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
                    var lignes = new List<object>();

                    foreach (var s in sessions)
                    {
                        DateTime debut = s.DateOuverture;
                        DateTime fin = s.DateCloture ?? DateTime.Now;

                        var ventesSession = ctx.ventes
                            .AsNoTracking()
                            .Where(v => v.UserId == s.UserId && v.DateVente >= debut && v.DateVente <= fin && v.Statut == "Active")
                            .ToList();

                        var ids = ventesSession.Select(v => v.IdVente).ToHashSet();
                        var paiementsSession = ctx.paiement
                            .AsNoTracking()
                            .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                            .ToList();

                        decimal encaisse = paiementsSession.Where(p => ids.Contains(p.VenteId)).Sum(p => p.Montant);
                        decimal rendu = ventesSession.Where(v => v.Type == ModesPaiement.Comptant).Sum(v => v.MontantRendu);
                        decimal theorique = s.FondOuverture + encaisse - rendu;
                        decimal? ecart = s.MontantCompteCloture.HasValue ? s.MontantCompteCloture.Value - theorique : null;

                        lignes.Add(new
                        {
                            SessionId = s.Id,
                            Caissier = s.User != null ? $"{s.User.Prenom} {s.User.Nom}" : "—",
                            Ouverture = s.DateOuverture.ToString("dd/MM/yyyy HH:mm"),
                            Cloture = s.DateCloture.HasValue ? s.DateCloture.Value.ToString("dd/MM/yyyy HH:mm") : "En cours",
                            s.Statut,
                            Fond = s.FondOuverture,
                            Encaisse = encaisse,
                            Theorique = theorique,
                            Compte = s.MontantCompteCloture,
                            Ecart = ecart,
                            NbVentes = ventesSession.Count.ToString()
                        });
                    }

                    dgvSessions.DataSource = lignes;
                    lblNbSessions.Text = $"{Format.Compte(sessions.Count, "session")}, dont {sessions.Count(s => s.Statut == "Ouverte")} en cours";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Sessions de caisse", ex);
                MessageBox.Show($"Les sessions n'ont pas pu être affichées : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSessions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSessions.Rows.Count) return;
            var ligne = dgvSessions.Rows[e.RowIndex];
            if (ligne.Cells["Statut"].Value?.ToString() == "Ouverte")
            {
                e.CellStyle.BackColor = Theme.SuccesFond;
                e.CellStyle.ForeColor = Theme.SuccesTexte;
            }
            if (dgvSessions.Columns[e.ColumnIndex].Name == "Ecart" && ligne.Cells["Ecart"].Value is decimal ecart && ecart < 0)
            {
                e.CellStyle.ForeColor = Theme.UrgentTexte;
                e.CellStyle.Font = Theme.PoliceGrille(dgvSessions, FontStyle.Bold);
            }
        }

        private void DgvSessions_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSessions.SelectedRows.Count == 0) return;
            _sessionSelectionneeId = Convert.ToInt32(dgvSessions.SelectedRows[0].Cells["SessionId"].Value);
            ChargerDetailSession(_sessionSelectionneeId);
        }

        private void ChargerDetailSession(int sessionId)
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    var session = ctx.SessionsCaisse.AsNoTracking().Include(s => s.User).FirstOrDefault(s => s.Id == sessionId);
                    if (session == null) return;

                    DateTime debut = session.DateOuverture;
                    DateTime fin = session.DateCloture ?? DateTime.Now;

                    var ventes = ctx.ventes
                        .AsNoTracking()
                        .Include(v => v.Paiements)
                        .Where(v => v.UserId == session.UserId && v.DateVente >= debut && v.DateVente <= fin)
                        .OrderByDescending(v => v.DateVente)
                        .ToList();

                    var actives = ventes.Where(v => v.Statut == "Active").ToList();

                    decimal sesEspeces = actives.Where(v => v.Type == ModesPaiement.Comptant).Sum(v => v.MontantTotal);
                    decimal sesMobile = actives.Where(v => ModesPaiement.EstMobile(v.Type)).Sum(v => v.MontantTotal);
                    decimal sesCB = actives.Where(v => v.Type == ModesPaiement.CarteBancaire).Sum(v => v.MontantTotal);
                    decimal sesCheque = actives.Where(v => v.Type == ModesPaiement.Cheque).Sum(v => v.MontantTotal);
                    decimal sesMutuelle = actives.Where(v => v.Type == ModesPaiement.Mutuelle).Sum(v => v.MontantTotal);
                    decimal sesCredit = actives.Where(v => v.Type == ModesPaiement.Credit).Sum(v => v.MontantTotal);

                    dgvDetailSession.DataSource = ventes.Select(v => new
                    {
                        Heure = v.DateVente.ToString("HH:mm:ss"),
                        Numero = v.numeroVente,
                        Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                        v.Type,
                        v.Statut,
                        Total = v.MontantTotal,
                        Verse = v.MontantVerse,
                        Reste = v.MontantRestant
                    }).ToList();

                    var ids = actives.Select(v => v.IdVente).ToHashSet();
                    var paiements = ctx.paiement.AsNoTracking().Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin).ToList();
                    decimal encaisse = paiements.Where(p => ids.Contains(p.VenteId)).Sum(p => p.Montant);
                    decimal rendu = actives.Where(v => v.Type == ModesPaiement.Comptant).Sum(v => v.MontantRendu);
                    decimal theorique = session.FondOuverture + encaisse - rendu;
                    decimal? ecart = session.MontantCompteCloture.HasValue ? session.MontantCompteCloture.Value - theorique : null;

                    string caissier = session.User != null ? $"{session.User.Prenom} {session.User.Nom}" : "—";

                    lblDetailSession.Text =
                        $"{caissier} — du {debut:dd/MM/yyyy HH:mm} à " +
                        $"{(session.DateCloture.HasValue ? session.DateCloture.Value.ToString("HH:mm") : "maintenant")} — " +
                        $"fond de caisse {Format.Montant(session.FondOuverture)} — encaissé {Format.Montant(encaisse)} — " +
                        $"attendu {Format.Montant(theorique)}" +
                        (ecart.HasValue ? $" — écart {(ecart >= 0 ? "+" : "")}{Format.Montant(ecart.Value)}" : "");

                    lblDetailSession.ForeColor = ecart.HasValue && ecart < 0 ? Theme.UrgentTexte : Theme.SuccesTexte;

                    lblVentilationSession.Text =
                        $"Espèces : {Format.Montant(sesEspeces)}  |  Mobile : {Format.Montant(sesMobile)}  |  " +
                        $"Carte : {Format.Montant(sesCB)}  |  Chèque : {Format.Montant(sesCheque)}  |  " +
                        $"Mutuelle : {Format.Montant(sesMutuelle)}  |  Crédit : {Format.Montant(sesCredit)}";
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Détail d'une session de caisse", ex);
                MessageBox.Show($"Le détail de la session n'a pas pu être affiché : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualiserSessions_Click(object sender, EventArgs e) => ChargerSessions();

        // ── Coloriage des types de paiement ───────────────────────────────

        private static Color CouleurType(string type) => type switch
        {
            ModesPaiement.Comptant => Theme.Accent,
            ModesPaiement.Credit => Theme.AttentionTexte,
            ModesPaiement.Mutuelle => Theme.Neutre,
            _ => Theme.InfoTexte
        };

        private void ColorerLigneVente(DataGridView grille, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= grille.Rows.Count) return;
            var ligne = grille.Rows[e.RowIndex];

            if (ligne.Cells["Statut"].Value?.ToString() == "Annulée")
            {
                e.CellStyle.ForeColor = Theme.Neutre;
                e.CellStyle.BackColor = Theme.InfoFond;
            }
            else if (grille.Columns[e.ColumnIndex].Name == "Type" && ligne.Cells["Type"].Value is string type)
            {
                e.CellStyle.BackColor = CouleurType(type);
                e.CellStyle.ForeColor = Theme.Blanc;
            }
        }

        private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) => ColorerLigneVente(dgvDetail, e);

        private void dgvDetailSession_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) => ColorerLigneVente(dgvDetailSession, e);
    
        /// <summary>Mode compact : colonnes secondaires masquées dans les deux tableaux détaillés.</summary>
        private bool _compact;
        private int _etatDetail = -1;

        /// <summary>Détail des ventes : colonnes secondaires masquées quand la place manque (mode compact ou tableau étroit).</summary>
        private void AppliquerColonnesDetail()
        {
            double largeur = dgvDetail.Width / Theme.Echelle(dgvDetail);
            int etat = (_compact || largeur < 900) ? 1 : 0;
            if (etat == _etatDetail) return;
            _etatDetail = etat;
            ModeCompact.MasquerColonnes(dgvDetail, etat == 1, "Motif", "Especes", "Rendu", "Vendeur");
            Theme.AjusterColonnes(dgvDetail);
        }

        public void DefinirCompact(bool compact)
        {
            var m = (int a, int b, int c, int d) => new Padding(a, b, c, d);
            tlpRapport.SuspendLayout();
            if (compact)
            {
                // une seule colonne : filtres, cartes, tiroir, moyens de paiement, détail ; l'onglet défile verticalement
                tabRapport.AutoScroll = true;
                tlpRapport.Dock = DockStyle.Top;
                tlpRapport.AutoSize = true;
                tlpRapport.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                foreach (var c in new Control[] { pnlFiltres, pnlCartes, pnlCaisse })
                {
                    tlpRapport.SetColumnSpan(c, 1);
                    tlpRapport.SetRowSpan(c, 1);
                }
                pnlCaisse.AutoSize = true;
                pnlCaisse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                tlpVentilation.Dock = DockStyle.Top;
                tlpVentilation.Height = Theme.Px(this, 190);
                dgvDetail.Dock = DockStyle.Top;
                dgvDetail.Height = Theme.Px(this, 260);
                ModeCompact.Recomposer(tlpRapport, new[] { "P100" }, new[] { "A", "A", "A", "A", "A", "A" },
                    (pnlFiltres, 0, 0, pnlFiltres.Margin), (pnlCartes, 0, 1, m(0, 0, 0, 8)), (pnlCaisse, 0, 2, m(0, 0, 0, 8)),
                    (tlpVentilation, 0, 3, m(0, 0, 0, 8)), (lblDetailVentes, 0, 4, lblDetailVentes.Margin), (dgvDetail, 0, 5, m(0, 0, 0, 0)));
            }
            else
            {
                tabRapport.AutoScroll = false;
                tlpRapport.Dock = DockStyle.Fill;
                tlpRapport.AutoSize = false;
                pnlCaisse.AutoSize = false;
                tlpVentilation.Dock = DockStyle.Fill;
                dgvDetail.Dock = DockStyle.Fill;
                ModeCompact.Recomposer(tlpRapport, new[] { "P36", "P64" }, new[] { "A", "A", "P45", "A", "P55" },
                    (pnlFiltres, 0, 0, pnlFiltres.Margin), (pnlCartes, 0, 1, m(0, 0, 0, 8)), (pnlCaisse, 0, 2, m(0, 0, 12, 0)),
                    (tlpVentilation, 1, 2, tlpVentilation.Margin), (lblDetailVentes, 1, 3, lblDetailVentes.Margin), (dgvDetail, 1, 4, m(0, 0, 0, 0)));
                tlpRapport.SetColumnSpan(pnlFiltres, 2);
                tlpRapport.SetColumnSpan(pnlCartes, 2);
                tlpRapport.SetRowSpan(pnlCaisse, 3);
            }
            tlpRapport.ResumeLayout(true);
            _compact = compact;
            AppliquerColonnesDetail();
            ModeCompact.MasquerColonnes(dgvSessions, compact, "Fond", "Theorique", "Compte", "NbVentes");
            ModeCompact.MasquerColonnes(dgvDetailSession, compact, "Verse");
            Theme.AjusterColonnes(dgvDetail);
            Theme.AjusterColonnes(dgvSessions);
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
