using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Helpers;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Controller;
using EnigmaVault.Desktop.ViewModels.Components.Models;
using EnigmaVault.PasswordService.ApiClient.Clients;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
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
        private readonly IIconCategoryService _iconCategoryService;
        private readonly IIconService _iconService;
        private readonly IUserContext _userContext;

        /*--Инициализация---------------------------------------------------------------------------------*/

        public PasswordPageViewModel(
            ITagService tagService,
            IIconCategoryService iconCategoryService,
            IIconService iconService,
            IUserContext userContext)
        {
            _tagService = tagService;
            _iconCategoryService = iconCategoryService;
            _iconService = iconService;
            _userContext = userContext;

            CurrentDisplayUserControl = UserControlsName.Tags;

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

            IconView = CollectionViewSource.GetDefaultView(Icons);
            IconCategoryView = CollectionViewSource.GetDefaultView(IconCategories);

            UpdateIconsView();
            UpdateIconCategoriesView();
        }

        public async Task InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            try
            {
                await GetTags();
                await GetIconCategories();
                await GetIcons();

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

        public ObservableCollection<IconViewModel> Icons { get; init; } = [];
        public ICollectionView IconView { get; private set; } = null!;

        public ObservableCollection<IconCategoryViewModel> IconCategories { get; init; } = [];
        public ICollectionView IconCategoryView { get; private init; } = null!;

        /*--Свойства--------------------------------------------------------------------------------------*/

        public ToolTipController RightToolTipController { get; } = new(ToolTipPlacement.CenterRight);
        public ToolTipController LeftToolTipController { get; } = new(ToolTipPlacement.CenterLeft);
        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController BottomToolTipController { get; } = new(ToolTipPlacement.CenterBottom);

        public PopupController PasswordMenuPopup { get; } = new(); 
        public PopupController AddIconPopup { get; } = new(); 
        public PopupController AddIconCategoryPopup { get; } = new(); 

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

        #region SVG

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCommand))]
        private string? _svgCode;

        partial void OnSvgCodeChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SvgConvertError = string.Empty;
                Svg = null;
                return;
            }

            try
            {
                Svg = ConvertSvgInString(value);
            }
            catch (Exception ex)
            {
                SvgConvertError = ex.Message;
            }

        }

        [ObservableProperty]
        private DrawingImage? _svg;

        [ObservableProperty]
        private string? _svgConvertError;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCommand))]
        private IconCategoryViewModel? _selectedIconCategory;

        #endregion

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCategoryCommand))]
        private string? _iconCategoryName;

        #region Свойство: [SelectedEditableCategory], Методы: [OnSelectedEditableCategoryChanging, OnSelectedEditableCategoryChanged, OnCategoryPropertyChanged]

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateIconCategortyCommand))]
        private IconCategoryViewModel? _selectedEditableCategory;

        partial void OnSelectedEditableCategoryChanging(IconCategoryViewModel? value)
        {
            if (SelectedEditableCategory is not null)
                SelectedEditableCategory.PropertyChanged -= OnCategoryPropertyChanged;
        }

        partial void OnSelectedEditableCategoryChanged(IconCategoryViewModel? value)
        {
            if (value is not null)
                value.PropertyChanged += OnCategoryPropertyChanged;

            UpdateIconCategortyCommand.NotifyCanExecuteChanged();
        }

        private void OnCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IconCategoryViewModel.HasChanges))
                UpdateIconCategortyCommand.NotifyCanExecuteChanged();
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команда [CreateTagCommand]: Создает тэг

        [RelayCommand(CanExecute = nameof(CanCreateTag))]
        private async Task CreateTag()
        {
            var result = await _tagService.CreateAsync(new CreateTagRequest(_userContext.Id, NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue))));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            Tags.Add(new TagViewModel(new TagResponse(result.Value, _userContext.Id, NameTag!, Helpers.ColorConverter.RgbToHex(int.Parse(Red), int.Parse(Green), int.Parse(Blue)))));

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

            SelectedTag.CommitChanges(new TagResponse(SelectedTag.Id, _userContext.Id, SelectedTag.TagName!, SelectedTag.HexColor));
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

        #region Команда [SaveIconCommand]: Отвечает за сохранение Svg иконки

        [RelayCommand(CanExecute = nameof(CanSaveIcon))]
        private async Task SaveIcon()
        {
            var result = await _iconService.CreatePersonalAsync(new CreateIconPersonalRequest(_userContext.Id, SvgCode!, "Тестовое название", SelectedIconCategory!.Id));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Icons.Add(new IconViewModel(new IconResponse(result.Value, _userContext.Id, SvgCode!, "", SelectedIconCategory.Id)));

            Svg = null;
            SvgCode = string.Empty;
            SelectedIconCategory = null;
        }

        private bool CanSaveIcon() => !string.IsNullOrWhiteSpace(SvgCode) && SelectedIconCategory != null;

        #endregion

        #region Команда [SaveIconCategoryCommand]: Отвечает за сохранение категории иконки

        [RelayCommand(CanExecute = nameof(CanSaveIconCategory))]
        private async Task SaveIconCategory()
        {
            var result = await _iconCategoryService.CreatePersonalAsync(new CreateIconCategoryPersonalRequest(IconCategoryName!, Guid.Parse(_userContext.Id)));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            IconCategories.Add(new IconCategoryViewModel(new IconCategoryResponse(result.Value, _userContext.Id, IconCategoryName!)));

            IconCategoryName = string.Empty;

            UpdateIconCategoriesView();
        }

        private bool CanSaveIconCategory() => !string.IsNullOrWhiteSpace(IconCategoryName);

        #endregion

        #region Команда [UpdateIconCategoryCommand]: Отвечает за обновление категории

        [RelayCommand(CanExecute = nameof(CanUpdateIconCategorty))]
        private async Task UpdateIconCategorty(IconCategoryViewModel value)
        {
            var result = await _iconCategoryService.UpdatePersonalAsync(new UpdatePersonalIconCategoryRequest(Guid.Parse(value.Id), Guid.Parse(_userContext.Id), value.Name));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            value.Comit();
            UpdateIconCategortyCommand.NotifyCanExecuteChanged();

            foreach (var icon in Icons)
            {
                if (icon.IconCategoryId == value.Id)
                    icon.SetCategory(value);
            }

            UpdateIconsView();
        }

        private bool CanUpdateIconCategorty()
        {
            if (SelectedEditableCategory is null)
                return false;

            return SelectedEditableCategory.HasChanges;
        }

        #endregion

        #region Команда [DeleteIconCategoryCommand] : Отвечает за удаление категории

        [RelayCommand]
        private async Task DeleteIconCategory(IconCategoryViewModel value)
        {
            var result = await _iconCategoryService.DeletePersonalAsync(_userContext.Id, value.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var valueToRemove = IconCategories.FirstOrDefault(ic => ic.Id == value.Id);

            if (valueToRemove is null)
            {
                MessageBox.Show("Элемент уже удален. Если категория не исчезла - то перезапустите приложение.");
                return;
            }

            IconCategories.Remove(valueToRemove);
        }
        

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение данных

        public async Task GetTags()
        {
            var result = await _tagService.GetAll(_userContext.Id); 

            foreach (var item in result.Value)
            {
                Tags.Add(new TagViewModel(item));
            }
        }

        public async Task GetIcons()
        {
            var result = await _iconService.GetAll(_userContext.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            foreach (var item in result.Value)
            {
                var iconVm = new IconViewModel(item);
                iconVm.SetIcon(ConvertSvgInString(iconVm.SvgCode!));
                var ict = IconCategories.FirstOrDefault(ic => ic.Id == item.IconCategoryId);
                iconVm.SetCategory(ict);
                Icons.Add(iconVm);
            }

            UpdateIconsView();
        }

        public async Task GetIconCategories()
        {
            var result = await _iconCategoryService.GetAllAsync(_userContext.Id);

            foreach (var item in result.Value)
            {
                var iconCategoryVM = new IconCategoryViewModel(item);
                IconCategories.Add(iconCategoryVM);
            }    
        }

        #endregion

        #region Преобразование Svg

        //private DrawingImage? ConvertSVG(string? svgCode)
        //{
        //    ArgumentNullException.ThrowIfNull(_defaultIcon, nameof(_defaultIcon));

        //    if (string.IsNullOrWhiteSpace(svgCode))
        //        return _defaultIcon;

        //    if (_iconCache.TryGetValue(svgCode, out var cachedIcon))
        //        return cachedIcon;

        //    var svgString = ReplaceDoubleQuotesWithSingle(svgCode)!;
        //    var newIcon = ConvertSvgInString(svgString);

        //    if (newIcon is null)
        //        return _defaultIcon!;

        //    _iconCache[svgCode] = newIcon;
        //    return newIcon;
        //}

        private DrawingImage? ConvertSvgInString(string svgCode)
        {
            if (string.IsNullOrWhiteSpace(svgCode)) return null;

            var settings = new WpfDrawingSettings
            {
                IncludeRuntime = true,
                TextAsGeometry = true
            };

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgCode));
            FileSvgReader? reader = null;

            try
            {
                reader = new FileSvgReader(settings);
                DrawingGroup drawingGroup = reader!.Read(stream);

                if (drawingGroup != null)
                {
                    var drawingImage = new DrawingImage(drawingGroup);
                    drawingImage.Freeze();
                    return drawingImage;
                }

                return null;
            }
            catch (Exception ex)
            {
                SvgConvertError = ex.Message;
            }

            return null;
        }

        //private void SetDefaultIcon()
        //{
        //    if (_defaultIcon is not null) return;

        //    var defaultIconImage = ConvertSvgInString(DefaultIconConstants.DEFOULT_SECRET_SVG) ?? throw new InvalidOperationException("Не удалось создать дефолтную иконку.");

        //    _defaultIcon = defaultIconImage;

        //    // _iconCache[DefaultIconConstants.DEFOULT_SECRET_SVG] = _defaultIcon;
        //}

        private static string? ReplaceDoubleQuotesWithSingle(string inputString) => inputString?.Replace("\"", "'");

        #endregion

        #region ICollectionView 

        private void UpdateIconsView()
        {
            IconView.SortDescriptions.Clear();
            IconView.GroupDescriptions.Clear();

            IconView.SortDescriptions.Add(new SortDescription(nameof(IconViewModel.IconCategoryName), ListSortDirection.Ascending));
            IconView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IconViewModel.IconCategoryName)));
        }

        private void UpdateIconCategoriesView()
        {
            IconCategoryView.SortDescriptions.Clear();

            IconCategoryView.SortDescriptions.Add(new SortDescription(nameof(IconCategoryViewModel.Name), ListSortDirection.Ascending));
        }

        #endregion
    }
}