using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

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
            Theme.Appliquer(this);
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
                lblEtat.Text = "Aucune caisse ouverte aujourd'hui.";
                lblEtat.ForeColor = Theme.AttentionTexte;
                btnOuvrir.Enabled = true;
                btnCloture.Enabled = false;
                numFond.Enabled = true;
                numMontantReel.Enabled = false;
                txtCommentaire.Enabled = false;
                lblInfoSession.Text = "Indiquez l'argent qui se trouve déjà dans le tiroir, puis cliquez sur « Ouvrir la caisse ».";
                AcceptButton = btnOuvrir;
            }
            else
            {
                lblEtat.Text = $"Caisse ouverte le {_sessionOuverte.DateOuverture:dd/MM/yyyy à HH:mm}";
                lblEtat.ForeColor = Theme.SuccesTexte;
                btnOuvrir.Enabled = false;
                btnCloture.Enabled = true;
                numFond.Enabled = false;
                numFond.Value = Math.Min(numFond.Maximum, _sessionOuverte.FondOuverture);
                numMontantReel.Enabled = true;
                txtCommentaire.Enabled = true;
                lblInfoSession.Text = "Caisse en cours. Comptez l'argent du tiroir et saisissez le montant avant de clôturer.";
                AcceptButton = btnCloture;
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
                    FondOuverture = numFond.Value,
                    Statut = "Ouverte",
                    CommentaireCloture = ""
                });
                ctx.SaveChanges();
            }

            BandeauNotification.Succes("Caisse ouverte. Bonne journée !");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCloture_Click(object sender, EventArgs e)
        {
            if (_sessionOuverte == null) return;

            decimal montantCompte = numMontantReel.Value;

            var confirm = MessageBox.Show(
                $"Clôturer la session caisse ?\n\n" +
                $"Montant compté en caisse : {Format.Montant(montantCompte)}\n\n" +
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

            BandeauNotification.Succes("Caisse clôturée");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => Close();
    }
}