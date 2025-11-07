namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateTagRequest(string UserId, string Name);
}