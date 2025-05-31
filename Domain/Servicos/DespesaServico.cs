using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfacesServicos;
using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servicos
{
    public class DespesaServico(InterfaceDespesa _interfaceDespesa) : IDespesaServico
    {
        public async Task AdicionarDespesa(Despesa despesa)
        {
            var data = DateTime.UtcNow;
            despesa.DataCadastro = data;
            despesa.Ano = data.Year;
            despesa.Mes = data.Month;

            var valido = despesa.ValidarPropriedadeString(despesa.DespesaNome!, "Nome");
            if (valido)
                await _interfaceDespesa.Add(despesa);
        }

        public async Task AlterarDespesa(Despesa despesa)
        {
            var data = DateTime.UtcNow;
            despesa.DataAtualizacao = data;

            if (despesa.Pago)
            {
                despesa.DataPagamento = data;
                //despesa.DataVencimento = data;
            }

            var valido = despesa.ValidarPropriedadeString(despesa.DespesaNome!, "Nome");
            if (valido)
              await  _interfaceDespesa.Update(despesa);            
        }
    }
}
