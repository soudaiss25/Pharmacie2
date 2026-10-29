using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Fichier Excel (.xlsx) des achats d'une mutuelle pour une période.
    ///   - « Pour l'entreprise » : ce que l'on peut envoyer à l'employeur (aucune information interne : ni téléphone, ni versement, ni reste dû du patient, ni motif) ;
    ///   - « Interne (pharmacie) » : toutes les colonnes de suivi.
    /// Par défaut seules les ventes non réglées sont exportées, pour ne jamais facturer deux fois la même vente.
    /// </summary>
    public static class MutuelleExportService
    {
        public enum VersionFichier { Entreprise, Interne }

        public const string PatientNonIdentifie = MutuelleService.PatientNonIdentifieLibelle;

        public class LigneMedicament
        {
            public string NumeroVente { get; set; } = "";
            public DateTime DateVente { get; set; }
            public string Employe { get; set; } = "";
            public string Produit { get; set; } = "";
            public string UniteVendue { get; set; } = "";
            public int Quantite { get; set; }
            public decimal PrixUnitaire { get; set; }
            public decimal SousTotal => Quantite * PrixUnitaire;
        }

        public class LigneEmployeMutuelle
        {
            public string NomEmployeur { get; set; } = "";
            public string MatriculeEmploye { get; set; } = "";
            public string NomClient { get; set; } = "";
            public string PrenomClient { get; set; } = "";
            public string TelephoneClient { get; set; } = "";
            public DateTime DateVente { get; set; }
            public string NumeroVente { get; set; } = "";
            public decimal MontantTotal { get; set; }
            public decimal TauxMutuelle { get; set; }
            public decimal PartMutuelle { get; set; }
            public decimal PartPatient { get; set; }
            public decimal MontantVerse { get; set; }
            public decimal ResteAPayer { get; set; }
            public string Statut { get; set; } = "";
            public string Motif { get; set; } = "";
            public bool Reglee { get; set; }
            public DateTime? DateReglement { get; set; }
            public bool PatientNonIdentifie { get; set; }
            public List<LigneMedicament> Medicaments { get; set; } = new();

            public string TexteReglee => Reglee ? "Oui" + (DateReglement.HasValue ? $" ({DateReglement:dd/MM/yyyy})" : "") : "Non";
            public string ResumeMedicaments => string.Join(" ; ", Medicaments.Select(m => $"{m.Produit} ×{m.Quantite} ({m.UniteVendue})"));
        }

        // ─── Requête données ─────────────────────────────────────────────

        /// <summary>Ventes ACTIVES d'une mutuelle sur une période (seulement les non réglées si <paramref name="nonRegleesSeulement"/>).</summary>
        public static List<LigneEmployeMutuelle> GetDonnees(int mutuelId, DateTime debut, DateTime fin, bool nonRegleesSeulement = false)
        {
            using var ctx = new AppDbContext();

            var ventes = ctx.ventes
                .Include(v => v.Mutuel)
                .Include(v => v.Paiements)
                .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                .Where(v => v.MutuelId == mutuelId
                         && v.Type == "Mutuelle"
                         && v.Statut == "Active"
                         && v.DateVente >= debut
                         && v.DateVente <= fin
                         && (!nonRegleesSeulement || !v.MutuelleReglee))
                .OrderBy(v => v.NomClient)
                .ThenBy(v => v.DateVente)
                .ToList();

            var dates = ctx.MutuelPaiements.AsNoTracking().Where(p => p.MutuelId == mutuelId).ToList()
                .GroupBy(p => p.VenteId).ToDictionary(g => g.Key, g => g.Max(p => p.DatePaiement));

            return ventes.Select(v =>
            {
                bool inconnu = MutuelleService.PatientNonIdentifie(v);
                string employe = inconnu ? PatientNonIdentifie : $"{v.PrenomClient} {v.NomClient}".Trim();
                return new LigneEmployeMutuelle
                {
                    NomEmployeur = v.Mutuel?.NomEmployeur ?? "—",
                    MatriculeEmploye = MutuelleService.MatriculeVide(v.MatriculeEmploye) ? "—" : v.MatriculeEmploye!,
                    NomClient = v.NomClient ?? "",
                    PrenomClient = v.PrenomClient ?? "",
                    TelephoneClient = string.IsNullOrWhiteSpace(v.TelephoneClient) ? "—" : v.TelephoneClient,
                    DateVente = v.DateVente,
                    NumeroVente = v.numeroVente,
                    MontantTotal = v.MontantTotal,
                    TauxMutuelle = v.TauxMutuelle,
                    PartMutuelle = v.MontantMutuelle,
                    PartPatient = v.MontantTotal - v.MontantMutuelle,
                    MontantVerse = v.MontantVerse,
                    ResteAPayer = v.MontantRestant,
                    Statut = v.Statut,
                    Motif = v.MotifAchat ?? "",
                    Reglee = v.MutuelleReglee,
                    DateReglement = v.MutuelleReglee && dates.TryGetValue(v.IdVente, out var d) ? d : null,
                    PatientNonIdentifie = inconnu,
                    Medicaments = v.Lignes.OrderBy(l => l.Id).Select(l => new LigneMedicament
                    {
                        NumeroVente = v.numeroVente,
                        DateVente = v.DateVente,
                        Employe = employe,
                        Produit = l.Produit?.Nom ?? "(produit supprimé)",
                        UniteVendue = l.UniteVendue,
                        Quantite = l.Quantite,
                        PrixUnitaire = l.PrixUnitaire
                    }).ToList()
                };
            }).OrderBy(l => l.PatientNonIdentifie)                    // les ventes sans identité en dernier
              .ThenBy(l => l.NomClient).ThenBy(l => l.DateVente).ToList();
        }

        // ─── Export Excel ─────────────────────────────────────────────────

        private const string FormatMontant = "#,##0.00 \"KMF\"";

        private sealed record Colonne(string Titre, Func<LigneEmployeMutuelle, object> Valeur, string Format = "");

        public static void Exporter(
            string cheminFichier,
            int mutuelId,
            string nomMutuelle,
            DateTime debut,
            DateTime fin,
            bool nonRegleesSeulement = true,
            VersionFichier version = VersionFichier.Entreprise)
        {
            var lignes = GetDonnees(mutuelId, debut, fin, nonRegleesSeulement);
            bool interne = version == VersionFichier.Interne;
            string portee = nonRegleesSeulement ? "Ventes non réglées" : "Toutes les ventes";

            using var wb = new XLWorkbook();

            // ── Feuille 1 : détail des ventes ─────────────────────────────
            var ws = wb.Worksheets.Add("Détail employés");
            var communes = new List<Colonne>
            {
                new("N° Vente", l => l.NumeroVente),
                new("Date", l => l.DateVente.ToString("dd/MM/yyyy")),
                new("Matricule", l => l.MatriculeEmploye),
                new("Nom", l => l.PatientNonIdentifie ? PatientNonIdentifie : l.NomClient),
                new("Prénom", l => l.PrenomClient),
            };
            var colonnes = new List<Colonne>(communes);
            if (interne)
            {
                colonnes.Add(new("Téléphone", l => l.TelephoneClient));
                colonnes.Add(new("Motif / ordonnance", l => l.Motif));
            }
            else
                colonnes.Add(new("Médicaments achetés", l => l.ResumeMedicaments));
            colonnes.Add(new("Montant total", l => l.MontantTotal, FormatMontant));
            colonnes.Add(new("Taux mutuelle (%)", l => l.TauxMutuelle / 100m, "0.0%"));
            colonnes.Add(new($"Part {nomMutuelle}", l => l.PartMutuelle, FormatMontant));
            if (interne)
            {
                colonnes.Add(new("Part patient", l => l.PartPatient, FormatMontant));
                colonnes.Add(new("Montant versé", l => l.MontantVerse, FormatMontant));
                colonnes.Add(new("Reste à payer", l => l.ResteAPayer, FormatMontant));
                colonnes.Add(new("Statut", l => l.Statut));
            }
            colonnes.Add(new("Réglée par la mutuelle", l => l.TexteReglee));

            int headerRow = EcrireEntete(ws, nomMutuelle, debut, fin, portee, colonnes.Count);
            for (int i = 0; i < colonnes.Count; i++)
                StyleEntete(ws.Cell(headerRow, i + 1), colonnes[i].Titre, XLColor.FromArgb(46, 125, 50));
            ws.Row(headerRow).Height = 22;

            int ligne = headerRow + 1;
            foreach (var l in lignes)
            {
                for (int c = 0; c < colonnes.Count; c++)
                {
                    var cell = ws.Cell(ligne, c + 1);
                    var valeur = colonnes[c].Valeur(l);
                    if (valeur is decimal d) cell.Value = d; else cell.Value = Convert.ToString(valeur) ?? "";
                    if (colonnes[c].Format != "") cell.Style.NumberFormat.Format = colonnes[c].Format;
                }
                if (l.PatientNonIdentifie) ws.Row(ligne).Style.Font.FontColor = XLColor.OrangeRed;
                else if (ligne % 2 == 0) ws.Row(ligne).Style.Fill.BackgroundColor = XLColor.FromArgb(240, 248, 240);
                ws.Row(ligne).Height = 18;
                ligne++;
            }

            if (lignes.Count > 0)
            {
                ws.Cell(ligne, 1).Value = "TOTAL";
                ws.Cell(ligne, 1).Style.Font.Bold = true;
                for (int c = 0; c < colonnes.Count; c++)
                {
                    if (colonnes[c].Format != FormatMontant) continue;
                    string lettre = ColLetter(c + 1);
                    var cell = ws.Cell(ligne, c + 1);
                    cell.FormulaA1 = $"=SUM({lettre}{headerRow + 1}:{lettre}{ligne - 1})";
                    cell.Style.NumberFormat.Format = FormatMontant;
                    cell.Style.Font.Bold = true;
                }
                ws.Range(ligne, 1, ligne, colonnes.Count).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 230, 200);
                ws.Row(ligne).Height = 22;
            }
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(headerRow);
            ws.SheetView.FreezeColumns(1);

            // ── Feuille 2 : récapitulatif par employé ─────────────────────
            var wsRecap = wb.Worksheets.Add("Récap par employé");
            var recapHeaders = new List<string> { "Matricule", "Nom", "Prénom", "Nb achats", "Total achats", $"Part {nomMutuelle}" };
            if (interne) recapHeaders.Add("Part patient");
            int hRecap = EcrireEntete(wsRecap, nomMutuelle, debut, fin, portee, recapHeaders.Count);
            for (int i = 0; i < recapHeaders.Count; i++)
                StyleEntete(wsRecap.Cell(hRecap, i + 1), recapHeaders[i], XLColor.FromArgb(25, 118, 210));
            wsRecap.Row(hRecap).Height = 22;

            // Les ventes sans nom ni matricule sont regroupées sous « Patient non identifié », en dernier
            var groupes = lignes.Where(l => !l.PatientNonIdentifie)
                .GroupBy(l => new { l.MatriculeEmploye, l.NomClient, l.PrenomClient })
                .OrderBy(g => g.Key.NomClient).ThenBy(g => g.Key.PrenomClient)
                .Select(g => (Matricule: g.Key.MatriculeEmploye, Nom: g.Key.NomClient, Prenom: g.Key.PrenomClient, Ventes: g.ToList()))
                .ToList();
            var inconnus = lignes.Where(l => l.PatientNonIdentifie).ToList();
            if (inconnus.Count > 0) groupes.Add(("—", PatientNonIdentifie, "", inconnus));

            int derniereColonne = recapHeaders.Count;
            int rr = hRecap + 1;
            foreach (var g in groupes)
            {
                wsRecap.Cell(rr, 1).Value = g.Matricule;
                wsRecap.Cell(rr, 2).Value = g.Nom;
                wsRecap.Cell(rr, 3).Value = g.Prenom;
                wsRecap.Cell(rr, 4).Value = g.Ventes.Count;
                wsRecap.Cell(rr, 5).Value = g.Ventes.Sum(x => x.MontantTotal);
                wsRecap.Cell(rr, 6).Value = g.Ventes.Sum(x => x.PartMutuelle);
                if (interne) wsRecap.Cell(rr, 7).Value = g.Ventes.Sum(x => x.PartPatient);
                for (int c = 5; c <= derniereColonne; c++) wsRecap.Cell(rr, c).Style.NumberFormat.Format = FormatMontant;
                if (g.Nom == PatientNonIdentifie) wsRecap.Row(rr).Style.Font.FontColor = XLColor.OrangeRed;
                wsRecap.Row(rr).Height = 18;
                rr++;
            }
            if (groupes.Count > 0)
            {
                wsRecap.Cell(rr, 2).Value = "TOTAL";
                wsRecap.Cell(rr, 2).Style.Font.Bold = true;
                for (int c = 5; c <= derniereColonne; c++)
                {
                    wsRecap.Cell(rr, c).FormulaA1 = $"=SUM({ColLetter(c)}{hRecap + 1}:{ColLetter(c)}{rr - 1})";
                    wsRecap.Cell(rr, c).Style.NumberFormat.Format = FormatMontant;
                    wsRecap.Cell(rr, c).Style.Font.Bold = true;
                }
                wsRecap.Range(rr, 1, rr, derniereColonne).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 230, 200);
            }
            wsRecap.Columns().AdjustToContents();

            // ── Feuille 3 : détail des médicaments (une ligne par produit acheté, un total par vente) ──
            var wsMed = wb.Worksheets.Add("Détail des médicaments");
            string[] medHeaders = { "N° Vente", "Date", "Employé", "Produit", "Unité vendue", "Quantité", "Prix unitaire", "Sous-total" };
            int hMed = EcrireEntete(wsMed, nomMutuelle, debut, fin, portee, medHeaders.Length);
            for (int i = 0; i < medHeaders.Length; i++)
                StyleEntete(wsMed.Cell(hMed, i + 1), medHeaders[i], XLColor.FromArgb(123, 31, 162));
            wsMed.Row(hMed).Height = 22;

            int rm = hMed + 1;
            foreach (var l in lignes)
            {
                int premiere = rm;
                foreach (var m in l.Medicaments)
                {
                    wsMed.Cell(rm, 1).Value = m.NumeroVente;
                    wsMed.Cell(rm, 2).Value = m.DateVente.ToString("dd/MM/yyyy");
                    wsMed.Cell(rm, 3).Value = m.Employe;
                    wsMed.Cell(rm, 4).Value = m.Produit;
                    wsMed.Cell(rm, 5).Value = m.UniteVendue;
                    wsMed.Cell(rm, 6).Value = m.Quantite;
                    wsMed.Cell(rm, 7).Value = m.PrixUnitaire;
                    wsMed.Cell(rm, 8).Value = m.SousTotal;
                    wsMed.Cell(rm, 7).Style.NumberFormat.Format = FormatMontant;
                    wsMed.Cell(rm, 8).Style.NumberFormat.Format = FormatMontant;
                    rm++;
                }
                // total de la vente (numéro laissé vide : la ligne n'est pas un médicament)
                wsMed.Cell(rm, 4).Value = $"Total de la vente {l.NumeroVente}";
                wsMed.Cell(rm, 8).FormulaA1 = l.Medicaments.Count > 0 ? $"=SUM(H{premiere}:H{rm - 1})" : "=0";
                wsMed.Cell(rm, 8).Style.NumberFormat.Format = FormatMontant;
                wsMed.Range(rm, 1, rm, 8).Style.Font.Bold = true;
                wsMed.Range(rm, 1, rm, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(243, 229, 245);
                rm++;
            }
            wsMed.Columns().AdjustToContents();

            wb.SaveAs(cheminFichier);
        }

        // ─── Helpers ─────────────────────────────────────────────────────

        /// <summary>En-tête commun : pharmacie, entreprise, période, date d'édition, mention de portée. Retourne la ligne des titres de colonnes.</summary>
        private static int EcrireEntete(IXLWorksheet ws, string nomMutuelle, DateTime debut, DateTime fin, string portee, int nbColonnes)
        {
            ws.Cell("A1").Value = AppInfo.NomPharmacie;
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(27, 94, 32);
            ws.Cell("A2").Value = $"Entreprise : {nomMutuelle}";
            ws.Cell("A2").Style.Font.Bold = true;
            ws.Cell("A3").Value = $"Période : du {debut:dd/MM/yyyy} au {fin:dd/MM/yyyy}     Édité le : {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Cell("A3").Style.Font.Italic = true;
            ws.Cell("A4").Value = portee;
            ws.Cell("A4").Style.Font.Bold = true;
            ws.Cell("A4").Style.Font.FontColor = XLColor.OrangeRed;
            for (int r = 1; r <= 4; r++) ws.Range(r, 1, r, Math.Max(2, nbColonnes)).Merge();
            ws.Row(1).Height = 30;
            return 6;
        }

        private static void StyleEntete(IXLCell cell, string titre, XLColor fond)
        {
            cell.Value = titre;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = fond;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.OutsideBorderColor = XLColor.White;
        }

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
