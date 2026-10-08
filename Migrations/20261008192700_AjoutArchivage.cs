using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class AjoutArchivage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tous les enregistrements existants restent ACTIFS (defaultValue: true).
            migrationBuilder.DropForeignKey(
                name: "FK_commandes_fournisseur_FournisseurId",
                table: "commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneCommandes_produits_ProduitId",
                table: "LigneCommandes");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneVentes_produits_ProduitId",
                table: "LigneVentes");

            migrationBuilder.DropForeignKey(
                name: "FK_MutuelPaiements_mutuels_MutuelId",
                table: "MutuelPaiements");

            migrationBuilder.AddColumn<bool>(
                name: "Actif",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Actif",
                table: "produits",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Actif",
                table: "mutuels",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Actif",
                table: "fournisseur",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_commandes_fournisseur_FournisseurId",
                table: "commandes",
                column: "FournisseurId",
                principalTable: "fournisseur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneCommandes_produits_ProduitId",
                table: "LigneCommandes",
                column: "ProduitId",
                principalTable: "produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneVentes_produits_ProduitId",
                table: "LigneVentes",
                column: "ProduitId",
                principalTable: "produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MutuelPaiements_mutuels_MutuelId",
                table: "MutuelPaiements",
                column: "MutuelId",
                principalTable: "mutuels",
                principalColumn: "IdMutuel",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commandes_fournisseur_FournisseurId",
                table: "commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneCommandes_produits_ProduitId",
                table: "LigneCommandes");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneVentes_produits_ProduitId",
                table: "LigneVentes");

            migrationBuilder.DropForeignKey(
                name: "FK_MutuelPaiements_mutuels_MutuelId",
                table: "MutuelPaiements");

            migrationBuilder.DropColumn(
                name: "Actif",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Actif",
                table: "produits");

            migrationBuilder.DropColumn(
                name: "Actif",
                table: "mutuels");

            migrationBuilder.DropColumn(
                name: "Actif",
                table: "fournisseur");

            migrationBuilder.AddForeignKey(
                name: "FK_commandes_fournisseur_FournisseurId",
                table: "commandes",
                column: "FournisseurId",
                principalTable: "fournisseur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneCommandes_produits_ProduitId",
                table: "LigneCommandes",
                column: "ProduitId",
                principalTable: "produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneVentes_produits_ProduitId",
                table: "LigneVentes",
                column: "ProduitId",
                principalTable: "produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MutuelPaiements_mutuels_MutuelId",
                table: "MutuelPaiements",
                column: "MutuelId",
                principalTable: "mutuels",
                principalColumn: "IdMutuel",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
