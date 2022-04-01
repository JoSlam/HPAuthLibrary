using HealthPass.Auth.Core;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace HealthPass.Auth.Tests
{
    [TestClass]
    public class TokenManagerTests
    {
        private int sessionLength;
        private ITokenManager tokenManager;

        [TestInitialize]
        public void Initialize()
        {
            sessionLength = 5;
            tokenManager = new TokenManager(sessionLength);
        }

        [TestMethod]
        public void CanCreateValidToken()
        {
            string tokenString = tokenManager.GetToken("John");
            Assert.IsNotNull(tokenString);

            Token token = JsonConvert.DeserializeObject<Token>(tokenString);
            Assert.IsNotNull(token);
            Assert.IsTrue(token.ExpiryTime > token.IssueTime);
        }

        [TestMethod]
        public void CanCheckTokenValidity()
        {
            DateTime testTime = DateTime.UtcNow;

            // Create an expired token
            Token token = new Token()
            {
                Name = "John",
                IssueTime = testTime.Ticks,
                ExpiryTime = testTime.AddHours(1).Ticks
            };

            // Token should not be valid (result = false)
            bool result = tokenManager.IsTokenValid(JsonConvert.SerializeObject(token));
            Assert.IsFalse(result);
        }
    }
}
