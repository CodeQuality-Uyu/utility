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
        /// Checks if string is null or white space
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIsNullOrEmpty(string? value, string propName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(propName, $"Value of parameter cannot be null or empty");
            }
        }

        /// <summary>
        /// Checks if string has minimum length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowMinimumLength(string value, int length, string propName)
        {
            if (value.Length < length) 
            {
                throw new ArgumentException($"Parameter '{propName}' must have minimum {length} characters");
            }
        }

        /// <summary>
        /// Checks if string has max length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="propName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowMaximumLength(string value, int length, string propName)
        {
            if (value.Length > length)
            {
                throw new ArgumentException($"Parameter '{propName}' must have maximum {length} characters");
            }
        }

        /// <summary>
        /// Replace '<script>, </script>' tags and '<, </>' tags with empty string
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="NullReferenceException">If value is null</exception>
        /// <returns>String without the tags</returns>
        public static string Encode(string value)
        {
            var withoutScript = value.Replace("<script>", "").Replace("</script>","");
            var withoutJs = withoutScript.Replace("<", "").Replace("</>","");

            return withoutJs;
        }

        /// <summary>
        /// Checks the format of the email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowEmailFormat(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the email matches the pattern
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Incorrect format of email");
            }
        }

        /// <summary>
        /// Checks that the password has at least on special character and number
        /// </summary>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowPasswordFormat(string password)
        {
            string specialCharacterPattern = @"[!@#$%^&*(),.?""\:{ }|<>]";
            string numberPattern = @"\d";
            if (!Regex.IsMatch(password, specialCharacterPattern) || !Regex.IsMatch(password, numberPattern))
            {
                throw new ArgumentException("Password must have at least one number and special character");
            }
        }
    }
}