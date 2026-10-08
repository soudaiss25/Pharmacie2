using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Bandeau non bloquant en haut de la zone de contenu : un succès s'affiche 4 secondes puis disparaît.
    /// Les MessageBox restent réservées aux confirmations et aux erreurs.
    /// </summary>
    public class BandeauNotification : Panel
    {
        private readonly Label _texte = new Label();
        private readonly System.Windows.Forms.Timer _minuteur = new System.Windows.Forms.Timer { Interval = 4000 };

        /// <summary>Bandeau de la page ouverte (null hors des pages principales : l'appel est alors ignoré).</summary>
        public static BandeauNotification? Courant { get; set; }

        public BandeauNotification()
        {
            Visible = false;
            Dock = DockStyle.Fill;
            Padding = new Padding(16, 8, 16, 8);
            MinimumSize = new Size(0, 40);
            _texte.Dock = DockStyle.Fill;
            _texte.AutoSize = false;
            _texte.TextAlign = ContentAlignment.MiddleLeft;
            _texte.Font = Theme.Police(10, FontStyle.Bold);
            Controls.Add(_texte);
            _minuteur.Tick += (s, e) => Masquer();
        }

        public void Afficher(string message, string niveau = "succes")
        {
            var (texte, fond) = Theme.Niveau(niveau);
            BackColor = fond;
            _texte.ForeColor = texte;
            _texte.BackColor = fond;
            _texte.Text = message;
            Visible = true;
            _minuteur.Stop();
            _minuteur.Start();
        }

        public void Masquer()
        {
            _minuteur.Stop();
            Visible = false;
        }

        /// <summary>Notification de succès non bloquante (sans effet si aucune page n'est ouverte).</summary>
        public static void Succes(string message) => Courant?.Afficher(message, "succes");

        public static void Attention(string message) => Courant?.Afficher(message, "attention");

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _minuteur.Stop();
                _minuteur.Dispose();
                if (ReferenceEquals(Courant, this)) Courant = null;
            }
            base.Dispose(disposing);
        }
    }
}
