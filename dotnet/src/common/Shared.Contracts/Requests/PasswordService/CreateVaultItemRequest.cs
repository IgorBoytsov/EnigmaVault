namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateVaultItemRequest(string UserId, string PasswordType, string EncryptedOverview, string EncryptedDetails);
}