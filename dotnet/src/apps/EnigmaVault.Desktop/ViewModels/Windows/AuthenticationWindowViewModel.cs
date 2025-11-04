using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Authentication;
using Shared.WPF.Navigations.Windows;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class AuthenticationWindowViewModel : BaseWindowViewModel
    {
        private readonly IWindowNavigation _windowNavigation;
        private readonly IPageNavigation _pageNavigation;
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthService _authService;
        private readonly ITokenManager _tokenManager;

        public AuthenticationWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IUserManagementService userManagementService,
        IAuthService authService,
        ITokenManager tokenManager) : base(windowNavigation, pageNavigation)
        {
            _windowNavigation = windowNavigation;
            _pageNavigation = pageNavigation;
            _userManagementService = userManagementService;
            _authService = authService;
            _tokenManager = tokenManager;

            Login = new(_windowNavigation, _authService, _tokenManager);
            Registration = new(_userManagementService);
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