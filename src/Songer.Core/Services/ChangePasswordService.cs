using Songer.Core.Client;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class ChangePasswordService : IChangePasswordService
    {
        public ChangePasswordService(
            IRestClient client,
            IUserService userService)
        {
            _client = client;
            _userService = userService;
        }

        private readonly IRestClient _client;
        private readonly IUserService _userService;

        public async Task<bool> IsCorrectPassword(string password)
        {
            var jsonPassword = new
            {
                password = password
            };

            var response = await _client.PostAsJsonAsync("users/password", jsonPassword);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();
            }

            return false;
        }

        public async Task ChangePassword(string password)
        {
            await _userService.EditUser(password: password);
        }
    }
}