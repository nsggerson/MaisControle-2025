

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppDesktop.Token
{
    /// <summary>
    /// Classe utilitária para manipulação de tokens JWT.
    /// Fornece métodos estáticos para extrair informações de tokens JWT de forma segura.
    /// 
    /// Como utilizar:
    /// 1. Configure sua chave secreta (a mesma usada na geração do token)
    /// 2. Use os métodos estáticos para extrair informações específicas do token
    /// 3. Trate adequadamente as exceções para tokens inválidos ou expirados
    /// 
    /// Exemplo básico:
    /// <code>
    /// const string secretKey = "sua_chave_secreta_aqui";
    /// var token = "seu_token_jwt_aqui";
    /// 
    /// // Obter ID do usuário
    /// var userId = JwtTokenHelper.GetUserId(token, secretKey);
    /// 
    /// // Verificar se token está expirado
    /// if (JwtTokenHelper.IsTokenExpired(token, secretKey))
    /// {
    ///     Console.WriteLine("Token expirado!");
    /// }
    /// </code>
    /// 
    /// Requisitos:
    /// - Token deve ser um JWT válido
    /// - Chave secreta deve corresponder à usada na geração
    /// - System.IdentityModel.Tokens.Jwt (pacote NuGet)
    /// </summary>
    public static class JwtTokenHelper
    {
        private static readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        /// <summary>
        /// Extrai todas as claims do token JWT
        /// </summary>
        public static ClaimsPrincipal GetClaimsPrincipal(string token, string secretKey)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return _tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        /// <summary>
        /// Obtém o ID do usuário do token
        /// </summary>
        public static string GetUserId(string token, string secretKey)
        {
            var claims = GetClaimsPrincipal(token, secretKey);
            return claims.FindFirst("ID")?.Value!;
        }

        /// <summary>
        /// Obtém o email do usuário do token
        /// </summary>
        public static string GetUserEmail(string token, string secretKey)
        {
            var claims = GetClaimsPrincipal(token, secretKey);
            return claims.FindFirst("Email")?.Value!;
        }

        /// <summary>
        /// Obtém o primeiro nome do usuário do token
        /// </summary>
        public static string GetFirstName(string token, string secretKey)
        {
            var claims = GetClaimsPrincipal(token, secretKey);
            return claims.FindFirst("FirstName")?.Value!;
        }

        /// <summary>
        /// Obtém o documento (CPF) do usuário do token
        /// </summary>
        public static string GetDocument(string token, string secretKey)
        {
            var claims = GetClaimsPrincipal(token, secretKey);
            return claims.FindFirst("Document")?.Value!;
        }

        /// <summary>
        /// Obtém a data de expiração do token
        /// </summary>
        public static DateTime GetExpirationDate(string token, string secretKey)
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

        /// <summary>
        /// Verifica se o token está expirado
        /// </summary>
        public static bool IsTokenExpired(string token, string secretKey)
        {
            return GetExpirationDate(token, secretKey) < DateTime.UtcNow;
        }

        /// <summary>
        /// Obtém todas as claims como um dicionário
        /// </summary>
        public static Dictionary<string, string> GetAllClaims(string token, string secretKey)
        {
            var claims = GetClaimsPrincipal(token, secretKey);
            return claims.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
