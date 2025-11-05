using EnigmaVault.PasswordService.Application.Features.Validators;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreatePersonal
{
    public sealed class CreatePersonalCategoryCommandValidator : AbstractValidator<CreatePersonalCategoryCommand>
    {
        public CreatePersonalCategoryCommandValidator()
        {
            Include(new IconCategoryNameValidator());
            Include(new MustUserIdValidator());
        }
    }
}