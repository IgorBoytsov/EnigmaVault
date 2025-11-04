using EnigmaVault.Desktop.Ioc;
using EnigmaVault.Desktop.Services.Initializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EnigmaVault.Desktop
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("apiconfig.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddWindows();
            services.AddPages();
            services.AddServices();
            services.AddHttpServices(configuration);

            ServiceProvider = services.BuildServiceProvider();

            var appInitializer = ServiceProvider.GetService<IApplicationInitializer>();

            try
            {
                appInitializer!.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка при запуске: {ex.Message}");
                Shutdown();
            }

            base.OnStartup(e);
        }
    }
}