using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeleteCommon
{
    public sealed record DeleteCommonIconCategoryCommand(Guid Id) : IRequest<Result<Unit>>;
}