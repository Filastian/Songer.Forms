using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IChangePasswordService
    {
        Task ChangePassword(string password);
        Task<bool> IsCorrectPassword(string password);
    }
}