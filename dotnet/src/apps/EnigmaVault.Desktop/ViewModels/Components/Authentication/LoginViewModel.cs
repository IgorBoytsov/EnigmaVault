using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests;
using Shared.WPF.Navigations.Windows;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Components.Authentication
{
    internal sealed partial class LoginViewModel(
        IWindowNavigation windowNavigation,
        IAuthService authService,
        ITokenManager tokenManager) : BaseViewModel
    {
        private readonly IWindowNavigation _windowNavigation = windowNavigation;
        private readonly IAuthService _authService = authService;
        private readonly ITokenManager _tokenManager = tokenManager;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authLogin = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authPassword = string.Empty;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            var request = new LoginRequest(AuthLogin, AuthPassword);

            var result = await _authService.Login(request);

            result.Switch(
                onSuccess: () =>
                {
                    _tokenManager.SaveToken(result.Value!.RefreshToken);
                    _windowNavigation.Open(WindowsName.MainWindow);
                    _windowNavigation.Close(WindowsName.AuthenticationWindow);
                },
                onFailure: errors => MessageBox.Show(result.StringMessage));
        }

        private bool CanLogin() => !string.IsNullOrWhiteSpace(AuthLogin) && !string.IsNullOrWhiteSpace(AuthPassword);

        [RelayCommand]
        private void LoginWithoutAuth()
        {
            _windowNavigation.Open(WindowsName.MainWindow);
            _windowNavigation.Close(WindowsName.AuthenticationWindow);
        }
    }
}