using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class AreaComum
    {
        [Key]
        public int IdAreaComum { get; set; }
        public string NomeAreaComum { get; set; }
        public List<PessoaAreaComum> PessoaACAreaComum { get; set; } 
    }
}