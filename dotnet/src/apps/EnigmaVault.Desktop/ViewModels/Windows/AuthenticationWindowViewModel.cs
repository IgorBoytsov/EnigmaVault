using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Features.Authentication;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class AuthenticationWindowViewModel : BaseWindowViewModel
    {
        private readonly IWindowNavigation _windowNavigation;
        private readonly IPageNavigation _pageNavigation;
        private readonly IUserManagementService _userManagementService;
        private readonly IUserContext _userContext;
        private readonly IAuthService _authService;
        private readonly ITokenManager _tokenManager;
        private readonly IKeyManager _keyManager;
        private readonly ISecureDataService _secureDataService ;

        public AuthenticationWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IUserManagementService userManagementService,
        IUserContext userContext,
        IAuthService authService,
        ITokenManager tokenManager,
        IKeyManager keyManager,
        ISecureDataService secureDataService) : base(windowNavigation, pageNavigation)
        {
            _windowNavigation = windowNavigation;
            _pageNavigation = pageNavigation;
            _userManagementService = userManagementService;
            _userContext = userContext;
            _authService = authService;
            _tokenManager = tokenManager;
            _keyManager = keyManager;
            _secureDataService = secureDataService;

            Login = new(_windowNavigation, _pageNavigation, _authService, _userManagementService, _userContext, _tokenManager, _keyManager, _secureDataService);
            Registration = new(_userManagementService, _secureDataService);
            RecoveryAccess = new(_userManagementService);

            Registration.Registered += () => CurrentAuthenticationType = AuthenticationType.Authentication;
            RecoveryAccess.Sent += () => CurrentAuthenticationType = AuthenticationType.Authentication;
        }

        public LoginViewModel Login { get; }

        public RegistrationViewModel Registration { get; } 

        public RecoveryAccessViewModel RecoveryAccess { get; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SwitchAuthenticationControlCommand))]
        [NotifyCanExecuteChangedFor(nameof(SwitchRegistrationControlCommand))]
        [NotifyCanExecuteChangedFor(nameof(SwitchRecoveryAccessControlCommand))]
        private AuthenticationType _currentAuthenticationType;

        [RelayCommand(CanExecute = nameof(CanSwitchAuthenticationControl))]
        private void SwitchAuthenticationControl() => CurrentAuthenticationType = AuthenticationType.Authentication;

        private bool CanSwitchAuthenticationControl() => CurrentAuthenticationType != AuthenticationType.Authentication;

        [RelayCommand(CanExecute = nameof(CanSwitchRegistrationControl))]
        private void SwitchRegistrationControl() => CurrentAuthenticationType = AuthenticationType.Registration;

        private bool CanSwitchRegistrationControl() => CurrentAuthenticationType != AuthenticationType.Registration;

        [RelayCommand(CanExecute = nameof(CanSwitchRecoveryAccessControl))]
        private void SwitchRecoveryAccessControl() => CurrentAuthenticationType = AuthenticationType.RecoveryAccess;

        private bool CanSwitchRecoveryAccessControl() => CurrentAuthenticationType != AuthenticationType.RecoveryAccess;
    }
}