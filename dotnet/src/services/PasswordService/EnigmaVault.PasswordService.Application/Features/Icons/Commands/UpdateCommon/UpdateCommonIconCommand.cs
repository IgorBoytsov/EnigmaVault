using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdateCommon
{
    public sealed record UpdateCommonIconCommand(Guid Id, string Name, string SvgCode, Guid IconCategoryId) : IRequest<Result<Unit>>,
        IHasName,
        IHasSvgCode;
}