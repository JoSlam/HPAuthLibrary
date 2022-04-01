namespace HealthPass.Data.Entities.Interfaces
{
    public interface ITokenManager
    {
        string GetToken(string name);
        bool IsTokenValid(string token);
    }
}
