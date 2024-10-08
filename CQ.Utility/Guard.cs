﻿using System.Text.RegularExpressions;

namespace CQ.Utility
{
    public static class Guard
    {
        #region LessThan
        public static void ThrowIsLessThan(string value, int length, string propName)
        {
            if (value.Length < length)
            {
                throw new ArgumentException($"Must have minimum {length} characters", propName);
            }
        }

        public static void ThrowIsLessThanOrEqual(string value, int length, string propName)
        {
            if (value.Length <= length)
            {
                throw new ArgumentException($"Must have minimum {length} characters", propName);
            }
        }

        public static void ThrowIsLessThanOrEqual(double value, double max, string propName)
        {
            if (value <= max)
            {
                throw new ArgumentException($"Must be grater than {max}", propName);
            }
        }

        public static void ThrowIsLessThan(int value, int max, string propName)
        {
            if (value < max)
            {
                throw new ArgumentException($"Must be grater than {max} or equal", propName);
            }
        }

        public static void ThrowIsLessThanOrEqual(int value, int max, string propName)
        {
            if (value <= max)
            {
                throw new ArgumentException($"Must be grater than {max}", propName);
            }
        }
        #endregion

        #region MoreThan
        public static void ThrowIsMoreThan(string value, int length, string propName)
        {
            if (value.Length > length)
            {
                throw new ArgumentException($"Must have maximum {length} characters", propName);
            }
        }

        public static void ThrowIsMoreThanOrEqual(string value, int length, string propName)
        {
            if (value.Length >= length)
            {
                throw new ArgumentException($"Must have maximum {length} characters", propName);
            }
        }

        public static void ThrowIsMoreThan(int value, int max, string propName)
        {
            if (value > max)
            {
                throw new ArgumentException($"Must be less than {max} or equal", propName);
            }
        }

        public static void ThrowIsMoreThanOrEqual(int value, int max, string propName)
        {
            if (value >= max)
            {
                throw new ArgumentException($"Must be less than {max}", propName);
            }
        }

        public static void ThrowIsMoreThan(double value, double max, string propName)
        {
            if (value > max)
            {
                throw new ArgumentException($"Must be less than {max} or equal", propName);
            }
        }

        public static void ThrowIsMoreThanOrEqual(double value, double max, string propName)
        {
            if (value >= max)
            {
                throw new ArgumentException($"Must be less than {max}", propName);
            }
        }
        #endregion

        public static string Encode(string? value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            value = value.Trim();

            var withoutScript = value.Replace("<script>", string.Empty).Replace("</script>", string.Empty);
            var withoutJs = withoutScript.Replace("<>", string.Empty).Replace("</>", string.Empty);
            var withoutTags = withoutJs.Replace("<", string.Empty).Replace(">", string.Empty);

            return withoutTags;
        }

        public static string Encode(string? value, string prop)
        {
            value ??= string.Empty;

            value = value.Trim();

            var withoutScript = value.Replace("<script>", string.Empty).Replace("</script>", string.Empty);
            var withoutJs = withoutScript.Replace("<>", string.Empty).Replace("</>", string.Empty);
            var withoutTags = withoutJs.Replace("<", string.Empty).Replace(">", string.Empty);

            ThrowIsNullOrEmpty(value, prop);

            return withoutTags;
        }

        #region Email
        public static void ThrowIsInvalidEmailFormat(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the email matches the pattern
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Incorrect format", nameof(email));
            }
        }

        public static void ThrowIsInputInvalidEmail(string input, string propName = "email")
        {
            ThrowIsNullOrEmpty(input, propName);

            ThrowIsInvalidEmailFormat(input);
        }
        #endregion

        #region Password
        public static void ThrowIsInvalidPasswordFormat(string password)
        {
            string specialCharacterPattern = @"[!@#$%^&*(),.?""\:{ }|<>]";
            string numberPattern = @"\d";
            if (!Regex.IsMatch(password, specialCharacterPattern) || !Regex.IsMatch(password, numberPattern))
            {
                throw new ArgumentException("Must have at least one number and special character", nameof(password));
            }
        }

        public static void ThrowIsInputInvalidPassword(string input, string propName = "password", int minLength = 8, int? maxLength = null)
        {
            ThrowIsInputInvalid(input, propName, minLength, maxLength);

            ThrowIsInvalidPasswordFormat(input);
        }
        #endregion

        public static void ThrowIsInputInvalid(string input, string propName, int minLength = 0, int? maxLength = null)
        {
            ThrowIsNullOrEmpty(input, propName);

            ThrowIsLessThan(input, minLength, propName);

            if (maxLength.HasValue)
            {
                ThrowIsMoreThan(input, maxLength.Value, propName);
            }
        }

        public static string Normalize(string input)
        {
            if (input.Length == 0)
            {
                return input;
            }

            if (input.Length == 1)
            {
                return $"{char.ToUpper(input[0])}";
            }

            return $"{char.ToUpper(input[0])}{input[1..]}";
        }
    }
}