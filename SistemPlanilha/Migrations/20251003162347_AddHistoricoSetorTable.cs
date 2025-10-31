using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemPlanilha.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricoSetorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoSetores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventarioId = table.Column<int>(type: "int", nullable: false),
                    SetorOrigemId = table.Column<int>(type: "int", nullable: false),
                    SetorDestinoId = table.Column<int>(type: "int", nullable: false),
                    ResponsavelAlteracao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoSetores_Inventario_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoSetores_Setores_SetorDestinoId",
                        column: x => x.SetorDestinoId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricoSetores_Setores_SetorOrigemId",
                        column: x => x.SetorOrigemId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSetores_InventarioId",
                table: "HistoricoSetores",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSetores_SetorDestinoId",
                table: "HistoricoSetores",
                column: "SetorDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSetores_SetorOrigemId",
                table: "HistoricoSetores",
                column: "SetorOrigemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoSetores");
        }
    }
}
