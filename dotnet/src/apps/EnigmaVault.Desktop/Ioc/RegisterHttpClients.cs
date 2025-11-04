using EnigmaVault.Authentication.ApiClient.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterHttpClients
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? authServiceApiUrl = configuration.GetValue<string>("BaseAuthServiceUrl");
            const string authServiceApiClientName = "AuthApiClient";
            services.AddHttpClient(authServiceApiClientName, client => client.BaseAddress = new Uri(authServiceApiUrl!));
            services.AddHttpClient<IAuthService, AuthService>(authServiceApiClientName);

            string? userManagementServiceApiUrl = configuration.GetValue<string>("BaseUserManagementServiceUrl");
            const string userManagementServiceApiClientName = "UserManagementApiClient";
            services.AddHttpClient(userManagementServiceApiClientName, client => client.BaseAddress = new Uri(userManagementServiceApiUrl!));
            services.AddHttpClient<IUserManagementService, UserManagementService>(userManagementServiceApiClientName);

            return services;
        }
    }
}