using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppDesktop.Token
{
    /// <summary>
    /// Fornece um método estático para criar uma chave de segurança simétrica (SymmetricSecurityKey)
    /// a partir de uma string secreta. Essa chave é comumente usada para assinar e verificar tokens JWT (JSON Web Tokens).
    /// </summary>
    /// <remarks>
    /// A segurança dos tokens JWT depende fortemente da força e do sigilo da chave secreta utilizada.
    /// É crucial que a string secreta seja longa o suficiente para o algoritmo de criptografia desejado (por exemplo,
    /// pelo menos 16 bytes para HMACSHA256 e 32 bytes para HMACSHA512) e mantida em segurança para evitar
    /// acesso não autorizado e falsificação de tokens.
    /// </remarks>
    public class JwtSecurityKey
    {
        /// <summary>
        /// Cria uma instância de SymmetricSecurityKey a partir da string secreta fornecida,
        /// codificando a string para bytes usando a codificação UTF-8.
        /// </summary>
        /// <param name="secret">A string secreta usada para criar a chave de segurança.</param>
        /// <returns>Uma nova instância de SymmetricSecurityKey.</returns>
        public static SymmetricSecurityKey Create(string secret)
        {
            // Certifique-se de que a chave tenha pelo menos 16 bytes de comprimento para HMACSHA256
            // e 32 bytes de comprimento para HMACSHA512
            // Por exemplo: "seu-segredo-de-256-bits" tem 32 bytes de comprimento
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}
