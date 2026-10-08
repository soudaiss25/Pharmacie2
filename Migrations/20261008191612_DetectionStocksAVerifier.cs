using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class DetectionStocksAVerifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotifVerification",
                table: "produits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StockAVerifier",
                table: "produits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UnitesManquantesEstimees",
                table: "produits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            // Avant la phase 6, réceptionner N boîtes ajoutait N unités au lieu de N × NbUniteParBoite.
            // Cette migration ne s'exécute qu'une fois, à la mise à jour : donc avant toute réception
            // faite avec la nouvelle version. Seuls les produits NbUniteParBoite > 1 sont concernés.
            // Rien n'est corrigé ici : on ne fait que marquer et estimer.
            migrationBuilder.Sql(@"
UPDATE produits SET
  StockAVerifier = 1,
  UnitesManquantesEstimees =
    (SELECT COALESCE(SUM(lc.Quantite), 0) FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
      WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu') * (NbUniteParBoite - 1),
  MotifVerification =
    CASE
      WHEN (SELECT COALESCE(SUM(lc.Quantite), 0) FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
             WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu') > 0
      THEN
        (SELECT COALESCE(SUM(lc.Quantite), 0) FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
          WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu')
        || ' boîte(s) reçue(s) avant la correction : seulement '
        || (SELECT COALESCE(SUM(lc.Quantite), 0) FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
             WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu')
        || ' unité(s) ajoutée(s) au lieu de '
        || ((SELECT COALESCE(SUM(lc.Quantite), 0) FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
              WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu') * NbUniteParBoite)
        || '.'
        || CASE WHEN EXISTS (SELECT 1 FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
                              WHERE lc.ProduitId = produits.Id AND c.Statut = 'Reçu partiellement')
                THEN ' De plus, une commande reçue partiellement : quantité reçue inconnue.' ELSE '' END
      ELSE 'Commande reçue partiellement avant la correction : quantité reçue inconnue, le stock est à vérifier.'
    END
WHERE NbUniteParBoite > 1
  AND EXISTS (SELECT 1 FROM LigneCommandes lc JOIN commandes c ON c.Id = lc.CommandeId
               WHERE lc.ProduitId = produits.Id AND c.Statut IN ('Reçu', 'Reçu partiellement'));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotifVerification",
                table: "produits");

            migrationBuilder.DropColumn(
                name: "StockAVerifier",
                table: "produits");

            migrationBuilder.DropColumn(
                name: "UnitesManquantesEstimees",
                table: "produits");
        }
    }
}
