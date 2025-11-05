using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EnigmaVault.PasswordService.Application.Ioc
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(cfg => cfg.LicenseKey = configuration["AutoMapper:AutoMapperKey"], Assembly.GetExecutingAssembly());

            return services;
        }
    }
}