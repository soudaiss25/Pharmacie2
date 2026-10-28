using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutCoutAchatLigneVente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoutAchatUnitaire",
                table: "LigneVentes",
                type: "TEXT",
                nullable: true);

            // Reprise (documentée) : l'ancienne version ne gardait pas le coût du jour de la vente. On prend le prix d'achat ACTUEL du produit
            // ÷ max(1, nombre d'unités par boîte), arrondi à 4 décimales. NULL si le produit n'existe plus. Dès maintenant le coût est figé à chaque vente.
            migrationBuilder.Sql(@"UPDATE LigneVentes
SET CoutAchatUnitaire = (SELECT ROUND(CAST(p.PrixAchat AS REAL) / MAX(1, p.NbUniteParBoite), 4)
                         FROM produits p WHERE p.Id = LigneVentes.ProduitId);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoutAchatUnitaire",
                table: "LigneVentes");
        }
    }
}
