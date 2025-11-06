using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Application.Features.Folders.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateRoot
{
    public sealed class CreateRootFolderCommandValidator : AbstractValidator<CreateRootFolderCommand>
    {
        public CreateRootFolderCommandValidator()
        {
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
            Include(new HexColorValidator());
        }
    }
}