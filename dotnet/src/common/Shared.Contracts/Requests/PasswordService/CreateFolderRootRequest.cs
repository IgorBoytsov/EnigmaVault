namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateFolderRootRequest(string UserId, string Name, string Color);
}