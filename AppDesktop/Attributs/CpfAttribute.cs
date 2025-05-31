using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDesktop.Attributs
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var cpf = value as string;
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            // Aqui faria a validação real do CPF
            return cpf.Length == 11;
        }
    }
}
