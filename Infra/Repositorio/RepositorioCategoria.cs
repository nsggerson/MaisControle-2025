using Domain.Interfaces.Generics;
using Domain.Interfaces.ICategoria;
using Domain.Interfaces.IDespesa;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorio
{
    public class RepositorioCategoria : RepositoryGenerics<Categoria>, InterfaceCategoria
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositorioCategoria()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<IList<Categoria>> ListarCategoriaUsuario(string emailUsuario)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await (
                        from s in banco.SistemaFinanceiro
                        join c in banco.Categoria on s.SistemaFinanceiroID equals c.SistemaID
                        join us in banco.UsuarioSistemaFinanceiro on s.SistemaFinanceiroID equals us.SistemaID
                        where us.EmailUsuario!.Equals(emailUsuario) && us.SistemaAtual
                        select c
                    ).AsNoTracking().ToListAsync();
            }
        }
    }
}
