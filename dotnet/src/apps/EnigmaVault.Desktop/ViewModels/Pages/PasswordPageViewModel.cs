using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Helpers;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Controller;
using EnigmaVault.Desktop.ViewModels.Components.Models;
using EnigmaVault.PasswordService.ApiClient.Clients;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Pages
{
    internal sealed partial class EncryptedPassword : BaseViewModel
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
        private readonly ITagService _tagService;

        /*--Инициализация---------------------------------------------------------------------------------*/

        public PasswordPageViewModel(ITagService tagService)
        {
            _tagService = tagService;

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

        public async Task InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            try
            {
                await GetTags();

                IsInitialize = true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Произошла ошибка инициализации: {ex}");
            }
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {

        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ObservableCollection<EncryptedPassword> Passwords { get; init; } = [];
        public ObservableCollection<TagViewModel> Tags { get; init; } = [];

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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTagCommand))]
        private string? _nameTag;

        [ObservableProperty]
        private TagViewModel? _selectedTag;

        partial void OnSelectedTagChanged(TagViewModel? value)
        {
            if (value is not null)
            {
                UpdateRed = value!.RgbColor.R.ToString();
                UpdateGreen = value!.RgbColor.G.ToString();
                UpdateBlue = value!.RgbColor.B.ToString();

                value.SetColor(Color.FromRgb(byte.Parse(UpdateRed), byte.Parse(UpdateGreen), byte.Parse(UpdateBlue)));
            }
        }

        #region Цвета

        [ObservableProperty]
        private string _red = "255";

        [ObservableProperty]
        private string _green = "255";

        [ObservableProperty]
        private string _blue = "255";

        [ObservableProperty]
        private string _updateRed = null!;

        [ObservableProperty]
        private string _updateGreen = null!;

        [ObservableProperty]
        private string _updateBlue = null!;

        partial void OnUpdateRedChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateGreenChanged(string value) => UpdateSelectedTagColor();

        partial void OnUpdateBlueChanged(string value) => UpdateSelectedTagColor();

        private void UpdateSelectedTagColor()
        {
            if (SelectedTag == null)
                return;

            bool redParsed = byte.TryParse(UpdateRed, out byte r);
            bool greenParsed = byte.TryParse(UpdateGreen, out byte g);
            bool blueParsed = byte.TryParse(UpdateBlue, out byte b);

            if (redParsed && greenParsed && blueParsed)
                SelectedTag?.SetColor(System.Windows.Media.Color.FromRgb(r, g, b));
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команда [CreateTagCommand]: Создает тэг

        [RelayCommand(CanExecute = nameof(CanCreateTag))]
        private async Task CreateTag()
        {
            var result = await _tagService.CreateAsync(new CreateTagRequest("b8f34e7a-b3c8-42e8-8cc2-8329fc8e4949", NameTag!, Helper.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue))));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Add(new TagViewModel(new TagResponse(result.Value, "b8f34e7a-b3c8-42e8-8cc2-8329fc8e4949", NameTag!, Helper.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue)))));

            NameTag = string.Empty;
        }

        private bool CanCreateTag() => !string.IsNullOrWhiteSpace(NameTag);

        #endregion

        #region Команда [DeleteTagCommand]: Удаляет тэг

        [RelayCommand]
        private async Task DeleteTag()
        {
            var result = await _tagService.DeleteAsync(SelectedTag!.Id);

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Remove(SelectedTag);
        }

        #endregion

        #region Команда [UpdateTagCommand]: Обновляет изменение в тэге

        [RelayCommand]
        private async Task UpdateTag()
        {
            var result = await _tagService.UpdateAsync(new UpdateTagRequest(SelectedTag!.Id, SelectedTag.TagName!, SelectedTag.HexColor));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                SelectedTag.RevertChanges();
                return;
            }

            SelectedTag.CommitChanges(new TagResponse(SelectedTag.Id, "b8f34e7a-b3c8-42e8-8cc2-8329fc8e4949", SelectedTag.TagName!, SelectedTag.HexColor));
        }

        #endregion

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

        public async Task GetTags()
        {
            var result = await _tagService.GetAll("b8f34e7a-b3c8-42e8-8cc2-8329fc8e4949"); 

            foreach (var item in result.Value)
            {
                Tags.Add(new TagViewModel(item));
            }
        }
    }
}