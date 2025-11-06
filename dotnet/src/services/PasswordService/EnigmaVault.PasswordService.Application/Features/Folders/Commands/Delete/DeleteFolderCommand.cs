using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Delete
{
    public sealed record DeleteFolderCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId;
}