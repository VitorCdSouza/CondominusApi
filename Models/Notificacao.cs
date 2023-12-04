using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Notificacao
    {
        [Key]
        public int IdNotificacao { get; set; }
        public string AssuntoNotificacao { get; set; }
        public string MensagemNotificacao { get; set; }
        public DateTime DataEnvioNotificacao { get; set; }
        public List<PessoaNoti> PessoasNotificacoes { get; set; }
        public int IdPessoaNotificacao { get; set; }
    }
}