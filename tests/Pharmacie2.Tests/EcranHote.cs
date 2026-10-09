using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

/// <summary>Configuration d'affichage simulée : taille de fenêtre en unités LOGIQUES et zoom Windows.</summary>
public sealed record Config(int W, int H, float Zoom)
{
    public override string ToString() => $"{W}x{H}@{(int)Math.Round(Zoom * 100)}";
    public int Phys(double logique) => (int)Math.Round(logique * Zoom);
}

/// <summary>Un écran ouvert dans son hôte de test (fenêtre normale, ou zone de contenu défilante pour un UserControl).</summary>
public sealed class Hote : IDisposable
{
    public Form Fenetre = null!;
    public Control Ecran = null!;
    public bool Dialogue;
    public void Dispose()
    {
        Theme.EchelleTest = null;
        try { Fenetre.Close(); } catch { }
        Fenetre.Dispose();
    }
}

/// <summary>
/// Ouvre un écran comme le fait l'application, puis simule un zoom Windows en agrandissant polices, tailles et minimums
/// (Control.Scale). Les écrans sont conçus en pixels à 96 DPI (AutoScaleMode.Dpi) : 100 % = taille de conception.
/// </summary>
public static class EcranHote
{
    public const int LargeurMenu = 230;
    private static readonly Size Grand = new(6000, 4000);

    /// <summary>1280 × 640 logiques à 150 % (écran 1920 × 1080 avec barre des tâches), 1366 × 700 à 100 %, 1920 × 1040 à 100 %.</summary>
    public static readonly Config[] Matrice =
    {
        new(1280, 640, 1.5f), new(1366, 700, 1.0f), new(1920, 1040, 1.0f)
    };

    public static Control Creer(Type t, LayoutSeed.Ids ids)
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
            case "FormReinitialisationMdp": return new Pharmacie2.views.FormReinitialisationMdp("admin");
            case "FormSessionCaisse": return new Pharmacie2.views.FormSessionCaisse(ids.Caissier);
            case "PageAccueil": return new Pharmacie2.views.PageAccueil(SessionUtilisateur.Courant);
            case "PageCaissier":
                {
                    using var ctx = new AppDbContext();
                    return new Pharmacie2.views.PageCaissier(ctx.Users.First(u => u.Id == ids.Caissier));
                }
            default: return (Control)Activator.CreateInstance(t)!;
        }
    }

    public static bool EstPage(Type t) => t.Name is "PageAccueil" or "PageCaissier";

    public static bool EstRedimensionnable(Control c)
        => c is Form f && f.FormBorderStyle is FormBorderStyle.Sizable or FormBorderStyle.SizableToolWindow;

    /// <summary>
    /// Ouvre l'écran dans la configuration demandée.
    /// UserControl : hébergé comme dans PageAccueil (zone défilante, écran = max(zone, minimum)).
    /// Page ou fenêtre redimensionnable : taille de fenêtre = taille de la configuration.
    /// Dialogue : taille naturelle (zoomée), à l'écran.
    /// </summary>
    public static Hote Ouvrir(Type type, LayoutSeed.Ids ids, Config cfg)
    {
        Theme.EchelleTest = cfg.Zoom;
        var ecran = Creer(type, ids);
        var hote = new Hote { Ecran = ecran };

        if (ecran is not Form form)
        {
            var fen = new Form { StartPosition = FormStartPosition.Manual, Location = Point.Empty, FormBorderStyle = FormBorderStyle.None, AutoScaleMode = AutoScaleMode.None };
            var zone = new Panel { Dock = DockStyle.Fill, Name = "zoneContenu" };
            fen.Controls.Add(zone);
            Hebergement.Heberger(zone, ecran);
            Zoomer(fen, cfg.Zoom);
            fen.MaximumSize = Grand;   // sans cela Windows limite la fenêtre à la taille de l'écran du poste de CI
            fen.ClientSize = new Size(cfg.Phys(cfg.W - LargeurMenu), cfg.Phys(cfg.H));
            hote.Fenetre = fen;
            fen.Show();
        }
        else if (EstPage(type) || EstRedimensionnable(form))
        {
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = Point.Empty;
            form.MinimumSize = Size.Empty;
            Zoomer(form, cfg.Zoom);
            form.MaximumSize = Grand;
            form.ClientSize = new Size(cfg.Phys(cfg.W), cfg.Phys(cfg.H));
            hote.Fenetre = form;
            form.Show();
            form.WindowState = FormWindowState.Normal;
            form.ClientSize = new Size(cfg.Phys(cfg.W), cfg.Phys(cfg.H));
        }
        else
        {
            hote.Dialogue = true;
            Zoomer(form, cfg.Zoom);
            hote.Fenetre = form;
            form.Show();
        }

        Application.DoEvents();
        hote.Fenetre.PerformLayout();
        Application.DoEvents();
        return hote;
    }

    /// <summary>Simule un zoom : polices, tailles, minimums, hauteurs de grilles multipliés par le facteur.</summary>
    public static void Zoomer(Control racine, float f)
    {
        if (Math.Abs(f - 1f) < 0.001f) return;

        var tous = new List<Control>();
        void Collecter(Control c) { tous.Add(c); foreach (Control k in c.Controls) Collecter(k); }
        Collecter(racine);

        var fonts = tous.ToDictionary(c => c, c => c.Font);
        var mins = tous.ToDictionary(c => c, c => (c.MinimumSize, c.MaximumSize));
        var grilles = tous.OfType<DataGridView>().ToDictionary(g => g, g => (g.ColumnHeadersHeight, g.RowTemplate.Height,
            g.Columns.Cast<DataGridViewColumn>().Select(col => col.MinimumWidth).ToArray()));

        foreach (var c in tous)
        {
            var o = fonts[c];
            c.Font = new Font(o.FontFamily, o.Size * f, o.Style, GraphicsUnit.Point);
        }
        racine.Scale(new SizeF(f, f));

        foreach (var c in tous)
        {
            var (mn, mx) = mins[c];
            c.MinimumSize = new Size((int)Math.Round(mn.Width * f), (int)Math.Round(mn.Height * f));
            c.MaximumSize = new Size((int)Math.Round(mx.Width * f), (int)Math.Round(mx.Height * f));
        }
        foreach (var kv in grilles)
        {
            var g = kv.Key;
            var (hEntete, hLigne, minLarg) = kv.Value;
            g.ColumnHeadersHeight = (int)Math.Round(hEntete * f);
            g.RowTemplate.Height = (int)Math.Round(hLigne * f);
            foreach (DataGridViewRow r in g.Rows) r.Height = g.RowTemplate.Height;
            for (int i = 0; i < g.Columns.Count; i++) g.Columns[i].MinimumWidth = (int)Math.Round(minLarg[i] * f);
        }
    }
}
