using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public interface IVaultService
    {
        Task<Result<string>> CreateAsync(CreateVaultItemRequest request);
    }
}