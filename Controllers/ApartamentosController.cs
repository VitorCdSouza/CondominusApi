using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using CondominusApi.Data;
using CondominusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CondominusApi.Controllers
{
    [Authorize(Roles = "Admin, Sindico")]
    [ApiController]
    [Route("[controller]")]
    public class ApartamentosController : ControllerBase
    {
        private readonly DataContext _context; 

        public ApartamentosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Apartamento> apartamentos = await _context.Apartamentos.ToListAsync();                
                return Ok(apartamentos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Apartamento novoApartamento)
        {
            try
            {
                await _context.Apartamentos.AddAsync(novoApartamento);
                await _context.SaveChangesAsync();

                return Ok(novoApartamento.IdApart);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}