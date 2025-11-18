using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests;
using Shared.WPF.Navigations.Windows;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Components.Authentication
{
    internal sealed partial class LoginViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IAuthService authService,
        IUserManagementService userManagementService,
        IUserContext userContext,
        ITokenManager tokenManager) : BaseViewModel
    {
        private readonly IWindowNavigation _windowNavigation = windowNavigation;
        private readonly IPageNavigation _pageNavigation = pageNavigation;
        private readonly IAuthService _authService = authService;
        private readonly IUserManagementService _userManagementService = userManagementService;
        private readonly IUserContext _userContext = userContext;
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

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            _tokenManager.SaveTokens(new AccessData(result.Value!.AccessToken, result.Value!.RefreshToken));
            var userInfo = await _userManagementService.Me(result.Value.AccessToken);

            _userContext.UpdateUserInfo(new UserInfo(userInfo.Value!.Id, userInfo.Value.Login));
            _userContext.UpdateTokens(new AccessData(result.Value.AccessToken, result.Value.RefreshToken));

            _windowNavigation.Open(WindowsName.MainWindow);
            _windowNavigation.Close(WindowsName.AuthenticationWindow);
            _pageNavigation.Navigate(PagesName.Password, FramesName.MainFrame);
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