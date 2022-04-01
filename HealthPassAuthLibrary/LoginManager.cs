using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using HealthPassAuthLibrary;
using System.Security.Cryptography;
using System.Text;

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
            User? user = GetUser(email);
            if (user.IsLocked || IsRequestSignatureBlocked(requestDetails))
            {
                return false;
            }

            bool result = authModule.LoginUser(email, password);
            HandleLoginResult(requestDetails, user, result);
            return result;
        }


        public bool RegisterUser(RequestDetails requestDetails, UserDetails userDetails)
        {
            return authModule.RegisterUser(userDetails);
        }

        public bool IsTokenValid(string token)
        {
            return tokenManager.IsTokenValid(token);
        }


        public void UnlockUser(string email)
        {
            User? user = dbContext.Users.SingleOrDefault(i => i.Email == email);
            Console.WriteLine($"Unlocked user: {email}");

            // Unlock and Save
            user.IsLocked = false;
            dbContext.SaveChanges();
        }

        private User? GetUser(string email)
        {
            return dbContext.Users.SingleOrDefault(i => i.Email == email);
        }

        private void HandleLoginResult(RequestDetails requestDetails, User? user, bool result)
        {
            // Log login attempt to database
            string requestSignature = GetRequestSignature(requestDetails);
            LogResult(requestSignature, result);

            if (!result)
            {
                user.LoginAttempts += 1;
                if (user.LoginAttempts >= 3)
                {
                    LockUser(user);
                }

                List<LoginDetails> details = GetFailedLoginAttemptsBySignature(requestSignature);
                bool canBlockRequestSignature = details.Count >= lockoutCriteria.MaxAttempts;
                if (canBlockRequestSignature)
                {
                    BlockRequestSignature(requestSignature);
                }
                else
                {
                    UnblockRequestSignature(requestSignature);
                }

                dbContext.SaveChanges();
            }
            else
            {
                // Clear login attempts
                user.LoginAttempts = 0;
                dbContext.SaveChanges();
            }
        }

        private bool IsRequestSignatureBlocked(RequestDetails requestDetails)
        {
            string signature = GetRequestSignature(requestDetails);
            BlockedSignature? blockedSignature = dbContext.BlockedSignatures.SingleOrDefault(i => i.Signature == signature);

            DateTime current = DateTime.UtcNow;

            bool signatureIsActive = blockedSignature != null && !blockedSignature.IsActive;
            bool signatureIsNotExpired = current < blockedSignature.ExpiryDateUTC;

            return signatureIsActive && signatureIsNotExpired;
        }

        private List<LoginDetails> GetFailedLoginAttemptsBySignature(string requestSignature)
        {
            DateTime policyDate = DateTime.UtcNow.AddMinutes(-(lockoutCriteria.LockoutTime));
            List<LoginDetails> loginAttempts = dbContext.LoginDetails
                .Where(i => i.RequestSignature == requestSignature
                                && i.CreatedDateUTC >= policyDate
                                && i.Success == false)
                .ToList();
            return loginAttempts;
        }

        private void LockUser(User user)
        {
            Console.WriteLine($"Locked user: {user.Email} indefinitely.");
            user.IsLocked = true;

            dbContext.SaveChanges();
        }

        private void BlockRequestSignature(string signature)
        {
            Console.WriteLine($"Blocked signature {signature} for {lockoutCriteria.LockoutTime} minutes.");

            DateTime current = DateTime.UtcNow;
            BlockedSignature blocked = new BlockedSignature()
            {
                IsActive = true,
                Signature = signature,
                CreatedDateUTC = current,
                ExpiryDateUTC = current,
            };

            // Add blocked signature
            dbContext.BlockedSignatures.Add(blocked);
            dbContext.SaveChanges();
        }

        private void UnblockRequestSignature(string signature)
        {
            List<BlockedSignature> active = dbContext.BlockedSignatures.Where(i => i.Signature == signature && i.IsActive == true).ToList();
            active.ForEach(i => i.IsActive = false);

            Console.WriteLine($"Unblocked signature {signature}.");
            dbContext.SaveChanges();
        }

        private void LogResult(string requestSignature, bool result)
        {
            LoginDetails login = new LoginDetails()
            {
                RequestSignature = requestSignature,
                Success = result,
                CreatedDateUTC = DateTime.UtcNow
            };

            dbContext.LoginDetails.Add(login);
            dbContext.SaveChanges();
        }


        private string GetRequestSignature(RequestDetails details)
        {
            string signatureString = $"{details.Agent};{details.ClientIP};{details.ClientCookies}";
            byte[] signatureBytes = Encoding.ASCII.GetBytes(signatureString);

            HashAlgorithm hashAlgorithm = MD5.Create();
            byte[] hashResult = hashAlgorithm.ComputeHash(signatureBytes);
            return Encoding.ASCII.GetString(hashResult);
        }
    }
}
