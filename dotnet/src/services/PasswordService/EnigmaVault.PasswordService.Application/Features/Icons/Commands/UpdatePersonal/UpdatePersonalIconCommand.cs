using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal
{
    public sealed record UpdatePersonalIconCommand(Guid Id, Guid UserId, string Name, string SvgCode, Guid IconCategoryId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId,
        IHasName,
        IHasSvgCode;
}