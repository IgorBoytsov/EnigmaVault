using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Windows;
using EnigmaVault.Desktop.Views.Windows;
using System.Windows;

namespace EnigmaVault.Desktop.Factories.Windows
{
    internal sealed class MainWindowFactory(Func<MainWindowViewModel> vmFactory) : IWindowFactory
    {
        private readonly Func<MainWindowViewModel> _vmFactory = vmFactory;

        public WindowsName WindowName => WindowsName.MainWindow;

        public Window CreateWindow()
        {
            var vm = _vmFactory();

            var window = new MainWindow() { DataContext = vm };

            window.Loaded += (s, e) =>
            {

            };

            return window;
        }
    }
}