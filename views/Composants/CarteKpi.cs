using System.Drawing.Drawing2D;
using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Carte de chiffre clé : titre, valeur en gras, détail (comparaison) avec flèche de tendance.
    /// Se dessine elle-même ; toutes les distances sont en pixels à 96 DPI, mises à l'échelle du zoom Windows.
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
            Size = new Size(220, 96);
            AutoSize = true;   // les rangées automatiques mesurent GetPreferredSize
            Cursor = Cursors.Default;
        }

        public override Size GetPreferredSize(Size proposedSize) => new Size(Theme.Px(this, 170), Theme.Px(this, 96));

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
            float k = Theme.Echelle(this);
            int Px(float v) => (int)Math.Round(v * k);

            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var fond = new SolidBrush(Theme.Blanc)) g.FillRectangle(fond, r);
            using (var bord = new Pen(Theme.Bordure)) g.DrawRectangle(bord, r);

            var (texteNiveau, _) = Theme.Niveau(string.IsNullOrEmpty(_niveau) ? "succes" : _niveau);
            using (var barre = new SolidBrush(string.IsNullOrEmpty(_niveau) ? Theme.Accent : texteNiveau))
                g.FillRectangle(barre, 0, 0, Px(4), Height);

            int x = Px(14), w = Width - x - Px(8);
            using var fTitre = Theme.Adapter(Theme.Police(9.5f, FontStyle.Bold));
            using var fValeur = Theme.Adapter(Theme.Police(20, FontStyle.Bold));
            using var fDetail = Theme.Adapter(Theme.Note);
            var fmt = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

            // trois lignes centrées verticalement : titre, valeur, détail
            int hTitre = fTitre.Height, hValeur = fValeur.Height, hDetail = fDetail.Height;
            int total = hTitre + hValeur + hDetail + Px(6);
            int y = Math.Max(Px(4), (Height - total) / 2);

            using (var br = new SolidBrush(Theme.Neutre))
                g.DrawString(_titre, fTitre, br, new RectangleF(x, y, w, hTitre), fmt);

            using (var br = new SolidBrush(Theme.Texte))
                g.DrawString(_valeur, fValeur, br, new RectangleF(x, y + hTitre + Px(2), w, hValeur), fmt);

            int yDetail = y + hTitre + hValeur + Px(4);
            int xTexte = x;
            Color couleurDetail = Theme.Neutre;
            if (_tendance != 0)
            {
                bool bon = (_tendance > 0) == _hausseEstBonne;
                couleurDetail = bon ? Theme.SuccesTexte : Theme.UrgentTexte;
                int t = Px(10), h = Px(8), dy = (hDetail - h) / 2;
                var pts = _tendance > 0
                    ? new[] { new Point(x, yDetail + dy + h), new Point(x + t, yDetail + dy + h), new Point(x + t / 2, yDetail + dy) }
                    : new[] { new Point(x, yDetail + dy), new Point(x + t, yDetail + dy), new Point(x + t / 2, yDetail + dy + h) };
                using (var b = new SolidBrush(couleurDetail)) g.FillPolygon(b, pts);
                xTexte = x + t + Px(6);
            }
            using (var brT = new SolidBrush(couleurDetail))
                g.DrawString(_detail, fDetail, brT, new RectangleF(xTexte, yDetail, w - (xTexte - x), hDetail), fmt);
        }
    }
}
