using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// Exporte toutes les tables en CSV UTF-8 BOM dans un dossier horodaté.
    /// Utilisable pour migration future vers PostgreSQL.
    /// </summary>
    public static class SauvegardeService
    {
        public static void ExporterTout(string dossier)
        {
            using var ctx = new AppDbContext();

            ExporterVentes(ctx, dossier);
            ExporterLignesVente(ctx, dossier);
            ExporterPaiements(ctx, dossier);
            ExporterProduits(ctx, dossier);
            ExporterFournisseurs(ctx, dossier);
            ExporterCommandes(ctx, dossier);
            ExporterLignesCommande(ctx, dossier);
            ExporterMutuelles(ctx, dossier);
            ExporterMutuelPaiements(ctx, dossier);
            ExporterSessions(ctx, dossier);
            ExporterUtilisateurs(ctx, dossier);

            // Fichier README
            File.WriteAllText(
                Path.Combine(dossier, "README.txt"),
                $"Sauvegarde Pharmacie2\nDate : {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                $"Encodage : UTF-8 BOM\nSeparateur : point-virgule (;)\n",
                new UTF8Encoding(true));
        }

        private static void Ecrire(string chemin, string contenu)
            => File.WriteAllText(chemin, contenu, new UTF8Encoding(true));

        private static string Echap(string s)
            => string.IsNullOrEmpty(s) ? "" : s.Replace(";", ",").Replace("\n", " ").Replace("\r", "");

        // ── Tables ────────────────────────────────────────────────────────

        private static void ExporterVentes(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("IdVente;NumeroVente;NomClient;PrenomClient;Telephone;DateVente;" +
                          "MontantTotal;MontantEspeces;MontantRendu;MontantMutuelle;MontantVerse;" +
                          "MoyenPaiement;Type;Statut;MotifAchat;MatriculeEmploye;" +
                          "MutuelId;TauxMutuelle;MutuelleReglee;UserId");

            foreach (var v in ctx.ventes.AsNoTracking().ToList())
                sb.AppendLine($"{v.IdVente};{v.numeroVente};{Echap(v.NomClient)};{Echap(v.PrenomClient)};" +
                    $"{Echap(v.TelephoneClient)};{v.DateVente:yyyy-MM-dd HH:mm:ss};" +
                    $"{v.MontantTotal};{v.MontantEspeces};{v.MontantRendu};{v.MontantMutuelle};{v.MontantVerse};" +
                    $"{v.MoyenPaiement};{v.Type};{v.Statut};{Echap(v.MotifAchat)};{Echap(v.MatriculeEmploye)};" +
                    $"{v.MutuelId};{v.TauxMutuelle};{v.MutuelleReglee};{v.UserId}");

            Ecrire(Path.Combine(dossier, "ventes.csv"), sb.ToString());
        }

        private static void ExporterLignesVente(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;VenteId;ProduitId;Quantite;PrixUnitaire;UniteVendue");

            foreach (var l in ctx.LigneVentes.AsNoTracking().ToList())
                sb.AppendLine($"{l.Id};{l.VenteId};{l.ProduitId};{l.Quantite};{l.PrixUnitaire};{l.UniteVendue}");

            Ecrire(Path.Combine(dossier, "lignes_vente.csv"), sb.ToString());
        }

        private static void ExporterPaiements(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;NumeroPaiement;VenteId;Montant;DatePaiement;UserId");

            foreach (var p in ctx.paiement.AsNoTracking().ToList())
                sb.AppendLine($"{p.Id};{p.NumeroPaiement};{p.VenteId};{p.Montant};" +
                    $"{p.DatePaiement:yyyy-MM-dd HH:mm:ss};{p.UserId}");

            Ecrire(Path.Combine(dossier, "paiements.csv"), sb.ToString());
        }

        private static void ExporterProduits(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;Nom;Type;PrixAchat;MargeBeneficiaire;PrixVente;" +
                          "QuantiteEnStock;SeuilAlerte;DateExpiration;UniteVente;" +
                          "NbUniteParBoite;FournisseurId;Indication;Posologie;NbFoisParJour");

            foreach (var p in ctx.produits.AsNoTracking().ToList())
                sb.AppendLine($"{p.Id};{Echap(p.Nom)};{p.Type};{p.PrixAchat};{p.MargeBeneficiaire};" +
                    $"{p.PrixVente};{p.QuantiteEnStock};{p.SeuilAlerte};" +
                    $"{p.DateExpiration:yyyy-MM-dd};{p.UniteVente};{p.NbUniteParBoite};" +
                    $"{p.FournisseurId};{Echap(p.Indication)};{Echap(p.Posologie)};{p.NbFoisParJour}");

            Ecrire(Path.Combine(dossier, "produits.csv"), sb.ToString());
        }

        private static void ExporterFournisseurs(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;Nom;Contact");

            foreach (var f in ctx.fournisseur.AsNoTracking().ToList())
                sb.AppendLine($"{f.Id};{Echap(f.Nom)};{Echap(f.Contact)}");

            Ecrire(Path.Combine(dossier, "fournisseurs.csv"), sb.ToString());
        }

        private static void ExporterCommandes(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;FournisseurId;DateCommande;Statut;NoteCommande;DateReception");

            foreach (var c in ctx.commandes.AsNoTracking().ToList())
                sb.AppendLine($"{c.Id};{c.FournisseurId};{c.DateCommande:yyyy-MM-dd HH:mm:ss};" +
                    $"{c.Statut};{Echap(c.NoteCommande)};" +
                    $"{(c.DateReception.HasValue ? c.DateReception.Value.ToString("yyyy-MM-dd") : "")}");

            Ecrire(Path.Combine(dossier, "commandes.csv"), sb.ToString());
        }

        private static void ExporterLignesCommande(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;CommandeId;ProduitId;Quantite;PrixAchatUnitaire");

            foreach (var l in ctx.LigneCommandes.AsNoTracking().ToList())
                sb.AppendLine($"{l.Id};{l.CommandeId};{l.ProduitId};{l.Quantite};{l.PrixAchatUnitaire}");

            Ecrire(Path.Combine(dossier, "lignes_commande.csv"), sb.ToString());
        }

        private static void ExporterMutuelles(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("IdMutuel;NomEmployeur;TauxPriseEnCharge;EmailContact;Telephone");

            foreach (var m in ctx.mutuels.AsNoTracking().ToList())
                sb.AppendLine($"{m.IdMutuel};{Echap(m.NomEmployeur)};{m.TauxPriseEnCharge};" +
                    $"{Echap(m.EmailContact)};{Echap(m.telephoneEmployeur)}");

            Ecrire(Path.Combine(dossier, "mutuelles.csv"), sb.ToString());
        }

        private static void ExporterMutuelPaiements(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;MutuelId;Montant;DatePaiement;UserId;Commentaire");

            foreach (var p in ctx.MutuelPaiements.AsNoTracking().ToList())
                sb.AppendLine($"{p.Id};{p.MutuelId};{p.Montant};" +
                    $"{p.DatePaiement:yyyy-MM-dd HH:mm:ss};{p.UserId};{Echap(p.Commentaire)}");

            Ecrire(Path.Combine(dossier, "mutuel_paiements.csv"), sb.ToString());
        }

        private static void ExporterSessions(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;UserId;DateOuverture;DateCloture;FondOuverture;MontantCompteCloture;Statut");

            foreach (var s in ctx.SessionsCaisse.AsNoTracking().ToList())
                sb.AppendLine($"{s.Id};{s.UserId};{s.DateOuverture:yyyy-MM-dd HH:mm:ss};" +
                    $"{(s.DateCloture.HasValue ? s.DateCloture.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")};" +
                    $"{s.FondOuverture};{s.MontantCompteCloture};{s.Statut}");

            Ecrire(Path.Combine(dossier, "sessions.csv"), sb.ToString());
        }

        private static void ExporterUtilisateurs(AppDbContext ctx, string dossier)
        {
            var sb = new StringBuilder();
            // Ne pas exporter les mots de passe hashés — seulement les infos utiles
            sb.AppendLine("Id;Nom;Prenom;Login;Role");

            foreach (var u in ctx.Users.AsNoTracking().ToList())
                sb.AppendLine($"{u.Id};{Echap(u.Nom)};{Echap(u.Prenom)};{Echap(u.Login)};{u.Role}");

            Ecrire(Path.Combine(dossier, "utilisateurs.csv"), sb.ToString());
        }
    }
}