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
                foreach (Apartamento x in apartamentos)
                {
                    ApartamentoDTO apartamentoDTO = new ApartamentoDTO
                    {
                        Id = x.IdApart,
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
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                int idCondominio = Int32.Parse(idCondominioToken);
                Condominio condominio = await _context.Condominios
                .FirstOrDefaultAsync(cond => cond.IdCond == idCondominio);

                novoApartamento.IdCondominioApart = idCondominio;
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

        [HttpPut]
        public async Task<IActionResult> Update(Apartamento apartamento)
        {
            try
            {
                Apartamento ap = await _context.Apartamentos
                    .FirstOrDefaultAsync(x => x.IdApart == apartamento.IdApart);

                if (ap != null)
                {
                    ap.NumeroApart = apartamento.NumeroApart;
                    ap.TelefoneApart = apartamento.TelefoneApart;

                    _context.Apartamentos.Update(ap);
                    await _context.SaveChangesAsync();
                    return Ok(ap.IdApart);
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
        public async Task<IActionResult> DeleteApartamentos([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione apartamentos para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var apartamentosParaDeletar = await _context.Apartamentos
                    .Where(u => ids.Contains(u.IdApart))
                    .ToListAsync();

                if (apartamentosParaDeletar.Count == 0)
                {
                    return NotFound("Nenhum apartamento encontrado para os IDs fornecidos.");
                }

                _context.Apartamentos.RemoveRange(apartamentosParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                // Após deletar as áreas comuns, recupere a lista atualizada
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Apartamento> apartamentos = await _context.Apartamentos
                .Include(c => c.CondominioApart)
                .Where(ap => ap.IdCondominioApart.ToString() == idCondominioToken)
                .ToListAsync();

                List<ApartamentoDTO> apartamentosRetorno = new List<ApartamentoDTO>();
                foreach (Apartamento x in apartamentos)
                {
                    ApartamentoDTO apartamentoDTO = new ApartamentoDTO
                    {
                        Id = x.IdApart,
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
    }
}