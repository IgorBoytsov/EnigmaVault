using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal
{
    public sealed class GetAllPersonalIconCategoriesValidator : AbstractValidator<GetAllPersonalIconCategories>
    {
        public GetAllPersonalIconCategoriesValidator() => Include(new MustUserIdValidator());
    }
}