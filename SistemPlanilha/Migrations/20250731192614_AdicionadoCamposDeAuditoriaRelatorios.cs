using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemPlanilha.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoCamposDeAuditoriaRelatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AtualizadoPor",
                table: "RelatoriosManutencao",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "RelatoriosManutencao",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "RelatoriosManutencao",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtualizadoPor",
                table: "RelatoriosManutencao");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "RelatoriosManutencao");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "RelatoriosManutencao");
        }
    }
}
