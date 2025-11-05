using Common.Core.Guard;
using Common.Core.Results;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon
{
    public readonly record struct IconName
    {
        public readonly string Value { get; }

        private IconName(string value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static IconName Create(string value)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(value), () => new DomainException(Error.New(ErrorCode.Validation, "Название для иконки было пустым.")));

            return new IconName(value);
        }

        public static implicit operator string(IconName value) => value.Value;
    }
}