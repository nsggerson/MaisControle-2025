using Domain.Interfaces.InterfacesServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servicos
{
    public class UsuarioSistemaFinanceiroServico(InterfaceUsuarioSistemaFinanceiro _interfaceUsuarioSistemaFinanceiro) : IUsuarioSistemaFinanceiroServico
    {
        public async Task AdicionarUsuarioSistemaFinanceiro(UsuarioSistemaFinanceiro usuarioSistemaFinanceiro)
        {
            await _interfaceUsuarioSistemaFinanceiro.Add(usuarioSistemaFinanceiro);
        }

        public Task AlterarUsuarioSistemaFinanceiro(UsuarioSistemaFinanceiro usuarioSistemaFinanceiro)
        {
            throw new NotImplementedException();
        }
    }
}
