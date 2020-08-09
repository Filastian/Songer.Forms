using Songer.WebAPI.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Songer.WebAPI.Models
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<SongDto> Songs { get; set; }

        public PlaylistDto(Playlist playlist)
        {
            Id = playlist.Id;
            Title = playlist.Title;
            Songs = playlist.PlaylistSongs.Select(s => s.Song)
                                          .ToSongDtosList();
        }
    }
}
