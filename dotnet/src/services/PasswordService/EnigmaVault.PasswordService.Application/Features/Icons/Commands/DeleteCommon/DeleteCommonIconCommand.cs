using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeleteCommon
{
    public sealed record DeleteCommonIconCommand(Guid Id) : IRequest<Result<Unit>>;
}