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
    public class DependentesController : ControllerBase
    {
        private readonly DataContext _context;

        public DependentesController(DataContext context)
        {
            _context = context;
        }

        // listagem geral de pessoas
        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Dependente> lista = await _context.Dependentes.Include(r => r.PessoaDependente).ToListAsync();
                List<DependenteDTO> dependenteRetorno = new List<DependenteDTO>();
                foreach (Dependente u in lista)
                {
                    DependenteDTO dependenteDTO = new DependenteDTO
                    {
                        Id = u.IdDependente,
                        NomeDependenteDTO = u.NomeDependente,
                        CpfDependenteDTO = u.CpfDependente,
                        NomePessoaDependenteDTO = u.PessoaDependente.NomePessoa
                    };
                    dependenteRetorno.Add(dependenteDTO);
                }
                return Ok(dependenteRetorno);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCondominio")]
        public async Task<IActionResult> ListarPorCondominioAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                string idUsuarioToken = Criptografia.ObterIdUsuarioDoToken(token.Remove(0, 7));

                List<Dependente> dependentes = await _context.Dependentes
                .Include(x => x.PessoaDependente)
                .ThenInclude(x => x.ApartamentoPessoa)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.PessoaDependente.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();

                List<DependenteDTO> dependentesRetorno = new List<DependenteDTO>();
                foreach (Dependente x in dependentes)
                {
                    DependenteDTO dependenteDTO = new DependenteDTO
                    {
                        Id = x.IdDependente,
                        NomeDependenteDTO = x.NomeDependente,
                        CpfDependenteDTO = x.CpfDependente,
                        NomePessoaDependenteDTO = x.PessoaDependente.NomePessoa
                    };
                    dependentesRetorno.Add(dependenteDTO);
                }

                return Ok(dependentesRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllDepCondominio")]
        public async Task<IActionResult> ListarPorDepCondominioAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Dependente> dependentes = await _context.Dependentes
                .Include(x => x.PessoaDependente)
                .ThenInclude(x => x.ApartamentoPessoa)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.PessoaDependente.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();

                List<DependenteDTO> dependentesRetorno = new List<DependenteDTO>();
                foreach (Dependente x in dependentes)
                {
                    DependenteDTO dependenteDTO = new DependenteDTO
                    {
                        Id = x.IdDependente,
                        NomeDependenteDTO = x.NomeDependente,
                        CpfDependenteDTO = x.CpfDependente,
                        NomePessoaDependenteDTO = x.PessoaDependente.NomePessoa
                    };
                    dependentesRetorno.Add(dependenteDTO);
                }

                return Ok(dependentesRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Dependente novoDependente)
        {
            try
            {
                Pessoa temp = await _context.Pessoas.FirstOrDefaultAsync(p => p.IdPessoa == novoDependente.IdPessoaDependente);
                novoDependente.PessoaDependente = temp;

                await _context.Dependentes.AddAsync(novoDependente);
                await _context.SaveChangesAsync();

                return Ok(novoDependente.PessoaDependente.NomePessoa);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Dependente dependente)
        {
            try
            {
                Dependente dp = await _context.Dependentes
                    .FirstOrDefaultAsync(x => x.IdDependente == dependente.IdDependente);

                if (dp != null)
                {
                    if (dependente.NomeDependente != null)
                    {
                        dp.NomeDependente = dependente.NomeDependente;
                    }
                    if (dependente.CpfDependente != null)
                    {
                        dp.CpfDependente = dependente.CpfDependente;
                    }

                    _context.Dependentes.Update(dp);
                    await _context.SaveChangesAsync();
                    return Ok(dp.IdDependente);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarMuitos")]
        public async Task<IActionResult> DeleteDependentes([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione dependentes para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var dependentesParaDeletar = await _context.Dependentes
                    .Where(u => ids.Contains(u.IdDependente))
                    .ToListAsync();

                if (dependentesParaDeletar.Count == 0)
                {
                    return NotFound("Nenhum dependente encontrado para os IDs fornecidos.");
                }

                _context.Dependentes.RemoveRange(dependentesParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                // Após deletar as áreas comuns, recupere a lista atualizada
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Dependente> dependentes = await _context.Dependentes
                .Include(x => x.PessoaDependente)
                .ThenInclude(x => x.ApartamentoPessoa)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.PessoaDependente.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();

                List<DependenteDTO> dependentesRetorno = new List<DependenteDTO>();
                foreach (Dependente x in dependentes)
                {
                    DependenteDTO dependenteDTO = new DependenteDTO
                    {
                        Id = x.IdDependente,
                        NomeDependenteDTO = x.NomeDependente,
                        CpfDependenteDTO = x.CpfDependente,
                        NomePessoaDependenteDTO = x.PessoaDependente.NomePessoa
                    };
                    dependentesRetorno.Add(dependenteDTO);
                }

                return Ok(dependentesRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}