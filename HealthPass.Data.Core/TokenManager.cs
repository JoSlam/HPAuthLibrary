using HealthPass.Data.Entities;
using HealthPass.Data.Entities.Interfaces;
using Newtonsoft.Json;

namespace HealthPass.Auth.Core
{
    public class TokenManager : ITokenManager
    {
        private readonly int SessionLength;

        public TokenManager(int sessionLengthMinutes)
        {
            SessionLength = sessionLengthMinutes;
        }


        //TODO: token encoding? - Low priority
        public string GetToken(string name)
        {
            DateTime current = DateTime.UtcNow;
            Token newToken = new Token()
            {
                Name = name,
                IssueTime = current.Ticks,
                ExpiryTime = current.AddMinutes(SessionLength).Ticks
            };
            return JsonConvert.SerializeObject(newToken);
        }

        public bool IsTokenValid(string token)
        {
            Token convertedToken = JsonConvert.DeserializeObject<Token>(token);
            return convertedToken.ExpiryTime <= DateTime.UtcNow.Ticks;
        }
    }
}
