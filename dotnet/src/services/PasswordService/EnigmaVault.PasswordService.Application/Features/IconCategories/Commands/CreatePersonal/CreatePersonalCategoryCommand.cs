using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreatePersonal
{
    public sealed record CreatePersonalCategoryCommand(string Name, Guid UserId) : IRequest<Result<Unit>>,
        IHasName,
        IMustHasUserId;
}