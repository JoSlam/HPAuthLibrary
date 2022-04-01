using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using HealthPassAuthLibrary;

namespace HealthPass.Auth.Core
{
    public class LoginManager : ILoginManager
    {
        private readonly AuthenticationModule authModule;
        private readonly LockoutCriteria lockoutCriteria;
        private readonly TokenManager tokenManager;
        private readonly HealthPassContext dbContext;

        public LoginManager(AuthenticationModule authModule, LockoutCriteria lockoutCriteria, TokenManager tokenManager, HealthPassContext dbContext)
        {
            this.authModule = authModule;
            this.lockoutCriteria = lockoutCriteria;
            this.tokenManager = tokenManager;
            this.dbContext = dbContext;
        }

        public bool LoginUser(RequestDetails requestDetails, string email, string password)
        {
            
            bool result = authModule.LoginUser(email, password);

        }

        public bool RegisterUser(RequestDetails requestDetails, UserDetails userDetails)
        {
            throw new NotImplementedException();
        }

        public bool UnlockUser(RequestDetails requestDetails, string email)
        {
            throw new NotImplementedException();
        }

        private RequestSignature GetRequestSignature(RequestDetails details)
        {

        }
    }
}
