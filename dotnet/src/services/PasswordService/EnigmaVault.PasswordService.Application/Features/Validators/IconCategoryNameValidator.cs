using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Validators
{
    public sealed class IconCategoryNameValidator : AbstractValidator<IHasName>
    {
        public IconCategoryNameValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty().WithMessage("ОШИБКА ИЗ ВАЛИДАТОРА Название не может быть пустым.")
               .Length(IconCategoryName.MIN_LENGTH, IconCategoryName.MAX_LENGTH).WithMessage($"Допустимый диапазон для название от {IconCategoryName.MIN_LENGTH} до {IconCategoryName.MAX_LENGTH} символов.");
        }
    }
}