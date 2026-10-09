using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Models;
using Xunit;

/// <summary>
/// Test automatique de mise en page. Cible : 1280 × 640 logiques minimum (écran 1920 × 1080 à 150 %),
/// plus 1366 × 700 à 100 % et 1920 × 1040 à 100 %. Les écrans y sont ouverts comme dans l'application
/// (zone de contenu défilante pour les UserControl) avec zoom simulé.
/// Vérifie : aucun contrôle hors de son parent, aucun chevauchement, champs de saisie jamais écrasés,
/// tableaux et graphiques qui s'étirent, écran lisible à sa taille minimale sans défilement.
/// </summary>
public class LayoutTests
{
    // Écrans déjà conformes à la nouvelle cible. Les autres sont photographiés (CapturesTests) mais pas encore bloquants.
    private static readonly HashSet<string> Conformes = new()
    {
        "PageAccueil", "Uc_MaJournee", "Uc_Stock", "Uc_Vente", "FormVente", "Uc_Administration"
    };

    // Passe à true quand tous les écrans sont conformes : le test de couverture devient bloquant
    private const bool VerifierCouvertureComplete = false;

    private static readonly Size EcranMiniLogique = new(1280, 720);   // plus petit écran visé, en pixels logiques

    private static readonly string[] Composants = { ".Composants" };

    public static IEnumerable<Type> Decouvrir()
        => typeof(User).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsPublic
                        && (t.IsSubclassOf(typeof(Form)) || t.IsSubclassOf(typeof(UserControl)))
                        && !Composants.Any(c => (t.Namespace ?? "").EndsWith(c)))
            .OrderBy(t => t.Name);

    public static IEnumerable<object[]> Ecrans() => Decouvrir().Select(t => new object[] { t.Name });

    [Theory]
    [MemberData(nameof(Ecrans))]
    public void Mise_en_page_conforme(string nomEcran)
    {
        if (!Conformes.Contains(nomEcran))
            return;

        var type = Decouvrir().Single(t => t.Name == nomEcran);
        var ids = LayoutSeed.PreparerDemo();
        var cts = UiHelper.FermerLesMessages(false);
        var erreurs = new List<string>();
        try
        {
            UiHelper.EnSta(() => ExecuterVerifications(type, ids, erreurs));
        }
        finally { cts.Cancel(); Pharmacie2.Services.Theme.EchelleTest = null; }

        Assert.True(erreurs.Count == 0, nomEcran + " :\n  " + string.Join("\n  ", erreurs));
    }

    [Fact]
    public void Tous_les_ecrans_sont_couverts_par_le_test()
    {
        if (!VerifierCouvertureComplete) return;
        var manquants = Decouvrir().Select(t => t.Name).Where(n => !Conformes.Contains(n)).ToList();
        Assert.True(manquants.Count == 0, "Écrans non conformes : " + string.Join(", ", manquants));
    }

    // ───────── le détecteur lui-même doit détecter les défauts ─────────
    [Fact]
    public void Le_detecteur_signale_un_controle_hors_cadre_et_un_chevauchement()
    {
        var erreurs = new List<string>();
        UiHelper.EnSta(() =>
        {
            using var f = new Form { ClientSize = new Size(400, 300), StartPosition = FormStartPosition.Manual, Location = new Point(0, 0) };
            var hors = new Button { Name = "horsCadre", Location = new Point(350, 10), Size = new Size(120, 30) };
            var a = new Button { Name = "a", Location = new Point(10, 100), Size = new Size(100, 40) };
            var b = new Button { Name = "b", Location = new Point(50, 110), Size = new Size(100, 40) };
            f.Controls.AddRange(new Control[] { hors, a, b });
            f.Show();
            f.PerformLayout();
            Verifier(f, "f", erreurs);
        });
        Assert.Contains(erreurs, e => e.Contains("horsCadre") && e.Contains("hors"));
        Assert.Contains(erreurs, e => e.Contains("chevauche"));
    }

    [Fact]
    public void Le_detecteur_signale_un_champ_de_saisie_trop_etroit()
    {
        var erreurs = new List<string>();
        UiHelper.EnSta(() =>
        {
            using var f = new Form { ClientSize = new Size(400, 300), StartPosition = FormStartPosition.Manual, Location = new Point(0, 0) };
            f.Controls.Add(new ComboBox { Name = "cbEtroit", Location = new Point(10, 60), Width = 40 });
            f.Show();
            f.PerformLayout();
            Verifier(f, "f", erreurs);
        });
        Assert.Contains(erreurs, e => e.Contains("cbEtroit") && e.Contains("trop étroit"));
    }

    [Fact]
    public void Le_detecteur_accepte_une_mise_en_page_saine()
    {
        var erreurs = new List<string>();
        UiHelper.EnSta(() =>
        {
            using var f = new Form { ClientSize = new Size(400, 300), StartPosition = FormStartPosition.Manual, Location = new Point(0, 0) };
            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlp.Controls.Add(new Button { Dock = DockStyle.Fill, Text = "A" }, 0, 0);
            tlp.Controls.Add(new Button { Dock = DockStyle.Fill, Text = "B" }, 1, 0);
            f.Controls.Add(tlp);
            f.Show();
            f.PerformLayout();
            Verifier(f, "f", erreurs);
        });
        Assert.Empty(erreurs);
    }

    // ───────── vérifications ─────────
    private static void ExecuterVerifications(Type type, LayoutSeed.Ids ids, List<string> erreurs)
    {
        var etirables = new Dictionary<Config, Dictionary<string, Size>>();

        foreach (var cfg in EcranHote.Matrice)
        {
            using var h = EcranHote.Ouvrir(type, ids, cfg);
            var locales = new List<string>();

            if (h.Dialogue)
            {
                // Dialogue : jamais plus grand que 90 % du plus petit écran (en pixels logiques), ouverture centrée
                var f = h.Fenetre;
                double largeur = f.Width / cfg.Zoom, hauteur = f.Height / cfg.Zoom;
                if (largeur > EcranMiniLogique.Width * 0.9 || hauteur > EcranMiniLogique.Height * 0.9)
                    locales.Add($"dialogue trop grand : {largeur:0}x{hauteur:0} logiques (max {EcranMiniLogique.Width * 0.9:0}x{EcranMiniLogique.Height * 0.9:0})");
                if (f.StartPosition is not (FormStartPosition.CenterParent or FormStartPosition.CenterScreen))
                    locales.Add($"dialogue non centré (StartPosition = {f.StartPosition})");
                if (f.AcceptButton == null && f.CancelButton == null)
                    locales.Add("ni bouton par défaut (Entrée) ni bouton d'annulation (Échap)");
            }
            else if (h.Ecran is UserControl uc)
            {
                // L'écran doit tenir dans la zone au minimum garanti, sans défilement horizontal ni vertical
                var zone = EcranHote.Chercher(h.Fenetre, "panelContent")!;
                // (la page défile verticalement en mode compact : seule la largeur est contraignante)
                if (uc.Width > zone.ClientSize.Width + 1)
                    locales.Add($"{h.Ecran.Name} : largeur minimale {uc.Width / cfg.Zoom:0} px logiques > zone disponible {zone.ClientSize.Width / cfg.Zoom:0}");
                if (uc.Height > zone.ClientSize.Height + 1)
                    locales.Add($"{h.Ecran.Name} : hauteur minimale {uc.Height / cfg.Zoom:0} px logiques > zone disponible {zone.ClientSize.Height / cfg.Zoom:0} (défilement vertical nécessaire)");
            }

            Verifier(h.Fenetre, type.Name, locales);
            VerifierOnglets(h.Fenetre, type.Name, locales);
            VerifierGrilles(h.Fenetre, type.Name, locales, cfg);
            erreurs.AddRange(locales.Select(e => $"[{cfg}] {e}"));
            etirables[cfg] = Etirables(h.Fenetre, cfg.Zoom);
        }

        // Sur grand écran, tableaux et graphiques (en pixels logiques) doivent être plus grands qu'au minimum garanti
        var petit = etirables[EcranHote.Matrice[0]];
        var grand = etirables[EcranHote.Matrice[2]];
        foreach (var kv in petit)
            if (grand.TryGetValue(kv.Key, out var s) && s.Width <= kv.Value.Width && s.Height <= kv.Value.Height)
                erreurs.Add($"{kv.Key} ne s'étire pas ({kv.Value.Width}x{kv.Value.Height} au minimum, {s.Width}x{s.Height} en 1920)");
    }

    private static Dictionary<string, Size> Etirables(Control racine, float zoom)
    {
        var d = new Dictionary<string, Size>();
        void Visiter(Control c)
        {
            foreach (Control k in c.Controls)
            {
                if (!k.Visible) continue;
                if (k is DataGridView || k.GetType().Name == "GraphiqueBarres")
                    d[k.Name] = new Size((int)(k.Width / zoom), (int)(k.Height / zoom));
                Visiter(k);
            }
        }
        Visiter(racine);
        return d;
    }

    /// <summary>Affiche successivement chaque onglet des TabControl et vérifie sa mise en page.</summary>
    private static void VerifierOnglets(Control racine, string chemin, List<string> erreurs)
    {
        foreach (Control c in racine.Controls)
        {
            if (c is TabControl tc)
            {
                for (int i = 0; i < tc.TabPages.Count; i++)
                {
                    tc.SelectedIndex = i;
                    Application.DoEvents();
                    tc.PerformLayout();
                    tc.TabPages[i].PerformLayout();
                    Application.DoEvents();
                    Verifier(tc.TabPages[i], $"{chemin}/{tc.Name}[{tc.TabPages[i].Name}]", erreurs);
                }
                tc.SelectedIndex = 0;
            }
            else
            {
                VerifierOnglets(c, chemin, erreurs);
            }
        }
    }

    /// <summary>Aucun en-tête de colonne ni cellule de date tronqué : texte mesuré ≤ largeur de la colonne.</summary>
    private static readonly string[] ColonnesNoms = { "Produit", "Client", "Fournisseur", "Nom", "Mutuelle", "Mutuelle / Employeur", "Description" };

    private static void VerifierGrilles(Control racine, string chemin, List<string> erreurs, Config cfg)
    {
        foreach (Control c in racine.Controls)
        {
            if (c is DataGridView g && g.Visible)
            {
                // 1. Polices des styles de cellule : toujours celle de la grille (même taille), à chaque configuration
                var baseFont = g.DefaultCellStyle.Font ?? g.Font;
                bool signale = false;
                DataGridViewCellFormattingEventHandler suivi = (s, e) =>
                {
                    if (!signale && e.RowIndex >= 0 && e.CellStyle.Font != null && Math.Abs(e.CellStyle.Font.SizeInPoints - baseFont.SizeInPoints) > 0.05f)
                    {
                        signale = true;
                        erreurs.Add($"{chemin}/{g.Name} : police de cellule {e.CellStyle.Font.SizeInPoints}pt différente de celle de la grille ({baseFont.SizeInPoints}pt)");
                    }
                };
                g.CellFormatting += suivi;
                try
                {
                    using var bmp = new Bitmap(Math.Max(1, g.Width), Math.Max(1, g.Height));
                    g.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));   // le dessin déclenche CellFormatting
                }
                finally { g.CellFormatting -= suivi; }

                // 2. Les noms (produit, client, fournisseur…) ne sont jamais tronqués en 1366 × 700 et 1920 × 1040 à 100 %
                if (cfg.Zoom == 1f && cfg.W >= 1366)
                {
                    var policeNom = g.DefaultCellStyle.Font ?? g.Font;
                    foreach (DataGridViewColumn col in g.Columns)
                    {
                        if (!col.Visible || !ColonnesNoms.Contains(col.HeaderText)) continue;
                        foreach (DataGridViewRow r in g.Rows)
                        {
                            string t = Convert.ToString(r.Cells[col.Index].FormattedValue) ?? "";
                            if (TextRenderer.MeasureText(t, policeNom).Width + 8 > col.Width)
                            {
                                erreurs.Add($"{chemin}/{g.Name} : « {t} » tronqué dans la colonne {col.HeaderText} ({col.Width}px)");
                                break;
                            }
                        }
                    }
                }

                var policeEntete = g.ColumnHeadersDefaultCellStyle.Font ?? g.Font;
                var policeCellule = g.DefaultCellStyle.Font ?? g.Font;
                foreach (DataGridViewColumn col in g.Columns)
                {
                    if (!col.Visible || string.IsNullOrEmpty(col.HeaderText)) continue;
                    int besoin = TextRenderer.MeasureText(col.HeaderText, policeEntete).Width + 8;
                    if (besoin > col.Width)
                        erreurs.Add($"{chemin}/{g.Name} : en-tête « {col.HeaderText} » tronqué ({besoin}px pour {col.Width}px)");
                    if (col.HeaderText == "Date")
                        foreach (DataGridViewRow r in g.Rows)
                        {
                            string texte = Convert.ToString(r.Cells[col.Index].FormattedValue) ?? "";
                            if (TextRenderer.MeasureText(texte, policeCellule).Width + 8 > col.Width)
                            {
                                erreurs.Add($"{chemin}/{g.Name} : date « {texte} » tronquée dans {col.Width}px");
                                break;
                            }
                        }
                }
            }
            VerifierGrilles(c, chemin, erreurs, cfg);
        }
    }

    private static bool Descendre(Control c)
        => c is Panel or TableLayoutPanel or FlowLayoutPanel or GroupBox or UserControl or Form or TabControl or TabPage or SplitContainer or SplitterPanel;

    public static void Verifier(Control parent, string chemin, List<string> erreurs)
    {
        var visibles = parent.Controls.Cast<Control>().Where(c => c.Visible).ToList();
        bool defilement = parent is ScrollableControl sc && sc.AutoScroll;
        var zone = parent.ClientRectangle;

        foreach (var c in visibles)
        {
            string nom = chemin + "/" + (string.IsNullOrEmpty(c.Name) ? c.GetType().Name : c.Name);
            const int tol = 1;

            if (!defilement)
            {
                if (c.Left < -tol || c.Top < -tol || c.Right > zone.Width + tol || c.Bottom > zone.Height + tol)
                    erreurs.Add($"{nom} hors de son parent : {c.Bounds} dans {zone.Size}");
            }
            else if (c.Left < -tol || c.Right > zone.Width + tol)
            {
                erreurs.Add($"{nom} déborde horizontalement (parent en défilement) : {c.Bounds} dans {zone.Size}");
            }

            if (c is TextBox { Multiline: false } or ComboBox or NumericUpDown)
            {
                // champs de saisie : jamais écrasés
                if (c.Height < c.Font.Height + 6)
                    erreurs.Add($"{nom} trop bas pour saisir ({c.Height}px, police {c.Font.Height}px)");
                if (c.Width < 80)
                    erreurs.Add($"{nom} trop étroit pour saisir ({c.Width}px)");
            }

            if (Descendre(c))
                Verifier(c, nom, erreurs);
        }

        // Chevauchement entre frères visibles
        for (int i = 0; i < visibles.Count; i++)
            for (int j = i + 1; j < visibles.Count; j++)
            {
                var inter = Rectangle.Intersect(visibles[i].Bounds, visibles[j].Bounds);
                if (inter.Width * inter.Height > 16)
                    erreurs.Add($"{chemin} : {NomDe(visibles[i])} chevauche {NomDe(visibles[j])} ({inter.Width}x{inter.Height})");
            }
    }

    private static string NomDe(Control c) => string.IsNullOrEmpty(c.Name) ? c.GetType().Name : c.Name;
}
