using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveFromFavorites
{
    public sealed record RemoveFromFavoritesCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}