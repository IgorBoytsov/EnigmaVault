namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdatePersonalIconRequest(string Id, string UserId, string Name, string SvgCode, string IconCategoryId);
}