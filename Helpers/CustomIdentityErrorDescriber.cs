using Microsoft.AspNetCore.Identity;

namespace Helpers;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length)
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.PasswordTooShort).ToString(),
            Description = $"A senha deve ter pelo menos {length} caracteres."
        };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.PasswordRequiresNonAlphanumeric).ToString(),
            Description = "A senha deve conter ao menos um caractere não alfanumérico."
        };

    public override IdentityError PasswordRequiresDigit()
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.PasswordRequiresDigit).ToString(),
            Description = "A senha deve conter ao menos um dígito ('0'-'9')."
        };

    public override IdentityError PasswordRequiresLower()
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.PasswordRequiresLower).ToString(),
            Description = "A senha deve conter ao menos uma letra minúscula ('a'-'z')."
        };

    public override IdentityError PasswordRequiresUpper()
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.PasswordRequiresUpper).ToString(),
            Description = "A senha deve conter ao menos uma letra maiúscula ('A'-'Z')."
        };

    public override IdentityError DuplicateUserName(string userName)
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.DuplicateUserName).ToString(),
            Description = $"O nome de usuário '{userName}' já está em uso."
        };

    public override IdentityError DuplicateEmail(string email)
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.DuplicateEmail).ToString(),
            Description = $"O email '{email}' já está em uso."
        };

    public override IdentityError InvalidUserName(string? userName)
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.InvalidUserName).ToString(),
            Description = $"O nome de usuário '{userName}' é inválido."
        };

    public override IdentityError InvalidEmail(string? email)
        => new IdentityError
        {
            Code = ((int)IdentityErrorCode.InvalidEmail).ToString(),
            Description = $"O email '{email}' é inválido."
        };
}
