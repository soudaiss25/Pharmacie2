using Pharmacie2.Models;
using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Bandeau discret en bas de la page : « Une mise à jour (version X) est prête. Elle sera installée à la prochaine ouverture de
    /// l'application. » avec, pour l'administrateur seulement, le bouton « Installer maintenant » (refusé pendant une vente en cours).
    /// Caché tant qu'aucune mise à jour n'est prête.
    /// </summary>
    public class BandeauMiseAJour : Panel
    {
        private readonly Label _texte = new();
        private readonly Button _bouton = new();

        public BandeauMiseAJour(bool peutInstaller)
        {
            Name = "bandeauMiseAJour";
            Visible = false;
            Dock = DockStyle.Bottom;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(16, 6, 16, 6);
            BackColor = Theme.InfoFond;

            _texte.Name = "lblMiseAJour";
            _texte.AutoSize = true;
            _texte.MaximumSize = new Size(900, 0);
            _texte.Dock = DockStyle.Left;
            _texte.ForeColor = Theme.InfoTexte;
            _texte.Font = Theme.Police(10, FontStyle.Regular);
            _texte.Padding = new Padding(0, 6, 0, 6);

            _bouton.Name = "btnInstallerMaintenant";
            _bouton.Text = "Installer maintenant";
            _bouton.AutoSize = true;
            _bouton.Dock = DockStyle.Right;
            _bouton.Visible = peutInstaller;
            _bouton.Click += (s, e) => Installer();

            Controls.Add(_bouton);
            Controls.Add(_texte);
        }

        public void Afficher(string version)
        {
            _texte.Text = Format.Insecable($"Une mise à jour (version {version}) est prête. Elle sera installée à la prochaine ouverture de l'application.");
            Visible = true;
        }

        private void Installer()
        {
            var service = MiseAJourService.Instance;
            var r = service.TenterInstallation(
                Pharmacie2.views.Composants.VerrouillageAuto.VenteEnCours,
                () => Messages.Afficher(FindForm(),
                          $"Installer la version {service.VersionPrete} maintenant ?\n\nL'application va se fermer puis se rouvrir toute seule. Vos données ne sont pas touchées.",
                          "Installer la mise à jour", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            if (!r.Lancee && r.Message.Length > 0 && r.Message != "Installation annulée.")
                Messages.Afficher(FindForm(), r.Message, "Installer la mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>Branche le bandeau de mise à jour et l'annonce « Application mise à jour en version X » sur une page principale.</summary>
    public static class MiseAJourUi
    {
        public static void Installer(Form page, User user)
        {
            var service = MiseAJourService.Instance;
            var bandeau = new BandeauMiseAJour(user.Role == Roles.Administrateur);
            page.Controls.Add(bandeau);

            Action<string> surPrete = version =>
            {
                try
                {
                    if (!page.IsDisposed && page.IsHandleCreated)
                        page.BeginInvoke(new Action(() => { if (!page.IsDisposed) bandeau.Afficher(version); }));
                }
                catch { /* la page se ferme : rien à afficher */ }
            };
            service.MiseAJourPrete += surPrete;
            page.FormClosed += (s, e) => service.MiseAJourPrete -= surPrete;

            if (service.VersionPrete is { } deja) bandeau.Afficher(deja);   // déjà téléchargée avant l'ouverture de cette page

            page.Shown += (s, e) =>
            {
                if (service.PrendreAnnonce() is { } annonce) BandeauNotification.Succes(annonce);
            };
        }
    }
}
