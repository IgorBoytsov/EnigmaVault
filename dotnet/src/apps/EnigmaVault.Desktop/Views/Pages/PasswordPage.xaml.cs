using EnigmaVault.Desktop.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Views.Pages
{
    public partial class PasswordPage : Page
    {
        public PasswordPage()
        {
            InitializeComponent();
        }

        private void PasswordPage_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);

            if (window != null)
            {
                window.LocationChanged += Popup_Deactivated;
                window.Deactivated += Popup_Deactivated;
            }   
        }

        private void PasswordPage_Unloaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.LocationChanged -= Popup_Deactivated;
                window.Deactivated -= Popup_Deactivated;
            }      
        }

        private void Popup_Deactivated(object? sender, EventArgs e)
        {
            if (DataContext is PasswordPageViewModel viewModel)
                if (viewModel.PasswordMenuPopup.IsOpen)
                    viewModel.PasswordMenuPopup.HideCommand.Execute(null);
        }
    }
}