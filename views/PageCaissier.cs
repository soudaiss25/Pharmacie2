using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class PageCaissier : Form, IPageSession
    {
        private static readonly CultureInfo Fr = new CultureInfo("fr-FR");
        private readonly User _user;

        public bool DeconnexionDemandee { get; private set; }
        private Pharmacie2.views.Composants.VerrouillageAuto? _verrou;

        // ─── Constructeur ────────────────────────────────────────────────
        public PageCaissier(User user)
        {
            _user = user;
            InitializeComponent();
            Pharmacie2.views.Composants.ModeCompact.CartesAdaptatives(tlpKpi);
            Theme.Appliquer(this);
            _verrou = new Pharmacie2.views.Composants.VerrouillageAuto(this, this);

            Text = AppInfo.Titre("Caisse");
            BandeauNotification.Courant = bandeau;
            string jour = DateTime.Today.ToString("dddd d MMMM yyyy", Fr);
            lblBienvenue.Text = $"Bonjour {user.Prenom} {user.Nom} — {jour}";

            VenteEvenements.VenteModifiee += SurVenteModifiee;
            FormClosed += (s, e) =>
            {
                VenteEvenements.VenteModifiee -= SurVenteModifiee;
                if (ReferenceEquals(BandeauNotification.Courant, bandeau)) BandeauNotification.Courant = null;
            };

            // 1. Ouvrir / vérifier la session caisse AVANT d'afficher les ventes
            GererSessionCaisse();

            // 2. Charger uniquement les ventes de la session active
            ChargerVentesSession();
            MettreAJourBandeauSession();
        }

        private void SurVenteModifiee(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired) BeginInvoke(new Action(ChargerVentesSession)); else ChargerVentesSession();
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
        private SessionCaisse? GetSessionActive()
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

        /// <summary>Met à jour la ligne d'état en bas avec les infos de la session active.</summary>
        private void MettreAJourBandeauSession()
        {
            var session = GetSessionActive();

            if (session != null)
            {
                lblSessionInfo.Text =
                    $"Caisse ouverte à {session.DateOuverture:HH:mm} — caissier : {_user.Prenom} {_user.Nom}";
                lblSessionInfo.ForeColor = Theme.SuccesTexte;
            }
            else
            {
                lblSessionInfo.Text = "Aucune caisse ouverte";
                lblSessionInfo.ForeColor = Theme.AttentionTexte;
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
                    var session = ctx.SessionsCaisse
                        .Where(s => s.UserId == _user.Id
                                 && s.Statut == "Ouverte"
                                 && s.DateOuverture.Date == DateTime.Today)
                        .OrderByDescending(s => s.DateOuverture)
                        .FirstOrDefault();

                    if (session == null)
                    {
                        dgvVentes.DataSource = null;
                        carteEncaisse.Valeur = "—";
                        carteEncaisse.Detail = "Aucune caisse ouverte";
                        carteVentes.Valeur = "—";
                        carteVentes.Detail = "";
                        carteReste.Valeur = "—";
                        carteReste.Detail = "";
                        VentilationVide();
                        return;
                    }

                    DateTime debut = session.DateOuverture;
                    DateTime fin = DateTime.Now;

                    var ventes = ctx.ventes
                        .AsNoTracking()
                        .Include(v => v.Paiements)
                        .Where(v => v.UserId == _user.Id && v.DateVente >= debut && v.DateVente <= fin)
                        .OrderByDescending(v => v.DateVente)
                        .ToList();

                    dgvVentes.DataSource = ventes.Select(v => new
                    {
                        Id = v.IdVente,
                        Numero = v.numeroVente,
                        Client = $"{v.PrenomClient} {v.NomClient}".Trim(),
                        Heure = v.DateVente.ToString("HH:mm"),
                        Total = v.MontantTotal,
                        Verse = v.MontantVerse,
                        Restant = v.MontantRestant,
                        Mode = LibelleMode(v),
                        v.Statut
                    }).ToList();

                    var actives = ventes.Where(v => v.Statut == "Active").ToList();

                    // Argent réellement encaissé par ce caissier depuis l'ouverture (nouvelles ventes ET règlements de crédits)
                    var paiements = ctx.paiement
                        .AsNoTracking()
                        .Include(p => p.Vente)
                        .Where(p => p.UserId == _user.Id && p.DatePaiement >= debut && p.DatePaiement <= fin
                                 && p.Vente.Statut != "Annulée")
                        .ToList();

                    decimal Somme(Func<string, bool> mode) => paiements.Where(p => mode(p.Vente.Type ?? "")).Sum(p => p.Montant);
                    decimal especes = Somme(t => t == ModesPaiement.Comptant);
                    decimal cheque = Somme(t => t == ModesPaiement.Cheque);
                    decimal digital = Somme(t => t == ModesPaiement.CarteBancaire || ModesPaiement.EstMobile(t));
                    decimal mutuellePatient = Somme(t => t == ModesPaiement.Mutuelle);
                    decimal credit = Somme(t => t == ModesPaiement.Credit);
                    decimal totalEncaisse = paiements.Sum(p => p.Montant);

                    decimal resteCredit = actives.Where(v => v.Type == ModesPaiement.Credit).Sum(v => v.MontantRestant);
                    decimal partEntreprise = actives.Where(v => v.Type == ModesPaiement.Mutuelle && !v.MutuelleReglee).Sum(v => v.MontantMutuelle);

                    carteEncaisse.Valeur = Format.Montant(totalEncaisse);
                    carteEncaisse.Detail = $"depuis {debut:HH:mm}";
                    carteVentes.Valeur = actives.Count.ToString(Fr);
                    carteVentes.Detail = $"pour {Format.Montant(actives.Sum(v => v.MontantTotal))} de ventes";
                    carteReste.Valeur = Format.Montant(resteCredit + partEntreprise);
                    carteReste.Detail = $"crédits : {Format.Montant(resteCredit)} — mutuelles : {Format.Montant(partEntreprise)}";
                    carteReste.Niveau = resteCredit + partEntreprise > 0 ? "attention" : "succes";

                    lblEspeces.Text = "Espèces : " + Format.Montant(especes);
                    lblCheque.Text = "Chèques : " + Format.Montant(cheque);
                    lblDigital.Text = "Carte et mobile : " + Format.Montant(digital);
                    lblMutuelle.Text = "Part patient mutuelle : " + Format.Montant(mutuellePatient);
                    lblCredit.Text = "Versements sur crédits : " + Format.Montant(credit);
                }
            }
            catch (Exception ex)
            {
                Journal.Erreur("Chargement des ventes de la session", ex);
                MessageBox.Show("Impossible d'afficher les ventes : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentilationVide()
        {
            lblEspeces.Text = lblCheque.Text = lblDigital.Text = lblMutuelle.Text = lblCredit.Text = "";
        }

        private static string LibelleMode(Vente v)
        {
            if (v.Type == ModesPaiement.Credit)
            {
                if (v.MontantVerse > 0 && v.MontantRestant <= 0) return "Crédit soldé";
                return v.MontantVerse > 0 ? "Crédit partiel" : "Crédit";
            }
            if (v.Type == ModesPaiement.Mutuelle)
                return v.MutuelleReglee ? "Mutuelle réglée" : "Mutuelle";
            return v.MoyenPaiement ?? "";
        }

        private void dgvVentes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvVentes.Rows.Count) return;
            var ligne = dgvVentes.Rows[e.RowIndex];
            string statut = ligne.Cells["Statut"].Value?.ToString() ?? "";
            string mode = ligne.Cells["Mode"].Value?.ToString() ?? "";

            if (statut == "Annulée")
            {
                e.CellStyle.BackColor = Theme.InfoFond;
                e.CellStyle.ForeColor = Theme.Neutre;
            }
            else if (mode == "Crédit")                       // rien versé : à relancer
            {
                e.CellStyle.BackColor = Theme.UrgentFond;
            }
            else if (mode == "Crédit partiel" || mode == "Mutuelle")
            {
                e.CellStyle.BackColor = Theme.AttentionFond;
            }
        }

        // ─── Boutons ──────────────────────────────────────────────────────

        private void PageCaissier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.L)
            {
                e.Handled = true;
                _verrou?.VerrouillerMaintenant();
            }
            else if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                btnNouvelleVente_Click(sender, EventArgs.Empty);
            }
        }

        private void btnNouvelleVente_Click(object sender, EventArgs e)
        {
            // Vérifier qu'une session est bien ouverte avant de vendre
            if (GetSessionActive() == null)
            {
                var rep = MessageBox.Show(
                    "Aucune caisse ouverte.\nVoulez-vous en ouvrir une ?",
                    "Caisse requise", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (rep != DialogResult.Yes) return;
                GererSessionCaisse();
                if (GetSessionActive() == null) return;
            }

            using (var form = new FormVente())
            {
                form.ShowDialog(this);
                ChargerVentesSession();
                MettreAJourBandeauSession();
            }
        }

        private int? VenteSelectionnee()
            => dgvVentes.SelectedRows.Count == 0 ? null : Convert.ToInt32(dgvVentes.SelectedRows[0].Cells["Id"].Value);

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (VenteSelectionnee() is not int id) return;
            using (var form = new FormDetailVente(id))
                form.ShowDialog(this);
        }

        private void btnPaiement_Click(object sender, EventArgs e)
        {
            if (VenteSelectionnee() is not int id) return;
            using (var form = new FormPaiements(id))
                form.ShowDialog(this);
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

        private void btnDeconnexion_Click(object sender, EventArgs e) => Deconnecter();

        private void btnVerrouiller_Click(object sender, EventArgs e) => _verrou?.VerrouillerMaintenant();

        public void Deconnecter()
        {
            DeconnexionDemandee = true;
            Close();   // Form1 (la fenêtre de connexion, unique) se ré-affiche : pas de Application.Exit
        }
    }
}
