using Pharmacie2.Services;

namespace Pharmacie2.views.Composants
{
    /// <summary>Écran dont la disposition s'adapte aux petites largeurs (mode compact) sans être recréé.</summary>
    public interface IModeCompact
    {
        /// <summary>Bascule de disposition : même instance, mêmes données, saisie en cours conservée.</summary>
        void DefinirCompact(bool compact);
    }

    /// <summary>Règle unique : sous 1200 unités logiques de largeur de fenêtre, on passe en mode compact.</summary>
    public static class ModeCompact
    {
        public const int Seuil = 1200;

        public static bool Est(Control fenetre) => fenetre.ClientSize.Width / Theme.Echelle(fenetre) < Seuil;

        /// <summary>Replace des contrôles dans un tableau : colonnes, lignes et cellules, sans en créer ni en perdre.</summary>
        public static void Recomposer(TableLayoutPanel tlp, string[] colonnes, string[] lignes, params (Control c, int col, int ligne, Padding marge)[] cellules)
        {
            tlp.SuspendLayout();
            tlp.ColumnStyles.Clear();
            tlp.RowStyles.Clear();
            tlp.ColumnCount = colonnes.Length;
            tlp.RowCount = lignes.Length;
            foreach (var c in colonnes) tlp.ColumnStyles.Add(Colonne(c, tlp));
            foreach (var l in lignes) tlp.RowStyles.Add(Ligne(l, tlp));
            foreach (var (c, col, ligne, marge) in cellules)
            {
                tlp.SetCellPosition(c, new TableLayoutPanelCellPosition(col, ligne));
                c.Margin = marge;
            }
            tlp.ResumeLayout(true);
        }

        // "A" = automatique, "P50" = 50 %
        // "F260" = 260 pixels logiques (mis à l'échelle du zoom)
        private static (SizeType, float) Lire(string s, Control c)
        {
            if (s == "A") return (SizeType.AutoSize, 0f);
            float v = float.Parse(s.Substring(1), System.Globalization.CultureInfo.InvariantCulture);
            return s[0] == 'F' ? (SizeType.Absolute, v * Theme.Echelle(c)) : (SizeType.Percent, v);
        }

        private static ColumnStyle Colonne(string s, Control c) { var (t, v) = Lire(s, c); return new ColumnStyle(t, v); }
        private static RowStyle Ligne(string s, Control c) { var (t, v) = Lire(s, c); return new RowStyle(t, v); }

        /// <summary>Masque ou montre des colonnes de tableau par leur nom.</summary>
        public static void MasquerColonnes(DataGridView g, bool masquer, params string[] noms)
        {
            foreach (var n in noms)
                if (g.Columns[n] is DataGridViewColumn c) c.Visible = !masquer;
        }
    }
}
