using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddTag
{
    public sealed record AddTagToVaulItemCommand(Guid UserId, Guid VaultItemId, Guid TagId) : IRequest<Result<Unit>>;
}