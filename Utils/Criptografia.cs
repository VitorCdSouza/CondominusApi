using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Utils
{
    public class Criptografia
    {
         public static void CriarPasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerificarPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static string ObterIdCondominioDoToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var IdCond = jsonToken?.Subject;

            return IdCond;
        }

        public static string ObterIdUsuarioDoToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var IdUser = jsonToken?.Actor;

            return IdUser;
        }


    }
}