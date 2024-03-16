using System.Text.RegularExpressions;

namespace CQ.Utility
{
    public static class Guard
    {
        /// <summary>
        /// Checks if value is null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentNullException"> When value is null</exception>
        public static void ThrowIsNull(object? value, string propName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propName, "Value of parameter cannot be null");
            }
        }

        /// <summary>
        /// Checks if value is null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <exception cref="TException"> When value is null</exception>
        public static void ThrowIsNull<TException>(object? value)
            where TException : Exception, new()
        {
            if (value == null)
            {
                throw new TException();
            }
        }

        /// <summary>
        /// Checks if value is null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <exception cref="TException"> When value is null</exception>
        public static void ThrowIsNull<TException>(object? value, params object?[] args)
            where TException : Exception, new()
        {
            if (value == null)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), args);
            }
        }

        /// <summary>
        /// Checks if objects is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(object? value)
        {
            return value == null;
        }

        /// <summary>
        /// Checks if objects is not null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNull(object? value)
        {
            return value != null;
        }

        /// <summary>
        /// Checks if string is null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Checks if string is null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty<TValue>(List<TValue>? value)
        {
            return value == null || value.Count == 0;
        }

        /// <summary>
        /// Checks if string is not null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Checks if string has minimum length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsLessThan(string value, int length, string propName)
        {
            if (value.Length < length)
            {
                throw new ArgumentException($"Must have minimum {length} characters", propName);
            }
        }

        /// <summary>
        /// Checks if int is less than max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsLessThan(int value, int max, string propName)
        {
            if (value < max)
            {
                throw new ArgumentException($"Must be grater than {max}", propName);
            }
        }

        /// <summary>
        /// Checks if string has max length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsMoreThan(string value, int length, string propName)
        {
            if (value.Length > length)
            {
                throw new ArgumentException($"Must have maximum {length} characters", propName);
            }
        }

        /// <summary>
        /// Checks if int is more than max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsMoreThan(int value, int max, string propName)
        {
            if (value > max)
            {
                throw new ArgumentException($"Must be less than {max}", propName);
            }
        }

        /// <summary>
        /// Replace '<script>, </script>', '<>, </>', '<, >' tags with empty string
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="NullReferenceException">If value is null</exception>
        /// <returns>String without the tags</returns>
        public static string Encode(string? value)
        {
            if (value == null) return string.Empty;

            value = value.Trim();

            var withoutScript = value.Replace("<script>", string.Empty).Replace("</script>", string.Empty);
            var withoutJs = withoutScript.Replace("<>", string.Empty).Replace("</>", string.Empty);
            var withoutTags = withoutJs.Replace("<", string.Empty).Replace(">", string.Empty);

            return withoutTags;
        }

        /// <summary>
        /// Checks the format of the email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsInvalidEmailFormat(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the email matches the pattern
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Incorrect format", nameof(email));
            }
        }

        /// <summary>
        /// Checks if input is not null or empty and has a email format
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentNullException"></exception>"
        /// <exception cref="ArgumentException"></exception>"
        public static void ThrowIsInputInvalidEmail(string input, string propName = "email")
        {
            ThrowIsNullOrEmpty(input, propName);

            ThrowIsInvalidEmailFormat(input);
        }

        /// <summary>
        /// Checks that the password has at least on special character and number
        /// </summary>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsInvalidPasswordFormat(string password)
        {
            string specialCharacterPattern = @"[!@#$%^&*(),.?""\:{ }|<>]";
            string numberPattern = @"\d";
            if (!Regex.IsMatch(password, specialCharacterPattern) || !Regex.IsMatch(password, numberPattern))
            {
                throw new ArgumentException("Must have at least one number and special character", nameof(password));
            }
        }

        /// <summary>
        /// Checks if input is a valid input and has a valid password format with special characters and number
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propName"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <exception cref="ArgumentNullException"></exception>"
        /// <exception cref="ArgumentException"></exception>"
        public static void ThrowIsInputInvalidPassword(string input, string propName = "password", int minLength = 8, int? maxLength = null)
        {
            ThrowIsInputInvalid(input, propName, minLength, maxLength);

            ThrowIsInvalidPasswordFormat(input);
        }

        /// <summary>
        /// Checks if the input is not null or empty and if it's between the lengths allowed in cased their are provided
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propName"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <exception cref="ArgumentNullException"></exception>"
        /// <exception cref="ArgumentException"></exception>"
        public static void ThrowIsInputInvalid(string input, string propName, int minLength = 0, int? maxLength = null)
        {
            ThrowIsNullOrEmpty(input, propName);

            ThrowIsLessThan(input, minLength, propName);

            if (maxLength.HasValue)
            {
                ThrowIsMoreThan(input, maxLength.Value, propName);
            }
        }
    }
}