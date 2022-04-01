using HealthPass.Data.Entities.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HealthPass.Auth.Core
{
    public class MD5PasswordHasher : IPasswordHasher
    {
        public override string GeneratePasswordHash(string password)
        {
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

            // Compute password hash
            HashAlgorithm hashAlgorithm = MD5.Create();
            byte[] hashResult = hashAlgorithm.ComputeHash(passwordBytes);
            return Encoding.ASCII.GetString(hashResult);
        }
    }
}
