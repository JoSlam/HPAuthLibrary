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

        public string LoginUser(string email, string password)
        {
            // Find existing user
            bool result = VerifyCredentials(email, password);
            Console.WriteLine($"Found user with email {email}");

            if (result)
            {
                //TODO: User token manager to get an access token
                return "AccessToken";
            }

            return "";
        }


        //TODO: Consider swapping bool return with token for immediate access on register
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
            bool passwordResult = SetUserPassword(newUser, userDetails.Password);

            if (passwordResult)
            {
                context.Users.Add(newUser);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool VerifyCredentials(string email, string password)
        {
            User existingUser = context.Users.SingleOrDefault(i => i.Email == email);
            return (existingUser != null)
                && existingUser.CheckPasswordHash(password, passwordHasher);
        }


        public bool SetUserPassword(User user, string password)
        {
            bool overallResult = PasswordRules.All(rule => PasswordRuleValidator.Execute(rule, password));

            if (overallResult)
            {
                user.SetPassword(password, passwordHasher);
                return true;
            }
            return false;
        }

        public bool ValidateToken(string token)
        {
            return false;
        }
    }
}