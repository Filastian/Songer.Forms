using Songer.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongAsync(int songId);

        Task<Song> AddAsync(CreateSongModel model);
        Task<Song> UpdateAsync(UpdateSongModel model, int songId);
        Task DeleteSongAsync(int songId);
    }
}
