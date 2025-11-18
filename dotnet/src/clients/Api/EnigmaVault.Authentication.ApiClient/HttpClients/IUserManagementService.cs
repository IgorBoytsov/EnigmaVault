using Common.Core.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Responses;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public interface IUserManagementService
    {
        Task<Result<string?>> Register(RegisterUserRequest request);
        Task<Result<string?>> RecoveryAccess(RecoveryAccessRequest request);
        Task<Result<UserResponse?>> Me(string accesToken);
    }
}