using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class PessoaAreaComum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPessArea { get; set; }
        public DateTime dataHoraInicioPessArea { get; set; } // data e hora do inicio de area comum
        public DateTime dataHoraFimPessArea { get; set; } // data e hora do fim de area comum
        public Pessoa PessoaPessArea { get; set; }
        public int IdPessoaPessArea { get; set; }
        public AreaComum AreaComumPessArea { get; set; }
        public int IdAreaComumPessArea { get; set; }
    }
}