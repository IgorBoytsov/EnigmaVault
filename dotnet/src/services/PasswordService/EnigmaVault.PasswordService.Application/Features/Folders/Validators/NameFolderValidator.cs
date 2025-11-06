using EnigmaVault.PasswordService.Application.Features.Validators;
using EnigmaVault.PasswordService.Domain.ValueObjects.Folder;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Validators
{
    public sealed class NameFolderValidator : AbstractValidator<IHasName>
    {
        public NameFolderValidator()
            => RuleFor(f => f.Name)
                .NotEmpty().WithMessage("Название папки не должно быть пустым. Введите название.")
                .Length(FolderName.MIN_LENGTH, FolderName.MAX_LENGTH).WithMessage($"Диапазон названия должен быть от {FolderName.MIN_LENGTH} до {FolderName.MAX_LENGTH} символов");
    }
}