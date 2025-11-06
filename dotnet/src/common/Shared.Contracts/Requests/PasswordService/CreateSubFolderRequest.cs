namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record CreateSubFolderRequest(string UserId, string ParentFolderId, string Name, string Color);
}