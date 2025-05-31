using System.IdentityModel.Tokens.Jwt;

namespace AppDesktop.Token
{
    /// <summary>
    /// Representa um token JWT (JSON Web Token) e fornece acesso facilitado
    /// às suas propriedades e à sua representação em string.
    /// </summary>
    /// <remarks>
    /// Esta classe recebe uma instância de <see cref="JwtSecurityToken"/> no seu construtor,
    /// expondo algumas de suas informações mais relevantes de forma direta.
    /// Ela simplifica a obtenção da data de validade do token e a sua representação
    /// completa como uma string, pronta para ser utilizada em cabeçalhos de autorização
    /// ou em outros contextos onde o token JWT é necessário.
    /// </remarks>
    public class TokenJWT
    {
        private readonly JwtSecurityToken _token;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TokenJWT"/>.
        /// </summary>
        /// <param name="token">A instância do <see cref="JwtSecurityToken"/> a ser encapsulada.</param>
        public TokenJWT(JwtSecurityToken token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Obtém a data e hora em que o token JWT expira e não será mais válido.
        /// </summary>
        public DateTime ValidTo => _token.ValidTo;

        /// <summary>
        /// Obtém a representação em string completa do token JWT.
        /// </summary>
        public string Token => new JwtSecurityTokenHandler().WriteToken(_token);
    }
}
