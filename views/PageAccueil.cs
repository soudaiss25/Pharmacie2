using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;
using Pharmacie2.views.UserControls;

namespace Pharmacie2.views
{
    public partial class PageAccueil : Form, IPageSession
    {
        private readonly User _user;
        private BoutonMenu? _boutonActif;

        public bool DeconnexionDemandee { get; private set; }

        public PageAccueil(User user)
        {
            _user = user;
            InitializeComponent();
            Theme.Appliquer(this);

            Text = AppInfo.NomLogiciel;
            lblNomPharmacie.Text = AppInfo.NomPharmacie;
            lblUtilisateur.Text = $"{user.Prenom} {user.Nom} ({user.Role})";
            BandeauNotification.Courant = bandeau;

            flpMenu.Resize += (s, e) => AjusterBoutonsMenu();
            AjusterBoutonsMenu();
            FormClosed += (s, e) => { if (ReferenceEquals(BandeauNotification.Courant, bandeau)) BandeauNotification.Courant = null; };

            OuvrirMaJournee();   // plus d'écran vide à l'ouverture
        }

        // ── Navigation ────────────────────────────────────────────────────

        private void AjusterBoutonsMenu()
        {
            int largeur = Math.Max(120, flpMenu.ClientSize.Width);
            foreach (Control c in flpMenu.Controls)
                if (c is BoutonMenu) c.Width = largeur;
        }

        private void Ouvrir(UserControl uc, BoutonMenu? bouton)
        {
            var anciens = panelContent.Controls.Cast<Control>().ToList();
            panelContent.Controls.Clear();
            foreach (var a in anciens) a.Dispose();

            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);

            if (_boutonActif != null) _boutonActif.Actif = false;
            _boutonActif = bouton;
            if (bouton != null) bouton.Actif = true;
            bandeau.Masquer();
        }

        private void OuvrirMaJournee()
        {
            var uc = new Uc_MaJournee();
            uc.OuvrirDemande += SurDemandeDepuisMaJournee;
            Ouvrir(uc, btnMaJournee);
        }

        private void SurDemandeDepuisMaJournee(TypeAFaire type)
        {
            switch (type)
            {
                case TypeAFaire.Perimes: OuvrirStock("Périmés"); break;
                case TypeAFaire.Ruptures: OuvrirStock("En rupture"); break;
                case TypeAFaire.PeremptionProche: OuvrirStock("Péremption proche"); break;
                case TypeAFaire.StocksAVerifier: OuvrirStock("À vérifier"); break;
                case TypeAFaire.MutuellesEnRetard:
                    var m = new Uc_Mutuelle();
                    Ouvrir(m, btnMutuelles);
                    m.FiltrerEnRetard();
                    break;
            }
        }

        /// <summary>Ouvre l'écran Stock déjà filtré (« À vérifier », « Périmés », « En rupture »…).</summary>
        public void OuvrirStock(string filtre)
        {
            var stock = new Uc_Stock();
            Ouvrir(stock, btn_stock);
            stock.Filtrer(filtre);
        }

        /// <summary>Ouvre l'écran Stock filtré sur les produits « À vérifier » (message à la connexion).</summary>
        public void OuvrirStockAVerifier() => OuvrirStock("À vérifier");

        private void btnMaJournee_Click(object sender, EventArgs e) => OuvrirMaJournee();

        private void btnProduits_Click(object sender, EventArgs e) => Ouvrir(new Uc_Produits(), btnProduits);

        private void btnFournisseurs_Click(object sender, EventArgs e) => Ouvrir(new Uc_Fournisser(), btnFournisseurs);

        private void btnVentes_Click(object sender, EventArgs e) => Ouvrir(new Uc_Vente(), btnVentes);

        private void btnCredits_Click(object sender, EventArgs e)
        {
            var v = new Uc_Vente();
            Ouvrir(v, btnCredits);
            v.FiltrerCredits();
        }

        private void btn_stock_Click(object sender, EventArgs e) => Ouvrir(new Uc_Stock(), btn_stock);

        private void btnMutuelles_Click(object sender, EventArgs e) => Ouvrir(new Uc_Mutuelle(), btnMutuelles);

        private void btnStatistiques_Click(object sender, EventArgs e)
        {
            var uc = new Uc_Statistique();
            uc.OuvrirDemande += SurDemandeDepuisMaJournee;
            Ouvrir(uc, btnStatistiques);
        }

        private void btnDepenses_Click(object sender, EventArgs e) => Ouvrir(new Uc_Depenses(), btnDepenses);

        private void BtnCaisse_Click_1(object sender, EventArgs e) => Ouvrir(new Uc_Caisse(), BtnCaisse);

        private void BtnGestionUtilisateur_Click(object sender, EventArgs e) => Ouvrir(new Uc_Utilisateurs(), BtnGestionUtilisateur);

        private void btnCommandes_Click(object sender, EventArgs e) => Ouvrir(new Uc_Commande(), btnCommandes);

        // ── Raccourcis clavier ────────────────────────────────────────────

        private void PageAccueil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                using var form = new FormVente();
                form.ShowDialog(this);   // les écrans se rafraîchissent via VenteEvenements
            }
        }

        // ── Sauvegarde ────────────────────────────────────────────────────

        private void btnSauvegarde_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Choisissez le dossier de sauvegarde";
            if (fbd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string dossier = Path.Combine(fbd.SelectedPath, $"Pharmacie2_backup_{DateTime.Now:yyyy-MM-dd_HHmmss}");
                Directory.CreateDirectory(dossier);
                SauvegardeService.ExporterTout(dossier);
                bandeau.Afficher("Sauvegarde effectuée dans : " + dossier);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Sauvegarde CSV", ex);
                MessageBox.Show("La sauvegarde a échoué : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOuvrirDossierSauvegardes_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = CheminsApp.DossierSauvegardes,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Journal.Erreur("Ouverture du dossier des sauvegardes", ex);
                MessageBox.Show("Impossible d'ouvrir le dossier des sauvegardes :\n" + CheminsApp.DossierSauvegardes,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeconnexion_Click(object sender, EventArgs e)
        {
            DeconnexionDemandee = true;
            Close();   // Form1 (la fenêtre de connexion, unique) se ré-affiche : pas de Application.Exit
        }
    }
}
