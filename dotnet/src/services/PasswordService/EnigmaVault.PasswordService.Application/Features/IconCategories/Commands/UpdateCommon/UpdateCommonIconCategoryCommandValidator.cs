using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdateCommon
{
    public sealed class UpdateCommonIconCategoryCommandValidator : AbstractValidator<UpdateCommonIconCategoryCommand>
    {
        public UpdateCommonIconCategoryCommandValidator()
        {
            Include(new GuidValidator());
            Include(new IconCategoryNameValidator());
        }
    }
}