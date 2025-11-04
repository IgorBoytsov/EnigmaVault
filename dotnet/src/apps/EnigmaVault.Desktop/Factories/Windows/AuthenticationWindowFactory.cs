using EnigmaVault.Desktop.ViewModels.Windows;
using EnigmaVault.Desktop.Views.Windows;
using Shared.WPF.Navigations.Windows;
using System.Windows;

namespace EnigmaVault.Desktop.Factories.Windows
{
    internal sealed class AuthenticationWindowFactory(Func<AuthenticationWindowViewModel> vmFactory) : IWindowFactory
    {
        private readonly Func<AuthenticationWindowViewModel> _vmFactory = vmFactory;

        public Window CreateWindow()
            => new AuthenticationWindow() { DataContext = _vmFactory() };
    }
}