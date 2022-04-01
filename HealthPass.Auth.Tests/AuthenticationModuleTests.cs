using HealthPass.Auth.Core;
using HealthPass.Auth.Tests.Mock;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using HealthPassAuthLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace HealthPass.Auth.Tests
{
    [TestClass]
    public class AuthenticationModuleTests
    {
        AuthenticationModule authModule;
        HealthPassDataContext mockDbContext;
        User testUser;


        [TestInitialize]
        public void Initialize()
        {
            mockDbContext = new MockDBContext().BuildMockContext();
            List<PasswordRule> passwordRules = new List<PasswordRule>();
            IPasswordHasher passwordHasher = new MD5PasswordHasher();

            // Add test user
            testUser = new User("John Doe", "johndoe@example.com");
            testUser.SetPassword("password123", passwordHasher);

            mockDbContext.Users.Add(testUser);
            mockDbContext.SaveChanges();

            authModule = new AuthenticationModule(mockDbContext, passwordRules, passwordHasher);
        }

        [TestMethod]
        public void CanRegisterUser()
        {
            UserDetails userDetails = new UserDetails()
            {
                Name = "Jane Johnson",
                Email = "jane@example.com",
                Password = "password123"
            };

            // Confirm result
            bool result = authModule.RegisterUser(userDetails);
            Assert.IsTrue(result);

            // Confirm user added to database
            User? fetchedUser = mockDbContext.Users.SingleOrDefault(i => i.Email == userDetails.Email);
            Assert.IsNotNull(fetchedUser);
            Assert.AreEqual(fetchedUser.Name, userDetails.Name);
        }

        [TestMethod]
        public void CanLoginUser()
        {
            string validResult = authModule.LoginUser(testUser.Email, "password123");
            Assert.IsTrue(!string.IsNullOrEmpty(validResult));

            string invalidResult = authModule.LoginUser(testUser.Email, "example");
            Assert.IsTrue(string.IsNullOrEmpty(invalidResult));
        }
    }
}