using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
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
                lblEtat.Text = $"Session ouverte le {_sessionOuverte.DateOuverture:dd/MM/yyyy à HH:mm}";
                lblEtat.ForeColor = System.Drawing.Color.ForestGreen;
                btnOuvrir.Enabled = false;
                btnCloture.Enabled = true;
                numMontantReel.Enabled = true;
                txtCommentaire.Enabled = true;
                lblInfoSession.Text = "Session en cours. Renseignez le montant compté en caisse avant de clôturer.";
            }
        }

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
                    FondOuverture = 0,
                    Statut = "Ouverte",
                    CommentaireCloture = ""
                });
                ctx.SaveChanges();
            }

            MessageBox.Show("Caisse ouverte ✅\nBonne journée !",
                "Ouverture effectuée", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCloture_Click(object sender, EventArgs e)
        {
            if (_sessionOuverte == null) return;

            decimal montantCompte = numMontantReel.Value;

            var confirm = MessageBox.Show(
                $"Clôturer la session caisse ?\n\n" +
                $"Montant compté en caisse : {montantCompte:N0} KMF\n\n" +
                $"Confirmez-vous la clôture ?",
                "Clôture de caisse",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (var ctx = new AppDbContext())
            {
                var session = ctx.SessionsCaisse
                    .FirstOrDefault(s => s.Id == _sessionOuverte.Id);

                if (session == null) return;

                session.DateCloture = DateTime.Now;
                session.MontantCompteCloture = montantCompte;
                session.CommentaireCloture = txtCommentaire.Text.Trim();
                session.Statut = "Clôturée";
                ctx.SaveChanges();
            }

            MessageBox.Show("Session clôturée ✅",
                "Clôture effectuée", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => Close();
    }
}