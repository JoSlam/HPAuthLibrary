using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Enums;

namespace HealthPass.Auth.Core
{
    public class PasswordRuleValidator
    {

        public bool Execute(PasswordRule rule, string password)
        {
            dynamic passwordProperty = GetComparisonProperty(rule.Property, password);
            switch (rule.Operator)
            {
                case OperatorTypeEnum.Contains:
                    break;
                case OperatorTypeEnum.Equals:
                    break;
                case OperatorTypeEnum.NotEquals:
                    break;
                case OperatorTypeEnum.GreaterThan:
                    break;
                case OperatorTypeEnum.GreaterThanOrEqualTo:
                    break;
                case OperatorTypeEnum.LessThan:
                    break;
                case OperatorTypeEnum.LessThanOrEqualTo:
                    break;
            }
        }

        public dynamic GetComparisonProperty(PropertyTypeEnum propertyType, string password)
        {
            return propertyType switch
            {
                PropertyTypeEnum.Length => password.Length,
                _ => password,
            };
        }

    }
}
