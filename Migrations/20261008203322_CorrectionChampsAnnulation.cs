using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionChampsAnnulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MotifAnnulation",
                table: "ventes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            // Les ventes actives avaient DateAnnulation = date de vente et MotifAnnulation = "null" (une chaîne).
            migrationBuilder.Sql("UPDATE ventes SET DateAnnulation = NULL, MotifAnnulation = NULL WHERE Statut <> 'Annulée';");
            migrationBuilder.Sql("UPDATE ventes SET MotifAnnulation = NULL WHERE MotifAnnulation = 'null';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MotifAnnulation",
                table: "ventes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
