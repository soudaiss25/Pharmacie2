using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandeChamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateLivraisonPrevue",
                table: "commandes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReception",
                table: "commandes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteCommande",
                table: "commandes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "commandes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateLivraisonPrevue",
                table: "commandes");

            migrationBuilder.DropColumn(
                name: "DateReception",
                table: "commandes");

            migrationBuilder.DropColumn(
                name: "NoteCommande",
                table: "commandes");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "commandes");
        }
    }
}
