using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using Shared.Contracts.Enums;
using Shared.Contracts.Responses.PasswordService;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault
{
    public sealed partial class CredentialsVaultViewModel : BaseViewModel
    {
        private readonly EncryptedVaultResponse _model;
        private readonly ISecureDataService _crypto;
        private readonly byte[] _key;

        public CredentialsVaultViewModel(EncryptedVaultResponse model, ISecureDataService crypto, byte[] key, IReadOnlyCollection<TagViewModel> tags)
        {
            _model = model;
            _crypto = crypto;
            _key = key;
            _tags = tags;

            Tags = new ObservableCollection<TagViewModel>(model.TagsIds.Select(id => tags.FirstOrDefault(t => t.Id == id)).Where(t => t is not null)!);
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


            Decrypt(EncryptedOverview);
        }

        private IReadOnlyCollection<TagViewModel> _tags;

        public string Id => _model.Id;

        public VaultType Type => Enum.Parse<VaultType>(_model.Type);

        public ObservableCollection<TagViewModel> Tags { get; private set; }

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

        public void UpdateEncrypted(string encryptedOverview, string encryptedDetails)
        {
            EncryptedOverview = encryptedOverview;
            EncryptedDetails = encryptedDetails;

            Decrypt(encryptedOverview);
        }

        public void UpdateDate(DateTime dateUpdate) => DateUpdate = dateUpdate;

        public void AddTag(string tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);

            if (tag != null && !Tags.Contains(tag))
                Tags.Add(tag);
        }

        public void RemoveTag(string tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);

            if (tag != null && Tags.Contains(tag))
                Tags.Remove(tag);
        }

        private void Decrypt(string encryptedOverview)
        {

            try
            {
                var overview = _crypto.DecryptData<OverviewPayload>(encryptedOverview, _key);

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
    }
}