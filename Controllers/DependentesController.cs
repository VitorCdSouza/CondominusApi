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

        //listagem geral de pessoas
        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Dependente> dependentes = await _context.Dependentes.ToListAsync();
                return Ok(dependentes);
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
                // Pessoa temp = await _context.Pessoas.FirstOrDefaultAsync(p => p.Id.Equals(novoDependente.IdPessoa));
                // temp.Dependentes.Add(novoDependente);
                // _context.Pessoas.Update(temp);

                await _context.Dependentes.AddAsync(novoDependente);
                await _context.SaveChangesAsync();

                return Ok(novoDependente.IdDependente);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}