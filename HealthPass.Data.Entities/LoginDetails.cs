namespace HealthPass.Data.Entities
{
    public class LoginDetails
    {
        public int ID { get; set; }
        public string RequestSignature { get; set; }
        public int Attempts { get; set; }
    }
}
