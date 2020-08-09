using Songer.Core.Client;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class RegistrateService : IRegistrateService
    {
        public RegistrateService(IRestClient client)
        {
            _client = client;
        }

        private readonly IRestClient _client;

        public async Task<bool> Registrate(string login, string password, string name)
        {
            var regModel = new
            {
                login = login,
                password = password,
                name = name
            };

            var response = await _client.PostAsJsonAsync("register", regModel);

            return response.IsSuccessStatusCode;
        }
    }
}
