using System.Diagnostics;

namespace AppDesktop.Token
{
    using Microsoft.Maui.Storage;
    using System.Diagnostics;

    public class TokenService
    {
        private const string TokenKey = "auth_jwt_token";
        private const string WindowsTokenKey = "windows_jwt_token";

        // Armazena o token de forma segura
        public async Task StoreTokenAsync(string token)
        {
            try
            {
                // Tenta primeiro no SecureStorage
                await SecureStorage.SetAsync(TokenKey, token);

                // Verificação específica para Windows
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    // Fallback no Preferences se SecureStorage falhar
                    if (string.IsNullOrEmpty(await SecureStorage.GetAsync(TokenKey)))
                    {
                        Preferences.Set(WindowsTokenKey, token);
                        Debug.WriteLine("Token armazenado no Preferences (Windows fallback)");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao armazenar token: {ex.Message}");

                // Fallback obrigatório para Windows em caso de erro
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    Preferences.Set(WindowsTokenKey, token);
                }
            }
        }

        // Recupera o token
        public async Task<string> GetTokenAsync()
        {
            try
            {
                // Tenta primeiro do SecureStorage
                var token = await SecureStorage.GetAsync(TokenKey);

                // Fallback para Windows se necessário
                if (DeviceInfo.Platform == DevicePlatform.WinUI && string.IsNullOrEmpty(token))
                {
                    token = Preferences.Get(WindowsTokenKey, null);
                    if (token != null) Debug.WriteLine("Token recuperado do Preferences (Windows fallback)");
                }

                return token ?? default!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao recuperar token: {ex.Message}");

                // Fallback para Windows
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    return Preferences.Get(WindowsTokenKey, null) ?? default!;
                }
                return null!;
            }
        }

        // Remove o token
        public void RemoveToken()
        {
            try
            {
                SecureStorage.Remove(TokenKey);

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    Preferences.Remove(WindowsTokenKey);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao remover token: {ex.Message}");
            }
        }
    }
}
