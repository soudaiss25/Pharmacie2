using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>Une ligne cliquable de la liste « À faire maintenant ».</summary>
    public record ElementAction(string Texte, string Niveau, Action? Action);

    /// <summary>
    /// Liste cliquable d'éléments à traiter. Chaque ligne a une bande de couleur selon son niveau
    /// (rouge = urgent, orange = attention) et ouvre l'écran concerné au clic.
    /// </summary>
    public class ListeActions : FlowLayoutPanel
    {
        private const int HauteurLigne = 40;

        public ListeActions()
        {
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
            AutoScroll = true;
            BackColor = Theme.Fond;
            MinimumSize = new Size(240, 80);
            Resize += (s, e) => AjusterLargeurs();
        }

        public int NombreDeLignes => Controls.Count;

        public void Definir(IEnumerable<ElementAction> elements, string messageSiVide = "Tout est en ordre")
        {
            SuspendLayout();
            while (Controls.Count > 0)
            {
                var c = Controls[0];
                Controls.RemoveAt(0);
                c.Dispose();
            }

            var liste = elements.ToList();
            if (liste.Count == 0)
                liste.Add(new ElementAction(messageSiVide, "succes", null));

            foreach (var el in liste)
                Controls.Add(Creer(el));

            AjusterLargeurs();
            ResumeLayout(true);
        }

        private Control Creer(ElementAction el)
        {
            var (texte, fond) = Theme.Niveau(el.Niveau);
            var ligne = new Label
            {
                Text = Format.Insecable(el.Texte) + (el.Action != null ? "   ›" : ""),
                AutoSize = false,
                Height = HauteurLigne,
                Margin = new Padding(0, 0, 0, 4),
                Padding = new Padding(14, 0, 8, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = Theme.Police(10, el.Niveau == "urgent" ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = el.Niveau == "succes" ? texte : Theme.Texte,
                BackColor = fond,
                Cursor = el.Action != null ? Cursors.Hand : Cursors.Default,
                Tag = el
            };
            ligne.Paint += (s, e) =>
            {
                using var b = new SolidBrush(texte);
                e.Graphics.FillRectangle(b, 0, 0, 5, ligne.Height);
            };
            if (el.Action != null)
                ligne.Click += (s, e) => el.Action();
            return ligne;
        }

        private void AjusterLargeurs()
        {
            int barre = AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0;
            int largeur = Math.Max(120, ClientSize.Width - Padding.Horizontal - barre);
            foreach (Control c in Controls)
                c.Width = largeur;
        }
    }
}
