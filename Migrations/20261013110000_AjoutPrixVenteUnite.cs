using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutPrixVenteUnite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrixVenteUnite",
                table: "produits",
                type: "TEXT",
                nullable: true);

            // Reprise : les produits déjà vendus au détail gardent exactement le prix qu'ils avaient (prix de la boîte / unités, arrondi au franc),
            // pour que les ventes continuent comme avant le jour de la mise à jour. Le client pourra ensuite fixer lui-même ces prix.
            migrationBuilder.Sql(@"UPDATE produits
SET PrixVenteUnite = ROUND(CAST(PrixVente AS REAL) / NbUniteParBoite, 0)
WHERE NbUniteParBoite > 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrixVenteUnite",
                table: "produits");
        }
    }
}
