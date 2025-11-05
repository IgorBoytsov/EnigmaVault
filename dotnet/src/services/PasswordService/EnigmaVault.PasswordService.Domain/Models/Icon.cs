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
    }
}