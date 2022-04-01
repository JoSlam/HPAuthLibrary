using HealthPass.Data.Entities.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HealthPass.Auth.Core
{
    public class MD5PasswordHasher : IPasswordHasher
    {
        //TODO: Add salt etc if necessary - Low priority (for this proj)
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
