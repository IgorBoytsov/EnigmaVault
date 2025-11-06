using Common.Core.Guard;
using Common.Core.Results;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon
{
    public readonly record struct IconName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 100;

        public readonly string Value { get; }

        private IconName(string value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static IconName Create(string value)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(value), () => new DomainException(Error.New(ErrorCode.Validation, "Название для иконки было пустым.")));
            Guard.Against.That(value.Length > MAX_LENGTH || value.Length < int.MinValue, () => new DomainException(Error.New(ErrorCode.Validation, $"Допустимый диапазон длинны название от {MIN_LENGTH} до {MAX_LENGTH}")));

            return new IconName(value);
        }

        public static implicit operator string(IconName value) => value.Value;
    }
}