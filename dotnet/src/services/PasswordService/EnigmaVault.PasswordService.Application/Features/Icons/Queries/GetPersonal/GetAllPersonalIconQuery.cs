using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetPersonal
{
    public sealed record GetAllPersonalIconQuery(Guid UserId) : IRequest<List<IconResponse>>;
}