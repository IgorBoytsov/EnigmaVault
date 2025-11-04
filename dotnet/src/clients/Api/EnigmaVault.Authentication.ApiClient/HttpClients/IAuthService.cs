using Common.Core.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Responses;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public interface IAuthService
    {
        Task<Result<AuthResponse?>> Login(LoginRequest request);
        Task<Result<AuthResponse?>> LoginByToken(LoginByTokenRequest request);
        Task<Result<AuthResponse?>> Refresh(RefreshTokenRequest request);
    }
}