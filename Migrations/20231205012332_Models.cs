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
                    TipoNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCondominioNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                columns: new[] { "IdNotificacao", "AssuntoNotificacao", "DataEnvioNotificacao", "IdCondominioNotificacao", "MensagemNotificacao", "TipoNotificacao" },
                values: new object[] { 1, "Manutenção elétrica", new DateTime(2023, 12, 6, 9, 13, 22, 0, DateTimeKind.Unspecified), "1", "Haverá manutencão no quadro de força do prédio, dia: 20/12 as 14 horas", "Aviso" });

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
                    { 1, "NBR1354897", new DateTime(2023, 12, 4, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(314), new DateTime(2023, 12, 5, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(326), "Joao Guilherme", 1 },
                    { 2, "NBR2468135", new DateTime(2023, 12, 4, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(333), new DateTime(2023, 12, 6, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(334), "Maria Joaquina", 2 },
                    { 3, "NBR3581415", new DateTime(2023, 12, 4, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(335), new DateTime(2023, 12, 5, 22, 23, 32, 107, DateTimeKind.Local).AddTicks(336), "Ana Clara", 3 }
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
                values: new object[] { 1, null, "admin@gmail.com", 1, new byte[] { 153, 69, 153, 96, 250, 135, 142, 169, 122, 107, 219, 164, 125, 54, 0, 169, 92, 9, 61, 6, 144, 101, 226, 154, 118, 116, 170, 13, 85, 145, 234, 42, 167, 244, 16, 228, 142, 69, 1, 168, 128, 237, 95, 65, 206, 140, 187, 35, 94, 94, 30, 96, 77, 71, 81, 22, 18, 138, 68, 220, 22, 176, 68, 80 }, new byte[] { 194, 25, 9, 61, 133, 187, 16, 224, 94, 217, 133, 60, 240, 124, 1, 212, 216, 153, 236, 26, 35, 76, 252, 93, 212, 32, 51, 80, 128, 52, 187, 123, 140, 227, 242, 149, 2, 109, 29, 213, 194, 138, 32, 9, 86, 68, 219, 219, 219, 201, 240, 181, 66, 114, 124, 243, 102, 9, 147, 228, 156, 4, 138, 241, 31, 223, 232, 236, 100, 182, 33, 8, 100, 60, 70, 144, 158, 253, 32, 95, 66, 48, 229, 28, 215, 120, 158, 249, 121, 63, 57, 155, 226, 162, 141, 205, 208, 50, 147, 131, 20, 164, 253, 35, 150, 248, 80, 196, 112, 125, 5, 16, 142, 190, 187, 195, 181, 204, 142, 23, 54, 189, 9, 114, 123, 108, 78, 213 } });

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
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "PessoasAreasComuns");

            migrationBuilder.DropTable(
                name: "Usuarios");

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
