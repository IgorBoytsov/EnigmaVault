using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdatePersonal
{
    public sealed record UpdatePersonalIconCategoryCommand(Guid Id, Guid UserId, string Name) : IRequest<Result<Unit>>;
}