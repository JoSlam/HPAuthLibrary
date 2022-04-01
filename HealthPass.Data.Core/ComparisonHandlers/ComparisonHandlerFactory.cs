namespace HealthPass.Auth.Core.ComparisonHandlers
{
    public class ComparisonHandlerFactory
    {
        public static GenericComparisonHandler GetComparisonHandler(ComparisonType type)
        {
            return type switch
            {
                ComparisonType.Number => new NumberComparisonHandler(),
                _ => new GenericComparisonHandler(),
            };
        }
    }

    public enum ComparisonType
    {
        String = 0,
        Number = 1
    }
}
