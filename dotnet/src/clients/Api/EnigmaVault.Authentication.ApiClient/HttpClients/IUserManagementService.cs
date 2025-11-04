using Common.Core.Results;
using Shared.Contracts.Requests;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public interface IUserManagementService
    {
        Task<Result<string?>> Register(RegisterUserRequest request);
        Task<Result<string?>> RecoveryAccess(RecoveryAccessRequest request);
    }
}