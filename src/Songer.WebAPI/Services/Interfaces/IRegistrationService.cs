using System.Threading.Tasks;
using Songer.WebAPI.Models;

namespace Songer.WebAPI.Services
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterUserModel model);
    }
}
