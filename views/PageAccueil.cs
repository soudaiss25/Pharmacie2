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
        private UserControl? _ecranCourant;
        private bool _compact;
        private bool _menuDeplie;
        private Panel? _reserveMenu;
        private VoileMenu? _voile;
        private readonly ToolTip _infobulles = new ToolTip();

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
            ClientSizeChanged += (s, e) => AppliquerMode(false);
            LocationChanged += (s, e) => { if (_menuDeplie) BasculerMenu(); };
            // bordure nette sur le bord droit du menu quand il est déplié par-dessus le contenu
            panelMenu.Paint += (s, e) =>
            {
                if (!_menuDeplie) return;
                using var trait = new Pen(ColorTranslator.FromHtml("#A5D6A7"), Math.Max(2, Theme.Px(this, 2)));
                e.Graphics.DrawLine(trait, panelMenu.Width - 1, 0, panelMenu.Width - 1, panelMenu.Height);
            };
            FormClosed += (s, e) => { if (ReferenceEquals(BandeauNotification.Courant, bandeau)) BandeauNotification.Courant = null; };

            OuvrirMaJournee();   // plus d'écran vide à l'ouverture
            AppliquerMode(true);
        }

        // ── Mode compact (largeur < 1200 unités logiques) ─────────────────

        /// <summary>Mode actuel (pour les tests).</summary>
        public bool EstCompact => _compact;

        private void AppliquerMode(bool forcer)
        {
            bool compact = ModeCompact.Est(this);
            if (!forcer && compact == _compact) return;
            _compact = compact;
            if (!compact && _menuDeplie) BasculerMenu();
            AppliquerPresentationMenu();
            (_ecranCourant as IModeCompact)?.DefinirCompact(compact);
        }

        private IEnumerable<BoutonMenu> BoutonsMenu
            => flpMenu.Controls.OfType<BoutonMenu>().Concat(new[] { btnDeconnexion, btnMenu });

        /// <summary>Menu large (icône + libellé) ou réduit à une colonne d'icônes ; déplié par-dessus le contenu au clic sur le bouton Menu.</summary>
        private void AppliquerPresentationMenu()
        {
            bool reduit = _compact && !_menuDeplie;
            tlpPrincipal.ColumnStyles[0].Width = Theme.Px(this, _compact ? 56 : 230);
            btnMenu.Visible = _compact;
            lblNomPharmacie.Visible = !_compact;
            lblUtilisateur.Visible = !reduit;
            lblSectionQuotidien.Visible = lblSectionGestion.Visible = !reduit;
            traitAdmin.Width = Theme.Px(this, reduit ? 32 : 182);
            foreach (var b in BoutonsMenu)
            {
                b.Reduit = reduit;
                _infobulles.SetToolTip(b, reduit ? b.Text : "");
            }
            AjusterBoutonsMenu();
        }

        private void btnMenu_Click(object sender, EventArgs e) => BasculerMenu();

        /// <summary>Voile semi-transparent sur le contenu : un clic dessus referme le menu.</summary>
        private void AfficherVoile()
        {
            FermerVoile();
            int gauche = Theme.Px(this, 56);
            var origine = PointToScreen(new Point(gauche, 0));
            _voile = new VoileMenu { Bounds = new Rectangle(origine, new Size(Math.Max(1, ClientSize.Width - gauche), ClientSize.Height)) };
            _voile.Clique += () => { if (_menuDeplie) BasculerMenu(); };
            _voile.Show(this);
        }

        private void FermerVoile()
        {
            if (_voile == null) return;
            _voile.Close();
            _voile.Dispose();
            _voile = null;
        }

        private void BasculerMenu()
        {
            if (!_compact && !_menuDeplie) return;
            _menuDeplie = !_menuDeplie;
            if (_menuDeplie)
            {
                // le menu passe par-dessus le contenu ; une réserve verte garde la colonne réduite en place
                _reserveMenu = new Panel { BackColor = panelMenu.BackColor, Dock = DockStyle.Fill, Margin = Padding.Empty };
                tlpPrincipal.Controls.Remove(panelMenu);
                tlpPrincipal.Controls.Add(_reserveMenu, 0, 0);
                panelMenu.Dock = DockStyle.None;
                panelMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
                Controls.Add(panelMenu);
                panelMenu.SetBounds(0, 0, Theme.Px(this, 230), ClientSize.Height);
                panelMenu.BringToFront();
                AfficherVoile();
            }
            else
            {
                Controls.Remove(panelMenu);
                panelMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                panelMenu.Dock = DockStyle.Fill;
                if (_reserveMenu != null)
                {
                    tlpPrincipal.Controls.Remove(_reserveMenu);
                    _reserveMenu.Dispose();
                    _reserveMenu = null;
                }
                tlpPrincipal.Controls.Add(panelMenu, 0, 0);
                FermerVoile();
            }
            panelMenu.Invalidate();
            AppliquerPresentationMenu();
        }

        // ── Navigation ────────────────────────────────────────────────────

        private void AjusterBoutonsMenu()
        {
            int largeur = Math.Max(Theme.Px(this, 48), flpMenu.ClientSize.Width);
            foreach (Control c in flpMenu.Controls)
                if (c is BoutonMenu) c.Width = largeur;
        }

        /// <summary>Ouvre un écran dans la zone de contenu (sans bouton du menu surligné).</summary>
        public void Afficher(UserControl uc) => Ouvrir(uc, null);

        private void Ouvrir(UserControl uc, BoutonMenu? bouton)
        {
            if (_menuDeplie) BasculerMenu();   // une navigation replie le menu déplié
            var anciens = panelContent.Controls.Cast<Control>().ToList();
            panelContent.Controls.Clear();
            foreach (var a in anciens) a.Dispose();

            Hebergement.Heberger(panelContent, uc);   // zone défilante : l'écran prend max(zone, taille minimale)
            _ecranCourant = uc;
            (uc as IModeCompact)?.DefinirCompact(_compact);

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

        private void btnAdministration_Click(object sender, EventArgs e)
        {
            var admin = new Uc_Administration();
            admin.UtilisateursDemande += () => Ouvrir(new Uc_Utilisateurs(), btnAdministration);
            admin.SauvegardeDemande += () => btnSauvegarde_Click(this, EventArgs.Empty);
            admin.DossierDemande += () => btnOuvrirDossierSauvegardes_Click(this, EventArgs.Empty);
            Ouvrir(admin, btnAdministration);
        }

        private void btnCommandes_Click(object sender, EventArgs e) => Ouvrir(new Uc_Commande(), btnCommandes);

        // ── Raccourcis clavier ────────────────────────────────────────────

        private void PageAccueil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _menuDeplie)
            {
                e.Handled = true;
                BasculerMenu();
            }
            else if (e.KeyCode == Keys.F2)
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
