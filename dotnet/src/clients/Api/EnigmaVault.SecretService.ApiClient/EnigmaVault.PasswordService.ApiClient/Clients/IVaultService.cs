using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public interface IVaultService
    {
        Task<Result<string>> CreateAsync(CreateVaultItemRequest request);
        Task<Result<string>> UpdateAsync(UpdateVaultItemRequest request);
        Task<Result<Unit>> AddToFavoritesAsync(string userId, string vaultId);
        Task<Result<Unit>> RemoveFromFavoritesAsync(string userId, string vaultId);
        Task<Result<Unit>> ArchiveAsync(string userId, string vaultId);
        Task<Result<Unit>> UnArchiveAsync(string userId, string vaultId);
        Task<Result<Unit>> DeleteAsync(string userId, string vaultId);
        Task<Result<DateTime>> MoveToTrashAsync(string userId, string vaultId);
        Task<Result<Unit>> RestoreFromTrashAsync(string userId, string vaultId);
        Task<Result<Unit>> RestoreAllFromTrashAsync(string userId);
        Task<Result<Unit>> EmptyTrashAsync(string userId);
        Task<Result<List<EncryptedVaultResponse>>> GetAllAsync(string userId);
        Task<Result<Unit>> AddTagAsync(string userId, string vaultId, string tagId);
        Task<Result<Unit>> RemoveTagAsync(string userId, string vaultId, string tagId);
    }
}