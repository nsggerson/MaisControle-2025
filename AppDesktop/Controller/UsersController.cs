using AppDesktop.Dto;
using AppDesktop.Token;
using Entities.Entidades;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AppDesktop.Controller
{
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager= userManager;
            _signInManager = signInManager;
        }

        [Post("AdicionarUsuario")]
        public async Task<IResult> AdicionarUsuarioAsync(Login input)
        {
            try
            {
                // Validação do modelo
                if (!TryValidate(input, out var validationErrors))
                    return Result.Fail(statusCode: StatusCodes.Status400BadRequest, errors: validationErrors);

                // Verificar se o email já está cadastrado
                var existingUser = await _userManager.FindByEmailAsync(input.Email);
                if (existingUser != null)
                    return Result.Fail(
                        statusCode: StatusCodes.Status409Conflict,
                        errors: new List<string> { "Este email já está cadastrado." }
                    );

                // Verificar se o CPF já está cadastrado
                var existingCpfUser = _userManager.Users.FirstOrDefault(u => u.CPF == input.Cpf);
                if (existingCpfUser != null)
                    return Result.Fail(
                        statusCode: StatusCodes.Status409Conflict,
                        errors: new List<string> { "Este CPF já está cadastrado." }
                    );

                // Criação do usuário
                var user = new ApplicationUser
                {
                    Name = input.Name,
                    NormalizedName = input.NormalizedName,
                    FirstName = input.FirstName,
                    NormalizedFirstName = input.NormalizedFirstName,
                    UserName = input.Email,
                    NormalizedUserName = input.Email?.ToUpperInvariant(),
                    PhoneNumber = input.PhoneNumber,
                    Email = input.Email,
                    NormalizedEmail = input.Email?.ToUpperInvariant(),
                    CPF = input.Cpf,
                    EmailConfirmed = true // Ou false, dependendo da sua lógica de confirmação
                };

                // Criação do usuário com senha
                var result = await _userManager.CreateAsync(user, input.Password);

                if (!result.Succeeded)
                {
                    // Tratamento dos erros de criação
                    var errorDescriptions = result.Errors.Select(e => e.Description).ToList();
                    var statusCode = result.Errors.Any(e => e.Code == "DuplicateUserName")
                        ? StatusCodes.Status409Conflict
                        : StatusCodes.Status400BadRequest;

                    return Result.Fail(
                        statusCode: statusCode,
                        errors: errorDescriptions
                    );
                }

                // Opcional: Adicionar o usuário a uma role padrão
                // await _userManager.AddToRoleAsync(user, "BasicUser");

                return Result.Ok(
                    statusCode: StatusCodes.Status201Created,
                    message: "Cadastro realizado com sucesso!"
                );
            }
            catch (Exception ex)
            {
                // Logar o erro (implemente seu sistema de logging)
                //_logger.LogError(ex, "Erro ao cadastrar usuário");
                Console.WriteLine(ex);
                return Result.Fail(
                    statusCode: StatusCodes.Status500InternalServerError,
                    errors: new List<string> { "Ocorreu um erro inesperado ao processar seu cadastro. Por favor, tente novamente." }
                );
            }
        }
    }
}
