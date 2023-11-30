using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Condominio
    {
        public int IdCond { get; set; }
        public string NomeCond { get; set; }
        public string EnderecoCond { get; set; }
        public List<Apartamento> ApartamentosCond { set; get; }
    }
}