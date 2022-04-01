using HealthPass.Data.Entities.Interfaces;

namespace HealthPass.Data.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int LoginAttempts { get; set; } = 0;
        
        public bool IsLocked { get; set; } = false;

        private string PasswordHash;

        public User() { }
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public User(UserDetails userDetails)
        {
            Name = userDetails.Name;
            Email = userDetails.Email;
        }

        public void SetPassword(string password, IPasswordHasher passwordHasher)
        {
            PasswordHash = passwordHasher.GeneratePasswordHash(password);
        }

        public bool CheckPasswordHash(string password, IPasswordHasher passwordHasher)
        {
            return passwordHasher.CheckPasswordHash(PasswordHash, password);
        }
    }
}