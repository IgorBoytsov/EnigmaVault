using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveFromFavorites
{
    public sealed class RemoveFromFavoritesCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RemoveFromFavoritesCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (maybeVault.IsNone)
                    return Error.NotFound("Vault", request.VaultItemId);

                var vault = maybeVault.Value;

                vault.SetFavorite(false);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Error.Server("Произошла непредвиденная ошибка.");
            }
        }
    }
}