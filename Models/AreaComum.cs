using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class AreaComum
    {
        public int IdAreaComum { get; set; }
        public string NomeAreaComum { get; set; }
        public List<PessoaAreaComum> PessoaACAreaComum { get; set; } 
    }
}