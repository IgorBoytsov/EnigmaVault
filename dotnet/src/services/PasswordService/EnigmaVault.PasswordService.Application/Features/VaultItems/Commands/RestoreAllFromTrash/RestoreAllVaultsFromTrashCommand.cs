using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreAllFromTrash
{
    public sealed record RestoreAllVaultsFromTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}