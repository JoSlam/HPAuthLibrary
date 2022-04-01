using HealthPass.Auth.Core;
using HealthPass.Data.Entities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HealthPass.Auth.Tests
{
    [TestClass]
    public class LoginManagerTests
    {
        private ILoginManager loginManager;

        [TestInitialize]
        public void Initialize()
        {
            loginManager = new LoginManager();
        }


        [TestMethod]

    }
}
