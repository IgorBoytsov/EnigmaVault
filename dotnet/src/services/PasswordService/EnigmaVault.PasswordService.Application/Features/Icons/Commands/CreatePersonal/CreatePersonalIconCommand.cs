using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal
{
    public sealed record CreatePersonalIconCommand(Guid UserId, string SvgCode, string Name, Guid IconCategoryId) : IRequest<Result<string>>,
        IMustHasUserId;
}