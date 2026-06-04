using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutMutuelPaiement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MutuelPaiements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroPaiement = table.Column<string>(type: "TEXT", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Commentaire = table.Column<string>(type: "TEXT", nullable: false),
                    VenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    MutuelId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MutuelPaiements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MutuelPaiements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MutuelPaiements_mutuels_MutuelId",
                        column: x => x.MutuelId,
                        principalTable: "mutuels",
                        principalColumn: "IdMutuel",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MutuelPaiements_ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "ventes",
                        principalColumn: "IdVente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionsCaisse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    DateOuverture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFermeture = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MontantOuverture = table.Column<decimal>(type: "TEXT", nullable: false),
                    MontantFermeture = table.Column<decimal>(type: "TEXT", nullable: true),
                    Statut = table.Column<string>(type: "TEXT", nullable: false),
                    Observations = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionsCaisse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionsCaisse_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MutuelPaiements_MutuelId",
                table: "MutuelPaiements",
                column: "MutuelId");

            migrationBuilder.CreateIndex(
                name: "IX_MutuelPaiements_UserId",
                table: "MutuelPaiements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MutuelPaiements_VenteId",
                table: "MutuelPaiements",
                column: "VenteId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionsCaisse_UserId",
                table: "SessionsCaisse",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MutuelPaiements");

            migrationBuilder.DropTable(
                name: "SessionsCaisse");
        }
    }
}
