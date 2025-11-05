using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal
{
    public sealed record GetAllPersonalIconCategories(Guid UserId) : IRequest<List<IconCategoryResponse>>,
        IMustHasUserId;
}