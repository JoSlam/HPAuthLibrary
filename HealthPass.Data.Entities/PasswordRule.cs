using HealthPass.Data.Entities.Enums;

namespace HealthPass.Data.Entities
{
    public class PasswordRule
    {
        public OperatorTypeEnum Operator { get; set; }
        public PropertyTypeEnum Property { get; set; }
        public dynamic Value { get; set; }
        public string Message { get; set; }
    }
}
