using AppDesktop.Attributs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDesktop.Dto;

public class Login
{
    private string _name = string.Empty;
    private string _firstName = string.Empty;
   
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value?.Trim() ?? null!;
            // Atualiza o primeiro nome automaticamente quando o nome completo é definido
            if (!string.IsNullOrEmpty(_name))
            {
                _firstName = ExtractFirstName(_name);
            }
        }
    }

    public string NormalizedName => _name?.ToUpperInvariant() ?? null!;

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value?.Trim() ?? null!; // Permite sobrescrever se necessário
    }

    public string NormalizedFirstName => _firstName?.ToUpperInvariant() ?? null!;

    //[Required(ErrorMessage = "Informe um nome de usuário válido")]
    //public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; set; } = string.Empty;
    [CpfAttribute]
    [Required(ErrorMessage = "CPF é obrigatório")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [BrazilianPhone(ErrorMessage = "Telefone inválido. O formato deve ser (XX) XXXX-XXXX ou (XX) XXXXX-XXXX")]
    public string PhoneNumber { get; set; } = string.Empty;
    // Método auxiliar para extrair o primeiro nome
    private string ExtractFirstName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        // Remove espaços extras e divide por espaços
        var names = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Retorna o primeiro nome (primeira parte do array)
        return names.Length > 0 ? names[0] : string.Empty;
    }
    //public Login()
    //{
    //    Email = "mariana.fernandes@exemplo.com.br";
    //    Cpf = "98765432109"; // CPF válido
    //    Password = "Senha@Segura2024";
    //    ConfirmPassword = "Senha@Segura2024";
    //    PhoneNumber = "(31) 99876-5432"; // DDD de Minas Gerais
    //}
}
