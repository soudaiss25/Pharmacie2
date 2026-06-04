using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Génère un fichier Excel (.xlsx) listant tous les achats par mutuelle
    /// pour une entreprise donnée sur une période donnée.
    ///
    /// Dépendance NuGet : ClosedXML (>= 0.102)
    ///   dotnet add package ClosedXML
    /// </summary>
    public static class MutuelleExportService
    {
        // ─── DTO interne ─────────────────────────────────────────────────

        public class LigneEmployeMutuelle
        {
            public string NomEmployeur { get; set; }
            public string MatriculeEmploye { get; set; }
            public string NomClient { get; set; }
            public string PrenomClient { get; set; }
            public string TelephoneClient { get; set; }
            public DateTime DateVente { get; set; }
            public string NumeroVente { get; set; }
            public decimal MontantTotal { get; set; }
            public decimal TauxMutuelle { get; set; }
            public decimal PartMutuelle { get; set; }
            public decimal PartPatient { get; set; }
            public decimal MontantVerse { get; set; }
            public decimal ResteAPayer { get; set; }
            public string Statut { get; set; }
        }

        // ─── Requête données ─────────────────────────────────────────────

        /// <summary>
        /// Retourne les lignes de rapport pour une mutuelle sur une période,
        /// en n'incluant que les ventes ACTIVES.
        /// </summary>
        public static List<LigneEmployeMutuelle> GetDonnees(
            int mutuelId, DateTime debut, DateTime fin)
        {
            using var ctx = new AppDbContext();

            var ventes = ctx.ventes
                .Include(v => v.Mutuel)
                .Include(v => v.Paiements)
                .Where(v => v.MutuelId == mutuelId
                         && v.Type == "Mutuelle"
                         && v.Statut == "Active"
                         && v.DateVente >= debut
                         && v.DateVente <= fin)
                .OrderBy(v => v.NomClient)
                .ThenBy(v => v.DateVente)
                .ToList();

            return ventes.Select(v => new LigneEmployeMutuelle
            {
                NomEmployeur = v.Mutuel?.NomEmployeur ?? "—",
                MatriculeEmploye = v.MatriculeEmploye ?? "—",
                NomClient = v.NomClient ?? "",
                PrenomClient = v.PrenomClient ?? "",
                TelephoneClient = v.TelephoneClient ?? "—",
                DateVente = v.DateVente,
                NumeroVente = v.numeroVente,
                MontantTotal = v.MontantTotal,
                TauxMutuelle = v.TauxMutuelle,
                PartMutuelle = v.MontantMutuelle,
                PartPatient = v.MontantTotal - v.MontantMutuelle,
                MontantVerse = v.MontantVerse,
                ResteAPayer = v.MontantRestant,
                Statut = v.Statut
            }).ToList();
        }

        // ─── Export Excel ─────────────────────────────────────────────────

        /// <summary>
        /// Génère le fichier Excel et l'enregistre à <paramref name="cheminFichier"/>.
        /// </summary>
        public static void Exporter(
            string cheminFichier,
            int mutuelId,
            string nomMutuelle,
            DateTime debut,
            DateTime fin)
        {
            var lignes = GetDonnees(mutuelId, debut, fin);

            using var wb = new XLWorkbook();

            // ── Feuille 1 : Détail employés ───────────────────────────────
            var ws = wb.Worksheets.Add("Détail employés");

            // ── Titre principal ───────────────────────────────────────────
            ws.Cell("A1").Value = $"Rapport Mutuelle — {nomMutuelle}";
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(27, 94, 32);
            ws.Range("A1:N1").Merge();

            ws.Cell("A2").Value =
                $"Période : {debut:dd/MM/yyyy}  →  {fin:dd/MM/yyyy}" +
                $"     Généré le : {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Cell("A2").Style.Font.Italic = true;
            ws.Cell("A2").Style.Font.FontColor = XLColor.Gray;
            ws.Range("A2:N2").Merge();

            ws.Row(1).Height = 30;
            ws.Row(2).Height = 20;

            // ── En-têtes colonnes (ligne 4) ───────────────────────────────
            var entetes = new[]
            {
                "N° Vente",
                "Date",
                "Matricule",
                "Nom",
                "Prénom",
                "Téléphone",
                "Motif / ordonnance",
                "Montant total",
                $"Taux mutuelle (%)",
                $"Part {nomMutuelle}",
                "Part patient",
                "Montant versé",
                "Reste à payer",
                "Statut"
            };

            int headerRow = 4;
            for (int i = 0; i < entetes.Length; i++)
            {
                var cell = ws.Cell(headerRow, i + 1);
                cell.Value = entetes[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(46, 125, 50);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.White;
            }

            ws.Row(headerRow).Height = 22;

            // ── Données ───────────────────────────────────────────────────
            int dataRow = headerRow + 1;
            foreach (var l in lignes)
            {
                ws.Cell(dataRow, 1).Value = l.NumeroVente;
                ws.Cell(dataRow, 2).Value = l.DateVente.ToString("dd/MM/yyyy");
                ws.Cell(dataRow, 3).Value = l.MatriculeEmploye;
                ws.Cell(dataRow, 4).Value = l.NomClient;
                ws.Cell(dataRow, 5).Value = l.PrenomClient;
                ws.Cell(dataRow, 6).Value = l.TelephoneClient;
                ws.Cell(dataRow, 7).Value = "—";          // Motif non stocké ici, extensible
                ws.Cell(dataRow, 8).Value = l.MontantTotal;
                ws.Cell(dataRow, 9).Value = l.TauxMutuelle / 100m; // format %
                ws.Cell(dataRow, 10).Value = l.PartMutuelle;
                ws.Cell(dataRow, 11).Value = l.PartPatient;
                ws.Cell(dataRow, 12).Value = l.MontantVerse;
                ws.Cell(dataRow, 13).Value = l.ResteAPayer;
                ws.Cell(dataRow, 14).Value = l.Statut;

                // Format monétaire colonnes 8–13
                string fmt = "#,##0.00 \"KMF\"";
                for (int c = 8; c <= 13; c++)
                    ws.Cell(dataRow, c).Style.NumberFormat.Format = fmt;

                // Format pourcentage colonne 9
                ws.Cell(dataRow, 9).Style.NumberFormat.Format = "0.0%";

                // Colorer ligne si reste > 0
                if (l.ResteAPayer > 0)
                {
                    ws.Row(dataRow).Style.Fill.BackgroundColor =
                        XLColor.FromArgb(255, 243, 205);
                    ws.Cell(dataRow, 13).Style.Font.FontColor = XLColor.OrangeRed;
                    ws.Cell(dataRow, 13).Style.Font.Bold = true;
                }

                // Alternance légère
                if (dataRow % 2 == 0 && l.ResteAPayer <= 0)
                    ws.Row(dataRow).Style.Fill.BackgroundColor =
                        XLColor.FromArgb(240, 248, 240);

                ws.Row(dataRow).Height = 18;
                dataRow++;
            }

            // ── Ligne de totaux ───────────────────────────────────────────
            if (lignes.Count > 0)
            {
                int totalRow = dataRow;
                ws.Cell(totalRow, 1).Value = "TOTAL";
                ws.Cell(totalRow, 1).Style.Font.Bold = true;

                // Formules Excel dynamiques
                string range(int col) =>
                    $"{ColLetter(col)}{headerRow + 1}:{ColLetter(col)}{dataRow - 1}";

                ws.Cell(totalRow, 8).FormulaA1 = $"=SUM({range(8)})";
                ws.Cell(totalRow, 10).FormulaA1 = $"=SUM({range(10)})";
                ws.Cell(totalRow, 11).FormulaA1 = $"=SUM({range(11)})";
                ws.Cell(totalRow, 12).FormulaA1 = $"=SUM({range(12)})";
                ws.Cell(totalRow, 13).FormulaA1 = $"=SUM({range(13)})";

                string fmtTotal = "#,##0.00 \"KMF\"";
                foreach (int c in new[] { 8, 10, 11, 12, 13 })
                {
                    ws.Cell(totalRow, c).Style.NumberFormat.Format = fmtTotal;
                    ws.Cell(totalRow, c).Style.Font.Bold = true;
                }

                ws.Range(totalRow, 1, totalRow, entetes.Length)
                  .Style.Fill.BackgroundColor = XLColor.FromArgb(200, 230, 200);
                ws.Row(totalRow).Height = 22;
            }

            // ── Mise en forme finale ──────────────────────────────────────
            ws.Columns().AdjustToContents();
            ws.Column(7).Width = 30; // motif

            // Figer les 4 premières lignes + la colonne 1
            ws.SheetView.FreezeRows(headerRow);
            ws.SheetView.FreezeColumns(1);

            // ── Feuille 2 : Récapitulatif par employé ─────────────────────
            var wsRecap = wb.Worksheets.Add("Récap par employé");

            wsRecap.Cell("A1").Value =
                $"Récapitulatif par employé — {nomMutuelle}  ({debut:MM/yyyy})";
            wsRecap.Cell("A1").Style.Font.Bold = true;
            wsRecap.Cell("A1").Style.Font.FontSize = 13;
            wsRecap.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(27, 94, 32);
            wsRecap.Range("A1:G1").Merge();
            wsRecap.Row(1).Height = 26;

            string[] recapHeaders =
            {
                "Matricule", "Nom", "Prénom",
                "Nb achats", "Total achats",
                $"Part {nomMutuelle}", "Part patient"
            };
            for (int i = 0; i < recapHeaders.Length; i++)
            {
                var c = wsRecap.Cell(3, i + 1);
                c.Value = recapHeaders[i];
                c.Style.Font.Bold = true;
                c.Style.Font.FontColor = XLColor.White;
                c.Style.Fill.BackgroundColor = XLColor.FromArgb(25, 118, 210);
                c.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
            wsRecap.Row(3).Height = 22;

            // Grouper par employé
            var parEmploye = lignes
                .GroupBy(l => new { l.MatriculeEmploye, l.NomClient, l.PrenomClient })
                .OrderBy(g => g.Key.NomClient)
                .Select(g => new
                {
                    g.Key.MatriculeEmploye,
                    g.Key.NomClient,
                    g.Key.PrenomClient,
                    NbAchats = g.Count(),
                    TotalAchats = g.Sum(x => x.MontantTotal),
                    PartMutuelle = g.Sum(x => x.PartMutuelle),
                    PartPatient = g.Sum(x => x.PartPatient)
                })
                .ToList();

            int rr = 4;
            foreach (var emp in parEmploye)
            {
                wsRecap.Cell(rr, 1).Value = emp.MatriculeEmploye;
                wsRecap.Cell(rr, 2).Value = emp.NomClient;
                wsRecap.Cell(rr, 3).Value = emp.PrenomClient;
                wsRecap.Cell(rr, 4).Value = emp.NbAchats;
                wsRecap.Cell(rr, 5).Value = emp.TotalAchats;
                wsRecap.Cell(rr, 6).Value = emp.PartMutuelle;
                wsRecap.Cell(rr, 7).Value = emp.PartPatient;

                string fmtR = "#,##0.00 \"KMF\"";
                wsRecap.Cell(rr, 5).Style.NumberFormat.Format = fmtR;
                wsRecap.Cell(rr, 6).Style.NumberFormat.Format = fmtR;
                wsRecap.Cell(rr, 7).Style.NumberFormat.Format = fmtR;

                if (rr % 2 == 0)
                    wsRecap.Row(rr).Style.Fill.BackgroundColor =
                        XLColor.FromArgb(235, 245, 255);
                wsRecap.Row(rr).Height = 18;
                rr++;
            }

            // Total récap
            if (parEmploye.Count > 0)
            {
                wsRecap.Cell(rr, 2).Value = "TOTAL";
                wsRecap.Cell(rr, 2).Style.Font.Bold = true;
                wsRecap.Cell(rr, 5).FormulaA1 = $"=SUM(E4:E{rr - 1})";
                wsRecap.Cell(rr, 6).FormulaA1 = $"=SUM(F4:F{rr - 1})";
                wsRecap.Cell(rr, 7).FormulaA1 = $"=SUM(G4:G{rr - 1})";
                foreach (int c in new[] { 5, 6, 7 })
                {
                    wsRecap.Cell(rr, c).Style.NumberFormat.Format = "#,##0.00 \"KMF\"";
                    wsRecap.Cell(rr, c).Style.Font.Bold = true;
                }
                wsRecap.Range(rr, 1, rr, 7)
                       .Style.Fill.BackgroundColor = XLColor.FromArgb(200, 230, 200);
            }

            wsRecap.Columns().AdjustToContents();

            // ── Sauvegarde ────────────────────────────────────────────────
            wb.SaveAs(cheminFichier);
        }

        // ─── Helpers ─────────────────────────────────────────────────────

        /// <summary>Convertit un numéro de colonne (1-based) en lettre Excel.</summary>
        private static string ColLetter(int col)
        {
            string result = "";
            while (col > 0)
            {
                col--;
                result = (char)('A' + col % 26) + result;
                col /= 26;
            }
            return result;
        }
    }
}