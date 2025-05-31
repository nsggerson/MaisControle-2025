using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Notificacoes
{
    [NotMapped]
    public class Notifica
    {
        public string? NomePropriedade { get; set; }

        public string? mensagem { get; set; }

        public List<Notifica> notificacoes { get; }

        public Notifica()
        {
            notificacoes = new List<Notifica>();
        }

        public bool ValidarPropriedadeString(string valor, string nomePropriedade)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                notificacoes.Add(new Notifica
                {
                    mensagem = "O campo não pode ser nulo ou em branco.",
                    NomePropriedade = nomePropriedade,
                });
                return false;
            }
           return true;
        }
        public bool ValidarPropriedadeString(int valor, string nomePropriedade)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                notificacoes.Add(new Notifica
                {
                    mensagem = "O campo não pode ser nulo ou menor que 1.",
                    NomePropriedade = nomePropriedade,
                });
                return false;
            }
            return true;
        }
    }
}
