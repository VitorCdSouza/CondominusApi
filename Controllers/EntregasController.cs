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
                foreach (Entrega x in entregas)
                {
                    EntregaDTO entregaDTO = new EntregaDTO
                    {
                        Id = x.IdEnt,
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

        [HttpGet("GetAllCondominioMorador")]
        public async Task<IActionResult> ListarPorMoradorAsync()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                string idUsuarioToken = Criptografia.ObterIdUsuarioDoToken(token.Remove(0, 7));

                Pessoa pessoa = await _context.Pessoas
                .Include(u => u.UsuarioPessoa).FirstOrDefaultAsync(x => x.UsuarioPessoa.IdUsuario.ToString() == idUsuarioToken);

                List<Entrega> entregas = await _context.Entregas
                .Include(x => x.ApartamentoEnt)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.ApartamentoEnt.CondominioApart.IdCond.ToString() == idCondominioToken)
                .Where(x => x.ApartamentoEnt.PessoasApart.Any(x => x.IdApartamentoPessoa == pessoa.IdApartamentoPessoa))
                .ToListAsync();

                List<EntregaDTO> entregasRetorno = new List<EntregaDTO>();
                foreach (Entrega x in entregas)
                {
                    EntregaDTO entregaDTO = new EntregaDTO
                    {
                        Id = x.IdEnt,
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

        [HttpPut]
        public async Task<IActionResult> Update(Entrega entrega)
        {
            try
            {
                Entrega et = await _context.Entregas
                    .FirstOrDefaultAsync(x => x.IdEnt == entrega.IdEnt);

                if (et != null)
                {
                    if (entrega.IdApartamentoEnt != 0)
                    {
                        et.IdApartamentoEnt = entrega.IdApartamentoEnt;
                    }
                    if (entrega.DestinatarioEnt != null)
                    {
                        et.DestinatarioEnt = entrega.DestinatarioEnt;
                    }
                    if (entrega.DataRetiradaEnt != null)
                    {
                        et.DataRetiradaEnt = entrega.DataRetiradaEnt;
                    }

                    _context.Entregas.Update(et);
                    await _context.SaveChangesAsync();
                    return Ok(et.IdEnt);
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
        public async Task<IActionResult> DeleteEntregas([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione entregas para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var entregasParaDeletar = await _context.Entregas
                    .Where(u => ids.Contains(u.IdEnt))
                    .ToListAsync();

                if (entregasParaDeletar.Count == 0)
                {
                    return NotFound("Nenhuma entrega encontrada para os IDs fornecidos.");
                }

                _context.Entregas.RemoveRange(entregasParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                // Após deletar as áreas comuns, recupere a lista atualizada
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                string idUsuarioToken = Criptografia.ObterIdUsuarioDoToken(token.Remove(0, 7));

                Pessoa pessoa = await _context.Pessoas
                .Include(u => u.UsuarioPessoa).FirstOrDefaultAsync(x => x.UsuarioPessoa.IdUsuario.ToString() == idUsuarioToken);

                List<Entrega> entregas = await _context.Entregas
                .Include(x => x.ApartamentoEnt)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.ApartamentoEnt.CondominioApart.IdCond.ToString() == idCondominioToken)
                .Where(x => x.ApartamentoEnt.PessoasApart.Any(x => x.IdPessoa == pessoa.IdPessoa))
                .ToListAsync();

                List<EntregaDTO> entregasRetorno = new List<EntregaDTO>();
                foreach (Entrega x in entregas)
                {
                    EntregaDTO entregaDTO = new EntregaDTO
                    {
                        Id = x.IdEnt,
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
    }
}