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
                    ApartamentoEntIdApart = table.Column<int>(type: "int", nullable: true),
                    IdApartamentoEnt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.IdEnt);
                    table.ForeignKey(
                        name: "FK_Entregas_Apartamentos_ApartamentoEntIdApart",
                        column: x => x.ApartamentoEntIdApart,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApart");
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
                name: "Notificacoes",
                columns: table => new
                {
                    IdNotificacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssuntoNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MensagemNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEnvioNotificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PessoaNotificacaoIdPessoa = table.Column<int>(type: "int", nullable: true),
                    IdPessoaNotificacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.IdNotificacao);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Pessoas_PessoaNotificacaoIdPessoa",
                        column: x => x.PessoaNotificacaoIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
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

            migrationBuilder.CreateTable(
                name: "PessoaNotis",
                columns: table => new
                {
                    IdPessoaPessoaNoti = table.Column<int>(type: "int", nullable: false),
                    IdNotificacaoPessoaNoti = table.Column<int>(type: "int", nullable: false),
                    IdPessoaNoti = table.Column<int>(type: "int", nullable: false),
                    PessoaPessoaNotiIdPessoa = table.Column<int>(type: "int", nullable: true),
                    NotificacaoPessoaNotiIdNotificacao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaNotis", x => new { x.IdPessoaPessoaNoti, x.IdNotificacaoPessoaNoti });
                    table.ForeignKey(
                        name: "FK_PessoaNotis_Notificacoes_NotificacaoPessoaNotiIdNotificacao",
                        column: x => x.NotificacaoPessoaNotiIdNotificacao,
                        principalTable: "Notificacoes",
                        principalColumn: "IdNotificacao");
                    table.ForeignKey(
                        name: "FK_PessoaNotis_Pessoas_PessoaPessoaNotiIdPessoa",
                        column: x => x.PessoaPessoaNotiIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
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
                table: "Entregas",
                columns: new[] { "IdEnt", "ApartamentoEntIdApart", "CodEnt", "DataEntregaEnt", "DataRetiradaEnt", "DestinatarioEnt", "IdApartamentoEnt" },
                values: new object[,]
                {
                    { 1, null, "NBR1354897", new DateTime(2023, 12, 3, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1169), new DateTime(2023, 12, 4, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1182), "Joao Guilherme", 1 },
                    { 2, null, "NBR2468135", new DateTime(2023, 12, 3, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1188), new DateTime(2023, 12, 5, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1189), "Maria Joaquina", 2 },
                    { 3, null, "NBR3581415", new DateTime(2023, 12, 3, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1190), new DateTime(2023, 12, 4, 23, 9, 24, 914, DateTimeKind.Local).AddTicks(1191), "Ana Clara", 3 }
                });

            migrationBuilder.InsertData(
                table: "Notificacoes",
                columns: new[] { "IdNotificacao", "AssuntoNotificacao", "DataEnvioNotificacao", "IdPessoaNotificacao", "MensagemNotificacao", "PessoaNotificacaoIdPessoa" },
                values: new object[] { 1, "Manutenção elétrica", new DateTime(2023, 12, 6, 9, 13, 22, 0, DateTimeKind.Unspecified), 0, "Haverá manutencão no quadro de força do prédio, dia: 20/12 as 14 horas", null });

            migrationBuilder.InsertData(
                table: "PessoaNotis",
                columns: new[] { "IdNotificacaoPessoaNoti", "IdPessoaPessoaNoti", "IdPessoaNoti", "NotificacaoPessoaNotiIdNotificacao", "PessoaPessoaNotiIdPessoa" },
                values: new object[] { 1, 1, 1, null, null });

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
                table: "Usuarios",
                columns: new[] { "IdUsuario", "DataAcessoUsuario", "EmailUsuario", "IdPessoaUsuario", "PasswordHashUsuario", "PasswordSaltUsuario" },
                values: new object[] { 1, null, "admin@gmail.com", 1, new byte[] { 31, 185, 69, 48, 152, 106, 69, 25, 100, 159, 118, 44, 102, 192, 214, 240, 4, 23, 193, 81, 147, 116, 83, 99, 0, 169, 229, 141, 134, 146, 179, 46, 117, 160, 107, 32, 102, 121, 192, 158, 4, 173, 178, 81, 42, 80, 58, 36, 177, 46, 99, 176, 152, 69, 255, 16, 29, 104, 12, 113, 0, 36, 254, 53 }, new byte[] { 59, 24, 107, 168, 252, 120, 28, 218, 22, 235, 63, 31, 30, 121, 3, 94, 161, 30, 191, 168, 250, 122, 254, 152, 245, 56, 143, 204, 48, 231, 249, 189, 16, 64, 211, 83, 28, 238, 34, 66, 33, 208, 203, 87, 221, 128, 102, 193, 154, 228, 71, 17, 102, 105, 49, 25, 70, 16, 18, 190, 228, 47, 207, 102, 37, 251, 108, 229, 208, 55, 79, 21, 215, 223, 177, 14, 242, 168, 72, 203, 72, 67, 54, 115, 131, 1, 23, 145, 3, 210, 1, 116, 27, 13, 228, 219, 133, 170, 51, 5, 182, 207, 220, 19, 77, 100, 210, 121, 15, 226, 133, 240, 86, 172, 108, 172, 154, 211, 52, 205, 87, 86, 62, 255, 42, 166, 247, 60 } });

            migrationBuilder.CreateIndex(
                name: "IX_Apartamentos_IdCondominioApart",
                table: "Apartamentos",
                column: "IdCondominioApart");

            migrationBuilder.CreateIndex(
                name: "IX_Dependentes_IdPessoaDependente",
                table: "Dependentes",
                column: "IdPessoaDependente");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_ApartamentoEntIdApart",
                table: "Entregas",
                column: "ApartamentoEntIdApart");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_PessoaNotificacaoIdPessoa",
                table: "Notificacoes",
                column: "PessoaNotificacaoIdPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_PessoaNotis_NotificacaoPessoaNotiIdNotificacao",
                table: "PessoaNotis",
                column: "NotificacaoPessoaNotiIdNotificacao");

            migrationBuilder.CreateIndex(
                name: "IX_PessoaNotis_PessoaPessoaNotiIdPessoa",
                table: "PessoaNotis",
                column: "PessoaPessoaNotiIdPessoa");

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
