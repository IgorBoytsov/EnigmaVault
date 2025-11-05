using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace EnigmaVault.PasswordService.Domain.Models
{
    public sealed class IconCategory : AggregateRoot<IconCategoryId>
    {
        public UserId? UserId { get; set; }
        public IconCategoryName Name { get; set; }

        private IconCategory() { }

        private IconCategory(IconCategoryId id, IconCategoryName name) : base(id)
        {
            Name = name;
        }

        public static IconCategory Create(string name, Guid? userId)
        {
            var entity = new IconCategory(IconCategoryId.New(), IconCategoryName.Create(name));

            if (userId.HasValue)
                entity.UserId = ValueObjects.User.UserId.Create(userId.Value);

            return entity;
        }

        public void UpdateName(IconCategoryName iconCategoryName)
        {
            if(Name != iconCategoryName)
                Name = iconCategoryName;
        }
    }
}