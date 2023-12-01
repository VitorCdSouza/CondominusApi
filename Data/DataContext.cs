using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CondominusApi.Models;
using CondominusApi.Data;
using System.Data;
using CondominusApi.Utils;

namespace CondominusApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Apartamento> Apartamentos { get; set; }
        public DbSet<AreaComum> AreasComuns { get; set; }
        public DbSet<Condominio> Condominios { get; set; }
        public DbSet<Dependente> Dependentes { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<PessoaAreaComum> PessoasAreasComuns { get; set; }
        public DbSet<PessoaNoti> PessoaNotis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // declaracao do relacionamento entre pessoa e area comum N para N
            modelBuilder.Entity<PessoaAreaComum>()
                .HasKey(pac => new { pac.IdPessoaPessArea, pac.IdAreaComumPessArea });


            modelBuilder.Entity<Condominio>().HasData(
                new Condominio() { IdCond = 1, NomeCond = "Vila Nova Maria", EnderecoCond = "Rua Guaranésia, 1070", ApartamentosCond = {}},
                new Condominio() { IdCond = 2, NomeCond = "Condomínio Aquarella Pari Colore", EnderecoCond = "Rua Paulo Andrighetti, 1573"},
                new Condominio() { IdCond = 3, NomeCond = "Condomínio Edifício Antônio Walter Santiago", EnderecoCond = "Rua Paulo Andrighetti, 449"}
            );
            modelBuilder.Entity<Apartamento>().HasData(
                new Apartamento() { IdApart = 1, TelefoneApart = "11912345678", NumeroApart = "A001", IdCondominioApart = 1 },
                new Apartamento() { IdApart = 2, TelefoneApart = "11912345678", NumeroApart = "B002", IdCondominioApart = 1  },
                new Apartamento() { IdApart = 3, TelefoneApart = "11887654321", NumeroApart = "C003", IdCondominioApart = 1  }
            );
            modelBuilder.Entity<Entrega>().HasData( // new DateTime(2021, 06, 05, 10, 20, 30) yyyy/MM/dd HH:mm:ss
                new Entrega() { IdEnt = 1, DestinatarioEnt = "Joao Guilherme", CodEnt = "NBR1354897", DataEntregaEnt = new DateTime(2023, 12, 25, 24, 59, 52), DataRetiradaEnt = new DateTime(2023, 12, 26, 24, 59, 52), IdApartamentoEnt = 1 },
                new Entrega() { IdEnt = 2, DestinatarioEnt = "Maria Joaquina", CodEnt = "NBR2468135", DataEntregaEnt = new DateTime(2023, 11, 25, 14, 30, 12), DataRetiradaEnt = new DateTime(2023, 11, 26, 14, 30, 12), IdApartamentoEnt = 2 },
                new Entrega() { IdEnt = 3, DestinatarioEnt = "Ana Clara", CodEnt = "NBR3581415", DataEntregaEnt = new DateTime(2023, 10, 25, 22, 00, 30), DataRetiradaEnt = new DateTime(2023, 10, 26, 22, 00, 30), IdApartamentoEnt = 3 }
            );
            modelBuilder.Entity<Pessoa>().HasData( // usuario adicionado depois
                new Pessoa(){ IdPessoa = 1, NomePessoa = "João Gomes", TelefonePessoa = "11924316523", TipoPessoa = "Sindico", CpfPessoa = "56751898901", IdApartamentoPessoa = 1},
                new Pessoa(){ IdPessoa = 2, NomePessoa = "Maria Oliveira", TelefonePessoa = "1198254351", TipoPessoa = "Morador", CpfPessoa = "89674156892", IdApartamentoPessoa = 2},
                new Pessoa(){ IdPessoa = 3, NomePessoa = "João Viana", TelefonePessoa = "11984512345", TipoPessoa = "Morador", CpfPessoa = "32569874561", IdApartamentoPessoa = 3}
            );
            // criacao senha para usuarios padroes
            Criptografia.CriarPasswordHash("123456", out byte[] hash, out byte[] salt);
            Criptografia.CriarPasswordHash("654321", out byte[] hashJ, out byte[] saltJ);

            modelBuilder.Entity<Usuario>().HasData
            (
                new Usuario() { IdUsuario = 1, EmailUsuario = "admin@gmail.com", PasswordHashUsuario = hash, PasswordSaltUsuario = salt, SenhaUsuario = null, IdPessoaUsuario = 1 }
            );
            modelBuilder.Entity<AreaComum>().HasData(
                new AreaComum() { Id = 1, Capacidade = 50, Nome = "Salão de Festas" },
                new AreaComum() { Id = 2, Capacidade = 30, Nome = "Churrasqueira" },
                new AreaComum() { Id = 3, Capacidade = 20, Nome = "Sala de Jogos" },
                new AreaComum() { Id = 4, Capacidade = 10, Nome = "Piscina" }
            );
            modelBuilder.Entity<Reserva>().HasData(
                new Reserva(){ Id = 1},
                new Reserva(){ Id = 2},
                new Reserva(){ Id = 3},
                new Reserva(){ Id = 4}
            );
            modelBuilder.Entity<Dependente>().HasData(
                new Dependente() { Id = 1, Nome = "João Gomes", Telefone = "11924316523", IdPessoa = 1},
                new Dependente() { Id = 2, Nome = "Maria Silva", Telefone = "11876543210", IdPessoa = 1 },
                new Dependente() { Id = 3, Nome = "Carlos Oliveira", Telefone = "11234567890", IdPessoa = 2 },
                new Dependente() { Id = 4, Nome = "Ana Souza", Telefone = "11987654321", IdPessoa = 3 },
                new Dependente() { Id = 5, Nome = "Pedro Santos", Telefone = "11765432109", IdPessoa = 3 }
            );
        }
    }
}