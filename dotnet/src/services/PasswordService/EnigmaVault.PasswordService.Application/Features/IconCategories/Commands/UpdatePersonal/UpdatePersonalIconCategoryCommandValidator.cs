using EnigmaVault.PasswordService.Application.Features.Validators;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalIconCategoryCommandValidator : AbstractValidator<UpdatePersonalIconCategoryCommand>
    {
        public UpdatePersonalIconCategoryCommandValidator()
        {
            Include(new GuidValidator());
            Include(new IconCategoryNameValidator());
        }
    }
}