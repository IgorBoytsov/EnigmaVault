using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreFromTrash
{
    public sealed class RestoreVaultFromTrashCommandHandler(
        IVaultItemRepository vaultItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RestoreVaultFromTrashCommand, Result<Unit>>
    {
        private readonly IVaultItemRepository _vaultItemRepository = vaultItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(RestoreVaultFromTrashCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeVault = await _vaultItemRepository.GetAsync(request.VaultItemId, request.UserId, cancellationToken);

                if (maybeVault.IsNone)
                    return Error.NotFound("Данные не найдены.");

                var vaultItem = maybeVault.Value;

                vaultItem.SetInTrash(false);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                return Error.Server("Не удалось востановить данные из корзины.");
            }
        }
    }
}