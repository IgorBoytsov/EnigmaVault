using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.ViewModels.Base;
using Shared.Contracts.Enums;
using Shared.Contracts.Responses.PasswordService;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Components.Models.Vaults
{
    public sealed partial class EncryptedVaultViewModel : BaseViewModel
    {
        private readonly EncryptedVaultResponse _model;

        public EncryptedVaultViewModel(EncryptedVaultResponse model, ISecureDataService crypto, byte[] key)
        {
            _model = model;
            IsFavorite = model.IsFavorite;
            IsArchive = model.IsArchive;
            IsInTrash = model.IsInTrash;
            EncryptedOverview = Convert.ToBase64String(model.EncryptedOverview);
            EncryptedDetails = Convert.ToBase64String(model.EncryptedDetails);
            DateAdded = model.DateAdded.ToLocalTime();

            if (model.DateUpdate is not null)
                DateUpdate = model.DateUpdate.Value.ToLocalTime();

            if (model.DeletedAt is not null)
                DeletedAt = model.DeletedAt.Value.ToLocalTime();

            try
            {
                var overview = crypto.DecryptData<OverviewPayload>(EncryptedOverview, key);

                if (overview != null)
                {
                    ServiceName = overview.ServiceName;
                    Url = overview.Url;
                    SvgCode = overview.SvgIcon!;
                }
            }
            catch
            {

            }
        }

        public string Id => _model.Id;

        public VaultType Type => Enum.Parse<VaultType>(_model.Type);

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        private bool _isArchive;

        [ObservableProperty]
        private bool _isInTrash;

        [ObservableProperty]
        private string _encryptedOverview = null!;

        [ObservableProperty]
        private string _encryptedDetails = null!;

        [ObservableProperty]
        private string _serviceName = null!;

        [ObservableProperty]
        private string? _url;

        [ObservableProperty]
        private string _svgCode = null!;

        [ObservableProperty]
        private DrawingImage? _icon;

        [ObservableProperty]
        private DateTime _dateAdded;

        [ObservableProperty]
        private DateTime? _dateUpdate;

        [ObservableProperty]
        private DateTime? _deletedAt;
    }
}