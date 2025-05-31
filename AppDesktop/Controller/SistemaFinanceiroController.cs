using Domain.Interfaces.InterfacesServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Helpers;

namespace AppDesktop.Controller
{
    public class SistemaFinanceiroController(InterfaceSistemaFinanceiro _interfaceSistemaFinanceiro, ISistemaFinanceiroServico _iSistemaFinanceiroServico) : BaseController
    {
   
        [Get("ListarSistemasUsuario")]
        public async Task<IResult> ListarSistemasUsuarioAsync(string email)
        {
            try
            {
                var result = await _interfaceSistemaFinanceiro.ListarSistemasUsuario(email);

                return Result.Ok(result);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Post("AdicionarSistemaFinanceiro")]
        public async Task<IResult> AdicionarSistemaFinanceiroAsync(SistemaFinanceiro sistemaFinanceiro)
        {
            try
            {
                if (!TryValidate(sistemaFinanceiro, out var errors))
                    return Result.Fail(errors);

                await _iSistemaFinanceiroServico.AdicionarSistemaFinanceiro(sistemaFinanceiro);

                return Result.Ok(Task.FromResult(sistemaFinanceiro));
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Put("AdicionarSistemaFinanceiro")]
        public async Task<IResult> AlterarSistemaFinanceiroAsync(SistemaFinanceiro sistemaFinanceiro)
        {
            try
            {
                if (!TryValidate(sistemaFinanceiro, out var errors))
                    return Result.Fail(errors);

                await _iSistemaFinanceiroServico.AlterarSistemaFinanceiro(sistemaFinanceiro);

                return Result.Ok(Task.FromResult(sistemaFinanceiro));
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Get("ObterSistemaFinaceiro")]
        public async Task<IResult> ObterSistemaFinaceiroAsync(int id)
        {
            try
            {
               var result= await _interfaceSistemaFinanceiro.GetEntityById(id);

                return Result.Ok(result);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        [Delete("DeleteSistemaFinaceiro")]
        public async Task<IResult> DeleteSistemaFinaceiroAsync(int id)
        {
            try
            {
                var result = await _interfaceSistemaFinanceiro.GetEntityById(id);
                await _interfaceSistemaFinanceiro.Delete(result);
                return Result.Ok($"Sistema {result.SistemaFinanceiroNome} excluído com sucesso.");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}
