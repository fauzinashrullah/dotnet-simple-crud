using System.Security.Cryptography;
using System.Text;

namespace SimpleApi.Helpers;

public static class PasswordHasher
{
    public static string CreateHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}