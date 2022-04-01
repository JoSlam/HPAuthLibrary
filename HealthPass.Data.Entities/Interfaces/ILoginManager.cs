namespace HealthPass.Data.Entities.Interfaces
{
    public interface ILoginManager
    {
        public string LoginUser(RequestDetails requestDetails, string email, string password);
        public string RegisterUser(RequestDetails requestDetails, UserDetails userDetails);
        public void UnlockUser(string email);
    }
}
