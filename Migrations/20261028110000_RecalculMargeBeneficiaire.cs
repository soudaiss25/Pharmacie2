using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class RecalculMargeBeneficiaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // La marge n'est plus une saisie : elle se déduit des deux prix saisis par le client. Reprise : (prix de vente − prix d'achat) ÷ prix d'achat × 100,
            // arrondie à 2 décimales ; 0 si le prix d'achat n'est pas renseigné. Aucun prix n'est modifié.
            migrationBuilder.Sql(@"UPDATE produits
SET MargeBeneficiaire = CASE WHEN CAST(PrixAchat AS REAL) > 0
    THEN ROUND((CAST(PrixVente AS REAL) - CAST(PrixAchat AS REAL)) / CAST(PrixAchat AS REAL) * 100, 2)
    ELSE 0 END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // La marge d'origine (0 % partout) n'est pas restaurée : elle n'avait aucun sens.
        }
    }
}
