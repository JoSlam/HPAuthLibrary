namespace HealthPass.Data.Entities
{
    public class RequestDetails
    {
        public string Agent { get; set; }
        public string ClientIP { get; set; }
        // Assuming cookies come in as a string, not much knowledge of how cookies are handled by the system atm
        public string ClientCookies { get; set; } 
    }
}
