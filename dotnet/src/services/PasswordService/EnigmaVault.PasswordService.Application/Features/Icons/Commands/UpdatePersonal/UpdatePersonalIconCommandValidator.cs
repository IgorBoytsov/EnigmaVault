using EnigmaVault.PasswordService.Application.Features.Icons.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalIconCommandValidator : AbstractValidator<UpdatePersonalIconCommand>
    {
        public UpdatePersonalIconCommandValidator()
        {
            Include(new GuidValidator());
            Include(new MustUserIdValidator());
            Include(new IconNameValidator());
            Include(new SvgCodeValidator());
        }
    }
}