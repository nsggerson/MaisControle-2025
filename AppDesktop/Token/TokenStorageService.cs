using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace AppDesktop.Token;
public class TokenStorageService
{
    private const string TokenKey = "auth_token";

    public async Task SetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(token));
        }

        // Valida se o token é um JWT válido antes de armazenar
        if (!TryParseJwt(token, out _))
        {
            throw new SecurityTokenException("Invalid JWT token format");
        }

        await SecureStorage.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public async Task<DateTime?> GetExpirationAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        if (TryParseJwt(token, out var jwtToken))
        {
            return jwtToken!.ValidTo;
        }

        return null;
    }

    public async Task<bool> IsSessionValidAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        return await Task.Run(() =>
        {
            try
            {
                if (TryParseJwt(token, out var jwtToken))
                {
                    // Buffer de segurança de 5 minutos
                    var safetyBuffer = TimeSpan.FromMinutes(5);
                    return jwtToken?.ValidTo > DateTime.UtcNow.Add(safetyBuffer);
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log opcional (remova se não for necessário)
                Console.WriteLine($"Erro ao validar token: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> IsSessionValidAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;

        if (TryParseJwt(token, out var jwtToken))
        {
            // Adiciona um buffer de segurança (ex: 5 minutos) para evitar usar tokens que estão prestes a expirar
            return jwtToken!.ValidTo > DateTime.UtcNow.AddMinutes(5);
        }

        return false;
    }

    public async Task RefreshTokenAsync(string newToken)
    {
        if (string.IsNullOrWhiteSpace(newToken))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(newToken));
        }

        if (!TryParseJwt(newToken, out _))
        {
            throw new SecurityTokenException("Invalid JWT token format");
        }

        await SetTokenAsync(newToken);
    }

    public void Logout()
    {
        SecureStorage.Remove(TokenKey);
    }

    private bool TryParseJwt(string token, out JwtSecurityToken? jwtToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            jwtToken = handler.ReadJwtToken(token);
            return true;
        }
        catch
        {
            jwtToken = null;
            return false;
        }
    }
}
