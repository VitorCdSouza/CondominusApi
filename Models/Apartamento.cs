using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Apartamento
    {
        public int IdApart { get; set; }
        public string TelefoneApart { get; set; }
        public string NumeroApart { get; set; }
        public Condominio CondominioApart { get; set; }
        public int IdCondominioApart { get; set; }
        public List<Pessoa> PessoasApart { get; set; }
        public List<Entrega> EntregasApart { get; set; }
    }
}