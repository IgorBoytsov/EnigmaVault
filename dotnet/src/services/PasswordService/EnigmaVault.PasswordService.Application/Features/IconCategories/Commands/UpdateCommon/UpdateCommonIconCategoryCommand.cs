using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdateCommon
{
    public sealed record UpdateCommonIconCategoryCommand(Guid Id, string Name) : IRequest<Result<Unit>>;
}