using EnigmaVault.PasswordService.Application.Features.Icons.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdateCommon
{
    public sealed class UpdateCommonIconCommandValidator : AbstractValidator<UpdateCommonIconCommand>
    {
        public UpdateCommonIconCommandValidator()
        {
            Include(new IconNameValidator());
            Include(new SvgCodeValidator());
        }
    }
}