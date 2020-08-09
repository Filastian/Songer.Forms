using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class Playlist : Entity
    {
        [Required]
        public string Title { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<PlaylistSong> PlaylistSongs { get; set; }

        public Playlist()
        {
            PlaylistSongs = new List<PlaylistSong>();
        }
    }
}
