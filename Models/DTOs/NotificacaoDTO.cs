using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class NotificacaoDTO
    {
        public int Id { get; set; }
        public string AssuntoNotificacaoDTO { get; set; }
        public string MensagemNotificacaoDTO { get; set; }
        public DateTime? DataEnvioNotificacaoDTO { get; set; }
    }
}

