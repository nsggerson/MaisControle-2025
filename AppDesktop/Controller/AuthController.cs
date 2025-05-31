using AppDesktop.Dto;
using AppDesktop.Token;
using Entities.Entidades;
using Helpers;
using Microsoft.AspNetCore.Identity;

namespace AppDesktop.Controller
{
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Post("Login")]
        public async Task<IResult> LoginAsync(InputModel input)
        {
            try
            {
                if (!TryValidate(input, out var errors)) return Result.Fail(errors);

                if (await _userManager.FindByEmailAsync(input.Username!) is not { } user || !await _userManager.CheckPasswordAsync(user, input.Password!))
                {
                    return Result.Fail(message: "Usuário ou senha inválidos", statusCode: 401, error:"");
                    //return Result.Fail("Usuário ou senha inválidos", 401);
                }

                var result = await _userManager.FindByEmailAsync(input.Username!);

                //var token2 = new TokenJWTBuilder()
                //    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-mQ7fzr4k3T9wYyB7vH2qP1lA5sJ8dF0gX6cN4bV1xZ9pL2oM3eK5iU8hR7tG0j"))
                //    .AddSubject("Sistema de controle")
                //    .AddIssuer("Teste.Securiry.Bearer")
                //    .AddAudience("Teste.Security.Bearer")
                //    .AddClaim("UsuarioDoskTopMobile", "1")
                //    .AddExpiry(10)
                //    .Builder();
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-mQ7fzr4k3T9wYyB7vH2qP1lA5sJ8dF0gX6cN4bV1xZ9pL2oM3eK5iU8hR7tG0j"))
                    .AddSubject("Sistema de controle")
                    .AddIssuer("Teste.Securiry.Bearer")
                    .AddAudience("Teste.Security.Bearer")
                    .AddClaim("ID", result?.Id!) // ID do usuário
                    .AddClaim("Email", result?.Email!) // Email do usuário
                    .AddClaim("FirstName", result?.NormalizedFirstName!) // Primeiro nome
                    .AddClaim("Document", result?.CPF!) // Documento (CPF, RG, etc.)
                    .AddExpiry(10) // 10 minutos de expiração
                    .Builder();

                return Result.Ok(value:token.Token, message:"Login realizado com sucesso!");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }

        }
    }
}
