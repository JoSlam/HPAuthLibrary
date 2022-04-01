namespace HealthPass.Data.Entities
{
    public record Token
    {
        public string Name { get; set; }
        public long IssueTime { get; set; }
        public long ExpiryTime { get; set; }
    }
}
