using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfacesServicos;
using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servicos
{
    public class CategoriaServico(InterfaceCategoria _interfaceCategoria) : ICategoriaServico
    {
        
        public async Task AdicionarCartegoria(Categoria categoria)
        {
            var valido = categoria.ValidarPropriedadeString(categoria.CategoriaNome!, "Nome");
            if (valido)
                await _interfaceCategoria.Add(categoria);
        }

        public async Task AlterarCartegoria(Categoria categoria)
        {
            var valido = categoria.ValidarPropriedadeString(categoria.CategoriaNome!, "Nome");
            if (valido)
                await _interfaceCategoria.Update(categoria);
        }
    }
}
