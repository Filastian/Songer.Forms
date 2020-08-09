using Songer.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAllSongs(bool updateToken = false);
        Task<IEnumerable<Song>> GetUserSongs(bool updateToken = false);

        Task<Stream> GetSong(int songId);

        Task<bool> AddSong(string title, string performer, byte[] data);
        Task<bool> UpdateSong(Song song);
        Task<bool> DeleteSong(Song song);

        Task AddToUserSongs(Song song);
        Task DeleteFromUserSongs(Song song);
    }
}
