using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutModePaiementPaiement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mode",
                table: "paiement",
                type: "TEXT",
                nullable: false,
                defaultValue: "Espèces");

            // Reprise : le mode d'un paiement existant est celui de sa vente pour Chèque / Carte bancaire / Mvola / Huri Money ;
            // Comptant, Mutuelle et Crédit étaient réellement encaissés en espèces (valeur par défaut).
            migrationBuilder.Sql(@"UPDATE paiement
SET Mode = (SELECT v.Type FROM ventes v WHERE v.IdVente = paiement.VenteId)
WHERE VenteId IN (SELECT IdVente FROM ventes WHERE Type IN ('Chèque', 'Carte bancaire', 'Mvola', 'Huri Money'));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "paiement");
        }
    }
}
