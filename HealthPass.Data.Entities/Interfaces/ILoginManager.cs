namespace HealthPass.Data.Entities.Interfaces
{
    public interface ILoginManager
    {
        public bool LoginUser(RequestDetails requestDetails, string email, string password);
        public bool RegisterUser(RequestDetails requestDetails, UserDetails userDetails);
        public void UnlockUser(string email);
    }
}
