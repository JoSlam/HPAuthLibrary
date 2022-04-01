namespace HealthPass.Data.Entities.Interfaces
{
    public abstract class IPasswordHasher
    {
        public abstract string GeneratePasswordHash(string password);
        public bool CheckPasswordHash(string hashedPassword, string password)
        {
            string newPasswordHash = GeneratePasswordHash(password);
            return hashedPassword == newPasswordHash;
        }
    }
}
