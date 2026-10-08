using System.Reflection;
using System.Runtime.InteropServices;

/// <summary>Aides pour piloter des formulaires WinForms sans les afficher (tests).</summary>
public static class UiHelper
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string cls, string title);
    [DllImport("user32.dll")] static extern bool PostMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetWindowText(IntPtr h, System.Text.StringBuilder sb, int max);

    public static readonly List<string> Titres = new();

    /// <summary>
    /// Ferme toute MessageBox : répond « Oui » (ou « Non » via WM_CLOSE) et note son titre.
    /// </summary>
    public static CancellationTokenSource FermerLesMessages(bool repondreOui)
    {
        var cts = new CancellationTokenSource();
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
                    if (repondreOui)
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
        var t = new Thread(() => { try { a(); } catch (Exception e) { err = e; } });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();
        if (err != null) throw err;
    }
}
