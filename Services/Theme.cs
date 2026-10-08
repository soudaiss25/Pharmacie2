using System.Drawing;

namespace Pharmacie2.Services
{
    public enum StyleBouton
    {
        Primaire,
        Secondaire,
        Danger,
        Neutre
    }

    /// <summary>
    /// Palette et styles uniques de l'application. Le thème ne change à l'exécution que les COULEURS
    /// et la police de contrôles précis (titres, chiffres) — jamais Form.Font / UserControl.Font.
    /// Rouge = urgent, orange = attention, vert = tout va bien, gris-bleu = information.
    /// </summary>
    public static class Theme
    {
        // ── Couleurs ──────────────────────────────────────────────────────
        public static readonly Color Principal = ColorTranslator.FromHtml("#1B5E20");
        public static readonly Color Accent = ColorTranslator.FromHtml("#2E7D32");
        public static readonly Color Fond = ColorTranslator.FromHtml("#F5F7F5");
        public static readonly Color Texte = ColorTranslator.FromHtml("#212121");
        public static readonly Color Neutre = ColorTranslator.FromHtml("#455A64");
        public static readonly Color Blanc = Color.White;
        public static readonly Color Bordure = ColorTranslator.FromHtml("#CFD8DC");
        public static readonly Color LigneAlternee = ColorTranslator.FromHtml("#F9FBF9");
        public static readonly Color Selection = ColorTranslator.FromHtml("#C8E6C9");

        public static readonly Color SuccesTexte = ColorTranslator.FromHtml("#1B5E20");
        public static readonly Color SuccesFond = ColorTranslator.FromHtml("#E8F5E9");
        public static readonly Color AttentionTexte = ColorTranslator.FromHtml("#E65100");
        public static readonly Color AttentionFond = ColorTranslator.FromHtml("#FFF3E0");
        public static readonly Color UrgentTexte = ColorTranslator.FromHtml("#C62828");
        public static readonly Color UrgentFond = ColorTranslator.FromHtml("#FFEBEE");
        public static readonly Color InfoTexte = ColorTranslator.FromHtml("#455A64");
        public static readonly Color InfoFond = ColorTranslator.FromHtml("#ECEFF1");

        // ── Polices (échelle typographique, rien sous 9 pt) ──────────────
        private static readonly Dictionary<(float, FontStyle), Font> _polices = new();

        public static Font Police(float points, FontStyle style = FontStyle.Regular)
        {
            points = Math.Max(9f, points);
            lock (_polices)
            {
                if (!_polices.TryGetValue((points, style), out var f))
                    _polices[(points, style)] = f = new Font("Segoe UI", points, style, GraphicsUnit.Point);
                return f;
            }
        }

        public static Font Kpi => Police(22, FontStyle.Bold);
        public static Font TitrePage => Police(16, FontStyle.Bold);
        public static Font TitreSection => Police(11, FontStyle.Bold);
        public static Font Texte10 => Police(10);
        public static Font Note => Police(9);

        // ── Niveaux d'alerte ──────────────────────────────────────────────
        public static (Color texte, Color fond) Niveau(string? niveau) => niveau switch
        {
            "urgent" => (UrgentTexte, UrgentFond),
            "attention" => (AttentionTexte, AttentionFond),
            "succes" => (SuccesTexte, SuccesFond),
            _ => (InfoTexte, InfoFond)
        };

        // ── Application récursive sur un écran ───────────────────────────
        /// <summary>
        /// À appeler après InitializeComponent(). Les rôles se déclarent par <c>Tag</c> dans le Designer :
        /// boutons « primaire » / « secondaire » / « danger » / « neutre » ; labels « titre » / « section » / « kpi » / « note » /
        /// « urgent » / « attention » / « succes » / « info » ; panneaux « carte ».
        /// </summary>
        public static void Appliquer(Control racine)
        {
            racine.BackColor = Fond;

            foreach (Control c in racine.Controls)
                AppliquerControle(c);
        }

        private static void AppliquerControle(Control c)
        {
            string tag = c.Tag as string ?? "";

            switch (c)
            {
                case Button b:
                    if (tag != "menu")   // les boutons du menu (BoutonMenu) se dessinent eux-mêmes
                        StyleBoutonDepuisTag(b, tag);
                    break;

                case DataGridView g:
                    StyliserGrille(g);
                    break;

                case Label l:
                    StyliserLabel(l, tag);
                    break;

                case TextBox t:
                    t.BackColor = Blanc;
                    t.ForeColor = Texte;
                    break;

                case ComboBox cb:
                    cb.BackColor = Blanc;
                    cb.ForeColor = Texte;
                    break;

                case NumericUpDown n:
                    n.BackColor = Blanc;
                    n.ForeColor = Texte;
                    break;

                case Panel p when tag == "carte":
                    p.BackColor = Blanc;
                    p.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case TableLayoutPanel or FlowLayoutPanel or Panel or GroupBox or SplitContainer or TabControl:
                    if (tag == "carte") c.BackColor = Blanc;
                    else if (c.BackColor == SystemColors.Control || c.BackColor == Color.Transparent) c.BackColor = Fond;
                    break;
            }

            foreach (Control enfant in c.Controls)
                AppliquerControle(enfant);

            if (c is SplitContainer sc)
            {
                foreach (Control e in sc.Panel1.Controls) AppliquerControle(e);
                foreach (Control e in sc.Panel2.Controls) AppliquerControle(e);
            }
            else if (c is TabControl tc)
            {
                foreach (TabPage page in tc.TabPages)
                {
                    page.BackColor = Fond;
                    foreach (Control e in page.Controls) AppliquerControle(e);
                }
            }
        }

        private static void StyleBoutonDepuisTag(Button b, string tag)
        {
            var style = tag switch
            {
                "primaire" => StyleBouton.Primaire,
                "danger" => StyleBouton.Danger,
                "neutre" => StyleBouton.Neutre,
                _ => StyleBouton.Secondaire
            };
            Style(b, style);
        }

        public static void Style(Button b, StyleBouton style)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.AutoSize = true;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            b.Padding = new Padding(12, 4, 12, 4);
            b.MinimumSize = new Size(Math.Max(b.MinimumSize.Width, 88), Math.Max(b.MinimumSize.Height, 36));
            b.Cursor = Cursors.Hand;

            switch (style)
            {
                case StyleBouton.Primaire:
                    b.BackColor = Accent; b.ForeColor = Blanc;
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.MouseOverBackColor = Principal;
                    break;
                case StyleBouton.Danger:
                    b.BackColor = UrgentTexte; b.ForeColor = Blanc;
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#B71C1C");
                    break;
                case StyleBouton.Neutre:
                    b.BackColor = Neutre; b.ForeColor = Blanc;
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#37474F");
                    break;
                default:
                    b.BackColor = Blanc; b.ForeColor = Principal;
                    b.FlatAppearance.BorderSize = 1;
                    b.FlatAppearance.BorderColor = Accent;
                    b.FlatAppearance.MouseOverBackColor = SuccesFond;
                    break;
            }
        }

        public static void StyliserLabel(Label l, string tag)
        {
            switch (tag)
            {
                case "titre": l.Font = TitrePage; l.ForeColor = Principal; break;
                case "section": l.Font = TitreSection; l.ForeColor = Principal; break;
                case "kpi": l.Font = Kpi; l.ForeColor = Texte; break;
                case "note": l.Font = Note; l.ForeColor = Neutre; break;
                case "urgent": l.ForeColor = UrgentTexte; break;
                case "attention": l.ForeColor = AttentionTexte; break;
                case "succes": l.ForeColor = SuccesTexte; break;
                case "info": l.ForeColor = InfoTexte; break;
                default:
                    if (l.ForeColor == SystemColors.ControlText) l.ForeColor = Texte;
                    break;
            }
            if (l.BackColor == SystemColors.Control) l.BackColor = Color.Transparent;
        }

        /// <summary>Style commun des tableaux (règle B0.8). À compléter par colonne : montants avec MarquerMontant.</summary>
        public static void StyliserGrille(DataGridView g)
        {
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.BackgroundColor = Blanc;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = ColorTranslator.FromHtml("#E0E6E0");
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 36;
            g.RowTemplate.Height = 30;
            g.ColumnHeadersDefaultCellStyle.BackColor = Principal;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Blanc;
            g.ColumnHeadersDefaultCellStyle.Font = Police(10, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Principal;
            g.DefaultCellStyle.Font = Texte10;
            g.DefaultCellStyle.ForeColor = Texte;
            g.DefaultCellStyle.BackColor = Blanc;
            g.DefaultCellStyle.SelectionBackColor = Selection;
            g.DefaultCellStyle.SelectionForeColor = Texte;
            g.AlternatingRowsDefaultCellStyle.BackColor = LigneAlternee;

            foreach (DataGridViewColumn col in g.Columns)
            {
                if (col.MinimumWidth < 60) col.MinimumWidth = 60;
                if (col.Tag as string == "montant") MarquerMontant(col);
            }

            // Une seule inscription de l'événement par grille
            g.CellFormatting -= FormaterMontants;
            g.CellFormatting += FormaterMontants;
            g.DataBindingComplete -= AjusterApresLiaison;
            g.DataBindingComplete += AjusterApresLiaison;
        }

        /// <summary>Colonne de montant : alignée à droite, affichée par Format.Montant (« 12 500 KMF »).</summary>
        public static void MarquerMontant(DataGridViewColumn col)
        {
            col.Tag = "montant";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private static void FormaterMontants(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is not DataGridView g || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (g.Columns[e.ColumnIndex].Tag as string != "montant") return;

            switch (e.Value)
            {
                case null:
                case DBNull:
                    e.Value = "—"; e.FormattingApplied = true; break;
                case decimal d: e.Value = Format.Montant(d); e.FormattingApplied = true; break;
                case double db: e.Value = Format.Montant((decimal)db); e.FormattingApplied = true; break;
                case int i: e.Value = Format.Montant((decimal)i); e.FormattingApplied = true; break;
                case long lg: e.Value = Format.Montant((decimal)lg); e.FormattingApplied = true; break;
            }
        }

        /// <summary>Colonnes générées automatiquement : mêmes largeurs minimales et montants repérés par leur en-tête.</summary>
        private static void AjusterApresLiaison(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender is not DataGridView g) return;
            foreach (DataGridViewColumn col in g.Columns)
            {
                if (col.MinimumWidth < 60) col.MinimumWidth = 60;
                if (col.Tag as string == "montant") MarquerMontant(col);
            }
        }
    }
}
