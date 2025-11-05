using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreateCommon
{
    public sealed record CreateCommonIconCategoryCommand(string Name) : IRequest<Result<Unit>>;
}