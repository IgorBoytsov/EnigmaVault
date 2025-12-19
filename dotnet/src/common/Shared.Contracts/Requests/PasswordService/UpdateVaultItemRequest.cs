namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateVaultItemRequest(string UserId, string VaultItemId, string EncryptedOverview, string EncryptedDetails);
}