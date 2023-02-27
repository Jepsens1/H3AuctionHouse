using AuctionHouseBackend.Models;
using System.Security.Cryptography;

namespace AuctionHouseBackend.Cryption
{
    public class CryptoService
    {
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password + storedSalt, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
        public static HashModel SaltPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password + salt, saltBytes, 10000);
            string hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashModel hashSalt = new HashModel(hashPassword, salt);
            return hashSalt;
        }
    }
}
