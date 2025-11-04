namespace Shared.Contracts.Requests
{
    public sealed record RecoveryAccessRequest(string Login, string Email, string NewPassword);
}