using Songer.WebAPI.Models;
using Songer.WebAPI.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.WebAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        
        public RegistrationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterAsync(RegisterUserModel model)
        {
            var user = new User
            {
                Login = model.Login,
                Name = model.Name,
                Password = model.Password,
                Role = Role.Default
            };

            await _userRepository.AddAsync(user);
        }
    }
}
