using Microsoft.EntityFrameworkCore;
using Songer.WebAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly TestAuthContext _context;

        public PlaylistRepository(TestAuthContext context)
        {
            _context = context;
        }

        public async Task<Playlist> GetPlaylistAsync(int playlistId)
        {
            return await _context.Playlists.Where(s => s.Id == playlistId)
                                           .Include(u => u.PlaylistSongs)
                                           .ThenInclude(w => w.Song)
                                           .FirstOrDefaultAsync();
        }

        public async Task<Playlist> AddAsync(CreatePlaylistModel model, int userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
                                           .FirstOrDefaultAsync();

            var playlist = new Playlist() { Title = model.Title, User = user };
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            foreach (var id in model.SongIds)
            {
                playlist.PlaylistSongs.Add(new PlaylistSong { PlaylistId = playlist.Id, SongId = id });
            }

            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();

            return await GetPlaylistAsync(playlist.Id);
        }

        public async Task<Playlist> UpdateAsync(EditPlaylistModel model, int playlistId, int userId)
        {
            var playlist = await _context.Playlists.Where(c => c.Id == playlistId)
                                                   .Include(p => p.PlaylistSongs)
                                                   .ThenInclude(ps => ps.Song)
                                                   .FirstOrDefaultAsync();

            if (playlist.UserId != userId)
                throw new ArgumentException("No access to playlist");

            if (!string.IsNullOrWhiteSpace(model.Title))
                playlist.Title = model.Title;

            var songIds = playlist.PlaylistSongs.Select(s => s.SongId)
                                                .ToList();

            foreach (var id in songIds)
            {
                if (!model.SongIds.Contains(id))
                {
                    var ps = playlist.PlaylistSongs.Where(r => r.PlaylistId == playlist.Id && r.SongId == id)
                                                   .FirstOrDefault();

                    playlist.PlaylistSongs.Remove(ps);
                }
            }

            foreach (var id in model.SongIds)
            {
                if (!songIds.Contains(id))
                {
                    playlist.PlaylistSongs.Add(new PlaylistSong { PlaylistId = playlist.Id, SongId = id });
                }
            }

            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();

            return await GetPlaylistAsync(playlist.Id);
        }

        public async Task DeleteAsync(int playlistId)
        {
            var playlist = await FindPlaylist(playlistId);

            _context.Playlists.Remove(playlist);

            await _context.SaveChangesAsync();
        }

        //  private methods

        private async Task<Playlist> FindPlaylist(int playlistId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);

            if (playlist == null)
                throw new ArgumentException("Such playlist does not exist");

            return playlist;
        }
    }
}
