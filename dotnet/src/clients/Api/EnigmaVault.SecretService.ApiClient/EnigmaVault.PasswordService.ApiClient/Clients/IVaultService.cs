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
        Task<Result<List<EncryptedVaultResponse>>> GetAllAsync(string userId);
    }
}