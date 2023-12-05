using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class DependenteDTO
    {
        public int Id { get; set; }
        public string NomeDependenteDTO { get; set; }
        public string CpfDependenteDTO { get; set; }
        public string NomePessoaDependenteDTO { get; set; }
    }
}

