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
            // declaracao do relacionamento entre pessoa e notificacao N para N
            modelBuilder.Entity<PessoaNoti>()
                .HasKey(pac => new { pac.IdPessoaPessoaNoti, pac.IdNotificacaoPessoaNoti });
            // declaracao do relacionamento entre pessoa e usuario 1 para 1
            modelBuilder.Entity<Pessoa>()
            .HasOne(p => p.UsuarioPessoa)
            .WithOne(u => u.PessoaUsuario)
            .HasForeignKey<Usuario>(u => u.IdUsuario)
            .IsRequired();

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
                new Entrega() { IdEnt = 1, DestinatarioEnt = "Joao Guilherme", CodEnt = "NBR1354897", DataEntregaEnt = DateTime.Now, DataRetiradaEnt = DateTime.Now.AddDays(1), IdApartamentoEnt = 1 },
                new Entrega() { IdEnt = 2, DestinatarioEnt = "Maria Joaquina", CodEnt = "NBR2468135", DataEntregaEnt = DateTime.Now, DataRetiradaEnt = DateTime.Now.AddDays(2), IdApartamentoEnt = 2 },
                new Entrega() { IdEnt = 3, DestinatarioEnt = "Ana Clara", CodEnt = "NBR3581415", DataEntregaEnt = DateTime.Now, DataRetiradaEnt = DateTime.Now.AddDays(1), IdApartamentoEnt = 3 }
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
                new AreaComum() { IdAreaComum = 1, NomeAreaComum = "Churrasqueira" },
                new AreaComum() { IdAreaComum = 2, NomeAreaComum = "Salão de Jogos" },
                new AreaComum() { IdAreaComum = 3, NomeAreaComum = "Quadra" }
            );
            modelBuilder.Entity<PessoaAreaComum>().HasData(
                new PessoaAreaComum() { IdPessArea = 1, dataHoraInicioPessArea = new DateTime(2023, 12, 05, 15, 00, 00), dataHoraFimPessArea = new DateTime(2023, 12, 05, 18, 00, 00), IdPessoaPessArea = 1, IdAreaComumPessArea = 1 },
                new PessoaAreaComum() { IdPessArea = 2, dataHoraInicioPessArea = new DateTime(2023, 12, 06, 13, 00, 00), dataHoraFimPessArea = new DateTime(2023, 12, 06, 16, 00, 00), IdPessoaPessArea = 2, IdAreaComumPessArea = 1 },
                new PessoaAreaComum() { IdPessArea = 2, dataHoraInicioPessArea = new DateTime(2023, 12, 16, 18, 00, 00), dataHoraFimPessArea = new DateTime(2023, 12, 16, 21, 00, 00), IdPessoaPessArea = 2, IdAreaComumPessArea = 2 }
            );

            modelBuilder.Entity<Notificacao>().HasData(
                new Notificacao() { IdNotificacao = 1, AssuntoNotificacao = "Manutenção elétrica", MensagemNotificacao = "Haverá manutencão no quadro de força do prédio, dia: 20/12 as 14 horas",  DataEnvioNotificacao = new DateTime(2023, 12, 06, 09, 13, 22)}
            );
            modelBuilder.Entity<PessoaNoti>().HasData(
                new PessoaNoti() { IdPessoaNoti = 1, IdPessoaPessoaNoti = 1, IdNotificacaoPessoaNoti = 1 }
            );

            modelBuilder.Entity<Dependente>().HasData(
                new Dependente() { IdDependente = 1, NomeDependente = "João Gomes", CpfDependente = "11242100083", IdPessoaDependente = 1},
                new Dependente() { IdDependente = 2, NomeDependente = "Maria Silva", CpfDependente = "30777454025", IdPessoaDependente = 1 },
                new Dependente() { IdDependente = 3, NomeDependente = "Carlos Oliveira", CpfDependente = "53086593032", IdPessoaDependente = 2 },
                new Dependente() { IdDependente = 4, NomeDependente = "Ana Souza", CpfDependente = "54710630070", IdPessoaDependente = 3 },
                new Dependente() { IdDependente = 5, NomeDependente = "Pedro Santos", CpfDependente = "03940474002", IdPessoaDependente = 3 }
            );
        }
    }
}