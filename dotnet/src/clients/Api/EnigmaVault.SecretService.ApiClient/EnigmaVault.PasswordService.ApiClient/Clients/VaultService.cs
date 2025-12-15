using System.Net.Http;
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
    }
}