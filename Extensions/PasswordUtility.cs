using System.Security.Cryptography;
using System.Text;

namespace BlazingBlog.Extensions;

public static class PasswordUtility
{
    public static string GenerateRandomSalt()
    {
        using var sha512 = SHA512.Create();
        var salt = new byte[512]; // 16 bytes for salt
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        var hashedSalt = sha512.ComputeHash(salt);
        return Convert.ToBase64String(hashedSalt);
    }
        
    public static string HashPassword(this string password, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);

        using var sha512 = SHA512.Create();
        var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
        Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

        var hashedPassword = sha512.ComputeHash(combinedBytes);
        var hashedPasswordString = Convert.ToBase64String(hashedPassword);
        return $"{hashedPasswordString}{salt}";
    }
}