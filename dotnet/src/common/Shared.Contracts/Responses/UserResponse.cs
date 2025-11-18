namespace Shared.Contracts.Responses
{
    public sealed record UserResponse(string Id, string Login, string PasswordHash, List<string> Roles);
}