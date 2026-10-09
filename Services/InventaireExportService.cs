using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Feuille d'inventaire à imprimer : stock affiché en boîtes + unités, colonnes vides à remplir
    /// après comptage en rayon. Les produits « À vérifier » sont en tête et surlignés.
    /// </summary>
    public static class InventaireExportService
    {
        public static void Exporter(string chemin)
        {
            List<Produit> produits;
            using (var ctx = new AppDbContext())
            {
                produits = ctx.produits.AsNoTracking()
                    .Where(p => p.Actif)
                    .OrderByDescending(p => p.StockAVerifier)
                    .ThenBy(p => p.Nom)
                    .ToList();
            }

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Inventaire");

            ws.Cell(1, 1).Value = $"Feuille d'inventaire — {DateTime.Now:dd/MM/yyyy}";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;

            string[] entetes = { "Produit", "Unité", "Stock affiché", "Boîtes comptées", "Unités en vrac comptées", "À vérifier" };
            for (int i = 0; i < entetes.Length; i++)
            {
                var c = ws.Cell(3, i + 1);
                c.Value = entetes[i];
                c.Style.Font.Bold = true;
                c.Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            int ligne = 4;
            foreach (var p in produits)
            {
                int nb = Math.Max(1, p.NbUniteParBoite);
                ws.Cell(ligne, 1).Value = p.Nom;
                ws.Cell(ligne, 2).Value = nb > 1 ? $"Boîte de {nb} {p.UniteVente?.ToLowerInvariant()}(s)" : "Boîte";
                ws.Cell(ligne, 3).Value = StockService.Formater(p);
                ws.Cell(ligne, 6).Value = p.StockAVerifier ? "À vérifier" : "";

                if (p.StockAVerifier)
                    ws.Range(ligne, 1, ligne, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 224, 178);

                ligne++;
            }

            var tableau = ws.Range(3, 1, Math.Max(3, ligne - 1), 6);
            tableau.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableau.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            ws.Columns().AdjustToContents();
            ws.Column(4).Width = 18;
            ws.Column(5).Width = 24;
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);
            ws.SheetView.FreezeRows(3);

            wb.SaveAs(chemin);
        }
    }
}
