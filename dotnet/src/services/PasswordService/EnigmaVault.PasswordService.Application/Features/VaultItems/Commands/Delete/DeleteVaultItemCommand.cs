using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Delete
{
    public sealed record DeleteVaultItemCommand(Guid UserId, Guid VaultItemId) : IRequest<Result<Unit>>;
}