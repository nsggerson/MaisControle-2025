using Helpers;
using AppDesktop.Token;
using Domain.Interfaces.Generics;
using Domain.Interfaces.ICategoria;
using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfacesServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Domain.Servicos;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio;
using Infra.Repositorio.Generics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppDesktop.Controller;
using MudBlazor.Services;

namespace AppDesktop;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Configuração do DbContext
        builder.Services.AddDbContext<ContextBase>(options =>
        {
            options.UseSqlServer(
                @"Data Source=JHONPC\SQLEXPRESS;Initial Catalog=FN2025;Integrated Security=True;TrustServerCertificate=True",
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
        });

        // Configuração do Identity com CustomIdentityErrorDescriber
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<ContextBase>()
        .AddErrorDescriber<CustomIdentityErrorDescriber>()
        .AddDefaultTokenProviders();

        // INTERFACES E REPOSITORIOS
        builder.Services.AddSingleton(typeof(InterfaceGeneric<>), typeof(RepositoryGenerics<>));
        builder.Services.AddSingleton<InterfaceCategoria, RepositorioCategoria>();
        builder.Services.AddSingleton<InterfaceDespesa, RepositorioDespesa>();
        builder.Services.AddSingleton<InterfaceSistemaFinanceiro, RepositorioSistemaFinanceiro>();
        builder.Services.AddSingleton<InterfaceUsuarioSistemaFinanceiro, RepositorioUsuarioSistemaFinanceiro>();

        // SERVIÇOS E DOMINIOS
        builder.Services.AddSingleton<ICategoriaServico, CategoriaServico>();
        builder.Services.AddSingleton<IDespesaServico, DespesaServico>();
        builder.Services.AddSingleton<ISistemaFinanceiroServico, SistemaFinanceiroServico>();
        builder.Services.AddSingleton<IUsuarioSistemaFinanceiroServico, UsuarioSistemaFinanceiroServico>();

        // Serviços auxiliares
        builder.Services.AddSingleton<TokenStorageService>();
        builder.Services.AddSingleton<StartPage>();

        // Blazor e MudBlazor
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Registre seus controllers locais
        builder.Services.AddTransient<AuthController>();
        builder.Services.AddTransient<UsersController>();

        var app = builder.Build();

        // Configura o ServiceProvider para o roteador interno
        LocalRouter.ServiceProvider = app.Services;

        return app;
    }
}




//using Helpers;
//using AppDesktop.Token;
//using Domain.Interfaces.Generics;
//using Domain.Interfaces.ICategoria;
//using Domain.Interfaces.IDespesa;
//using Domain.Interfaces.InterfacesServicos;
//using Domain.Interfaces.ISistemaFinanceiro;
//using Domain.Interfaces.IUsuarioSistemaFinanceiro;
//using Domain.Servicos;
//using Entities.Entidades;
//using Infra.Configuracao;
//using Infra.Repositorio;
//using Infra.Repositorio.Generics;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using AppDesktop.Controller;
//using MudBlazor.Services;


//namespace AppDesktop
//{
//    public static class MauiProgram
//    {
//        public static MauiApp CreateMauiApp()
//        {
//            var builder = MauiApp.CreateBuilder();

//            builder
//                .UseMauiApp<App>()
//                .ConfigureFonts(fonts =>
//                {
//                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                });

//             // Configuração PRIMÁRIA do DbContext
//            builder.Services.AddDbContext<ContextBase>(options =>
//            {
//                options.UseSqlServer(
//                    @"Data Source=JHONPC\SQLEXPRESS;Initial Catalog=FN2025;Integrated Security=True;TrustServerCertificate=True",
//                    sqlOptions =>
//                    {
//                        sqlOptions.EnableRetryOnFailure(
//                            maxRetryCount: 5,
//                            maxRetryDelay: TimeSpan.FromSeconds(10),
//                            errorNumbersToAdd: null);
//                    });
//            });

//            // Configuração do Identity DEVE vir após o AddDbContext
//            builder.Services.AddIdentityCore<ApplicationUser>(options =>
//            {
//                options.SignIn.RequireConfirmedAccount = false;
//                options.Password.RequireDigit = false;
//                options.Password.RequiredLength = 6;
//                options.Password.RequireNonAlphanumeric = false;
//                options.Password.RequireUppercase = false;
//                options.Password.RequireLowercase = false;
//            })
//                .AddRoles<IdentityRole>() // Se estiver usando roles
//                .AddEntityFrameworkStores<ContextBase>()
//                .AddDefaultTokenProviders();

//            // Configuração adicional para resolver problemas específicos
//            builder.Services.ConfigureApplicationCookie(options =>
//            {
//                options.Events.OnRedirectToLogin = context =>
//                {
//                    context.Response.StatusCode = 401;
//                    return Task.CompletedTask;
//                };
//            });


//            // Configuração do Identity
//            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<ContextBase>()
//                .AddDefaultTokenProviders();

//            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<ContextBase>() // substitua pelo seu contexto
//                .AddErrorDescriber<CustomIdentityErrorDescriber>()
//                .AddDefaultTokenProviders();

//            // INTERFACES E REPOSITORIOS
//            builder.Services.AddSingleton(typeof(InterfaceGeneric<>), typeof(RepositoryGenerics<>));
//            builder.Services.AddSingleton<InterfaceCategoria, RepositorioCategoria>();
//            builder.Services.AddSingleton<InterfaceDespesa, RepositorioDespesa>();
//            builder.Services.AddSingleton<InterfaceSistemaFinanceiro, RepositorioSistemaFinanceiro>();
//            builder.Services.AddSingleton<InterfaceUsuarioSistemaFinanceiro, RepositorioUsuarioSistemaFinanceiro>();

//            // SERVIÇOS E DOMINIOS
//            builder.Services.AddSingleton<ICategoriaServico, CategoriaServico>();
//            builder.Services.AddSingleton<IDespesaServico, DespesaServico>();
//            builder.Services.AddSingleton<ISistemaFinanceiroServico, SistemaFinanceiroServico>();
//            builder.Services.AddSingleton<IUsuarioSistemaFinanceiroServico, UsuarioSistemaFinanceiroServico>();

//            builder.Services.AddSingleton<TokenStorageService>();

//            builder.Services.AddMauiBlazorWebView();
//            builder.Services.AddMudServices();

//#if DEBUG
//            builder.Services.AddBlazorWebViewDeveloperTools();

//            builder.Logging.AddDebug();
//#endif

//            // Registre seus controllers
//            builder.Services.AddTransient<AuthController>();
//            builder.Services.AddTransient<UsersController>();

//            var app = builder.Build();

//            // Configura o ServiceProvider para o LocalRouter
//            LocalRouter.ServiceProvider = app.Services;

//            return app;
//        }
//    }
//}


//using Helpers;
//using AppDesktop.Token;
//using Domain.Interfaces.Generics;
//using Domain.Interfaces.ICategoria;
//using Domain.Interfaces.IDespesa;
//using Domain.Interfaces.InterfacesServicos;
//using Domain.Interfaces.ISistemaFinanceiro;
//using Domain.Interfaces.IUsuarioSistemaFinanceiro;
//using Domain.Servicos;
//using Entities.Entidades;
//using Infra.Configuracao;
//using Infra.Repositorio;
//using Infra.Repositorio.Generics;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using AppDesktop.Controller;

//namespace AppDesktop
//{
//    public static class MauiProgram
//    {
//        public static MauiApp CreateMauiApp()
//        {
//            var builder = MauiApp.CreateBuilder();

//            builder
//                .UseMauiApp<App>()
//                .ConfigureFonts(fonts =>
//                {
//                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                });

//            // Injeção do LocalRouter
//            //
//            //builder.Services.AddSingleton<LocalRouter>();
//            //builder.Services.AddDbContext<ContextBase>(options =>
//            //   options.UseSqlServer(
//            //       builder.Configuration.GetConnectionString("DefaultConnection")));

//            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//            //    .AddEntityFrameworkStores<ContextBase>();
//            // Registra o ContextBase no DI
//            //builder.Services.AddDbContext<ContextBase>(options =>
//            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//            //// Registra o ContextBase no DI (não precisa passar a string de conexão aqui)
//            //builder.Services.AddDbContext<ContextBase>();

//            //// Registra UserManager, SignInManager e RoleManager manualmente
//            //builder.Services.AddScoped<UserManager<ApplicationUser>>();
//            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
//            //builder.Services.AddScoped<RoleManager<IdentityRole>>();
//            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ContextBase>()
//    .AddDefaultTokenProviders();

//            //INTERFACES E REPOSITORIOS
//            builder.Services.AddSingleton(typeof(InterfaceGeneric<>), typeof(RepositoryGenerics<>));
//            builder.Services.AddSingleton<InterfaceCategoria, RepositorioCategoria>();
//            builder.Services.AddSingleton<InterfaceDespesa, RepositorioDespesa>();
//            builder.Services.AddSingleton<InterfaceSistemaFinanceiro, RepositorioSistemaFinanceiro>();
//            builder.Services.AddSingleton<InterfaceUsuarioSistemaFinanceiro, RepositorioUsuarioSistemaFinanceiro>();

//            //SERVIÇOS E DOMINIOS
//            builder.Services.AddSingleton<ICategoriaServico, CategoriaServico>();
//            builder.Services.AddSingleton<IDespesaServico, DespesaServico>();
//            builder.Services.AddSingleton<ISistemaFinanceiroServico, SistemaFinanceiroServico>();
//            builder.Services.AddSingleton<IUsuarioSistemaFinanceiroServico, UsuarioSistemaFinanceiroServico>();

//            builder.Services.AddSingleton<TokenStorageService>();



//            builder.Services.AddMauiBlazorWebView();

//#if DEBUG
//            builder.Services.AddBlazorWebViewDeveloperTools();
//            builder.Logging.AddDebug();
//#endif

//            // Registre seus controllers
//            builder.Services.AddTransient<AuthController>(); // <- adicione todos os que quiser injetar

//            var app = builder.Build();

//            // Aqui é onde você configura o ServiceProvider
//            LocalRouter.ServiceProvider = app.Services;

//            //return app;
//            return builder.Build();
//        }
//    }
//}