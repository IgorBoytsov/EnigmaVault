using Common.Core.Guard;
using Common.Core.Results;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory
{
    public readonly record struct IconCategoryName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 2;

        public readonly string Value { get; }

        private IconCategoryName(string value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static IconCategoryName Create(string value)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(value), () => new DomainException(Error.New(ErrorCode.Validation, "Название записи было пустым.")));
            Guard.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new DomainException(Error.New(ErrorCode.Validation, $"Максимально допустимы диапазон от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new IconCategoryName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(IconCategoryName value) => value.Value;
    }
}