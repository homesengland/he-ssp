using System;
using System.Globalization;

namespace He.AspNetCore.Mvc.Gds.Components.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the substring "Controller" from a string.
        /// Very specific use case - when want to use static typing in Controllers for routing, ie. "nameof(HomeController).RemoveController()".
        /// </summary>
        /// <param name="str">The string to remove from.</param>
        /// <returns>The updated string.</returns>
        public static string RemoveController(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Replace("Controller", string.Empty, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Fluent syntax for the standard string IsNullOrEmpty check.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>Whether it IsNullOrEmpty.</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Fluent syntax for the inverse of the standard string IsNullOrEmpty check.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>Whether it is not NullOrEmpty.</returns>
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Fluent syntax for the standard string TrimStart function.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <param name="trimChars">The chars to trim from the start.</param>
        /// <returns>The trimmed string.</returns>
        public static string TrimStart(this string target, string trimChars)
        {
            return target?.TrimStart(trimChars?.ToCharArray());
        }

        /// <summary>
        /// Fluent syntax for the standard string TrimEnd function.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <param name="trimChars">The chars to trim from the end.</param>
        /// <returns>The trimmed string.</returns>
        public static string TrimEnd(this string target, string trimChars)
        {
            return target?.TrimEnd(trimChars?.ToCharArray());
        }

        /// <summary>
        /// Fluent syntax for the standard string Format function.
        /// </summary>
        /// <param name="value">The target string.</param>
        /// <param name="args">The array of arguments to insert into the string.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, value, args);
        }

        /// <summary>
        /// Return this string as a comma formatted number.
        /// </summary>
        /// <param name="value">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatAsNumber(this string value)
        {
            var isInt = int.TryParse(value, out var intValue);
            if (!isInt)
            {
                return value;
            }
            else
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:n0}", intValue);
            }
        }
    }
}
