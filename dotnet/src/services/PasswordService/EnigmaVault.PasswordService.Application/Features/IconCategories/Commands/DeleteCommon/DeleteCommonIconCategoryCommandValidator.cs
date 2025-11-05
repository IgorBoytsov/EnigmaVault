using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeleteCommon
{
    public sealed class DeleteCommonIconCategoryCommandValidator : AbstractValidator<DeleteCommonIconCategoryCommand>
    {
        public DeleteCommonIconCategoryCommandValidator() => Include(new GuidValidator());
    }
}