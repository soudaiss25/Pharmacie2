using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class ArrondiRepriseVentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // La reprise (RepriseVentesPayeesEnTotalite) calcule la différence en REAL sans arrondi : une vente à centimes peut donner
            // 586.6700000000001. On arrondit à 2 décimales les seuls paiements de reprise ; la migration d'origine n'est pas modifiée.
            migrationBuilder.Sql(@"UPDATE paiement SET Montant = ROUND(CAST(Montant AS REAL), 2) WHERE NumeroPaiement LIKE 'P-REPRISE-%';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Arrondi non réversible : rien à défaire.
        }
    }
}
