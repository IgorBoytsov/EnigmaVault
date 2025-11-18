using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Initializers;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.WindowNavigation;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Navigations.Windows;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IWindowNavigation, WindowNavigation>();
            services.AddSingleton<IPageNavigation, PageNavigation>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IApplicationInitializer, ApplicationInitializer>();
            services.AddSingleton<IUserContext, UserContext>();

            return services;
        }
    }
}