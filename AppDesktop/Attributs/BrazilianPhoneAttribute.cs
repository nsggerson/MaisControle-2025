using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppDesktop.Attributs
{
    public class BrazilianPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Telefone é obrigatório");
            }

            string phoneNumber = value.ToString()?? null!;

            // Remove todos os caracteres não numéricos
            string digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

            // Verifica se tem o tamanho mínimo (10 dígitos para fixo, 11 para celular)
            if (digitsOnly.Length < 10 || digitsOnly.Length > 11)
            {
                return new ValidationResult("Telefone deve ter 10 ou 11 dígitos");
            }

            // Formata o telefone
            string formattedPhone;
            if (digitsOnly.Length == 10) // Telefone fixo
            {
                formattedPhone = $"({digitsOnly.Substring(0, 2)}) {digitsOnly.Substring(2, 4)}-{digitsOnly.Substring(6)}";
            }
            else // Telefone celular (11 dígitos)
            {
                formattedPhone = $"({digitsOnly.Substring(0, 2)}) {digitsOnly.Substring(2, 5)}-{digitsOnly.Substring(7)}";
            }

            // Atualiza o valor do campo com o telefone formatado
            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName!);
            if (property != null)
            {
                property.SetValue(validationContext.ObjectInstance, formattedPhone, null);
            }

            return ValidationResult.Success!;
        }
    }
}
