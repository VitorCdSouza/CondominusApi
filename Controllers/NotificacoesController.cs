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
    public class NotificacoesController : ControllerBase
    {
        private readonly DataContext _context; 

        public NotificacoesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Notificacao> notificacaos = await _context
                .Notificacoes
                .ToListAsync();                
                return Ok(notificacaos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFeedCondominio")]
        public async Task<IActionResult> ListarPorFeedCondominioAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                var notificacoes = _context.Condominios
                    .Where(c => c.IdCond.ToString() == idCondominioToken)
                    .SelectMany(c => c.ApartamentosCond)
                    .SelectMany(a => a.PessoasApart)
                    .Where(p => p.TipoPessoa == "Morador")
                    .SelectMany(p => p.PessoaNotiPessoa)
                    .Select(pn => pn.NotificacaoPessoaNoti)
                    .Distinct()
                    .ToList();
                
                return Ok(notificacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAvisosCondominio")]
        public async Task<IActionResult> ListarPorAvisosCondominioAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                var notificacoes = _context.Condominios
                    .Where(c => c.IdCond.ToString() == idCondominioToken)
                    .SelectMany(c => c.ApartamentosCond)
                    .SelectMany(a => a.PessoasApart)
                    .Where(p => p.TipoPessoa == "Sindico")
                    .SelectMany(p => p.PessoaNotiPessoa)
                    .Select(pn => pn.NotificacaoPessoaNoti)
                    .Distinct()
                    .ToList();
                
                return Ok(notificacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}