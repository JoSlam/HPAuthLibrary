using HealthPass.Auth.Core.ComparisonHandlers;
using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Enums;

namespace HealthPass.Auth.Core
{
    public class PasswordRuleValidator
    {

        public static bool Execute(PasswordRule rule, string password)
        {
            dynamic passwordProperty = GetComparisonProperty(rule.Property, password);
            ComparisonType comparisonType = GetComparisonType(passwordProperty, rule.Value);
            GenericComparisonHandler comparisonHandler = ComparisonHandlerFactory.GetComparisonHandler(comparisonType);

            return rule.Operator switch
            {
                OperatorTypeEnum.Contains => comparisonHandler.Contains(passwordProperty, rule.Value),
                OperatorTypeEnum.Equals => comparisonHandler.EqualTo(passwordProperty, rule.Value),
                OperatorTypeEnum.NotEquals => comparisonHandler.NotEqualTo(passwordProperty, rule.Value),
                OperatorTypeEnum.GreaterThan => comparisonHandler.GreaterThan(passwordProperty, rule.Value),
                OperatorTypeEnum.GreaterThanOrEqualTo => comparisonHandler.GreaterThanOrEqualTo(passwordProperty, rule.Value),
                OperatorTypeEnum.LessThan => comparisonHandler.LessThan(passwordProperty, rule.Value),
                OperatorTypeEnum.LessThanOrEqualTo => comparisonHandler.LessThanOrEqualTo(passwordProperty, rule.Value),
                _ => false,
            };
        }

        private static ComparisonType GetComparisonType(dynamic passwordProperty, dynamic comparisonValue)
        {
            if (passwordProperty is decimal && comparisonValue is decimal)
            {
                return ComparisonType.Number;
            }
            return ComparisonType.String;
        }

        private static dynamic GetComparisonProperty(PropertyTypeEnum propertyType, string password)
        {
            return propertyType switch
            {
                PropertyTypeEnum.Length => password.Length,
                _ => password,
            };
        }

    }
}
