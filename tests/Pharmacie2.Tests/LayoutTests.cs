using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Pharmacie2.Models;
using Xunit;

/// <summary>
/// Test automatique de mise en page (règles B0) : pour chaque écran, aucun contrôle visible hors de son parent
/// (sauf parent en défilement vertical), aucun chevauchement, tableaux et graphiques qui s'étirent sur grand écran.
/// </summary>
public class LayoutTests
{
    // Écrans déjà convertis aux règles B0 (la liste grandit au fil de la refonte ; voir TousLesEcransSontCouverts)
    private static readonly HashSet<string> Convertis = new()
    {
        "PageAccueil", "Uc_MaJournee", "Uc_Stock", "Uc_Statistique", "PageCaissier", "FormVente", "Uc_Vente", "FormModificationVente", "FormPaiements", "FormDetailVente", "FormAnnulationVente", "FormSessionCaisse", "Uc_Caisse", "Uc_Depenses", "FormAddDepense", "Uc_Produits", "FormAddProduit", "FormChoixProduit", "Uc_Fournisser", "FormaddFournisseur", "Uc_Commande", "FormCommandeProduit", "FormCommandesFournisseur", "FormReceptionPartielle"
    };

    // Passe à true quand tous les écrans sont convertis : le test de couverture devient bloquant
    private const bool VerifierCouvertureComplete = false;

    private static readonly Size[] Tailles = { new(1366, 700), new(1600, 860), new(1920, 1040) };
    private static readonly Size EcranMini = new(1366, 768);

    private static readonly string[] Composants = { ".Composants" };

    public static IEnumerable<Type> Decouvrir()
        => typeof(User).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsPublic
                        && (t.IsSubclassOf(typeof(Form)) || t.IsSubclassOf(typeof(UserControl)))
                        && !Composants.Any(c => (t.Namespace ?? "").EndsWith(c)))
            .OrderBy(t => t.Name);

    public static IEnumerable<object[]> Ecrans() => Decouvrir().Select(t => new object[] { t.Name });

    // ───────── fabriques (écrans à constructeur paramétré) ─────────
    private static Control Creer(Type t, LayoutSeed.Ids ids)
    {
        switch (t.Name)
        {
            case "FormCleSecours": return new Pharmacie2.views.FormCleSecours("K7MQ-4XRT-9WPA-H3ZD");
            case "FormAddProduit": return new Pharmacie2.views.FormAddProduit(ids.Produit);
            case "FormAnnulationVente": return new Pharmacie2.views.FormAnnulationVente(ids.Vente);
            case "FormCommandeProduit": return new Pharmacie2.views.FormCommandeProduit(ids.Produit, "Doliprane 500 mg");
            case "FormCommandesFournisseur": return new Pharmacie2.views.FormCommandesFournisseur(ids.Fournisseur, "Pharma Distribution");
            case "FormDetailVente": return new Pharmacie2.views.FormDetailVente(ids.Vente);
            case "FormModificationVente": return new Pharmacie2.views.FormModificationVente(ids.Vente);
            case "FormPaiements": return new Pharmacie2.views.FormPaiements(ids.Vente);
            case "FormPeriodeExportMutuelle": return new Pharmacie2.views.FormPeriodeExportMutuelle("Entreprise Moheli");
            case "FormReceptionPartielle": return new Pharmacie2.views.FormReceptionPartielle(ids.Commande);
            case "FormSessionCaisse": return new Pharmacie2.views.FormSessionCaisse(ids.Caissier);
            case "PageAccueil": return new Pharmacie2.views.PageAccueil(SessionUtilisateur.Courant);
            case "PageCaissier":
                {
                    using var ctx = new AppDbContext();
                    return new Pharmacie2.views.PageCaissier(ctx.Users.First(u => u.Id == ids.Caissier));
                }
            default: return (Control)Activator.CreateInstance(t);
        }
    }

    // ───────── le test ─────────
    [Theory]
    [MemberData(nameof(Ecrans))]
    public void Mise_en_page_conforme(string nomEcran)
    {
        if (!Convertis.Contains(nomEcran))
            return;   // pas encore converti aux règles B0

        var type = Decouvrir().Single(t => t.Name == nomEcran);
        var ids = LayoutSeed.Preparer();
        var cts = UiHelper.FermerLesMessages(false);
        var erreurs = new List<string>();
        try
        {
            UiHelper.EnSta(() => ExecuterVerifications(type, ids, erreurs));
        }
        finally { cts.Cancel(); }

        Assert.True(erreurs.Count == 0, nomEcran + " :\n  " + string.Join("\n  ", erreurs));
    }

    [Fact]
    public void Tous_les_ecrans_sont_couverts_par_le_test()
    {
        if (!VerifierCouvertureComplete) return;
        var manquants = Decouvrir().Select(t => t.Name).Where(n => !Convertis.Contains(n)).ToList();
        Assert.True(manquants.Count == 0, "Écrans non convertis : " + string.Join(", ", manquants));
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
        bool estUc = type.IsSubclassOf(typeof(UserControl));
        bool estPage = type.Name is "PageAccueil" or "PageCaissier";

        if (estUc || estPage || EstRedimensionnable(type, ids))
        {
            var tailles = new Dictionary<Size, Dictionary<string, Size>>();
            foreach (var taille in Tailles)
            {
                var ctrl = Creer(type, ids);
                Form hote;
                if (ctrl is Form f)
                {
                    hote = f;
                    hote.WindowState = FormWindowState.Normal;
                    hote.StartPosition = FormStartPosition.Manual;
                    hote.Location = new Point(0, 0);
                    hote.MinimumSize = Size.Empty;
                    hote.ClientSize = estPage ? taille : taille;
                }
                else
                {
                    // UserControl : conteneur de la largeur de la zone de contenu (écran moins le menu de 220 px)
                    hote = new Form { StartPosition = FormStartPosition.Manual, Location = new Point(0, 0), FormBorderStyle = FormBorderStyle.None };
                    ctrl.Dock = DockStyle.Fill;
                    hote.Controls.Add(ctrl);
                    hote.ClientSize = new Size(taille.Width - 220, taille.Height);
                }

                try
                {
                    hote.Show();
                    if (ctrl is Form) { hote.WindowState = FormWindowState.Normal; hote.ClientSize = taille; }   // une page « maximisée » reprend la taille demandée
                    Application.DoEvents();
                    hote.PerformLayout();
                    Application.DoEvents();

                    var locales = new List<string>();
                    Verifier(hote, type.Name, locales);
                    VerifierOnglets(hote, type.Name, locales);
                    erreurs.AddRange(locales.Select(e => $"[{taille.Width}x{taille.Height}] {e}"));
                    tailles[taille] = Etirables(hote);
                }
                finally { hote.Close(); hote.Dispose(); }
            }

            // Sur grand écran, tableaux et graphiques doivent être plus grands qu'en 1366 (ils s'étirent)
            var petit = tailles[Tailles[0]];
            var grand = tailles[Tailles[2]];
            foreach (var kv in petit)
            {
                if (grand.TryGetValue(kv.Key, out var s) && s.Width <= kv.Value.Width && s.Height <= kv.Value.Height)
                    erreurs.Add($"{kv.Key} ne s'étire pas ({kv.Value.Width}x{kv.Value.Height} en 1366, {s.Width}x{s.Height} en 1920)");
            }
        }
        else
        {
            // Dialogue : taille naturelle, jamais plus grand que 90 % de l'écran, ouverture centrée
            var f = (Form)Creer(type, ids);
            try
            {
                f.Show();
                Application.DoEvents();
                f.PerformLayout();
                Application.DoEvents();

                if (f.Width > EcranMini.Width * 0.9 || f.Height > EcranMini.Height * 0.9)
                    erreurs.Add($"dialogue trop grand : {f.Width}x{f.Height} (max {(int)(EcranMini.Width * 0.9)}x{(int)(EcranMini.Height * 0.9)})");
                if (f.StartPosition is not (FormStartPosition.CenterParent or FormStartPosition.CenterScreen))
                    erreurs.Add($"dialogue non centré (StartPosition = {f.StartPosition})");

                Verifier(f, type.Name, erreurs);
                VerifierOnglets(f, type.Name, erreurs);
            }
            finally { f.Close(); f.Dispose(); }
        }
    }

    private static bool EstRedimensionnable(Type type, LayoutSeed.Ids ids)
    {
        if (!type.IsSubclassOf(typeof(Form))) return false;
        using var f = (Form)Creer(type, ids);
        return f.FormBorderStyle is FormBorderStyle.Sizable or FormBorderStyle.SizableToolWindow;
    }

    private static Dictionary<string, Size> Etirables(Control racine)
    {
        var d = new Dictionary<string, Size>();
        void Visiter(Control c)
        {
            foreach (Control k in c.Controls)
            {
                if (!k.Visible) continue;
                if (k is DataGridView || k.GetType().Name == "GraphiqueBarres")
                    d[k.Name] = k.Size;
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

    private static bool Descendre(Control c)
        => c is Panel or TableLayoutPanel or FlowLayoutPanel or GroupBox or UserControl or Form or TabControl or TabPage or SplitContainer or SplitterPanel;

    public static void Verifier(Control parent, string chemin, List<string> erreurs, bool grandEcran = false)
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
                // champs de saisie : largeur maximale raisonnable, même sur grand écran
                if (c.Width > 520)
                    erreurs.Add($"{nom} trop large pour un champ de saisie ({c.Width}px)");
            }

            if (Descendre(c))
                Verifier(c, nom, erreurs, grandEcran);
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
