using Common.Core.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Update
{
    public sealed record UpdateVaultItemCommand(Guid UserId, Guid VaultItemId, byte[] EncryptedOverview, byte[] EncryptedDetails) : IRequest<Result<string>>;
}