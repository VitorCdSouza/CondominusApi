using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CondominusApi.Migrations
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreasComuns",
                columns: table => new
                {
                    IdAreaComum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeAreaComum = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasComuns", x => x.IdAreaComum);
                });

            migrationBuilder.CreateTable(
                name: "Condominios",
                columns: table => new
                {
                    IdCond = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCond = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnderecoCond = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condominios", x => x.IdCond);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    IdNotificacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssuntoNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MensagemNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEnvioNotificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdPessoaNotificacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.IdNotificacao);
                });

            migrationBuilder.CreateTable(
                name: "Apartamentos",
                columns: table => new
                {
                    IdApart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelefoneApart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroApart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCondominioApart = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartamentos", x => x.IdApart);
                    table.ForeignKey(
                        name: "FK_Apartamentos_Condominios_IdCondominioApart",
                        column: x => x.IdCondominioApart,
                        principalTable: "Condominios",
                        principalColumn: "IdCond",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entregas",
                columns: table => new
                {
                    IdEnt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinatarioEnt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodEnt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEntregaEnt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataRetiradaEnt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdApartamentoEnt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.IdEnt);
                    table.ForeignKey(
                        name: "FK_Entregas_Apartamentos_IdApartamentoEnt",
                        column: x => x.IdApartamentoEnt,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApart",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    IdPessoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomePessoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonePessoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPessoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpfPessoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdApartamentoPessoa = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioPessoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.IdPessoa);
                    table.ForeignKey(
                        name: "FK_Pessoas_Apartamentos_IdApartamentoPessoa",
                        column: x => x.IdApartamentoPessoa,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApart",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dependentes",
                columns: table => new
                {
                    IdDependente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDependente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpfDependente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdPessoaDependente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependentes", x => x.IdDependente);
                    table.ForeignKey(
                        name: "FK_Dependentes_Pessoas_IdPessoaDependente",
                        column: x => x.IdPessoaDependente,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PessoaNotis",
                columns: table => new
                {
                    IdPessoaPessoaNoti = table.Column<int>(type: "int", nullable: false),
                    IdNotificacaoPessoaNoti = table.Column<int>(type: "int", nullable: false),
                    IdPessoaNoti = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaNotis", x => new { x.IdPessoaPessoaNoti, x.IdNotificacaoPessoaNoti });
                    table.ForeignKey(
                        name: "FK_PessoaNotis_Notificacoes_IdNotificacaoPessoaNoti",
                        column: x => x.IdNotificacaoPessoaNoti,
                        principalTable: "Notificacoes",
                        principalColumn: "IdNotificacao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoaNotis_Pessoas_IdPessoaPessoaNoti",
                        column: x => x.IdPessoaPessoaNoti,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PessoasAreasComuns",
                columns: table => new
                {
                    IdPessoaPessArea = table.Column<int>(type: "int", nullable: false),
                    IdAreaComumPessArea = table.Column<int>(type: "int", nullable: false),
                    IdPessArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataHoraInicioPessArea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataHoraFimPessArea = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoasAreasComuns", x => new { x.IdPessoaPessArea, x.IdAreaComumPessArea });
                    table.ForeignKey(
                        name: "FK_PessoasAreasComuns_AreasComuns_IdAreaComumPessArea",
                        column: x => x.IdAreaComumPessArea,
                        principalTable: "AreasComuns",
                        principalColumn: "IdAreaComum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoasAreasComuns_Pessoas_IdPessoaPessArea",
                        column: x => x.IdPessoaPessArea,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    EmailUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHashUsuario = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSaltUsuario = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DataAcessoUsuario = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdPessoaUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Pessoas_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AreasComuns",
                columns: new[] { "IdAreaComum", "NomeAreaComum" },
                values: new object[,]
                {
                    { 1, "Churrasqueira" },
                    { 2, "Salão de Jogos" },
                    { 3, "Quadra" }
                });

            migrationBuilder.InsertData(
                table: "Condominios",
                columns: new[] { "IdCond", "EnderecoCond", "NomeCond" },
                values: new object[,]
                {
                    { 1, "Rua Guaranésia, 1070", "Vila Nova Maria" },
                    { 2, "Rua Paulo Andrighetti, 1573", "Condomínio Aquarella Pari Colore" },
                    { 3, "Rua Paulo Andrighetti, 449", "Condomínio Edifício Antônio Walter Santiago" }
                });

            migrationBuilder.InsertData(
                table: "Notificacoes",
                columns: new[] { "IdNotificacao", "AssuntoNotificacao", "DataEnvioNotificacao", "IdPessoaNotificacao", "MensagemNotificacao" },
                values: new object[] { 1, "Manutenção elétrica", new DateTime(2023, 12, 6, 9, 13, 22, 0, DateTimeKind.Unspecified), 0, "Haverá manutencão no quadro de força do prédio, dia: 20/12 as 14 horas" });

            migrationBuilder.InsertData(
                table: "Apartamentos",
                columns: new[] { "IdApart", "IdCondominioApart", "NumeroApart", "TelefoneApart" },
                values: new object[,]
                {
                    { 1, 1, "A001", "11912345678" },
                    { 2, 1, "B002", "11912345678" },
                    { 3, 1, "C003", "11887654321" }
                });

            migrationBuilder.InsertData(
                table: "Entregas",
                columns: new[] { "IdEnt", "CodEnt", "DataEntregaEnt", "DataRetiradaEnt", "DestinatarioEnt", "IdApartamentoEnt" },
                values: new object[,]
                {
                    { 1, "NBR1354897", new DateTime(2023, 12, 4, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8709), new DateTime(2023, 12, 5, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8724), "Joao Guilherme", 1 },
                    { 2, "NBR2468135", new DateTime(2023, 12, 4, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8730), new DateTime(2023, 12, 6, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8731), "Maria Joaquina", 2 },
                    { 3, "NBR3581415", new DateTime(2023, 12, 4, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8733), new DateTime(2023, 12, 5, 16, 52, 49, 642, DateTimeKind.Local).AddTicks(8733), "Ana Clara", 3 }
                });

            migrationBuilder.InsertData(
                table: "Pessoas",
                columns: new[] { "IdPessoa", "CpfPessoa", "IdApartamentoPessoa", "IdUsuarioPessoa", "NomePessoa", "TelefonePessoa", "TipoPessoa" },
                values: new object[,]
                {
                    { 1, "56751898901", 1, null, "João Gomes", "11924316523", "Admin" },
                    { 2, "89674156892", 2, null, "Maria Oliveira", "1198254351", "Morador" },
                    { 3, "32569874561", 3, null, "João Viana", "11984512345", "Morador" }
                });

            migrationBuilder.InsertData(
                table: "Dependentes",
                columns: new[] { "IdDependente", "CpfDependente", "IdPessoaDependente", "NomeDependente" },
                values: new object[,]
                {
                    { 1, "11242100083", 1, "João Gomes" },
                    { 2, "30777454025", 1, "Maria Silva" },
                    { 3, "53086593032", 2, "Carlos Oliveira" },
                    { 4, "54710630070", 3, "Ana Souza" },
                    { 5, "03940474002", 3, "Pedro Santos" }
                });

            migrationBuilder.InsertData(
                table: "PessoaNotis",
                columns: new[] { "IdNotificacaoPessoaNoti", "IdPessoaPessoaNoti", "IdPessoaNoti" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "DataAcessoUsuario", "EmailUsuario", "IdPessoaUsuario", "PasswordHashUsuario", "PasswordSaltUsuario" },
                values: new object[] { 1, null, "admin@gmail.com", 1, new byte[] { 232, 179, 122, 22, 7, 112, 236, 81, 30, 170, 55, 87, 242, 122, 254, 225, 112, 228, 182, 122, 75, 202, 100, 241, 248, 152, 74, 20, 21, 136, 26, 43, 116, 85, 204, 201, 101, 192, 106, 53, 116, 223, 60, 89, 63, 244, 160, 9, 37, 34, 41, 76, 228, 153, 39, 47, 215, 103, 188, 22, 2, 185, 104, 44 }, new byte[] { 195, 120, 245, 121, 241, 22, 63, 75, 234, 191, 11, 87, 14, 91, 81, 186, 161, 55, 199, 241, 62, 29, 67, 132, 118, 102, 40, 177, 67, 158, 211, 36, 26, 136, 194, 131, 253, 247, 39, 22, 230, 115, 113, 201, 231, 113, 113, 195, 230, 120, 65, 43, 118, 197, 52, 13, 218, 81, 24, 87, 86, 50, 42, 36, 229, 98, 217, 224, 20, 222, 68, 192, 95, 80, 24, 91, 100, 135, 225, 9, 61, 209, 124, 144, 208, 175, 124, 14, 168, 194, 35, 181, 6, 35, 160, 180, 82, 95, 177, 22, 167, 61, 86, 73, 212, 101, 35, 226, 191, 225, 43, 245, 24, 98, 101, 78, 163, 84, 38, 66, 181, 154, 41, 148, 99, 161, 85, 170 } });

            migrationBuilder.CreateIndex(
                name: "IX_Apartamentos_IdCondominioApart",
                table: "Apartamentos",
                column: "IdCondominioApart");

            migrationBuilder.CreateIndex(
                name: "IX_Dependentes_IdPessoaDependente",
                table: "Dependentes",
                column: "IdPessoaDependente");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_IdApartamentoEnt",
                table: "Entregas",
                column: "IdApartamentoEnt");

            migrationBuilder.CreateIndex(
                name: "IX_PessoaNotis_IdNotificacaoPessoaNoti",
                table: "PessoaNotis",
                column: "IdNotificacaoPessoaNoti");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_IdApartamentoPessoa",
                table: "Pessoas",
                column: "IdApartamentoPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_PessoasAreasComuns_IdAreaComumPessArea",
                table: "PessoasAreasComuns",
                column: "IdAreaComumPessArea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dependentes");

            migrationBuilder.DropTable(
                name: "Entregas");

            migrationBuilder.DropTable(
                name: "PessoaNotis");

            migrationBuilder.DropTable(
                name: "PessoasAreasComuns");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "AreasComuns");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Apartamentos");

            migrationBuilder.DropTable(
                name: "Condominios");
        }
    }
}
