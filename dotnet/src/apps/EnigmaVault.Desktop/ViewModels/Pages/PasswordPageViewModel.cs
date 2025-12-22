using Common.Core.Results;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Controller;
using EnigmaVault.Desktop.ViewModels.Components.Models;
using EnigmaVault.Desktop.ViewModels.Components.Models.Vaults;
using EnigmaVault.PasswordService.ApiClient.Clients;
using Shared.Contracts.Enums;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Pages
{
    internal sealed partial class PasswordPageViewModel : BasePageViewModel, IAsyncInitializable, IUpdatable, ISidebarController
    {
        private readonly IVaultService _vaultService;
        private readonly ITagService _tagService;
        private readonly IIconCategoryService _iconCategoryService;
        private readonly IIconService _iconService;
        private readonly IUserContext _userContext;
        private readonly ISecureDataService _secureDataService;

        /*--Инициализация---------------------------------------------------------------------------------*/

        public PasswordPageViewModel(
            IVaultService vaultService,
            ITagService tagService,
            IIconCategoryService iconCategoryService,
            IIconService iconService,
            IUserContext userContext,
            ISecureDataService secureDataService)
        {
            _vaultService = vaultService;
            _tagService = tagService;
            _iconCategoryService = iconCategoryService;
            _iconService = iconService;
            _userContext = userContext;
            _secureDataService = secureDataService;

            SelectedPasswordType = PasswordTypes.FirstOrDefault();
            CurrentDisplayUserControlLeftSideMenu = UserControlsName.Tags;
            CurrentActionRightSideMenu = ActionOnData.Create;

            IconView = CollectionViewSource.GetDefaultView(Icons);
            IconCategoryView = CollectionViewSource.GetDefaultView(IconCategories);

            UpdateIconsView();
            UpdateIconCategoriesView();

            ArchivesPopup = new(PopupPlacementMode.CustomRightUp, PlacementMode.Custom, () => ArchivedPasswords.Count > 0);

            ArchivedPasswords.CollectionChanged += (s, e) =>
            {
                ArchivesPopup?.UpdateCanExecute();
            };
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

                await GetEncreptedOverview();

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

        public ObservableCollection<EncryptedVaultViewModel> Passwords { get; init; } = [];
        public ObservableCollection<EncryptedVaultViewModel> ArchivedPasswords { get; init; } = [];
        public ObservableCollection<TagViewModel> Tags { get; init; } = [];

        public ObservableCollection<IconViewModel> Icons { get; init; } = [];
        public ICollectionView IconView { get; private set; } = null!;

        public ObservableCollection<IconCategoryViewModel> IconCategories { get; init; } = [];
        public ICollectionView IconCategoryView { get; private init; } = null!;

        public ObservableCollection<KeyValuePair<VaultType, string>> PasswordTypes { get; private set; } =
        [
            new KeyValuePair<VaultType, string>(VaultType.Password, "Пароль"),
            new KeyValuePair<VaultType, string>(VaultType.Server, "Данные сервера"),
            new KeyValuePair<VaultType, string>(VaultType.CreditCard, "Банковские карты"),
            new KeyValuePair<VaultType, string>(VaultType.ApiKey, "Апи Ключи"),
        ];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public ToolTipController RightToolTipController { get; } = new(ToolTipPlacement.CenterRight);
        public ToolTipController LeftToolTipController { get; } = new(ToolTipPlacement.CenterLeft);
        public ToolTipController TopToolTipController { get; } = new(ToolTipPlacement.CenterTop);
        public ToolTipController BottomToolTipController { get; } = new(ToolTipPlacement.CenterBottom);

        public PopupController PasswordMenuPopup { get; } = new(); 
        public PopupController AddIconPopup { get; } = new(); 
        public PopupController AddIconCategoryPopup { get; } = new();
        public PopupController ArchivesPopup { get; }

        #region Свойства: [CurrentDisplayUserControlLeftSideMenu, CurrentDisplayUserControlRightSideMenu] - Текущий отображаемый элемент в боковых меню

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetLeftSideMenuControlCommand))]
        private UserControlsName _currentDisplayUserControlLeftSideMenu = UserControlsName.Folders;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetRightSideMenuActionCommand))]
        private ActionOnData _currentActionRightSideMenu = ActionOnData.View;

        partial void OnCurrentActionRightSideMenuChanged(ActionOnData value)
        {
            SetReadOnly(value);

            if (value == ActionOnData.Create || value == ActionOnData.Update)
                SelectedIcon = Icons.FirstOrDefault(i => i.SvgCode == SelectedEncryptedOverview?.SvgCode);
            else
                SelectedIcon = null;
        }

        #endregion

        #region Свойства: Tags, Метод: [OnSelectedTagChanged]

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

        #region Свойство: [SelectedIcon]

        [ObservableProperty]
        private IconViewModel? _selectedIcon;

        partial void OnSelectedIconChanged(IconViewModel? value)
        {
            if (value is null)
                return;

            if (CurrentActionRightSideMenu is ActionOnData.Create || CurrentActionRightSideMenu is ActionOnData.Update)
            {
                if (SelectedVaultItemBaseViewModel is null)
                    return;

                string? code = SelectedIcon?.SvgCode;
                SelectedVaultItemBaseViewModel.SvgCode = code;
                SelectedVaultItemBaseViewModel?.SetIcon(ConvertSvgInString(code!));
            }
        }

        #endregion

        #region Свойства: IconCategories, Методы: [OnSelectedEditableCategoryChanging, OnSelectedEditableCategoryChanged, OnCategoryPropertyChanged]

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveIconCategoryCommand))]
        private string? _iconCategoryName;

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

        #region Свойсто: [SelectedEncryptedOverview] - Выбор зашифрованного элемента

        [ObservableProperty]
        private EncryptedVaultViewModel? _selectedEncryptedOverview;

        partial void OnSelectedEncryptedOverviewChanged(EncryptedVaultViewModel? value)
        {
            if (value is null)
                return;

            CreateViewModelForType(value.Type, value);
            SelectedPasswordType = PasswordTypes.FirstOrDefault(pt => pt.Key == value.Type);
            CurrentActionRightSideMenu = ActionOnData.View;
            SetReadOnly(CurrentActionRightSideMenu);
            SelectedVaultItemBaseViewModel?.Decrypt(value.EncryptedOverview, value.EncryptedDetails, _secureDataService, _userContext);
            SelectedVaultItemBaseViewModel?.SetIcon(ConvertSvgInString(value.SvgCode!));
        }

        #endregion

        #region Свойсто: [SelectedArchivedEncryptedOverview] - Выбор зашифрованного элемента в архиве

        [ObservableProperty]
        private EncryptedVaultViewModel? _selectedArchivedEncryptedOverview;

        #endregion

        #region Свойство: [SelectedPasswordType], Метод [OnSelectedPasswordTypeChanged]

        [ObservableProperty]
        private KeyValuePair<VaultType, string> _selectedPasswordType;

        partial void OnSelectedPasswordTypeChanged(KeyValuePair<VaultType, string> value)
        {
            CreateViewModelForType(value.Key, SelectedEncryptedOverview!);
            SetReadOnly(CurrentActionRightSideMenu);
        }

        #endregion

        #region Свойсто: [SelectedPasswordViewModel]

        [ObservableProperty]
        private VaultItemBaseViewModel? _selectedVaultItemBaseViewModel;

        partial void OnSelectedVaultItemBaseViewModelChanged(VaultItemBaseViewModel? value)
        {
            value?.SetIsReadOnly(CurrentActionRightSideMenu != ActionOnData.View);
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

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команда [CreateVault]: Создание зашифрованного элемента

        [RelayCommand]
        public async Task CreateVault()
        {
            (string EncryptedOverView, string EncryptedDetails) = SelectedVaultItemBaseViewModel!.Encrypt(_secureDataService, _userContext);

            var result = await _vaultService.CreateAsync(new CreateVaultItemRequest(_userContext.Id, SelectedPasswordType.Key.ToString(), EncryptedOverView, EncryptedDetails));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            var encryptedVm = new EncryptedVaultViewModel(
                    new EncryptedVaultResponse(
                        result.Value,
                        SelectedPasswordType.Key.ToString(),
                        DateTime.Now,
                        DateUpdate: null,
                        DeletedAt: null,
                        IsFavorite: false,
                        IsArchive: false,
                        IsInTrash: false,
                        Convert.FromBase64String(EncryptedOverView),
                        Convert.FromBase64String(EncryptedDetails)),
                    _secureDataService, _userContext.Dek);

            encryptedVm.Icon = ConvertSvgInString(encryptedVm.SvgCode);

            Passwords.Add(encryptedVm);
        }

        #endregion

        #region Команда [UpdateVault]: Обноволение записи

        [RelayCommand(CanExecute = nameof(CanUpdateVault))]
        private async Task UpdateVault()
        {
            (string EncryptedOverView, string EncryptedDetails) = SelectedVaultItemBaseViewModel!.Encrypt(_secureDataService, _userContext);

            var result = await _vaultService.UpdateAsync(new UpdateVaultItemRequest(_userContext.Id, SelectedEncryptedOverview!.Id, EncryptedOverView, EncryptedDetails));

            if (result.IsFailure)
            {
                MessageBox.Show($"{result.StringMessage}");
                return;
            }

            SelectedEncryptedOverview!.UpdateEncrypted(EncryptedOverView, EncryptedDetails);
            SelectedEncryptedOverview!.UpdateDate(DateTime.Parse(result.Value).ToLocalTime());
        }

        private bool CanUpdateVault() => SelectedEncryptedOverview is not null;

        #endregion

        #region Команда [SetFavorite]: Измнение статуса избранного

        [RelayCommand(CanExecute = nameof(CanSetFavorite))]
        private async Task SetFavorite(EncryptedVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsFavorite = condition;
            }

            if (model.IsFavorite)
            {
                var result = await _vaultService.RemoveFromFavoritesAsync(_userContext.Id, model.Id);
                SetValue(result, false);
            }
            else
            {
                var result = await _vaultService.AddToFavoritesAsync(_userContext.Id, model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetFavorite(EncryptedVaultViewModel model) => model is not null;

        #endregion

        #region Команда [SetArchive]: Измнение статуса архивации

        [RelayCommand(CanExecute = nameof(CanSetArchive))]
        private async Task SetArchive(EncryptedVaultViewModel model)
        {
            void SetValue(Result<Unit> result, bool condition)
            {
                if (result.IsFailure)
                {
                    MessageBox.Show($"{result.StringMessage}");
                    return;
                }

                model!.IsArchive = condition;

                if (condition)
                {
                    Passwords.Remove(model);
                    ArchivedPasswords.Add(model);
                }
                else
                {
                    ArchivedPasswords.Remove(model);
                    Passwords.Add(model);
                }

                PasswordMenuPopup.HideCommand.Execute(null);
            }

            if (model.IsArchive)
            {
                var result = await _vaultService.UnArchiveAsync(_userContext.Id, model.Id);
                SetValue(result, false);

                if (ArchivedPasswords.Count <= 0)
                    ArchivesPopup.HideCommand.Execute(null);
            }
            else
            {
                var result = await _vaultService.ArchiveAsync(_userContext.Id, model.Id);
                SetValue(result, true);
            }
        }

        private bool CanSetArchive(EncryptedVaultViewModel model) => model is not null;

        #endregion

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

        #region Команда [SetLeftSideMenuControlCommand]: Отвечает за выбор текущего оборажаемого контрола на левой боковой понели

        [RelayCommand(CanExecute = nameof(CanSetLeftSideMenuControl))]
        private void SetLeftSideMenuControl(UserControlsName controlName) => CurrentDisplayUserControlLeftSideMenu = controlName;

        private bool CanSetLeftSideMenuControl(UserControlsName controlsName) => CurrentDisplayUserControlLeftSideMenu != controlsName;

        #endregion

        #region Команда [SetRightSideMenuActionCommand]: Отвечает за выбор текущего действия на правой боковой понели

        [RelayCommand(CanExecute = nameof(CanSetRightSideMenuAction))]
        private void SetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu = action;

        private bool CanSetRightSideMenuAction(ActionOnData action) => CurrentActionRightSideMenu != action;

        #endregion

        #region Команда [SelectAndShowPasswordMenuPopup]: Отвечает за выбор элемента списка при открытие контекстного меню 

        [RelayCommand]
        private void SelectAndShowPasswordMenuPopup(EncryptedVaultViewModel password)
        {
            if (password is null) return;

            SelectedEncryptedOverview = password;

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
            var iconVm = new IconViewModel(new IconResponse(result.Value, _userContext.Id, SvgCode!, "", SelectedIconCategory.Id));
            iconVm.SetIcon(ConvertSvgInString(iconVm.SvgCode!));
            var ict = IconCategories.FirstOrDefault(ic => ic.Id == SelectedIconCategory.Id);
            iconVm.SetCategory(ict);

            Icons.Add(iconVm);
           
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

        public async Task GetEncreptedOverview()
        {
            var result = await _vaultService.GetAllAsync(_userContext.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            foreach (var encrypted in result.Value)
            {
                var encryptedVm = new EncryptedVaultViewModel(encrypted, _secureDataService, _userContext.Dek);
                encryptedVm.Icon = ConvertSvgInString(encryptedVm.SvgCode!);

                if (encrypted.IsArchive)
                {
                    ArchivedPasswords.Add(encryptedVm);
                    continue;
                }
                
                Passwords.Add(encryptedVm);
            }
        }

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

        private void CreateViewModelForType(VaultType type, EncryptedVaultViewModel encryptedVm)
        {
            var encrypted = encryptedVm;

            encrypted ??= new(new EncryptedVaultResponse(string.Empty, SelectedPasswordType.Key.ToString(), DateTime.UtcNow, null, null, false, false, false, [], []), _secureDataService, _userContext.Dek);

            SelectedVaultItemBaseViewModel = type switch
            {
                VaultType.Password => new StandardPasswordViewModel(encrypted),
                VaultType.Server => new ServerPasswordViewModel(encrypted),
                VaultType.ApiKey => new ApiKeyViewModel(encrypted),
                VaultType.CreditCard => new CreditCardViewModel(encrypted),
                _ => null,
            };
        }

        #region Управление правым боковым меню

        [ObservableProperty]
        private double _rightSidebarWidth = 250;

        [ObservableProperty]
        private bool _isSidebarOpen;

        public void ToggleSidebar()
        {
            IsSidebarOpen = !IsSidebarOpen;

            if (IsSidebarOpen)
                RightSidebarWidth = 250;
            else
                RightSidebarWidth = 0;
        }

        #endregion

        private void SetReadOnly(ActionOnData action)
        {
            if (action == ActionOnData.View)
                SelectedVaultItemBaseViewModel?.SetIsReadOnly(true);
            else
                SelectedVaultItemBaseViewModel?.SetIsReadOnly(false);
        }
    }
}