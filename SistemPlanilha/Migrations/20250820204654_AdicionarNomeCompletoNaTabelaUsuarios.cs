using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemPlanilha.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarNomeCompletoNaTabelaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona APENAS a coluna que falta na tabela de usuários.
            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a coluna caso precisemos reverter a migração.
            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "AspNetUsers");
        }
    }
}