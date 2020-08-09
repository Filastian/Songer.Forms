using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IRegistrateService
    {
        Task<bool> Registrate(string login, string password, string name);
    }
}
