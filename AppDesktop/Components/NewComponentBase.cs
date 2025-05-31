using AppDesktop.Components.Layout.SharedComponents;
using AppDesktop.Token;
using Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppDesktop.Components;

public class NewComponentBase : LayoutComponentBase, IDisposable
{
    const string secretKey = "Secret_Key-mQ7fzr4k3T9wYyB7vH2qP1lA5sJ8dF0gX6cN4bV1xZ9pL2oM3eK5iU8hR7tG0j";

    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public TokenStorageService TokenStorage { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public StartPage StartPage { get; set; } = default!;

    //private Timer _countdownTimer;

    private bool _disposed;
    public string? GLOBAL_TOKEN { get; set; }
    public string CURRENT_URI { get; set; } = string.Empty;
    public string? USERI_ID { get; set; }
    public string? USER_EMAIL { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? USER_DOCUMENT { get; set; }

    public bool InicializedPage { get; set; } = true;

    public RenderFragment _body { get; set; } = null!;


    public DateTime? EXPIRATION_TIME { get; set; }
    public DateTime? USER_TIME { get; set; }


    private bool _lastUriChecked;
    public string PageName { get; set; } = string.Empty;
    private bool _isInitialLoad= true;

    private List<string> ItemLockedOnScreen = new List<string> { "Login", "Cadastro" };

    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();

        //CURRENT_URI = NavigationManager.Uri;
        NavigationManager.LocationChanged += OnLocationChanged;
        RouteUser.LoadRoutesFromEnum(); // Carrega as rotas do enum
        await LoadInitialData();
    }

    private async Task LoadInitialData()
    {
        GLOBAL_TOKEN = await TokenStorage.GetTokenAsync();

        if (string.IsNullOrEmpty(GLOBAL_TOKEN))
        {
            CurrentPath.LoginSuccess = false;
            return;
        }

        CurrentPath.LoginSuccess = await TokenStorage.IsSessionValidAsync();

        if (!CurrentPath.LoginSuccess)
        {
            GLOBAL_TOKEN = null;
        }

        else if (CurrentPath.LoginSuccess)
        {
           await LoadUserData();
        }

        await Task.Delay(500);
        StateHasChanged();
    }
    private void LoadCurrentPath()
    {
        CURRENT_URI = NavigationManager.Uri;

        if (Body?.Target != null && Body?.Target is RouteView routeView)
        {           
            CurrentPath.CurrentName = routeView.RouteData.PageType.Name;
        }
        if (!string.IsNullOrEmpty(NavigationManager.Uri))
        {
            CurrentPath.Path = NavigationManager.Uri;
        }        
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadInitialData();
            await TrocarPagina();
        }

        //if (!string.IsNullOrEmpty(GLOBAL_TOKEN) && !await TokenStorage.IsSessionValidAsync())
        //{
        //    await LoadInitialData();
        //}
    }

    private async Task TrocarPagina()
    {
        this.LoadCurrentPath();
        StartPage.InicializedPage = false;
        StateHasChanged();

        var path = CurrentPath.Path;
        var name = CurrentPath.CurrentName;
        var page = PageInfoExtensions.GetPageInfoByPath(path);
        if (!PageInfoExtensions.PathExists(path) && !CurrentPath.DestinationPageName.Equals(name))
        {
            CurrentPath.DestinationPageName = "Error";
            NavigationManager.NavigateTo("/error", forceLoad: true);
            return;
        }
        else if (page != null && page!.HasBeenValidated)
        {
            
        }
        else if (!CurrentPath.LoginSuccess && !CurrentPath.DestinationPageName.Equals(name))
        {
            CurrentPath.DestinationPageName = "Login";
            NavigationManager.NavigateTo("/user/login", forceLoad: true);
        }
        
        StartPage.InicializedPage = true;
        await Task.Delay(500);
        StateHasChanged();
    }

    //private async Task CheckSessionAsync()
    //{
    //    if (Body?.Target == null) return;

    //    if (Body?.Target is RouteView routeView)
    //    {
    //        PageName = Regex.Replace(routeView.RouteData.PageType.Name, @"Page$", "");

    //        if (ItemLockedOnScreen.Contains(PageName))
    //        {
    //            return;
    //        }
    //    }

    //    GLOBAL_TOKEN = await TokenStorage.GetTokenAsync();

    //    if (string.IsNullOrWhiteSpace(GLOBAL_TOKEN))
    //    {
    //        NavigationManager.NavigateTo("/user/login", forceLoad: true);
    //    }
    //    else if (!await TokenStorage.IsSessionValidAsync())
    //    {
    //        TokenStorage.Logout();
    //        NavigationManager.NavigateTo("/user/login", forceLoad: true);
    //        await _alerts("Sessão expirada. Faça login novamente.", Severity.Error);
    //    }
    //    else
    //    {
    //        await LoadUserData();
    //    }
    //}
    //private async Task CheckSessionAsync()
    //{
    //    if (Body?.Target == null) return;

    //    if (Body?.Target is RouteView routeView)
    //    {
    //        PageName = Regex.Replace(routeView.RouteData.PageType.Name, @"Page$", "");

    //        if (IsRouteAllowed(PageName))
    //        {
    //            return;
    //        }
    //    }

    //    // Obtém nome da página e valida via Enum
    //   // PageName = Regex.Replace(routeView.RouteData.PageType.Name, @"Page$", "");

    //    // Verifica se a rota existe e está liberada no Enum
    //    //if ()
    //    //{
    //    //    Console.WriteLine($"Acesso bloqueado: {PageName}");
    //    //    return;
    //    //}

    //    // Lógica de autenticação
    //    GLOBAL_TOKEN = await TokenStorage.GetTokenAsync();

    //    if (string.IsNullOrWhiteSpace(GLOBAL_TOKEN))
    //    {
    //        NavigationManager.NavigateTo("/user/login", forceLoad: true);
    //    }
    //    else if (!await TokenStorage.IsSessionValidAsync())
    //    {
    //        TokenStorage.Logout();
    //        NavigationManager.NavigateTo("/user/login", forceLoad: true);
    //        await _alerts("Sessão expirada. Faça login novamente.", Severity.Error);
    //    }
    //    else
    //    {
    //        await LoadUserData();
    //    }
    //}

    // Método auxiliar que verifica tudo no Enum
    private bool IsRouteAllowed(string pageName)
    {
        // Rotas públicas que não requerem validação
        if (pageName.Equals("LOGIN", StringComparison.OrdinalIgnoreCase))
            return true;

        // Busca na lista carregada do enum
        var routeInfo = RouteUser.RouteUserList.FirstOrDefault(r =>
            r.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) ?? false);

        return routeInfo is { HasBeenValidated: true };
    }

    private async Task LoadUserData()
    {
        if (string.IsNullOrEmpty(GLOBAL_TOKEN) || !await TokenStorage.IsSessionValidAsync() || !string.IsNullOrEmpty(USERI_ID)) return;

        USERI_ID = JwtTokenHelper.GetUserId(GLOBAL_TOKEN, secretKey);
        USER_EMAIL = JwtTokenHelper.GetUserEmail(GLOBAL_TOKEN, secretKey);
        FIRST_NAME = JwtTokenHelper.GetFirstName(GLOBAL_TOKEN, secretKey);
        USER_DOCUMENT = JwtTokenHelper.GetDocument(GLOBAL_TOKEN, secretKey);
        // Nova linha adicionada para calcular os minutos restantes
        USER_TIME = JwtTokenHelper.GetExpirationDate(GLOBAL_TOKEN, secretKey);
        // Aqui, armazena a data de expiração
        EXPIRATION_TIME = JwtTokenHelper.GetExpirationDate(GLOBAL_TOKEN, secretKey);
        await InvokeAsync(StateHasChanged);
    }

    private async void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        await TrocarPagina();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    public Task _alerts(string message, Severity severity, string position = Defaults.Classes.Position.BottomEnd)
    {
        Snackbar.Clear();
        Snackbar.Configuration.PositionClass = position;
        Snackbar.Configuration.MaxDisplayedSnackbars = 10;
        Snackbar.Add(message, severity);
        return Task.CompletedTask;
    }

    protected async Task<IResult> SendDataAsync<T>(T userModel, string url)
    {
        try
        {
            var json = JsonSerializer.Serialize(userModel);
            var result = await LocalRouter.ExecuteAsync(url, json);
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task ReloadPage()
    {
        await JS.InvokeVoidAsync("eval", "location.reload()");
    }

    public async Task<bool> HandleYesNoDialog(string title, MaxWidth maxWidth, DialogParameters<YesNoDialog> parameters)
    {        
        var options = new DialogOptions() { CloseButton = true, MaxWidth = maxWidth };

        var dialog = await DialogService.ShowAsync<YesNoDialog>(title, parameters, options);

        var result = await dialog.Result;

        return result!.Canceled;
    }
}



static class CurrentPath
{
    private static string _path = string.Empty;
    private static string _currentName = string.Empty;
    private static string _destinationPageName = string.Empty;
    public static bool LoginSuccess { get; set; }
    public static string Path
    {
        get => _path;
        set => _path = HandlePath(value);
    }

    public static string CurrentName
    {
        get => _currentName;
        set => _currentName = HandleCurrentName(value);
    }

    public static string DestinationPageName
    {
        get => _destinationPageName;
        set => _destinationPageName = HandleCurrentName(value);
    }

    private static string HandlePath(string param)
    {
        // Verifica se a string é nula ou vazia
        if (string.IsNullOrWhiteSpace(param))
            return "/";

        // Remove o protocolo e domínio (https://0.0.0.1/)
        var uri = new Uri(param);
        string path = uri.AbsolutePath; // Isso já inclui a barra inicial

        // Remove barras duplicadas acidentais
        path = Regex.Replace(path, "/{2,}", "/");

        // Caso especial para Home (apenas a barra)
        if (path.Equals("/", StringComparison.OrdinalIgnoreCase))
            return "/";

        // Remove a barra final apenas se não for parte de um path composto
        // Exemplo: "/user/cadastro" permanece como está
        //          "/user/cadastro/" vira "/user/cadastro" (se não for um path válido com barra)
        string pathWithoutSlash = path.TrimEnd('/');

        // Verifica qual versão existe no enum (com ou sem barra)
        bool pathExists = PageInfoExtensions.PathExists(path);
        bool pathWithoutSlashExists = PageInfoExtensions.PathExists(pathWithoutSlash);

        // Decide qual versão retornar
        if (pathExists && !pathWithoutSlashExists)
            return path; // Mantém com barra
        else
            return pathWithoutSlash; // Retorna sem barra
    }

    private static string HandleCurrentName(string param)
    {
        if (string.IsNullOrEmpty(param)) return string.Empty;

        return param.Replace("Page", "", StringComparison.OrdinalIgnoreCase)
                    /*.Replace("Component", "", StringComparison.OrdinalIgnoreCase)*/;
    }

}
