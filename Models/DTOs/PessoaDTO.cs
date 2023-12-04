using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class PessoaDTO
    {
        public int IdPessoaDTO { get; set; }
        public string NomePessoaDTO { get; set; }
        public string TelefonePessoaDTO { get; set; }
        public string CpfPessoaDTO { get; set; }
        public string NumeroApartPessoaDTO { get; set; }
    }
}

