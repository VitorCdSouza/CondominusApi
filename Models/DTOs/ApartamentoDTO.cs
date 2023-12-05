using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class ApartamentoDTO
    {
        public int Id { get; set; }
        public string NumeroApartamentoDTO { get; set; }
        public string TelefoneApartamentoDTO { get; set; }
    }
}

