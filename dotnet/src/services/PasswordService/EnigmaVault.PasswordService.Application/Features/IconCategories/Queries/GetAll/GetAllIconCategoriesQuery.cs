using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetAll
{
    public sealed record GetAllIconCategoriesQuery(Guid UserId) : IRequest<List<IconCategoryResponse>>,
        IMustHasUserId;
}