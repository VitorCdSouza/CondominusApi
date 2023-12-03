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

        private string CriarToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.EmailUsuario),
                new Claim(ClaimTypes.Role, usuario.PessoaUsuario.TipoPessoa)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration
            .GetSection("ConfiguracaoToken:Chave").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = null,
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> Get(Usuario u)
        {
            try
            {
                Usuario uRetornado = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Nome == u.Nome && u.Email == u.Email);

                if (uRetornado == null)
                    throw new Exception("Usuário não encontrado");

                return Ok(uRetornado);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Usuario> lista = await _context.Usuarios.ToListAsync();
                List<Usuario> usuariosMoradores = await _context.Usuarios.Where(u => u.Perfil == "Morador").ToListAsync();
                List<UsuarioDTO> usuariosRetorno = new List<UsuarioDTO>();
                foreach (Usuario u in usuariosMoradores){
                    UsuarioDTO usuarioDTO = new UsuarioDTO{
                        Id = u.Id,
                        Nome = u.Nome,
                        Telefone = u.Telefone,
                        Cpf = u.Cpf,
                        Perfil = u.Perfil,
                        Email = u.Email,
                        DataAcesso = u.DataAcesso,
                        Apartamento = u.Apartamento,
                        IdApartamento = u.IdApartamento
                    };
                    usuariosRetorno.Add(usuarioDTO);
                }
                return Ok(usuariosRetorno);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> UsuarioExistente(string username)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
        private async Task<bool> ApartamentoExistente(int ap)
        {
            if (await _context.Usuarios.AnyAsync(x => x.IdApartamento == ap))
            {
                return true;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario user)
        {
            try
            {
                if (await UsuarioExistente(user.Email))
                    throw new System.Exception("E-mail já cadastrado.");
                if (await ApartamentoExistente(user.IdApartamento))
                    throw new System.Exception("Apartamento já cadastrado.");

                Apartamento ap = await _context.Apartamentos 
                    .FirstOrDefaultAsync(x => x.Id == user.IdApartamento);
                user.Apartamento = ap;
                
                Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                await _context.Usuarios.AddAsync(user);
                
                Pessoa pessoa = new Pessoa
                {
                    Nome = user.Nome,
                    Perfil = user.Perfil,
                    Telefone = user.Telefone,
                    Cpf = user.Cpf,
                    Apartamento = ap,
                    IdApartamento = user.IdApartamento
                };
                await _context.Pessoas.AddAsync(pessoa);
                await _context.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /* eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJVc3VhcmlvQWRtaW4iLCJuYmYiOjE2OTMyMjQ0NjAsImV4cCI6MTY5MzMxMDg2MCwiaWF0IjoxNjkzMjI0NDYwfQ.tfzKDR22gQ7K60A5Sj0hf5_v9spygsgfGQ4mPeLYxTqYRpKRPeijMtGjGMrI9LhOgAoHdLlFDbABWjdzB2uq0Q*/
        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                   .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(credenciais.Email.ToLower()));

                if (usuario == null)
                    throw new System.Exception("Usuário não encontrado.");
                else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                    throw new System.Exception("Senha incorreta.");
                else
                {
                    usuario.DataAcesso = System.DateTime.Now;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync(); //Confirma a alteração no banco

                    usuario.PasswordHash = null;
                    usuario.PasswordSalt = null;
                    usuario.Token = CriarToken(usuario);

                    return Ok(usuario);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetUsuario(int usuarioId)
        {
            try
            {
                //List exigirá o using System.Collections.Generic
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == usuarioId);
                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [HttpPost("DeletarMuitos")]
        // public async Task<IActionResult> DeletarUsuarios([FromBody] int[] ids)
        // {
        //     try
        //     {
        //         if (ids.Length < 1)
        //             throw new Exception("Selecione usuarios");

        //         foreach (int i in ids) {
        //             Usuario usuarioParaDeletar = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == i);
        //             _context.Usuarios.Remove(usuarioParaDeletar);
        //         }
                
        //         int linhaAfetadas = await _context.SaveChangesAsync();
        //         return Ok(linhaAfetadas);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

        [HttpPost("DeletarMuitos")]
        public async Task<IActionResult> DeletarUsuarios([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("Selecione usuários para deletar.");
                }

                // Filtrar apenas IDs válidos e existentes no banco de dados
                var usuariosParaDeletar = await _context.Usuarios
                    .Where(u => ids.Contains(u.Id))
                    .ToListAsync();

                if (usuariosParaDeletar.Count == 0)
                {
                    return NotFound("Nenhum usuário encontrado para os IDs fornecidos.");
                }

                _context.Usuarios.RemoveRange(usuariosParaDeletar);
                
                int linhasAfetadas = await _context.SaveChangesAsync();
                
                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para alteração do e-mail
        [HttpPut("AtualizarUsuario")]
        public async Task<IActionResult> AtualizarUsuario(Usuario u)
        {
            try
            {
                if (await ApartamentoExistente(u.IdApartamento))
                    throw new System.Exception("Apartamento já cadastrado.");

                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == u.Id);
                Pessoa pessoa = await _context.Pessoas //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == u.Id);
                Apartamento ap = await _context.Apartamentos 
                    .FirstOrDefaultAsync(x => x.Id == u.IdApartamento);

                usuario.Nome = u.Nome;
                usuario.Telefone = u.Telefone;
                usuario.Cpf = u.Cpf;
                usuario.Apartamento = ap;
                usuario.IdApartamento = u.IdApartamento;
                usuario.Email = u.Email;

                pessoa.Telefone = u.Telefone;
                pessoa.Nome = u.Nome;
                pessoa.Cpf = u.Cpf;
                pessoa.Apartamento = ap;
                pessoa.IdApartamento = u.IdApartamento;

                var attach = _context.Attach(usuario);
                attach.Property(x => x.Id).IsModified = false;
                attach.Property(x => x.Email).IsModified = true;
                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm)
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}