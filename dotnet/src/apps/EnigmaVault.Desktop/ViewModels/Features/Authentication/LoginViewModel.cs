using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using Quantropic.Security.Abstractions;
using Shared.Contracts.Requests.Authentication;
using System.Security.Cryptography;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Features.Authentication
{
    internal sealed partial class LoginViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IAuthService authService,
        IUserManagementService userManagementService,
        IUserContext userContext,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISrpClient srpClient,
        IKeyDerivationService keyDerivationService,
        ICryptoServices cryptoServices) : BaseViewModel
    {
        private readonly IWindowNavigation _windowNavigation = windowNavigation;
        private readonly IPageNavigation _pageNavigation = pageNavigation;
        private readonly IAuthService _authService = authService;
        private readonly IUserManagementService _userManagementService = userManagementService;
        private readonly IUserContext _userContext = userContext;
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly IKeyManager _keyManager = keyManager;
        private readonly ISrpClient _srpClient = srpClient;
        private readonly IKeyDerivationService _keyDerivationService = keyDerivationService;
        private readonly ICryptoServices _cryptoServices = cryptoServices;

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

            var (kek, _) = _keyDerivationService.DeriveKeysFromPassword(AuthLogin, AuthPassword, salt);

            byte[]? dek;

            try
            {
                dek = _cryptoServices.DecryptData<byte[]>(publicInfo.EncryptedDek, kek);
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Неверный пароль (не удалось расшифровать ключ)!");
                return;
            }

            var srpChallengeRequest = new SrpChallengeRequest(AuthLogin);
            var srpChallengeResult = await _authService.GetSrpChallenge(srpChallengeRequest);

            if (srpChallengeResult.IsFailure)
            {
                MessageBox.Show(srpChallengeResult.StringMessage);
                return;
            }

            var (A, M1, S) = _srpClient.GenerateSrpProof(AuthLogin, AuthPassword, srpChallengeResult.Value.Salt, srpChallengeResult.Value.B);

            var srpVerifierRequest = new SrpVerifyRequest(AuthLogin, A, M1);
            var srpVerifierResult = await _authService.VerifySrpProof(srpVerifierRequest);

            if (srpChallengeResult.IsFailure)
            {
                MessageBox.Show(srpChallengeResult.StringMessage);
                return;
            }

            var isValidServer = _srpClient.VerifyServerM2(A, M1, S, srpVerifierResult.Value.M2!);

            if (!isValidServer)
            {
                MessageBox.Show("Подлинность сервера не получилось подтвердить");
                return;
            }

            _tokenManager.SaveTokens(new AccessData(srpVerifierResult.Value!.AccessToken, srpVerifierResult.Value!.RefreshToken));
            _keyManager.SaveKey(dek!);

            var userInfo = await _userManagementService.Me(srpVerifierResult.Value.AccessToken);

            _userContext.UpdateUserInfo(new UserInfo(userInfo.Value!.Id, userInfo.Value.Login));
            _userContext.UpdateTokens(new AccessData(srpVerifierResult.Value.AccessToken, srpVerifierResult.Value.RefreshToken));
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