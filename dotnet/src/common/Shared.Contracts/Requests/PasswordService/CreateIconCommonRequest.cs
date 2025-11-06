namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateIconCommonRequest(string SvgCode, string Name, string IconCategoryId);
}