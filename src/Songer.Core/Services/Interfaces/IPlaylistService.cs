using Songer.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetUserPlaylists(bool updateToken = false);
        Task<bool> CreatePlaylist(string title, List<int> songIds);
        Task<bool> EditPlaylist(string title, List<int> songIds, int playlistId);
        Task<bool> DeletePlaylist(Playlist playlist);

        void DeleteSongFromPlaylistCashe(Song song);
        void UpdateSongInPlaylistCashe(Song song);
    }
}
