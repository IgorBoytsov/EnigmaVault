using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Helpers;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Controller;
using System.Collections.ObjectModel;

namespace EnigmaVault.Desktop.ViewModels.Pages
{
    internal sealed class EncryptedPassword
    {
        public string FolderName { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdate { get; set; }
        public string Url { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
    }

    internal sealed partial class PasswordPageViewModel : BasePageViewModel, IAsyncInitializable, IUpdatable
    {
        /*--Инициализация---------------------------------------------------------------------------------*/

        public PasswordPageViewModel()
        {
            var random = new Random();

            Passwords = Enumerable.Range(0, 30).Select(x => new EncryptedPassword
            {
                DateAdded = DateTime.UtcNow.AddDays(random.Next(-80 , -20)),
                DateUpdate = DateTime.UtcNow.AddDays(random.Next(-19, -1)),
                FolderName = $"Папка номер {random.Next(0, 20)}",
                Image = @"C:\Users\light\Downloads\f855982e-2b4e-4629-9c6f-6820e05690d9.png",
                ServiceName = $"Сервис номер {random.Next(0, 100)}",
                Url = "https://www.youtube.com/@ZUBAREV_CUTTING"
            }).ToObservableCollection();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {

        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ObservableCollection<EncryptedPassword> Passwords { get; init; } = [];
        public ObservableCollection<string> Tags { get; init; } = Enumerable.Range(0, 100).Select(x => $"Тэг №{x}").ToObservableCollection();
        public ObservableCollection<string> Folders { get; init; } = Enumerable.Range(0, 100).Select(x => $"Папка №{x}").ToObservableCollection();

        /*--Свойства--------------------------------------------------------------------------------------*/

        public ToolTipController RightToolTipController { get; } = new(ToolTipPlacement.CenterRight);
        public ToolTipController LeftToolTipController { get; } = new(ToolTipPlacement.CenterLeft);
        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController BottomToolTipController { get; } = new(ToolTipPlacement.CenterBottom);

        public PopupController PasswordMenuPopup { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetSideMenuControlCommand))]
        private UserControlsName _currentDisplayUserControl = UserControlsName.Folders;

        [ObservableProperty]
        private EncryptedPassword? _selectedEncryptedPassword;

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команда [SetSideMenuControlCommand]: Отвечает за выбор текущего оборажаемого контрола на боковой понели

        [RelayCommand(CanExecute = nameof(CanSetSideMenuControl))]
        private void SetSideMenuControl(UserControlsName controlName) => CurrentDisplayUserControl = controlName;

        private bool CanSetSideMenuControl(UserControlsName controlsName) => CurrentDisplayUserControl != controlsName;
        #endregion

        #region Команда [SelectAndShowPasswordMenuPopup]: Отвечает за выбор элемента списка при открытие контекстного меню 

        [RelayCommand]
        private void SelectAndShowPasswordMenuPopup(EncryptedPassword password)
        {
            if (password is null) return;

            SelectedEncryptedPassword = password;

            PasswordMenuPopup.ShowAtMouse();
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

    }
}