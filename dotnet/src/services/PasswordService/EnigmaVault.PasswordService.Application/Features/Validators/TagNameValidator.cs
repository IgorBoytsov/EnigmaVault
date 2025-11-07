using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Validators
{
    public sealed class TagNameValidator : AbstractValidator<IHasName>
    {
        public TagNameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("У тэга должно быть название.")
                .Length(TagName.MIN_LENGTH, TagName.MAX_LENGTH).WithMessage($"Диапазон для название от {TagName.MIN_LENGTH} до {TagName.MAX_LENGTH} символов");
        }
    }
}