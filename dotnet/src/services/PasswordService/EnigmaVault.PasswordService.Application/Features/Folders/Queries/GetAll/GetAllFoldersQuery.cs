using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Queries.GetAll
{
    public sealed record GetAllFoldersQuery(Guid UserId) : IRequest<List<FolderResponse>>; 
}