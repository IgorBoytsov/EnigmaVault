using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreFromTrash
{
    public sealed record RestoreVaultFromTrashCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}