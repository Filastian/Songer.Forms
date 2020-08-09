using Songer.Core.Client;
using Songer.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRestClient _client;

        public UserService(IRestClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var response = await _client.GetAsync("users");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsJsonAsync<List<User>>();
        }

        public async Task<User> GetUser(int userId)
        {
            var response = await _client.GetAsync($"users/{userId}");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsJsonAsync<User>();
        }

        public async Task<bool> EditUser(string password = null, string name = null)
        {
            if (password == null && name == null) 
                return false;

            var updateModel = new
            {
                password = password,
                name = name
            };

            var response = await _client.PutAsJsonAsync($"users/edit", updateModel);

            return response.IsSuccessStatusCode;
        }

        public async Task DeleteUser(int userId)
        {
            await _client.DeleteAsync($"users/delete/{userId}");
        }
    }
}
