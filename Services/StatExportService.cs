using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Pharmacie2.Services
{
    public static class StatExportService
    {
        // ══════════════════════════════════════════════════════════════════
        // EXPORT PDF — PdfSharp, sans iText, sans BouncyCastle
        // ══════════════════════════════════════════════════════════════════

        public static void ExportPDF(string path, string periode, StatsPeriode stats, string alertes)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var doc = new PdfDocument();
            doc.Info.Title = "Rapport — " + AppInfo.NomPharmacie;
            doc.Info.Creator = AppInfo.NomLogiciel;

            var page = doc.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            var gfx = XGraphics.FromPdfPage(page);

            var ctx = new PdfCtx
            {
                Doc = doc,
                Page = page,
                Gfx = gfx,
                X = 40,
                Y = 40,
                W = page.Width.Point - 80
            };

            // En-tête
            var vert = XColor.FromArgb(27, 94, 32);
            ctx.Gfx.DrawRectangle(new XSolidBrush(vert), ctx.X - 5, ctx.Y - 5, ctx.W + 10, 48);
            ctx.Gfx.DrawString("RAPPORT — " + AppInfo.NomPharmacie,
                new XFont("Arial", 16, XFontStyleEx.Bold),
                new XSolidBrush(XColors.White),
                new XRect(ctx.X, ctx.Y, ctx.W, 38), XStringFormats.Center);
            ctx.Y += 55;

            ctx.Gfx.DrawString(
                $"Période : {periode}   |   Généré le {DateTime.Now:dd/MM/yyyy à HH:mm}",
                new XFont("Arial", 8, XFontStyleEx.Regular),
                new XSolidBrush(XColors.Gray),
                new XRect(ctx.X, ctx.Y, ctx.W, 14), XStringFormats.Center);
            ctx.Y += 26;

            // Chiffres clés
            double kw = (ctx.W - 30) / 4.0;
            DrawKpi(ctx.Gfx, "Chiffre d'affaires", Format.Montant(stats.CA), ctx.X + 0 * (kw + 10), ctx.Y, kw, XColor.FromArgb(46, 125, 50));
            DrawKpi(ctx.Gfx, "Bénéfice", Format.Montant(stats.BeneficeNet), ctx.X + 1 * (kw + 10), ctx.Y, kw, XColor.FromArgb(69, 90, 100));
            DrawKpi(ctx.Gfx, "Ventes", $"{stats.NbVentes}", ctx.X + 2 * (kw + 10), ctx.Y, kw, XColor.FromArgb(69, 90, 100));
            DrawKpi(ctx.Gfx, "Argent à récupérer", Format.Montant(stats.ArgentARecuperer), ctx.X + 3 * (kw + 10), ctx.Y, kw, XColor.FromArgb(198, 40, 40));
            ctx.Y += 70;

            // Argent reçu par moyen de paiement
            DrawTitreSection(ctx, "ARGENT REÇU PAR MOYEN DE PAIEMENT", vert);
            DrawEntete(ctx, new[] { "Moyen de paiement", "Ventes", "Argent reçu", "Part" },
                           new[] { 0.46, 0.14, 0.26, 0.14 });
            foreach (var m in stats.Modes)
                DrawLigne(ctx, new[] { m.Mode, m.NbVentes.ToString(), Format.Montant(m.Recu), m.Pourcentage.ToString("0.#") + " %" },
                               new[] { 0.46, 0.14, 0.26, 0.14 });
            ctx.Y += 10;

            // Produits les plus vendus
            NouvellePageSiNecessaire(ctx);
            DrawTitreSection(ctx, "PRODUITS LES PLUS VENDUS", vert);
            DrawEntete(ctx, new[] { "Produit", "Quantité vendue", "Ventes" },
                           new[] { 0.40, 0.35, 0.25 });
            foreach (var p in stats.TopProduits)
                DrawLigne(ctx, new[] { p.Produit, p.Vendu, Format.Montant(p.CA) }, new[] { 0.40, 0.35, 0.25 });
            ctx.Y += 10;

            // Argent à récupérer chez les clients
            NouvellePageSiNecessaire(ctx);
            DrawTitreSection(ctx, "CRÉDITS CLIENTS", XColor.FromArgb(198, 40, 40));
            DrawEntete(ctx, new[] { "Client", "Téléphone", "Ventes", "Reste à payer" },
                           new[] { 0.35, 0.25, 0.15, 0.25 });
            foreach (var c in stats.Credits)
                DrawLigne(ctx, new[] { c.Client, c.Telephone, c.NbVentes.ToString(), Format.Montant(c.Restant) },
                               new[] { 0.35, 0.25, 0.15, 0.25 });
            ctx.Y += 10;

            // À surveiller
            if (!string.IsNullOrWhiteSpace(alertes))
            {
                NouvellePageSiNecessaire(ctx);
                DrawTitreSection(ctx, "À SURVEILLER", XColor.FromArgb(230, 81, 0));

                foreach (var ligne in alertes.Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(ligne)) { ctx.Y += 6; continue; }
                    ctx.Gfx.DrawString(ligne.Trim(),
                        new XFont("Arial", 9, XFontStyleEx.Regular),
                        new XSolidBrush(XColors.Black),
                        new XRect(ctx.X, ctx.Y, ctx.W, 14), XStringFormats.TopLeft);
                    ctx.Y += 15;
                    if (ctx.Y > ctx.Page.Height.Point - 60)
                        NouvellePageSiNecessaire(ctx);
                }
            }

            // Pied de page
            ctx.Gfx.DrawString(
                $"{AppInfo.NomLogiciel}  —  Document généré le {DateTime.Now:dd/MM/yyyy}",
                new XFont("Arial", 8, XFontStyleEx.Regular),
                new XSolidBrush(XColors.Gray),
                new XRect(ctx.X, ctx.Page.Height.Point - 28, ctx.W, 18),
                XStringFormats.BottomCenter);

            doc.Save(path);
        }

        // ── Classe contexte pour passer page/gfx par référence ────────────

        private class PdfCtx
        {
            public PdfDocument Doc { get; set; }
            public PdfPage Page { get; set; }
            public XGraphics Gfx { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double W { get; set; }
            public bool AltRow { get; set; }
        }

        // ── Helpers PDF ───────────────────────────────────────────────────

        private static void DrawKpi(XGraphics gfx, string label, string val,
            double kx, double ky, double kw, XColor coul)
        {
            var gris = XColor.FromArgb(245, 245, 245);
            gfx.DrawRectangle(new XSolidBrush(gris), kx, ky, kw, 58);
            gfx.DrawRectangle(new XSolidBrush(coul), kx, ky, kw, 4);
            gfx.DrawString(label, new XFont("Arial", 8, XFontStyleEx.Regular),
                new XSolidBrush(XColors.Gray),
                new XRect(kx + 4, ky + 10, kw - 8, 14), XStringFormats.Center);
            gfx.DrawString(val, new XFont("Arial", 9, XFontStyleEx.Bold),
                new XSolidBrush(coul),
                new XRect(kx + 4, ky + 28, kw - 8, 22), XStringFormats.Center);
        }

        private static void DrawTitreSection(PdfCtx ctx, string titre, XColor coul)
        {
            ctx.Gfx.DrawRectangle(new XSolidBrush(coul), ctx.X, ctx.Y, ctx.W, 20);
            ctx.Gfx.DrawString(titre,
                new XFont("Arial", 11, XFontStyleEx.Bold),
                new XSolidBrush(XColors.White),
                new XRect(ctx.X + 8, ctx.Y + 2, ctx.W - 8, 18), XStringFormats.TopLeft);
            ctx.Y += 24;
            ctx.AltRow = false;
        }

        private static void DrawEntete(PdfCtx ctx, string[] cols, double[] pcts)
        {
            var gris2 = XColor.FromArgb(220, 220, 220);
            ctx.Gfx.DrawRectangle(new XSolidBrush(gris2), ctx.X, ctx.Y - 2, ctx.W, 18);
            double cx = ctx.X;
            for (int i = 0; i < cols.Length; i++)
            {
                ctx.Gfx.DrawString(cols[i],
                    new XFont("Arial", 9, XFontStyleEx.Bold),
                    new XSolidBrush(XColors.Black),
                    new XRect(cx + 3, ctx.Y, ctx.W * pcts[i] - 6, 16),
                    XStringFormats.TopLeft);
                cx += ctx.W * pcts[i];
            }
            ctx.Y += 20;
        }

        private static void DrawLigne(PdfCtx ctx, string[] vals, double[] pcts)
        {
            var gris = XColor.FromArgb(245, 245, 245);
            if (ctx.AltRow)
                ctx.Gfx.DrawRectangle(new XSolidBrush(gris), ctx.X, ctx.Y - 2, ctx.W, 16);

            double cx = ctx.X;
            for (int i = 0; i < vals.Length; i++)
            {
                ctx.Gfx.DrawString(vals[i] ?? "—",
                    new XFont("Arial", 9, XFontStyleEx.Regular),
                    new XSolidBrush(XColors.Black),
                    new XRect(cx + 3, ctx.Y, ctx.W * pcts[i] - 6, 14),
                    XStringFormats.TopLeft);
                cx += ctx.W * pcts[i];
            }
            ctx.Y += 16;
            ctx.AltRow = !ctx.AltRow;

            if (ctx.Y > ctx.Page.Height.Point - 80)
                NouvellePageSiNecessaire(ctx);
        }

        private static void NouvellePageSiNecessaire(PdfCtx ctx)
        {
            if (ctx.Y <= ctx.Page.Height.Point - 120) return;
            ctx.Page = ctx.Doc.AddPage();
            ctx.Page.Size = PdfSharp.PageSize.A4;
            ctx.Gfx = XGraphics.FromPdfPage(ctx.Page);
            ctx.Y = 40;
            ctx.AltRow = false;
        }

        // ══════════════════════════════════════════════════════════════════
        // EXPORT EXCEL — vrai classeur .xlsx (ClosedXML)
        // ══════════════════════════════════════════════════════════════════

        public static void ExportExcel(string path, string periode, StatsPeriode stats)
        {
            using var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.AddWorksheet("Rapport");
            int l = 1;

            ws.Cell(l, 1).Value = "Rapport — " + AppInfo.NomPharmacie;
            ws.Cell(l, 1).Style.Font.Bold = true;
            ws.Cell(l, 1).Style.Font.FontSize = 14;
            l++;
            ws.Cell(l++, 1).Value = $"Période : {periode}   |   Généré le {DateTime.Now:dd/MM/yyyy HH:mm}";
            l++;

            void Titre(string t)
            {
                ws.Cell(l, 1).Value = t;
                ws.Cell(l, 1).Style.Font.Bold = true;
                ws.Range(l, 1, l, 4).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#E8F5E9");
                l++;
            }
            void Entete(params string[] cols)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    ws.Cell(l, i + 1).Value = cols[i];
                    ws.Cell(l, i + 1).Style.Font.Bold = true;
                }
                l++;
            }

            Titre("Chiffres clés");
            Entete("Indicateur", "Valeur");
            ws.Cell(l, 1).Value = "Chiffre d'affaires"; ws.Cell(l++, 2).Value = Format.Montant(stats.CA);
            ws.Cell(l, 1).Value = "Bénéfice (après dépenses)"; ws.Cell(l++, 2).Value = Format.Montant(stats.BeneficeNet);
            ws.Cell(l, 1).Value = "Ventes"; ws.Cell(l++, 2).Value = stats.NbVentes;
            ws.Cell(l, 1).Value = "Argent à récupérer"; ws.Cell(l++, 2).Value = Format.Montant(stats.ArgentARecuperer);
            l++;

            Titre("Argent reçu par moyen de paiement");
            Entete("Moyen de paiement", "Ventes", "Argent reçu", "Part");
            foreach (var m in stats.Modes)
            {
                ws.Cell(l, 1).Value = m.Mode; ws.Cell(l, 2).Value = m.NbVentes;
                ws.Cell(l, 3).Value = Format.Montant(m.Recu); ws.Cell(l++, 4).Value = m.Pourcentage.ToString("0.#") + " %";
            }
            l++;

            Titre("Produits les plus vendus");
            Entete("Produit", "Quantité vendue", "Ventes");
            foreach (var p in stats.TopProduits)
            {
                ws.Cell(l, 1).Value = p.Produit; ws.Cell(l, 2).Value = p.Vendu; ws.Cell(l++, 3).Value = Format.Montant(p.CA);
            }
            l++;

            Titre("Crédits clients");
            Entete("Client", "Téléphone", "Ventes", "Reste à payer");
            foreach (var c in stats.Credits)
            {
                ws.Cell(l, 1).Value = c.Client; ws.Cell(l, 2).Value = c.Telephone;
                ws.Cell(l, 3).Value = c.NbVentes; ws.Cell(l++, 4).Value = Format.Montant(c.Restant);
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(path);
        }
    }
}
