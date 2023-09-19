using System.Text.RegularExpressions;

namespace TodoList.Application.Sanitization
{
    public static class InputSanitizer
    {
        public static string SanitizeInput(string? input)
        {
            if (input == null) return string.Empty;

            // Removing HTML tags to prevent XSS attacks
            input = Regex.Replace(input, @"<(.|\n)*?>", string.Empty);

            // Removing potential SQL injection attempts
            input = Regex.Replace(input, @"('|--|;|/\*|xp_|sp_)", string.Empty);

            // Limiting the length of the input to prevent DoS attacks
            input = input.Length > 1000 ? input.Substring(0, 1000) : input;

            // Encoding the input to safely display special characters
            input = System.Net.WebUtility.HtmlEncode(input);

            return input;
        }
    }
}
