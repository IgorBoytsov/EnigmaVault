namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdatePersonalIconCategoryRequest(Guid Id, Guid UserId, string Name);
}