using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.EmptyTrash
{
    public sealed record EmptyVaultsTrashCommand(Guid UserId) : IRequest<Result<Unit>>;
}