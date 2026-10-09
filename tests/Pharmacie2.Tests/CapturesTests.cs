using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Xunit;

/// <summary>
/// Génère une image PNG de chaque écran dans chaque configuration (base de démonstration réaliste).
/// Active seulement si la variable CAPTURES_DIR est définie (CI : artefact « captures-ecrans »).
/// DrawToBitmap rend mal certains contrôles natifs : les écarts connus sont listés dans le compte rendu.
/// </summary>
public class CapturesTests
{
    [Theory]
    [MemberData(nameof(LayoutTests.Ecrans), MemberType = typeof(LayoutTests))]
    public void Capturer(string nomEcran)
    {
        string? dossier = Environment.GetEnvironmentVariable("CAPTURES_DIR");
        if (string.IsNullOrWhiteSpace(dossier)) return;
        Directory.CreateDirectory(dossier);

        var type = LayoutTests.Decouvrir().Single(t => t.Name == nomEcran);
        var ids = LayoutSeed.PreparerDemo();
        var cts = UiHelper.FermerLesMessages(false);
        try
        {
            UiHelper.EnSta(() =>
            {
                foreach (var cfg in EcranHote.Matrice)
                {
                    try
                    {
                        using var h = EcranHote.Ouvrir(type, ids, cfg);
                        Enregistrer(h.Fenetre, Path.Combine(dossier, $"{nomEcran}_{cfg}.png"));

                        // menu déplié par-dessus le contenu (modes compacts)
                        if (h.Page != null && h.Page.EstCompact)
                        {
                            UiHelper.Appeler(h.Page, "btnMenu_Click", null!, EventArgs.Empty);
                            Application.DoEvents();
                            Enregistrer(h.Fenetre, Path.Combine(dossier, $"{nomEcran}_{cfg}_menu.png"));
                            UiHelper.Appeler(h.Page, "btnMenu_Click", null!, EventArgs.Empty);
                        }

                        // chaque onglet du premier TabControl rencontré
                        var tc = Trouver<TabControl>(h.Fenetre);
                        if (tc != null)
                            for (int i = 1; i < tc.TabPages.Count; i++)
                            {
                                tc.SelectedIndex = i;
                                Application.DoEvents();
                                h.Fenetre.PerformLayout();
                                Enregistrer(h.Fenetre, Path.Combine(dossier, $"{nomEcran}_{cfg}_onglet{i + 1}.png"));
                            }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(Path.Combine(dossier, "erreurs-captures.txt"), $"{nomEcran} {cfg} : {ex.GetType().Name} {ex.Message}\n");
                    }
                }
            });
        }
        finally { cts.Cancel(); Pharmacie2.Services.Theme.EchelleTest = null; }
    }

    private static T? Trouver<T>(Control racine) where T : Control
    {
        foreach (Control c in racine.Controls)
        {
            if (c is T t) return t;
            var r = Trouver<T>(c);
            if (r != null) return r;
        }
        return null;
    }

    private static void Enregistrer(Form f, string chemin)
    {
        Application.DoEvents();
        using var bmp = new Bitmap(Math.Max(1, f.Width), Math.Max(1, f.Height));
        f.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
        bmp.Save(chemin, ImageFormat.Png);

        // arbre des contrôles (nom, type, position, taille) pour comprendre une mise en page sans la voir
        var sb = new System.Text.StringBuilder();
        void Vider(Control c, int niveau)
        {
            sb.AppendLine($"{new string(' ', niveau * 2)}{(string.IsNullOrEmpty(c.Name) ? "-" : c.Name)} [{c.GetType().Name}] {c.Bounds}{(c.Visible ? "" : " (caché)")}");
            foreach (Control k in c.Controls) Vider(k, niveau + 1);
        }
        Vider(f, 0);
        File.WriteAllText(Path.ChangeExtension(chemin, ".txt"), sb.ToString());
    }
}
