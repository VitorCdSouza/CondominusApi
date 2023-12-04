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
                foreach (Dependente u in lista){
                    DependenteDTO dependenteDTO = new DependenteDTO{
                        IdDependenteDTO = u.IdDependente,
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
    }
}