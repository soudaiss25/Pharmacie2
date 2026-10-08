using System.Drawing.Drawing2D;
using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>Graphique en barres dessiné en GDI+ (sans bibliothèque), redimensionnable.</summary>
    public class GraphiqueBarres : Control
    {
        private List<(string etiquette, decimal valeur)> _donnees = new();
        private string _titre = "";

        public GraphiqueBarres()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);
            MinimumSize = new Size(280, 150);
            Size = new Size(480, 220);
        }

        public string Titre { get => _titre; set { _titre = value; Invalidate(); } }

        public void Definir(IEnumerable<(string etiquette, decimal valeur)> donnees)
        {
            _donnees = donnees.ToList();
            Invalidate();
        }

        public int NombreDeBarres => _donnees.Count;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Theme.Blanc);
            using (var bord = new Pen(Theme.Bordure)) g.DrawRectangle(bord, 0, 0, Width - 1, Height - 1);

            int hautTitre = string.IsNullOrEmpty(_titre) ? 8 : 34;
            if (!string.IsNullOrEmpty(_titre))
                using (var br = new SolidBrush(Theme.Principal))
                    g.DrawString(_titre, Theme.TitreSection, br, 12, 8);

            if (_donnees.Count == 0)
            {
                using var br = new SolidBrush(Theme.Neutre);
                g.DrawString("Aucune donnée", Theme.Texte10, br, 12, hautTitre + 8);
                return;
            }

            var police = Theme.Note;
            decimal max = _donnees.Max(d => d.valeur);
            if (max <= 0) max = 1;

            // échelle « ronde » pour les lignes de repère
            decimal pas = PasRond(max / 4m);
            decimal sommet = Math.Ceiling(max / pas) * pas;
            if (sommet <= 0) sommet = pas;

            int margeGauche = (int)g.MeasureString(Format.Nombre(sommet), police).Width + 14;
            var zone = new Rectangle(margeGauche, hautTitre + 12, Width - margeGauche - 12, Height - hautTitre - 12 - 30);
            if (zone.Width < 40 || zone.Height < 40) return;

            // lignes de repère
            using (var gris = new Pen(Color.FromArgb(224, 230, 224)))
            using (var brGris = new SolidBrush(Theme.Neutre))
            {
                for (decimal v = 0; v <= sommet; v += pas)
                {
                    int y = zone.Bottom - (int)(zone.Height * (v / sommet));
                    g.DrawLine(gris, zone.Left, y, zone.Right, y);
                    var txt = Format.Nombre(v);
                    var tm = g.MeasureString(txt, police);
                    g.DrawString(txt, police, brGris, zone.Left - tm.Width - 4, y - tm.Height / 2);
                }
            }

            // barres
            int n = _donnees.Count;
            float pasX = zone.Width / (float)n;
            float largeur = Math.Min(pasX * 0.6f, 90f);
            for (int i = 0; i < n; i++)
            {
                var (etiquette, valeur) = _donnees[i];
                float x = zone.Left + pasX * i + (pasX - largeur) / 2f;
                int h = (int)(zone.Height * (double)(valeur / sommet));
                var rect = new RectangleF(x, zone.Bottom - h, largeur, Math.Max(h, valeur > 0 ? 2 : 0));
                bool derniere = i == n - 1;
                using (var b = new SolidBrush(derniere ? Theme.Principal : Theme.Accent))
                    g.FillRectangle(b, rect);

                var fmtC = new StringFormat { Alignment = StringAlignment.Center };
                using (var br = new SolidBrush(Theme.Texte))
                {
                    var txt = Format.Nombre(valeur);
                    var tm = g.MeasureString(txt, police);
                    if (tm.Width < pasX)
                        g.DrawString(txt, police, br, x + largeur / 2f, rect.Top - tm.Height - 1, fmtC);
                    g.DrawString(etiquette, police, br, new RectangleF(zone.Left + pasX * i, zone.Bottom + 4, pasX, 22), fmtC);
                }
            }
        }

        private static decimal PasRond(decimal brut)
        {
            if (brut <= 0) return 1;
            decimal ordre = 1;
            while (brut >= ordre * 10) ordre *= 10;
            while (brut < ordre) ordre /= 10;
            foreach (var m in new[] { 1m, 2m, 2.5m, 5m, 10m })
                if (brut <= ordre * m) return Math.Max(1m, ordre * m);
            return ordre * 10;
        }
    }
}
