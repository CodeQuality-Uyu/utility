using System.Text.RegularExpressions;

namespace PlayerFinder.Utility
{
    public static class Guard
    {
        public static void ThrowIsNull(object? value, string propName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(propName);
            }
        }

        public static void ThrowIsNullOrEmpty(string? value, string propName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(propName);
            }
        }

        public static void ThrowMinimumLength(string value, int length, string propName)
        {
            if (value.Length < length) 
            {
                throw new ArgumentException($"{propName} must have minimum {length} characters.");
            }
        }

        public static string Encode(string value)
        {
            var withoutScript = value.Replace("<script>", "").Replace("</script>","");
            var withoutJs = withoutScript.Replace("<", "").Replace("</>","");

            return withoutJs;
        }

        public static void ThrowEmailFormat(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the email matches the pattern
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Incorrect format of email");
            }
        }

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