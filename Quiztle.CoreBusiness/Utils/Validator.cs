using System.Text.RegularExpressions;

namespace Quiztle.CoreBusiness.Utils
{
    public static class Validator
    {
        // Regular expression to validate email
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // Regular expression to validate password
        // Ajustada para garantir que qualquer caractere especial seja aceito
        private static readonly Regex PasswordRegex = new(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^\w\s])[A-Za-z\d\W]{6,}$", RegexOptions.Compiled);


        /// <summary>
        /// Checks if the email is valid and returns a dictionary with the result and an error message
        /// </summary>
        public static Dictionary<string, object> IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Dictionary<string, object>
                {
                    { "isValid", false },
                    { "error", "Email cannot be empty." }
                };
            }

            bool isValid = EmailRegex.IsMatch(email);
            return new Dictionary<string, object>
            {
                { "isValid", isValid },
                { "error", isValid ? string.Empty : "Invalid email format." }
            };
        }

        /// <summary>
        /// Checks if the password is valid and returns a dictionary with the result and an error message
        /// </summary>
        public static Dictionary<string, object> IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new Dictionary<string, object>
                {
                    { "isValid", false },
                    { "error", "Password cannot be empty." }
                };
            }

            bool isValid = PasswordRegex.IsMatch(password);
            return new Dictionary<string, object>
            {
                { "isValid", isValid },
                { "error", isValid ? string.Empty : "Password must have at least 6 characters, 1 letter, 1 number, and 1 special character." }
            };
        }

        /// <summary>
        /// Validates both email and password and returns a list of two dictionaries.
        /// </summary>
        public static List<Dictionary<string, object>> IsValidEmailAndPassword(string email, string password)
        {
            var emailValidation = IsValidEmail(email);
            var passwordValidation = IsValidPassword(password);

            return new List<Dictionary<string, object>> { emailValidation, passwordValidation };
        }
    }
}
