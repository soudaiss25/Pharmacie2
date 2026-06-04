using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    /// <summary>
    /// Dialogue d'ouverture / clôture de session caisse.
    /// MODIFICATION : Plus de fond d'ouverture — on ouvre la caisse directement sans déclarer de montant.
    /// </summary>
    public partial class FormSessionCaisse : Form
    {
        private readonly int _userId;
        private SessionCaisse _sessionOuverte;

        public FormSessionCaisse(int userId)
        {
            _userId = userId;
            InitializeComponent();
            ChargerEtat();
        }

        // ── Détecte s'il existe une session ouverte pour ce caissier ──────

        private void ChargerEtat()
        {
            using (var ctx = new AppDbContext())
            {
                _sessionOuverte = ctx.SessionsCaisse
                    .Include(s => s.User)
                    .Where(s => s.UserId == _userId
                             && s.Statut == "Ouverte"
                             && s.DateOuverture.Date == DateTime.Today)
                    .OrderByDescending(s => s.DateOuverture)
                    .FirstOrDefault();
            }

            if (_sessionOuverte == null)
            {
                // ── Mode ouverture ─────────────────────────────────────
                lblEtat.Text = "Aucune session ouverte pour aujourd'hui.";
                lblEtat.ForeColor = System.Drawing.Color.OrangeRed;
                btnOuvrir.Enabled = true;
                btnCloture.Enabled = false;
                numMontantReel.Enabled = false;
                txtCommentaire.Enabled = false;
                lblInfoSession.Text = "Cliquez sur « Ouvrir la caisse » pour démarrer votre session.";
            }
            else
            {
                // ── Mode clôture ───────────────────────────────────────
                lblEtat.Text = $"Session ouverte le {_sessionOuverte.DateOuverture:dd/MM/yyyy à HH:mm}";
                lblEtat.ForeColor = System.Drawing.Color.ForestGreen;
                btnOuvrir.Enabled = false;
                btnCloture.Enabled = true;
                numMontantReel.Enabled = true;
                txtCommentaire.Enabled = true;

                decimal theorique = CalculerTheorique(
                    _sessionOuverte.UserId ?? _userId,
                    _sessionOuverte.DateOuverture);

                lblInfoSession.Text =
                    $"Montant théorique en caisse : {theorique:N0} KMF\n" +
                    $"(total encaissé - rendus depuis l'ouverture de {_sessionOuverte.DateOuverture:HH:mm})";

                if (theorique >= numMontantReel.Minimum && theorique <= numMontantReel.Maximum)
                    numMontantReel.Value = theorique;
            }
        }

        // ── Calcule le montant théorique attendu (encaissements - rendus, sans fond) ──

        private decimal CalculerTheorique(int userId, DateTime dateOuverture)
        {
            using (var ctx = new AppDbContext())
            {
                DateTime debut = dateOuverture;
                DateTime fin = DateTime.Now;

                var ventesActives = ctx.ventes
                    .Where(v => v.UserId == userId
                             && v.DateVente >= debut
                             && v.DateVente <= fin
                             && v.Statut == "Active")
                    .ToList();

                var idsActives = ventesActives.Select(v => v.IdVente).ToHashSet();

                var paiements = ctx.paiement
                    .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin)
                    .ToList();

                decimal encaisse = paiements
                    .Where(p => idsActives.Contains(p.VenteId))
                    .Sum(p => (decimal)p.Montant);

                decimal rendu = ventesActives
                    .Where(v => v.Type == "Comptant")
                    .Sum(v => (decimal)v.MontantRendu);

                // Pas de fond d'ouverture → uniquement ce qui a été encaissé - rendu
                return encaisse - rendu;
            }
        }

        // ── Bouton : Ouvrir la session ────────────────────────────────────

        private void btnOuvrir_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Ouvrir la session de caisse maintenant ?",
                "Ouverture de caisse",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (var ctx = new AppDbContext())
            {
                ctx.SessionsCaisse.Add(new SessionCaisse
                {
                    UserId = _userId,
                    DateOuverture = DateTime.Now,
                    FondOuverture = 0,      // Toujours 0 — plus de fond d'ouverture
                    Statut = "Ouverte",
                    CommentaireCloture = ""
                });
                ctx.SaveChanges();
            }

            MessageBox.Show(
                "Caisse ouverte ✅\nBonne journée !",
                "Ouverture effectuée",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Bouton : Clôturer la session ──────────────────────────────────

        private void btnCloture_Click(object sender, EventArgs e)
        {
            if (_sessionOuverte == null) return;

            decimal montantReel = numMontantReel.Value;
            decimal theorique = CalculerTheorique(
                _sessionOuverte.UserId ?? _userId,
                _sessionOuverte.DateOuverture);

            decimal ecart = montantReel - theorique;
            string signe = ecart >= 0 ? "+" : "";
            string msgEcart = ecart == 0
                ? "✅ Caisse parfaitement équilibrée."
                : $"⚠️ Écart : {signe}{ecart:N0} KMF";

            var confirm = MessageBox.Show(
                $"Clôturer la session caisse ?\n\n" +
                $"Montant théorique : {theorique:N0} KMF\n" +
                $"Montant compté    : {montantReel:N0} KMF\n\n" +
                msgEcart,
                "Clôture de caisse",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (var ctx = new AppDbContext())
            {
                var session = ctx.SessionsCaisse
                    .FirstOrDefault(s => s.Id == _sessionOuverte.Id);

                if (session == null)
                {
                    MessageBox.Show("Session introuvable en base.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                session.DateCloture = DateTime.Now;
                session.MontantCompteCloture = montantReel;
                session.CommentaireCloture = txtCommentaire.Text.Trim();
                session.Statut = "Clôturée";
                ctx.SaveChanges();
            }

            MessageBox.Show(
                $"Session clôturée ✅\n{msgEcart}",
                "Clôture effectuée",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => Close();
    }
}