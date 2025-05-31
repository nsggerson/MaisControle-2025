
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace AppDesktop.Dto;

public class InputModel
{
    [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string? Password { get; set; }
    //public string ConfirmPassword { get; set; }
    //public string Nome { get; set; }
    //public string Telefone { get; set; }
    //public string Cpf { get; set; }
    //public string Cnpj { get; set; }
    //public string RazaoSocial { get; set; }
    //public string NomeFantasia { get; set; }
    //public string Endereco { get; set; }
    //public string Cidade { get; set; }
    //public string Estado { get; set; }
    //public string Cep { get; set; }
    //public DateTime DataNascimento { get; set; }
}
