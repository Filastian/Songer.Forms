using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class Song : Entity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Performer { get; set; }

        [Required]
        public string Path { get; set; }

        public List<UserSong> UserSongs { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; }

        public Song()
        {
            UserSongs = new List<UserSong>();
            PlaylistSongs = new List<PlaylistSong>();
        }
    }
}