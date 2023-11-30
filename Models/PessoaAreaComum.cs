using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class PessoaAreaComum
    {
        public int IdPessArea { get; set; }
        public DateTime dataHoraPessArea { get; set; }
        public Pessoa PessoaPessArea { get; set; }
        public int IdPessoaPessArea { get; set; }
        public AreaComum AreaComumPessArea { get; set; }
        public int IdAreaComumPessArea { get; set; }
    }
}