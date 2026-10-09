using System.Globalization;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views.UserControls
{
    /// <summary>
    /// « Ma journée » : ce qu'il faut savoir en quelques secondes — argent reçu, ce qui manque ou va périmer,
    /// qui doit de l'argent. Écran d'ouverture de l'application.
    /// </summary>
    public partial class Uc_MaJournee : UserControl, IModeCompact
    {
        private static readonly CultureInfo Fr = new CultureInfo("fr-FR");

        /// <summary>Demande d'ouverture d'un écran (déjà filtré) depuis la liste « À faire maintenant ».</summary>
        public event Action<TypeAFaire>? OuvrirDemande;

        public Uc_MaJournee()
        {
            InitializeComponent();
            Theme.Appliquer(this);

            graphique.MessageSiVide = "Aucune vente ces 7 derniers jours";
            tlpBas.Resize += (s, e) => AjusterGraphique();
            AjusterGraphique();

            VenteEvenements.VenteModifiee += SurVenteModifiee;
            Disposed += (s, e) => VenteEvenements.VenteModifiee -= SurVenteModifiee;

            Actualiser();
        }

        /// <summary>Le graphique occupe environ 45 % de la hauteur disponible : la liste « À faire » reste l'élément principal.</summary>
        private void AjusterGraphique()
        {
            if (_compact) return;
            int disponible = tlpBas.ClientSize.Height - lblGraphique.Height - lblGraphique.Margin.Vertical;
            graphique.Height = Math.Max(Theme.Px(this, 150), (int)(disponible * 0.75));
        }

        private void SurVenteModifiee(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            if (InvokeRequired) BeginInvoke(new Action(Actualiser)); else Actualiser();
        }

        public void Actualiser()
        {
            var maintenant = DateTime.Now;
            string prenom = SessionUtilisateur.Courant?.Prenom ?? "";
            string jour = maintenant.ToString("dddd d MMMM yyyy", Fr);
            jour = char.ToUpper(jour[0], Fr) + jour.Substring(1);
            lblBonjour.Text = (string.IsNullOrWhiteSpace(prenom) ? "Bonjour" : "Bonjour " + prenom) + " — " + jour;

            try
            {
                var k = TableauDeBordService.Kpis(maintenant);

                carteEncaisse.Valeur = Format.Montant(k.EncaisseAujourdhui);
                carteEncaisse.Detail = Format.EvolutionOuTexte(k.EncaisseAujourdhui, k.EncaisseHier, "pas de vente hier à cette heure") + (k.EncaisseHier == 0 ? "" : " vs hier à la même heure");
                carteEncaisse.Tendance = k.EncaisseHier == 0 ? 0 : Format.Tendance(k.EncaisseAujourdhui, k.EncaisseHier);

                carteVentes.Valeur = k.VentesAujourdhui.ToString(Fr);
                carteVentes.Detail = Format.EvolutionOuTexte(k.VentesAujourdhui, k.VentesHier, "pas de vente hier à cette heure") + (k.VentesHier == 0 ? "" : " vs hier à la même heure");
                carteVentes.Tendance = k.VentesHier == 0 ? 0 : Format.Tendance(k.VentesAujourdhui, k.VentesHier);

                carteARecuperer.Valeur = Format.Montant(k.ArgentARecuperer);
                carteARecuperer.Detail = $"{Format.Compte(k.ClientsDebiteurs, "client")} et {Format.Compte(k.MutuellesDebitrices, "mutuelle")}";
                carteARecuperer.Niveau = k.ArgentARecuperer > 0 ? "attention" : "succes";
                carteARecuperer.HausseEstBonne = false;

                carteBenefice.Valeur = Format.Montant(k.BeneficeMois);
                carteBenefice.Detail = Format.EvolutionOuTexte(k.BeneficeMois, k.BeneficeMoisPrecedent, "pas de vente le mois dernier") + (k.BeneficeMoisPrecedent == 0 ? "" : " vs mois dernier");
                carteBenefice.Tendance = k.BeneficeMoisPrecedent == 0 ? 0 : Format.Tendance(k.BeneficeMois, k.BeneficeMoisPrecedent);

                var aFaire = TableauDeBordService.AFaireMaintenant(maintenant)
                    .Select(a => new ElementAction(a.Texte, a.Niveau, () => OuvrirDemande?.Invoke(a.Type)));
                listeActions.Definir(aFaire);

                graphique.Definir(TableauDeBordService.Encaissements7Jours(maintenant));
            }
            catch (Exception ex)
            {
                Journal.Erreur("Calcul de l'écran Ma journée", ex);
                listeActions.Definir(new[] { new ElementAction("Les chiffres n'ont pas pu être calculés. Voir le journal.", "urgent", null) });
            }
        }
    
        private bool _compact;

        /// <summary>Compact : cartes 2 × 2 ; « À faire » et graphique empilés ; l'écran défile verticalement.</summary>
        public void DefinirCompact(bool compact)
        {
            _compact = compact;
            SuspendLayout();
            var m = (int a, int b, int c, int d) => new Padding(a, b, c, d);
            if (compact)
            {
                AutoScroll = true;
                tlpRoot.Dock = DockStyle.Top;
                tlpRoot.AutoSize = true;
                tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                ModeCompact.Recomposer(tlpRoot, new[] { "P100" }, new[] { "A", "A", "A" },
                    (lblBonjour, 0, 0, lblBonjour.Margin), (tlpKpi, 0, 1, tlpKpi.Margin), (tlpBas, 0, 2, tlpBas.Margin));
                ModeCompact.Recomposer(tlpKpi, new[] { "P50", "P50" }, new[] { "A", "A" },
                    (carteEncaisse, 0, 0, m(0, 0, 12, 12)), (carteVentes, 1, 0, m(0, 0, 0, 12)),
                    (carteARecuperer, 0, 1, m(0, 0, 12, 0)), (carteBenefice, 1, 1, m(0, 0, 0, 0)));
                tlpBas.Dock = DockStyle.Top;
                tlpBas.AutoSize = true;
                tlpBas.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                listeActions.Dock = DockStyle.None;
                listeActions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                listeActions.AutoScroll = false;
                listeActions.AutoSize = true;
                listeActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                ModeCompact.Recomposer(tlpBas, new[] { "P100" }, new[] { "A", "A", "A", "A" },
                    (lblAFaire, 0, 0, m(0, 0, 0, 6)), (listeActions, 0, 1, m(0, 0, 0, 0)),
                    (lblGraphique, 0, 2, m(0, 12, 0, 6)), (graphique, 0, 3, m(0, 0, 0, 0)));
                graphique.Height = Theme.Px(this, 240);
            }
            else
            {
                AutoScroll = false;
                tlpRoot.Dock = DockStyle.Fill;
                tlpRoot.AutoSize = false;
                ModeCompact.Recomposer(tlpRoot, new[] { "P100" }, new[] { "A", "A", "P100" },
                    (lblBonjour, 0, 0, lblBonjour.Margin), (tlpKpi, 0, 1, tlpKpi.Margin), (tlpBas, 0, 2, tlpBas.Margin));
                ModeCompact.Recomposer(tlpKpi, new[] { "P25", "P25", "P25", "P25" }, new[] { "A" },
                    (carteEncaisse, 0, 0, m(0, 0, 12, 0)), (carteVentes, 1, 0, m(0, 0, 12, 0)),
                    (carteARecuperer, 2, 0, m(0, 0, 12, 0)), (carteBenefice, 3, 0, m(0, 0, 0, 0)));
                tlpBas.Dock = DockStyle.Fill;
                tlpBas.AutoSize = false;
                listeActions.AutoSize = false;
                listeActions.AutoScroll = true;
                listeActions.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                listeActions.Dock = DockStyle.Fill;
                ModeCompact.Recomposer(tlpBas, new[] { "P55", "P45" }, new[] { "A", "P100" },
                    (lblAFaire, 0, 0, m(0, 0, 0, 6)), (lblGraphique, 1, 0, m(12, 0, 0, 6)),
                    (listeActions, 0, 1, m(0, 0, 12, 0)), (graphique, 1, 1, m(12, 0, 0, 0)));
                AjusterGraphique();
            }
            ResumeLayout(true);
        }

    }
}
