using Common.Core.Results;
using LanguageExt.Pipes;
using Shared.Contracts.Requests;
using Shared.Contracts.Responses;
using System.Net.Http.Headers;
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

        public async Task<Result<UserResponse?>> Me(string accesToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);

                var responseMessage = await _httpClient.GetAsync("api/users/me");

                responseMessage.EnsureSuccessStatusCode();
                var userResponse = await responseMessage.Content.ReadFromJsonAsync<UserResponse>(_jsonSerializerOptions);

                return Result<UserResponse?>.Success(userResponse);
            }
            catch (HttpRequestException ex)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка: {ex}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/login: {jsonEx.Message}"));
            }
        }
    }
}