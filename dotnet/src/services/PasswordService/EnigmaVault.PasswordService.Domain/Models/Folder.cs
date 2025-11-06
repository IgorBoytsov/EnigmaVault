using Common.Core.Guard;
using Common.Core.Results;
using EnigmaVault.PasswordService.Domain.ValueObjects.Common;
using EnigmaVault.PasswordService.Domain.ValueObjects.Folder;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace EnigmaVault.PasswordService.Domain.Models
{
    public sealed class Folder : AggregateRoot<FolderId>
    {
        public UserId UserId { get; private set; }
        public FolderId? ParentFolderId { get; private set; }
        public FolderName FolderName { get; private set; }
        public Color Color { get; private set; }

        private Folder() { }

        private Folder(FolderId id, FolderId? parentFolderId, UserId userId, FolderName folderName, Color color) : base(id)
        {
            UserId = userId;
            FolderName = folderName;
            ParentFolderId = parentFolderId;
            Color = color;
        }

        public static Folder CreateRoot(Guid userId, string folderName, string color)
            => new(FolderId.New(), null, UserId.Create(userId), FolderName.Create(folderName), Color.FromHex(color));

        public static Folder CreateSubfolder(Guid userId, Guid parentFolderId, string folderName, string color)
            => new(FolderId.New(), FolderId.Create(parentFolderId), UserId.Create(userId), FolderName.Create(folderName), Color.FromHex(color));

        public void Move(FolderId? newParentId)
        {
            Guard.Against.That(newParentId == this.Id, () => new DomainException(Error.New(ErrorCode.Rule, "Папку нельзя перемещать в саму себя.")));

            ParentFolderId = newParentId;
        }

        public void Rename(FolderName folderName)
        {
            if (FolderName != folderName)
                FolderName = folderName;
        }
    }
}