using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetAll
{
    public sealed class GetAllIconCategoriesQueryValidator : AbstractValidator<GetAllIconCategoriesQuery>
    {
        public GetAllIconCategoriesQueryValidator() => Include(new MustUserIdValidator());
    }
}