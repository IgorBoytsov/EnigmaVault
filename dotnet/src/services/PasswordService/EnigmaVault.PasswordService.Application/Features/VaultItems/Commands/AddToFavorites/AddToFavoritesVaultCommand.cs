using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddToFavorites
{
    public sealed record AddToFavoritesVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}