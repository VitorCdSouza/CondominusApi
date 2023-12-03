using Microsoft.AspNetCore.Mvc;
using CondominusApi.Data;
using CondominusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CondominusApi.Controllers
{
    [Authorize(Roles = "Admin, Sindico")]
    [ApiController]
    [Route("[controller]")]
    public class CondominiosController : ControllerBase
    {
        private readonly DataContext _context; 

        public CondominiosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Condominio> condominios = await _context.Condominios.ToListAsync();                
                return Ok(condominios);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Condominio novoCondominio)
        {
            try
            {
                await _context.Condominios.AddAsync(novoCondominio);
                await _context.SaveChangesAsync();

                return Ok(novoCondominio.IdCond);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}