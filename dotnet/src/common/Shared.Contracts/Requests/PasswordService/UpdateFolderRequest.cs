namespace Shared.Contracts.Requests.PasswordService
{
    public sealed record UpdateFolderRequest(string Id, string UserId, string? ParentFolderId, string Name);
}