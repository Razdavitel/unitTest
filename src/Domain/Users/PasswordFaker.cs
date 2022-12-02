using System.Security.Cryptography;
using System.Text;

namespace Domain.Users;
public static class PasswordFaker
{
    private readonly static HMACSHA512 hmac = new HMACSHA512();
    
    public static byte[] GetFakeHash()
    {
        return hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
    }

    public static byte[] GetFakeSalt()
    {
        return hmac.Key;
    }
}
