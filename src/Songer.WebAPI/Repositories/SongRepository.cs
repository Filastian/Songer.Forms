using Microsoft.EntityFrameworkCore;
using Songer.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Songer.WebAPI.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly TestAuthContext _context;

        public SongRepository(TestAuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<Song> GetSongAsync(int songId)
        {
            return await FindSong(songId);
        }

        public async Task<Song> AddAsync(CreateSongModel model)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", model.File.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var song = new Song()
            {
                Title = model.Title,
                Performer = model.Performer,
                Path = path
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return song;
        }

        public async Task<Song> UpdateAsync(UpdateSongModel model, int songId)
        {
            var song = await FindSong(songId);

            if (!string.IsNullOrWhiteSpace(model.Title))
                song.Title = model.Title;

            _context.Songs.Update(song);
            await _context.SaveChangesAsync();

            return song;
        }

        public async Task DeleteSongAsync(int songId)
        {
            var song = await FindSong(songId);

            _context.Songs.Remove(song);

            await _context.SaveChangesAsync();
        }

        //  private methods

        private async Task<Song> FindSong(int songId)
        {
            var song = await _context.Songs.FindAsync(songId);

            if (song == null)
                throw new ArgumentException("Such song does not exist");

            return song;
        }
    }
}
