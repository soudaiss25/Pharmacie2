using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutQuantiteUnitesLigneVente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantiteUnites",
                table: "LigneVentes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            // Reprise de l'historique : équivalent en unités de base, d'après le NbUniteParBoite actuel.
            migrationBuilder.Sql(
                @"UPDATE LigneVentes SET QuantiteUnites = CASE WHEN UniteVendue='Boîte' THEN Quantite * COALESCE((SELECT CASE WHEN NbUniteParBoite>1 THEN NbUniteParBoite ELSE 1 END FROM produits WHERE produits.Id=LigneVentes.ProduitId),1) ELSE Quantite END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantiteUnites",
                table: "LigneVentes");
        }
    }
}
