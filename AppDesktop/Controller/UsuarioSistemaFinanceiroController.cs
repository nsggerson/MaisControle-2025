using Domain.Interfaces.InterfacesServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Domain.Servicos;
using Entities.Entidades;
using Helpers;

namespace AppDesktop.Controller
{
    public class UsuarioSistemaFinanceiroController(InterfaceUsuarioSistemaFinanceiro _InterfaceUsuarioSistemaFinanceiro, IUsuarioSistemaFinanceiroServico _IUsuarioSistemaFinanceiroServico) : BaseController
    {
        [Get("ListarUsuarioSistemaFinanceiro")]
        public async Task<IResult> ListarUsuarioSistemaFinanceiroAsync(int id)
        {
            try
            {
                var result = await _InterfaceUsuarioSistemaFinanceiro.ListarUsuarioSistemaFinanceiro(id);

                return Result.Ok(result);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Post("AdicionarUsuarioSistemaFinanceiro")]
        public async Task<IResult> AdicionarUsuarioSistemaFinanceiroAsync(int sistemaId, string emailUsuario)
        {
            try
            {
                await _IUsuarioSistemaFinanceiroServico.AdicionarUsuarioSistemaFinanceiro(
                new UsuarioSistemaFinanceiro{
                    SistemaID = sistemaId,
                    EmailUsuario = emailUsuario,
                    Administrador = false,
                    SistemaAtual = true
                });

                return Result.Ok(Task.FromResult("Usuário sistema financeiro cadastrado com sucesso."));
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Delete("DeleteUsuarioSistemaFinanceiro")]
        public async Task<IResult> DeleteUsuarioSistemaFinanceiroAsync(int id)
        {
            try
            {
                var result = await _InterfaceUsuarioSistemaFinanceiro.GetEntityById(id);
                await _InterfaceUsuarioSistemaFinanceiro.Delete(result);
                return Result.Ok($"Usuário excluído do sistema {result.SistemaFinanceiro!.SistemaFinanceiroNome} com sucesso.");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }               

        [Put("AlterarUsuarioSistemaFinanceiro")]
        public async Task<IResult> AlterarUsuarioSistemaFinanceiroAsync(UsuarioSistemaFinanceiro usuarioSistemaFinanceiro)
        {
            try
            {
                if (!TryValidate(usuarioSistemaFinanceiro, out var errors))
                    return Result.Fail(errors);

                await _IUsuarioSistemaFinanceiroServico.AlterarUsuarioSistemaFinanceiro(usuarioSistemaFinanceiro);

                return Result.Ok(Task.FromResult(usuarioSistemaFinanceiro));
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}
