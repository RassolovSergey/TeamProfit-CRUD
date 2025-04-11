// PasswordHelper.cs
using System.Security.Cryptography;
using System.Text;

namespace Web_Api.Services
{
    public static class PasswordHelper
    {
        // Метод для хеширования пароля
        public static string ComputeHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = password + salt;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return Convert.ToBase64String(bytes);
            }
        }

        // Метод для генерации соли
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
