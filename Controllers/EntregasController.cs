using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using CondominusApi.Data;
using CondominusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CondominusApi.Utils;

namespace CondominusApi.Controllers
{
    [Authorize(Roles = "Admin, Sindico, Morador")]
    [ApiController]
    [Route("[controller]")]
    public class EntregasController : ControllerBase
    {
        private readonly DataContext _context; 

        public EntregasController(DataContext context)
        {
            _context = context;
        }  

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Entrega> entregas = await _context.Entregas.ToListAsync();                
                return Ok(entregas);
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
                List<Entrega> entregas = await _context.Entregas
                .Include(x => x.ApartamentoEnt)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.ApartamentoEnt.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();
                
                List<EntregaDTO> entregasRetorno = new List<EntregaDTO>();
                foreach (Entrega x in entregas){
                    EntregaDTO entregaDTO = new EntregaDTO{
                        IdEntDTO = x.IdEnt,
                        DestinatarioEntDTO = x.DestinatarioEnt,
                        NumeroApartDTO = x.ApartamentoEnt.NumeroApart,
                        DataEntregaEntDTO = x.DataEntregaEnt,
                        DataRetiradaEntDTO = x.DataRetiradaEnt
                    };
                    entregasRetorno.Add(entregaDTO);
                }

                return Ok(entregasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Entrega novaEntrega)
        {
            try
            {
                await _context.Entregas.AddAsync(novaEntrega);
                await _context.SaveChangesAsync();

                return Ok(novaEntrega.IdEnt);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}