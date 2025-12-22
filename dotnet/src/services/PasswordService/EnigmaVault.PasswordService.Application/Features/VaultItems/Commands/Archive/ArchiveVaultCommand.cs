using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Archive
{
    public sealed record ArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}