using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class NettoyageChampsAnnulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Les ventes actives avaient DateAnnulation = date de vente et MotifAnnulation = "null" (une chaîne).
            // La colonne est devenue nullable dans CorrectionChampsAnnulation (migration précédente).
            migrationBuilder.Sql("UPDATE ventes SET DateAnnulation = NULL, MotifAnnulation = NULL WHERE Statut <> 'Annulée';");
            migrationBuilder.Sql("UPDATE ventes SET MotifAnnulation = NULL WHERE MotifAnnulation = 'null';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
