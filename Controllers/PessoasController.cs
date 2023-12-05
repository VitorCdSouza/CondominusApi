using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CondominusApi.Data;
using CondominusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CondominusApi.Utils;

namespace CondominusApi.Controllers
{
    [Authorize(Roles = "Admin, Sindico")]
    [ApiController]
    [Route("[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly DataContext _context;

        public PessoasController(DataContext context)
        {
            _context = context;
        }

        // listagem geral de pessoas
        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Pessoa> lista = await _context.Pessoas
                .Include(d => d.DependentesPessoa)
                .Include(a => a.ApartamentoPessoa)
                .ToListAsync();
                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in lista)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }
                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // listagem de pessoas com tipo morador
        [HttpGet("GetMoradores")]
        public async Task<IActionResult> ListarMoradoresAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas
                .Include(d => d.DependentesPessoa)
                .Include(a => a.ApartamentoPessoa)
                .ToListAsync();
                List<Pessoa> moradores = pessoas.Where(p => p.TipoPessoa == "Morador").ToList();
                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in moradores)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }
                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMoradoresCondominio")]
        public async Task<IActionResult> ListarPorCondominioAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Pessoa> pessoas = await _context.Pessoas
                .Include(a => a.ApartamentoPessoa)
                .ThenInclude(c => c.CondominioApart)
                .Where(p => p.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .Where(p => p.TipoPessoa == "Morador")
                .ToListAsync();

                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in pessoas)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart,
                        IdUsuarioPessoaDTO = p.IdUsuarioPessoa.ToString()
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }

                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // listagem de pessoas com tipo sindico
        [HttpGet("GetSindicos")]
        public async Task<IActionResult> ListarSindicosAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas
                .Include(d => d.DependentesPessoa)
                .Include(a => a.ApartamentoPessoa)
                .ToListAsync();
                List<Pessoa> moradores = pessoas.Where(p => p.TipoPessoa == "Sindico").ToList();
                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in moradores)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }
                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // listagem de pessoas com tipo porteiro
        [HttpGet("GetPorteiros")]
        public async Task<IActionResult> ListarPorteirosAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas
                .Include(d => d.DependentesPessoa)
                .Include(a => a.ApartamentoPessoa)
                .ToListAsync();
                List<Pessoa> moradores = pessoas.Where(p => p.TipoPessoa == "Porteiro").ToList();
                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in moradores)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }
                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> CpfCadastrado(string cpf)
        {
            if (await _context.Pessoas.AnyAsync(x => x.CpfPessoa == cpf))
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Pessoa novoPessoa)
        {
            try
            {
                if (await CpfCadastrado(novoPessoa.CpfPessoa))
                    throw new Exception("Cpf já cadastrado.");

                Apartamento ap = await _context.Apartamentos
                .FirstOrDefaultAsync(x => x.IdApart == novoPessoa.IdApartamentoPessoa);
                novoPessoa.ApartamentoPessoa = ap;

                await _context.Pessoas.AddAsync(novoPessoa);
                await _context.SaveChangesAsync();

                return Ok(novoPessoa.ApartamentoPessoa.NumeroApart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Pessoa p)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoas //Busca pessoa no banco através do Id
                    .FirstOrDefaultAsync(x => x.IdPessoa == p.IdPessoa);

                pessoa.NomePessoa = p.NomePessoa;
                pessoa.TelefonePessoa = p.TelefonePessoa;
                pessoa.CpfPessoa = p.CpfPessoa;
                if (p.IdApartamentoPessoa != 0)
                {
                    pessoa.IdApartamentoPessoa = p.IdApartamentoPessoa;
                }

                var attach = _context.Attach(pessoa);
                attach.Property(x => x.IdPessoa).IsModified = false;
                attach.Property(x => x.NomePessoa).IsModified = true;
                attach.Property(x => x.TelefonePessoa).IsModified = true;
                attach.Property(x => x.TipoPessoa).IsModified = false;
                attach.Property(x => x.CpfPessoa).IsModified = true;
                if (p.IdApartamentoPessoa != 0)
                {
                    attach.Property(x => x.IdApartamentoPessoa).IsModified = true;
                }

                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm)
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Pessoa pRemover = await _context.Pessoas.FirstOrDefaultAsync(p => p.IdPessoa == id);

                _context.Pessoas.Remove(pRemover);
                int linhaAfetadas = await _context.SaveChangesAsync();
                return Ok(linhaAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarMuitos")]
        public async Task<IActionResult> DeletarPessoas([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione pessoas para deletar.");
                }

                // filtrar apenas IDs válidos e existentes no banco de dados
                var pessoasParaDeletar = await _context.Pessoas
                    .Where(u => ids.Contains(u.IdPessoa))
                    .ToListAsync();

                if (pessoasParaDeletar.Count == 0)
                {
                    return NotFound("Nenhuma pessoa encontrada para os IDs fornecidos.");
                }

                _context.Pessoas.RemoveRange(pessoasParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                // após deletar as pessoas, recupere a lista atualizada de usuários
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Pessoa> pessoas = await _context.Pessoas
                .Include(a => a.ApartamentoPessoa)
                .ThenInclude(c => c.CondominioApart)
                .Where(p => p.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .Where(p => p.TipoPessoa == "Morador")
                .ToListAsync();

                List<PessoaDTO> pessoasRetorno = new List<PessoaDTO>();
                foreach (Pessoa p in pessoas)
                {
                    PessoaDTO pessoaDTO = new PessoaDTO
                    {
                        Id = p.IdPessoa,
                        NomePessoaDTO = p.NomePessoa,
                        TelefonePessoaDTO = p.TelefonePessoa,
                        CpfPessoaDTO = p.CpfPessoa,
                        NumeroApartPessoaDTO = p.ApartamentoPessoa.NumeroApart,
                        IdUsuarioPessoaDTO = p.IdUsuarioPessoa.ToString()
                    };
                    pessoasRetorno.Add(pessoaDTO);
                }

                return Ok(pessoasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}