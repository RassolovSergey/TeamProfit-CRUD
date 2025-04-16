using System.Text.RegularExpressions;

namespace Web_Api.Services
{
    public static class PasswordStrengthService
    {
        public static bool HasLowerCase(string password) =>
            Regex.IsMatch(password, "[a-z]");

        public static bool HasUpperCase(string password) =>
            Regex.IsMatch(password, "[A-Z]");

        public static bool HasDigit(string password) =>
            Regex.IsMatch(password, "[0-9]");

        public static bool HasSpecialChar(string password) =>
            Regex.IsMatch(password, "[^a-zA-Z0-9]");

        public static List<string> GetWeaknesses(string password)
        {
            var errors = new List<string>();

            if (!HasLowerCase(password)) errors.Add("Добавьте хотя бы одну строчную букву.");
            if (!HasUpperCase(password)) errors.Add("Добавьте хотя бы одну заглавную букву.");
            if (!HasDigit(password)) errors.Add("Добавьте хотя бы одну цифру.");
            if (!HasSpecialChar(password)) errors.Add("Добавьте хотя бы один специальный символ.");

            return errors;
        }
    }
}
