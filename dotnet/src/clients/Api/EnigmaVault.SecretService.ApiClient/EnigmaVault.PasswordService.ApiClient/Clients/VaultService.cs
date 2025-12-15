using Common.Core.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.PasswordService.ApiClient.Clients
{
    public sealed class VaultService(HttpClient client) : IVaultService
    {
        private readonly HttpClient _httpClient = client;
        private readonly string _url = "api/vault";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<string>> CreateAsync(CreateVaultItemRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_url, request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync() ?? "";
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.ToString());
            }
        }

        public async Task<Result<List<EncryptedVaultResponse>>> GetAllAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_url}/{userId}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<EncryptedVaultResponse>>(_jsonSerializerOptions) ?? [];
            }
            catch (Exception ex)
            {
                return Error.New(ErrorCode.ApiError, ex.Message);
            }
        }
    }
}