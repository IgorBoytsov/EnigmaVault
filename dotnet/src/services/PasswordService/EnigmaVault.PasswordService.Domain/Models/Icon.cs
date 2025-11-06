using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace EnigmaVault.PasswordService.Domain.Models
{
    public sealed class Icon : AggregateRoot<IconId>
    {
        public UserId? UserId { get; set; }
        public SvgCode SvgCode { get; set; }
        public IconName IconName { get; set; }
        public IconCategoryId IconCategoryId { get; set; }

        private Icon() { }

        private Icon(IconId id, IconName name, SvgCode svgCode, IconCategoryId iconCategoryId) : base(id)
        {
            IconName = name;
            SvgCode = svgCode;
            IconCategoryId = iconCategoryId;
        }

        public static Icon Create(string name, Guid? userId, string svgCode, Guid iconCategoryId)
        {
            var entity = new Icon(IconId.New(), IconName.Create(name), SvgCode.Create(svgCode), IconCategoryId.Create(iconCategoryId));

            if (userId.HasValue)
                entity.UserId = ValueObjects.User.UserId.Create(userId.Value);

            return entity;
        }

        public void UpdateName(IconName iconCategoryName)
        {
            if (IconName != iconCategoryName)
                IconName = iconCategoryName;
        }

        public void UpdateCategory(IconCategoryId iconCategoryId)
        {
            if (IconCategoryId != iconCategoryId)
                IconCategoryId = iconCategoryId;
        }

        public void UpdateSvg(SvgCode svgCode)
        {
            if (SvgCode != svgCode)
                SvgCode = svgCode;
        }
    }
}