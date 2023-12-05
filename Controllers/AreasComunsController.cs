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
    public class AreasComunsController : ControllerBase
    {
        private readonly DataContext _context;

        public AreasComunsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<AreaComum> areasComuns = await _context.AreasComuns.ToListAsync();
                return Ok(areasComuns);
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
                var areasComuns = await _context.AreasComuns
                .Where(c => c.IdCondominioAreaComum == idCondominioToken)
                .ToListAsync();

                List<AreaComumDTO> areasRetorno = new List<AreaComumDTO>();
                foreach (AreaComum x in areasComuns)
                {
                    AreaComumDTO areaDTO = new AreaComumDTO
                    {
                        Id = x.IdAreaComum,
                        NomeAreaComumDTO = x.NomeAreaComum
                    };
                    areasRetorno.Add(areaDTO);
                }

                return Ok(areasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AreaComum novaAreaComum)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                novaAreaComum.IdCondominioAreaComum = idCondominioToken;

                await _context.AreasComuns.AddAsync(novaAreaComum);
                await _context.SaveChangesAsync();

                return Ok(novaAreaComum.IdAreaComum);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(AreaComum areaComum)
        {
            try
            {
                AreaComum areaComumBuscada = await _context.AreasComuns
                .FirstOrDefaultAsync(x => x.IdAreaComum == areaComum.IdAreaComum);

                if (areaComumBuscada == null)
                {
                    return BadRequest("Área comum não encontrada");
                }

                areaComumBuscada.NomeAreaComum = areaComum.NomeAreaComum;

                _context.AreasComuns.Update(areaComumBuscada);
                await _context.SaveChangesAsync();

                return Ok(areaComumBuscada);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarMuitos")]
        public async Task<IActionResult> DeleteAreasComuns([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione áreas comuns para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var areasParaDeletar = await _context.AreasComuns
                    .Where(u => ids.Contains(u.IdAreaComum))
                    .ToListAsync();

                if (areasParaDeletar.Count == 0)
                {
                    return NotFound("Nenhuma área comum encontrada para os IDs fornecidos.");
                }

                _context.AreasComuns.RemoveRange(areasParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                var areasComuns = await _context.AreasComuns
                .Where(c => c.IdCondominioAreaComum == idCondominioToken)
                .ToListAsync();

                List<AreaComumDTO> areasRetorno = new List<AreaComumDTO>();
                foreach (AreaComum x in areasComuns)
                {
                    AreaComumDTO areaDTO = new AreaComumDTO
                    {
                        Id = x.IdAreaComum,
                        NomeAreaComumDTO = x.NomeAreaComum
                    };
                    areasRetorno.Add(areaDTO);
                }

                return Ok(areasRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}