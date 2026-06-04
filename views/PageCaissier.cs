using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class PageCaissier : Form
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
                        int idx = dgvVentes.Rows.Add(
                            v.IdVente,
                            v.numeroVente,
                            $"{v.PrenomClient} {v.NomClient}".Trim(),
                            v.DateVente.ToString("HH:mm"),
                            v.MontantTotal.ToString("0.00"),
                            v.MontantVerse.ToString("0.00"),
                            v.MontantRestant.ToString("0.00"),
                            v.MoyenPaiement,
                            v.Statut
                        );

                        // Coloration lignes
                        if (v.Statut == "Annulée")
                        {
                            dgvVentes.Rows[idx].DefaultCellStyle.BackColor =
                                System.Drawing.Color.FromArgb(255, 235, 238);
                            dgvVentes.Rows[idx].DefaultCellStyle.ForeColor =
                                System.Drawing.Color.OrangeRed;
                        }
                        else if (v.MontantRestant > 0)
                        {
                            dgvVentes.Rows[idx].Cells["colRestant"].Style.ForeColor =
                                System.Drawing.Color.OrangeRed;
                        }
                    }

                    // Stats de la session
                    var ventesActives = ventes.Where(v => v.Statut == "Active").ToList();
                    decimal totalSession = ventesActives.Sum(v => (decimal)v.MontantTotal);
                    int nbVentes = ventesActives.Count;

                    lblStats.Text =
                        $"Ventes de cette session : {nbVentes}  |  " +
                        $"Total : {totalSession:N0} KMF  |  " +
                        $"Session depuis : {debut:HH:mm}";
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

        private void btnDeconnexion_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form1().Show();
        }
    }
}