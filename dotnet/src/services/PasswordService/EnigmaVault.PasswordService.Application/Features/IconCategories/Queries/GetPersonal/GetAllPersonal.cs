using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal
{
    public sealed record GetAllPersonal(Guid UserId) : IRequest<List<IconCategoryResponse>>;
}