using System.Xml.Linq;

namespace Helpers;

public enum PagesEnum
{
    [PageInfo(
        Name = "Login",
        NickName = "LoginPage",
        Path = "/user/login",
        HasBeenValidated = true
    )] LOGIN = 1,

    [PageInfo(
        Name = "Home",
        NickName = "HomePage",
        Path = "/"
       
    )] HOME = 2,

    [PageInfo(
        Name = "Cadastro",
        NickName = "CadastroPage",
        Path = "/user/cadastro"
    )] CADASTRO = 3,

    [PageInfo(
        Name = "Pessoal",
        NickName = "PessoalPage",
        Path = "/users/pagina-pessoal"
    )] PESSOAL = 4,

    [PageInfo(
        Name = "Error",
        NickName = "ErrorPage",
        Path = "/error",
        HasBeenValidated = true
    )] ERROR = 99
}

public static class PageInfoExtensions
{
    private static readonly List<PageInfoAttribute> _cachedPages;

    static PageInfoExtensions()
    {
        _cachedPages = Enum.GetValues(typeof(PagesEnum))
                          .Cast<PagesEnum>()
                          .Select(p => p.GetPageInfo())
                          .Where(p => p != null)
                          .ToList()!;
    }
    public static bool PathExists(string path)
    {
        return _cachedPages.Any(p =>
            p.Path != null &&
            p.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
    }

    public static bool TryGetPageInfoByPath(string path, out PageInfoAttribute? pageInfo)
    {
        pageInfo = _cachedPages.FirstOrDefault(p =>
            p.Path != null &&
            p.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        return pageInfo != null;
    }

    public static PageInfoAttribute? GetPageInfo(this PagesEnum page)
    {
        var fieldInfo = page.GetType().GetField(page.ToString());
        return fieldInfo?.GetCustomAttributes(typeof(PageInfoAttribute), false)
                      .FirstOrDefault() as PageInfoAttribute;
    }

    // Novo método para buscar pelo Path
    public static PageInfoAttribute? GetPageInfoByPath(string path)
    {
        return _cachedPages.FirstOrDefault(p =>
            p.Path != null &&
            p.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
    }

    // Método para verificar se um path existe no enum
    public static bool PathExists(this IEnumerable<PagesEnum> pages, string path)
    {
        return pages.Any(p =>
            p.GetPageInfo()?.Path?.Equals(path, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
public static class RouteUser
{
    public static List<PageInfoAttribute> RouteUserList { get; private set; } = new();

    public static void LoadRoutesFromEnum()
    {
        RouteUserList.Clear();

        foreach (PagesEnum page in Enum.GetValues(typeof(PagesEnum)))
        {
            var pageInfo = page.GetPageInfo();
            if (pageInfo != null)
            {
                RouteUserList.Add(pageInfo);
            }
        }
    }

    // Método para futura integração com banco de dados
    public static async Task SyncWithDatabaseAsync()
    {
        // Implementação futura para sincronizar com BD
        await Task.CompletedTask;
    }
}
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class PageInfoAttribute : Attribute
{
    public string? Name { get; set; }
    public string? NickName { get; set; }
    public string? Path { get; set; }
    public int PermissionGroup { get; set; }
    public bool HasBeenValidated { get; set; } // Corrigido nome para seguir convenção C#
}

