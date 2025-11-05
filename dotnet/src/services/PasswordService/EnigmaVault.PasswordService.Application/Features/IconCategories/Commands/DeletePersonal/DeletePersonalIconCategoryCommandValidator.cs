using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeletePersonal
{
    public sealed class DeletePersonalIconCategoryCommandValidator : AbstractValidator<DeletePersonalIconCategoryCommand>
    {
        public DeletePersonalIconCategoryCommandValidator()
        {
            Include(new GuidValidator());
            Include(new MustUserIdValidator());
        }
    }
}