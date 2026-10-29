using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Pharmacie2;

/// <summary>Aides pour piloter des formulaires WinForms sans les afficher (tests).</summary>
public static class UiHelper
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string cls, string title);
    [DllImport("user32.dll")] static extern bool PostMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetWindowText(IntPtr h, System.Text.StringBuilder sb, int max);

    public static readonly List<string> Titres = new();

    /// <summary>Messages d'erreur (icône Erreur ou titre « Erreur ») émis par l'application depuis le dernier <see cref="ViderErreurs"/>.</summary>
    public static readonly List<string> ErreursAffichees = new();

    private static readonly List<Func<string, bool>> Repondeurs = new();

    public static void ViderErreurs() { lock (ErreursAffichees) ErreursAffichees.Clear(); }

    public static string[] Erreurs() { lock (ErreursAffichees) return ErreursAffichees.ToArray(); }

    /// <summary>
    /// Initialisation de tout le projet de tests, avant le moindre contrôle : une exception non gérée dans un écran fait échouer le test
    /// (jamais de boîte « Une exception non gérée s'est produite » à laquelle personne ne répondrait), et aucun message de l'application
    /// n'ouvre de fenêtre modale : ils sont interceptés, notés, et reçoivent une réponse automatique.
    /// </summary>
    [ModuleInitializer]
    internal static void InitialiserLesTests()
    {
        ConfigurerExceptions();
        Messages.Intercepteur = Intercepter;
    }

    private static void ConfigurerExceptions()
    {
        try { Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException); }
        catch (InvalidOperationException) { /* déjà configuré après la création d'un contrôle sur ce thread : sans importance ici */ }
    }

    private static DialogResult? Intercepter(MessageAffiche m)
    {
        lock (Titres) Titres.Add(m.Titre);
        if (m.Icone == MessageBoxIcon.Error || m.Titre == "Erreur")
            lock (ErreursAffichees) ErreursAffichees.Add($"{m.Titre} : {m.Texte}");

        bool oui;
        lock (Repondeurs) oui = Repondeurs.Count > 0 && Repondeurs[^1](m.Titre);
        return m.Boutons switch
        {
            MessageBoxButtons.OK => DialogResult.OK,
            MessageBoxButtons.OKCancel => oui ? DialogResult.OK : DialogResult.Cancel,
            MessageBoxButtons.YesNo => oui ? DialogResult.Yes : DialogResult.No,
            MessageBoxButtons.YesNoCancel => oui ? DialogResult.Yes : DialogResult.No,
            MessageBoxButtons.RetryCancel => DialogResult.Cancel,
            _ => DialogResult.OK
        };
    }

    /// <summary>
    /// Ferme toute MessageBox : répond « Oui » (ou « Non » via WM_CLOSE) et note son titre.
    /// </summary>
    public static CancellationTokenSource FermerLesMessages(bool repondreOui) => FermerLesMessages(_ => repondreOui);

    public static void ViderTitres() { lock (Titres) Titres.Clear(); }

    /// <summary>Comme ci-dessus, mais la réponse (Oui / Non) dépend du titre de la fenêtre.</summary>
    public static CancellationTokenSource FermerLesMessages(Func<string, bool> reponseOui)
    {
        var cts = new CancellationTokenSource();
        lock (Repondeurs) Repondeurs.Add(reponseOui);
        cts.Token.Register(() => { lock (Repondeurs) Repondeurs.Remove(reponseOui); });
        // les messages de l'application sont interceptés (voir Intercepter) ; ce fil ne ferme que les autres fenêtres modales (dialogues)
        new Thread(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                var h = FindWindow("#32770", null);
                if (h != IntPtr.Zero)
                {
                    var sb = new System.Text.StringBuilder(256);
                    GetWindowText(h, sb, 256);
                    lock (Titres) Titres.Add(sb.ToString());
                    if (reponseOui(sb.ToString()))
                    {
                        PostMessage(h, 0x0111, (IntPtr)6, IntPtr.Zero);   // WM_COMMAND IDYES
                        PostMessage(h, 0x0111, (IntPtr)1, IntPtr.Zero);   // WM_COMMAND IDOK
                    }
                    else
                    {
                        PostMessage(h, 0x0111, (IntPtr)7, IntPtr.Zero);   // WM_COMMAND IDNO
                        PostMessage(h, 0x0111, (IntPtr)1, IntPtr.Zero);   // WM_COMMAND IDOK
                        PostMessage(h, 0x0111, (IntPtr)2, IntPtr.Zero);   // WM_COMMAND IDCANCEL
                    }
                    PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero);     // WM_CLOSE (secours)
                }
                Thread.Sleep(100);
            }
        })
        { IsBackground = true }.Start();
        return cts;
    }

    public static bool TitreVu(string titre) { lock (Titres) return Titres.Contains(titre); }

    public static T Champ<T>(object o, string nom)
        => (T)o.GetType().GetField(nom, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(o);

    public static void Appeler(object o, string methode, params object[] args)
        => o.GetType().GetMethod(methode, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Invoke(o, args);

    public static void EnSta(Action a)
    {
        Exception err = null;
        var t = new Thread(() => { ConfigurerExceptions(); try { a(); } catch (Exception e) { err = e; } });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();
        if (err != null) throw err;
    }
}
