using System.Runtime.CompilerServices;

namespace Common.Core.Guard
{
    public static class GuardClauseExtensions
    {
        /// <summary>
        /// Универсальный Guard для пользовательских условий. Срабатывает, если условие истинно.
        /// Использовать только для редких, специфичных проверок, для которых нет стандартного Guard-метода.
        /// </summary>
        public static void That(this GuardClause _, bool condition, Func<Exception> exceptionFactory)
        {
            if (condition)
                throw exceptionFactory();
        }

        /// <summary>
        /// Универсальный Guard для пользовательских условий с простым сообщением.
        /// </summary>
        /// <param name="condition">Условие, которое, будучи истинным, вызовет исключение.</param>
        /// <param name="message">Сообщение для исключения.</param>
        /// <exception cref="ArgumentException">Исключение с указанным сообщением.</exception>
        public static void That(this GuardClause _, bool condition, string message)
        {
            if (condition)
                throw new ArgumentException(message);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public static void Null<T>(this GuardClause _, T input, [CallerArgumentExpression(nameof(input))] string parameterName = null!)
        {
            if (input is null)
                throw new ArgumentNullException(parameterName);
        }

        /// <exception cref="ArgumentException"></exception>
        public static void NullOrEmpty(this GuardClause _, string input, [CallerArgumentExpression(nameof(input))] string parameterName = null!)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Строка не может быть null или пустой.", parameterName);
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void OutOfRange(this GuardClause _, int input, int rangeFrom, int rangeTo, [CallerArgumentExpression(nameof(input))] string parameterName = null!)
        {
            if (input < rangeFrom || input > rangeTo)
                throw new ArgumentOutOfRangeException(parameterName, input, $"Значение должно быть в диапазоне от {rangeFrom} до {rangeTo}.");
        }

    }
}