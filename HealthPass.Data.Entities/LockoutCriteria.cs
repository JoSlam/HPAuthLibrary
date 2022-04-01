namespace HealthPass.Data.Entities
{
    public class LockoutCriteria
    {
        public int MaxAttempts { get; set; }
        public int LockoutTime { get; set; }
    }
}
