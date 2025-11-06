namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateIconPersonalRequest(string UserId, string SvgCode, string Name, string IconCategoryId);
}