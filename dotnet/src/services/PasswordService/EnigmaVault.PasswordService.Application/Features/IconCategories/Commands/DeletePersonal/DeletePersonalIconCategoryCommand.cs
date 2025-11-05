using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeletePersonal
{
    public sealed record DeletePersonalIconCategoryCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}