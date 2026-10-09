using System.Drawing.Drawing2D;
using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Carte de chiffre clé : titre, valeur en 22 pt gras, détail (comparaison) avec flèche de tendance.
    /// Se dessine elle-même et s'élargit avec sa cellule.
    /// </summary>
    public class CarteKpi : Control
    {
        private string _titre = "";
        private string _valeur = "";
        private string _detail = "";
        private int _tendance;
        private bool _hausseEstBonne = true;
        private string _niveau = "";   // "", "urgent", "attention", "succes" : colore la bande gauche

        public CarteKpi()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(220, 104);
            AutoSize = true;   // les rangées automatiques mesurent GetPreferredSize, pas la taille mise à l'échelle
            Cursor = Cursors.Default;
        }

        /// <summary>Hauteur et largeur utiles fixes en pixels : le dessin est en pixels, pas en échelle de police.</summary>
        public override Size GetPreferredSize(Size proposedSize) => new Size(170, 104);

        public string Titre { get => _titre; set { _titre = value; Invalidate(); } }
        public string Valeur { get => _valeur; set { _valeur = value; Invalidate(); } }
        public string Detail { get => _detail; set { _detail = value; Invalidate(); } }

        /// <summary>1 hausse, -1 baisse, 0 stable : flèche dessinée devant le détail.</summary>
        public int Tendance { get => _tendance; set { _tendance = value; Invalidate(); } }

        /// <summary>Pour « Argent à récupérer », une hausse est mauvaise : la flèche passe en orange/rouge.</summary>
        public bool HausseEstBonne { get => _hausseEstBonne; set { _hausseEstBonne = value; Invalidate(); } }

        public string Niveau { get => _niveau; set { _niveau = value; Invalidate(); } }

        public bool Cliquable
        {
            get => Cursor == Cursors.Hand;
            set => Cursor = value ? Cursors.Hand : Cursors.Default;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var fond = new SolidBrush(Theme.Blanc)) g.FillRectangle(fond, r);
            using (var bord = new Pen(Theme.Bordure)) g.DrawRectangle(bord, r);

            var (texteNiveau, _) = Theme.Niveau(string.IsNullOrEmpty(_niveau) ? "succes" : _niveau);
            using (var barre = new SolidBrush(string.IsNullOrEmpty(_niveau) ? Theme.Accent : texteNiveau))
                g.FillRectangle(barre, 0, 0, 5, Height);

            int x = 16, y = 10, w = Width - x - 8;
            var fmt = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

            using (var br = new SolidBrush(Theme.Neutre))
                g.DrawString(_titre, Theme.Police(10, FontStyle.Bold), br, new RectangleF(x, y, w, 20), fmt);

            using (var br = new SolidBrush(Theme.Texte))
                g.DrawString(_valeur, Theme.Kpi, br, new RectangleF(x, y + 22, w, 38), fmt);

            int yDetail = y + 22 + 40;
            int xTexte = x;
            if (_tendance != 0)
            {
                bool bon = (_tendance > 0) == _hausseEstBonne;
                var couleur = bon ? Theme.SuccesTexte : Theme.UrgentTexte;
                var haut = _tendance > 0;
                var pts = haut
                    ? new[] { new Point(x, yDetail + 12), new Point(x + 12, yDetail + 12), new Point(x + 6, yDetail + 2) }
                    : new[] { new Point(x, yDetail + 2), new Point(x + 12, yDetail + 2), new Point(x + 6, yDetail + 12) };
                using (var b = new SolidBrush(couleur)) g.FillPolygon(b, pts);
                xTexte = x + 18;
                using var brT = new SolidBrush(couleur);
                g.DrawString(_detail, Theme.Note, brT, new RectangleF(xTexte, yDetail, w - 18, 20), fmt);
            }
            else
            {
                using var brT = new SolidBrush(Theme.Neutre);
                g.DrawString(_detail, Theme.Note, brT, new RectangleF(xTexte, yDetail, w, 20), fmt);
            }
        }
    }
}
