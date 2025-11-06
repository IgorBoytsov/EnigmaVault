using EnigmaVault.PasswordService.Application.Features.Validators;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Validators
{
    public sealed class IconNameValidator : AbstractValidator<IHasName>
    {
        public IconNameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название иконки не может быть пустым.")
                .Length(IconName.MIN_LENGTH, IconName.MAX_LENGTH).WithMessage($"Допустимый диапазон длинны название от {IconName.MIN_LENGTH} до {IconName.MAX_LENGTH} символов.");
        }
    }
}