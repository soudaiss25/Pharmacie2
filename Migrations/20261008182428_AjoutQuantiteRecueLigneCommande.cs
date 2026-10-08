using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutQuantiteRecueLigneCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantiteRecue",
                table: "LigneCommandes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            // Commandes déjà « Reçu » : tout a été reçu. Les autres : 0 (les « Reçu partiellement »
            // sont à vérifier à la main, la quantité réellement reçue n'ayant jamais été mémorisée).
            migrationBuilder.Sql(
                "UPDATE LigneCommandes SET QuantiteRecue = Quantite WHERE CommandeId IN (SELECT Id FROM commandes WHERE Statut = 'Reçu');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantiteRecue",
                table: "LigneCommandes");
        }
    }
}
