using EnigmaVault.PasswordService.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal
{
    public sealed class CreatePersonalIconCommandValidator : AbstractValidator<CreatePersonalIconCommand>
    {
        public CreatePersonalIconCommandValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("Название не должно быть пустым.");

            RuleFor(i => i.SvgCode)
                .NotEmpty().WithMessage("Код для картинки не должен быть пустым.");

            RuleFor(i => i.IconCategoryId)
                .NotEmpty().WithMessage("Категория для иконки не может быть пустым.");

            Include(new MustUserIdValidator());
        }
    }
}