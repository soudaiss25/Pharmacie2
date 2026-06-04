using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class CreationInitiale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fournisseur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Contact = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fournisseur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mutuels",
                columns: table => new
                {
                    IdMutuel = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomEmployeur = table.Column<string>(type: "TEXT", nullable: false),
                    EmailContact = table.Column<string>(type: "TEXT", nullable: false),
                    telephoneEmployeur = table.Column<string>(type: "TEXT", nullable: false),
                    TauxPriseEnCharge = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mutuels", x => x.IdMutuel);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MotDePasse = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "commandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCommande = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FournisseurId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_commandes_fournisseur_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "fournisseur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "produits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    PrixAchat = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrixVente = table.Column<decimal>(type: "TEXT", nullable: false),
                    MargeBeneficiaire = table.Column<decimal>(type: "TEXT", nullable: false),
                    QuantiteEnStock = table.Column<int>(type: "INTEGER", nullable: false),
                    SeuilAlerte = table.Column<int>(type: "INTEGER", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FournisseurId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_produits_fournisseur_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "fournisseur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ventes",
                columns: table => new
                {
                    IdVente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numeroVente = table.Column<string>(type: "TEXT", nullable: false),
                    DateVente = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NomClient = table.Column<string>(type: "TEXT", nullable: false),
                    PrenomClient = table.Column<string>(type: "TEXT", nullable: false),
                    TelephoneClient = table.Column<string>(type: "TEXT", nullable: false),
                    MotifAchat = table.Column<string>(type: "TEXT", nullable: false),
                    MontantTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    MontantEspeces = table.Column<decimal>(type: "TEXT", nullable: false),
                    MontantRendu = table.Column<decimal>(type: "TEXT", nullable: false),
                    MoyenPaiement = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    MutuelId = table.Column<int>(type: "INTEGER", nullable: true),
                    TauxMutuelle = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ventes", x => x.IdVente);
                    table.ForeignKey(
                        name: "FK_ventes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ventes_mutuels_MutuelId",
                        column: x => x.MutuelId,
                        principalTable: "mutuels",
                        principalColumn: "IdMutuel",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LigneCommandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProduitId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantite = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixAchatUnitaire = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigneCommandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LigneCommandes_commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "commandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LigneCommandes_produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LigneVentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProduitId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantite = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixUnitaire = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigneVentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LigneVentes_produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LigneVentes_ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "ventes",
                        principalColumn: "IdVente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paiement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroPaiement = table.Column<string>(type: "TEXT", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    VenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paiement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paiement_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_paiement_ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "ventes",
                        principalColumn: "IdVente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commandes_FournisseurId",
                table: "commandes",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneCommandes_CommandeId",
                table: "LigneCommandes",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneCommandes_ProduitId",
                table: "LigneCommandes",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneVentes_ProduitId",
                table: "LigneVentes",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneVentes_VenteId",
                table: "LigneVentes",
                column: "VenteId");

            migrationBuilder.CreateIndex(
                name: "IX_paiement_UserId",
                table: "paiement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_paiement_VenteId",
                table: "paiement",
                column: "VenteId");

            migrationBuilder.CreateIndex(
                name: "IX_produits_FournisseurId",
                table: "produits",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_ventes_MutuelId",
                table: "ventes",
                column: "MutuelId");

            migrationBuilder.CreateIndex(
                name: "IX_ventes_UserId",
                table: "ventes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LigneCommandes");

            migrationBuilder.DropTable(
                name: "LigneVentes");

            migrationBuilder.DropTable(
                name: "paiement");

            migrationBuilder.DropTable(
                name: "commandes");

            migrationBuilder.DropTable(
                name: "produits");

            migrationBuilder.DropTable(
                name: "ventes");

            migrationBuilder.DropTable(
                name: "fournisseur");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "mutuels");
        }
    }
}
