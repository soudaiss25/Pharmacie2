using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>Bouton du menu de gauche. La page active est surlignée (couleur d'accent + barre à gauche).</summary>
    public class BoutonMenu : Button
    {
        private bool _actif;

        public BoutonMenu()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TextAlign = ContentAlignment.MiddleLeft;
            Padding = new Padding(20, 0, 8, 0);
            Margin = new Padding(0, 0, 0, 2);
            Height = 40;
            Cursor = Cursors.Hand;
            Tag = "menu";
            UseVisualStyleBackColor = false;
            AppliquerCouleurs();
        }

        public bool Actif
        {
            get => _actif;
            set { _actif = value; AppliquerCouleurs(); Invalidate(); }
        }

        private void AppliquerCouleurs()
        {
            ForeColor = Theme.Blanc;
            BackColor = _actif ? Theme.Accent : Theme.Principal;
            FlatAppearance.MouseOverBackColor = _actif ? Theme.Accent : ColorTranslator.FromHtml("#276B2B");
            FlatAppearance.MouseDownBackColor = Theme.Accent;
            Font = Theme.Police(10, _actif ? FontStyle.Bold : FontStyle.Regular);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (_actif)
            {
                using var barre = new SolidBrush(ColorTranslator.FromHtml("#A5D6A7"));
                pevent.Graphics.FillRectangle(barre, 0, 0, 5, Height);
            }
        }
    }
}
