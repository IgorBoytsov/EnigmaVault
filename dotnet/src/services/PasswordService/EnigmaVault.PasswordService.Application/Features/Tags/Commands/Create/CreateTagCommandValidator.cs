using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create
{
    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            Include(new MustUserIdValidator());
            Include(new TagNameValidator());
        }
    }
}