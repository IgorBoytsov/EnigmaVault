using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Components.Authentication
{
    internal sealed partial class RecoveryAccessViewModel(IUserManagementService userManagement) : BaseViewModel
    {
        private readonly IUserManagementService _userManagement = userManagement;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RecoveryAccessCommand))]
        private string _recoveryLogin = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RecoveryAccessCommand))]
        private string _recoveryNewPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RecoveryAccessCommand))]
        private string _recoveryEmail = string.Empty;

        public event Action? Sent;

        [RelayCommand(CanExecute = nameof(CanRegister))]
        private async Task RecoveryAccess()
        {
            var request = new RecoveryAccessRequest(RecoveryLogin, RecoveryEmail, RecoveryNewPassword);

            var result = await _userManagement.RecoveryAccess(request);

            result.Switch(
                onSuccess: () => Sent?.Invoke(),
                onFailure: errors => MessageBox.Show(result.StringMessage));
        }

        private bool CanRegister() => !string.IsNullOrWhiteSpace(RecoveryLogin) && !string.IsNullOrWhiteSpace(RecoveryNewPassword) && !string.IsNullOrWhiteSpace(RecoveryEmail);
    }
}