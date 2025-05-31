using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioUsuarioSistemaFinanceiro : RepositoryGenerics<UsuarioSistemaFinanceiro>, InterfaceUsuarioSistemaFinanceiro
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositorioUsuarioSistemaFinanceiro()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<IList<UsuarioSistemaFinanceiro>> ListarUsuarioSistemaFinanceiro(int IdSistema)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return  await banco.UsuarioSistemaFinanceiro
                    .Where(u => u.SistemaID == IdSistema)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<UsuarioSistemaFinanceiro> ObterUsuarioPorEmail(string emailUsuario)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await banco.UsuarioSistemaFinanceiro
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.EmailUsuario!.Equals(emailUsuario)) ?? null!;
            }
        }

        public async Task RemoveUsuarioSistemaFinanceiro(List<UsuarioSistemaFinanceiro> usuarios)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                banco.UsuarioSistemaFinanceiro
                     .RemoveRange(usuarios);

                await banco.SaveChangesAsync();
                //await banco.UsuarioSistemaFinanceiro
                //    .Where(u => usuarios.Any(uu => uu.UsuarioSistemaFinanceiroID == u.UsuarioSistemaFinanceiroID))
                //    .ExecuteDeleteAsync();
            }
        }
    }
}
