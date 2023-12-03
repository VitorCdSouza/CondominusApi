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
                    CondominioApartIdCond = table.Column<int>(type: "int", nullable: true),
                    IdCondominioApart = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartamentos", x => x.IdApart);
                    table.ForeignKey(
                        name: "FK_Apartamentos_Condominios_CondominioApartIdCond",
                        column: x => x.CondominioApartIdCond,
                        principalTable: "Condominios",
                        principalColumn: "IdCond");
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
                    ApartamentoPessoaIdApart = table.Column<int>(type: "int", nullable: true),
                    IdApartamentoPessoa = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioPessoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.IdPessoa);
                    table.ForeignKey(
                        name: "FK_Pessoas_Apartamentos_ApartamentoPessoaIdApart",
                        column: x => x.ApartamentoPessoaIdApart,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApart");
                });

            migrationBuilder.CreateTable(
                name: "Dependentes",
                columns: table => new
                {
                    IdDependente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDependente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpfDependente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PessoaDependenteIdPessoa = table.Column<int>(type: "int", nullable: true),
                    IdPessoaDependente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependentes", x => x.IdDependente);
                    table.ForeignKey(
                        name: "FK_Dependentes_Pessoas_PessoaDependenteIdPessoa",
                        column: x => x.PessoaDependenteIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
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
                    IdPessArea = table.Column<int>(type: "int", nullable: false),
                    dataHoraInicioPessArea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataHoraFimPessArea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PessoaPessAreaIdPessoa = table.Column<int>(type: "int", nullable: true),
                    AreaComumPessAreaIdAreaComum = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoasAreasComuns", x => new { x.IdPessoaPessArea, x.IdAreaComumPessArea });
                    table.ForeignKey(
                        name: "FK_PessoasAreasComuns_AreasComuns_AreaComumPessAreaIdAreaComum",
                        column: x => x.AreaComumPessAreaIdAreaComum,
                        principalTable: "AreasComuns",
                        principalColumn: "IdAreaComum");
                    table.ForeignKey(
                        name: "FK_PessoasAreasComuns_Pessoas_PessoaPessAreaIdPessoa",
                        column: x => x.PessoaPessAreaIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
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
                table: "Apartamentos",
                columns: new[] { "IdApart", "CondominioApartIdCond", "IdCondominioApart", "NumeroApart", "TelefoneApart" },
                values: new object[,]
                {
                    { 1, null, 1, "A001", "11912345678" },
                    { 2, null, 1, "B002", "11912345678" },
                    { 3, null, 1, "C003", "11887654321" }
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
                table: "Dependentes",
                columns: new[] { "IdDependente", "CpfDependente", "IdPessoaDependente", "NomeDependente", "PessoaDependenteIdPessoa" },
                values: new object[,]
                {
                    { 1, "11242100083", 1, "João Gomes", null },
                    { 2, "30777454025", 1, "Maria Silva", null },
                    { 3, "53086593032", 2, "Carlos Oliveira", null },
                    { 4, "54710630070", 3, "Ana Souza", null },
                    { 5, "03940474002", 3, "Pedro Santos", null }
                });

            migrationBuilder.InsertData(
                table: "Entregas",
                columns: new[] { "IdEnt", "ApartamentoEntIdApart", "CodEnt", "DataEntregaEnt", "DataRetiradaEnt", "DestinatarioEnt", "IdApartamentoEnt" },
                values: new object[,]
                {
                    { 1, null, "NBR1354897", new DateTime(2023, 12, 3, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1854), new DateTime(2023, 12, 4, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1869), "Joao Guilherme", 1 },
                    { 2, null, "NBR2468135", new DateTime(2023, 12, 3, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1876), new DateTime(2023, 12, 5, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1877), "Maria Joaquina", 2 },
                    { 3, null, "NBR3581415", new DateTime(2023, 12, 3, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1879), new DateTime(2023, 12, 4, 15, 10, 19, 154, DateTimeKind.Local).AddTicks(1879), "Ana Clara", 3 }
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
                table: "Pessoas",
                columns: new[] { "IdPessoa", "ApartamentoPessoaIdApart", "CpfPessoa", "IdApartamentoPessoa", "IdUsuarioPessoa", "NomePessoa", "TelefonePessoa", "TipoPessoa" },
                values: new object[,]
                {
                    { 1, null, "56751898901", 1, null, "João Gomes", "11924316523", "Sindico" },
                    { 2, null, "89674156892", 2, null, "Maria Oliveira", "1198254351", "Morador" },
                    { 3, null, "32569874561", 3, null, "João Viana", "11984512345", "Morador" }
                });

            migrationBuilder.InsertData(
                table: "PessoasAreasComuns",
                columns: new[] { "IdAreaComumPessArea", "IdPessoaPessArea", "AreaComumPessAreaIdAreaComum", "IdPessArea", "PessoaPessAreaIdPessoa", "dataHoraFimPessArea", "dataHoraInicioPessArea" },
                values: new object[,]
                {
                    { 1, 1, null, 1, null, new DateTime(2023, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 5, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 2, null, 2, null, new DateTime(2023, 12, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 6, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, null, 2, null, new DateTime(2023, 12, 16, 21, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "DataAcessoUsuario", "EmailUsuario", "IdPessoaUsuario", "PasswordHashUsuario", "PasswordSaltUsuario" },
                values: new object[] { 1, null, "admin@gmail.com", 1, new byte[] { 76, 82, 99, 239, 101, 0, 118, 171, 229, 114, 129, 203, 22, 70, 225, 171, 22, 195, 70, 62, 198, 140, 184, 216, 190, 150, 80, 186, 20, 198, 104, 227, 161, 221, 244, 222, 3, 155, 227, 50, 123, 89, 16, 197, 17, 235, 166, 102, 90, 56, 47, 84, 151, 121, 141, 172, 224, 26, 19, 25, 71, 141, 163, 110 }, new byte[] { 33, 237, 141, 147, 206, 176, 46, 83, 186, 126, 77, 143, 127, 105, 48, 15, 94, 45, 132, 245, 248, 226, 141, 223, 81, 34, 120, 223, 105, 220, 139, 188, 190, 161, 239, 198, 62, 133, 155, 48, 238, 247, 230, 223, 140, 246, 179, 215, 209, 89, 97, 68, 109, 33, 148, 146, 251, 195, 116, 63, 192, 220, 72, 186, 235, 144, 87, 73, 59, 226, 79, 28, 49, 227, 163, 211, 95, 172, 126, 21, 7, 82, 128, 142, 199, 192, 222, 177, 244, 145, 151, 227, 89, 110, 130, 63, 64, 167, 124, 124, 32, 114, 10, 168, 236, 152, 42, 199, 252, 32, 38, 148, 140, 36, 132, 224, 166, 171, 181, 131, 145, 250, 238, 95, 227, 34, 197, 210 } });

            migrationBuilder.CreateIndex(
                name: "IX_Apartamentos_CondominioApartIdCond",
                table: "Apartamentos",
                column: "CondominioApartIdCond");

            migrationBuilder.CreateIndex(
                name: "IX_Dependentes_PessoaDependenteIdPessoa",
                table: "Dependentes",
                column: "PessoaDependenteIdPessoa");

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
                name: "IX_Pessoas_ApartamentoPessoaIdApart",
                table: "Pessoas",
                column: "ApartamentoPessoaIdApart");

            migrationBuilder.CreateIndex(
                name: "IX_PessoasAreasComuns_AreaComumPessAreaIdAreaComum",
                table: "PessoasAreasComuns",
                column: "AreaComumPessAreaIdAreaComum");

            migrationBuilder.CreateIndex(
                name: "IX_PessoasAreasComuns_PessoaPessAreaIdPessoa",
                table: "PessoasAreasComuns",
                column: "PessoaPessAreaIdPessoa");
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
