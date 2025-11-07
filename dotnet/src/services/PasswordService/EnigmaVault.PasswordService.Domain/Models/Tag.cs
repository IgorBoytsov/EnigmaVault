using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace EnigmaVault.PasswordService.Domain.Models
{
    public sealed class Tag : AggregateRoot<TagId>
    {
        public UserId UserId { get; private set; }
        public TagName Name { get; private set; }

        private Tag() { }

        private Tag(TagId id, UserId userId, TagName name) : base(id)
        {
            UserId = userId;
            Name = name;
        }

        public static Tag Create(Guid userId, string name)
            => new(TagId.New(), UserId.Create(userId), TagName.Create(name));

        public void UpdateName(TagName tagName)
        {
            if (Name != tagName)
                Name = tagName;
        }
    }
}