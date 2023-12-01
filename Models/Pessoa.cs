using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondominusApi.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        public string TelefonePessoa { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfPessoa { get; set; }
        public Apartamento ApartamentoPessoa { get; set;}
        public int IdApartamentoPessoa { get; set;}
        public Usuario UsuarioPessoa { get; set; }
        public int IdUsuarioPessoa { get; set; }
        public List<Dependente> DependentesPessoa { get; set; }
        public List<PessoaAreaComum> PessoaACPessoa { get; set; }
        public List<PessoaNoti> PessoaNotiPessoa { get; set; }
    }
}