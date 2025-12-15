using Common.Core.Guard;
using Common.Core.Results;
using EnigmaVault.PasswordService.Domain.Enums;
using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace EnigmaVault.PasswordService.Domain.Models
{
    public sealed class VaultItem : AggregateRoot<VaultItemId>
    {
        public UserId UserId { get; private set; }
        public VaultType PasswordType { get; private set; }

        public EncryptedData EncryptedOverview { get; private set; }
        public EncryptedData EncryptedDetails { get; private set; }

        public bool IsFavorite { get; private set; }
        public bool IsArchive { get; private set; }
        public bool IsInTrash { get; private set; }

        public DateTime? DeletedAt { get; private set; } 
        public DateTime DateAdded { get; private set; }
        public DateTime? DateUpdated { get; private set; }

        private VaultItem() { }

        private VaultItem(VaultItemId id, UserId userId, VaultType type, EncryptedData encryptedOverview, EncryptedData encryptedDetails, bool isFavorite) : base(id)
        {
            UserId = userId;
            PasswordType = type;
            EncryptedOverview = encryptedOverview;
            EncryptedDetails = encryptedDetails;
            IsFavorite = isFavorite;
        }

        public static VaultItem Create(UserId UserId, VaultType type, EncryptedData encryptedOverview, EncryptedData encryptedDetails, bool isFavorite = false)
        {
            return new VaultItem(
                VaultItemId.New(),
                UserId,
                type,
                encryptedOverview,
                encryptedDetails,
                isFavorite)
            {
                DateAdded = DateTime.UtcNow,
            };
        }

        public void UpdateOverview(EncryptedData encryptedOverview)
        {
            EncryptedOverview = encryptedOverview;
            UpdateDate();
        }

        public void UpdateDetails(EncryptedData encryptedDetails)
        {
            EncryptedDetails = encryptedDetails;
            UpdateDate();
        }

        public void SetFavorite(bool isFavorite)
        {
            if (IsFavorite == isFavorite) return;

            IsFavorite = isFavorite;
        }

        public void SetArchive(bool isFavorite)
        {
            if (IsFavorite == isFavorite)
                return;
            
            Guard.Against.That(isFavorite && IsInTrash, () => new DomainException(Error.Validation("Нельзя архивировать запись, находящуюся в корзине")));

            IsFavorite = isFavorite;
            UpdateDate();
        }

        public void SetInTrash(bool isInTrash)
        {
            if (IsInTrash == isInTrash)
                return;

            if (isInTrash)
            {
                IsInTrash = true;
                DeletedAt = DateTime.UtcNow.AddDays(30);
                IsArchive = false; 
            }
            else
            {
                IsInTrash = false;
                DeletedAt = null;
            }
            IsInTrash = isInTrash;
            UpdateDate();
        }

        private void UpdateDate() => DateUpdated = DateTime.UtcNow;

    }
}