using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class NouveauxChamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAnnulation",
                table: "ventes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatriculeEmploye",
                table: "ventes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MontantMutuelle",
                table: "ventes",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MotifAnnulation",
                table: "ventes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "ventes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NbUniteParBoite",
                table: "produits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UniteVente",
                table: "produits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniteVendue",
                table: "LigneVentes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAnnulation",
                table: "ventes");

            migrationBuilder.DropColumn(
                name: "MatriculeEmploye",
                table: "ventes");

            migrationBuilder.DropColumn(
                name: "MontantMutuelle",
                table: "ventes");

            migrationBuilder.DropColumn(
                name: "MotifAnnulation",
                table: "ventes");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "ventes");

            migrationBuilder.DropColumn(
                name: "NbUniteParBoite",
                table: "produits");

            migrationBuilder.DropColumn(
                name: "UniteVente",
                table: "produits");

            migrationBuilder.DropColumn(
                name: "UniteVendue",
                table: "LigneVentes");
        }
    }
}
