using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Entrega
    {
        [Key]
        public int IdEnt { get; set; }
        public string DestinatarioEnt { get; set; }
        public string CodEnt { get; set; }
        public DateTime? DataEntregaEnt { get; set; }
        public DateTime? DataRetiradaEnt { get; set; }
        public Apartamento ApartamentoEnt { get; set; }
        public int IdApartamentoEnt { get; set; }      
    }
}