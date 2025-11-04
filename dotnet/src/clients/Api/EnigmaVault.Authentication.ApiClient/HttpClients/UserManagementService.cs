using Common.Core.Results;
using Shared.Contracts.Requests;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public sealed class UserManagementService(HttpClient httpClient) : IUserManagementService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<string?>> Register(RegisterUserRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();

                return responseData;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }

        public async Task<Result<string?>> RecoveryAccess(RecoveryAccessRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/recovery-access", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();

                return responseData;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}");
            }
        }
    }
}