using HealthPass.Auth.Core;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HealthPass.Auth.Tests
{
    [TestClass]
    public class PasswordRuleValidatorTests
    {

        [TestMethod]
        public void CanValidateContainsRule()
        {
            PasswordRule stringRule = new PasswordRule()
            {
                Operator = OperatorTypeEnum.Contains,
                Property = PropertyTypeEnum.None,
                Value = "123",
                Message = "Password validation failed"
            };

            bool result = PasswordRuleValidator.Execute(stringRule, "password123");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanValidatePasswordLength()
        {
            PasswordRule numberRule = new PasswordRule()
            {
                Operator = OperatorTypeEnum.GreaterThanOrEqualTo,
                Property = PropertyTypeEnum.Length,
                Value = 8,
                Message = "Password must have atleast 8 characters",
            };

            bool result = PasswordRuleValidator.Execute(numberRule, "pass");
            Assert.IsFalse(result);
        }

    }
}
