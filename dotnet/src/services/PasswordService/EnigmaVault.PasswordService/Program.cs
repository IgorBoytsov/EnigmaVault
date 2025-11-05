
using EnigmaVault.PasswordService.Application.Ioc;
using EnigmaVault.PasswordService.Infrastructure.Ioc;

namespace EnigmaVault.PasswordService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}