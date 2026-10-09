namespace Pharmacie2.views.Composants
{
    /// <summary>
    /// Voile semi-transparent posé sur le contenu quand le menu est déplié (mode compact).
    /// Fenêtre sans bordure qui n'enlève jamais le focus à la fenêtre principale ; un clic dessus referme le menu.
    /// </summary>
    public sealed class VoileMenu : Form
    {
        public event Action? Clique;

        public VoileMenu()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Black;
            Opacity = 0.35;
            Cursor = Cursors.Hand;
            Click += (s, e) => Clique?.Invoke();
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x08000000 /* WS_EX_NOACTIVATE */ | 0x00000080 /* WS_EX_TOOLWINDOW */;
                return cp;
            }
        }
    }
}
