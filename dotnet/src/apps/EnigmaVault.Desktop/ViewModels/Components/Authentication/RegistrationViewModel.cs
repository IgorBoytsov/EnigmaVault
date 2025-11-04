using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Authentication.ApiClient.HttpClients;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Requests;
using System.Windows;

namespace EnigmaVault.Desktop.ViewModels.Components.Authentication
{
    internal sealed partial class RegistrationViewModel(IUserManagementService userManagement) : BaseViewModel
    {
        private readonly IUserManagementService _userManagement = userManagement;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _registrationLogin = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _registrationPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _registrationUserName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _registrationEmail = string.Empty;

        public event Action? Registered;

        [RelayCommand(CanExecute = nameof(CanRegister))]
        private async Task Register()
        {
            var request = new RegisterUserRequest(RegistrationLogin, RegistrationPassword, RegistrationUserName, RegistrationEmail, null, null, null);

            var result = await _userManagement.Register(request);

            result.Switch(
                onSuccess: (Action)(() =>
                {
                    MessageBox.Show(result.Value);
                    Registered?.Invoke();
                }),
                onFailure: errors => MessageBox.Show(result.StringMessage));
        }

        private bool CanRegister() => !string.IsNullOrWhiteSpace(RegistrationLogin) && !string.IsNullOrWhiteSpace(RegistrationPassword) && !string.IsNullOrWhiteSpace(RegistrationUserName) && !string.IsNullOrWhiteSpace(RegistrationEmail);
    }
}