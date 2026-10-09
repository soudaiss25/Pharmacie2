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

        /// <summary>Pour les fenêtres de dialogue : le mode dépend de la taille de l'ÉCRAN, pas de celle de la fenêtre.</summary>
        public static bool EstEcranPetit(Control c) => Theme.EcranLogique(c).Width < Seuil;

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

        /// <summary>
        /// Cartes de chiffres clés : autant de colonnes que la largeur le permet (au moins <paramref name="largeurMini"/> unités logiques
        /// par carte), sur une ou plusieurs lignes. Se recalcule à chaque changement de largeur, sans créer ni perdre de carte.
        /// </summary>
        public static void CartesAdaptatives(TableLayoutPanel tlp, int largeurMini = 210, int marge = 12)
        {
            var cartes = tlp.Controls.Cast<Control>().OrderBy(c => tlp.GetRow(c) * 100 + tlp.GetColumn(c)).ToList();
            int derniere = -1;
            void Appliquer()
            {
                if (tlp.Width <= 0) return;
                int n = cartes.Count;
                double largeur = tlp.Width / Theme.Echelle(tlp);
                var candidats = new[] { n, (int)Math.Ceiling(n / 2.0), 2, 1 }.Where(c => c >= 1 && c <= n).Distinct().OrderByDescending(c => c);
                int cols = candidats.FirstOrDefault(c => largeur / c >= largeurMini);
                if (cols == 0) cols = 1;
                if (cols == derniere) return;
                derniere = cols;
                int lignes = (int)Math.Ceiling(n / (double)cols);
                var colonnes = Enumerable.Repeat("P" + (100f / cols).ToString(System.Globalization.CultureInfo.InvariantCulture), cols).ToArray();
                var rangees = Enumerable.Repeat("A", lignes).ToArray();
                var cellules = cartes.Select((c, i) =>
                {
                    int col = i % cols, lig = i / cols;
                    return (c, col, lig, new Padding(0, 0, col < cols - 1 ? Theme.Px(tlp, marge) : 0, lig < lignes - 1 ? Theme.Px(tlp, marge) : 0));
                }).ToArray();
                Recomposer(tlp, colonnes, rangees, cellules);
            }
            tlp.Resize += (s, e) => Appliquer();
            Appliquer();
        }

        /// <summary>Masque ou montre des colonnes de tableau par leur nom.</summary>
        public static void MasquerColonnes(DataGridView g, bool masquer, params string[] noms)
        {
            foreach (var n in noms)
                if (g.Columns[n] is DataGridViewColumn c) c.Visible = !masquer;
        }
    }
}
