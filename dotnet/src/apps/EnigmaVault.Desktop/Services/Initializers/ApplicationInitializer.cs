using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.Managers;
using Shared.Contracts.Requests;
using Shared.WPF.Navigations.Windows;

namespace EnigmaVault.Desktop.Services.Initializers
{
    internal class ApplicationInitializer(
        ITokenManager tokenManager,
        IAuthService authService,
        IWindowNavigation windowNavigation) : IApplicationInitializer
    {
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly IAuthService _authService = authService;
        private readonly IWindowNavigation _windowNavigation = windowNavigation;

        public void InitializeAsync()
        {
            var tokenMaybe = _tokenManager!.GetToken();

            tokenMaybe.Match(
                onSome: async token =>
                {
                    var authResult = await _authService!.LoginByToken(new LoginByTokenRequest(token));

                    authResult.Switch(
                        onSuccess: () =>
                        {
                            _tokenManager.SaveToken(authResult.Value!.RefreshToken);
                            _windowNavigation.Open(WindowsName.MainWindow);
                        },
                        onFailure: errors => _windowNavigation!.Open(WindowsName.AuthenticationWindow));
                },
                onNone: () => _windowNavigation!.Open(WindowsName.AuthenticationWindow));
        }
    }
}