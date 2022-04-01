using HealthPass.Auth.Core;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using HealthPassAuthLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace HealthPass.Auth.Tests
{
    [TestClass]
    public class AuthenticationModuleTests
    {
        AuthenticationModule authModule;
        HealthPassContext mockDbContext;
        User testUser;


        [TestInitialize]
        public void Initialize()
        {
            mockDbContext = new MockDBContext().BuildMockContext();
            List<PasswordRule> passwordRules = new List<PasswordRule>();
            LockoutCriteria userLockoutCriteria = new LockoutCriteria();
            IPasswordHasher passwordHasher = new MD5PasswordHasher();

            // Add test user
            testUser = new User("John Doe", "johndoe@example.com");
            testUser.SetPassword("password123", passwordHasher);

            mockDbContext.Users.Add(testUser);
            mockDbContext.SaveChanges();

            authModule = new AuthenticationModule(mockDbContext, passwordRules, userLockoutCriteria, passwordHasher);
        }

        [TestMethod]
        public void CanCreateUserInDB()
        {
            User newUser = new User("Jane Johnson", "jane@example.com");
            mockDbContext.Users.Add(newUser);
            mockDbContext.SaveChanges();

            User fetchedUser = mockDbContext.Users.SingleOrDefault(i => i.Email == newUser.Email);
            Assert.IsNotNull(fetchedUser);
            Assert.AreEqual(fetchedUser.Name, newUser.Name);
        }

        [TestMethod]
        public void CanVerifyCredentials()
        {
            bool validResult = authModule.VerifyCredentials(testUser.Email, "password123");
            Assert.IsTrue(validResult);

            bool invalidResult = authModule.VerifyCredentials(testUser.Email, "example");
            Assert.IsFalse(invalidResult);
        }
    }
}