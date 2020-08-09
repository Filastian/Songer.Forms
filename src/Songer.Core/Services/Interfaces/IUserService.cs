using Songer.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IUserService
    {
        Task DeleteUser(int userId);
        Task<bool> EditUser(string password = null, string name = null);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
    }
}
