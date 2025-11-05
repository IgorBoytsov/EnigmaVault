namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateIconCategoryPersonalRequest(string Name, Guid UserId);
}