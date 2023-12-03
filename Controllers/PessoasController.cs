using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CondominusApi.Data;
using CondominusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
                List<Pessoa> pessoas = await _context.Pessoas.ToListAsync();
                return Ok(pessoas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // listagem geral de pessoas com dependentes
        [HttpGet("GetAllDependentes")]
        public async Task<IActionResult> ListarDepAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas
                    .Include(p => p.DependentesPessoa).ToListAsync();
                return Ok(pessoas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //listagem de pessoas com tipo morador
        [HttpGet("GetMoradores")]
        public async Task<IActionResult> ListarMoradoresAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas.ToListAsync();           
                List<Pessoa> moradores = pessoas.Where(p => p.TipoPessoa == "Morador").ToList();                
                return Ok(moradores);     
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //listagem de pessoas com tipo sindico
        [HttpGet("GetSindicos")]
        public async Task<IActionResult> ListarSindicosAsync()
        {
            try
            {
                List<Pessoa> pessoas = await _context.Pessoas.ToListAsync();           
                List<Pessoa> sindicos = pessoas.Where(p => p.TipoPessoa == "Sindico").ToList();                
                return Ok(sindicos);     
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Pessoa novoPessoa)
        {
            try
            {
                await _context.Pessoas.AddAsync(novoPessoa);
                await _context.SaveChangesAsync();

                return Ok(novoPessoa.IdPessoa);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("AtualizarPessoa")]
        public async Task<IActionResult> Update(Pessoa p)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoas //Busca pessoa no banco através do Id
                    .FirstOrDefaultAsync(x => x.IdPessoa == p.IdPessoa);

                pessoa.NomePessoa = p.NomePessoa;
                pessoa.TelefonePessoa = p.TelefonePessoa;
                pessoa.CpfPessoa = p.CpfPessoa;
                pessoa.IdApartamentoPessoa = p.IdApartamentoPessoa;

                var attach = _context.Attach(pessoa);
                attach.Property(x => x.IdPessoa).IsModified = false;
                attach.Property(x => x.NomePessoa).IsModified = true;
                attach.Property(x => x.TelefonePessoa).IsModified = true;
                attach.Property(x => x.CpfPessoa).IsModified = true;
                attach.Property(x => x.IdApartamentoPessoa).IsModified = true;
                
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
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}