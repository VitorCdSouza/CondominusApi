using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
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
                List<Apartamento> apartamentos = await _context.Apartamentos
                .Include(c => c.CondominioApart)
                .Where(ap => ap.IdCondominioApart.ToString() == idCondominioToken)
                .ToListAsync();
                
                List<ApartamentoDTO> apartamentosRetorno = new List<ApartamentoDTO>();
                foreach (Apartamento x in apartamentos){
                    ApartamentoDTO apartamentoDTO = new ApartamentoDTO{
                        IdApartamentoDTO = x.IdApart,
                        NumeroApartamentoDTO = x.NumeroApart,
                        TelefoneApartamentoDTO = x.TelefoneApart
                    };
                    apartamentosRetorno.Add(apartamentoDTO);
                }

                return Ok(apartamentosRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Apartamento novoApartamento)
        {
            try
            {
                Condominio condominio = await _context.Condominios
                .FirstOrDefaultAsync(cond => cond.IdCond == novoApartamento.IdCondominioApart);

                novoApartamento.CondominioApart = condominio;

                await _context.Apartamentos.AddAsync(novoApartamento);
                await _context.SaveChangesAsync();

                return Ok(novoApartamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}