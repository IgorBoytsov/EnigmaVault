using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Delete
{
    public sealed class DeleteFolderCommandValidator : AbstractValidator<DeleteFolderCommand>
    {
        public DeleteFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new MustUserIdValidator());
        }
    }
}