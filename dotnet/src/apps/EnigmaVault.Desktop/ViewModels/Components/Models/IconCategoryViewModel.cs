using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.Desktop.ViewModels.Components.Models
{
    internal sealed partial class IconCategoryViewModel(IconCategoryResponse model) : BaseViewModel
    {
        private IconCategoryResponse _model = model;

        public string Id => _model.Id;
        public string UserId => _model.UserId!;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasChanges))]
        private string _name = model.Name;

        public bool HasChanges => _model.Name != Name;

        public bool HasUserId => string.IsNullOrWhiteSpace(_model.UserId);

        public void Revers() => this.Name = _model.Name;

        public void Comit()
        {
            _model = _model with { Name = this.Name };
            OnPropertyChanged(nameof(HasChanges));    
        }
    }
}