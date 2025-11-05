using Common.Core.Guard;
using Common.Core.Results;
using EnigmaVault.PasswordService.Domain.Exception;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory
{
    public readonly record struct IconCategoryId
    {
        public readonly Guid Value { get; }

        private IconCategoryId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static IconCategoryId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(Error.New(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(IconCategoryId)}")));

            return new IconCategoryId(value);
        }
        
        public static IconCategoryId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(IconCategoryId value) => value.Value;
    }
}