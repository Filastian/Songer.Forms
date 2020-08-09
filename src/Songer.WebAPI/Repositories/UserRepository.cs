using Microsoft.EntityFrameworkCore;
using Songer.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TestAuthContext _context;

        public UserRepository(TestAuthContext context)
        {
            _context = context;

            if(!_context.Users.Any())
            {
                _context.Users.Add(new User { Login = "administrator", Password = "123456", Name = "admin name:)", Role = Role.Admin });
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await FindUser(id);
        }

        public async Task AddAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Login == user.Login))
                throw new ArgumentException($"Login {user.Login} is already there");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateUserModel model, int userId)
        {
            var user = await FindUser(userId);

            if (!string.IsNullOrWhiteSpace(model.Name))
                user.Name = model.Name;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.Password = model.Password;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await FindUser(id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        //  Songs managements

        public async Task<IEnumerable<Song>> GetAllUserSongsAsync(int userId)
        {
            var user = await _context.Users.Where(c => c.Id == userId)
                                           .Include(v => v.UserSongs)
                                           .ThenInclude(b => b.Song)
                                           .FirstOrDefaultAsync();

            return user.UserSongs.Select(s => s.Song).ToList();
        }

        public async Task AddSongToUserAsync(int userId, int songId)
        {
            var user = await _context.Users.Where(c => c.Id == userId)
                                     .Include(v => v.UserSongs)
                                     .ThenInclude(b => b.Song)
                                     .FirstOrDefaultAsync();

            bool exist = user.UserSongs.Any(c => c.SongId == songId);

            if (exist)
                throw new ArgumentException("User already has this song");

            user.UserSongs.Add(new UserSong { UserId = userId, SongId = songId });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserSongAsync(int userId, int songId)
        {
            var song = await _context.Songs.Where(c => c.Id == songId)
                                           .Include(v => v.UserSongs)
                                           .FirstOrDefaultAsync();

            bool exist = song.UserSongs.Any(c => c.UserId == userId);

            if (!exist)
                throw new ArgumentException("User does not have this song");

            var userSongType = song.UserSongs
                                   .Where(at => at.SongId == songId && at.UserId == userId)
                                   .FirstOrDefault();

            song.UserSongs.Remove(userSongType);

            await _context.SaveChangesAsync();
        }

        // Playlists managements

        public async Task<IEnumerable<Playlist>> GetAllUserPlaylistsAsync(int userId)
        {
            return await _context.Playlists.Where(s => s.UserId == userId)
                                           .Include(u => u.PlaylistSongs)
                                           .ThenInclude(w => w.Song)
                                           .ToListAsync();
        }

        //  private methods

        private async Task<User> FindUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new ArgumentException("Such user does not exist");

            return user;
        }
    }
}
