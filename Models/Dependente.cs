using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Dependente
    {
        public int IdDependente { get; set; }
        public string NomeDependente { get; set; }
        public string CpfDependente { get; set; }
        public Pessoa PessoaDependente { get; set; }
        public int IdPessoaDependente { get; set; }
    }
}