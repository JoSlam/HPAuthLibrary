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

        public AuthenticationModule(HealthPassContext context, List<PasswordRule> passwordRules, IPasswordHasher passwordHasher)
        {
            this.context = context;
            PasswordRules = passwordRules;
            this.passwordHasher = passwordHasher;
        }

        public bool LoginUser(string email, string password)
        {
            bool result = VerifyCredentials(email, password);
            Console.WriteLine($"Found user with email {email}");
            return result;
        }


        public bool RegisterUser(UserDetails userDetails)
        {
            //Check for existing user
            User? existingUser = context.Users.SingleOrDefault(i => i.Email == userDetails.Email);
            if (existingUser != null)
            {
                Console.WriteLine($"A user exists already exists with the provided email address");
                return false;
            }

            User newUser = new User(userDetails);
            bool passwordResult = SetUserPassword(newUser, userDetails.Password);

            if (passwordResult)
            {
                context.Users.Add(newUser);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        private bool VerifyCredentials(string email, string password)
        {
            // Find existing user
            User existingUser = context.Users.SingleOrDefault(i => i.Email == email);
            return (existingUser != null)
                && existingUser.CheckPasswordHash(password, passwordHasher);
        }


        private bool SetUserPassword(User user, string password)
        {
            bool overallResult = PasswordRules.All(rule => PasswordRuleValidator.Execute(rule, password));

            if (overallResult)
            {
                user.SetPassword(password, passwordHasher);
                return true;
            }
            return false;
        }
    }
}