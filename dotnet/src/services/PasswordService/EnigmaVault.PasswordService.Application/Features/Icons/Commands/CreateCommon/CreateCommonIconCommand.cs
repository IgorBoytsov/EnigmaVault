using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreateCommon
{
    public sealed record CreateCommonIconCommand(string SvgCode, string Name, Guid IconCategoryId) : IRequest<Result<Unit>>;
}