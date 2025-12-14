using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests;
using Shared.WPF.Navigations.Windows;
using System.Security.Cryptography;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Components.Authentication
{
    internal sealed partial class LoginViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IAuthService authService,
        IUserManagementService userManagementService,
        IUserContext userContext,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISecureDataService secureDataService) : BaseViewModel
    {
        private readonly IWindowNavigation _windowNavigation = windowNavigation;
        private readonly IPageNavigation _pageNavigation = pageNavigation;
        private readonly IAuthService _authService = authService;
        private readonly IUserManagementService _userManagementService = userManagementService;
        private readonly IUserContext _userContext = userContext;
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly IKeyManager _keyManager = keyManager;
        private readonly ISecureDataService _secureDataService = secureDataService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authLogin = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _authPassword = string.Empty;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            var userPublicInfo = await _userManagementService.GetPublicEncryptionInfo(AuthLogin);

            if (userPublicInfo.IsFailure)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            var publicInfo = userPublicInfo.Value;
            byte[] salt = Convert.FromBase64String(publicInfo.ClientSalt);

            var (kek, authHash) = _secureDataService.DeriveKeysFromPassword(AuthPassword, salt);

            byte[]? dek;

            try
            {
                dek = _secureDataService.DecryptData<byte[]>(publicInfo.EncryptedDek, kek);
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Неверный пароль (не удалось расшифровать ключ)!");
                return;
            }

            var request = new LoginRequest(AuthLogin, authHash);
            var result = await _authService.Login(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            _tokenManager.SaveTokens(new AccessData(result.Value!.AccessToken, result.Value!.RefreshToken));
            _keyManager.SaveKey(dek!);

            var userInfo = await _userManagementService.Me(result.Value.AccessToken);

            _userContext.UpdateUserInfo(new UserInfo(userInfo.Value!.Id, userInfo.Value.Login));
            _userContext.UpdateTokens(new AccessData(result.Value.AccessToken, result.Value.RefreshToken));
            _userContext.UpdateDek(dek!);

            Array.Clear(kek, 0, kek.Length);

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