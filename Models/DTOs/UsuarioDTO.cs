using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class UsuarioDTO
    {
        public int IdUsuarioDTO { get; set; }
        public string NomeUsuarioDTO { get; set; }
        public string EmailUsuarioDTO { get; set; }
        public DateTime? DataAcessoUsuarioDTO{ get; set; }
    }
}

