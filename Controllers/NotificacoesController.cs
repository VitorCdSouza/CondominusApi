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
                List<Notificacao> feedback = await _context.Notificacoes
                    .Where(c => c.IdCondominioNotificacao == idCondominioToken)
                    .Where(p => p.TipoNotificacao == "Feedback")
                    .ToListAsync();

                List<NotificacaoDTO> notificacoesRetorno = new List<NotificacaoDTO>();
                foreach (Notificacao x in feedback)
                {
                    NotificacaoDTO feedDTO = new NotificacaoDTO
                    {
                        Id = x.IdNotificacao,
                        AssuntoNotificacaoDTO = x.AssuntoNotificacao,
                        MensagemNotificacaoDTO = x.MensagemNotificacao,
                        DataEnvioNotificacaoDTO = x.DataEnvioNotificacao
                    };
                    notificacoesRetorno.Add(feedDTO);
                }

                return Ok(notificacoesRetorno);
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
                List<Notificacao> avisos = await _context.Notificacoes
                    .Where(c => c.IdCondominioNotificacao == idCondominioToken)
                    .Where(p => p.TipoNotificacao == "Aviso")
                    .ToListAsync();

                List<NotificacaoDTO> notificacoesRetorno = new List<NotificacaoDTO>();
                foreach (Notificacao x in avisos)
                {
                    NotificacaoDTO feedDTO = new NotificacaoDTO
                    {
                        Id = x.IdNotificacao,
                        AssuntoNotificacaoDTO = x.AssuntoNotificacao,
                        MensagemNotificacaoDTO = x.MensagemNotificacao,
                        DataEnvioNotificacaoDTO = x.DataEnvioNotificacao
                    };
                    notificacoesRetorno.Add(feedDTO);
                }

                return Ok(notificacoesRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Notificacao novaNotificacao)
        {
            try
            {

                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                novaNotificacao.IdCondominioNotificacao = idCondominioToken;

                await _context.Notificacoes.AddAsync(novaNotificacao);
                await _context.SaveChangesAsync();

                return Ok(novaNotificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}