namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Règle unique d'hébergement d'un écran dans une zone de contenu :
    /// la zone défile (AutoScroll) et l'écran n'est plus ancré en « remplir » ; il prend la taille
    /// max(zone visible, taille minimale de l'écran). Les barres de défilement n'apparaissent donc
    /// que si la fenêtre est plus petite que le minimum, et l'écran s'étire au-delà.
    /// </summary>
    public static class Hebergement
    {
        /// <summary>Marge intérieure appliquée à un écran qui n'en a pas.</summary>
        public const int Marge = 12;

        public static void Heberger(ScrollableControl zone, Control ecran)
        {
            zone.AutoScroll = true;
            if (ecran.Padding == Padding.Empty)
                ecran.Padding = new Padding(Marge);

            ecran.Dock = DockStyle.None;
            ecran.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            ecran.Location = Point.Empty;

            bool enCours = false;
            void Ajuster()
            {
                if (enCours || ecran.IsDisposed) return;
                enCours = true;
                try
                {
                    var visible = zone.ClientSize;
                    var minimum = ecran.MinimumSize;
                    ecran.Size = new Size(Math.Max(visible.Width, minimum.Width), Math.Max(visible.Height, minimum.Height));
                }
                finally { enCours = false; }
            }

            EventHandler surTaille = (s, e) => Ajuster();
            zone.Controls.Add(ecran);
            Ajuster();
            zone.ClientSizeChanged += surTaille;
            ecran.Disposed += (s, e) => zone.ClientSizeChanged -= surTaille;
        }
    }
}
