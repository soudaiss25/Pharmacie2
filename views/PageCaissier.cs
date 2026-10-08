using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    public partial class PageCaissier : Form, IPageSession
    {
        private readonly User _user;

        // ─── Constructeur ────────────────────────────────────────────────
        public PageCaissier(User user)
        {
            _user = user;
            InitializeComponent();

            lblBienvenue.Text =
                $"Bonjour, {user.Prenom} {user.Nom}  —  Caisse du {DateTime.Today:dd/MM/yyyy}";

            // 1. Ouvrir / vérifier la session caisse AVANT d'afficher les ventes
            GererSessionCaisse();

            // 2. Charger uniquement les ventes de la session active
            ChargerVentesSession();
            MettreAJourBandeauSession();
        }

        // ─── Gestion session caisse ──────────────────────────────────────

        /// <summary>
        /// Vérifie s'il existe une session ouverte pour ce caissier aujourd'hui.
        /// Si non → ouvre le dialogue pour démarrer la session.
        /// </summary>
        private void GererSessionCaisse()
        {
            using (var ctx = new AppDbContext())
            {
                bool sessionExiste = ctx.SessionsCaisse.Any(s =>
                    s.UserId == _user.Id &&
                    s.Statut == "Ouverte" &&
                    s.DateOuverture.Date == DateTime.Today);

                if (!sessionExiste)
                {
                    using (var form = new FormSessionCaisse(_user.Id))
                        form.ShowDialog(this);
                }
            }
        }

        /// <summary>Retourne la session ouverte du caissier aujourd'hui (ou null).</summary>
        private SessionCaisse GetSessionActive()
        {
            using (var ctx = new AppDbContext())
            {
                return ctx.SessionsCaisse
                    .Where(s => s.UserId == _user.Id
                             && s.Statut == "Ouverte"
                             && s.DateOuverture.Date == DateTime.Today)
                    .OrderByDescending(s => s.DateOuverture)
                    .FirstOrDefault();
            }
        }

        /// <summary>Met à jour le bandeau en bas avec les infos de la session active.</summary>
        private void MettreAJourBandeauSession()
        {
            var session = GetSessionActive();

            if (session != null)
            {
                lblSessionInfo.Text =
                    $"🟢 Session ouverte à {session.DateOuverture:HH:mm}  |  " +
                    $"Caissier : {_user.Prenom} {_user.Nom}";
                lblSessionInfo.ForeColor = System.Drawing.Color.FromArgb(27, 94, 32);
            }
            else
            {
                lblSessionInfo.Text = "⚪ Aucune session active";
                lblSessionInfo.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        // ─── Chargement ventes filtrées par session ──────────────────────

        /// <summary>
        /// Charge uniquement les ventes effectuées depuis l'ouverture de la session active.
        /// Le caissier ne voit PAS les ventes des sessions précédentes.
        /// </summary>
        private void ChargerVentesSession()
        {
            try
            {
                using (var ctx = new AppDbContext())
                {
                    // Récupérer la session active
                    var session = ctx.SessionsCaisse
                        .Where(s => s.UserId == _user.Id
                                 && s.Statut == "Ouverte"
                                 && s.DateOuverture.Date == DateTime.Today)
                        .OrderByDescending(s => s.DateOuverture)
                        .FirstOrDefault();

                    if (session == null)
                    {
                        dgvVentes.Rows.Clear();
                        lblStats.Text = "Aucune session active — ouvrez une session pour commencer.";
                        return;
                    }

                    // Fenêtre temporelle de la session
                    DateTime debut = session.DateOuverture;
                    DateTime fin = DateTime.Now;

                    // Ventes UNIQUEMENT dans la fenêtre de la session en cours
                    var ventes = ctx.ventes
                        .Include(v => v.Paiements)
                        .Where(v => v.UserId == _user.Id
                                 && v.DateVente >= debut
                                 && v.DateVente <= fin)
                        .OrderByDescending(v => v.DateVente)
                        .ToList();

                    dgvVentes.Rows.Clear();

                    foreach (var v in ventes)
                    {
                        // Déterminer le libellé selon le type et l'état
                        string typeLabel;
                        if (v.Type == "Crédit")
                        {
                            if (v.MontantVerse > 0 && v.MontantRestant <= 0)
                                typeLabel = "✅ CRÉDIT SOLDÉ";
                            else if (v.MontantVerse > 0)
                                typeLabel = "💰 CRÉDIT PARTIEL";
                            else
                                typeLabel = "📋 CRÉDIT";
                        }
                        else if (v.Type == "Mutuelle")
                        {
                            typeLabel = v.MutuelleReglee ? "✅ MUTUELLE RÉGLÉE" : "🏥 MUTUELLE";
                        }
                        else
                        {
                            typeLabel = v.MoyenPaiement.ToUpper();
                        }

                        int idx = dgvVentes.Rows.Add(
                            v.IdVente,
                            v.numeroVente,
                            $"{v.PrenomClient} {v.NomClient}".Trim(),
                            v.DateVente.ToString("HH:mm"),
                            $"{v.MontantTotal:N0}",
                            $"{v.MontantVerse:N0}",
                            v.MontantRestant > 0 ? $"{v.MontantRestant:N0}" : "—",
                            typeLabel,
                            v.Statut
                        );

                        var row = dgvVentes.Rows[idx];

                        // Coloration selon statut et type
                        if (v.Statut == "Annulée")
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                        }
                        else if (v.Type == "Crédit" && v.MontantRestant > 0 && v.MontantVerse > 0)
                        {
                            // Crédit partiel — orange clair
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 243, 205);
                            row.Cells["colRestant"].Style.ForeColor = System.Drawing.Color.FromArgb(180, 80, 0);
                            row.Cells["colRestant"].Style.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                        }
                        else if (v.Type == "Crédit" && v.MontantVerse == 0)
                        {
                            // Crédit total non payé — rouge clair
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                            row.Cells["colRestant"].Style.ForeColor = System.Drawing.Color.OrangeRed;
                            row.Cells["colRestant"].Style.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                        }
                        else if (v.Type == "Mutuelle" && !v.MutuelleReglee)
                        {
                            // Mutuelle en attente — bleu clair
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
                        }
                    }

                    // ── Stats de la session ──────────────────────────────
                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();
                    decimal totalSession = ventesActives.Sum(v => (decimal)v.MontantTotal);
                    int nbVentes = ventesActives.Count;

                    // ── Ventilation par mode — montants RÉELLEMENT encaissés ──────
                    // Comptant : tout le montant est reçu
                    decimal totalEspeces = ventesActives
                        .Where(v => v.Type == "Comptant")
                        .Sum(v => (decimal)v.MontantTotal);

                    // Chèque : tout reçu (à déposer en banque)
                    decimal totalCheque = ventesActives
                        .Where(v => v.Type == "Chèque")
                        .Sum(v => (decimal)v.MontantTotal);

                    // Digital CB/Mobile : tout reçu électroniquement
                    decimal totalDigital = ventesActives
                        .Where(v => v.Type == "Carte bancaire"
                                 || v.Type == ModesPaiement.Mvola
                                 || v.Type == ModesPaiement.HuriMoney)
                        .Sum(v => (decimal)v.MontantTotal);

                    // Mutuelle : seulement la PART PATIENT est encaissée
                    // La part entreprise (MontantMutuelle) est un crédit en attente
                    decimal partPatientMutuelle = ventesActives
                        .Where(v => v.Type == "Mutuelle")
                        .Sum(v => (decimal)(v.MontantTotal - v.MontantMutuelle));
                    decimal partEntrepriseMutuelle = ventesActives
                        .Where(v => v.Type == "Mutuelle")
                        .Sum(v => (decimal)v.MontantMutuelle);

                    // Crédit : seulement ce qui a été versé (MontantVerse)
                    // Le reste (MontantRestant) n'est pas encore encaissé
                    decimal avancesCredit = ventesActives
                        .Where(v => v.Type == "Crédit")
                        .Sum(v => (decimal)v.MontantVerse);
                    decimal resteCredit = ventesActives
                        .Where(v => v.Type == "Crédit")
                        .Sum(v => (decimal)v.MontantRestant);

                    // Total réellement encaissé
                    decimal totalEncaisse = totalEspeces + totalCheque + totalDigital
                                          + partPatientMutuelle + avancesCredit;

                    // Total paiements reçus sur ventes crédit (tous paiements confondus)
                    var idsCredit = ventesActives
                        .Where(v => v.Type == "Crédit")
                        .Select(v => v.IdVente).ToHashSet();
                    decimal totalPaiementsCredit = ctx.paiement
                        .Where(p => idsCredit.Contains(p.VenteId)
                                 && p.DatePaiement >= debut
                                 && p.DatePaiement <= fin)
                        .ToList()
                        .Sum(p => (decimal)p.Montant);

                    lblStats.Text =
                        $"Ventes : {nbVentes}  |  " +
                        $"CA facturé : {totalSession:N0} KMF  |  " +
                        $"✅ Encaissé : {totalEncaisse:N0} KMF  |  " +
                        $"💵 Espèces : {totalEspeces:N0}  |  " +
                        $"📄 Chèque : {totalCheque:N0}  |  " +
                        $"💳 Digital : {totalDigital:N0}  |  " +
                        $"🏥 Mutuelle patient : {partPatientMutuelle:N0} (ent. : {partEntrepriseMutuelle:N0})  |  " +
                        $"📋 Crédit — facturé : {ventesActives.Where(v => v.Type == "Crédit").Sum(v => (decimal)v.MontantTotal):N0}  " +
                        $"| reçu : {totalPaiementsCredit:N0}  " +
                        $"| reste : {resteCredit:N0}  " +
                        $"(KMF)  —  Depuis : {debut:HH:mm}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur chargement ventes : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─── Boutons ──────────────────────────────────────────────────────

        private void btnNouvelleVente_Click(object sender, EventArgs e)
        {
            // Vérifier qu'une session est bien ouverte avant de vendre
            using (var ctx = new AppDbContext())
            {
                bool ok = ctx.SessionsCaisse.Any(s =>
                    s.UserId == _user.Id &&
                    s.Statut == "Ouverte" &&
                    s.DateOuverture.Date == DateTime.Today);

                if (!ok)
                {
                    var rep = MessageBox.Show(
                        "Aucune session caisse ouverte.\nVoulez-vous en ouvrir une ?",
                        "Session requise",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (rep == DialogResult.Yes)
                        GererSessionCaisse();
                    else
                        return;
                }
            }

            using (var form = new FormVente())
            {
                form.ShowDialog();
                ChargerVentesSession();
                MettreAJourBandeauSession();
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (dgvVentes.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colId"].Value);
            using (var form = new FormDetailVente(id))
                form.ShowDialog();
        }

        private void btnPaiement_Click(object sender, EventArgs e)
        {
            if (dgvVentes.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["colId"].Value);
            using (var form = new FormPaiements(id))
                form.ShowDialog();
            ChargerVentesSession();
        }

        private void btnActualiser_Click(object sender, EventArgs e)
        {
            ChargerVentesSession();
            MettreAJourBandeauSession();
        }

        private void btnCloturerSession_Click(object sender, EventArgs e)
        {
            using (var form = new FormSessionCaisse(_user.Id))
                form.ShowDialog(this);

            // Après clôture, on vide la grille car il n'y a plus de session active
            MettreAJourBandeauSession();
            ChargerVentesSession();
        }

        public bool DeconnexionDemandee { get; private set; }

        private void btnDeconnexion_Click(object sender, EventArgs e)
        {
            DeconnexionDemandee = true;
            Close();   // Form1 (la fenêtre de connexion, unique) se ré-affiche : pas de Application.Exit
        }
    }
}