using Songer.Core.Models;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IAuthenticateService
    {
        User LoggedUser { get; }

        Task<bool> Auth(string login, string password);
        void LogOut();
    }
}
