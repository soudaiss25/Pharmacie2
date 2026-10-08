using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie2.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionLibelleMvola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // « Mvolo » était une faute : le service de Telma Comores s'appelle Mvola.
            migrationBuilder.Sql("UPDATE ventes SET MoyenPaiement = 'Mvola' WHERE MoyenPaiement = 'Mvolo';");
            migrationBuilder.Sql("UPDATE ventes SET Type = 'Mvola' WHERE Type = 'Mvolo';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE ventes SET MoyenPaiement = 'Mvolo' WHERE MoyenPaiement = 'Mvola';");
            migrationBuilder.Sql("UPDATE ventes SET Type = 'Mvolo' WHERE Type = 'Mvola';");
        }
    }
}
