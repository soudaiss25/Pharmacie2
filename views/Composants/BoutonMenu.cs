using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Bouton du menu de gauche : icône monochrome + libellé. La page active est surlignée (couleur d'accent + barre à gauche).
    /// En mode réduit (petits écrans), seule l'icône est dessinée, centrée ; le libellé reste disponible en infobulle.
    /// </summary>
    public class BoutonMenu : Button
    {
        private bool _actif;
        private bool _reduit;
        private bool _survol;
        private string _icone = "";
        private Image? _image;

        public BoutonMenu()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Height = 32;
            Cursor = Cursors.Hand;
            Tag = "menu";
            Font = Theme.Police(10);
            MouseEnter += (s, e) => { _survol = true; Invalidate(); };
            MouseLeave += (s, e) => { _survol = false; Invalidate(); };
        }

        /// <summary>Nom de l'icône (fichier de resources/icones, sans extension).</summary>
        public string Icone
        {
            get => _icone;
            set { _icone = value ?? ""; _image = _icone.Length == 0 ? null : Icones.Charger(_icone); Invalidate(); }
        }

        public bool Actif
        {
            get => _actif;
            set { _actif = value; Invalidate(); }
        }

        /// <summary>Icône seule (menu replié sur petits écrans).</summary>
        public bool Reduit
        {
            get => _reduit;
            set { _reduit = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            float k = Theme.Echelle(this);
            int Px(float v) => (int)Math.Round(v * k);

            Color fond = _actif ? Theme.Accent : _survol ? ColorTranslator.FromHtml("#276B2B") : Theme.Principal;
            using (var b = new SolidBrush(fond)) g.FillRectangle(b, ClientRectangle);

            if (_actif)
                using (var barre = new SolidBrush(ColorTranslator.FromHtml("#A5D6A7")))
                    g.FillRectangle(barre, 0, 0, Px(4), Height);

            int taille = Px(20);
            int xTexte = Px(16);
            if (_image != null)
            {
                int x = _reduit ? (Width - taille) / 2 : Px(16);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(_image, new Rectangle(x, (Height - taille) / 2, taille, taille));
                xTexte = Px(16) + taille + Px(12);
            }

            if (!_reduit && Text.Length > 0)
            {
                using var police = new Font(Font, _actif ? FontStyle.Bold : FontStyle.Regular);
                TextRenderer.DrawText(g, Text, police, new Rectangle(xTexte, 0, Math.Max(0, Width - xTexte - Px(8)), Height), Theme.Blanc,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            }

            if (Focused && ShowFocusCues)
                using (var pen = new Pen(Color.FromArgb(200, Theme.Blanc)) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                    g.DrawRectangle(pen, Px(2), Px(2), Width - Px(5), Height - Px(5));
        }
    }
}
