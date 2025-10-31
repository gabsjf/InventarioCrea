using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemPlanilha.Migrations
{
    /// <inheritdoc />
    public partial class MudarSenhaPrimeiroAcesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MudarSenhaPrimeiroAcesso",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MudarSenhaPrimeiroAcesso",
                table: "AspNetUsers");
        }
    }
}
