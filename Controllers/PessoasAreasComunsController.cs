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
    public class PessoasAreasComunsController : ControllerBase
    {
        private readonly DataContext _context; 

        public PessoasAreasComunsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<PessoaAreaComum> pessoasAreasComuns = await _context
                .PessoasAreasComuns
                .Include(p => p.PessoaPessArea)
                .Include(ac => ac.AreaComumPessArea)
                .ToListAsync();                
                return Ok(pessoasAreasComuns);
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
                List<PessoaAreaComum> pessoasAreasComuns = await _context.PessoasAreasComuns
                .Include(x => x.AreaComumPessArea)
                .Include(x => x.PessoaPessArea)
                .ThenInclude(x => x.ApartamentoPessoa)
                .ThenInclude(x => x.CondominioApart)
                .Where(x => x.PessoaPessArea.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();
                
                List<PessoaAreaComumDTO> pessoasAreasComunsRetorno = new List<PessoaAreaComumDTO>();
                foreach (PessoaAreaComum x in pessoasAreasComuns){
                    PessoaAreaComumDTO pessoaAreaComumDTO = new PessoaAreaComumDTO{
                        IdPessAreaDTO = x.IdPessoaPessArea,
                        NomeAreaPessAreaDTO = x.AreaComumPessArea.NomeAreaComum,   
                        dataHoraInicioPessAreaDTO = x.dataHoraInicioPessArea,
                        dataHoraFimPessAreaDTO = x.dataHoraFimPessArea,
                        NomePessoaDTO = x.PessoaPessArea.NomePessoa
                    };
                    pessoasAreasComunsRetorno.Add(pessoaAreaComumDTO);
                }

                return Ok(pessoasAreasComunsRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PessoaAreaComum novaReserva)
        {
            try
            {
                AreaComum ac = await _context.AreasComuns
                    .FirstOrDefaultAsync(x => x.IdAreaComum == novaReserva.IdAreaComumPessArea);

                Pessoa p = await _context.Pessoas
                    .FirstOrDefaultAsync(x => x.IdPessoa == novaReserva.IdPessoaPessArea);

                novaReserva.AreaComumPessArea = ac;
                novaReserva.PessoaPessArea = p;
                await _context.PessoasAreasComuns.AddAsync(novaReserva);
                await _context.SaveChangesAsync();

                return Ok(novaReserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(PessoaAreaComum reservaAtualizada)
        {
            try
            {
                PessoaAreaComum reserva = await _context.PessoasAreasComuns.FindAsync(reservaAtualizada.IdPessoaPessArea);

                if (reserva == null)
                    return NotFound("Reserva não encontrada");

                // mudar para poder alterar as duas datas: inicial e/ou final
                reserva.dataHoraInicioPessArea = reservaAtualizada.dataHoraInicioPessArea;
                reserva.dataHoraFimPessArea = reservaAtualizada.dataHoraFimPessArea;

                _context.PessoasAreasComuns.Update(reserva);
                await _context.SaveChangesAsync();

                return Ok(reserva.IdPessArea);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarMuitos")]
        public async Task<IActionResult> DeleteReservas([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione reservas para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var reservasParaDeletar = await _context.PessoasAreasComuns
                    .Where(u => ids.Contains(u.IdPessArea))
                    .ToListAsync();

                if (reservasParaDeletar.Count == 0)
                {
                    return NotFound("Nenhuma reserva encontrada para os IDs fornecidos.");
                }

                _context.PessoasAreasComuns.RemoveRange(reservasParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                // Após deletar as reservas, recupere a lista atualizada de reservas
                var listaAtualizada = await _context.PessoasAreasComuns.ToListAsync();
                return Ok(listaAtualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}