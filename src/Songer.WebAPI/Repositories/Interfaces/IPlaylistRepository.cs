using Songer.WebAPI.Models;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> GetPlaylistAsync(int playlistId);

        Task<Playlist> AddAsync(CreatePlaylistModel model, int userId);
        Task<Playlist> UpdateAsync(EditPlaylistModel model, int playlistId, int userId);
        Task DeleteAsync(int playlistId);
    }
}