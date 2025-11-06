using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Folders.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateSubFolder
{
    public sealed record CreateSubFolderCommand(Guid UserId, Guid ParentFolderId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName,
        IHasHexColor,
        IMustHasUserId;
}