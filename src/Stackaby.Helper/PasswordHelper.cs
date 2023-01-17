using BC = BCrypt.Net.BCrypt;

namespace Stackaby.Helper;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        return BC.HashPassword(password, 16);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}