using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CondominusApi.Models;
using CondominusApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CondominusApi.Utils;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace CondominusApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public UsuariosController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string CriarToken(Usuario usuario, string numeroCond)
        {
            List<Claim> claims = new List<Claim> // informacoes que aparecerao no token
            {
                new Claim(ClaimTypes.Actor, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.EmailUsuario),
                new Claim(ClaimTypes.Role, usuario.PessoaUsuario.TipoPessoa),
                new Claim(JwtRegisteredClaimNames.Sub, numeroCond)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration
            .GetSection("ConfiguracaoToken:Chave").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Usuario> lista = await _context.Usuarios.Include(r => r.PessoaUsuario).ToListAsync();
                List<Usuario> usuariosMoradores = await _context.Usuarios.Where(u => u.PessoaUsuario.TipoPessoa == "Morador").ToListAsync();
                List<UsuarioDTO> usuariosRetorno = new List<UsuarioDTO>();
                foreach (Usuario u in usuariosMoradores)
                {
                    UsuarioDTO usuarioDTO = new UsuarioDTO
                    {
                        Id = u.IdUsuario,
                        EmailUsuarioDTO = u.EmailUsuario,
                        DataAcessoUsuarioDTO = u.DataAcessoUsuario,
                        NomeUsuarioDTO = u.PessoaUsuario.NomePessoa
                    };
                    usuariosRetorno.Add(usuarioDTO);
                }
                return Ok(usuariosRetorno);
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
                List<Usuario> usuariosMoradores = await _context.Usuarios
                .Include(x => x.PessoaUsuario)
                .ThenInclude(x => x.ApartamentoPessoa)
                .ThenInclude(x => x.CondominioApart)
                .Where(u => u.PessoaUsuario.TipoPessoa == "Morador")
                .Where(x => x.PessoaUsuario.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                .ToListAsync();

                List<UsuarioDTO> usuariosRetorno = new List<UsuarioDTO>();
                foreach (Usuario u in usuariosMoradores)
                {
                    UsuarioDTO usuarioDTO = new UsuarioDTO
                    {
                        Id = u.IdUsuario,
                        NomeUsuarioDTO = u.PessoaUsuario.NomePessoa,
                        EmailUsuarioDTO = u.EmailUsuario,
                        DataAcessoUsuarioDTO = u.DataAcessoUsuario
                    };
                    usuariosRetorno.Add(usuarioDTO);
                }

                return Ok(usuariosRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> UsuarioExistente(string email)
        {
            if (await _context.Usuarios.AnyAsync(x => x.EmailUsuario.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }
        private async Task<bool> UsuarioPessoaExistente(string cpf)
        {
            Pessoa pessoa = await _context.Pessoas
                    .FirstOrDefaultAsync(x => x.CpfPessoa == cpf);
            if (pessoa.UsuarioPessoa == null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> PessoaPreviamenteCadastrada(string cpf)
        {
            if (await _context.Pessoas.AnyAsync(x => x.CpfPessoa == cpf))
            {
                return true;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromQuery] string cpf, [FromBody] Usuario usuario)
        {
            try
            {
                if (await UsuarioExistente(usuario.EmailUsuario))
                    throw new Exception("E-mail já cadastrado.");
                if (await UsuarioPessoaExistente(cpf))
                    throw new Exception("Pessoa já possui cadastro.");
                if (!await PessoaPreviamenteCadastrada(cpf))
                    throw new Exception("Pessoa não cadastrada pelo síndico.");

                Pessoa pessoa = await _context.Pessoas
                    .FirstOrDefaultAsync(x => x.CpfPessoa == cpf);

                Criptografia.CriarPasswordHash(usuario.SenhaUsuario, out byte[] hash, out byte[] salt);
                usuario.SenhaUsuario = string.Empty;
                usuario.PasswordHashUsuario = hash;
                usuario.PasswordSaltUsuario = salt;

                usuario.IdPessoaUsuario = pessoa.IdPessoa;
                usuario.PessoaUsuario = pessoa;


                await _context.Usuarios.AddAsync(usuario);

                pessoa.IdUsuarioPessoa = usuario.IdUsuario;

                _context.Attach(pessoa).Property(x => x.IdUsuarioPessoa).IsModified = true;

                await _context.SaveChangesAsync();

                return Ok(usuario.IdUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                .Include(r => r.PessoaUsuario).ThenInclude(a => a.ApartamentoPessoa)
                .ThenInclude(c => c.CondominioApart)
                .FirstOrDefaultAsync(x => x.EmailUsuario == credenciais.EmailUsuario);

                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");
                else if (!Criptografia.VerificarPasswordHash(credenciais.SenhaUsuario, usuario.PasswordHashUsuario, usuario.PasswordSaltUsuario))
                    throw new Exception("Senha incorreta.");
                else
                {
                    usuario.DataAcessoUsuario = DateTime.Now;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    usuario.PasswordHashUsuario = null;
                    usuario.PasswordSaltUsuario = null;
                    usuario.TokenUsuario = CriarToken(usuario, usuario.PessoaUsuario.ApartamentoPessoa.CondominioApart.IdCond.ToString());

                    return Ok(usuario);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarMuitos")]
        public async Task<IActionResult> DeletarUsuarios([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione usuários para deletar.");
                }

                var usuariosParaDeletar = await _context.Usuarios
                    .Where(u => ids.Contains(u.IdUsuario))
                    .ToListAsync();

                if (usuariosParaDeletar.Count == 0)
                {
                    return NotFound("Nenhum usuário encontrado para os IDs fornecidos.");
                }

                _context.Usuarios.RemoveRange(usuariosParaDeletar);

                int linhasAfetadas = await _context.SaveChangesAsync();

                string token = HttpContext.Request.Headers["Authorization"].ToString();
                string idCondominioToken = Criptografia.ObterIdCondominioDoToken(token.Remove(0, 7));
                List<Usuario> usuariosMoradores = await _context.Usuarios
                                .Include(x => x.PessoaUsuario)
                                .ThenInclude(x => x.ApartamentoPessoa)
                                .ThenInclude(x => x.CondominioApart)
                                .Where(u => u.PessoaUsuario.TipoPessoa == "Morador")
                                .Where(x => x.PessoaUsuario.ApartamentoPessoa.CondominioApart.IdCond.ToString() == idCondominioToken)
                                .ToListAsync();

                List<UsuarioDTO> usuariosRetorno = new List<UsuarioDTO>();
                foreach (Usuario u in usuariosMoradores)
                {
                    UsuarioDTO usuarioDTO = new UsuarioDTO
                    {
                        Id = u.IdUsuario,
                        NomeUsuarioDTO = u.PessoaUsuario.NomePessoa,
                        EmailUsuarioDTO = u.EmailUsuario,
                        DataAcessoUsuarioDTO = u.DataAcessoUsuario
                    };
                    usuariosRetorno.Add(usuarioDTO);
                }


                return Ok(usuariosRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AtualizarUsuario")]
        public async Task<IActionResult> AtualizarUsuario(Usuario u)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.IdUsuario == u.IdUsuario);

                usuario.EmailUsuario = u.EmailUsuario;

                var attach = _context.Attach(usuario);
                attach.Property(x => x.IdUsuario).IsModified = false;
                attach.Property(x => x.EmailUsuario).IsModified = true;
                int linhasAfetadas = await _context.SaveChangesAsync(); // confirma a alteração no banco
                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}