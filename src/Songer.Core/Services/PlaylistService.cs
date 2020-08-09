using Songer.Core.Client;
using Songer.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IRestClient _client;
        private readonly ICasheService _casheService;

        private static string PlaylistKey = nameof(PlaylistKey);

        public PlaylistService(
            IRestClient client,
            ICasheService casheService)
        {
            _client = client;
            _casheService = casheService;
        }

        public async Task<IEnumerable<Playlist>> GetUserPlaylists(bool updateToken = false)
        {
            var playlists = default(IEnumerable<Playlist>);

            if (updateToken)
            {
                var response = await _client.GetAsync("playlists");

                if (!response.IsSuccessStatusCode) return null;

                playlists = await response.Content.ReadAsJsonAsync<IEnumerable<Playlist>>();

                _casheService.Put(PlaylistKey, playlists);
            }
            else
            {
                playlists = _casheService.Get<IList<Playlist>>(PlaylistKey);

                if (playlists == null) return await GetUserPlaylists(true);
            }

            return playlists;
        }

        public async Task<bool> CreatePlaylist(string title, List<int> songIds)
        {
            var createModel = new
            {
                title = title,
                songIds = songIds
            };

            var response = await _client.PostAsJsonAsync($"playlists/add", createModel);

            if (!response.IsSuccessStatusCode) return false;

            var playlist = await response.Content.ReadAsJsonAsync<Playlist>();

            PutPlaylistToCashe(playlist);

            return true;
        }

        public async Task<bool> EditPlaylist(string title, List<int> songIds, int playlistId)
        {
            var editModel = new
            {
                title = title,
                songIds = songIds
            };

            var response = await _client.PutAsJsonAsync($"playlists/edit/{playlistId}", editModel);

            if (!response.IsSuccessStatusCode) return false;

            var playlist = await response.Content.ReadAsJsonAsync<Playlist>();

            PutPlaylistToCashe(playlist);

            return true;
        }

        public async Task<bool> DeletePlaylist(Playlist playlist)
        {
            var response = await _client.DeleteAsync($"playlists/delete/{playlist.Id}");

            if (!response.IsSuccessStatusCode) return false;

            DeletePlaylistFromCashe(playlist);

            return true;
        }

        public void DeleteSongFromPlaylistCashe(Song song)
        {
            var cashedPlaylists = _casheService.Get<IList<Playlist>>(PlaylistKey);

            if (cashedPlaylists == null) return;

            foreach(var playlist in cashedPlaylists)
            {
                var songs = playlist.Songs.ToList();

                var cashedSong = songs.Where(s => s.Id == song.Id)
                                      .FirstOrDefault();

                if (cashedSong != null)
                {
                    songs.Remove(cashedSong);
                    playlist.Songs = songs;
                }
            }

            _casheService.Put(PlaylistKey, cashedPlaylists);
        }

        public void UpdateSongInPlaylistCashe(Song song)
        {
            var cashedPlaylists = _casheService.Get<IList<Playlist>>(PlaylistKey);

            if (cashedPlaylists == null) return;

            foreach (var playlist in cashedPlaylists)
            {
                var songs = playlist.Songs.ToList();

                var cashedSong = songs.Where(s => s.Id == song.Id)
                                      .FirstOrDefault();

                if (cashedSong != null)
                {
                    cashedSong.Title = song.Title;
                    cashedSong.Performer = song.Performer;

                    playlist.Songs = songs;
                }
            }

            _casheService.Put(PlaylistKey, cashedPlaylists);
        }

        // Methods for working with cache

        private void PutPlaylistToCashe(Playlist playlist)
        {
            var cashedPlaylists = _casheService.Get<IList<Playlist>>(PlaylistKey);

            if (cashedPlaylists == null) return;

            var tempPlaylist = cashedPlaylists.Where(p => p.Id == playlist.Id)
                                              .FirstOrDefault();

            if (tempPlaylist != null)
            {
                tempPlaylist.Title = playlist.Title;
                tempPlaylist.Songs = playlist.Songs;
            }
            else
            {
                cashedPlaylists.Add(playlist);
            }

            _casheService.Put(PlaylistKey, cashedPlaylists);
        }

        private void DeletePlaylistFromCashe(Playlist playlist)
        {
            var cashedPlaylists = _casheService.Get<IList<Playlist>>(PlaylistKey);

            var cashedPlaylist = cashedPlaylists.Where(p => p.Id == playlist.Id)
                                                .FirstOrDefault();

            if (cashedPlaylist != null)
                cashedPlaylists.Remove(cashedPlaylist);

            _casheService.Put(PlaylistKey, cashedPlaylists);
        }
    }
}
