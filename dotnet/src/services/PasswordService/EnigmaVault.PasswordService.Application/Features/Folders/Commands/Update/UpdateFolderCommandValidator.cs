using EnigmaVault.PasswordService.Application.Features.Folders.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Update
{
    public sealed class UpdateFolderCommandValidator : AbstractValidator<UpdateFolderCommand>
    {
        public UpdateFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
        }
    }
}