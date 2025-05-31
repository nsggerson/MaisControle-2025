using Domain.Interfaces.InterfacesServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servicos
{
    public class SistemaFinanceiroServico(InterfaceSistemaFinanceiro _interfaceSistemaFinanceiro) : ISistemaFinanceiroServico
    {
        public async Task AdicionarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
        {
            var valido = sistemaFinanceiro.ValidarPropriedadeString(sistemaFinanceiro.SistemaFinanceiroNome!, "Nome");

            if (valido)
            {
                var data = DateTime.Now;

                sistemaFinanceiro.DiaFechamento = 1;
                sistemaFinanceiro.Mes = data.Month;
                sistemaFinanceiro.Ano = data.Year;
                sistemaFinanceiro.AnoCopia = data.Year;
                sistemaFinanceiro.MesCopia = data.Month;
                sistemaFinanceiro.GerarCopiaDespesas = true;

                await _interfaceSistemaFinanceiro.Add(sistemaFinanceiro);
            }                
        }

        public async Task AlterarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
        {
            var valido = sistemaFinanceiro.ValidarPropriedadeString(sistemaFinanceiro.SistemaFinanceiroNome!, "Nome");

            if (valido)
            {
                sistemaFinanceiro.DiaFechamento = 1;                
                await _interfaceSistemaFinanceiro.Add(sistemaFinanceiro);
            }
        }
    }
}
