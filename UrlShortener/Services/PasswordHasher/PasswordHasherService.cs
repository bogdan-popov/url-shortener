using System.Security.Cryptography;

namespace UrlShortener.Services.PasswordHasher
{
    public class PasswordHasherService : IPasswordHasher
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computerHash.SequenceEqual(passwordHash);
        }
    }
}
