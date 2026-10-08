using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Pharmacie2.Models;
using Xunit;

public class FormAddProduitTests
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string cls, string title);
    [DllImport("user32.dll")] static extern bool PostMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetWindowText(IntPtr h, System.Text.StringBuilder sb, int max);

    private static readonly List<string> Boites = new();

    /// <summary>Ferme automatiquement toute MessageBox (WM_CLOSE) pour ne pas bloquer le test.</summary>
    private static CancellationTokenSource Fermeur()
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
                    lock (Boites) Boites.Add(sb.ToString());
                    PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero);
                }
                Thread.Sleep(100);
            }
        })
        { IsBackground = true }.Start();
        return cts;
    }

    private static T Champ<T>(object f, string nom)
        => (T)f.GetType().GetField(nom, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(f);

    private static void Enregistrer(int pid, Action<Pharmacie2.views.FormAddProduit> modif)
    {
        Exception err = null;
        var t = new Thread(() =>
        {
            try
            {
                var f = new Pharmacie2.views.FormAddProduit(pid);
                modif(f);
                f.GetType().GetMethod("btnEnregistrer_Click", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(f, new object[] { null, EventArgs.Empty });
            }
            catch (Exception e) { err = e; }
        });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();
        if (err != null) throw err;
    }

    private static long Stock(int pid) => TestDb.Scalaire($"SELECT QuantiteEnStock FROM produits WHERE Id={pid}");

    [Fact]
    public void Phase5_modifier_le_prix_ne_touche_pas_au_stock_et_garde_la_boite_entamee()
    {
        TestDb.Reinitialiser();
        TestDb.MigrerTout();
        int pid = TestDb.NouveauProduit("Paracetamol", 9, 5);
        var cts = Fermeur();
        try
        {
            Enregistrer(pid, f => Champ<NumericUpDown>(f, "numPrixVente").Value = 250m);
            Assert.Equal(9, Stock(pid));
            using (var ctx = new AppDbContext()) Assert.Equal(250m, ctx.produits.Find(pid).PrixVente);

            // Changement de NbUniteParBoite : avertissement, rien n'est écrit
            Enregistrer(pid, f => Champ<NumericUpDown>(f, "numNbUniteParBoite").Value = 4);
            Assert.Equal(9, Stock(pid));
            Assert.Equal(5, TestDb.Scalaire($"SELECT NbUniteParBoite FROM produits WHERE Id={pid}"));
            lock (Boites) Assert.Contains("Stock à ressaisir", Boites);

            // Unités en vrac >= nb par boîte : refusé
            Enregistrer(pid, f => Champ<NumericUpDown>(f, "numUnitesVrac").Value = 5);
            Assert.Equal(9, Stock(pid));

            // Saisie explicite : 2 boîtes + 3 unités = 13
            Enregistrer(pid, f =>
            {
                Champ<NumericUpDown>(f, "numQuantite").Value = 2;
                Champ<NumericUpDown>(f, "numUnitesVrac").Value = 3;
            });
            Assert.Equal(13, Stock(pid));
        }
        finally { cts.Cancel(); }
    }
}
