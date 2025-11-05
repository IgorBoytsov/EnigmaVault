using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreateCommon
{
    public sealed class CreateCommonIconCategoryCommandValidator : AbstractValidator<CreateCommonIconCategoryCommand>
    {
        public CreateCommonIconCategoryCommandValidator()
            => Include(new IconCategoryNameValidator());
    }
}