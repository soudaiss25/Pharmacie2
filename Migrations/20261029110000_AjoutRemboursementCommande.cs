using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutRemboursementCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandeId",
                table: "DepensesAnnexes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PayeeALaCommande",
                table: "commandes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            // Reprise : les commandes existantes sont considérées comme payées (aucune fausse dette)
            migrationBuilder.Sql("UPDATE commandes SET PayeeALaCommande = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayeeALaCommande",
                table: "commandes");

            migrationBuilder.DropColumn(
                name: "CommandeId",
                table: "DepensesAnnexes");
        }
    }
}
