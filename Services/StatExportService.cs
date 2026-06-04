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

        public static void ExportPDF(
            string path,
            string periode,
            decimal ca,
            decimal marge,
            int nbVentes,
            int nbImpayees,
            decimal restant,
            List<dynamic> topProduits,
            List<dynamic> credits,
            string recommandations)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var doc = new PdfDocument();
            doc.Info.Title = "Rapport Pharmacie2";
            doc.Info.Creator = "Pharmacie2";

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
            var vert = XColor.FromArgb(46, 125, 50);
            ctx.Gfx.DrawRectangle(new XSolidBrush(vert), ctx.X - 5, ctx.Y - 5, ctx.W + 10, 48);
            ctx.Gfx.DrawString("RAPPORT DE CAISSE — PHARMACIE2",
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

            // KPI
            double kw = (ctx.W - 30) / 4.0;
            DrawKpi(ctx.Gfx, "Chiffre d'affaires", $"{ca:N0} KMF", ctx.X + 0 * (kw + 10), ctx.Y, kw, XColor.FromArgb(46, 125, 50));
            DrawKpi(ctx.Gfx, "Bénéfice (marge)", $"{marge:N0} KMF", ctx.X + 1 * (kw + 10), ctx.Y, kw, XColor.FromArgb(25, 118, 210));
            DrawKpi(ctx.Gfx, "Nb de ventes", $"{nbVentes}", ctx.X + 2 * (kw + 10), ctx.Y, kw, XColor.FromArgb(106, 27, 154));
            DrawKpi(ctx.Gfx, "Total restant dû", $"{restant:N0} KMF", ctx.X + 3 * (kw + 10), ctx.Y, kw, XColor.FromArgb(198, 40, 40));
            ctx.Y += 70;

            // Top produits
            DrawTitreSection(ctx, "TOP PRODUITS VENDUS", vert);
            DrawEntete(ctx, new[] { "Produit", "Qté vendue", "CA (KMF)" },
                           new[] { 0.55, 0.20, 0.25 });

            foreach (var p in topProduits)
            {
                // Conversion explicite en string[] pour éviter CS1503
                string nom = p.Produit?.ToString() ?? "—";
                string qte = p.QuantiteVendue?.ToString() ?? "—";
                string caStr = ((decimal)p.CA).ToString("N0");
                DrawLigne(ctx, new[] { nom, qte, caStr }, new[] { 0.55, 0.20, 0.25 });
            }
            ctx.Y += 10;

            // Crédits clients
            NouvellePageSiNecessaire(ctx);
            DrawTitreSection(ctx, "CRÉDITS CLIENTS IMPAYÉS", XColor.FromArgb(198, 40, 40));
            DrawEntete(ctx, new[] { "Client", "Téléphone", "Nb ventes", "Restant (KMF)" },
                           new[] { 0.35, 0.25, 0.15, 0.25 });

            foreach (var c in credits)
            {
                string client = c.Client?.ToString() ?? "—";
                string tel = c.Telephone?.ToString() ?? "—";
                string nb = c.NbVentes?.ToString() ?? "—";
                string restStr = ((decimal)c.Restant).ToString("N0");
                DrawLigne(ctx, new[] { client, tel, nb, restStr }, new[] { 0.35, 0.25, 0.15, 0.25 });
            }
            ctx.Y += 10;

            // Recommandations
            if (!string.IsNullOrWhiteSpace(recommandations))
            {
                NouvellePageSiNecessaire(ctx);
                DrawTitreSection(ctx, "RECOMMANDATIONS & ALERTES", XColor.FromArgb(230, 81, 0));

                foreach (var ligne in recommandations.Split('\n'))
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
                $"Pharmacie2  —  Document généré le {DateTime.Now:dd/MM/yyyy}",
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
        // EXPORT EXCEL — fichier CSV UTF-8 BOM (compatible Excel sans dépendance)
        // Pour un vrai .xlsx : installer EPPlus via NuGet et décommenter la version EPPlus
        // ══════════════════════════════════════════════════════════════════

        public static void ExportExcel(
            string path,
            decimal ca,
            decimal marge,
            int nbVentes,
            decimal restant,
            List<dynamic> topProduits,
            List<dynamic> credits)
        {
            // ── Version CSV UTF-8 BOM ─────────────────────────────────────
            // Renommer le fichier en .csv si Excel ne reconnaît pas le format
            var sb = new StringBuilder();

            // En-tête stylisé
            sb.AppendLine("RAPPORT PHARMACIE2");
            sb.AppendLine($"Généré le;{DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine();

            // Résumé
            sb.AppendLine("RÉSUMÉ");
            sb.AppendLine("Indicateur;Valeur");
            sb.AppendLine($"Chiffre d'affaires;{ca:N0} KMF");
            sb.AppendLine($"Bénéfice (marge);{marge:N0} KMF");
            sb.AppendLine($"Nombre de ventes;{nbVentes}");
            sb.AppendLine($"Total restant dû;{restant:N0} KMF");
            sb.AppendLine();

            // Top produits
            sb.AppendLine("TOP PRODUITS");
            sb.AppendLine("Produit;Quantité vendue;CA (KMF)");
            foreach (var p in topProduits)
            {
                string nom = p.Produit?.ToString() ?? "—";
                string qte = p.QuantiteVendue?.ToString() ?? "—";
                string ca2 = ((decimal)p.CA).ToString("N0");
                sb.AppendLine($"{nom};{qte};{ca2}");
            }
            sb.AppendLine();

            // Crédits clients
            sb.AppendLine("CRÉDITS CLIENTS IMPAYÉS");
            sb.AppendLine("Client;Téléphone;Nb ventes;Restant (KMF)");
            foreach (var c in credits)
            {
                string client = c.Client?.ToString() ?? "—";
                string tel = c.Telephone?.ToString() ?? "—";
                string nb = c.NbVentes?.ToString() ?? "—";
                string rest = ((decimal)c.Restant).ToString("N0");
                sb.AppendLine($"{client};{tel};{nb};{rest}");
            }

            // Écrire en UTF-8 BOM — Excel reconnaît les accents
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }
    }
}