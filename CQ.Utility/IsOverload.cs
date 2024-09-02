namespace CQ.Utility;

public partial sealed record class Guard
{
        public static bool Is(string value, string expected)
        {
            return string.Equals(value, expected, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsIn(string value, string[] expected)
        {
            var exists = expected.Any(e => Is(value, e));

            return exists;
        }

        public static bool IsNot(string value, string expected)
        {
            return !Is(value, expected);
        }

public static bool IsNot(bool value)
        {
            return !value;
        }

        public static bool IsNotIn(string value, string[] expected)
        {
            var notFound = !IsIn(value, expected);

            return notFound;
        }

        public static void ThrowIsNull(object? value, string propName)
        {
            if (IsNotNull(value))
            {
                return;
            }

            throw new ArgumentNullException(propName, "Value of parameter cannot be null");
        }

        public static void ThrowIsNull<TException>(object? value)
            where TException : Exception, new()
        {
            if (IsNotNull(value))
            {
                return;
            }

            throw new TException();
        }

        public static void ThrowIsNull<TException>(object? value, params object?[] args)
            where TException : Exception, new()
        {
            if (IsNotNull(value))
            {
                return;
            }

            throw (TException)Activator.CreateInstance(typeof(TException), args);
        }

        public static bool IsNull(object? value)
        {
            return value == null;
        }

        public static bool IsNotNull(object? value)
        {
            return value != null;
        }

        public static void ThrowIsNullOrEmpty(string? value, string propName)
        {
            if (IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(propName, $"Value of parameter cannot be null or empty");
            }
        }

        public static void ThrowIsNullOrEmpty<TValue>(List<TValue>? value, string propName)
        {

            if (IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(propName, $"Value of parameter cannot be null or empty");
            }
        }

        public static bool IsNullOrEmpty(string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty<TValue>(List<TValue>? value)
        {
            return value == null || value.Count == 0;
        }

        public static bool IsNotNullOrEmpty(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
}