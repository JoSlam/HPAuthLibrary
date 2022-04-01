using HealthPass.Auth.Core;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;

namespace HealthPassAuthLibrary
{
    public class AuthenticationModule
    {
        private readonly HealthPassContext context;
        private readonly IPasswordHasher passwordHasher;

        private List<PasswordRule> PasswordRules { get; set; }
        public LockoutCriteria LockoutCriteria { get; }


        public AuthenticationModule(HealthPassContext context, List<PasswordRule> passwordRules, LockoutCriteria lockoutCriteria, IPasswordHasher passwordHasher)
        {
            this.context = context;
            PasswordRules = passwordRules;
            LockoutCriteria = lockoutCriteria;
            this.passwordHasher = passwordHasher;
        }

        public bool VerifyCredentials(string email, string password) {
            User existingUser = context.Users.SingleOrDefault(i => i.Email == email);
            return (existingUser != null)
                && existingUser.CheckPasswordHash(password, passwordHasher);
        }

        public string LoginUser(string email, string password)
        {
            return "";
        }

        //TODO: Consider swapping return with token for immediate access
        public bool RegisterUser(UserDetails userDetails)
        {
            //Check for existing user
            User existingUser = context.Users.SingleOrDefault(i => i.Email == userDetails.Email);
            if (existingUser != null)
            {
                Console.WriteLine($"A user exists already exists with the provided email address");
                return false;
            }

            User newUser = new User(userDetails);
            context.Users.Add(newUser);
            context.SaveChanges();

            return true;
        }

        public bool SetUserPassword(User user, string password)
        {
            //TODO: perform password validations
            user.SetPassword(password, passwordHasher);
            return true;
        }

        public bool ValidateToken(string token)
        {
            return false;
        }
    }
}