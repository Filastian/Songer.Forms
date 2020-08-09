using Songer.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(int userId);

        Task AddAsync(User user);
        Task UpdateAsync(UpdateUserModel model, int userId);
        Task DeleteAsync(int userId);

        //  Songs managements

        Task<IEnumerable<Song>> GetAllUserSongsAsync(int userId);

        Task AddSongToUserAsync(int userId, int songId);
        Task DeleteUserSongAsync(int userId, int songId);

        // Playlists managements

        Task<IEnumerable<Playlist>> GetAllUserPlaylistsAsync(int userId);
    }
}
