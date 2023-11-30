using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class PessoaNoti
    {
        public int IdPessoaNoti { get; set; }
        public Pessoa PessoaPessoaNoti { get; set; }
        public int IdPessoaPessoaNoti { get; set; }
        public Notificacao NotificacaoPessoaNoti { get; set; }
        public int IdNotificacaoPessoaNoti { get; set; }
    }
}