using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutSessionCaisse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontantOuverture",
                table: "SessionsCaisse");

            migrationBuilder.RenameColumn(
                name: "Observations",
                table: "SessionsCaisse",
                newName: "FondOuverture");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "SessionsCaisse",
                newName: "CommentaireCloture");

            migrationBuilder.RenameColumn(
                name: "MontantFermeture",
                table: "SessionsCaisse",
                newName: "MontantCompteCloture");

            migrationBuilder.RenameColumn(
                name: "DateFermeture",
                table: "SessionsCaisse",
                newName: "DateCloture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontantCompteCloture",
                table: "SessionsCaisse",
                newName: "MontantFermeture");

            migrationBuilder.RenameColumn(
                name: "FondOuverture",
                table: "SessionsCaisse",
                newName: "Observations");

            migrationBuilder.RenameColumn(
                name: "DateCloture",
                table: "SessionsCaisse",
                newName: "DateFermeture");

            migrationBuilder.RenameColumn(
                name: "CommentaireCloture",
                table: "SessionsCaisse",
                newName: "Numero");

            migrationBuilder.AddColumn<decimal>(
                name: "MontantOuverture",
                table: "SessionsCaisse",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
