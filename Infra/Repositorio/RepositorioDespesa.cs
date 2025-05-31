using Domain.Interfaces.Generics;
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
    public class RepositorioDespesa : RepositoryGenerics<Despesa>, InterfaceDespesa
    {

        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositorioDespesa()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<IList<Despesa>> ListarDespesasNaoPagasMesAnterior(string emailUsuario)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.SistemaFinanceiroID equals c.SistemaID
                    join us in banco.UsuarioSistemaFinanceiro on c.SistemaID equals us.SistemaID
                    join d in banco.Despesa on c.CategoriaID equals d.CategoriaID
                    where us.EmailUsuario!.Equals(emailUsuario) && d.Mes < DateTime.Now.Month && !d.Pago
                    select d
                ).AsNoTracking().ToListAsync();
            }
        }

        public async Task<IList<Despesa>> ListarDespesasUsuario(string emailUsuario)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.SistemaFinanceiroID equals c.SistemaID
                    join us in banco.UsuarioSistemaFinanceiro on s.SistemaFinanceiroID equals us.SistemaID
                    join d in banco.Despesa on c.CategoriaID equals d.CategoriaID
                    where us.EmailUsuario!.Equals(emailUsuario) && s.Mes == d.Mes && s.Ano == d.Ano
                    select d).AsNoTracking().ToListAsync();
            }
        }
    }
}
