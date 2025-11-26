using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Responses.PasswordService;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Components.Models
{
    internal sealed partial class IconViewModel(IconResponse model) : BaseViewModel
    {
        private readonly IconResponse _model = model;

        public string? Id => _model.Id;
        public string? SvgCode => _model.SvgCode;
        public string? IconCategoryId => _model.IconCategoryId;
        public string? IconCategoryName => Category != null ? Category.Name : "Без категории";

        [ObservableProperty]
        private string? _name = model.IconName;

        [ObservableProperty]
        private DrawingImage? _icon;

        [ObservableProperty]
        private IconCategoryViewModel? _category;

        //[ObservableProperty]
        //private string? _iconCategoryName;

        public void SetIcon(DrawingImage? icon) => Icon = icon;

        public void SetCategory(IconCategoryViewModel? category) => Category = category;
    }
}