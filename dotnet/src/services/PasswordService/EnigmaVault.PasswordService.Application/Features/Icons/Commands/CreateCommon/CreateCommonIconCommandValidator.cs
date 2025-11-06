using FluentValidation;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreateCommon
{
    public sealed class CreateCommonIconCommandValidator : AbstractValidator<CreateCommonIconCommand>
    {
        public CreateCommonIconCommandValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("Название не должно быть пустым.");

            RuleFor(i => i.SvgCode)
                .NotEmpty().WithMessage("Код для картинки не должен быть пустым.");

            RuleFor(i => i.IconCategoryId)
                .NotEmpty().WithMessage("Категория для иконки не может быть пустым.");
        }
    }
}