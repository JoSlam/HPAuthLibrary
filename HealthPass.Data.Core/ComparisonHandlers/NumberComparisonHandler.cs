namespace HealthPass.Auth.Core.ComparisonHandlers
{
    public class NumberComparisonHandler : GenericComparisonHandler
    {
        public override bool EqualTo(dynamic left, dynamic right)
        {
            return Convert.ToDecimal(left) == Convert.ToDecimal(right);
        }

        public override bool GreaterThan(dynamic left, dynamic right)
        {
            return Convert.ToDecimal(left) > Convert.ToDecimal(right);
        }

        public override bool LessThan(dynamic left, dynamic right)
        {
            return Convert.ToDecimal(left) < Convert.ToDecimal(right);
        }
    }
}
