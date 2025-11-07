using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Queries.GetAll
{
    public sealed record GetAllTagsQuery(Guid UserId) : IRequest<List<TagResponse>>;
}