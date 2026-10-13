using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class RepriseVentesPayeesEnTotalite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Anciennes ventes payées en totalité (Comptant, Chèque, Carte bancaire, Mvola, Huri Money) dont les paiements
            // ne couvrent pas le total : on insère UN paiement de la différence, daté de la vente, au nom du vendeur.
            // SQL pur et idempotent : rien n'est inséré si P-REPRISE-<IdVente> existe déjà ; les ventes Crédit, Mutuelle et annulées ne sont pas touchées.
            migrationBuilder.Sql(@"INSERT INTO paiement (NumeroPaiement, DatePaiement, Montant, VenteId, UserId, Mode)
SELECT 'P-REPRISE-' || v.IdVente,
       v.DateVente,
       CAST(v.MontantTotal AS REAL) - IFNULL((SELECT SUM(CAST(p.Montant AS REAL)) FROM paiement p WHERE p.VenteId = v.IdVente), 0),
       v.IdVente,
       v.UserId,
       CASE v.Type WHEN 'Comptant' THEN 'Espèces' ELSE v.Type END
FROM ventes v
WHERE v.Statut = 'Active'
  AND v.Type IN ('Comptant', 'Chèque', 'Carte bancaire', 'Mvola', 'Huri Money')
  AND IFNULL((SELECT SUM(CAST(p.Montant AS REAL)) FROM paiement p WHERE p.VenteId = v.IdVente), 0) + 0.0001 < CAST(v.MontantTotal AS REAL)
  AND NOT EXISTS (SELECT 1 FROM paiement p2 WHERE p2.NumeroPaiement = 'P-REPRISE-' || v.IdVente);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Les paiements de reprise sont de vrais paiements : on ne les retire pas.
        }
    }
}
