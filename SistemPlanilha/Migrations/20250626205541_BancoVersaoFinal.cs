using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemPlanilha.Migrations
{
    /// <inheritdoc />
    public partial class BancoVersaoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusesManutencao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusesManutencao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WinVer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinVer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PcName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patrimonio = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrevisaoDevolucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicencaSO = table.Column<bool>(type: "bit", nullable: false),
                    LicencaOffice = table.Column<bool>(type: "bit", nullable: false),
                    Processador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ssd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataVerificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetorId = table.Column<int>(type: "int", nullable: true),
                    TipoId = table.Column<int>(type: "int", nullable: true),
                    SituacaoId = table.Column<int>(type: "int", nullable: true),
                    WinVerId = table.Column<int>(type: "int", nullable: true),
                    OfficeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventario_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventario_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventario_Situacoes_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Situacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventario_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventario_WinVer_WinVerId",
                        column: x => x.WinVerId,
                        principalTable: "WinVer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RelatoriosManutencao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventarioId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoManutencao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcoesRealizadas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempoEstimadoResolucao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusManutencaoId = table.Column<int>(type: "int", nullable: false),
                    ObservacoesAdicionais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProximaManutencao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsavelTecnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PecasSubstituidas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosManutencao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatoriosManutencao_Inventario_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatoriosManutencao_StatusesManutencao_StatusManutencaoId",
                        column: x => x.StatusManutencaoId,
                        principalTable: "StatusesManutencao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_OfficeId",
                table: "Inventario",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_SetorId",
                table: "Inventario",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_SituacaoId",
                table: "Inventario",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_TipoId",
                table: "Inventario",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_WinVerId",
                table: "Inventario",
                column: "WinVerId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosManutencao_InventarioId",
                table: "RelatoriosManutencao",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosManutencao_StatusManutencaoId",
                table: "RelatoriosManutencao",
                column: "StatusManutencaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatoriosManutencao");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "StatusesManutencao");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "Situacoes");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropTable(
                name: "WinVer");
        }
    }
}
