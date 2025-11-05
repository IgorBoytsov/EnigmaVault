using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetAll
{
    public sealed record GetAllIQuery(Guid UserId) : IRequest<List<IconCategoryResponse>>;
}