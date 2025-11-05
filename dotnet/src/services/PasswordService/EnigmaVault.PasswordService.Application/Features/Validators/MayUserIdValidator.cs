using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Validators
{
    public sealed class MayUserIdValidator : AbstractValidator<IMayHasUserId>
    {
        public MayUserIdValidator() => When(x => x.UserId.HasValue, () =>
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Идентификатор (Guid) не может быть пустым.");
        });
    }
}