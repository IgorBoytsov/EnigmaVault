using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Update
{
    public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator() => Include(new TagNameValidator());
    }
}