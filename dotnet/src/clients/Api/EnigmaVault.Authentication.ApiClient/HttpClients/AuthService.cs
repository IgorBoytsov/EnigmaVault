using Common.Core.Results;
using Shared.Contracts.Requests;
using Shared.Contracts.Responses;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.Authentication.ApiClient.HttpClients
{
    public sealed class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<AuthResponse?>> Login(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthResponse>();

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

        public async Task<Result<AuthResponse?>> LoginByToken(LoginByTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/token-login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthResponse>();

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

        public async Task<Result<AuthResponse?>> Refresh(RefreshTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/token-login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthResponse>();

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