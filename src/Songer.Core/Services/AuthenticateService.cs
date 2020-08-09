using Songer.Core.Client;
using Songer.Core.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        public AuthenticateService(IRestClient client)
        {
            _client = client;
        }

        public User LoggedUser { get; private set; }
        private readonly IRestClient _client;

        public async Task<bool> Auth(string login, string password)
        {
            try
            {
                var authModel = new
                {
                    login = login,
                    password = password
                };

                var response = await _client.PostAsJsonAsync("authenticate", authModel);

                if (!response.IsSuccessStatusCode) return false;

                LoggedUser = await response.Content.ReadAsJsonAsync<User>();

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoggedUser.Token);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void LogOut()
        {
            LoggedUser = null;
        }
    }
}
