using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeletePersonal
{
    public sealed record DeletePersonalIconCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}