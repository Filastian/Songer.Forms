using System.Threading.Tasks;
using Songer.WebAPI.Models;

namespace Songer.WebAPI.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto> AuthenticateAsync(AuthenticateUserModel model);
    }
}
