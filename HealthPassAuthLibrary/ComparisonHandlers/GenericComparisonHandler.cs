namespace HealthPass.Auth.Core.ComparisonHandlers
{
    public class GenericComparisonHandler
    {
        public virtual bool Contains(dynamic left, dynamic right)
        {
            string leftValue = (string)left;
            string rightValue = (string)right;

            int start = leftValue.Length - rightValue.Length;
            if (start < 0)
            {
                return false;
            }

            return leftValue.Contains(rightValue);
        }

        public virtual bool EqualTo(dynamic left, dynamic right)
        {
            return Convert.ToString(left).Equals(Convert.ToString(right));
        }

        public virtual bool NotEqualTo(dynamic left, dynamic right)
        {
            return !EqualTo(left, right);
        }

        public virtual bool GreaterThan(dynamic left, dynamic right)
        {
            int result = left.CompareTo(right);
            return result == 1;
        }

        public virtual bool GreaterThanOrEqualTo(dynamic left, dynamic right)
        {
            return EqualTo(left, right) || GreaterThan(left, right);
        }

        public virtual bool LessThan(dynamic left, dynamic right)
        {
            int result = left.CompareTo(right);
            return result == -1;
        }

        public virtual bool LessThanOrEqualTo(dynamic left, dynamic right)
        {
            return EqualTo(left, right) || LessThan(left, right);
        }
    }
}
