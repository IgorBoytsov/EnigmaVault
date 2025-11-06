using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetAll
{
    public sealed record GetAllIconQuery(Guid UserId) : IRequest<List<IconResponse>>;
}