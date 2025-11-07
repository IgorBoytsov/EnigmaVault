using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create
{
    public sealed record CreateTagCommand(Guid UserId, string Name) : IRequest<Result<Unit>>,
        IMustHasUserId,
        IHasName;
}