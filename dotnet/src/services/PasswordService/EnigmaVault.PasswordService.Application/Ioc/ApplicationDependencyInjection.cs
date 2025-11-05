using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreatePersonal;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviors;

namespace EnigmaVault.PasswordService.Application.Ioc
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var currentAssembly = typeof(CreatePersonalCategoryCommandHandler).Assembly;
            services.AddValidatorsFromAssembly(currentAssembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(currentAssembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddAutoMapper(cfg => cfg.LicenseKey = configuration["AutoMapper:AutoMapperKey"], currentAssembly);

            return services;
        }
    }
}