using Songer.Core.Client;
using Songer.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class SongService : ISongService
    {
        public SongService(
            IRestClient client,
            ICasheService casheService)
        {
            _client = client;
            _casheService = casheService;
        }

        private readonly IRestClient _client;
        private readonly ICasheService _casheService;

        private const string MySongsListKey = nameof(MySongsListKey);
        private const string AllSongsListKey = nameof(AllSongsListKey);

        public async Task<IEnumerable<Song>> GetAllSongs(bool updateToken = false)
        {
            var songs = default(IEnumerable<Song>);

            if(updateToken)
            {
                var response = await _client.GetAsync("songs");

                if (!response.IsSuccessStatusCode) return null;

                songs = await response.Content.ReadAsJsonAsync<IEnumerable<Song>>();

                _casheService.Put(AllSongsListKey, songs);
            }
            else
            {
                songs = _casheService.Get<IList<Song>>(AllSongsListKey);

                if (songs == null) return await GetAllSongs(true);
            }

            return songs;
        }

        public async Task<IEnumerable<Song>> GetUserSongs(bool updateToken = false)
        {
            var songs = default(IEnumerable<Song>);

            if (updateToken)
            {
                var response = await _client.GetAsync("songs/my");

                if (!response.IsSuccessStatusCode) return null;

                songs = await response.Content.ReadAsJsonAsync<IEnumerable<Song>>();

                _casheService.Put(MySongsListKey, songs);
            }
            else
            {
                songs = _casheService.Get<IList<Song>>(MySongsListKey);

                if (songs == null) return await GetUserSongs(true);
            }

            return songs;
        }

        public async Task<Stream> GetSong(int songId)
        {
            var response = await _client.GetAsync($"/songs/{songId}");

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<bool> AddSong(string title, string performer, byte[] data)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(title), "Title");
            content.Add(new StringContent(performer), "Performer");

            var byteArrayContent = new ByteArrayContent(data);
            content.Add(byteArrayContent, "File", $"{title} - {performer}.mp3");

            var response = await _client.PostAsync("songs/add", content);

            if (!response.IsSuccessStatusCode) return false;

            var song = await response.Content.ReadAsJsonAsync<Song>();

            //var song = new Song { Title = title, Performer = performer, Id = 99 };

            PutSongToAllSongsCashe(song);
            PutSongToMySongsCashe(song);

            return true;
        }

        public async Task<bool> UpdateSong(Song song)
        {
            var updateModel = new
            {
                title = song.Title,
                performer = song.Performer
            };

            var response = await _client.PutAsJsonAsync($"songs/edit/{song.Id}", updateModel);

            if (!response.IsSuccessStatusCode) return false;

            var updatedSong = await response.Content.ReadAsJsonAsync<Song>();

            PutSongToAllSongsCashe(updatedSong);
            PutSongToMySongsCashe(updatedSong);

            return true;
        }

        public async Task<bool> DeleteSong(Song song)
        {
            var response = await _client.DeleteAsync($"songs/delete/{song.Id}");

            if (!response.IsSuccessStatusCode) return false;

            DeleteSongFromAllSongsCashe(song);
            DeleteSongFromMySongsCashe(song);

            return true;
        }

        public async Task AddToUserSongs(Song song)
        {
            var response = await _client.PostAsync($"songs/my/add/{song.Id}", null);

            if(response.IsSuccessStatusCode)
                PutSongToMySongsCashe(song);
        }

        public async Task DeleteFromUserSongs(Song song)
        {
            var response = await _client.DeleteAsync($"songs/my/delete/{song.Id}");
            
            if(response.IsSuccessStatusCode)
                DeleteSongFromMySongsCashe(song);
        }

        // Methods for working with cache

        private void PutSongToAllSongsCashe(Song song)
        {
            var cashedSongs = _casheService.Get<IList<Song>>(AllSongsListKey);

            if (cashedSongs == null) return;

            var tempSong = cashedSongs.Where(s => s.Id == song.Id)
                                      .FirstOrDefault();

            if (tempSong != null)
            {
                tempSong.Title = song.Title;
                tempSong.Performer = song.Performer;
            }
            else
            {
                cashedSongs.Add(song);
            }

            _casheService.Put(AllSongsListKey, cashedSongs);
        }

        private void PutSongToMySongsCashe(Song song)
        {
            var cashedSongs = _casheService.Get<IList<Song>>(MySongsListKey);

            if (cashedSongs == null) return;

            var tempSong = cashedSongs.Where(s => s.Id == song.Id)
                                      .FirstOrDefault();

            if (tempSong != null)
            {
                tempSong.Title = song.Title;
                tempSong.Performer = song.Performer;
            }
            else
            {
                cashedSongs.Add(song);
            }

            _casheService.Put(MySongsListKey, cashedSongs);
        }

        private void DeleteSongFromAllSongsCashe(Song song)
        {
            var cashedSongs = _casheService.Get<IList<Song>>(AllSongsListKey);

            if (cashedSongs == null) return;

            var cashedSong = cashedSongs.Where(s => s.Id == song.Id)
                                        .FirstOrDefault();
            if(cashedSong != null)
                cashedSongs.Remove(cashedSong);

            _casheService.Put(AllSongsListKey, cashedSongs);
        }

        private void DeleteSongFromMySongsCashe(Song song)
        {
            var cashedSongs = _casheService.Get<IList<Song>>(MySongsListKey);

            if (cashedSongs == null) return;

            var cashedSong = cashedSongs.Where(s => s.Id == song.Id)
                                        .FirstOrDefault();

            if (cashedSong != null)
                cashedSongs.Remove(cashedSong);

            _casheService.Put(MySongsListKey, cashedSongs);
        }
    }
}
